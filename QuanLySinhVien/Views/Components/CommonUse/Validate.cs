using System.Text.RegularExpressions;

namespace QuanLySinhVien.Views.Components.CommonUse;

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
        return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    // Kiểm tra số điện thoại
    public static bool IsValidPhoneNumber(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        return Regex.IsMatch(input, @"^(0|\+84)(\d{9})$");
    }

    // Kiểm tra số 
    public static bool IsNumeric(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        return double.TryParse(input, out _);
    }

    public static bool IsPositiveInt(string input)
    {
        return Int32.TryParse(input, out int x) && x >= 0;
    }

    public static bool IsYear(string input)
    {
        if (!IsPositiveInt(input))
            return false;
        int year = Int32.Parse(input);
        
        if (year > 1900 && year < 2999)
        {
            return true;
        }

        return false;
    }
    
    public static bool IsStartYearAndEndYear(string input1, string input2)
    {
        Console.WriteLine(input1 + " " + input2);

        int year1 =  Int32.Parse(input1);
        int year2 = Int32.Parse(input2);

        if (year1 > year2) return false;
        return true;
    }
}