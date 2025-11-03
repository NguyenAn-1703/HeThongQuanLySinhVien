using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class CaThi_SinhVienDao
{
    private static CaThi_SinhVienDao _instance;

    private CaThi_SinhVienDao()
    {
    }

    public static CaThi_SinhVienDao GetInstance()
    {
        if (_instance == null)
            _instance = new CaThi_SinhVienDao();
        return _instance;
    }

    public List<CaThi_SinhVienDto> GetAll()
    {
        List<CaThi_SinhVienDto> result = new();
        using var conn = MyConnection.GetConnection();
        var query = "SELECT MaCT, MaSV FROM CaThi_SinhVien WHERE Status = 1";
        using var cmd = new MySqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new CaThi_SinhVienDto
            {
                MaCT = reader.GetInt32("MaCT"),
                MaSV = reader.GetInt32("MaSV")
            });

        return result;
    }

    public bool Insert(CaThi_SinhVienDto caThiSv)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        var query = @"INSERT INTO CaThi_SinhVien (MaCT, MaSV)
                         VALUES (@MaCT, @MaSV)";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCT", caThiSv.MaCT);
        cmd.Parameters.AddWithValue("@MaSV", caThiSv.MaSV);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public bool Delete(int maCT, int maSV)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        var query = @"UPDATE CaThi_SinhVien
                         SET Status = 0
                         WHERE MaCT = @MaCT AND MaSV = @MaSV";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCT", maCT);
        cmd.Parameters.AddWithValue("@MaSV", maSV);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public CaThi_SinhVienDto GetById(int maCT, int maSV)
    {
        CaThi_SinhVienDto result = null;
        using var conn = MyConnection.GetConnection();
        var query = @"SELECT MaCT, MaSV 
                         FROM CaThi_SinhVien 
                         WHERE Status = 1 AND MaCT = @MaCT AND MaSV = @MaSV";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCT", maCT);
        cmd.Parameters.AddWithValue("@MaSV", maSV);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new CaThi_SinhVienDto
            {
                MaCT = reader.GetInt32("MaCT"),
                MaSV = reader.GetInt32("MaSV")
            };

        return result;
    }

    public List<CaThi_SinhVienDto> GetByMaCT(int maCT)
    {
        List<CaThi_SinhVienDto> result = new();
        using var conn = MyConnection.GetConnection();
        var query = @"SELECT MaCT, MaSV 
                         FROM CaThi_SinhVien 
                         WHERE Status = 1 AND MaCT = @MaCT";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCT", maCT);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new CaThi_SinhVienDto
            {
                MaCT = reader.GetInt32("MaCT"),
                MaSV = reader.GetInt32("MaSV")
            });

        return result;
    }

    public bool HardDeleteByMaCT(int maCT)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        var query = @"DELETE FROM CaThi_SinhVien
                     WHERE MaCT = @MaCT";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCT", maCT);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }
}