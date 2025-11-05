using QuanLySinhVien.Model.DAO;
using QuanLySinhVien.Shared.DTO.ThongKe;
using QuanLySinhVien.Shared.Structs;

namespace QuanLySinhVien.Controller.Controllers;

public class ThongKeDoanhThuController
{
    private ThongKeDoanhThuDao _ThongKeDoanhThuDao;
    private static ThongKeDoanhThuController _instance;

    private ThongKeDoanhThuController()
    {
        _ThongKeDoanhThuDao = ThongKeDoanhThuDao.GetInstance();
    }

    public static ThongKeDoanhThuController GetInstance()
    {
        if (_instance == null) _instance = new ThongKeDoanhThuController();

        return _instance;
    }

    public List<ThongKeDoanhThuDto> GetListThongKeDoanhThu()
    {
        return _ThongKeDoanhThuDao.GetListThongKeDoanhThu();
    }

    public List<NganhDoanhThu> GetListPieChart()
    {
        List<ThongKeDoanhThuDto> listThongKeDoanhThu = _ThongKeDoanhThuDao.GetListThongKeDoanhThu();

        double tong = listThongKeDoanhThu.Sum(x => x.TongTien);
        
        var top5Nganh = listThongKeDoanhThu
            .GroupBy(dt => new { dt.TenNganh })
            .Select(g => new
            {
                TenNganh = g.Key.TenNganh,
                TongTien = g.Sum(dt => dt.TongTien)
            })
            .OrderByDescending(x => x.TongTien)
            .Take(5)
            .ToList();


        List<NganhDoanhThu> rs = new List<NganhDoanhThu>();
        foreach (var item in top5Nganh)
        {
            NganhDoanhThu nganhDoanhThu = new NganhDoanhThu
            {
                ten = item.TenNganh,
                phanTram = item.TongTien / tong * 100,
            };
            rs.Add(nganhDoanhThu);
        }

        return rs;
    }
}