using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class HocPhanDao
{
    private static HocPhanDao _instance;

    private HocPhanDao()
    {
    }

    public static HocPhanDao GetInstance()
    {
        if (_instance == null) _instance = new HocPhanDao();
        return _instance;
    }


    // lấy danh sách học phần ( hàm dùng mỗi ln loadData )
    public List<HocPhanDto> GetAll()
    {
        List<HocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand(
                "SELECT MaHP, MaHPTruoc, TenHP, SoTinChi, HeSoHocPhan, SoTietLyThuyet, SoTietThucHanh FROM hocphan WHERE Status = 1",
                conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new HocPhanDto
            {
                MaHP = reader.GetInt32(reader.GetOrdinal("MaHP")),
                MaHPTruoc = reader.IsDBNull(reader.GetOrdinal("MaHPTruoc"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("MaHPTruoc")),
                TenHP = reader.GetString(reader.GetOrdinal("TenHP")),
                SoTinChi = reader.GetInt32(reader.GetOrdinal("SoTinChi")),
                HeSoHocPhan = reader.GetString(reader.GetOrdinal("HeSoHocPhan")),
                SoTietLyThuyet = reader.GetInt32(reader.GetOrdinal("SoTietLyThuyet")),
                SoTietThucHanh = reader.GetInt32(reader.GetOrdinal("SoTietThucHanh"))
            });

        return result;
    }

    public bool Insert(HocPhanDto hocPhanDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            // status = 1, id auto +1
            var query = @"INSERT INTO hocphan (MaHPTruoc, TenHP, SoTinChi, HeSoHocPhan, SoTietLyThuyet, SoTietThucHanh)
                                 VALUES (@MaHPTruoc, @TenHP, @SoTinChi, @HeSoHocPhan, @SoTietLyThuyet, @SoTietThucHanh)";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaHPTruoc", hocPhanDto.MaHPTruoc ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TenHP", hocPhanDto.TenHP);
                cmd.Parameters.AddWithValue("@SoTinChi", hocPhanDto.SoTinChi);
                cmd.Parameters.AddWithValue("@HeSoHocPhan", hocPhanDto.HeSoHocPhan);
                cmd.Parameters.AddWithValue("@SoTietLyThuyet", hocPhanDto.SoTietLyThuyet);
                cmd.Parameters.AddWithValue("@SoTietThucHanh", hocPhanDto.SoTietThucHanh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // edit học phần -> get id = getById call form controller
    public bool Update(HocPhanDto hocPhanDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE hocphan 
                                 SET MaHPTruoc = @MaHPTruoc,
                                     TenHP = @TenHP,
                                     SoTinChi = @SoTinChi,
                                     HeSoHocPhan = @HeSoHocPhan,
                                     SoTietLyThuyet = @SoTietLyThuyet,
                                     SoTietThucHanh = @SoTietThucHanh
                                 WHERE MaHP = @MaHP";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaHPTruoc", hocPhanDto.MaHPTruoc ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TenHP", hocPhanDto.TenHP);
                cmd.Parameters.AddWithValue("@SoTinChi", hocPhanDto.SoTinChi);
                cmd.Parameters.AddWithValue("@HeSoHocPhan", hocPhanDto.HeSoHocPhan);
                cmd.Parameters.AddWithValue("@SoTietLyThuyet", hocPhanDto.SoTietLyThuyet);
                cmd.Parameters.AddWithValue("@SoTietThucHanh", hocPhanDto.SoTietThucHanh);
                cmd.Parameters.AddWithValue("@MaHP", hocPhanDto.MaHP);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // delete học phần
    public bool Delete(int maHP)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            // update status = 0 
            var query = @"UPDATE hocphan
                               SET Status = 0
                               WHERE MaHP = @MaHP;";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaHP", maHP);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // id -> data (1row)
    public HocPhanDto GetHocPhanById(int maHP)
    {
        HocPhanDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand(
                "SELECT MaHP, MaHPTruoc, TenHP, SoTinChi, HeSoHocPhan, SoTietLyThuyet, SoTietThucHanh FROM hocphan WHERE Status = 1 AND MaHP = @MaHP",
                conn);
        cmd.Parameters.AddWithValue("@MaHP", maHP);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new HocPhanDto
            {
                MaHP = reader.GetInt32(reader.GetOrdinal("MaHP")),
                MaHPTruoc = reader.IsDBNull(reader.GetOrdinal("MaHPTruoc"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("MaHPTruoc")),
                TenHP = reader.GetString(reader.GetOrdinal("TenHP")),
                SoTinChi = reader.GetInt32(reader.GetOrdinal("SoTinChi")),
                HeSoHocPhan = reader.GetString(reader.GetOrdinal("HeSoHocPhan")),
                SoTietLyThuyet = reader.GetInt32(reader.GetOrdinal("SoTietLyThuyet")),
                SoTietThucHanh = reader.GetInt32(reader.GetOrdinal("SoTietThucHanh"))
            };

        return result;
    }

    public HocPhanDto GetHocPhanByTen(string tenHP)
    {
        HocPhanDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand(
                "SELECT MaHP, MaHPTruoc, TenHP, SoTinChi, HeSoHocPhan, SoTietLyThuyet, SoTietThucHanh FROM hocphan WHERE Status = 1 AND TenHP = @TenHP",
                conn);
        cmd.Parameters.AddWithValue("@TenHP", tenHP);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new HocPhanDto
            {
                MaHP = reader.GetInt32(reader.GetOrdinal("MaHP")),
                MaHPTruoc = reader.IsDBNull(reader.GetOrdinal("MaHPTruoc"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("MaHPTruoc")),
                TenHP = reader.GetString(reader.GetOrdinal("TenHP")),
                SoTinChi = reader.GetInt32(reader.GetOrdinal("SoTinChi")),
                HeSoHocPhan = reader.GetString(reader.GetOrdinal("HeSoHocPhan")),
                SoTietLyThuyet = reader.GetInt32(reader.GetOrdinal("SoTietLyThuyet")),
                SoTietThucHanh = reader.GetInt32(reader.GetOrdinal("SoTietThucHanh"))
            };

        return result;
    }

    // Get all HocPhan for ComboBox display (format: "MaHP - TenHP")
    public List<string> GetAllForCombobox()
    {
        List<string> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaHP, TenHP FROM hocphan WHERE Status = 1 ORDER BY MaHP", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read()) result.Add($"{reader.GetInt32("MaHP")} - {reader.GetString("TenHP")}");

        return result;
    }

    public static void TestHocPhanDAO()
    {
        try
        {
            var dao = new HocPhanDao();

            Console.WriteLine("===== BẮT ĐẦU TEST HOCPHANDAO =====");

            // 1️⃣ Test Insert
            var newHocPhan = new HocPhanDto
            {
                MaHPTruoc = null,
                TenHP = "Học phần Thử Nghiệm",
                SoTinChi = 3,
                HeSoHocPhan = "5:5",
                SoTietLyThuyet = 30,
                SoTietThucHanh = 15
            };

            var insertResult = dao.Insert(newHocPhan);
            Console.WriteLine($"[INSERT] Kết quả: {(insertResult ? "Thành công" : "Thất bại")}");

            // 2️⃣ Test GetAll
            var allHocPhan = dao.GetAll();
            Console.WriteLine("[GET ALL] Danh sách học phần (Status=1):");
            foreach (var hp in allHocPhan)
                Console.WriteLine($" - MaHP={hp.MaHP}, TenHP={hp.TenHP}, SoTinChi={hp.SoTinChi}");

            // 3️⃣ Test GetById (lấy phần tử mới nhất)
            var lastHocPhan = allHocPhan[^1]; // phần tử cuối danh sách
            var getByIdResult = dao.GetHocPhanById(lastHocPhan.MaHP);
            Console.WriteLine($"[GET BY ID] MaHP={getByIdResult.MaHP}, TenHP={getByIdResult.TenHP}");

            // 4️⃣ Test Update
            getByIdResult.TenHP = "Học phần Sau Khi Cập Nhật";
            var updateResult = dao.Update(getByIdResult);
            Console.WriteLine($"[UPDATE] Kết quả: {(updateResult ? "Thành công" : "Thất bại")}");

            // 5️⃣ Test Delete
            var deleteResult = dao.Delete(getByIdResult.MaHP);
            Console.WriteLine($"[DELETE] Kết quả: {(deleteResult ? "Thành công" : "Thất bại")}");

            Console.WriteLine("===== KẾT THÚC TEST HOCPHANDAO =====");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi trong quá trình test: {ex.Message}");
        }
    }
}