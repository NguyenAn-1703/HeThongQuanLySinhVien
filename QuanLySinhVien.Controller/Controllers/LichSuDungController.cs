using QuanLySinhVien.Model.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class LichSuDungController
{
    private static LichSuDungController _instance;
    private readonly LichSuDungDao _lichSuDungDao;

    private List<LichSuDungDto> _listLichSuDung;

    private LichSuDungController()
    {
        _lichSuDungDao = LichSuDungDao.GetInstance();
        _listLichSuDung = _lichSuDungDao.GetAll();
    }

    public static LichSuDungController GetInstance()
    {
        if (_instance == null) _instance = new LichSuDungController();
        return _instance;
    }

    public List<LichSuDungDto> GetAll()
    {
        return _lichSuDungDao.GetAll();
    }

    public LichSuDungDto GetById(int maLSD)
    {
        return _lichSuDungDao.GetById(maLSD);
    }

    public bool Insert(LichSuDungDto dto)
    {
        bool rs = _lichSuDungDao.Insert(dto);
        _listLichSuDung = _lichSuDungDao.GetAll();
        return rs;
    }

    public bool Update(LichSuDungDto dto)
    {
        bool rs = _lichSuDungDao.Update(dto);
        _listLichSuDung = _lichSuDungDao.GetAll();
        return rs;
    }

    public bool Delete(int maLSD)
    {
        bool rs = _lichSuDungDao.Delete(maLSD);
        _listLichSuDung = _lichSuDungDao.GetAll();
        return rs;
    }

    public bool ExistByMaPhong(int maPhong)
    {
        foreach (LichSuDungDto dto in _listLichSuDung)
        {
            if (dto.MaPH == maPhong)
            {
                return true;
            }
        }
        return false;
    }
    
    public List<LichSuDungDto> GetDsLichSdByMaPh(int maPh)
    {
        List<LichSuDungDto> listLich = new List<LichSuDungDto>();
        foreach (LichSuDungDto dto in _listLichSuDung)
        {
            if (dto.MaPH == maPh)
            {
                listLich.Add(dto);
            }
        }
        return listLich;
    }
    
    public int GetAutoIncrement()
    {
        return _lichSuDungDao.GetAutoIncrement();
    }

    public int GetLastId()
    {
        _listLichSuDung = _lichSuDungDao.GetAll();
        int lastId = _listLichSuDung.Count - 1;
        return _listLichSuDung[lastId].MaLSD;
    }
}
