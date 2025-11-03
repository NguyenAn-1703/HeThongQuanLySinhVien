using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class DiemSinhVienDao
{
    private static DiemSinhVienDao? _instance;

    private DiemSinhVienDao()
    {
    }

    public static DiemSinhVienDao GetInstance()
    {
        if (_instance == null) _instance = new DiemSinhVienDao();
        return _instance;
    }

    // Lấy điểm sinh viên theo kỳ
    public List<DiemSinhVienDto> GetDiemTheoKy(int maSinhVien, int hocKy, string nam)
    {
        List<DiemSinhVienDto> result = new();
        using var conn = MyConnection.GetConnection();

        var sql = @"
            SELECT 
                kq.MaKQ,
                kq.MaSV,
                kq.MaHP,
                hp.TenHP,
                hp.SoTinChi,
                IFNULL(dqt.DiemSo, 0) AS DiemQuaTrinh,
                IFNULL(kq.DiemThi, 0) AS DiemThi,
                IFNULL(kq.DiemHe10, 0) AS DiemHe10,
                IFNULL(kq.DiemHe4, 0) AS DiemHe4,
                kq.HocKy,
                kq.Nam,
                CASE 
                    WHEN kq.DiemHe4 >= 1.0 THEN 'Đạt'
                    WHEN kq.DiemThi = -1 THEN 'Chưa thi'
                    ELSE 'Không đạt'
                END AS KetQua
            FROM KetQua kq
            INNER JOIN HocPhan hp ON kq.MaHP = hp.MaHP
            LEFT JOIN DiemQuaTrinh dqt ON kq.MaDQT = dqt.MaDQT
            WHERE kq.MaSV = @MaSV 
            AND kq.HocKy = @HocKy 
            AND kq.Nam = @Nam
            AND kq.Status = 1
            ORDER BY hp.TenHP";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSinhVien);
        cmd.Parameters.AddWithValue("@HocKy", hocKy);
        cmd.Parameters.AddWithValue("@Nam", nam);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new DiemSinhVienDto
            {
                MaKetQua = reader.GetInt32("MaKQ"),
                MaSinhVien = reader.GetInt32("MaSV"),
                MaHocPhan = reader.GetInt32("MaHP"),
                TenHocPhan = reader.GetString("TenHP"),
                SoTinChi = reader.GetInt32("SoTinChi"),
                DiemQuaTrinh = reader.GetFloat("DiemQuaTrinh"),
                DiemThi = reader.GetFloat("DiemThi"),
                DiemHe10 = reader.GetFloat("DiemHe10"),
                DiemHe4 = reader.GetFloat("DiemHe4"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                KetQua = reader.GetString("KetQua")
            });

        return result;
    }

    // Lấy danh sách kỳ học của sinh viên
    public List<(int HocKy, string Nam)> GetDanhSachKyHoc(int maSinhVien)
    {
        List<(int HocKy, string Nam)> result = new();
        using var conn = MyConnection.GetConnection();

        var sql = @"
            SELECT DISTINCT HocKy, Nam
            FROM KetQua
            WHERE MaSV = @MaSV AND Status = 1
            ORDER BY Nam DESC, HocKy DESC";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSinhVien);

        using var reader = cmd.ExecuteReader();

        while (reader.Read()) result.Add((reader.GetInt32("HocKy"), reader.GetString("Nam")));

        return result;
    }

    // Tính điểm trung bình theo kỳ
    public (float DiemTBHe10, float DiemTBHe4, int TongTinChi) TinhDiemTrungBinhTheoKy(int maSinhVien, int hocKy,
        string nam)
    {
        using var conn = MyConnection.GetConnection();

        var sql = @"
            SELECT 
                SUM(kq.DiemHe10 * hp.SoTinChi) / SUM(hp.SoTinChi) AS DiemTBHe10,
                SUM(kq.DiemHe4 * hp.SoTinChi) / SUM(hp.SoTinChi) AS DiemTBHe4,
                SUM(hp.SoTinChi) AS TongTinChi
            FROM KetQua kq
            INNER JOIN HocPhan hp ON kq.MaHP = hp.MaHP
            WHERE kq.MaSV = @MaSV 
            AND kq.HocKy = @HocKy 
            AND kq.Nam = @Nam
            AND kq.Status = 1
            AND kq.DiemThi >= 0";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSinhVien);
        cmd.Parameters.AddWithValue("@HocKy", hocKy);
        cmd.Parameters.AddWithValue("@Nam", nam);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            var diemTBHe10 = reader.IsDBNull(0) ? 0 : reader.GetFloat(0);
            var diemTBHe4 = reader.IsDBNull(1) ? 0 : reader.GetFloat(1);
            var tongTinChi = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

            return (diemTBHe10, diemTBHe4, tongTinChi);
        }

        return (0, 0, 0);
    }
}