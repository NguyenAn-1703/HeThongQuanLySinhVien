using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System;
using System.Collections.Generic;

public class ChiTietQuyenDao
{
    private static ChiTietQuyenDao _instance;
    private ChiTietQuyenDao() { }

    public static ChiTietQuyenDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ChiTietQuyenDao();
        }
        return _instance;
    }

    public List<ChiTietQuyenDto> GetAll()
    {
        List<ChiTietQuyenDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaCN, MaNQ, HanhDong FROM chitietquyen WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new ChiTietQuyenDto
            {
                MaCN = reader.GetInt32("MaCN"),
                MaNQ = reader.GetInt32("MaNQ"),
                HanhDong = reader.GetString("HanhDong")
            });
        }

        return result;
    }

    public bool Insert(ChiTietQuyenDto chiTietQuyenDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO chitietquyen (MaCN, MaNQ, HanhDong)
                             VALUES (@MaCN, @MaNQ, @HanhDong)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaCN", chiTietQuyenDto.MaCN);
                cmd.Parameters.AddWithValue("@MaNQ", chiTietQuyenDto.MaNQ);
                cmd.Parameters.AddWithValue("@HanhDong", chiTietQuyenDto.HanhDong);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public bool Delete(int maNQ, int maCN, string hanhDong)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE chitietquyen
                             SET Status = 0
                             WHERE MaNQ = @MaNQ
                             AND MaCN = @MaCN
                             AND HanhDong = @HanhDong
                            ;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNQ", maNQ);
                cmd.Parameters.AddWithValue("@MaCN", maCN);
                cmd.Parameters.AddWithValue("@HanhDong", hanhDong);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }
    
    public bool HardDelete(int maNQ, int maCN, string hanhDong)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"DELETE FROM chitietquyen
                             WHERE MaNQ = @MaNQ
                             AND MaCN = @MaCN
                             AND HanhDong = @HanhDong
                            ;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNQ", maNQ);
                cmd.Parameters.AddWithValue("@MaCN", maCN);
                cmd.Parameters.AddWithValue("@HanhDong", hanhDong);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public ChiTietQuyenDto GetById(int maNQ, int maCN, string hanhDong)
    {
        ChiTietQuyenDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaCN, MaNQ, HanhDong FROM chitietquyen WHERE Status = 1 AND MaNQ = @MaNQ AND MaCN = @MaCN AND HanhDong = @HanhDong ", conn);
        
        cmd.Parameters.AddWithValue("@MaNQ", maNQ);
        cmd.Parameters.AddWithValue("@MaCN", maCN);
        cmd.Parameters.AddWithValue("@HanhDong", hanhDong);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new ChiTietQuyenDto
            {
                MaCN = reader.GetInt32("MaCN"),
                MaNQ = reader.GetInt32("MaNQ"),
                HanhDong = reader.GetString("HanhDong")
            };
        }

        return result;
    }

    public List<ChiTietQuyenDto> GetByMaNQMaCN(int maNQ, int maCN)
    {
        List<ChiTietQuyenDto> result = new();
        using var con = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaCN, MaNQ, HanhDong FROM chitietquyen WHERE Status = 1 AND MaCN = @MaCN AND MaNQ = @MaNQ ", con);

        cmd.Parameters.AddWithValue("@MaCN", maCN);
        cmd.Parameters.AddWithValue("@MaNQ", maNQ);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            result.Add(new ChiTietQuyenDto
            {
                MaCN = reader.GetInt32("MaCN"),
                MaNQ = reader.GetInt32("MaNQ"),
                HanhDong = reader.GetString("HanhDong")
            });
        }
        return result;
    }
    
}
