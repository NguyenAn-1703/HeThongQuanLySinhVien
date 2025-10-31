namespace QuanLySinhVien.Models;

public class CotDiemDto
{
    public int MaCD { get; set; }        // Khóa chính
    public int MaDQT { get; set; }       // Mã điểm quá trình (liên kết với bảng DiemQuaTrinh)
    public string TenCotDiem { get; set; } // Tên cột điểm (VD: "Cột điểm 1", "Giữa kỳ", ...)
    public float DiemSo { get; set; }    // Điểm số
    public float HeSo { get; set; }      // Hệ số của cột điểm
    public byte Status { get; set; } = 1; // Trạng thái (1 = còn dùng, 0 = xóa mềm)
}
