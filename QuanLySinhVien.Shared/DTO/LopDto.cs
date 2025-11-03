namespace QuanLySinhVien.Shared.DTO;

public class LopDto
{
    public LopDto()
    {
    }

    public LopDto(int maLop, int maGv, int maNganh, string tenLop, int soLuongSv)
    {
        MaLop = maLop;
        MaGV = maGv;
        MaNganh = maNganh;
        TenLop = tenLop;
        SoLuongSV = soLuongSv;
        SoSinhVienHienTai = 0; // Sẽ được tính toán từ database
    }

    public int MaLop { get; set; }
    public int MaGV { get; set; }
    public int MaNganh { get; set; }
    public string TenLop { get; set; }
    public int SoLuongSV { get; set; }
    public int SoSinhVienHienTai { get; set; }

    // Computed property để hiển thị thông tin lớp
    public string DisplayText => $"{TenLop} ({SoSinhVienHienTai}/{SoLuongSV} SV)";
}