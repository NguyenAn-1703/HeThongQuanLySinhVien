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
        return _hocPhanDao.GetAll();
    }

    public bool Insert(HocPhanDto hocPhanDto)
    {
        return _hocPhanDao.Insert(hocPhanDto);
    }

    public bool Update(HocPhanDto hocPhanDto)
    {
        return _hocPhanDao.Update(hocPhanDto);
    }

    public bool Delete(int idHocPhan)
    {
        return _hocPhanDao.Delete(idHocPhan);
    }

    public HocPhanDto GetHocPhanById(int id)
    {
        return _hocPhanDao.GetHocPhanById(id);
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