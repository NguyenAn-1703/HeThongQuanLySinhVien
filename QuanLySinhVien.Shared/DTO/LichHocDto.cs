namespace QuanLySinhVien.Shared.DTO;

public class LichHocDto
{
    public int MaLH { get; set; }
    public int MaPH { get; set; }
    public int MaNHP { get; set; }
    public string Thu { get; set; }
    public int TietBatDau { get; set; }
    public DateTime TuNgay { get; set; }
    public DateTime DenNgay { get; set; }
    public int TietKetThuc { get; set; }
    public int SoTiet { get; set; }
    public string Type { get; set; }

    // Thông tin bổ sung từ JOIN
    public string TenPH { get; set; }
    public string TenHP { get; set; }
    public string TenGV { get; set; }
    public int SiSo { get; set; }

    // lấy thời gian -> hiển thị trên panel
    public string ThoiGianBatDau => GetThoiGian(TietBatDau);
    public string ThoiGianKetThuc => GetThoiGian(TietKetThuc + 1);

    private string GetThoiGian(int tiet)
    {
        // dựa trên tkb sgu 50p -> 1 tiết
        // Buổi sáng: Tiết 1-5 (7:00 - 11:30)
        // Buổi chiều: Tiết 6-10 (13:00 - 17:30)
        return tiet switch
        {
            1 => "07:00",
            2 => "07:50",
            3 => "09:00",
            4 => "09:50",
            5 => "10:40",
            6 => "13:00",
            7 => "13:50",
            8 => "15:00",
            9 => "15:50",
            10 => "16:40",
            _ => "N/A"
        };
    }
}