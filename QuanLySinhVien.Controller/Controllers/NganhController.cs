using QuanLySinhVien.Model.DAO;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.DTO.ThongKe;
using QuanLySinhVien.Shared.Structs;

namespace QuanLySinhVien.Controller.Controllers;

public class NganhController
{
    private static NganhController _instance;
    private readonly NganhDao _nganhDao;
    private List<NganhDto> _listNganh;
    
    
    private LopController _lopController;
    private SinhVienController _sinhVienController;
    private KetQuaController _ketQuaController;

    private Dictionary<int, int> svGioiXuatSacDic;
    private Dictionary<int, int> svGioiXuatSaDic;
    

    // Constructor riêng tư (Singleton)
    private NganhController()
    {
        _nganhDao = NganhDao.GetInstance();
        _listNganh = _nganhDao.GetAll();
    }


    // Lấy instance duy nhất
    public static NganhController GetInstance()
    {
        if (_instance == null) _instance = new NganhController();

        return _instance;
    }

    // Lấy danh sách tất cả ngành
    public List<NganhDto> GetAll()
    {
        return _nganhDao.GetAll();
    }

    // Thêm ngành mới
    public bool Insert(NganhDto nganhDto)
    {
        return _nganhDao.Insert(nganhDto);
    }

    // Cập nhật ngành
    public bool Update(NganhDto nganhDto)
    {
        return _nganhDao.Update(nganhDto);
    }

    // Xóa ngành theo ID
    public bool Delete(int idNganh)
    {
        return _nganhDao.Delete(idNganh);
    }

    // Lấy thông tin ngành theo ID
    public NganhDto GetNganhById(int id)
    {
        return _nganhDao.GetNganhById(id);
    }

    public NganhDto GetByTen(string ten)
    {
        return _nganhDao.GetByTen(ten);
    }

    public int GetLastAutoIncrement()
    {
        _listNganh = _nganhDao.GetAll();
        var lastIndex = _listNganh.Count - 1;
        return _listNganh[lastIndex].MaNganh;
    }
    
    public ValidateResult Validate(
        string tenNganh, string hocPhi)
    {
        ValidateResult rs = new ValidateResult
        {
            index = -1,
            message = "",
        };
        if (Shared.Validate.IsEmpty(tenNganh))
        {
            rs.index = 0;
            rs.message = "Tên ngành không được để trống!";
            return rs;
        }

        if (Shared.Validate.IsEmpty(hocPhi))
        {
            rs.index = 1;
            rs.message = "Học phí không được để trống!";
            return rs;
        }
        

        return rs;
    }
    
   
}