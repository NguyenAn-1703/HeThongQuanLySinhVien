using System.Data;
using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models;

public class GiangVienModel
{
    
    
    public static List<GiangVienDTO> GetAll()
    {
        List<GiangVienDTO> list = new List<GiangVienDTO>();
        
        try
        {
            using var con = MyConnection.GetConnection();
            string sql = "select gv.MaGV, gv.MaTK, gv.MaKhoa, gv.TenGV, gv.NgaySinhGV, gv.GioiTinhGV, gv.SoDienThoai, gv.Email, gv.TrangThai, gv.Status, k.TenKHoa\nfrom GiangVien gv\njoin Khoa k on gv.MaKhoa = k.MaKhoa";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new GiangVienDTO
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

}