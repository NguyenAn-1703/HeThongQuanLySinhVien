using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class TaiKhoanController
{
    private static TaiKhoanController _instance;
    private readonly TaiKhoanDao _taiKhoanDao;
    private List<TaiKhoanDto> _listTaiKhoan;

    private TaiKhoanController()
    {
        _taiKhoanDao = TaiKhoanDao.GetInstance();
        _listTaiKhoan = _taiKhoanDao.GetAll();
    }

    public static TaiKhoanController getInstance()
    {
        if (_instance == null) _instance = new TaiKhoanController();
        return _instance;
    }

    public List<TaiKhoanDto> GetAll()
    {
        return _taiKhoanDao.GetAll();
    }

    public bool Insert(TaiKhoanDto taiKhoanDto)
    {
        return _taiKhoanDao.Insert(taiKhoanDto);
    }

    public bool Update(TaiKhoanDto taiKhoanDto)
    {
        return _taiKhoanDao.Update(taiKhoanDto);
    }


    public bool Delete(int idTaiKhoan)
    {
        return _taiKhoanDao.Delete(idTaiKhoan);
    }

    public TaiKhoanDto GetTaiKhoanById(int id)
    {
        return _taiKhoanDao.GetTaiKhoanById(id);
        ;
    }

    public TaiKhoanDto? GetTaiKhoanByUsrName(string usrName)
    {
        return _taiKhoanDao.GetTaiKhoanByUsrName(usrName);
    }

    public List<TaiKhoanDto> GetTaiKhoanNotUsed()
    {
        return _taiKhoanDao.GetTaiKhoanNotUsed();
    }

    public bool ExistByTen(string ten)
    {
        var listPh = GetAll();
        foreach (var item in listPh)
            if (item.TenDangNhap.Equals(ten))
                return true;

        return false;
    }
}