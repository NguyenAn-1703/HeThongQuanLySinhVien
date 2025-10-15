using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Models.DAO;

public class SinhVienDAO
{
    public List<SinhVienDTO> getTableSinhVien()
    {
        List<SinhVienDTO> result = new();
        using var conn = MyConnection.GetConnection();
        const string sql = @" SELECT s.MaSV,s.TenSV,s.NgaySinhSV,s.GioiTinhSV,IFNULL(n.TenNganh, 'Chưa xác định') AS Nganh,s.TrangThaiSV AS TrangThai
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

    public SinhVienDTO GetSinhVienById(int maSinhVien){ 
        SinhVienDTO result = new();
        using var conn = MyConnection.GetConnection();
        const string sql = @"SELECT s.* , IFNULL(n.TenNganh, 'Chưa xác định') AS Nganh, s.MaLop, s.MaKhoaHoc
                            FROM SinhVien s 
                            LEFT JOIN Lop l ON s.MaLop = l.MaLop 
                            LEFT JOIN Nganh n ON n.MaNganh = l.MaNganh
                            WHERE s.MaSV = @MaSV";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSinhVien);
        using var reader = cmd.ExecuteReader();
        while (reader.Read()){
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

    public void Add(SinhVienDTO sinhVien)
    {
        using var conn = MyConnection.GetConnection();
        const string sql = @"INSERT INTO SinhVien (MaTK, MaLop, MaKhoaHoc, TenSV, SoDienThoaiSV, NgaySinhSV, 
                            QueQuanSV, TrangThaiSV, GioiTinhSV, EmailSV, CCCDSV, AnhDaiDienSV) 
                            VALUES (@MaTK, @MaLop, @MaKhoaHoc, @TenSV, @SoDienThoaiSV, @NgaySinhSV, 
                            @QueQuanSV, @TrangThaiSV, @GioiTinhSV, @EmailSV, @CCCDSV, @AnhDaiDienSV)";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaTK", sinhVien.MaTk ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@MaLop", sinhVien.MaLop);
        cmd.Parameters.AddWithValue("@MaKhoaHoc", 1);
        cmd.Parameters.AddWithValue("@TenSV", sinhVien.TenSinhVien);
        cmd.Parameters.AddWithValue("@SoDienThoaiSV", sinhVien.SdtSinhVien);
        cmd.Parameters.AddWithValue("@NgaySinhSV", sinhVien.NgaySinh);
        cmd.Parameters.AddWithValue("@QueQuanSV", sinhVien.QueQuanSinhVien);
        cmd.Parameters.AddWithValue("@TrangThaiSV", sinhVien.TrangThai);
        cmd.Parameters.AddWithValue("@GioiTinhSV", sinhVien.GioiTinh);
        cmd.Parameters.AddWithValue("@EmailSV", sinhVien.Email);
        cmd.Parameters.AddWithValue("@CCCDSV", sinhVien.CCCD);
        cmd.Parameters.AddWithValue("@AnhDaiDienSV", sinhVien.AnhDaiDienSinhVien ?? (object)DBNull.Value);
        cmd.ExecuteNonQuery();
    }
    
    public void Delete(int maSinhVien)
    {
        using var conn = MyConnection.GetConnection();
        const string sql = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", maSinhVien);
        cmd.ExecuteNonQuery();
    }

    public void Update(SinhVienDTO sinhVienDto)
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
                            TrangThaiSV = @TrangThaiSV
                            WHERE MaSV = @MaSV";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", sinhVienDto.MaSinhVien);
        cmd.Parameters.AddWithValue("@TenSV", sinhVienDto.TenSinhVien);
        cmd.Parameters.AddWithValue("@NgaySinhSV", sinhVienDto.NgaySinh);
        cmd.Parameters.AddWithValue("@GioiTinhSV", sinhVienDto.GioiTinh);
        cmd.Parameters.AddWithValue("@MaLop", sinhVienDto.MaLop);
        cmd.Parameters.AddWithValue("@MaKhoaHoc", sinhVienDto.MaKhoaHoc);
        cmd.Parameters.AddWithValue("@SoDienThoaiSV", sinhVienDto.SdtSinhVien);
        cmd.Parameters.AddWithValue("@QueQuanSV", sinhVienDto.QueQuanSinhVien);
        cmd.Parameters.AddWithValue("@EmailSV", sinhVienDto.Email);
        cmd.Parameters.AddWithValue("@CCCDSV", sinhVienDto.CCCD);
        cmd.Parameters.AddWithValue("@TrangThaiSV", sinhVienDto.TrangThai);
        cmd.ExecuteNonQuery();
    }

    public List<SinhVienDTO> Search(string searchText, string filter)
    {
        List<SinhVienDTO> result = new();
        using var conn = MyConnection.GetConnection();
        
        string sql = @"SELECT s.MaSV, s.TenSV, s.NgaySinhSV, s.GioiTinhSV, 
                       IFNULL(n.TenNganh, 'Chưa xác định') AS Nganh, s.TrangThaiSV AS TrangThai
                       FROM SinhVien s
                       LEFT JOIN Lop l ON s.MaLop = l.MaLop
                       LEFT JOIN Nganh n ON l.MaNganh = n.MaNganh";
        
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            sql += " WHERE ";
            
            if (filter == "Mã sinh viên")
            {
                sql += "CAST(s.MaSV AS CHAR) LIKE @SearchText";
            }
            else if (filter == "Tên sinh viên")
            {
                sql += "s.TenSV LIKE @SearchText";
            }
            else
            {
                sql += "(CAST(s.MaSV AS CHAR) LIKE @SearchText OR s.TenSV LIKE @SearchText)";
            }
        }
        
        sql += " ORDER BY s.MaSV";
        
        using var cmd = new MySqlCommand(sql, conn);
        
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
        }
        
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
    
    public int CountSinhVienByStatus(TrangThaiSV status)
    {
        using var conn = MyConnection.GetConnection();
        const string sql = "SELECT COUNT(*) FROM SinhVien WHERE TrangThaiSV = @Status";
        using var cmd = new MySqlCommand(sql, conn);
        string statusStr = TrangThaiSVHelper.ToDbString(status);
        cmd.Parameters.AddWithValue("@Status", statusStr);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}