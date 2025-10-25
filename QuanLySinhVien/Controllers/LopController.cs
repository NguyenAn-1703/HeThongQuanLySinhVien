using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class LopController
{
    private static LopController _instance;
    private readonly LopDAO _lopDao;
    private List<LopDto> _listLop;

    private LopController()
    {
        _lopDao = LopDAO.GetInstance();
        _listLop = _lopDao.GetAll();
    }

    public static LopController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new LopController();
        }
        return _instance;
    }

    public List<LopDto> GetAll()
    {
        return _lopDao.GetAll();
    }

    public bool Insert(LopDto lopDto)
    {
        return _lopDao.Insert(lopDto);
    }

    public bool Update(LopDto lopDto)
    {
        return _lopDao.Update(lopDto);
    }

    public bool Delete(int maLop)
    {
        return _lopDao.Delete(maLop);
    }

    public LopDto? GetLopById(int maLop)
    {
        return _lopDao.GetLopById(maLop);
    }
}
