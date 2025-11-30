using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Model.DAO;

public class LichSuDungDao
{
    private static LichSuDungDao _instance;

    private LichSuDungDao() { }

    public static LichSuDungDao GetInstance()
    {
        if (_instance == null) _instance = new LichSuDungDao();
        return _instance;
    }

    public List<LichSuDungDto> GetAll()
    {
        List<LichSuDungDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaLSD, MaPH, ThoiGianBatDau, ThoiGianKetThuc, GhiChu
              FROM lichsudung
              WHERE Status = 1", conn);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new LichSuDungDto
            {
                MaLSD = reader.GetInt32("MaLSD"),
                MaPH = reader.GetInt32("MaPH"),
                ThoiGianBatDau = reader.GetDateTime("ThoiGianBatDau"),
                ThoiGianKetThuc = reader.GetDateTime("ThoiGianKetThuc"),
                GhiChu = reader["GhiChu"]?.ToString()
            });
        }

        return result;
    }

    public bool Insert(LichSuDungDto dto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO lichsudung 
                          (MaPH, ThoiGianBatDau, ThoiGianKetThuc, GhiChu)
                          VALUES (@MaPH, @ThoiGianBatDau, @ThoiGianKetThuc, @GhiChu)";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaPH", dto.MaPH);
                cmd.Parameters.AddWithValue("@ThoiGianBatDau", dto.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("@ThoiGianKetThuc", dto.ThoiGianKetThuc);
                cmd.Parameters.AddWithValue("@GhiChu", dto.GhiChu ?? (object)DBNull.Value);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(LichSuDungDto dto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE lichsudung
                          SET MaPH = @MaPH,
                              ThoiGianBatDau = @ThoiGianBatDau,
                              ThoiGianKetThuc = @ThoiGianKetThuc,
                              GhiChu = @GhiChu
                          WHERE MaLSD = @MaLSD";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLSD", dto.MaLSD);
                cmd.Parameters.AddWithValue("@MaPH", dto.MaPH);
                cmd.Parameters.AddWithValue("@ThoiGianBatDau", dto.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("@ThoiGianKetThuc", dto.ThoiGianKetThuc);
                cmd.Parameters.AddWithValue("@GhiChu", dto.GhiChu ?? (object)DBNull.Value);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maLSD)
    {
        int rowAffected = 0;

        using (var conn = MyConnection.GetConnection())
        {
            var query = @"DELETE FROM lichsudung
                      WHERE MaLSD = @MaLSD";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLSD", maLSD);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }


    public LichSuDungDto GetById(int maLSD)
    {
        LichSuDungDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaLSD, MaPH, ThoiGianBatDau, ThoiGianKetThuc, GhiChu
              FROM lichsudung
              WHERE Status = 1 AND MaLSD = @MaLSD", conn);

        cmd.Parameters.AddWithValue("@MaLSD", maLSD);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new LichSuDungDto
            {
                MaLSD = reader.GetInt32("MaLSD"),
                MaPH = reader.GetInt32("MaPH"),
                ThoiGianBatDau = reader.GetDateTime("ThoiGianBatDau"),
                ThoiGianKetThuc = reader.GetDateTime("ThoiGianKetThuc"),
                GhiChu = reader["GhiChu"]?.ToString()
            };
        }

        return result;
    }
    
    public int GetAutoIncrement()
    {
        using var conn = MyConnection.GetConnection();

        var query = @"
        SELECT AUTO_INCREMENT
        FROM information_schema.TABLES
        WHERE TABLE_SCHEMA = @DbName
          AND TABLE_NAME = 'lichsudung';
    ";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@DbName", conn.Database);

        var result = cmd.ExecuteScalar();

        return Convert.ToInt32(result);
    }

}

