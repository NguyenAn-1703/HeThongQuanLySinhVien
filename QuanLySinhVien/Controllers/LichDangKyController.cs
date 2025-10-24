using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;

public class LichDangKyController
{
    private static LichDangKyController _instance;
    private readonly LichDangKyDao _lichDangKyDao;
    private List<LichDangKyDto> _listLichDangKy;

    private LichDangKyController()
    {
        _lichDangKyDao = LichDangKyDao.GetInstance();
        _listLichDangKy = _lichDangKyDao.GetAll();
    }

    public static LichDangKyController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new LichDangKyController();
        }
        return _instance;
    }

    public List<LichDangKyDto> GetAll()
    {
        return _lichDangKyDao.GetAll();
    }

    public bool Insert(LichDangKyDto lichDangKyDto)
    {
        return _lichDangKyDao.Insert(lichDangKyDto);
    }

    public bool Update(LichDangKyDto lichDangKyDto)
    {
        return _lichDangKyDao.Update(lichDangKyDto);
    }

    public bool Delete(int maLichDK)
    {
        return _lichDangKyDao.Delete(maLichDK);
    }

    public LichDangKyDto GetById(int maLichDK)
    {
        return _lichDangKyDao.GetById(maLichDK);
    }
}
