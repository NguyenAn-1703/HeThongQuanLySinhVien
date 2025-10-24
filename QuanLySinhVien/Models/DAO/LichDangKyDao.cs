using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System;
using System.Collections.Generic;


public class LichDangKyDao
{
    private static LichDangKyDao _instance;
    private LichDangKyDao() { }

    public static LichDangKyDao GetInstance()
    {
        if (_instance == null)
            _instance = new LichDangKyDao();
        return _instance;
    }

    public List<LichDangKyDto> GetAll()
    {
        List<LichDangKyDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaLichDK, ThoiGianBatDau, ThoiGianKetThuc FROM LichDangKy WHERE Status = 1",
            conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new LichDangKyDto
            {
                MaLichDK = reader.GetInt32("MaLichDK"),
                ThoiGianBatDau = reader.GetDateTime("ThoiGianBatDau"),
                ThoiGianKetThuc = reader.GetDateTime("ThoiGianKetThuc")
            });
        }

        return result;
    }

    public bool Insert(LichDangKyDto lichDto)
    {
        int rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO LichDangKy (ThoiGianBatDau, ThoiGianKetThuc)
                             VALUES (@ThoiGianBatDau, @ThoiGianKetThuc)";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ThoiGianBatDau", lichDto.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("@ThoiGianKetThuc", lichDto.ThoiGianKetThuc);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(LichDangKyDto lichDto)
    {
        int rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE LichDangKy 
                             SET ThoiGianBatDau = @ThoiGianBatDau,
                                 ThoiGianKetThuc = @ThoiGianKetThuc
                             WHERE MaLichDK = @MaLichDK";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLichDK", lichDto.MaLichDK);
                cmd.Parameters.AddWithValue("@ThoiGianBatDau", lichDto.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("@ThoiGianKetThuc", lichDto.ThoiGianKetThuc);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maLichDK)
    {
        int rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE LichDangKy
                             SET Status = 0
                             WHERE MaLichDK = @MaLichDK";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLichDK", maLichDK);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public LichDangKyDto GetById(int maLichDK)
    {
        LichDangKyDto? result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaLichDK, ThoiGianBatDau, ThoiGianKetThuc FROM LichDangKy WHERE Status = 1 AND MaLichDK = @MaLichDK",
            conn);
        cmd.Parameters.AddWithValue("@MaLichDK", maLichDK);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new LichDangKyDto
            {
                MaLichDK = reader.GetInt32("MaLichDK"),
                ThoiGianBatDau = reader.GetDateTime("ThoiGianBatDau"),
                ThoiGianKetThuc = reader.GetDateTime("ThoiGianKetThuc")
            };
        }

        return result;
    }
}

