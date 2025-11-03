using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class HocPhanController
{
    private static HocPhanController _instance;
    private readonly HocPhanDao _hocPhanDao;
    private List<HocPhanDto> _listHocPhan;

    private HocPhanController()
    {
        _hocPhanDao = HocPhanDao.GetInstance();
        _listHocPhan = _hocPhanDao.GetAll();
    }

    public static HocPhanController GetInstance()
    {
        if (_instance == null) _instance = new HocPhanController();

        return _instance;
    }

    public List<HocPhanDto> GetAll()
    {
        _listHocPhan = _hocPhanDao.GetAll();
        return _listHocPhan;
    }

    public bool Insert(HocPhanDto hocPhanDto)
    {
        var result = _hocPhanDao.Insert(hocPhanDto);
        if (result)
            _listHocPhan = _hocPhanDao.GetAll();
        return result;
    }

    public bool Update(HocPhanDto hocPhanDto)
    {
        var result = _hocPhanDao.Update(hocPhanDto);
        if (result)
            _listHocPhan = _hocPhanDao.GetAll();
        return result;
    }

    public bool Delete(int idHocPhan)
    {
        var result = _hocPhanDao.Delete(idHocPhan);
        if (result)
            _listHocPhan = _hocPhanDao.GetAll();
        return result;
    }

    public HocPhanDto GetHocPhanById(int id)
    {
        return _listHocPhan.First(hp => hp.MaHP == id);
    }


    public bool ExistByTen(string ten)
    {
        var listPh = GetAll();
        foreach (var item in listPh)
            if (item.TenHP.Equals(ten))
                return true;

        return false;
    }

    public HocPhanDto GetHocPhanByTen(string ten)
    {
        return _hocPhanDao.GetHocPhanByTen(ten);
    }
}