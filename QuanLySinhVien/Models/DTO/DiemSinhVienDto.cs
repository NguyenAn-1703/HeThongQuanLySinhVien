namespace QuanLySinhVien.Models;

public class DiemSinhVienDto
{
    public int MaKetQua { get; set; }
    public int MaSinhVien { get; set; }
    public int MaHocPhan { get; set; }
    public string TenHocPhan { get; set; }
    public int SoTinChi { get; set; }
    public float DiemQuaTrinh { get; set; }
    public float DiemThi { get; set; }
    public float DiemHe10 { get; set; }
    public float DiemHe4 { get; set; }
    public int HocKy { get; set; }
    public string Nam { get; set; }
    public string KetQua { get; set; } 
    
    public DiemSinhVienDto() { }
    
    public DiemSinhVienDto(int maKetQua, string tenHocPhan, int soTinChi, float diemHe10, float diemHe4, string ketQua)
    {
        MaKetQua = maKetQua;
        TenHocPhan = tenHocPhan;
        SoTinChi = soTinChi;
        DiemHe10 = diemHe10;
        DiemHe4 = diemHe4;
        KetQua = ketQua;
    }
}