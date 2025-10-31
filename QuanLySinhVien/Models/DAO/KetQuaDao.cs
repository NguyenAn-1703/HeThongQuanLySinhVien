using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System.Collections.Generic;

public class KetQuaDao
{
    private static KetQuaDao _instance;
    private KetQuaDao() { }

    public static KetQuaDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new KetQuaDao();
        }
        return _instance;
    }

    public List<KetQuaDto> GetAll()
    {
        List<KetQuaDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaKQ, MaHP, MaSV, DiemThi, DiemHe4, DiemHe10, HocKy, Nam
              FROM KetQua
              WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new KetQuaDto
            {
                MaKQ = reader.GetInt32("MaKQ"),
                MaHP = reader.GetInt32("MaHP"),
                MaSV = reader.GetInt32("MaSV"),
                DiemThi = reader.GetFloat("DiemThi"),
                DiemHe4 = reader.GetFloat("DiemHe4"),
                DiemHe10 = reader.GetFloat("DiemHe10"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam")
            });
        }

        return result;
    }

    public bool Insert(KetQuaDto ketQuaDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO KetQua (MaHP, MaSV, DiemThi, DiemHe4, DiemHe10, HocKy, Nam)
                             VALUES (@MaHP, @MaSV, @DiemThi, @DiemHe4, @DiemHe10, @HocKy, @Nam)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaHP", ketQuaDto.MaHP);
                cmd.Parameters.AddWithValue("@MaSV", ketQuaDto.MaSV);
                cmd.Parameters.AddWithValue("@DiemThi", ketQuaDto.DiemThi);
                cmd.Parameters.AddWithValue("@DiemHe4", ketQuaDto.DiemHe4);
                cmd.Parameters.AddWithValue("@DiemHe10", ketQuaDto.DiemHe10);
                cmd.Parameters.AddWithValue("@HocKy", ketQuaDto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", ketQuaDto.Nam);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(KetQuaDto ketQuaDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE KetQua
                             SET MaHP = @MaHP,
                                 MaSV = @MaSV,
                                 DiemThi = @DiemThi,
                                 DiemHe4 = @DiemHe4,
                                 DiemHe10 = @DiemHe10,
                                 HocKy = @HocKy,
                                 Nam = @Nam
                             WHERE MaKQ = @MaKQ";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKQ", ketQuaDto.MaKQ);
                cmd.Parameters.AddWithValue("@MaHP", ketQuaDto.MaHP);
                cmd.Parameters.AddWithValue("@MaSV", ketQuaDto.MaSV);
                cmd.Parameters.AddWithValue("@DiemThi", ketQuaDto.DiemThi);
                cmd.Parameters.AddWithValue("@DiemHe4", ketQuaDto.DiemHe4);
                cmd.Parameters.AddWithValue("@DiemHe10", ketQuaDto.DiemHe10);
                cmd.Parameters.AddWithValue("@HocKy", ketQuaDto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", ketQuaDto.Nam);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maKQ)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE KetQua
                             SET Status = 0
                             WHERE MaKQ = @MaKQ";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKQ", maKQ);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public KetQuaDto GetKetQuaById(int maKQ)
    {
        KetQuaDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaKQ, MaHP, MaSV, DiemThi, DiemHe4, DiemHe10, HocKy, Nam
              FROM KetQua
              WHERE Status = 1 AND MaKQ = @MaKQ", conn);
        cmd.Parameters.AddWithValue("@MaKQ", maKQ);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            result = new KetQuaDto
            {
                MaKQ = reader.GetInt32("MaKQ"),
                MaHP = reader.GetInt32("MaHP"),
                MaSV = reader.GetInt32("MaSV"),
                DiemThi = reader.GetFloat("DiemThi"),
                DiemHe4 = reader.GetFloat("DiemHe4"),
                DiemHe10 = reader.GetFloat("DiemHe10"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam")
            };
        }

        return result;
    }
}
