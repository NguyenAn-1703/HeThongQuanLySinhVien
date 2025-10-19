using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;

public class ChuongTrinhDaoTao_HocPhanController
{
    private static ChuongTrinhDaoTao_HocPhanController _instance;
    private readonly ChuongTrinhDaoTao_HocPhanDao _dao;
    private readonly List<ChuongTrinhDaoTao_HocPhanDto> _listChuongTrinh_HocPhan;

    private ChuongTrinhDaoTao_HocPhanController()
    {
        _dao = ChuongTrinhDaoTao_HocPhanDao.GetInstance();
        _listChuongTrinh_HocPhan = _dao.GetAll();
    }

    public static ChuongTrinhDaoTao_HocPhanController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ChuongTrinhDaoTao_HocPhanController();
        }
        return _instance;
    }

    public List<ChuongTrinhDaoTao_HocPhanDto> GetAll()
    {
        return _dao.GetAll();
    }

    public bool Insert(ChuongTrinhDaoTao_HocPhanDto dto)
    {
        return _dao.Insert(dto);
    }

    public bool Delete(int maCTDT, int maHP)
    {
        return _dao.Delete(maCTDT, maHP);
    }

    public ChuongTrinhDaoTao_HocPhanDto GetById(int maCTDT, int maHP)
    {
        return _dao.GetById(maCTDT, maHP);
    }

    public List<ChuongTrinhDaoTao_HocPhanDto> GetByMaCTDT(int MaCTDT)
    {
        return _dao.GetByMaCTDT(MaCTDT);
    }
    
}
