using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

public class TaiKhoanDao
{
    private static TaiKhoanDao _instance;
    private TaiKhoanDao(){}

    public static TaiKhoanDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new TaiKhoanDao();
        }
        return _instance;
    }
    
    public List<TaiKhoanDto> GetAll()
    {
        List<TaiKhoanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaTK, MaNQ, TenDangNhap, MatKhau FROM taikhoan WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new TaiKhoanDto
            {
                MaTK = reader.GetInt32("MaTK"),
                MaNQ = reader.GetInt32("MaNQ"),
                TenDangNhap = reader.GetString("TenDangNhap"),
                MatKhau = reader.GetString("MatKhau"),
            });
        }

        return result;
    }

    public bool Insert(TaiKhoanDto taiKhoanDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            // status = 1, id auto +1
            string query = @"INSERT INTO taikhoan (MaTK, MaNQ, TenDangNhap, MatKhau)
                                 VALUES (@MaTK, @MaNQ, @TenDangNhap, @MatKhau)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaTK", taiKhoanDto.MaTK);
                cmd.Parameters.AddWithValue("@MaNQ", taiKhoanDto.MaNQ);
                cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoanDto.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", taiKhoanDto.MatKhau);
                
                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 1;
    }

    public bool Update(TaiKhoanDto taiKhoanDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE taikhoan 
                                 SET MaNQ = @MaNQ,
                                     TenDangNhap = @TenDangNhap,
                                     MatKhau = @MatKhau
                                 WHERE MaTK = @MaTK";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaTK", taiKhoanDto.MaTK);
                cmd.Parameters.AddWithValue("@MaNQ", taiKhoanDto.MaNQ);
                cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoanDto.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", taiKhoanDto.MatKhau);
                
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maTK)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE taikhoan
                               SET Status = 0
                               WHERE MaTK = @MaTK;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaTK", maTK);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public TaiKhoanDto GetTaiKhoanById(int maTk)
    {
        TaiKhoanDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand("SELECT MaTK, MaNQ, TenDangNhap, MatKhau FROM taikhoan WHERE Status = 1 AND MaTK = @MaTK",
                conn);
        cmd.Parameters.AddWithValue("@MaTK", maTk);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new TaiKhoanDto
            {
                MaTK = reader.GetInt32("MaTK"),
                MaNQ = reader.GetInt32("MaNQ"),
                TenDangNhap = reader.GetString("TenDangNhap"),
                MatKhau = reader.GetString("MatKhau"),
            };
        }

        return result;
    }
    
    // ⚡⚡⚡ HÀM STATIC TEST TOÀN BỘ CRUD ⚡⚡⚡
    public static void Test()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== TEST TAIKHOAN DAO ===\n");

        var dao = new TaiKhoanDao();

        // 1️⃣ Insert
        Console.WriteLine("➡ Thêm tài khoản mới...");
        var newAcc = new TaiKhoanDto
        {
            MaNQ = 1,
            TenDangNhap = "user_test",
            MatKhau = "123456"
        };
        bool insertOk = dao.Insert(newAcc);
        Console.WriteLine($"Insert: {(insertOk ? "✅ Thành công" : "❌ Thất bại")}");

        // 2️⃣ GetAll
        Console.WriteLine("\n➡ Danh sách tài khoản:");
        var list = dao.GetAll();
        foreach (var tk in list)
        {
            Console.WriteLine($"- {tk.MaTK}: {tk.TenDangNhap} (Mật khẩu: {tk.MatKhau})");
        }

        // 3️⃣ GetTaiKhoanById
        Console.WriteLine("\n➡ Lấy tài khoản có MaTK = 1");
        var tk1 = dao.GetTaiKhoanById(1);
        if (tk1 != null && tk1.MaTK != 0)
            Console.WriteLine($"✅ Tìm thấy: {tk1.TenDangNhap} - Mật khẩu: {tk1.MatKhau}");
        else
            Console.WriteLine("❌ Không tìm thấy tài khoản MaTK = 1");

        // 4️⃣ Update
        Console.WriteLine("\n➡ Cập nhật tài khoản có MaTK = 1...");
        tk1.MatKhau = "new_password";
        bool updateOk = dao.Update(tk1);
        Console.WriteLine($"Update: {(updateOk ? "✅ Thành công" : "❌ Thất bại")}");

        // 5️⃣ Delete (soft delete)
        Console.WriteLine("\n➡ Xóa tài khoản có MaTK = 1...");
        bool deleteOk = dao.Delete(1);
        Console.WriteLine($"Delete: {(deleteOk ? "✅ Thành công" : "❌ Thất bại")}");

        Console.WriteLine("\n=== KẾT THÚC TEST ===");
    }
}