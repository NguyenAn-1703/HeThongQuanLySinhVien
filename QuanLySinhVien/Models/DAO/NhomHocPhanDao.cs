using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

using System.Collections.Generic;

using System.Collections.Generic;

public class NhomHocPhanDao
{
    private static NhomHocPhanDao _instance;
    private NhomHocPhanDao() { }

    public static NhomHocPhanDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new NhomHocPhanDao();
        }
        return _instance;
    }

    public List<NhomHocPhanDto> GetAll()
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo FROM NhomHocPhan WHERE Status = 1",
            conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new NhomHocPhanDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaGV = reader.GetInt32("MaGV"),
                MaHP = reader.GetInt32("MaHP"),
                MaLichDK = reader.GetInt32("MaLichDK"),
                MaLop = reader.GetInt32("MaLop"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                SiSo = reader.GetInt32("SiSo"),
            });
        }

        return result;
    }

    public bool Insert(NhomHocPhanDto dto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"INSERT INTO NhomHocPhan (MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo)
                             VALUES (@MaGV, @MaHP, @MaLichDK, @MaLop, @HocKy, @Nam, @SiSo)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaGV", dto.MaGV);
                cmd.Parameters.AddWithValue("@MaHP", dto.MaHP);
                cmd.Parameters.AddWithValue("@MaLichDK", dto.MaLichDK);
                cmd.Parameters.AddWithValue("@MaLop", dto.MaLop);
                cmd.Parameters.AddWithValue("@HocKy", dto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", dto.Nam);
                cmd.Parameters.AddWithValue("@SiSo", dto.SiSo);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public bool Update(NhomHocPhanDto dto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE NhomHocPhan
                             SET MaGV = @MaGV,
                                 MaHP = @MaHP,
                                 MaLichDK = @MaLichDK,
                                 MaLop = @MaLop,
                                 HocKy = @HocKy,
                                 Nam = @Nam,
                                 SiSo = @SiSo
                             WHERE MaNHP = @MaNHP";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNHP", dto.MaNHP);
                cmd.Parameters.AddWithValue("@MaGV", dto.MaGV);
                cmd.Parameters.AddWithValue("@MaHP", dto.MaHP);
                cmd.Parameters.AddWithValue("@MaLichDK", dto.MaLichDK);
                cmd.Parameters.AddWithValue("@MaLop", dto.MaLop);
                cmd.Parameters.AddWithValue("@HocKy", dto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", dto.Nam);
                cmd.Parameters.AddWithValue("@SiSo", dto.SiSo);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public bool Delete(int maNHP)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE NhomHocPhan
                             SET Status = 0
                             WHERE MaNHP = @MaNHP";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNHP", maNHP);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 0;
    }

    public NhomHocPhanDto GetById(int maNHP)
    {
        NhomHocPhanDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo FROM NhomHocPhan WHERE Status = 1 AND MaNHP = @MaNHP",
            conn);
        cmd.Parameters.AddWithValue("@MaNHP", maNHP);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new NhomHocPhanDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaGV = reader.GetInt32("MaGV"),
                MaHP = reader.GetInt32("MaHP"),
                MaLichDK = reader.GetInt32("MaLichDK"),
                MaLop = reader.GetInt32("MaLop"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                SiSo = reader.GetInt32("SiSo"),
            };
        }

        return result;
    }

    public List<NhomHocPhanDto> GetByLichMaDangKy(int maLichDk)
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo FROM NhomHocPhan WHERE Status = 1 AND MaLichDK = @MaLichDK",
            conn);
        cmd.Parameters.AddWithValue("@MaLichDK", maLichDk);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new NhomHocPhanDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaGV = reader.GetInt32("MaGV"),
                MaHP = reader.GetInt32("MaHP"),
                MaLichDK = reader.GetInt32("MaLichDK"),
                MaLop = reader.GetInt32("MaLop"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                SiSo = reader.GetInt32("SiSo"),
            });
        }

        return result;
    }

    public List<NhomHocPhanDto> GetByHkyNamMaHP(int hky, string nam, int maHp)
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo FROM NhomHocPhan " +
            "WHERE Status = 1 AND HocKy = @HocKy AND Nam = @Nam AND MaHP = @MaHP",
            conn);
        cmd.Parameters.AddWithValue("@HocKy", hky);
        cmd.Parameters.AddWithValue("@Nam", nam);
        cmd.Parameters.AddWithValue("@MaHP", maHp);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new NhomHocPhanDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaGV = reader.GetInt32("MaGV"),
                MaHP = reader.GetInt32("MaHP"),
                MaLichDK = reader.GetInt32("MaLichDK"),
                MaLop = reader.GetInt32("MaLop"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                SiSo = reader.GetInt32("SiSo"),
            });
        }

        return result;
    }
    
    public List<NhomHocPhanDto> GetByHkyNam(int hky, string nam)
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo FROM NhomHocPhan " +
            "WHERE Status = 1 AND HocKy = @HocKy AND Nam = @Nam",
            conn);
        cmd.Parameters.AddWithValue("@HocKy", hky);
        cmd.Parameters.AddWithValue("@Nam", nam);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new NhomHocPhanDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaGV = reader.GetInt32("MaGV"),
                MaHP = reader.GetInt32("MaHP"),
                MaLichDK = reader.GetInt32("MaLichDK"),
                MaLop = reader.GetInt32("MaLop"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                SiSo = reader.GetInt32("SiSo"),
            });
        }

        return result;
    }
    
    public List<NhomHocPhanDto> GetByHkyNamMaGV(int hky, string nam, int maGV)
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo FROM NhomHocPhan " +
            "WHERE Status = 1 AND HocKy = @HocKy AND Nam = @Nam AND MaGV = @MaGV",
            conn);
        cmd.Parameters.AddWithValue("@HocKy", hky);
        cmd.Parameters.AddWithValue("@Nam", nam);
        cmd.Parameters.AddWithValue("@MaGV", maGV);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new NhomHocPhanDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaGV = reader.GetInt32("MaGV"),
                MaHP = reader.GetInt32("MaHP"),
                MaLichDK = reader.GetInt32("MaLichDK"),
                MaLop = reader.GetInt32("MaLop"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                SiSo = reader.GetInt32("SiSo"),
            });
        }

        return result;
    }
}



