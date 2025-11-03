using System.Text.RegularExpressions;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class Validate
{
    public static bool IsEmpty(string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    // Kiểm tra độ dài tối thiểu
    public static bool HasMinLength(string input, int minLength)
    {
        return input.Length >= minLength;
    }

    // Kiểm tra độ dài tối đa
    public static bool HasMaxLength(string input, int maxLength)
    {
        return input.Length <= maxLength;
    }

    // Kiểm tra email
    public static bool IsValidEmail(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        // cú pháp chuẩn [Tên người dùng]@[Tên miền].[Miền cấp cao], một hoặc nhiều miền cc
        var pattern = @"^[\w]+@[\w]+(\.[\w]+)+$";

        return Regex.IsMatch(input, pattern);
    }


    // Kiểm tra số điện thoại
    public static bool IsValidPhoneNumber(string input)
    {
        // 0xxxxxxxxx   → 10 chữ số
        // +84xxxxxxxxx → 9 chữ số sau mã quốc gia

        if (string.IsNullOrEmpty(input)) return false;

        var pattern = @"^(0|\+84)\d{9}$";

        return Regex.IsMatch(input, pattern);
    }


    public static bool IsValidCCCD(string input)
    {
        // Mã tỉnh/thành phố hoặc mã quốc gia	3 số đầu	Ví dụ: 001 (Hà Nội), 079 (TP. HCM)
        // Mã giới tính + năm sinh	3 số tiếp theo	Ví dụ: 2xx (nam 2000–2099), 3xx (nữ 2000–2099)
        // Số ngẫu nhiên	6 số cuối	Dãy số định danh duy nhất
        if (string.IsNullOrEmpty(input)) return false;

        var pattern = @"^\d{3}[2-3]\d{8}$";

        return Regex.IsMatch(input, pattern);
    }

    // Kiểm tra số 
    public static bool IsNumeric(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        return double.TryParse(input, out _);
    }

    public static bool IsPositiveInt(string input)
    {
        return int.TryParse(input, out var x) && x >= 0;
    }

    public static bool IsYear(string input)
    {
        if (!IsPositiveInt(input))
            return false;
        var year = int.Parse(input);

        if (year > 1900 && year < 2999) return true;

        return false;
    }

    public static bool IsStartYearAndEndYear(string input1, string input2)
    {
        Console.WriteLine(input1 + " " + input2);

        var year1 = int.Parse(input1);
        var year2 = int.Parse(input2);

        if (year1 > year2) return false;
        return true;
    }

    public static bool IsAcademicYear(string input)
    {
        var x = input.Split("-");
        if (x.Length != 2) return false;

        if (!IsYear(x[0]) || !IsYear(x[1])) return false;

        if (!IsStartYearAndEndYear(x[0], x[1])) return false;

        return true;
    }

    public static bool IsValidDiem(string text)
    {
        if (double.TryParse(text, out var diem)) return diem >= 0 && diem <= 10;
        return false;
    }
}