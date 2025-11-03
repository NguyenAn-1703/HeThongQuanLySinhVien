using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class DangKyDao
{
    private static DangKyDao _instance;

    private DangKyDao()
    {
    }

    public static DangKyDao GetInstance()
    {
        if (_instance == null) _instance = new DangKyDao();
        return _instance;
    }

    public List<DangKyDto> GetAll()
    {
        List<DangKyDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaNHP, MaSV FROM DangKy WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new DangKyDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaSV = reader.GetInt32("MaSV")
            });

        return result;
    }

    public bool Insert(DangKyDto dangKyDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO DangKy (MaNHP, MaSV, Status)
                             VALUES (@MaNHP, @MaSV, 1)";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNHP", dangKyDto.MaNHP);
                cmd.Parameters.AddWithValue("@MaSV", dangKyDto.MaSV);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maNHP, int maSV)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE DangKy
                             SET Status = 0
                             WHERE MaNHP = @MaNHP AND MaSV = @MaSV";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNHP", maNHP);
                cmd.Parameters.AddWithValue("@MaSV", maSV);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool HardDelete(int maNHP, int maSV)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"DELETE FROM DangKy
                         WHERE MaNHP = @MaNHP AND MaSV = @MaSV";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNHP", maNHP);
                cmd.Parameters.AddWithValue("@MaSV", maSV);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }


    public List<DangKyDto> GetByMaNHP(int maNHP)
    {
        List<DangKyDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaNHP, MaSV FROM DangKy WHERE Status = 1 AND MaNHP = @MaNHP", conn);
        cmd.Parameters.AddWithValue("@MaNHP", maNHP);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new DangKyDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaSV = reader.GetInt32("MaSV")
            });

        return result;
    }

    public List<DangKyDto> GetByMaSV(int maSV)
    {
        List<DangKyDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaNHP, MaSV FROM DangKy WHERE Status = 1 AND MaSV = @MaSV", conn);
        cmd.Parameters.AddWithValue("@MaSV", maSV);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new DangKyDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaSV = reader.GetInt32("MaSV")
            });

        return result;
    }
}