using System.Globalization;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class ConvertDate
{
    public static DateTime ConvertStringToDate(string input)
    {
        input = input.Trim();
        return DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }
}