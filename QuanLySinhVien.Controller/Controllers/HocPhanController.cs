using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Structs;

namespace QuanLySinhVien.Controller.Controllers;

public class HocPhanController
{
    private static HocPhanController _instance;
    private readonly HocPhanDao _hocPhanDao;
    private List<HocPhanDto> _listHocPhan;

    private HocPhanController()
    {
        _hocPhanDao = HocPhanDao.GetInstance();
        _listHocPhan = _hocPhanDao.GetAll();
    }

    public static HocPhanController GetInstance()
    {
        if (_instance == null) _instance = new HocPhanController();

        return _instance;
    }

    public List<HocPhanDto> GetAll()
    {
        _listHocPhan = _hocPhanDao.GetAll();
        return _listHocPhan;
    }

    public bool Insert(HocPhanDto hocPhanDto)
    {
        var result = _hocPhanDao.Insert(hocPhanDto);
        if (result)
            _listHocPhan = _hocPhanDao.GetAll();
        return result;
    }

    public bool Update(HocPhanDto hocPhanDto)
    {
        var result = _hocPhanDao.Update(hocPhanDto);
        if (result)
            _listHocPhan = _hocPhanDao.GetAll();
        return result;
    }

    public bool Delete(int idHocPhan)
    {
        var result = _hocPhanDao.Delete(idHocPhan);
        if (result)
            _listHocPhan = _hocPhanDao.GetAll();
        return result;
    }

    public HocPhanDto GetHocPhanById(int id)
    {
        return _listHocPhan.First(hp => hp.MaHP == id);
    }


    public bool ExistByTen(string ten)
    {
        var listPh = GetAll();
        foreach (var item in listPh)
            if (item.TenHP.Equals(ten))
                return true;

        return false;
    }

    public HocPhanDto GetHocPhanByTen(string ten)
    {
        return _hocPhanDao.GetHocPhanByTen(ten);
    }
    
    public ValidateResult Validate(
        string tenHP, string soTinChiText, string heSoHPText,
        string soTietLTText, string soTietTHText)
    {
        ValidateResult rs = new ValidateResult
        {
            index = -1,
            message = "",
        };

        if (Shared.Validate.IsEmpty(tenHP))
        {
            rs.index = 0;
            rs.message = "Tên học phần không được để trống!";
            return rs;
        }

        if (!int.TryParse(soTinChiText, out _))
        {
            rs.index = 1;
            rs.message = "Số tín chỉ phải là số!";
            return rs;
        }

        if (!Shared.Validate.IsValidHeSo(heSoHPText))
        {
            rs.index = 2;
            rs.message = "Hệ số điểm không hợp lệ!";
            return rs;
        }

        if (!int.TryParse(soTietLTText, out _))
        {
            rs.index = 3;
            rs.message = "Số tiết lý thuyết phải là số!";
            return rs;
        }

        if (!int.TryParse(soTietTHText, out _))
        {
            rs.index = 4;
            rs.message = "Số tiết thực hành phải là số!";
            return rs;
        }
        
        return rs;
    }

    public List<HocPhanDto> GetListHocPhanByMaKhoa(int maKhoa)
    {
        _listHocPhan = _hocPhanDao.GetAll();
        List<HocPhanDto> result = new List<HocPhanDto>();
        foreach (var item in _listHocPhan)
        {
            if (item.MaKhoa == maKhoa)
            {
                result.Add(item);
            }
        }

        return result;
    }
    

}