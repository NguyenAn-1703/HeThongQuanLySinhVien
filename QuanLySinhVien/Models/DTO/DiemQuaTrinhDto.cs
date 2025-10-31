namespace QuanLySinhVien.Models;

public class DiemQuaTrinhDto
{
    public int MaDQT { get; set; }      // Khóa chính
    public int MaKQ { get; set; }       // Mã kết quả (liên kết bảng KetQua)
    public float DiemSo { get; set; }   // Tổng điểm quá trình
    public byte Status { get; set; } = 1; // 1 = còn hiệu lực, 0 = xóa mềm
}
