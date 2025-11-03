using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class ThongTinSinhVienDao
{
    private static ThongTinSinhVienDao? _instance;

    private ThongTinSinhVienDao()
    {
    }

    public static ThongTinSinhVienDao GetInstance()
    {
        if (_instance == null) _instance = new ThongTinSinhVienDao();
        return _instance;
    }

    public ThongTinSinhVienDto? GetByMaSinhVien(int maSinhVien)
    {
        ThongTinSinhVienDto? result = null;
        using var conn = MyConnection.GetConnection();

        var sql = @"
            SELECT 
                s.MaSV,
                s.TenSV,
                s.NgaySinhSV,
                s.GioiTinhSV,
                s.TrangThaiSV,
                s.SoDienThoaiSV,
                s.QueQuanSV,
                s.EmailSV,
                s.CCCDSV,
                s.AnhDaiDienSV,
                IFNULL(n.TenNganh, 'Chưa xác định') AS TenNganh,
                IFNULL(l.TenLop, 'Chưa xác định') AS TenLop,
                IFNULL(k.TenKhoa, 'Chưa xác định') AS TenKhoa,
                IFNULL(ctdt.TrinhDo, 'Chưa xác định') AS BacDaoTao,
                IFNULL(kh.NienKhoaHoc, 'Chưa xác định') AS NienKhoa,
                IFNULL(gv.TenGV, 'Chưa có') AS TenCoVan,
                IFNULL(gv.SoDienThoai, '') AS SdtCoVan,
                IFNULL(gv.Email, '') AS EmailCoVan,
                IFNULL(tk.TenDangNhap, '') AS TaiKhoanCoVan
            FROM SinhVien s
            LEFT JOIN Lop l ON s.MaLop = l.MaLop
            LEFT JOIN Nganh n ON l.MaNganh = n.MaNganh
            LEFT JOIN Khoa k ON n.MaKhoa = k.MaKhoa
            LEFT JOIN KhoaHoc kh ON s.MaKhoaHoc = kh.MaKhoaHoc
            LEFT JOIN ChuongTrinhDaoTao ctdt ON n.MaNganh = ctdt.MaNganh
            LEFT JOIN GiangVien gv ON l.MaGV = gv.MaGV
            LEFT JOIN TaiKhoan tk ON gv.MaTK = tk.MaTK
            WHERE s.MaSV = @MaSV AND s.Status = 1";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSinhVien);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new ThongTinSinhVienDto
            {
                MaSinhVien = reader.GetInt32("MaSV"),
                TenSinhVien = reader.GetString("TenSV"),
                NgaySinh = reader.GetString("NgaySinhSV"),
                GioiTinh = reader.GetString("GioiTinhSV"),
                TrangThai = reader.GetString("TrangThaiSV"),
                SdtSinhVien = reader.GetString("SoDienThoaiSV"),
                QueQuanSinhVien = reader.GetString("QueQuanSV"),
                Email = reader.GetString("EmailSV"),
                Cccd = reader.GetString("CCCDSV"),
                AnhDaiDienSinhVien = reader.IsDBNull(reader.GetOrdinal("AnhDaiDienSV"))
                    ? ""
                    : reader.GetString("AnhDaiDienSV"),
                Nganh = reader.GetString("TenNganh"),
                Lop = reader.GetString("TenLop"),
                Khoa = reader.GetString("TenKhoa"),
                BacDaoTao = reader.GetString("BacDaoTao"),
                NienKhoa = reader.GetString("NienKhoa"),
                TenCoVanHocTap = reader.GetString("TenCoVan"),
                SdtCoVanHocTap = reader.GetString("SdtCoVan"),
                EmailCoVanHocTap = reader.GetString("EmailCoVan"),
                TaiKhoanCoVanHocTap = reader.GetString("TaiKhoanCoVan")
            };

        return result;
    }
}