using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class HocPhiHocPhanController
{
    private static HocPhiHocPhanController _instance;
    private readonly HocPhiHocPhanDao _hocPhiHocPhanDao;
    private List<HocPhiHocPhanDto> _listHocPhiHocPhan;

    private HocPhiHocPhanController()
    {
        _hocPhiHocPhanDao = HocPhiHocPhanDao.GetInstance();
        _listHocPhiHocPhan = _hocPhiHocPhanDao.GetAll();
    }

    public static HocPhiHocPhanController GetInstance()
    {
        if (_instance == null) _instance = new HocPhiHocPhanController();
        return _instance;
    }

    public List<HocPhiHocPhanDto> GetAll()
    {
        return _hocPhiHocPhanDao.GetAll();
    }

    public bool Insert(HocPhiHocPhanDto hocPhiHocPhanDto)
    {
        return _hocPhiHocPhanDao.Insert(hocPhiHocPhanDto);
    }

    public bool Update(HocPhiHocPhanDto hocPhiHocPhanDto)
    {
        return _hocPhiHocPhanDao.Update(hocPhiHocPhanDto);
    }

    public bool DeleteByMaSVMaHP(int maSV, int maHP)
    {
        return _hocPhiHocPhanDao.Delete(maSV, maHP);
    }

    public HocPhiHocPhanDto GetByMaSVMaHP(int maSV, int maHP)
    {
        return _hocPhiHocPhanDao.GetById(maSV, maHP);
    }

    public List<HocPhiHocPhanDto> GetByMaSVHocKyNam(int maSV, int hocKy, string nam)
    {
        return _hocPhiHocPhanDao.GetByMaSVHocKyNam(maSV, hocKy, nam);
    }
    
    public List<HocPhiHocPhanDao.NamHocKy> GetNamHocKySVDaDK(int maSV)
    {
        return _hocPhiHocPhanDao.GetNamHocKySVDaDK(maSV);
    }
}