using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System;
using System.Collections.Generic;

public class HocPhiSVDao
{
    private static HocPhiSVDao _instance;
    private HocPhiSVDao() { }

    public static HocPhiSVDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new HocPhiSVDao();
        }
        return _instance;
    }

    public List<HocPhiSVDto> GetAll()
    {
        List<HocPhiSVDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaHP, MaSV, HocKy, Nam, TongHocPhi, DaThu, TrangThai FROM hocphisv WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new HocPhiSVDto
            {
                MaHP = reader.GetInt32("MaHP"),
                MaSV = reader.GetInt32("MaSV"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                TongHocPhi = reader.GetDouble("TongHocPhi"),
                DaThu = reader.GetDouble("DaThu"),
                TrangThai = reader.GetString("TrangThai")
            });
        }

        return result;
    }

    public bool Insert(HocPhiSVDto hocPhiSVDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO hocphisv (MaSV, HocKy, Nam, TongHocPhi, DaThu, TrangThai)
                             VALUES (@MaSV, @HocKy, @Nam, @TongHocPhi, @DaThu, @TrangThai)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", hocPhiSVDto.MaSV);
                cmd.Parameters.AddWithValue("@HocKy", hocPhiSVDto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", hocPhiSVDto.Nam);
                cmd.Parameters.AddWithValue("@TongHocPhi", hocPhiSVDto.TongHocPhi);
                cmd.Parameters.AddWithValue("@DaThu", hocPhiSVDto.DaThu);
                cmd.Parameters.AddWithValue("@TrangThai", hocPhiSVDto.TrangThai);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public bool Update(HocPhiSVDto hocPhiSVDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE hocphisv 
                             SET MaSV = @MaSV,
                                 HocKy = @HocKy,
                                 Nam = @Nam,
                                 TongHocPhi = @TongHocPhi,
                                 DaThu = @DaThu,
                                 TrangThai = @TrangThai
                             WHERE MaHP = @MaHP";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaHP", hocPhiSVDto.MaHP);
                cmd.Parameters.AddWithValue("@MaSV", hocPhiSVDto.MaSV);
                cmd.Parameters.AddWithValue("@HocKy", hocPhiSVDto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", hocPhiSVDto.Nam);
                cmd.Parameters.AddWithValue("@TongHocPhi", hocPhiSVDto.TongHocPhi);
                cmd.Parameters.AddWithValue("@DaThu", hocPhiSVDto.DaThu);
                cmd.Parameters.AddWithValue("@TrangThai", hocPhiSVDto.TrangThai);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public bool Delete(int maHP)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE hocphisv
                             SET Status = 0
                             WHERE MaHP = @MaHP;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaHP", maHP);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public HocPhiSVDto GetById(int maHP)
    {
        HocPhiSVDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaHP, MaSV, HocKy, Nam, TongHocPhi, DaThu, TrangThai FROM hocphisv WHERE Status = 1 AND MaHP = @MaHP",
            conn);
        cmd.Parameters.AddWithValue("@MaHP", maHP);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new HocPhiSVDto
            {
                MaHP = reader.GetInt32("MaHP"),
                MaSV = reader.GetInt32("MaSV"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                TongHocPhi = reader.GetDouble("TongHocPhi"),
                DaThu = reader.GetDouble("DaThu"),
                TrangThai = reader.GetString("TrangThai")
            };
        }

        return result;
    }
}
