using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class DotDangKyDao
{
    private static DotDangKyDao _instance;

    private DotDangKyDao() { }

    public static DotDangKyDao GetInstance()
    {
        return _instance ??= new DotDangKyDao();
    }

    // ============================
    //           GET ALL
    // ============================
    public List<DotDangKyDto> GetAll()
    {
        List<DotDangKyDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaDotDK, HocKy, Nam, ThoiGianBatDau, ThoiGianKetThuc
              FROM DotDangKy
              WHERE Status = 1", conn);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new DotDangKyDto
            {
                MaDotDK = reader.GetInt32("MaDotDK"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                ThoiGianBatDau = reader.GetDateTime("ThoiGianBatDau"),
                ThoiGianKetThuc = reader.GetDateTime("ThoiGianKetThuc")
            });
        }

        return result;
    }

    // ============================
    //           GET BY ID
    // ============================
    public DotDangKyDto? GetById(int maDotDK)
    {
        DotDangKyDto? result = null;
        using var conn = MyConnection.GetConnection();

        using var cmd = new MySqlCommand(
            @"SELECT MaDotDK, HocKy, Nam, ThoiGianBatDau, ThoiGianKetThuc
              FROM DotDangKy
              WHERE Status = 1 AND MaDotDK = @MaDotDK", conn);

        cmd.Parameters.AddWithValue("@MaDotDK", maDotDK);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            result = new DotDangKyDto
            {
                MaDotDK = reader.GetInt32("MaDotDK"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                ThoiGianBatDau = reader.GetDateTime("ThoiGianBatDau"),
                ThoiGianKetThuc = reader.GetDateTime("ThoiGianKetThuc")
            };
        }

        return result;
    }

    // ============================
    //            INSERT
    // ============================
    public bool Insert(DotDangKyDto dto)
    {
        using var conn = MyConnection.GetConnection();

        var query = @"INSERT INTO DotDangKy 
                        (HocKy, Nam, ThoiGianBatDau, ThoiGianKetThuc, Status)
                      VALUES 
                        (@HocKy, @Nam, @ThoiGianBatDau, @ThoiGianKetThuc, 1)";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@HocKy", dto.HocKy);
        cmd.Parameters.AddWithValue("@Nam", dto.Nam);
        cmd.Parameters.AddWithValue("@ThoiGianBatDau", dto.ThoiGianBatDau);
        cmd.Parameters.AddWithValue("@ThoiGianKetThuc", dto.ThoiGianKetThuc);

        return cmd.ExecuteNonQuery() > 0;
    }

    // ============================
    //            UPDATE
    // ============================
    public bool Update(DotDangKyDto dto)
    {
        using var conn = MyConnection.GetConnection();

        var query = @"UPDATE DotDangKy 
                      SET HocKy = @HocKy,
                          Nam = @Nam,
                          ThoiGianBatDau = @ThoiGianBatDau,
                          ThoiGianKetThuc = @ThoiGianKetThuc
                      WHERE MaDotDK = @MaDotDK";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaDotDK", dto.MaDotDK);
        cmd.Parameters.AddWithValue("@HocKy", dto.HocKy);
        cmd.Parameters.AddWithValue("@Nam", dto.Nam);
        cmd.Parameters.AddWithValue("@ThoiGianBatDau", dto.ThoiGianBatDau);
        cmd.Parameters.AddWithValue("@ThoiGianKetThuc", dto.ThoiGianKetThuc);

        return cmd.ExecuteNonQuery() > 0;
    }

    // ============================
    //             DELETE (Soft Delete)
    // ============================
    public bool Delete(int maDotDK)
    {
        using var conn = MyConnection.GetConnection();

        var query = @"UPDATE DotDangKy
                      SET Status = 0
                      WHERE MaDotDK = @MaDotDK";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaDotDK", maDotDK);

        return cmd.ExecuteNonQuery() > 0;
    }
}
