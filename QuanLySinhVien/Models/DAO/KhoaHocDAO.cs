using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

public class KhoaHocDAO
{
    // Lấy tất cả khóa học
    public List<KhoaHocDto> GetAll()
    {
        List<KhoaHocDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaKhoaHoc, MaCKDT, TenKhoaHoc, NienKhoaHoc FROM khoahoc WHERE Status = 1 ORDER BY TenKhoaHoc", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new KhoaHocDto
            {
                MaKhoaHoc = reader.GetInt32("MaKhoaHoc"),
                MaCKDT = reader.GetInt32("MaCKDT"),
                TenKhoaHoc = reader.GetString("TenKhoaHoc"),
                NienKhoaHoc = reader.GetString("NienKhoaHoc")
            });
        }

        return result;
    }

    // Lấy khóa học theo ID
    public KhoaHocDto GetById(int maKhoaHoc)
    {
        KhoaHocDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaKhoaHoc, MaCKDT, TenKhoaHoc, NienKhoaHoc FROM khoahoc WHERE Status = 1 AND MaKhoaHoc = @MaKhoaHoc", conn);
        cmd.Parameters.AddWithValue("@MaKhoaHoc", maKhoaHoc);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new KhoaHocDto
            {
                MaKhoaHoc = reader.GetInt32("MaKhoaHoc"),
                MaCKDT = reader.GetInt32("MaCKDT"),
                TenKhoaHoc = reader.GetString("TenKhoaHoc"),
                NienKhoaHoc = reader.GetString("NienKhoaHoc")
            };
        }

        return result;
    }

    // Lấy khóa học có tên hiển thị (TenKhoaHoc + NienKhoaHoc)
    public List<dynamic> GetAllWithDisplayText()
    {
        List<dynamic> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaKhoaHoc, CONCAT(TenKhoaHoc, ' (', NienKhoaHoc, ')') as DisplayText FROM khoahoc WHERE Status = 1 ORDER BY TenKhoaHoc", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new
            {
                MaKhoaHoc = reader.GetInt32("MaKhoaHoc"),
                DisplayText = reader.GetString("DisplayText")
            });
        }

        return result;
    }
}
