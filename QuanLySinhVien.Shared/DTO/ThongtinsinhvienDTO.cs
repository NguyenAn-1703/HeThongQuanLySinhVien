namespace QuanLySinhVien.Shared.DTO;

public class ThongTinSinhVienDto
{
    public ThongTinSinhVienDto()
    {
    }

    public ThongTinSinhVienDto(int maSinhVien, string tenSinhVien, string ngaySinh, string gioiTinh,
        string trangThai, string nganh, string lop, string khoa)
    {
        MaSinhVien = maSinhVien;
        TenSinhVien = tenSinhVien;
        NgaySinh = ngaySinh;
        GioiTinh = gioiTinh;
        TrangThai = trangThai;
        Nganh = nganh;
        Lop = lop;
        Khoa = khoa;
    }

    public int MaSinhVien { get; set; }
    public string TenSinhVien { get; set; }
    public string NgaySinh { get; set; }
    public string GioiTinh { get; set; }
    public string TrangThai { get; set; }
    public string SdtSinhVien { get; set; }
    public string QueQuanSinhVien { get; set; }
    public string Email { get; set; }
    public string Cccd { get; set; }
    public string AnhDaiDienSinhVien { get; set; }

    public string Nganh { get; set; }
    public string Lop { get; set; }
    public string Khoa { get; set; }
    public string BacDaoTao { get; set; }
    public string NienKhoa { get; set; }

    public string TenCoVanHocTap { get; set; }
    public string SdtCoVanHocTap { get; set; }
    public string EmailCoVanHocTap { get; set; }
    public string TaiKhoanCoVanHocTap { get; set; }
}