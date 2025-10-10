using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

public class SinhVienDAO
{
    public List<SinhVienDTO> getTableSinhVien()
    {
        List<SinhVienDTO> result = new();
        using var conn = MyConnection.GetConnection();
        const string sql = @" SELECT s.MaSV,s.TenSV,s.NgaySinhSV,s.GioiTinhSV,n.TenNganh AS Nganh,s.TrangThaiSV AS TrangThai
        FROM SinhVien s
        JOIN Lop l   ON s.MaLop = l.MaLop
        JOIN Nganh n ON l.MaNganh = n.MaNganh
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
    
    
}