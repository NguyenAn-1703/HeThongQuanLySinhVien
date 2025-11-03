namespace QuanLySinhVien.Shared.DTO;

public class KetQuaDto
{
    public int MaKQ { get; set; }
    public int MaHP { get; set; }
    public int MaSV { get; set; }
    public float DiemThi { get; set; } = 0;
    public float DiemHe4 { get; set; } = 0;
    public float DiemHe10 { get; set; } = 0;
    public int HocKy { get; set; }
    public string Nam { get; set; }
    public byte Status { get; set; } = 1;
}