using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class HocPhiSVDao
{
    private static HocPhiSVDao _instance;

    private HocPhiSVDao()
    {
    }

    public static HocPhiSVDao GetInstance()
    {
        if (_instance == null) _instance = new HocPhiSVDao();
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

        return result;
    }

    public bool Insert(HocPhiSVDto hocPhiSVDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO hocphisv (MaSV, HocKy, Nam, TongHocPhi, DaThu, TrangThai)
                             VALUES (@MaSV, @HocKy, @Nam, @TongHocPhi, @DaThu, @TrangThai)";
            using (var cmd = new MySqlCommand(query, conn))
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
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE hocphisv 
                             SET MaSV = @MaSV,
                                 HocKy = @HocKy,
                                 Nam = @Nam,
                                 TongHocPhi = @TongHocPhi,
                                 DaThu = @DaThu,
                                 TrangThai = @TrangThai
                             WHERE MaHP = @MaHP";
            using (var cmd = new MySqlCommand(query, conn))
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
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE hocphisv
                             SET Status = 0
                             WHERE MaHP = @MaHP;";
            using (var cmd = new MySqlCommand(query, conn))
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

        return result;
    }

    public List<HocPhiSVDto> GetByMaSV(int maSV)
    {
        List<HocPhiSVDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaHP, MaSV, HocKy, Nam, TongHocPhi, DaThu, TrangThai FROM hocphisv WHERE Status = 1 AND MaSV = @MaSV",
            conn);
        cmd.Parameters.AddWithValue("@MaSV", maSV);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
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

        return result;
    }

    public List<HocPhiHocPhanDetailDto> GetDetailsByMaSVHocKyNam(int maSV, int hocKy, string nam)
    {
        List<HocPhiHocPhanDetailDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT 
                hphp.MaHP,
                hp.TenHP,
                hp.SoTinChi,
                hphp.TongTien AS SoTien,
                hphp.TongTien AS PhaiThu,
                CASE 
                    WHEN hphp.Status = 0 THEN 'Chưa đóng'
                    WHEN hphp.Status = 1 THEN 'Đã đóng'
                    ELSE 'Chưa đóng'
                END AS TrangThai
            FROM hocphihocphan hphp
            INNER JOIN hocphan hp ON hphp.MaHP = hp.MaHP AND hp.Status = 1
            WHERE hphp.MaSV = @MaSV 
                AND hphp.HocKy = @HocKy 
                AND hphp.Nam = @Nam
            ORDER BY hp.MaHP",
            conn);
        cmd.Parameters.AddWithValue("@MaSV", maSV);
        cmd.Parameters.AddWithValue("@HocKy", hocKy);
        cmd.Parameters.AddWithValue("@Nam", nam);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new HocPhiHocPhanDetailDto
            {
                MaHP = reader.GetInt32("MaHP"),
                TenHP = reader.GetString("TenHP"),
                SoTinChi = reader.GetInt32("SoTinChi"),
                SoTien = reader.GetDouble("SoTien"),
                PhaiThu = reader.GetDouble("PhaiThu"),
                TrangThai = reader.GetString("TrangThai")
            });

        return result;
    }
}