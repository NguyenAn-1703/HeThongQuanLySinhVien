using System.Globalization;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class ConvertDate
{
    public static DateTime ConvertStringToDate(string input)
    {
        input = input.Trim();
        return DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }
    
    public static DateTime ConvertStringToDateTime(string input)
    {
        input = input.Trim();
        return DateTime.ParseExact(input, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }
    
    public static DateTime ConvertStringToTime(string input)
    {
        input = input.Trim();
        return DateTime.ParseExact(input, "HH:mm", CultureInfo.InvariantCulture);
    }

    public static int GetThuByDateTime(DateTime dt)
    {
        int thu = (int)dt.DayOfWeek;
        return thu + 1;
    }
}