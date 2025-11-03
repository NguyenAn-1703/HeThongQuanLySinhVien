using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models;

public static class GiangVienDao
{
    public static List<GiangVienDto> GetAll()
    {
        var list = new List<GiangVienDto>();

        try
        {
            using var con = MyConnection.GetConnection();
            var sql = @"
                SELECT gv.MaGV, gv.MaTK, gv.MaKhoa, gv.TenGV, gv.NgaySinhGV, 
                       gv.GioiTinhGV, gv.SoDienThoai, gv.Email, gv.Status, k.TenKhoa
                FROM GiangVien gv
                JOIN Khoa k ON gv.MaKhoa = k.MaKhoa WHERE gv.Status = 1";
            var cmd = new MySqlCommand(sql, con);
            var rd = cmd.ExecuteReader();
            while (rd.Read())
                list.Add(new GiangVienDto
                {
                    MaGV = rd.GetInt32("MaGV"),
                    MaTK = rd.GetInt32("MaTK"),
                    MaKhoa = rd.GetInt32("MaKhoa"),
                    TenGV = rd.GetString("TenGV"),
                    NgaySinhGV = rd.GetString("NgaySinhGV"),
                    GioiTinhGV = rd.GetString("GioiTinhGV"),
                    SoDienThoai = rd.GetString("SoDienThoai") + "",
                    Email = rd.GetString("Email"),
                    TenKhoa = rd.GetString("TenKhoa")
                });
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return list;
    }

    public static void HardDeleteById(int id)
    {
        try
        {
            using var con = MyConnection.GetConnection();
            var sql = "DELETE FROM GiangVien WHERE MaGV = @id";
            var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            var res = cmd.ExecuteNonQuery();

            if (res == 0) throw new Exception("Không tìm thấy giảng viên cần xóa");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static void SoftDeleteById(int id)
    {
        try
        {
            using var con = MyConnection.GetConnection();
            var sql = "UPDATE GiangVien SET Status = 0 WHERE MaGV = @id";
            var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            var res = cmd.ExecuteNonQuery();

            if (res == 0) throw new Exception("Không tìm thấy giảng viên cần xóa");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static GiangVienDto GetGVById(int id)
    {
        var dto = new GiangVienDto();
        try
        {
            using var con = MyConnection.GetConnection();
            var sql = @"
                SELECT MaGV, MaTK, MaKhoa, TenGV, NgaySinhGV, GioiTinhGV, 
                       SoDienThoai, Email, AnhDaiDienGV, Status
                FROM GiangVien
                WHERE MaGV = @id";
            var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            var rd = cmd.ExecuteReader();
            while (rd.Read())
                dto = new GiangVienDto
                {
                    MaGV = rd.GetInt32("MaGV"),
                    MaTK = rd.GetInt32("MaTK"),
                    MaKhoa = rd.GetInt32("MaKhoa"),
                    TenGV = rd.GetString("TenGV"),
                    NgaySinhGV = rd.GetString("NgaySinhGV"),
                    GioiTinhGV = rd.GetString("GioiTinhGV"),
                    SoDienThoai = rd.GetString("SoDienThoai") + "",
                    Email = rd.GetString("Email"),
                    AnhDaiDien = rd.GetString("AnhDaiDienGV"),
                    Status = rd.GetInt16("Status")
                };

            if (dto.MaTK == 0) throw new Exception("Không tìm thấy mã giảng viên");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return dto;
    }

    public static void UpdateGV(GiangVienDto dto)
    {
        try
        {
            using var con = MyConnection.GetConnection();
            var sql = @"
                UPDATE GiangVien
                SET MaKhoa = @MaKhoa,
                    MaTK = @MaTK,
                    TenGV = @TenGV,
                    NgaySinhGV = @NgaySinh,
                    GioiTinhGV = @GioiTinh,
                    SoDienThoai = @SoDienThoai,
                    Email = @Email,
                    AnhDaiDienGV = @AnhDaiDien
                WHERE MaGV = @MaGV";
            var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@MaKhoa", dto.MaKhoa);
            cmd.Parameters.AddWithValue("@MaTK", dto.MaTK);
            cmd.Parameters.AddWithValue("@TenGV", dto.TenGV);
            cmd.Parameters.AddWithValue("@NgaySinh", dto.NgaySinhGV);
            cmd.Parameters.AddWithValue("@GioiTinh", dto.GioiTinhGV);
            cmd.Parameters.AddWithValue("@SoDienThoai", dto.SoDienThoai);
            cmd.Parameters.AddWithValue("@Email", dto.Email);
            cmd.Parameters.AddWithValue("@AnhDaiDien", dto.AnhDaiDien);
            cmd.Parameters.AddWithValue("@MaGV", dto.MaGV);
            var res = cmd.ExecuteNonQuery();

            if (res == 0) throw new Exception("Không tìm thấy mã giảng viên cần sửa");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static void InsertGV(GiangVienDto dto)
    {
        try
        {
            using var con = MyConnection.GetConnection();
            var sql = @"
                INSERT INTO GiangVien (
                    MaKhoa, MaTK, TenGV, NgaySinhGV, GioiTinhGV, SoDienThoai, Email, AnhDaiDienGV
                ) VALUES (
                    @MaKhoa, @MaTK, @TenGV, @NgaySinh, @GioiTinh, @SoDienThoai, @Email, @AnhDaiDien
                )";
            var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@MaKhoa", dto.MaKhoa);
            cmd.Parameters.AddWithValue("@MaTK", dto.MaTK);
            cmd.Parameters.AddWithValue("@TenGV", dto.TenGV);
            cmd.Parameters.AddWithValue("@NgaySinh", dto.NgaySinhGV);
            cmd.Parameters.AddWithValue("@GioiTinh", dto.GioiTinhGV);
            cmd.Parameters.AddWithValue("@SoDienThoai", dto.SoDienThoai);
            cmd.Parameters.AddWithValue("@Email", dto.Email);
            cmd.Parameters.AddWithValue("@AnhDaiDien", dto.AnhDaiDien);
            var res = cmd.ExecuteNonQuery();

            if (res == 0) throw new Exception("Không thể thêm mới giảng viên");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static int CountGiangVienByStatus(short status)
    {
        using var conn = MyConnection.GetConnection();
        const string sql = "SELECT COUNT(*) FROM GiangVien WHERE Status = @Status";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Status", status);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public static GiangVienDto GetGVByTen(string tenGV)
    {
        var dto = new GiangVienDto();
        try
        {
            using var con = MyConnection.GetConnection();
            var sql = @"
                SELECT MaGV, MaTK, MaKhoa, TenGV, NgaySinhGV, GioiTinhGV, 
                       SoDienThoai, Email, AnhDaiDienGV, Status
                FROM GiangVien
                WHERE TenGV = @TenGV";
            var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@TenGV", tenGV);
            var rd = cmd.ExecuteReader();
            while (rd.Read())
                dto = new GiangVienDto
                {
                    MaGV = rd.GetInt32("MaGV"),
                    MaTK = rd.GetInt32("MaTK"),
                    MaKhoa = rd.GetInt32("MaKhoa"),
                    TenGV = rd.GetString("TenGV"),
                    NgaySinhGV = rd.GetString("NgaySinhGV"),
                    GioiTinhGV = rd.GetString("GioiTinhGV"),
                    SoDienThoai = rd.GetString("SoDienThoai") + "",
                    Email = rd.GetString("Email"),
                    AnhDaiDien = rd.GetString("AnhDaiDienGV"),
                    Status = rd.GetInt16("Status")
                };

            if (dto.MaTK == 0) throw new Exception("Không tìm thấy mã giảng viên");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return dto;
    }
}