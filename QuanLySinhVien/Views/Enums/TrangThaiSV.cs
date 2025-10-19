namespace QuanLySinhVien.Views.Enums;

public enum TrangThaiSV
{
    DangHoc,
    TamNghi,
    TotNghiep,
    BiDuoiHoc
}

public class TrangThaiSVHelper
{
    public static string ToDbString(TrangThaiSV status)
    {
        return status switch
        {
            TrangThaiSV.DangHoc => "Đang học",
            TrangThaiSV.TamNghi => "Tạm nghỉ",
            TrangThaiSV.TotNghiep => "Tốt nghiệp",
            TrangThaiSV.BiDuoiHoc => "Bị đuổi học",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}