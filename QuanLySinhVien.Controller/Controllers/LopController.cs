using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Structs;

namespace QuanLySinhVien.Controller.Controllers;

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
        if (_instance == null) _instance = new LopController();
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

    public LopDto GetLopById(int maLop)
    {
        return _lopDao.GetLopById(maLop);
    }

    public LopDto GetByTen(string tenLop)
    {
        return _lopDao.GetByTen(tenLop);
    }

    public bool ExistByTen(string ten)
    {
        var listPh = GetAll();
        foreach (var item in listPh)
            if (item.TenLop.Equals(ten))
                return true;

        return false;
    }

    public ValidateResult Validate(
        string tenLop)
    {
        ValidateResult rs = new ValidateResult
        {
            index = -1,
            message = "",
        };
        if (Shared.Validate.IsEmpty(tenLop))
        {
            rs.index = 0;
            rs.message = "Tên lớp không được để trống!";
            return rs;
        }

        return rs;
    }
    
    public bool ExistById(int id)
    {
        _listLop = _lopDao.GetAll();
        foreach (var item in _listLop)
        {
            if (item.MaLop == id) return true;
        }

        return false;
    }
}