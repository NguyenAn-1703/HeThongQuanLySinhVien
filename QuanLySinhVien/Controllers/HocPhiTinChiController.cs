using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;

public class HocPhiTinChiController
{
    private static HocPhiTinChiController _instance;
    private readonly HocPhiTinChiDao _hocPhiTinChiDao;
    private List<HocPhiTinChiDto> _listHocPhiTinChi;

    private HocPhiTinChiController()
    {
        _hocPhiTinChiDao = HocPhiTinChiDao.GetInstance();
        _listHocPhiTinChi = _hocPhiTinChiDao.GetAll();
    }

    public static HocPhiTinChiController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new HocPhiTinChiController();
        }
        return _instance;
    }

    public List<HocPhiTinChiDto> GetAll()
    {
        return _hocPhiTinChiDao.GetAll();
    }

    public bool Insert(HocPhiTinChiDto hocPhiTinChiDto)
    {
        return _hocPhiTinChiDao.Insert(hocPhiTinChiDto);
    }

    public bool Update(HocPhiTinChiDto hocPhiTinChiDto)
    {
        return _hocPhiTinChiDao.Update(hocPhiTinChiDto);
    }

    public bool Delete(int maHPTC)
    {
        return _hocPhiTinChiDao.Delete(maHPTC);
    }

    public HocPhiTinChiDto GetById(int maHPTC)
    {
        return _hocPhiTinChiDao.GetById(maHPTC);
    }

    public List<HocPhiTinChiDto> GetByMaNganh(int maNganh)
    {
        return _hocPhiTinChiDao.GetByMaNganh(maNganh);
    }
    
    public HocPhiTinChiDto GetNewestHocPhiTinChiByMaNganh(int maNganh)
    {
        _listHocPhiTinChi  = GetByMaNganh(maNganh);
        HocPhiTinChiDto newest = _listHocPhiTinChi[0];
        foreach (var item in _listHocPhiTinChi)
        {
            DateTime itemTime = ConvertDate.ConvertStringToDateTime(item.ThoiGianApDung);
            DateTime newestTime = ConvertDate.ConvertStringToDateTime(newest.ThoiGianApDung);
            if (itemTime > newestTime)
            {
                newest = item;
            }
        }

        return newest;
    }
}
