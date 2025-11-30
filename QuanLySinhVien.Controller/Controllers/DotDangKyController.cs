using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class DotDangKyController
{
    private static DotDangKyController _instance;
    private readonly DotDangKyDao _dotDangKyDao;
    private List<DotDangKyDto> _listLichDangKy;

    private DotDangKyController()
    {
        _dotDangKyDao = DotDangKyDao.GetInstance();
        _listLichDangKy = _dotDangKyDao.GetAll();
    }

    public static DotDangKyController GetInstance()
    {
        if (_instance == null) _instance = new DotDangKyController();
        return _instance;
    }

    public List<DotDangKyDto> GetAll()
    {
        return _dotDangKyDao.GetAll();
    }

    public bool Insert(DotDangKyDto dotDangKyDto)
    {
        return _dotDangKyDao.Insert(dotDangKyDto);
    }

    public bool Update(DotDangKyDto dotDangKyDto)
    {
        return _dotDangKyDao.Update(dotDangKyDto);
    }

    public bool Delete(int maLichDK)
    {
        return _dotDangKyDao.Delete(maLichDK);
    }

    public DotDangKyDto GetById(int maLichDK)
    {
        return _dotDangKyDao.GetById(maLichDK);
    }
}