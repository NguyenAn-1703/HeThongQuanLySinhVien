using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class KhoaHocController
{
    private static KhoaHocController _instance;
    private readonly KhoaHocDao _khoaHocDao;
    private List<KhoaHocDto> _listKhoaHoc;

    private KhoaHocController()
    {
        _khoaHocDao = KhoaHocDao.GetInstance();
        _listKhoaHoc = _khoaHocDao.GetAll();
    }

    public static KhoaHocController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new KhoaHocController();
        }
        return _instance;
    }

    public List<KhoaHocDto> GetAll()
    {
        return _khoaHocDao.GetAll();
    }

    public bool Insert(KhoaHocDto khoaHocDto)
    {
        return _khoaHocDao.Insert(khoaHocDto);
    }

    public bool Update(KhoaHocDto khoaHocDto)
    {
        return _khoaHocDao.Update(khoaHocDto);
    }

    public bool Delete(int maKhoaHoc)
    {
        return _khoaHocDao.Delete(maKhoaHoc);
    }

    public KhoaHocDto GetKhoaHocById(int maKhoaHoc)
    {
        return _khoaHocDao.GetKhoaHocById(maKhoaHoc);
    }
    
}
