using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class ChiTietQuyenController
{
    private static ChiTietQuyenController _instance;
    private readonly ChiTietQuyenDao _ChiTietQuyenDao;
    private List<ChiTietQuyenDto> _listChiTietQuyen;

    private ChiTietQuyenController()
    {
        _ChiTietQuyenDao = ChiTietQuyenDao.GetInstance();
        _listChiTietQuyen = _ChiTietQuyenDao.GetAll();
    }

    public static ChiTietQuyenController getInstance()
    {
        if (_instance == null)
        {
            _instance = new ChiTietQuyenController();
        }
        return _instance;
    }

    public List<ChiTietQuyenDto> GetAll()
    {
        return _ChiTietQuyenDao.GetAll();
    }

    public bool Insert(ChiTietQuyenDto ChiTietQuyenDto)
    {
        return (_ChiTietQuyenDao.Insert(ChiTietQuyenDto));
    }


    public bool Delete(int MaNQ, int MaCN, string hanhDong)
    {
        return _ChiTietQuyenDao.Delete(MaNQ, MaCN, hanhDong);
    }

    public ChiTietQuyenDto GetById(int MaNQ, int MaCN, string hanhDong)
    {
        return _ChiTietQuyenDao.GetById(MaNQ, MaCN, hanhDong);
    }

    public List<ChiTietQuyenDto> GetByMaNQMaCN(int MaNQ, int MaCN)
    {
        return _ChiTietQuyenDao.GetByMaNQMaCN(MaNQ, MaCN);
    }

    public bool HardDelete(int MaNQ, int MaCN, string hanhDong)
    {
        return _ChiTietQuyenDao.HardDelete(MaNQ, MaCN, hanhDong);
    }

    public bool DeleteAllCTQ(int maNQ)
    {
        return _ChiTietQuyenDao.DeleteAllCTQ(maNQ);
    }

    public List<ChiTietQuyenDto> GetByMaNQ(int maNQ)
    {
        return _ChiTietQuyenDao.GetByMaNQ(maNQ);
    }
}