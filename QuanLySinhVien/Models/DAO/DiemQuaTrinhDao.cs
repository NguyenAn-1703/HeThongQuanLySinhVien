using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System.Collections.Generic;

public class DiemQuaTrinhDao
{
    private static DiemQuaTrinhDao _instance;
    private DiemQuaTrinhDao() { }

    public static DiemQuaTrinhDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DiemQuaTrinhDao();
        }
        return _instance;
    }

    public List<DiemQuaTrinhDto> GetAll()
    {
        List<DiemQuaTrinhDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaDQT, MaKQ, DiemSo
              FROM DiemQuaTrinh
              WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new DiemQuaTrinhDto
            {
                MaDQT = reader.GetInt32("MaDQT"),
                MaKQ = reader.GetInt32("MaKQ"),
                DiemSo = reader.GetFloat("DiemSo")
            });
        }

        return result;
    }

    public bool Insert(DiemQuaTrinhDto diemQuaTrinhDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO DiemQuaTrinh (MaKQ, DiemSo)
                             VALUES (@MaKQ, @DiemSo)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKQ", diemQuaTrinhDto.MaKQ);
                cmd.Parameters.AddWithValue("@DiemSo", diemQuaTrinhDto.DiemSo);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(DiemQuaTrinhDto diemQuaTrinhDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE DiemQuaTrinh
                             SET MaKQ = @MaKQ,
                                 DiemSo = @DiemSo
                             WHERE MaDQT = @MaDQT";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDQT", diemQuaTrinhDto.MaDQT);
                cmd.Parameters.AddWithValue("@MaKQ", diemQuaTrinhDto.MaKQ);
                cmd.Parameters.AddWithValue("@DiemSo", diemQuaTrinhDto.DiemSo);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maDQT)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE DiemQuaTrinh
                             SET Status = 0
                             WHERE MaDQT = @MaDQT";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDQT", maDQT);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public DiemQuaTrinhDto GetDiemQuaTrinhById(int maDQT)
    {
        DiemQuaTrinhDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaDQT, MaKQ, DiemSo
              FROM DiemQuaTrinh
              WHERE Status = 1 AND MaDQT = @MaDQT", conn);
        cmd.Parameters.AddWithValue("@MaDQT", maDQT);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            result = new DiemQuaTrinhDto
            {
                MaDQT = reader.GetInt32("MaDQT"),
                MaKQ = reader.GetInt32("MaKQ"),
                DiemSo = reader.GetFloat("DiemSo")
            };
        }

        return result;
    }
    
    public bool HardDelete(int maDQT)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"DELETE FROM DiemQuaTrinh
                         WHERE MaDQT = @MaDQT";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaDQT", maDQT);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

}
