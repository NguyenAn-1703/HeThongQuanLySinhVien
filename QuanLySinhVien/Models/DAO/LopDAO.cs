using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

public class LopDAO
{
    public List<LopDto> GetByNganh(int maNganh)
    {
        List<LopDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(@"
            SELECT l.MaLop, l.MaGV, l.MaNganh, l.TenLop, l.SoLuongSV,
                   COALESCE(COUNT(s.MaSV), 0) as SoSinhVienHienTai
            FROM lop l
            LEFT JOIN sinhvien s ON l.MaLop = s.MaLop AND s.Status = 1
            WHERE l.Status = 1 AND l.MaNganh = @MaNganh
            GROUP BY l.MaLop, l.MaGV, l.MaNganh, l.TenLop, l.SoLuongSV
            ORDER BY l.TenLop", conn);
        cmd.Parameters.AddWithValue("@MaNganh", maNganh);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new LopDto
            {
                MaLop = reader.GetInt32("MaLop"),
                MaGV = reader.GetInt32("MaGV"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenLop = reader.GetString("TenLop"),
                SoLuongSV = reader.GetInt32("SoLuongSV"),
                SoSinhVienHienTai = reader.GetInt32("SoSinhVienHienTai")
            });
        }

        return result;
    }
    
    public LopDto GetById(int maLop)
    {
        LopDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(@"
            SELECT l.MaLop, l.MaGV, l.MaNganh, l.TenLop, l.SoLuongSV,
                   COALESCE(COUNT(s.MaSV), 0) as SoSinhVienHienTai
            FROM lop l
            LEFT JOIN sinhvien s ON l.MaLop = s.MaLop AND s.Status = 1
            WHERE l.Status = 1 AND l.MaLop = @MaLop
            GROUP BY l.MaLop, l.MaGV, l.MaNganh, l.TenLop, l.SoLuongSV", conn);
        cmd.Parameters.AddWithValue("@MaLop", maLop);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new LopDto
            {
                MaLop = reader.GetInt32("MaLop"),
                MaGV = reader.GetInt32("MaGV"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenLop = reader.GetString("TenLop"),
                SoLuongSV = reader.GetInt32("SoLuongSV"),
                SoSinhVienHienTai = reader.GetInt32("SoSinhVienHienTai")
            };
        }

        return result;
    }
}


