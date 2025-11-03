using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class DangKyController
{
    private static DangKyController _instance;
    private readonly DangKyDao _dangKyDao;
    private readonly NhomHocPhanController _nhomHocPhanController;
    private List<DangKyDto> _listDangKy;

    private DangKyController()
    {
        _dangKyDao = DangKyDao.GetInstance();
        _listDangKy = _dangKyDao.GetAll();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
    }

    public static DangKyController GetInstance()
    {
        if (_instance == null) _instance = new DangKyController();
        return _instance;
    }

    public List<DangKyDto> GetAll()
    {
        return _dangKyDao.GetAll();
    }

    public bool Insert(DangKyDto dangKyDto)
    {
        return _dangKyDao.Insert(dangKyDto);
    }

    public bool Delete(int maNHP, int maSV)
    {
        return _dangKyDao.Delete(maNHP, maSV);
    }

    public List<DangKyDto> GetByMaNHP(int maNHP)
    {
        return _dangKyDao.GetByMaNHP(maNHP);
    }

    public List<DangKyDto> GetByMaSV(int maSV)
    {
        return _dangKyDao.GetByMaSV(maSV);
    }

    public bool HardDelete(int maNhp, int maSV)
    {
        return _dangKyDao.HardDelete(maNhp, maSV);
    }

    //Hky, Nam, MaHP -> listMaSV
    public List<DangKyDto> GetListByHkyNamMaHP(int hky, string nam, int maHp)
    {
        var listNhp = _nhomHocPhanController.GetByHkyNamMaHP(hky, nam, maHp);
        var listDangKy = new List<DangKyDto>();

        for (var i = 0; i < listNhp.Count; i++)
        {
            var temp = GetByMaNHP(listNhp[i].MaNHP);
            listDangKy.AddRange(temp);
        }

        return listDangKy;
    }
}