using System.Globalization;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class FormatMoney
{
    public static string formatVN(double soTien)
    {
        return string.Format(new CultureInfo("vi-VN"), "{0:C0}", soTien);
    }
}