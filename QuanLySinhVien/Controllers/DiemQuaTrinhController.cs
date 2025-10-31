using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;

public class DiemQuaTrinhController
{
    private static DiemQuaTrinhController _instance;
    private readonly DiemQuaTrinhDao _diemQuaTrinhDao;
    private List<DiemQuaTrinhDto> _listDiemQuaTrinh;

    private DiemQuaTrinhController()
    {
        _diemQuaTrinhDao = DiemQuaTrinhDao.GetInstance();
        _listDiemQuaTrinh = _diemQuaTrinhDao.GetAll();
    }

    public static DiemQuaTrinhController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DiemQuaTrinhController();
        }
        return _instance;
    }

    public List<DiemQuaTrinhDto> GetAll()
    {
        return _diemQuaTrinhDao.GetAll();
    }

    public bool Insert(DiemQuaTrinhDto dto)
    {
        return _diemQuaTrinhDao.Insert(dto);
    }

    public bool Update(DiemQuaTrinhDto dto)
    {
        return _diemQuaTrinhDao.Update(dto);
    }

    public bool Delete(int maDQT)
    {
        return _diemQuaTrinhDao.Delete(maDQT);
    }

    public DiemQuaTrinhDto GetById(int maDQT)
    {
        return _diemQuaTrinhDao.GetDiemQuaTrinhById(maDQT);
    }

    public DiemQuaTrinhDto GetByMaKQ(int maKQ)
    {
        DiemQuaTrinhDto rs = new DiemQuaTrinhDto();
        _listDiemQuaTrinh =  _diemQuaTrinhDao.GetAll();
        foreach (DiemQuaTrinhDto dto in _listDiemQuaTrinh)
        {
            if (dto.MaKQ == maKQ)
            {
                rs = dto;
                return rs;
            }
        }
        return rs;
    }
    
    public bool ExistsByMaKQ(int maKQ)
    {
        _listDiemQuaTrinh =  _diemQuaTrinhDao.GetAll();
        foreach (DiemQuaTrinhDto dto in _listDiemQuaTrinh)
        {
            if (dto.MaKQ == maKQ)
            {
                return true;
            }
        }
        return false;
    }
}
