using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Models.DAO;

public class SinhVienDAO
{
    
    public List<SinhVienDTO> GetAll()
    {
        List<SinhVienDTO> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(@"
        SELECT MaSV, MaTK, MaLop, MaKhoaHoc, TenSV, SoDienThoaiSV, 
               NgaySinhSV, QueQuanSV, TrangThaiSV, GioiTinhSV, 
               EmailSV, CCCDSV, AnhDaiDienSV
        FROM SinhVien
        WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new SinhVienDTO
            {
                MaSinhVien = reader.GetInt32("MaSV"),
                MaTk = reader.GetInt32("MaTK"),
                MaLop = reader.GetInt32("MaLop"),
                MaKhoaHoc = reader.GetInt32("MaKhoaHoc"),
                TenSinhVien = reader.GetString("TenSV"),
                SdtSinhVien = reader.GetString("SoDienThoaiSV"),
                NgaySinh = reader.GetString("NgaySinhSV"),
                QueQuanSinhVien = reader.GetString("QueQuanSV"),
                TrangThai = reader.GetString("TrangThaiSV"),
                GioiTinh = reader.GetString("GioiTinhSV"),
                Email = reader.GetString("EmailSV"),
                CCCD = reader.GetString("CCCDSV"),
                AnhDaiDienSinhVien = reader.GetString("AnhDaiDienSV"),
                Nganh = "" // nếu có bảng ngành riêng thì phần này sẽ được gán khi JOIN
            });
        }

        return result;
    }
    
    public SinhVienDTO GetById(int id)
    {
        SinhVienDTO result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(@"
        SELECT MaSV, MaTK, MaLop, MaKhoaHoc, TenSV, SoDienThoaiSV, 
               NgaySinhSV, QueQuanSV, TrangThaiSV, GioiTinhSV, 
               EmailSV, CCCDSV, AnhDaiDienSV
        FROM SinhVien
        WHERE Status = 1 AND MaSV = @MaSV", conn);
        cmd.Parameters.AddWithValue("@MaSV", id);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new SinhVienDTO
            {
                MaSinhVien = reader.GetInt32("MaSV"),
                MaTk = reader.GetInt32("MaTK"),
                MaLop = reader.GetInt32("MaLop"),
                MaKhoaHoc = reader.GetInt32("MaKhoaHoc"),
                TenSinhVien = reader.GetString("TenSV"),
                SdtSinhVien = reader.GetString("SoDienThoaiSV"),
                NgaySinh = reader.GetString("NgaySinhSV"),
                QueQuanSinhVien = reader.GetString("QueQuanSV"),
                TrangThai = reader.GetString("TrangThaiSV"),
                GioiTinh = reader.GetString("GioiTinhSV"),
                Email = reader.GetString("EmailSV"),
                CCCD = reader.GetString("CCCDSV"),
                AnhDaiDienSinhVien = reader.GetString("AnhDaiDienSV")
            };
        }

        return result;
    }

    
    public List<SinhVienDTO> getTableSinhVien()
    {
        List<SinhVienDTO> result = new();
        using var conn = MyConnection.GetConnection();
        const string sql =
            @" SELECT s.MaSV,s.TenSV,s.NgaySinhSV,s.GioiTinhSV,IFNULL(n.TenNganh, 'Chưa xác định') AS Nganh,s.TrangThaiSV AS TrangThai
        FROM SinhVien s
        LEFT JOIN Lop l   ON s.MaLop = l.MaLop
        LEFT JOIN Nganh n ON l.MaNganh = n.MaNganh
        ORDER BY s.MaSV;";
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new SinhVienDTO(
                reader.GetInt32(reader.GetOrdinal("MaSV")),
                reader.GetString(reader.GetOrdinal("TenSV")),
                reader.GetString(reader.GetOrdinal("NgaySinhSV")),
                reader.GetString(reader.GetOrdinal("GioiTinhSV")),
                reader.GetString(reader.GetOrdinal("Nganh")),
                reader.GetString(reader.GetOrdinal("TrangThai"))
            ));
        }

        return result;
    }
    
    

    public SinhVienDTO GetSinhVienById(int maSinhVien)
    {
        SinhVienDTO result = new();
        using var conn = MyConnection.GetConnection();
        const string sql = @"SELECT s.* , IFNULL(n.TenNganh, 'Chưa xác định') AS Nganh 
                            FROM SinhVien s 
                            LEFT JOIN Lop l ON s.MaLop = l.MaLop 
                            LEFT JOIN Nganh n ON n.MaNganh = l.MaNganh
                            WHERE s.MaSV = @MaSV";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSinhVien);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.MaSinhVien = reader.GetInt32(reader.GetOrdinal("MaSV"));
            result.TenSinhVien = reader.GetString(reader.GetOrdinal("TenSV"));
            result.NgaySinh = reader.GetString(reader.GetOrdinal("NgaySinhSV"));
            result.GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinhSV"));
            result.SdtSinhVien = reader.GetString(reader.GetOrdinal("SoDienThoaiSV"));
            result.QueQuanSinhVien = reader.GetString(reader.GetOrdinal("QueQuanSV"));
            result.Email = reader.GetString(reader.GetOrdinal("EmailSV"));
            result.CCCD = reader.GetString(reader.GetOrdinal("CCCDSV"));
            result.TrangThai = reader.GetString(reader.GetOrdinal("TrangThaiSV"));
            result.Nganh = reader.GetString(reader.GetOrdinal("Nganh"));
            result.MaLop = reader.GetInt32(reader.GetOrdinal("MaLop"));
            result.MaKhoaHoc = reader.GetInt32(reader.GetOrdinal("MaKhoaHoc"));
        }

        return result;
    }

