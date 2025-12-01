using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class HocPhiHocPhanDao
{
    private static HocPhiHocPhanDao _instance;

    private HocPhiHocPhanDao()
    {
    }

    public static HocPhiHocPhanDao GetInstance()
    {
        if (_instance == null)
            _instance = new HocPhiHocPhanDao();
        return _instance;
    }

    public List<HocPhiHocPhanDto> GetAll()
    {
        List<HocPhiHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaSV, MaHP, TongTien, HocKy, Nam FROM hocphihocphan WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new HocPhiHocPhanDto
            {
                MaSV = reader.GetInt32("MaSV"),
                MaHP = reader.GetInt32("MaHP"),
                TongTien = reader.GetDouble("TongTien"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam")
            });

        return result;
    }

    public bool Insert(HocPhiHocPhanDto hocPhiHocPhanDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO hocphihocphan (MaSV, MaHP, TongTien, HocKy, Nam)
                             VALUES (@MaSV, @MaHP, @TongTien, @HocKy, @Nam)";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", hocPhiHocPhanDto.MaSV);
                cmd.Parameters.AddWithValue("@MaHP", hocPhiHocPhanDto.MaHP);
                cmd.Parameters.AddWithValue("@TongTien", hocPhiHocPhanDto.TongTien);
                cmd.Parameters.AddWithValue("@HocKy", hocPhiHocPhanDto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", hocPhiHocPhanDto.Nam);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(HocPhiHocPhanDto hocPhiHocPhanDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE hocphihocphan 
                             SET TongTien = @TongTien, HocKy = @HocKy, Nam = @Nam
                             WHERE MaSV = @MaSV AND MaHP = @MaHP";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", hocPhiHocPhanDto.MaSV);
                cmd.Parameters.AddWithValue("@MaHP", hocPhiHocPhanDto.MaHP);
                cmd.Parameters.AddWithValue("@TongTien", hocPhiHocPhanDto.TongTien);
                cmd.Parameters.AddWithValue("@HocKy", hocPhiHocPhanDto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", hocPhiHocPhanDto.Nam);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maSV, int maHP)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE hocphihocphan
                             SET Status = 0
                             WHERE MaSV = @MaSV AND MaHP = @MaHP";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", maSV);
                cmd.Parameters.AddWithValue("@MaHP", maHP);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public HocPhiHocPhanDto GetById(int maSV, int maHP)
    {
        HocPhiHocPhanDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaSV, MaHP, TongTien, HocKy, Nam 
              FROM hocphihocphan 
              WHERE Status = 1 AND MaSV = @MaSV AND MaHP = @MaHP",
            conn);
        cmd.Parameters.AddWithValue("@MaSV", maSV);
        cmd.Parameters.AddWithValue("@MaHP", maHP);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new HocPhiHocPhanDto
            {
                MaSV = reader.GetInt32("MaSV"),
                MaHP = reader.GetInt32("MaHP"),
                TongTien = reader.GetDouble("TongTien"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam")
            };

        return result;
    }

    public List<HocPhiHocPhanDto> GetByMaSVHocKyNam(int maSV, int hocKy, string nam)
    {
        List<HocPhiHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaSV, MaHP, TongTien, HocKy, Nam FROM hocphihocphan " +
            "WHERE Status = 1 AND MaSV = @MaSV AND HocKy = @HocKy AND Nam = @Nam", conn);
        cmd.Parameters.AddWithValue("@MaSV", maSV);
        cmd.Parameters.AddWithValue("@HocKy", hocKy);
        cmd.Parameters.AddWithValue("@Nam", nam);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new HocPhiHocPhanDto
            {
                MaSV = reader.GetInt32("MaSV"),
                MaHP = reader.GetInt32("MaHP"),
                TongTien = reader.GetDouble("TongTien"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam")
            });

        return result;
    }
    
    public record NamHocKy(string Nam, int HocKy);

    public List<NamHocKy> GetNamHocKySVDaDK(int maSV)
    {
        var result = new List<NamHocKy>();

        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT DISTINCT Nam, HocKy FROM hocphihocphan WHERE Status = 1 AND MaSV = @MaSV",
            conn
        );
        cmd.Parameters.AddWithValue("@MaSV", maSV);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string nam = reader.GetString("Nam");
            int hocKy = reader.GetInt32("HocKy");

            result.Add(new NamHocKy(nam, hocKy));
        }

        return result;
    }

}