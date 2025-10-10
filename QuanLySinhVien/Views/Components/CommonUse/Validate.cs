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
}