using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;

public class NhomHocPhanController
{
    private static NhomHocPhanController _instance;
    private readonly NhomHocPhanDao _nhomHocPhanDao;
    private List<NhomHocPhanDto> _listNhomHocPhan;

    private NhomHocPhanController()
    {
        _nhomHocPhanDao = NhomHocPhanDao.GetInstance();
        _listNhomHocPhan = _nhomHocPhanDao.GetAll();
    }

    public static NhomHocPhanController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new NhomHocPhanController();
        }
        return _instance;
    }

    public List<NhomHocPhanDto> GetAll()
    {
        return _nhomHocPhanDao.GetAll();
    }

    public bool Insert(NhomHocPhanDto dto)
    {
        return _nhomHocPhanDao.Insert(dto);
    }

    public bool Update(NhomHocPhanDto dto)
    {
        return _nhomHocPhanDao.Update(dto);
    }

    public bool Delete(int maNHP)
    {
        return _nhomHocPhanDao.Delete(maNHP);
    }

    public NhomHocPhanDto GetById(int maNHP)
    {
        return _nhomHocPhanDao.GetById(maNHP);
    }

    public List<NhomHocPhanDto> GetByLichMaDangKy(int maLichDk)
    {
        return _nhomHocPhanDao.GetByLichMaDangKy(maLichDk);
    }

    public List<NhomHocPhanDto> GetByHkyNamMaHP(int hky, string nam, int maHP)
    {
        return _nhomHocPhanDao.GetByHkyNamMaHP(hky, nam,  maHP);
    }

    public List<NhomHocPhanDto> GetByHkyNam(int hky, string nam)
    {
        return _nhomHocPhanDao.GetByHkyNam(hky, nam);
    }
    
    public List<NhomHocPhanDto> GetByHkyNamMaGV(int hky, string nam, int maGV)
    {
        return _nhomHocPhanDao.GetByHkyNamMaGV(hky, nam, maGV);
    }
    
}
