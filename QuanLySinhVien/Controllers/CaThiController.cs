using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;

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
        if (_instance == null)
        {
            _instance = new CaThiController();
        }
        return _instance;
    }

    public List<CaThiDto> GetAll()
    {
        return _caThiDao.GetAll();
    }

    public bool Insert(CaThiDto caThiDto)
    {
        return _caThiDao.Insert(caThiDto);
    }

    public bool Update(CaThiDto caThiDto)
    {
        return _caThiDao.Update(caThiDto);
    }

    public bool Delete(int maCT)
    {
        return _caThiDao.Delete(maCT);
    }

    public CaThiDto GetById(int maCT)
    {
        return _caThiDao.GetById(maCT);
    }
}
