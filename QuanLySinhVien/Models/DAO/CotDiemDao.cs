using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System.Collections.Generic;

public class CotDiemDao
{
    private static CotDiemDao _instance;
    private CotDiemDao() { }

    public static CotDiemDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new CotDiemDao();
        }
        return _instance;
    }

    public List<CotDiemDto> GetAll()
    {
        List<CotDiemDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaCD, MaDQT, TenCotDiem, DiemSo, HeSo
              FROM CotDiem
              WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new CotDiemDto
            {
                MaCD = reader.GetInt32("MaCD"),
                MaDQT = reader.GetInt32("MaDQT"),
                TenCotDiem = reader.GetString("TenCotDiem"),
                DiemSo = reader.GetFloat("DiemSo"),
                HeSo = reader.GetFloat("HeSo")
            });
        }

        return result;
    }

    public bool Insert(CotDiemDto cotDiemDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO CotDiem (MaDQT, TenCotDiem, DiemSo, HeSo)
                             VALUES (@MaDQT, @TenCotDiem, @DiemSo, @HeSo)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDQT", cotDiemDto.MaDQT);
                cmd.Parameters.AddWithValue("@TenCotDiem", cotDiemDto.TenCotDiem);
                cmd.Parameters.AddWithValue("@DiemSo", cotDiemDto.DiemSo);
                cmd.Parameters.AddWithValue("@HeSo", cotDiemDto.HeSo);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(CotDiemDto cotDiemDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE CotDiem
                             SET MaDQT = @MaDQT,
                                 TenCotDiem = @TenCotDiem,
                                 DiemSo = @DiemSo,
                                 HeSo = @HeSo
                             WHERE MaCD = @MaCD";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaCD", cotDiemDto.MaCD);
                cmd.Parameters.AddWithValue("@MaDQT", cotDiemDto.MaDQT);
                cmd.Parameters.AddWithValue("@TenCotDiem", cotDiemDto.TenCotDiem);
                cmd.Parameters.AddWithValue("@DiemSo", cotDiemDto.DiemSo);
                cmd.Parameters.AddWithValue("@HeSo", cotDiemDto.HeSo);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maCD)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE CotDiem
                             SET Status = 0
                             WHERE MaCD = @MaCD";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaCD", maCD);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public CotDiemDto GetCotDiemById(int maCD)
    {
        CotDiemDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaCD, MaDQT, TenCotDiem, DiemSo, HeSo
              FROM CotDiem
              WHERE Status = 1 AND MaCD = @MaCD", conn);
        cmd.Parameters.AddWithValue("@MaCD", maCD);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            result = new CotDiemDto
            {
                MaCD = reader.GetInt32("MaCD"),
                MaDQT = reader.GetInt32("MaDQT"),
                TenCotDiem = reader.GetString("TenCotDiem"),
                DiemSo = reader.GetFloat("DiemSo"),
                HeSo = reader.GetFloat("HeSo")
            };
        }

        return result;
    }
    public bool HardDelete(int maCD)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"DELETE FROM CotDiem
                         WHERE MaCD = @MaCD";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaCD", maCD);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

}
