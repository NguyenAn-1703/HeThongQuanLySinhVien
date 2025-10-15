namespace QuanLySinhVien.Views.Enums;

public enum TrangThaiGV
{
    DangCongTac,
    DangNghiPhep
}

public class TrangThaiGVHelper
{
    public static string ToDbString(TrangThaiGV status)
    {
        return status switch
        {
            TrangThaiGV.DangCongTac => "Đang công tác",
            TrangThaiGV.DangNghiPhep => "Đang nghỉ phép",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}