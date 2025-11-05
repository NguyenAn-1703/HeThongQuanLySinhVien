using QuanLySinhVien.Model.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.DTO.ThongKe;

namespace QuanLySinhVien.Controller.Controllers;

public class ThongKeHocLucController
{
    private ThongKeHocLucDao _thongKeHocLucDao;
    private static ThongKeHocLucController _instance;

    private ThongKeHocLucController()
    {
        _thongKeHocLucDao = ThongKeHocLucDao.GetInstance();
    }

    public static ThongKeHocLucController GetInstance()
    {
        if (_instance == null) _instance = new ThongKeHocLucController();

        return _instance;
    }

    public List<ThongKeHocLucDto> GetSinhVienThongKeHocLuc()
    {
        return _thongKeHocLucDao.GetSinhVienThongKeHocLuc();
    }
}