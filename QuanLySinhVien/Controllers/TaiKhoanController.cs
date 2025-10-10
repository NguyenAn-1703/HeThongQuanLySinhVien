using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Controllers;

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
        if (_instance == null)
        {
            _instance = new TaiKhoanController();
        }
        return _instance;
    }

    public List<TaiKhoanDto> GetAll()
    {
        return _taiKhoanDao.GetAll();
    }

    public bool Insert(TaiKhoanDto taiKhoanDto)
    {
        //Validate
        
        return (_taiKhoanDao.Insert(taiKhoanDto));
    }

    // edit khoa
    public bool Update(TaiKhoanDto taiKhoanDto)
    {
        //Validate
        
        
        return  (_taiKhoanDao.Update(taiKhoanDto));
    }


    // delete khoa
    public bool Delete(int idTaiKhoan)
    {
        // call DAO function
        return _taiKhoanDao.Delete(idTaiKhoan);
    }

    // get name by ID ( khoa.cs <-> dao )
    public TaiKhoanDto GetTaiKhoanById(int id)
    {
        return _taiKhoanDao.GetTaiKhoanById(id);;
    }
}