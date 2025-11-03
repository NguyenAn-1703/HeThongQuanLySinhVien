using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class ChucNangDao
{
    private static ChucNangDao _instance;

    private ChucNangDao()
    {
    }

    public static ChucNangDao GetInstance()
    {
        if (_instance == null) _instance = new ChucNangDao();
        return _instance;
    }

    public List<ChucNangDto> GetAll()
    {
        List<ChucNangDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaCN, TenChucNang FROM chucnang WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new ChucNangDto
            {
                MaCN = reader.GetInt32("MaCN"),
                TenCN = reader.GetString("TenChucNang")
            });

        return result;
    }

    public ChucNangDto GetById(int maCN)
    {
        ChucNangDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand("SELECT MaCN, TenChucNang FROM chucnang WHERE Status = 1 AND MaCN = @MaCN",
                conn);
        cmd.Parameters.AddWithValue("@MaCN", maCN);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new ChucNangDto
            {
                MaCN = reader.GetInt32("MaCN"),
                TenCN = reader.GetString("TenChucNang")
            };

        return result;
    }

    public ChucNangDto GetByTen(string tenCN)
    {
        ChucNangDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand("SELECT MaCN, TenChucNang FROM chucnang WHERE Status = 1 AND TenChucNang = @TenChucNang",
                conn);
        cmd.Parameters.AddWithValue("@TenChucNang", tenCN);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new ChucNangDto
            {
                MaCN = reader.GetInt32("MaCN"),
                TenCN = reader.GetString("TenChucNang")
            };

        return result;
    }
}