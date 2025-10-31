using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class CotDiemController
{
    private static CotDiemController _instance;
    private readonly CotDiemDao _cotDiemDao;
    private List<CotDiemDto> _listCotDiem;

    private CotDiemController()
    {
        _cotDiemDao = CotDiemDao.GetInstance();
        _listCotDiem = _cotDiemDao.GetAll();
    }

    public static CotDiemController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new CotDiemController();
        }
        return _instance;
    }

    public List<CotDiemDto> GetAll()
    {
        return _cotDiemDao.GetAll();
    }

    public bool Insert(CotDiemDto cotDiemDto)
    {
        return _cotDiemDao.Insert(cotDiemDto);
    }

    public bool Update(CotDiemDto cotDiemDto)
    {
        return _cotDiemDao.Update(cotDiemDto);
    }

    public bool Delete(int maCD)
    {
        return _cotDiemDao.Delete(maCD);
    }

    public CotDiemDto GetCotDiemById(int maCD)
    {
        return _cotDiemDao.GetCotDiemById(maCD);
    }

    public List<CotDiemDto> GetByMaDQT(int MaDQT)
    {
        List<CotDiemDto> rs = new List<CotDiemDto>(); 
        _listCotDiem = _cotDiemDao.GetAll();
        foreach (CotDiemDto item in _listCotDiem)
        {
            if (item.MaDQT == MaDQT)
            {
                rs.Add(item);
            }
        }

        return rs;
    }

    public bool HardDelete(int MaCD)
    {
        return _cotDiemDao.HardDelete(MaCD);
    }
}
