using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class ChucNangController
{
    private static ChucNangController _instance;
    private readonly ChucNangDao _ChucNangDao;
    private List<ChucNangDto> _listChucNang;

    private ChucNangController()
    {
        _ChucNangDao = ChucNangDao.GetInstance();
        _listChucNang = _ChucNangDao.GetAll();
    }

    public static ChucNangController getInstance()
    {
        if (_instance == null)
        {
            _instance = new ChucNangController();
        }
        return _instance;
    }

    public List<ChucNangDto> GetAll()
    {
        return _ChucNangDao.GetAll();
    }

    public ChucNangDto GetById(int id)
    {
        return _ChucNangDao.GetById(id);
    }


}