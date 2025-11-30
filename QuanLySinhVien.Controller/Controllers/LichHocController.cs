using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class LichHocController
{
    private static LichHocController _instance;
    private readonly LichHocDao _lichHocDao;
    private List<LichHocDto> _listLichHoc;

    private LichHocController()
    {
        _lichHocDao = LichHocDao.GetInstance();
        _listLichHoc = _lichHocDao.GetAll();
    }

    public static LichHocController GetInstance()
    {
        if (_instance == null) _instance = new LichHocController();
        return _instance;
    }

    public List<LichHocDto> GetAll()
    {
        return _lichHocDao.GetAll();
    }

    public bool Insert(LichHocDto lichHocDto)
    {
        return _lichHocDao.Insert(lichHocDto);
    }

    public bool Update(LichHocDto lichHocDto)
    {
        return _lichHocDao.Update(lichHocDto);
    }

    public bool Delete(int maLH)
    {
        return _lichHocDao.Delete(maLH);
    }

    public LichHocDto GetById(int maLH)
    {
        return _lichHocDao.GetById(maLH);
    }

    public List<LichHocDto> GetByMaNhp(int maNhp)
    {
        return _lichHocDao.GetByMaNhp(maNhp);
    }

    public bool HardDelete(int maLH)
    {
        return _lichHocDao.HardDelete(maLH);
    }

    public int GetAutoIncrement()
    {
        return _lichHocDao.GetAutoIncrement();
    }
}