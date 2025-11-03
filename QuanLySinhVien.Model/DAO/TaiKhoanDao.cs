using System.Text;
using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class TaiKhoanDao
{
    private static TaiKhoanDao _instance;

    private TaiKhoanDao()
    {
    }

    public static TaiKhoanDao GetInstance()
    {
        if (_instance == null) _instance = new TaiKhoanDao();
        return _instance;
    }

    public List<TaiKhoanDto> GetAll()
    {
        List<TaiKhoanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaTK, MaNQ, TenDangNhap, MatKhau, Type FROM taikhoan WHERE Status = 1",
            conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new TaiKhoanDto
            {
                MaTK = reader.GetInt32("MaTK"),
                MaNQ = reader.GetInt32("MaNQ"),
                TenDangNhap = reader.GetString("TenDangNhap"),
                MatKhau = reader.GetString("MatKhau"),
                Type = reader.GetString("Type")
            });

        return result;
    }

    public bool Insert(TaiKhoanDto taiKhoanDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            // status = 1, id auto +1
            var query = @"INSERT INTO taikhoan (MaTK, MaNQ, TenDangNhap, MatKhau, Type)
                                 VALUES (@MaTK, @MaNQ, @TenDangNhap, @MatKhau, @Type)";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaTK", taiKhoanDto.MaTK);
                cmd.Parameters.AddWithValue("@MaNQ", taiKhoanDto.MaNQ);
                cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoanDto.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", taiKhoanDto.MatKhau);
                cmd.Parameters.AddWithValue("@Type", taiKhoanDto.Type);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(TaiKhoanDto taiKhoanDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE taikhoan 
                                 SET MaNQ = @MaNQ,
                                     TenDangNhap = @TenDangNhap,
                                     MatKhau = @MatKhau,
                                     Type = @Type
                                 WHERE MaTK = @MaTK";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaTK", taiKhoanDto.MaTK);
                cmd.Parameters.AddWithValue("@MaNQ", taiKhoanDto.MaNQ);
                cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoanDto.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", taiKhoanDto.MatKhau);
                cmd.Parameters.AddWithValue("@Type", taiKhoanDto.Type);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maTK)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE taikhoan
                               SET Status = 0
                               WHERE MaTK = @MaTK;";
            using (var cmd = new MySqlCommand(query, conn))
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
            new MySqlCommand(
                "SELECT MaTK, MaNQ, TenDangNhap, MatKhau, Type FROM taikhoan WHERE Status = 1 AND MaTK = @MaTK",
                conn);
        cmd.Parameters.AddWithValue("@MaTK", maTk);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new TaiKhoanDto
            {
                MaTK = reader.GetInt32("MaTK"),
                MaNQ = reader.GetInt32("MaNQ"),
                TenDangNhap = reader.GetString("TenDangNhap"),
                MatKhau = reader.GetString("MatKhau"),
                Type = reader.GetString("Type")
            };

        return result;
    }

    public TaiKhoanDto? GetTaiKhoanByUsrName(string usrName)
    {
        TaiKhoanDto? result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand(
                "SELECT MaTK, MaNQ, TenDangNhap, MatKhau, Type FROM taikhoan WHERE Status = 1 AND TenDangNhap = @UsrName",
                conn);
        cmd.Parameters.AddWithValue("@UsrName", usrName);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new TaiKhoanDto
            {
                MaTK = reader.GetInt32("MaTK"),
                MaNQ = reader.GetInt32("MaNQ"),
                TenDangNhap = reader.GetString("TenDangNhap"),
                MatKhau = reader.GetString("MatKhau"),
                Type = reader.GetString("Type")
            };

        return result;
    }

    public List<TaiKhoanDto> GetTaiKhoanNotUsed()
    {
        List<TaiKhoanDto> result = new();

        using var conn = MyConnection.GetConnection();
        var query = @"
        SELECT tk.MaTK, tk.MaNQ, tk.TenDangNhap, tk.MatKhau, tk.Type
        FROM TaiKhoan tk
        LEFT JOIN SinhVien sv ON tk.MaTK = sv.MaTK AND sv.Status = 1
        LEFT JOIN GiangVien gv ON tk.MaTK = gv.MaTK AND gv.Status = 1
        WHERE tk.Status = 1
          AND sv.MaSV IS NULL
          AND gv.MaGV IS NULL;
    ";

        using var cmd = new MySqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new TaiKhoanDto
            {
                MaTK = reader.GetInt32("MaTK"),
                MaNQ = reader.GetInt32("MaNQ"),
                TenDangNhap = reader.GetString("TenDangNhap"),
                MatKhau = reader.GetString("MatKhau"),
                Type = reader.GetString("Type")
            });

        return result;
    }


    // ⚡⚡⚡ HÀM STATIC TEST TOÀN BỘ CRUD ⚡⚡⚡
    public static void Test()
    {
        Console.OutputEncoding = Encoding.UTF8;
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
        var insertOk = dao.Insert(newAcc);
        Console.WriteLine($"Insert: {(insertOk ? "✅ Thành công" : "❌ Thất bại")}");

        // 2️⃣ GetAll
        Console.WriteLine("\n➡ Danh sách tài khoản:");
        var list = dao.GetAll();
        foreach (var tk in list) Console.WriteLine($"- {tk.MaTK}: {tk.TenDangNhap} (Mật khẩu: {tk.MatKhau})");

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
        var updateOk = dao.Update(tk1);
        Console.WriteLine($"Update: {(updateOk ? "✅ Thành công" : "❌ Thất bại")}");

        // 5️⃣ Delete (soft delete)
        Console.WriteLine("\n➡ Xóa tài khoản có MaTK = 1...");
        var deleteOk = dao.Delete(1);
        Console.WriteLine($"Delete: {(deleteOk ? "✅ Thành công" : "❌ Thất bại")}");

        Console.WriteLine("\n=== KẾT THÚC TEST ===");
    }
}