using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System;
using System.Collections.Generic;

public class ChuongTrinhDaoTao_HocPhanDao
{
    private static ChuongTrinhDaoTao_HocPhanDao _instance;
    private ChuongTrinhDaoTao_HocPhanDao() { }

    public static ChuongTrinhDaoTao_HocPhanDao GetInstance()
    {
        if (_instance == null)
            _instance = new ChuongTrinhDaoTao_HocPhanDao();
        return _instance;
    }

    public List<ChuongTrinhDaoTao_HocPhanDto> GetAll()
    {
        List<ChuongTrinhDaoTao_HocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaCTDT, MaHP FROM chuongtrinhdaotao_hocphan WHERE Status = 1",
            conn
        );
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new ChuongTrinhDaoTao_HocPhanDto
            {
                MaCTDT = reader.GetInt32("MaCTDT"),
                MaHP = reader.GetInt32("MaHP")
            });
        }

        return result;
    }

    public bool Insert(ChuongTrinhDaoTao_HocPhanDto dto)
    {
        int rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        string query = @"INSERT INTO chuongtrinhdaotao_hocphan (MaCTDT, MaHP) 
                         VALUES (@MaCTDT, @MaHP)";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCTDT", dto.MaCTDT);
        cmd.Parameters.AddWithValue("@MaHP", dto.MaHP);
        rowAffected = cmd.ExecuteNonQuery();

        return rowAffected > 0;
    }

    public bool Delete(int maCTDT, int maHP)
    {
        int rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        string query = @"UPDATE chuongtrinhdaotao_hocphan
                         SET Status = 0
                         WHERE MaCTDT = @MaCTDT AND MaHP = @MaHP";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCTDT", maCTDT);
        cmd.Parameters.AddWithValue("@MaHP", maHP);
        rowAffected = cmd.ExecuteNonQuery();

        return rowAffected > 0;
    }

    public ChuongTrinhDaoTao_HocPhanDto GetById(int maCTDT, int maHP)
    {
        ChuongTrinhDaoTao_HocPhanDto result = null;
        using var conn = MyConnection.GetConnection();
        string query = @"SELECT MaCTDT, MaHP 
                         FROM chuongtrinhdaotao_hocphan 
                         WHERE Status = 1 AND MaCTDT = @MaCTDT AND MaHP = @MaHP";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCTDT", maCTDT);
        cmd.Parameters.AddWithValue("@MaHP", maHP);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new ChuongTrinhDaoTao_HocPhanDto
            {
                MaCTDT = reader.GetInt32("MaCTDT"),
                MaHP = reader.GetInt32("MaHP")
            };
        }

        return result;
    }
    
    public List<ChuongTrinhDaoTao_HocPhanDto> GetByMaCTDT(int maCTDT)
    {
        List<ChuongTrinhDaoTao_HocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaCTDT, MaHP FROM chuongtrinhdaotao_hocphan WHERE Status = 1 And MaCTDT = @MaCTDT",
            conn
        );
        cmd.Parameters.AddWithValue("@MaCTDT", maCTDT);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new ChuongTrinhDaoTao_HocPhanDto
            {
                MaCTDT = reader.GetInt32("MaCTDT"),
                MaHP = reader.GetInt32("MaHP")
            });
        }

        return result;
    }
    
    public bool HardDelete(int maCTDT, int maHP)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"DELETE FROM chuongtrinhdaotao_hocphan
                             WHERE MaCTDT = @MaCTDT
                             AND MaHP = @MaHP
                            ;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaCTDT", maCTDT);
                cmd.Parameters.AddWithValue("@MaHP", maHP);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }
    
    public bool DeleteAllByMaCTDT(int maCTDT)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"DELETE FROM chuongtrinhdaotao_hocphan
                             WHERE MaCTDT = @MaCTDT
                            ;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaCTDT", maCTDT);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }
    
    
}