public bool Add(SinhVienDTO sinhVien)
{
    using var conn = MyConnection.GetConnection();
    const string sql = @"INSERT INTO SinhVien 
                        (MaTK, MaLop, MaKhoaHoc, TenSV, SoDienThoaiSV, NgaySinhSV, 
                         QueQuanSV, TrangThaiSV, GioiTinhSV, EmailSV, CCCDSV, AnhDaiDienSV) 
                        VALUES 
                        (@MaTK, @MaLop, @MaKhoaHoc, @TenSV, @SoDienThoaiSV, @NgaySinhSV, 
                         @QueQuanSV, @TrangThaiSV, @GioiTinhSV, @EmailSV, @CCCDSV, @AnhDaiDienSV)";
    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@MaTK", sinhVien.MaTk);
    cmd.Parameters.AddWithValue("@MaLop", sinhVien.MaLop);
    cmd.Parameters.AddWithValue("@MaKhoaHoc", sinhVien.MaKhoaHoc);
    cmd.Parameters.AddWithValue("@TenSV", sinhVien.TenSinhVien);
    cmd.Parameters.AddWithValue("@SoDienThoaiSV", sinhVien.SdtSinhVien);
    cmd.Parameters.AddWithValue("@NgaySinhSV", sinhVien.NgaySinh);
    cmd.Parameters.AddWithValue("@QueQuanSV", sinhVien.QueQuanSinhVien);
    cmd.Parameters.AddWithValue("@TrangThaiSV", sinhVien.TrangThai);
    cmd.Parameters.AddWithValue("@GioiTinhSV", sinhVien.GioiTinh);
    cmd.Parameters.AddWithValue("@EmailSV", sinhVien.Email);
    cmd.Parameters.AddWithValue("@CCCDSV", sinhVien.CCCD);
    cmd.Parameters.AddWithValue("@AnhDaiDienSV", sinhVien.AnhDaiDienSinhVien ?? (object)DBNull.Value);

    int rows = cmd.ExecuteNonQuery();
    return rows > 0;
}

public bool Delete(int maSinhVien)
{
    using var conn = MyConnection.GetConnection();
    const string sql = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@MaSV", maSinhVien);

    int rows = cmd.ExecuteNonQuery();
    return rows > 0;
}

