using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System;
using System.Collections.Generic;

public class KhoaHocDao
{
    private static KhoaHocDao _instance;
    private KhoaHocDao() { }

    public static KhoaHocDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new KhoaHocDao();
        }
        return _instance;
    }

    // Lấy tất cả khóa học còn hoạt động (Status = 1)
    public List<KhoaHocDto> GetAll()
    {
        List<KhoaHocDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaKhoaHoc, MaCKDT, TenKhoaHoc, NienKhoaHoc FROM khoahoc WHERE Status = 1", conn);
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

    // Thêm mới 1 khóa học
    public bool Insert(KhoaHocDto khoaHocDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO khoahoc (MaCKDT, TenKhoaHoc, NienKhoaHoc)
                             VALUES (@MaCKDT, @TenKhoaHoc, @NienKhoaHoc)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaCKDT", khoaHocDto.MaCKDT);
                cmd.Parameters.AddWithValue("@TenKhoaHoc", khoaHocDto.TenKhoaHoc);
                cmd.Parameters.AddWithValue("@NienKhoaHoc", khoaHocDto.NienKhoaHoc);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // Cập nhật khóa học
    public bool Update(KhoaHocDto khoaHocDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE khoahoc
                             SET MaCKDT = @MaCKDT,
                                 TenKhoaHoc = @TenKhoaHoc,
                                 NienKhoaHoc = @NienKhoaHoc
                             WHERE MaKhoaHoc = @MaKhoaHoc";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhoaHoc", khoaHocDto.MaKhoaHoc);
                cmd.Parameters.AddWithValue("@MaCKDT", khoaHocDto.MaCKDT);
                cmd.Parameters.AddWithValue("@TenKhoaHoc", khoaHocDto.TenKhoaHoc);
                cmd.Parameters.AddWithValue("@NienKhoaHoc", khoaHocDto.NienKhoaHoc);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // Xóa mềm (chuyển Status = 0)
    public bool Delete(int maKhoaHoc)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE khoahoc
                             SET Status = 0
                             WHERE MaKhoaHoc = @MaKhoaHoc";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhoaHoc", maKhoaHoc);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // Lấy thông tin chi tiết 1 khóa học
    public KhoaHocDto GetKhoaHocById(int maKhoaHoc)
    {
        KhoaHocDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaKhoaHoc, MaCKDT, TenKhoaHoc, NienKhoaHoc FROM khoahoc WHERE Status = 1 AND MaKhoaHoc = @MaKhoaHoc", conn);
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
}
