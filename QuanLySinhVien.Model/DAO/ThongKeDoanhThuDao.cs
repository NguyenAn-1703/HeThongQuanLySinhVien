using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO.ThongKe;

namespace QuanLySinhVien.Model.DAO;

public class ThongKeDoanhThuDao
{
    private static ThongKeDoanhThuDao _instance;

    private ThongKeDoanhThuDao()
    {
    }

    public static ThongKeDoanhThuDao GetInstance()
    {
        if (_instance == null) _instance = new ThongKeDoanhThuDao();
        return _instance;
    }
    
    public List<ThongKeDoanhThuDto> GetListThongKeDoanhThu()
    {
        List<ThongKeDoanhThuDto> result = new();

        using var conn = MyConnection.GetConnection();
        string query = @"
        SELECT 
            n.MaNganh,
            n.TenNganh,
            IFNULL(SUM(hp.DaThu), 0) AS TongTien
        FROM nganh n
        JOIN lop l ON n.MaNganh = l.MaNganh
        JOIN sinhvien sv ON l.MaLop = sv.MaLop
        JOIN hocphisv hp ON sv.MaSV = hp.MaSV
        WHERE hp.Status = 1
        GROUP BY n.MaNganh, n.TenNganh
        ORDER BY TongTien DESC;
    ";

        using var cmd = new MySqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new ThongKeDoanhThuDto
            {
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh"),
                TongTien = reader.GetDouble("TongTien")
            });
        }

        return result;
    }

}