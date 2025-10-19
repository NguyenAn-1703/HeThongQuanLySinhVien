using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class ChuongTrinhDaoTaoController
{
    private static ChuongTrinhDaoTaoController _instance;
    private readonly ChuongTrinhDaoTaoDao _chuongTrinhDaoTaoDao;
    private List<ChuongTrinhDaoTaoDto> _listChuongTrinh;

    private ChuongTrinhDaoTaoController()
    {
        _chuongTrinhDaoTaoDao = ChuongTrinhDaoTaoDao.GetInstance();
        _listChuongTrinh = _chuongTrinhDaoTaoDao.GetAll();
    }

    public static ChuongTrinhDaoTaoController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ChuongTrinhDaoTaoController();
        }
        return _instance;
    }

    public List<ChuongTrinhDaoTaoDto> GetAll()
    {
        return _chuongTrinhDaoTaoDao.GetAll();
    }

    public ChuongTrinhDaoTaoDto GetById(int id)
    {
        return _chuongTrinhDaoTaoDao.GetById(id);
    }

    public bool Insert(ChuongTrinhDaoTaoDto dto)
    {

        return _chuongTrinhDaoTaoDao.Insert(dto);
    }

    public bool Update(ChuongTrinhDaoTaoDto dto)
    {
        return _chuongTrinhDaoTaoDao.Update(dto);
    }

    public bool Delete(int id)
    {
        return _chuongTrinhDaoTaoDao.Delete(id);
    }

    public bool ValidateNganhChuKy(int maNganh, int maChuKy)
    {
        // Kiểm tra nếu danh sách trống
        if (_listChuongTrinh.Count == 0)
            return true;

        // Tìm xem có chương trình nào có cùng mã ngành và mã chu kỳ không
        bool isDuplicate = _listChuongTrinh.Any(ct =>
            (ct.MaNganh == maNganh) &&
            (ct.MaCKDT == maChuKy));

        return !isDuplicate;
    }

}
