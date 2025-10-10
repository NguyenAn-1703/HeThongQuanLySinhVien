// using QuanLySinhVien.DB;
//
// namespace QuanLySinhVien.DAO;
//
// public class NganhDAO
// {
//     private AppDbContext db = new AppDbContext();
//
//     public List<Models.Nganh> getAllNganh()
//     {
//         return db.nganh.ToList();
//     }
//
//     public void addNganh(Models.Nganh nganh)
//     {
//         db.nganh.Add(nganh);
//         db.SaveChanges();
//     }
//
//     public void deleteNganh(int maNganh)
//     {
//         var nganh = db.nganh.Find(maNganh);
//         if (nganh != null)
//         {
//             db.nganh.Remove(nganh);
//             db.SaveChanges();
//         }
//     }
//
//     public void updateNganh(Models.Nganh nganh)
//     {
//         Console.WriteLine(nganh.MaNganh);
//         var existingNganh = db.nganh.Find(nganh.MaNganh);
//         if (existingNganh != null)
//         {
//             existingNganh.TenNganh = nganh.TenNganh;
//             existingNganh.MaKhoa = nganh.MaKhoa;
//             db.SaveChanges();
//         }
//         else
//         {
//             Console.Write("Nganh not found");
//         }
//     }
// }

using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

public class NganhDao
{
    // thông tin database local

    // lấy danh sách khoa ( hàm dùng mỗi ln loadData )
    public List<NganhDto> GetAll()
    {
        List<NganhDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaNganh, MaKhoa, TenNganh FROM nganh WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new NganhDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh")
            });
        }

        return result;
    }

    public bool Insert(NganhDto nganhDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            // status = 1, id auto +1
            string query = @"INSERT INTO nganh (MaKhoa, TenNganh)
                                 VALUES (@MaKhoa, @TenNganh)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhoa", nganhDto.MaKhoa);
                cmd.Parameters.AddWithValue("@TenNganh", nganhDto.TenNganh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }
        return rowAffected > 1;
    }

    // edit khoa -> get id = getById call form controller
    public bool Update(NganhDto nganhDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE nganh 
                                 SET MaKhoa = @MaKhoa,
                                     TenNganh = @TenNganh
                                 WHERE MaNganh = @MaNganh";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
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
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            // update status = 0 
            string query = @"UPDATE nganh
                               SET Status = 0
                               WHERE MaNganh = @MaNganh;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNganh", maNganh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // id -> data (1row)
    public NganhDto GetNganhById(int maNganh)
    {
        NganhDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand("SELECT MaNganh, MaKhoa, TenNganh FROM nganh WHERE Status = 1 AND MaNganh = @MaNganh",
                conn);
        cmd.Parameters.AddWithValue("@MaNganh", maNganh);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new NganhDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh")
            };
        }

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

            bool insertResult = dao.Insert(newNganh);
            Console.WriteLine($"[INSERT] Kết quả: {(insertResult ? "Thành công" : "Thất bại")}");

            // 2️⃣ Test GetAll
            var allNganh = dao.GetAll();
            Console.WriteLine("[GET ALL] Danh sách ngành (Status=1):");
            foreach (var nganh in allNganh)
            {
                Console.WriteLine($" - MaNganh={nganh.MaNganh}, MaKhoa={nganh.MaKhoa}, TenNganh={nganh.TenNganh}");
            }

            // 3️⃣ Test GetById (lấy phần tử mới nhất)
            var lastNganh = allNganh[^1]; // phần tử cuối danh sách
            var getByIdResult = dao.GetNganhById(lastNganh.MaNganh);
            Console.WriteLine($"[GET BY ID] MaNganh={getByIdResult.MaNganh}, TenNganh={getByIdResult.TenNganh}");

            // 4️⃣ Test Update
            getByIdResult.TenNganh = "Ngành Sau Khi Cập Nhật";
            bool updateResult = dao.Update(getByIdResult);
            Console.WriteLine($"[UPDATE] Kết quả: {(updateResult ? "Thành công" : "Thất bại")}");

            // 5️⃣ Test Delete
            bool deleteResult = dao.Delete(getByIdResult.MaNganh);
            Console.WriteLine($"[DELETE] Kết quả: {(deleteResult ? "Thành công" : "Thất bại")}");

            Console.WriteLine("===== KẾT THÚC TEST NGANHDAO =====");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi trong quá trình test: {ex.Message}");
        }
    }
    
}