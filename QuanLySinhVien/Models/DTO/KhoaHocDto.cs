namespace QuanLySinhVien.Models;

public class KhoaHocDto
{
    public int MaKhoaHoc { get; set; }
    public int MaCKDT { get; set; }
    public string TenKhoaHoc { get; set; }
    public string NienKhoaHoc { get; set; }
    
    public KhoaHocDto() { }
    
    public KhoaHocDto(int maKhoaHoc, int maCkdt, string tenKhoaHoc, string nienKhoaHoc)
    {
        MaKhoaHoc = maKhoaHoc;
        MaCKDT = maCkdt;
        TenKhoaHoc = tenKhoaHoc;
        NienKhoaHoc = nienKhoaHoc;
    }
}
