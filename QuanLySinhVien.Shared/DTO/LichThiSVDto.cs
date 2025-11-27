namespace QuanLySinhVien.Shared.DTO;

public class LichThiSVDto
{
    public int STT { get; set; }
    public string MaMH { get; set; }
    public string TenMonHoc { get; set; }
    public int SiSo { get; set; } 
    public string NgayThi { get; set; }
    public string GioBatDau { get; set; }
    public string PhongThi { get; set; }
    public string CoSo { get; set; }
    
    // lọc
    public int HocKy { get; set; }
    public string Nam { get; set; }
    
    // sort
    public DateTime ThoiGianThuc { get; set; }
}