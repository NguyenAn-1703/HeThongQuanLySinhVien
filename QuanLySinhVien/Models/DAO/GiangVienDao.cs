using System.Data;
using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Models;

public class GiangVienDao
{
    public static List<GiangVienDto> GetAll()
    {
        List<GiangVienDto> list = new List<GiangVienDto>();
        
        try
        {
            using var con = MyConnection.GetConnection();
            string sql = "select gv.MaGV, gv.MaTK, gv.MaKhoa, gv.TenGV, gv.NgaySinhGV, gv.GioiTinhGV, gv.SoDienThoai, gv.Email, gv.TrangThai, gv.Status, k.TenKHoa\nfrom GiangVien gv\njoin Khoa k on gv.MaKhoa = k.MaKhoa";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new GiangVienDto
                {
                    MaGV = rd.GetInt32("MaGV"),
                    MaTK = rd.GetInt32("MaTK"),
                    MaKhoa = rd.GetInt32("MaKhoa"),
                    TenGV = rd.GetString("TenGV"),
                    NgaySinhGV = rd.GetDateOnly("NgaySinhGV"),
                    GioiTinhGV = rd.GetString("GioiTinhGV"),
                    SoDienThoai = rd.GetString("SoDienThoai") + "",
                    Email = rd.GetString("Email"),
                    TenKhoa = rd.GetString("TenKhoa"),
                    TrangThai = rd.GetString("TrangThai"),
                    Status = rd.GetInt16("Status"),
                });
            }
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
            string sql = "delete from GiangVien gv where gv.MaGV = @id";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            int res = cmd.ExecuteNonQuery();

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
            string sql = "update GiangVien gv set gv.Status = 0 where gv.MaGV = @id";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            int res = cmd.ExecuteNonQuery();

            if (res == 0) throw new Exception("Không tìm thấy giảng viên cần xóa");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static GiangVienDto GetGVById(int id)
    {
        GiangVienDto dto = new GiangVienDto();
        try
        {
            using var con = MyConnection.GetConnection();
            string sql = "select magv, matk, makhoa, tengv, ngaysinhgv, gioitinhgv, sodienthoai, email, trangthai, anhdaidiengv, status\nfrom giangvien\nwhere giangvien.MaGV = @id";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                GiangVienDto gv = new GiangVienDto
                {
                    MaGV = rd.GetInt32("MaGV"),
                    MaTK = rd.GetInt32("MaTK"),
                    MaKhoa = rd.GetInt32("MaKhoa"),
                    TenGV = rd.GetString("TenGV"),
                    NgaySinhGV = rd.GetDateOnly("NgaySinhGV"),
                    GioiTinhGV = rd.GetString("GioiTinhGV"),
                    SoDienThoai = rd.GetString("SoDienThoai") + "",
                    Email = rd.GetString("Email"),
                    TrangThai = rd.GetString("TrangThai"),
                    AnhDaiDien = rd.GetString("anhdaidiengv"),
                    Status = rd.GetInt16("Status"),
                };
                dto = gv;
            }

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
            string sql = "update giangvien gv\nset gv.MaKhoa = @MaKhoa, gv.MaTK = @MaTK, gv.TenGV = @TenGV, gv.NgaySinhGV = @NgaySinh, gv.GioiTinhGV = @GioiTinh, gv.SoDienThoai = @SoDienThoai, gv.Email = @Email, gv.TrangThai = @TrangThai, gv.AnhDaiDienGV = @AnhDaiDien\nwhere gv.MaGV = @MaGV";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@MaKhoa", dto.MaKhoa);
            cmd.Parameters.AddWithValue("@MaTK", dto.MaTK);
            cmd.Parameters.AddWithValue("@TenGV", dto.TenGV);
            cmd.Parameters.AddWithValue("@NgaySinh", dto.NgaySinhGV);
            cmd.Parameters.AddWithValue("@GioiTinh", dto.GioiTinhGV);
            cmd.Parameters.AddWithValue("@SoDienThoai", dto.SoDienThoai);
            cmd.Parameters.AddWithValue("@Email", dto.Email);
            cmd.Parameters.AddWithValue("@TrangThai", dto.TrangThai);
            cmd.Parameters.AddWithValue("@AnhDaiDien", dto.AnhDaiDien);
            cmd.Parameters.AddWithValue("@MaGV", dto.MaGV);
            int res = cmd.ExecuteNonQuery();

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
            string sql = @"
                            insert into GiangVien (
                                MaKhoa, MaTK, TenGV, NgaySinhGV, GioiTinhGV, SoDienThoai, Email, TrangThai, AnhDaiDienGV
                            ) VALUES (
                                @MaKhoa, @MaTK, @TenGV, @NgaySinh, @GioiTinh, @SoDienThoai, @Email, @TrangThai, @AnhDaiDien
                            )";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@MaKhoa", dto.MaKhoa);
            cmd.Parameters.AddWithValue("@MaTK", dto.MaTK);
            cmd.Parameters.AddWithValue("@TenGV", dto.TenGV);
            cmd.Parameters.AddWithValue("@NgaySinh", dto.NgaySinhGV);
            cmd.Parameters.AddWithValue("@GioiTinh", dto.GioiTinhGV);
            cmd.Parameters.AddWithValue("@SoDienThoai", dto.SoDienThoai);
            cmd.Parameters.AddWithValue("@Email", dto.Email);
            cmd.Parameters.AddWithValue("@TrangThai", dto.TrangThai);
            cmd.Parameters.AddWithValue("@AnhDaiDien", dto.AnhDaiDien);
            int res = cmd.ExecuteNonQuery();

            if (res == 0) throw new Exception("Không thểm thêm mới giảng viên");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }
    
    public int CountGiangVienByStatus(TrangThaiGV status)
    {
        using var conn = MyConnection.GetConnection();
        const string sql = "SELECT COUNT(*) FROM GiangVien WHERE TrangThai = @Status";
        using var cmd = new MySqlCommand(sql, conn);
        string statusStr = TrangThaiGVHelper.ToDbString(status);
        cmd.Parameters.AddWithValue("@Status", statusStr);
        return Convert.ToInt16(cmd.ExecuteScalar());
    }

}