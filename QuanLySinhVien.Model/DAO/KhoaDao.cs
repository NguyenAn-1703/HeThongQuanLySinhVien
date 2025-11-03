using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class KhoaDao
{
    // thông tin database local

    // lấy danh sách khoa ( hàm dùng mỗi ln loadData )
    public List<KhoaDto> GetAll()
    {
        List<KhoaDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaKhoa, TenKhoa, Email, DiaChi FROM Khoa WHERE Status = 1",
            conn
        );
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new KhoaDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                TenKhoa = reader.GetString("TenKhoa"),
                Email = reader.GetString("Email"),
                DiaChi = reader.GetString("DiaChi")
            });

        return result;
    }


    // add Khoa
    public bool Insert(KhoaDto khoaDto)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();

        // status = 1, id tự tăng
        var query = @"INSERT INTO Khoa (TenKhoa, Email, DiaChi)
                     VALUES (@TenKhoa, @Email, @DiaChi);";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@TenKhoa", khoaDto.TenKhoa);
        cmd.Parameters.AddWithValue("@Email", khoaDto.Email);
        cmd.Parameters.AddWithValue("@DiaChi", khoaDto.DiaChi);

        rowAffected = cmd.ExecuteNonQuery();

        return rowAffected > 0;
    }


    // edit khoa -> get id = getById call form controller
    public bool Update(KhoaDto khoaDto)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();

        var query = @"UPDATE Khoa 
                     SET TenKhoa = @TenKhoa,
                         Email = @Email,
                         DiaChi = @DiaChi
                     WHERE MaKhoa = @MaKhoa;";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@TenKhoa", khoaDto.TenKhoa);
        cmd.Parameters.AddWithValue("@Email", khoaDto.Email);
        cmd.Parameters.AddWithValue("@DiaChi", khoaDto.DiaChi);
        cmd.Parameters.AddWithValue("@MaKhoa", khoaDto.MaKhoa);

        rowAffected = cmd.ExecuteNonQuery();

        return rowAffected > 0;
    }


    // delete khoa
    public bool Delete(int maKhoa)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE Khoa
                         SET Status = 0
                         WHERE MaKhoa = @MaKhoa;";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }


    // id -> data (1row)
    public KhoaDto GetKhoaById(int maKhoa)
    {
        KhoaDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaKhoa, TenKhoa, Email, DiaChi, Status FROM Khoa WHERE Status = 1 AND MaKhoa = @MaKhoa",
            conn);
        cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            result = new KhoaDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                TenKhoa = reader.GetString("TenKhoa"),
                Email = reader.GetString("Email"),
                DiaChi = reader.GetString("DiaChi")
            };

        return result;
    }

    public KhoaDto GetByTen(string tenKhoa)
    {
        KhoaDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaKhoa, TenKhoa, Email, DiaChi, Status FROM Khoa WHERE Status = 1 AND TenKhoa = @TenKhoa",
            conn);
        cmd.Parameters.AddWithValue("@TenKhoa", tenKhoa);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            result = new KhoaDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                TenKhoa = reader.GetString("TenKhoa"),
                Email = reader.GetString("Email"),
                DiaChi = reader.GetString("DiaChi")
            };

        return result;
    }

    public static void TestKhoaDAO()
    {
        var dao = new KhoaDao();

        Console.WriteLine("=== TEST KHOA DAO ===");

        // 🧩 1. Insert mới
        Console.WriteLine("\n--- INSERT ---");
        var newKhoaDto = new KhoaDto
        {
            TenKhoa = "Khoa CNTT",
            Email = "cntt@truong.edu.vn",
            DiaChi = "Tòa nhà A1"
        };

        var insertResult = dao.Insert(newKhoaDto);
        Console.WriteLine(insertResult ? "✅ Thêm mới thành công!" : "❌ Thêm mới thất bại!");

        // 🧩 2. Lấy danh sách tất cả Khoa
        Console.WriteLine("\n--- GET ALL ---");
        var allKhoa = dao.GetAll();
        foreach (var k in allKhoa)
            Console.WriteLine($"ID: {k.MaKhoa}, Tên: {k.TenKhoa}, Email: {k.Email}, Địa chỉ: {k.DiaChi}");

        // 🧩 3. Lấy 1 khoa theo ID (giả sử ID = 1)
        Console.WriteLine("\n--- GET BY ID (MaKhoa = 1) ---");
        var khoa = dao.GetKhoaById(1);
        if (khoa != null && khoa.MaKhoa != 0)
            Console.WriteLine($"Tên khoa: {khoa.TenKhoa}, Email: {khoa.Email}, Địa chỉ: {khoa.DiaChi}");
        else
            Console.WriteLine("❌ Không tìm thấy khoa có ID = 1");

        // 🧩 4. Update khoa (ví dụ ID = 1)
        Console.WriteLine("\n--- UPDATE (MaKhoa = 1) ---");
        khoa.TenKhoa = "Khoa Công nghệ Thông tin (Update)";
        khoa.Email = "update_cntt@truong.edu.vn";
        khoa.DiaChi = "Tòa nhà B2";
        var updateResult = dao.Update(khoa);
        Console.WriteLine(updateResult ? "✅ Cập nhật thành công!" : "❌ Cập nhật thất bại!");

        // 🧩 5. Xóa khoa (cập nhật Status = 0)
        Console.WriteLine("\n--- DELETE (MaKhoa = 1) ---");
        var deleteResult = dao.Delete(1);
        Console.WriteLine(deleteResult ? "✅ Xóa thành công!" : "❌ Xóa thất bại!");

        Console.WriteLine("\n=== KẾT THÚC TEST ===");
    }
}