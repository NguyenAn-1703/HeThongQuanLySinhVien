using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class LichThiController
{
    private static LichThiController _instance;
    private readonly LichThiDao _lichThiDao;

    public LichThiController()
    {
        _lichThiDao = new LichThiDao();
    }

    public static LichThiController GetInstance()
    {
        if (_instance == null) _instance = new LichThiController();
        return _instance;
    }

    // Lấy toàn bộ lịch thi của SV
    public List<LichThiSVDto> GetLichThiCaNhan(int maSV)
    {
        return _lichThiDao.GetLichThiBySinhVien(maSV);
    }

    // Lấy danh sách học kỳ để filter
    public List<string> GetListHocKyFilter(int maSV)
    {
        return _lichThiDao.GetListHocKyNam(maSV);
    }
}