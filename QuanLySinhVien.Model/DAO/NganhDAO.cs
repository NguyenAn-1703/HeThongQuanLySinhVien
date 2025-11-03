using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class NganhDao
{
    // thông tin database local

    // lấy danh sách khoa ( hàm dùng mỗi ln loadData )

    private static NganhDao _instance;

    private NganhDao()
    {
    }

    public static NganhDao GetInstance()
    {
        if (_instance == null) _instance = new NganhDao();

        return _instance;
    }


    public List<NganhDto> GetAll()
    {
        List<NganhDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaNganh, MaKhoa, TenNganh FROM nganh WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new NganhDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh")
            });

        return result;
    }

    public bool Insert(NganhDto nganhDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            // status = 1, id auto +1
            var query = @"INSERT INTO nganh (MaKhoa, TenNganh)
                                 VALUES (@MaKhoa, @TenNganh)";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhoa", nganhDto.MaKhoa);
                cmd.Parameters.AddWithValue("@TenNganh", nganhDto.TenNganh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // edit khoa -> get id = getById call form controller
    public bool Update(NganhDto nganhDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE nganh 
                                 SET MaKhoa = @MaKhoa,
                                     TenNganh = @TenNganh
                                 WHERE MaNganh = @MaNganh";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhoa", nganhDto.MaKhoa);
                cmd.Parameters.AddWithValue("@TenNganh", nganhDto.TenNganh);
                cmd.Parameters.AddWithValue("@MaNganh", nganhDto.MaNganh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // delete khoa
    public bool Delete(int maNganh)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            // update status = 0 
            var query = @"UPDATE nganh
                               SET Status = 0
                               WHERE MaNganh = @MaNganh;";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNganh", maNganh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public NganhDto GetNganhById(int maNganh)
    {
        NganhDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand("SELECT MaNganh, MaKhoa, TenNganh FROM nganh WHERE MaNganh = @MaNganh",
                conn);
        cmd.Parameters.AddWithValue("@MaNganh", maNganh);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new NganhDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh")
            };

        return result;
    }

    public NganhDto GetByTen(string tenNganh)
    {
        NganhDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand("SELECT MaNganh, MaKhoa, TenNganh FROM nganh WHERE TenNganh = @TenNganh",
                conn);
        cmd.Parameters.AddWithValue("@TenNganh", tenNganh);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new NganhDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh")
            };

        return result;
    }

    public static void TestNganhDAO()
    {
        try
        {
            var dao = new NganhDao();

            Console.WriteLine("===== BẮT ĐẦU TEST NGANHDAO =====");

            // 1️⃣ Test Insert
            var newNganh = new NganhDto
            {
                MaKhoa = 1, // giả sử khoa có ID = 1
                TenNganh = "Ngành Thử Nghiệm"
            };

            var insertResult = dao.Insert(newNganh);
            Console.WriteLine($"[INSERT] Kết quả: {(insertResult ? "Thành công" : "Thất bại")}");

            // 2️⃣ Test GetAll
            var allNganh = dao.GetAll();
            Console.WriteLine("[GET ALL] Danh sách ngành (Status=1):");
            foreach (var nganh in allNganh)
                Console.WriteLine($" - MaNganh={nganh.MaNganh}, MaKhoa={nganh.MaKhoa}, TenNganh={nganh.TenNganh}");

            // 3️⃣ Test GetById (lấy phần tử mới nhất)
            var lastNganh = allNganh[^1]; // phần tử cuối danh sách
            var getByIdResult = dao.GetNganhById(lastNganh.MaNganh);
            Console.WriteLine($"[GET BY ID] MaNganh={getByIdResult.MaNganh}, TenNganh={getByIdResult.TenNganh}");

            // 4️⃣ Test Update
            getByIdResult.TenNganh = "Ngành Sau Khi Cập Nhật";
            var updateResult = dao.Update(getByIdResult);
            Console.WriteLine($"[UPDATE] Kết quả: {(updateResult ? "Thành công" : "Thất bại")}");

            // 5️⃣ Test Delete
            var deleteResult = dao.Delete(getByIdResult.MaNganh);
            Console.WriteLine($"[DELETE] Kết quả: {(deleteResult ? "Thành công" : "Thất bại")}");

            Console.WriteLine("===== KẾT THÚC TEST NGANHDAO =====");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi trong quá trình test: {ex.Message}");
        }
    }

    public int CountNganhByStatus(int status)
    {
        using var conn = MyConnection.GetConnection();
        const string sql = "SELECT COUNT(*) AS Total FROM Nganh WHERE Status = @Status";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Status", status);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}