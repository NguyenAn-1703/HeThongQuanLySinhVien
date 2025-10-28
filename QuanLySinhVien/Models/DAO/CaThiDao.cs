using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System.Collections.Generic;

public class CaThiDao
{
    private static CaThiDao _instance;
    private CaThiDao() { }

    public static CaThiDao GetInstance()
    {
        if (_instance == null)
            _instance = new CaThiDao();
        return _instance;
    }

    public List<CaThiDto> GetAll()
    {
        List<CaThiDto> result = new();
        using var conn = MyConnection.GetConnection();
        string query = "SELECT MaCT, MaHP, MaPH, Thu, ThoiGianBatDau, ThoiLuong FROM CaThi WHERE Status = 1";
        using var cmd = new MySqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new CaThiDto
            {
                MaCT = reader.GetInt32("MaCT"),
                MaHP = reader.GetInt32("MaHP"),
                MaPH = reader.GetInt32("MaPH"),
                Thu = reader.GetString("Thu"),
                ThoiGianBatDau = reader.GetString("ThoiGianBatDau"),
                ThoiLuong = reader.GetString("ThoiLuong")
            });
        }

        return result;
    }

    public bool Insert(CaThiDto caThi)
    {
        int rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        string query = @"INSERT INTO CaThi (MaHP, MaPH, Thu, ThoiGianBatDau, ThoiLuong)
                         VALUES (@MaHP, @MaPH, @Thu, @ThoiGianBatDau, @ThoiLuong)";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaHP", caThi.MaHP);
        cmd.Parameters.AddWithValue("@MaPH", caThi.MaPH);
        cmd.Parameters.AddWithValue("@Thu", caThi.Thu);
        cmd.Parameters.AddWithValue("@ThoiGianBatDau", caThi.ThoiGianBatDau);
        cmd.Parameters.AddWithValue("@ThoiLuong", caThi.ThoiLuong);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public bool Update(CaThiDto caThi)
    {
        int rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        string query = @"UPDATE CaThi 
                         SET MaHP = @MaHP,
                             MaPH = @MaPH,
                             Thu = @Thu,
                             ThoiGianBatDau = @ThoiGianBatDau,
                             ThoiLuong = @ThoiLuong
                         WHERE MaCT = @MaCT";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCT", caThi.MaCT);
        cmd.Parameters.AddWithValue("@MaHP", caThi.MaHP);
        cmd.Parameters.AddWithValue("@MaPH", caThi.MaPH);
        cmd.Parameters.AddWithValue("@Thu", caThi.Thu);
        cmd.Parameters.AddWithValue("@ThoiGianBatDau", caThi.ThoiGianBatDau);
        cmd.Parameters.AddWithValue("@ThoiLuong", caThi.ThoiLuong);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public bool Delete(int maCT)
    {
        int rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        string query = @"UPDATE CaThi 
                         SET Status = 0 
                         WHERE MaCT = @MaCT";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCT", maCT);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public CaThiDto GetById(int maCT)
    {
        CaThiDto result = null;
        using var conn = MyConnection.GetConnection();
        string query = @"SELECT MaCT, MaHP, MaPH, Thu, ThoiGianBatDau, ThoiLuong 
                         FROM CaThi WHERE Status = 1 AND MaCT = @MaCT";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCT", maCT);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new CaThiDto
            {
                MaCT = reader.GetInt32("MaCT"),
                MaHP = reader.GetInt32("MaHP"),
                MaPH = reader.GetInt32("MaPH"),
                Thu = reader.GetString("Thu"),
                ThoiGianBatDau = reader.GetString("ThoiGianBatDau"),
                ThoiLuong = reader.GetString("ThoiLuong")
            };
        }

        return result;
    }
}
