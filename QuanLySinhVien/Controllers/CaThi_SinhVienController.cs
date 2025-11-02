using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;

public class CaThi_SinhVienController
{
    private static CaThi_SinhVienController _instance;
    private readonly CaThi_SinhVienDao _caThiSinhVienDao;
    private List<CaThi_SinhVienDto> _listCaThiSinhVien;
    private CaThiController _caThiController;

    private CaThi_SinhVienController()
    {
        _caThiSinhVienDao = CaThi_SinhVienDao.GetInstance();
        _listCaThiSinhVien = _caThiSinhVienDao.GetAll();
        _caThiController = CaThiController.GetInstance();
    }

    public static CaThi_SinhVienController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new CaThi_SinhVienController();
        }
        return _instance;
    }

    public List<CaThi_SinhVienDto> GetAll()
    {
        return _caThiSinhVienDao.GetAll();
    }

    public bool Insert(CaThi_SinhVienDto caThiSv)
    {
        bool result = _caThiSinhVienDao.Insert(caThiSv);
        if (result)
            _listCaThiSinhVien = _caThiSinhVienDao.GetAll();
        return result;
    }

    public bool Delete(int maCT, int maSV)
    {
        bool result = _caThiSinhVienDao.Delete(maCT, maSV);
        if (result)
            _listCaThiSinhVien = _caThiSinhVienDao.GetAll();
        return result;
    }

    public CaThi_SinhVienDto GetById(int maCT, int maSV)
    {
        return _listCaThiSinhVien
            .First(x => x.MaCT == maCT && x.MaSV == maSV);
    }

    public List<CaThi_SinhVienDto> GetByMaCT(int maCT)
    {
        return _listCaThiSinhVien
            .Where(x => x.MaCT == maCT)
            .ToList();
    }
    
    public bool HardDeleteByMaCT(int maCT)
    {
        bool result = _caThiSinhVienDao.HardDeleteByMaCT(maCT);
        if (result)
            _listCaThiSinhVien = _caThiSinhVienDao.GetAll();
        return result;
    }

    public bool ExistSVThiHp(int maHP, int maSV)
    {
        _listCaThiSinhVien = _caThiSinhVienDao.GetAll();

        foreach (CaThi_SinhVienDto ctsv in _listCaThiSinhVien)
        {
            if (ctsv.MaSV == maSV)
            {
                // sửa lỗi không có ca thi
                if (_caThiController.ExistById(ctsv.MaCT))
                {
                    CaThiDto caThi = _caThiController.GetById(ctsv.MaCT);
                    if (caThi.MaHP == maHP)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
