namespace QuanLySinhVien.View.utils;

public static class PasswordHasher
{
    // Băm mật khẩu để lưu vào DB
    public static string HashPassword(string password)
    {
        // Tự động sinh salt và băm (mặc định cost = 10)
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Kiểm tra mật khẩu nhập vào có khớp không
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}