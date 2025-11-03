using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class ChuKyDaoTaoDao
{
    private static ChuKyDaoTaoDao _instance;

    private ChuKyDaoTaoDao()
    {
    }

    public static ChuKyDaoTaoDao GetInstance()
    {
        if (_instance == null) _instance = new ChuKyDaoTaoDao();
        return _instance;
    }

    // Lấy tất cả các chu kỳ đào tạo còn hoạt động (Status = 1)
    public List<ChuKyDaoTaoDto> GetAll()
    {
        List<ChuKyDaoTaoDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaCKDT, NamBatDau, NamKetThuc FROM chukydaotao WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new ChuKyDaoTaoDto
            {
                MaCKDT = reader.GetInt32("MaCKDT"),
                NamBatDau = reader.GetString("NamBatDau"),
                NamKetThuc = reader.GetString("NamKetThuc")
            });

        return result;
    }

    // Thêm mới chu kỳ đào tạo
    public bool Insert(ChuKyDaoTaoDto dto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO chukydaotao (NamBatDau, NamKetThuc)
                             VALUES (@NamBatDau, @NamKetThuc)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NamBatDau", dto.NamBatDau);
            cmd.Parameters.AddWithValue("@NamKetThuc", dto.NamKetThuc);

            rowAffected = cmd.ExecuteNonQuery();
        }

        return rowAffected > 0;
    }

    // Cập nhật chu kỳ đào tạo
    public bool Update(ChuKyDaoTaoDto dto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE chukydaotao
                             SET NamBatDau = @NamBatDau,
                                 NamKetThuc = @NamKetThuc
                             WHERE MaCKDT = @MaCKDT";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaCKDT", dto.MaCKDT);
            cmd.Parameters.AddWithValue("@NamBatDau", dto.NamBatDau);
            cmd.Parameters.AddWithValue("@NamKetThuc", dto.NamKetThuc);

            rowAffected = cmd.ExecuteNonQuery();
        }

        return rowAffected > 0;
    }

    // Xóa mềm (set Status = 0)
    public bool Delete(int maCKDT)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE chukydaotao
                             SET Status = 0
                             WHERE MaCKDT = @MaCKDT";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaCKDT", maCKDT);

            rowAffected = cmd.ExecuteNonQuery();
        }

        return rowAffected > 0;
    }

    // Lấy chu kỳ đào tạo theo ID
    public ChuKyDaoTaoDto GetById(int maCKDT)
    {
        ChuKyDaoTaoDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaCKDT, NamBatDau, NamKetThuc FROM chukydaotao WHERE Status = 1 AND MaCKDT = @MaCKDT",
            conn);
        cmd.Parameters.AddWithValue("@MaCKDT", maCKDT);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new ChuKyDaoTaoDto
            {
                MaCKDT = reader.GetInt32("MaCKDT"),
                NamBatDau = reader.GetString("NamBatDau"),
                NamKetThuc = reader.GetString("NamKetThuc")
            };

        return result;
    }

    public ChuKyDaoTaoDto? GetByStartYear(int year)
    {
        ChuKyDaoTaoDto? result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaCKDT, NamBatDau, NamKetThuc FROM chukydaotao WHERE Status = 1 AND NamBatDau < @year AND NamKetThuc > @year",
            conn);
        cmd.Parameters.AddWithValue("@year", year);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new ChuKyDaoTaoDto
            {
                MaCKDT = reader.GetInt32("MaCKDT"),
                NamBatDau = reader.GetString("NamBatDau"),
                NamKetThuc = reader.GetString("NamKetThuc")
            };

        return result;
    }

    public ChuKyDaoTaoDto GetByStartYearEndYear(int startYear, int endYear)
    {
        var result = new ChuKyDaoTaoDto();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaCKDT, NamBatDau, NamKetThuc FROM chukydaotao WHERE Status = 1 AND NamBatDau = @StartYear AND NamKetThuc = @EndYear",
            conn);
        cmd.Parameters.AddWithValue("@StartYear", startYear);
        cmd.Parameters.AddWithValue("@EndYear", endYear);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new ChuKyDaoTaoDto
            {
                MaCKDT = reader.GetInt32("MaCKDT"),
                NamBatDau = reader.GetString("NamBatDau"),
                NamKetThuc = reader.GetString("NamKetThuc")
            };

        return result;
    }
}