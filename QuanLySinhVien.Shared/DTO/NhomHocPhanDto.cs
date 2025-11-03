namespace QuanLySinhVien.Shared.DTO;

public class NhomHocPhanDto
{
    public int MaNHP { get; set; }
    public int MaGV { get; set; }
    public int MaHP { get; set; }
    public int MaLichDK { get; set; } = 1; //lịch mặc định khi chưa mở đăng ký học phần
    public int MaLop { get; set; }
    public int HocKy { get; set; }
    public string Nam { get; set; }
    public int SiSo { get; set; }
}