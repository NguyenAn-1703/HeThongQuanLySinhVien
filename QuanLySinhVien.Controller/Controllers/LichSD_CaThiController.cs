using QuanLySinhVien.Model.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class LichSD_CaThiController
{
    private static LichSD_CaThiController _instance;
    private readonly LichSD_CaThiDao _lichSDCaThiDao;

    private List<LichSD_CaThiDto> _listLichSDCaThi;

    private LichSD_CaThiController()
    {
        _lichSDCaThiDao = LichSD_CaThiDao.GetInstance();
        _listLichSDCaThi = _lichSDCaThiDao.GetAll();
    }

    public static LichSD_CaThiController GetInstance()
    {
        if (_instance == null) _instance = new LichSD_CaThiController();
        return _instance;
    }

    // ============================================
    // GET ALL
    // ============================================
    public List<LichSD_CaThiDto> GetAll()
    {
        return _lichSDCaThiDao.GetAll();
    }

    // ============================================
    // INSERT
    // ============================================
    public bool Insert(LichSD_CaThiDto dto)
    {
        var success = _lichSDCaThiDao.Insert(dto);
        if (success) _listLichSDCaThi.Add(dto);
        return success;
    }
    
    // ============================================
    // DELETE
    // ============================================
    public bool Delete(int maLSD, int maCT)
    {
        var success = _lichSDCaThiDao.Delete(maLSD, maCT);
        if (success)
        {
            _listLichSDCaThi.RemoveAll(x => x.MaLSD == maLSD && x.MaCT == maCT);
        }
        return success;
    }

    // ============================================
    // GET BY ID
    // ============================================
    public LichSD_CaThiDto GetById(int maLSD, int maCT)
    {
        return _lichSDCaThiDao.GetById(maLSD, maCT);
    }
}
