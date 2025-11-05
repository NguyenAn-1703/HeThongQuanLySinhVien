using System.Data;
using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO.ThongKe;

namespace QuanLySinhVien.Model.DAO;

public class ThongKeHocLucDao
{
    private static ThongKeHocLucDao _instance;

    private ThongKeHocLucDao()
    {
    }

    public static ThongKeHocLucDao GetInstance()
    {
        if (_instance == null) _instance = new ThongKeHocLucDao();
        return _instance;
    }

    public List<ThongKeHocLucDto> GetSinhVienThongKeHocLuc()
    {
        List<ThongKeHocLucDto> result = new();

        using var conn = MyConnection.GetConnection();
        string query = @"
    SELECT 
        n.MaNganh,
        n.TenNganh,
        COUNT(sv.MaSV) AS TongSV,
        SUM(CASE WHEN tb.DiemTB > 3.0 THEN 1 ELSE 0 END) AS SVGioiXS
    FROM nganh n
    JOIN lop l ON n.MaNganh = l.MaNganh
    JOIN sinhvien sv ON l.MaLop = sv.MaLop
    LEFT JOIN (
        SELECT 
            MaSV, 
            AVG(DiemHe4) AS DiemTB
        FROM ketqua
        GROUP BY MaSV
    ) AS tb ON sv.MaSV = tb.MaSV
    GROUP BY n.MaNganh, n.TenNganh
    ORDER BY (SUM(CASE WHEN tb.DiemTB > 3.0 THEN 1 ELSE 0 END) / COUNT(sv.MaSV)) DESC;
";


        using var cmd = new MySqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int tongSV = reader.GetInt32("TongSV");
            int svGioiXS = reader.IsDBNull("SVGioiXS") ? 0 : reader.GetInt32("SVGioiXS");

            float tiLe = tongSV == 0 ? 0 : (float)Math.Round((svGioiXS * 100.0 / tongSV), 2);

            result.Add(new ThongKeHocLucDto
            {
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh"),
                TongSV = tongSV,
                SVGioiXS = svGioiXS,
                TiLe = tiLe
            });
        }

        return result;
    }
}