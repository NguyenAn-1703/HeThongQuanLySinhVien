using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class CaThiController
{
    private static CaThiController _instance;
    private readonly CaThiDao _caThiDao;
    private List<CaThiDto> _listCaThi;

    private CaThiController()
    {
        _caThiDao = CaThiDao.GetInstance();
        _listCaThi = _caThiDao.GetAll();
    }

    public static CaThiController GetInstance()
    {
        if (_instance == null) _instance = new CaThiController();
        return _instance;
    }

    public List<CaThiDto> GetAll()
    {
        return _caThiDao.GetAll();
    }

    public bool Insert(CaThiDto caThiDto)
    {
        var result = _caThiDao.Insert(caThiDto);
        if (result)
            _listCaThi = _caThiDao.GetAll();
        return result;
    }

    public bool Update(CaThiDto caThiDto)
    {
        var result = _caThiDao.Update(caThiDto);
        if (result)
            _listCaThi = _caThiDao.GetAll();
        return result;
    }

    public bool Delete(int maCT)
    {
        var result = _caThiDao.Delete(maCT);
        if (result)
            _listCaThi = _caThiDao.GetAll();
        return result;
    }

    public CaThiDto GetById(int maCT)
    {
        return _listCaThi.First(ct => ct.MaCT == maCT);
    }

    public List<CaThiDto> GetByHocKyNam(int hky, string nam)
    {
        return _caThiDao.GetByHocKyNam(hky, nam);
    }

    public int GetLastId()
    {
        _listCaThi = _caThiDao.GetAll();
        return _listCaThi[_listCaThi.Count - 1].MaCT;
    }

    public bool ExistById(int id)
    {
        _listCaThi = _caThiDao.GetAll();
        foreach (var item in _listCaThi)
            if (item.MaCT == id)
                return true;
        return false;
    }
}