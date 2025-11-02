using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

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
        if (_instance == null)
        {
            _instance = new HocPhanController();
        }

        return _instance;
    }

    public List<HocPhanDto> GetAll()
    {
        _listHocPhan = _hocPhanDao.GetAll();
        return _listHocPhan;
    }

    public bool Insert(HocPhanDto hocPhanDto)
    {
        bool result = _hocPhanDao.Insert(hocPhanDto);
        if (result)
            _listHocPhan = _hocPhanDao.GetAll();
        return result;
    }

    public bool Update(HocPhanDto hocPhanDto)
    {
        bool result = _hocPhanDao.Update(hocPhanDto);
        if (result)
            _listHocPhan = _hocPhanDao.GetAll();
        return result;
    }

    public bool Delete(int idHocPhan)
    {
        bool result = _hocPhanDao.Delete(idHocPhan);
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
        List<HocPhanDto> listPh = GetAll();
        foreach (HocPhanDto item in listPh)
        {
            if (item.TenHP.Equals(ten))
            {
                return true;
            }
        }

        return false;
    }

    public HocPhanDto GetHocPhanByTen(string ten)
    {
        return _hocPhanDao.GetHocPhanByTen(ten);
    }
}