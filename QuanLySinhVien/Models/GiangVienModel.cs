using System.Data;
using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models;

public class GiangVienModel
{
    
    
    public static List<GiangVien> GetAll()
    {
        List<GiangVien> list = new List<GiangVien>();
        
        try
        {
            using var con = MyConnection.GetConnection();
            string sql = "select gv.MaGV, gv.MaTK, gv.MaKhoa, gv.TenGV, gv.NgaySinhGV, gv.GioiTinhGV, gv.SoDienThoai, gv.Email, gv.Status, k.TenKHoa\nfrom GiangVien gv\njoin Khoa k on gv.MaKhoa = k.MaKhoa";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new GiangVien
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
                    Status = rd.GetString("Status"),
                });
            }
            con.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return list;
    }

}