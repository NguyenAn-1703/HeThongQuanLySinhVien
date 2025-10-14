using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class ChuKyDaoTaoController
{
    private static ChuKyDaoTaoController _instance;
    private readonly ChuKyDaoTaoDao _chuKyDaoTaoDao;
    private List<ChuKyDaoTaoDto> _listChuKyDaoTao;

    private ChuKyDaoTaoController()
    {
        _chuKyDaoTaoDao = ChuKyDaoTaoDao.GetInstance();
        _listChuKyDaoTao = _chuKyDaoTaoDao.GetAll();
    }

    public static ChuKyDaoTaoController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ChuKyDaoTaoController();
        }
        return _instance;
    }

    public List<ChuKyDaoTaoDto> GetAll()
    {
        return _chuKyDaoTaoDao.GetAll();
    }

    public bool Insert(ChuKyDaoTaoDto dto)
    {
        return _chuKyDaoTaoDao.Insert(dto);
    }

    public bool Update(ChuKyDaoTaoDto dto)
    {
        return _chuKyDaoTaoDao.Update(dto);
    }

    public bool Delete(int maCKDT)
    {
        return _chuKyDaoTaoDao.Delete(maCKDT);
    }

    public ChuKyDaoTaoDto GetById(int maCKDT)
    {
        return _chuKyDaoTaoDao.GetById(maCKDT);
    }
}