public bool Update(SinhVienDTO sinhVien)
{
    using var conn = MyConnection.GetConnection();
    const string sql = @"UPDATE SinhVien SET 
                            TenSV = @TenSV,
                            NgaySinhSV = @NgaySinhSV,
                            GioiTinhSV = @GioiTinhSV,
                            MaLop = @MaLop,
                            MaKhoaHoc = @MaKhoaHoc,
                            SoDienThoaiSV = @SoDienThoaiSV,
                            QueQuanSV = @QueQuanSV,
                            EmailSV = @EmailSV,
                            CCCDSV = @CCCDSV,
                            TrangThaiSV = @TrangThaiSV,
                            AnhDaiDienSV = @AnhDaiDienSV
                        WHERE MaSV = @MaSV";

    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@MaSV", sinhVien.MaSinhVien);
    cmd.Parameters.AddWithValue("@TenSV", sinhVien.TenSinhVien);
    cmd.Parameters.AddWithValue("@NgaySinhSV", sinhVien.NgaySinh);
    cmd.Parameters.AddWithValue("@GioiTinhSV", sinhVien.GioiTinh);
    cmd.Parameters.AddWithValue("@MaLop", sinhVien.MaLop);
    cmd.Parameters.AddWithValue("@MaKhoaHoc", sinhVien.MaKhoaHoc);
    cmd.Parameters.AddWithValue("@SoDienThoaiSV", sinhVien.SdtSinhVien);
    cmd.Parameters.AddWithValue("@QueQuanSV", sinhVien.QueQuanSinhVien);
    cmd.Parameters.AddWithValue("@EmailSV", sinhVien.Email);
    cmd.Parameters.AddWithValue("@CCCDSV", sinhVien.CCCD);
    cmd.Parameters.AddWithValue("@TrangThaiSV", sinhVien.TrangThai);
    cmd.Parameters.AddWithValue("@AnhDaiDienSV", sinhVien.AnhDaiDienSinhVien ?? (object)DBNull.Value);

    int rows = cmd.ExecuteNonQuery();
    return rows > 0;
}


    // public List<SinhVienDTO> Search(string searchText, string filter)
    // {
    //     List<SinhVienDTO> result = new();
    //     using var conn = MyConnection.GetConnection();
    //
    //     string sql = @"SELECT s.MaSV, s.TenSV, s.NgaySinhSV, s.GioiTinhSV, 
    //                    IFNULL(n.TenNganh, 'Chưa xác định') AS Nganh, s.TrangThaiSV AS TrangThai
    //                    FROM SinhVien s
    //                    LEFT JOIN Lop l ON s.MaLop = l.MaLop
    //                    LEFT JOIN Nganh n ON l.MaNganh = n.MaNganh";
    //
    //     if (!string.IsNullOrWhiteSpace(searchText))
    //     {
    //         sql += " WHERE ";
    //
    //         if (filter == "Mã sinh viên")
    //         {
    //             sql += "CAST(s.MaSV AS CHAR) LIKE @SearchText";
    //         }
    //         else if (filter == "Tên sinh viên")
    //         {
    //             sql += "s.TenSV LIKE @SearchText";
    //         }
    //         else
    //         {
    //             sql += "(CAST(s.MaSV AS CHAR) LIKE @SearchText OR s.TenSV LIKE @SearchText)";
    //         }
    //     }
    //
    //     sql += " ORDER BY s.MaSV";
    //
    //     using var cmd = new MySqlCommand(sql, conn);
    //
    //     if (!string.IsNullOrWhiteSpace(searchText))
    //     {
    //         cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
    //     }
    //
    //     using var reader = cmd.ExecuteReader();
    //
    //     while (reader.Read())
    //     {
    //         result.Add(new SinhVienDTO(
    //             reader.GetInt32(reader.GetOrdinal("MaSV")),
    //             reader.GetString(reader.GetOrdinal("TenSV")),
    //             reader.GetString(reader.GetOrdinal("NgaySinhSV")),
    //             reader.GetString(reader.GetOrdinal("GioiTinhSV")),
    //             reader.GetString(reader.GetOrdinal("Nganh")),
    //             reader.GetString(reader.GetOrdinal("TrangThai"))
    //         ));
    //     }
    //
    //     return result;
    // }

    public int CountSinhVienByStatus(TrangThaiSV status)
    {
        using var conn = MyConnection.GetConnection();
        const string sql = "SELECT COUNT(*) FROM SinhVien WHERE TrangThaiSV = @Status";
        using var cmd = new MySqlCommand(sql, conn);
        string statusStr = TrangThaiSVHelper.ToDbString(status);
        cmd.Parameters.AddWithValue("@Status", statusStr);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    // Thống kê số lượng sinh viên còn đang học theo khóa học
    public Dictionary<string, int> GetSinhVienCountByKhoaHoc()
    {
        var result = new Dictionary<string, int>();

        using var conn = MyConnection.GetConnection();
        const string sql = @"SELECT KhoaHoc.MaKhoaHoc, KhoaHoc.TenKhoaHoc, COUNT(*) AS SoLuong 
                             FROM SinhVien JOIN KhoaHoc ON SinhVien.MaKhoaHoc = KhoaHoc.MaKhoaHoc
                             WHERE TrangThaiSV = 'Đang học'
                             GROUP BY KhoaHoc.MaKhoaHoc, KhoaHoc.TenKhoaHoc;";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string tenKhoaHoc = reader.GetString("TenKhoaHoc");
            int soLuong = reader.GetInt32("SoLuong");
            result[tenKhoaHoc] = soLuong;
        }

        return result;
    }

    public Dictionary<string, float> TyLeSinhVienTotNghiepTheoNganh(bool IsDesc = true)
    {
        var result = new Dictionary<string, float>();

        try
        {
            using var conn = MyConnection.GetConnection();

            const string sql = @"
            SELECT 
                Nganh.MaNganh, 
                Nganh.TenNganh, 
                COUNT(*) AS TongSoSinhVien, 
                COUNT(CASE WHEN SinhVien.TrangThaiSV = 'Tốt nghiệp' THEN 1 END) AS SoLuongTotNghiep
            FROM SinhVien
            JOIN Lop ON SinhVien.MaLop = Lop.MaLop
            JOIN Nganh ON Lop.MaNganh = Nganh.MaNganh
            GROUP BY Nganh.MaNganh, Nganh.TenNganh;";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string tenNganh = reader.GetString("TenNganh");
                int tongSoSinhVien = reader.GetInt32("TongSoSinhVien");
                int soLuongTotNghiep = reader.GetInt32("SoLuongTotNghiep");

                float tyLeTotNghiep = tongSoSinhVien == 0
                    ? 0
                    : (float)Math.Round((float)soLuongTotNghiep / tongSoSinhVien * 100, 2);

                result[tenNganh] = tyLeTotNghiep;
            }

            if (IsDesc)
                result = result
                    .OrderByDescending(kv => kv.Value)
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
            else
                result = result
                    .OrderBy(kv => kv.Value)
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi truy vấn dữ liệu: {ex.Message}");
        }

        return result;
    }

    // Viết tạm chờ Học phí
    public ulong TongHocPhiDaThu()
    {
        ulong tongHocPhi = 0;
        using var conn = MyConnection.GetConnection();
        const string sql = @"SELECT SUM(DaThu) AS TongHocPhiDaThu FROM HocPhiSV WHERE Status = 1 ;";
        using var cmd = new MySqlCommand(sql, conn);
        var result = cmd.ExecuteScalar();
        tongHocPhi = result == DBNull.Value ? 0UL : Convert.ToUInt64(result);

        return tongHocPhi;
    }

    public Dictionary<string, double> SoLuongSinhVienTheoNamNhapHoc()
    {
        var result = new Dictionary<string, double>();

        try
        {
            using var conn = MyConnection.GetConnection();

            const string sql = @"
            SELECT  LEFT(khoahoc.NienKhoaHoc, 4) AS NamNhapHoc, COUNT(*) AS TongSo 
            FROM sinhvien
            JOIN khoahoc ON sinhvien.makhoahoc = khoahoc.makhoahoc
            GROUP BY khoahoc.NienKhoaHoc
            ORDER BY NamNhapHoc DESC";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string namNhapHoc = reader.GetString("NamNhapHoc");
                double tongSo = reader.GetInt32("TongSo");

                result[namNhapHoc] = tongSo;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi truy vấn dữ liệu: {ex.Message}");
        }

        return result;
    }
}