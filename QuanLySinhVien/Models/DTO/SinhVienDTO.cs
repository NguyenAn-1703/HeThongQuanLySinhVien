namespace QuanLySinhVien.Models;

public class SinhVienDTO
{
    public int MaSinhVien { get; set; }
    public string TenSinhVien { get; set; }
    public string NgaySinh { get; set; }
    public string GioiTinh { get; set; }
    public string Nganh { get; set; }
    public string TrangThai { get; set; }
    public string MaKhoaHoc { get; set; }
    public string MaLop { get; set; }
    public string MaTk { get; set; }
    public string SdtSinhVien { get; set; }
    public string QueQuanSinhVien { get; set; }
    public string Email { get; set; }
    public string CCCD { get; set; }
    public string AnhDaiDienSinhVien { get; set; }

    public SinhVienDTO() { }

    public SinhVienDTO(int maSinhVien, string tenSinhVien, String ngaySinh, string gioiTinh,  string nganh, string trangThai)
    {
        MaSinhVien = maSinhVien;
        TenSinhVien = tenSinhVien;
        NgaySinh = ngaySinh;
        GioiTinh = gioiTinh;
        Nganh = nganh;
        TrangThai = trangThai;
    }

    public SinhVienDTO(int maSinhVien, string tenSinhVien, string ngaySinh, string gioiTinh, string nganh,
        string trangThai, string maKhoaHoc, string maLop, string maTk,
        string sdtSinhVien, string queQuanSinhVien, string email, string cccd, string anhDaiDienSinhVien)
    {
        MaSinhVien = maSinhVien;
        TenSinhVien = tenSinhVien;
        NgaySinh = ngaySinh;
        GioiTinh = gioiTinh;
        Nganh = nganh;
        TrangThai = trangThai;  
        MaKhoaHoc = maKhoaHoc;
        MaLop = maLop;
        MaTk = maTk;
        SdtSinhVien = sdtSinhVien;
        QueQuanSinhVien = queQuanSinhVien;
        Email = email;
        CCCD = cccd;
        AnhDaiDienSinhVien = anhDaiDienSinhVien;
    }
}