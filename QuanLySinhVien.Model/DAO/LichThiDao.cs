using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;
using System.Globalization;

namespace QuanLySinhVien.Models.DAO;

public class LichThiDao
{
    // Lấy danh sách lịch thi của 1 sinh viên
    public List<LichThiSVDto> GetLichThiBySinhVien(int maSV)
    {
        List<LichThiSVDto> result = new();
        using var conn = MyConnection.GetConnection();
        
        // Query: Join CaThi_SinhVien -> CaThi -> HocPhan & PhongHoc
        // Subquery: Đếm số lượng sinh viên trong ca thi đó để lấy Sĩ số
        var query = @"
            SELECT 
                ct.MaHP, hp.TenHP, 
                ct.ThoiGianBatDau, ct.HocKy, ct.Nam,
                ph.TenPH, ph.CoSo,
                (SELECT COUNT(*) FROM CaThi_SinhVien ctsv_count WHERE ctsv_count.MaCT = ct.MaCT) as SiSo
            FROM CaThi_SinhVien ctsv
            JOIN CaThi ct ON ctsv.MaCT = ct.MaCT
            JOIN HocPhan hp ON ct.MaHP = hp.MaHP
            JOIN PhongHoc ph ON ct.MaPH = ph.MaPH
            WHERE ctsv.MaSV = @MaSV AND ct.Status = 1
            ORDER BY ct.ThoiGianBatDau ASC";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSV);

        using var reader = cmd.ExecuteReader();
        int stt = 1;
        while (reader.Read())
        {
            // Xử lý chuỗi thời gian từ DB (VD: '15/05/2025 07:30:00')
            string rawTime = reader.GetString("ThoiGianBatDau");
            DateTime dt = DateTime.ParseExact(rawTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            result.Add(new LichThiSVDto
            {
                STT = stt++,
                MaMH = reader.GetInt32("MaHP").ToString(),
                TenMonHoc = reader.GetString("TenHP"),
                SiSo = reader.GetInt32("SiSo"),
                NgayThi = dt.ToString("dd/MM/yyyy"),
                GioBatDau = dt.ToString("HH:mm"),
                PhongThi = reader.GetString("TenPH"),
                CoSo = reader.GetString("CoSo"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                ThoiGianThuc = dt
            });
        }

        return result;
    }

    // Lấy danh sách các Học kỳ - Năm có trong lịch thi của SV để đổ vào Combobox
    public List<string> GetListHocKyNam(int maSV)
    {
        List<string> list = new();
        using var conn = MyConnection.GetConnection();
        var query = @"
            SELECT DISTINCT ct.HocKy, ct.Nam
            FROM CaThi_SinhVien ctsv
            JOIN CaThi ct ON ctsv.MaCT = ct.MaCT
            WHERE ctsv.MaSV = @MaSV AND ct.Status = 1
            ORDER BY ct.Nam DESC, ct.HocKy DESC";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSV);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string item = $"Học kỳ {reader.GetInt32("HocKy")} năm {reader.GetString("Nam")}";
            list.Add(item);
        }
        return list;
    }
}