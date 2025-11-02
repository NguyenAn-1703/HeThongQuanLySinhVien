using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System;
using System.Collections.Generic;

public class HocPhiTinChiDao
{
    private static HocPhiTinChiDao _instance;
    private HocPhiTinChiDao() { }

    public static HocPhiTinChiDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new HocPhiTinChiDao();
        }
        return _instance;
    }

    public List<HocPhiTinChiDto> GetAll()
    {
        List<HocPhiTinChiDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaHPTC, MaNganh, SoTienMotTinChi, ThoiGianApDung FROM hocphitinchi WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new HocPhiTinChiDto
            {
                MaHPTC = reader.GetInt32("MaHPTC"),
                MaNganh = reader.GetInt32("MaNganh"),
                SoTienMotTinChi = reader.GetDouble("SoTienMotTinChi"),
                ThoiGianApDung = reader.GetString("ThoiGianApDung")
            });
        }

        return result;
    }

    public bool Insert(HocPhiTinChiDto hocPhiTinChiDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO hocphitinchi (MaNganh, SoTienMotTinChi, ThoiGianApDung)
                             VALUES (@MaNganh, @SoTienMotTinChi, @ThoiGianApDung)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNganh", hocPhiTinChiDto.MaNganh);
                cmd.Parameters.AddWithValue("@SoTienMotTinChi", hocPhiTinChiDto.SoTienMotTinChi);
                cmd.Parameters.AddWithValue("@ThoiGianApDung", hocPhiTinChiDto.ThoiGianApDung);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public bool Update(HocPhiTinChiDto hocPhiTinChiDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE hocphitinchi 
                             SET MaNganh = @MaNganh,
                                 SoTienMotTinChi = @SoTienMotTinChi,
                                 ThoiGianApDung = @ThoiGianApDung
                             WHERE MaHPTC = @MaHPTC";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaHPTC", hocPhiTinChiDto.MaHPTC);
                cmd.Parameters.AddWithValue("@MaNganh", hocPhiTinChiDto.MaNganh);
                cmd.Parameters.AddWithValue("@SoTienMotTinChi", hocPhiTinChiDto.SoTienMotTinChi);
                cmd.Parameters.AddWithValue("@ThoiGianApDung", hocPhiTinChiDto.ThoiGianApDung);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public bool Delete(int maHPTC)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE hocphitinchi
                             SET Status = 0
                             WHERE MaHPTC = @MaHPTC;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaHPTC", maHPTC);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public HocPhiTinChiDto GetById(int maHPTC)
    {
        HocPhiTinChiDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaHPTC, MaNganh, SoTienMotTinChi, ThoiGianApDung FROM hocphitinchi WHERE Status = 1 AND MaHPTC = @MaHPTC",
            conn);
        cmd.Parameters.AddWithValue("@MaHPTC", maHPTC);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new HocPhiTinChiDto
            {
                MaHPTC = reader.GetInt32("MaHPTC"),
                MaNganh = reader.GetInt32("MaNganh"),
                SoTienMotTinChi = reader.GetDouble("SoTienMotTinChi"),
                ThoiGianApDung = reader.GetString("ThoiGianApDung")
            };
        }

        return result;
    }

    public List<HocPhiTinChiDto> GetByMaNganh(int maNganh)
    {
        List<HocPhiTinChiDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaHPTC, MaNganh, SoTienMotTinChi, ThoiGianApDung FROM hocphitinchi " +
                                         "WHERE Status = 1 AND MaNganh = @MaNganh", conn);
        cmd.Parameters.AddWithValue("@MaNganh", maNganh);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new HocPhiTinChiDto
            {
                MaHPTC = reader.GetInt32("MaHPTC"),
                MaNganh = reader.GetInt32("MaNganh"),
                SoTienMotTinChi = reader.GetDouble("SoTienMotTinChi"),
                ThoiGianApDung = reader.GetString("ThoiGianApDung")
            });
        }

        return result;
    }
}
