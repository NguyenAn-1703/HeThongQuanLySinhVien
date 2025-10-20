namespace QuanLySinhVien.utils;

using BCrypt.Net;

public static class PasswordHasher
{
    // Băm mật khẩu để lưu vào DB
    public static string HashPassword(string password)
    {
        // Tự động sinh salt và băm (mặc định cost = 10)
        return BCrypt.HashPassword(password);
    }

    // Kiểm tra mật khẩu nhập vào có khớp không
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Verify(password, hashedPassword);
    }
}
