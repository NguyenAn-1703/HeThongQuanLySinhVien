using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class HocPhiSVController
{
    private static HocPhiSVController _instance;
    private readonly HocPhiSVDao _hocPhiSVDao;
    public List<HocPhiSVDto> _listHocPhiSV;

    private HocPhiSVController()
    {
        _hocPhiSVDao = HocPhiSVDao.GetInstance();
        _listHocPhiSV = _hocPhiSVDao.GetAll();
    }

    public static HocPhiSVController GetInstance()
    {
        if (_instance == null) _instance = new HocPhiSVController();
        return _instance;
    }

    public List<HocPhiSVDto> GetAll()
    {
        return _hocPhiSVDao.GetAll();
    }

    public bool Insert(HocPhiSVDto hocPhiSVDto)
    {
        var rs = _hocPhiSVDao.Insert(hocPhiSVDto);
        if (rs)
            _listHocPhiSV = _hocPhiSVDao.GetAll();
        return rs;
    }

    public bool Update(HocPhiSVDto hocPhiSVDto)
    {
        var rs = _hocPhiSVDao.Update(hocPhiSVDto);
        if (rs)
            _listHocPhiSV = _hocPhiSVDao.GetAll();
        return rs;
    }

    public bool Delete(int maHP)
    {
        return _hocPhiSVDao.Delete(maHP);
    }

    public HocPhiSVDto GetById(int maHP)
    {
        return _hocPhiSVDao.GetById(maHP);
    }

    public bool ExistByMaSVHKyNam(int masv, int hocky, string nam)
    {
        _listHocPhiSV = _hocPhiSVDao.GetAll();
        foreach (var item in _listHocPhiSV)
            if (item.MaSV == masv && item.HocKy == hocky && item.Nam == nam)
                return true;

        return false;
    }

    public HocPhiSVDto GetByMaSVHKyNam(int masv, int hocky, string nam)
    {
        var rs = new HocPhiSVDto();
        _listHocPhiSV = _hocPhiSVDao.GetAll();
        foreach (var item in _listHocPhiSV)
            if (item.MaSV == masv && item.HocKy == hocky && item.Nam == nam)
            {
                rs = item;
                return rs;
            }

        return rs;
    }
}