namespace QuanLySinhVien.Shared;

public class ConvertThu
{
    public static DayOfWeek ConvertToDayOfWeek(string thu)
    {
        return thu switch
        {
            "Thứ 2" => DayOfWeek.Monday,
            "Thứ 3" => DayOfWeek.Tuesday,
            "Thứ 4" => DayOfWeek.Wednesday,
            "Thứ 5" => DayOfWeek.Thursday,
            "Thứ 6" => DayOfWeek.Friday,
            "Thứ 7" => DayOfWeek.Saturday,
            _ => throw new Exception("Thứ không hợp lệ")
        };
    }

}