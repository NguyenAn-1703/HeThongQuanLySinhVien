using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;

namespace QuanLySinhVien.Controller.Controllers;

public class SinhVienController
{
    private static SinhVienController _instance;
    private readonly HocPhiSVController _HocPhiSVController;
    private readonly KhoaController _khoaController;
    private readonly LopController _lopController;
    private readonly NganhController _nganhController;

    private Dictionary<int, string> _lopDic;
    private Dictionary<int, string> _nganhDic;
    
    
    public LopDAO LopDao;
    public NganhDao NganhDao;
    public SinhVienDAO SinhVienDao;

    private SinhVienController()
    {
        SinhVienDao = new SinhVienDAO();
        NganhDao = NganhDao.GetInstance();
        LopDao = LopDAO.GetInstance();
        _lopController = LopController.GetInstance();
        _nganhController = NganhController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        _HocPhiSVController = HocPhiSVController.GetInstance();

        InitLookupData();
    }

    void InitLookupData()
    {
        var listLop = _lopController.GetAll();
        var listNganh = _nganhController.GetAll();
        _lopDic = listLop.ToDictionary(x => x.MaLop, x => x.TenLop);
        _nganhDic = listNganh.ToDictionary(x => x.MaNganh, x => x.TenNganh);
    }

    public static SinhVienController GetInstance()
    {
        if (_instance == null) _instance = new SinhVienController();
        return _instance;
    }

    public List<SinhVienDTO> GetAll()
    {
        return SinhVienDao.GetAll();
    }

    public List<SinhVienDTO> LayDanhSachSinhVienTable()
    {
        return SinhVienDao.getTableSinhVien();
    }

    public bool EditSinhVien(SinhVienDTO sinhVien)
    {
        return SinhVienDao.Update(sinhVien);
    }

    public bool DeleteSinhVien(int id)
    {
        return SinhVienDao.Delete(id);
    }

    public SinhVienDTO GetById(int id)
    {
        return SinhVienDao.GetById(id);
    }

    public bool AddSinhVien(SinhVienDTO sinhVien)
    {
        return SinhVienDao.Add(sinhVien);
    }

    public List<NganhDto> GetAllNganh()
    {
        return NganhDao.GetAll();
    }

    public List<LopDto> GetLopByNganh(int maNganh)
    {
        return LopDao.GetByNganh(maNganh);
    }


    public NganhDto GetNganhById(int maNganh)
    {
        return NganhDao.GetNganhById(maNganh);
    }

    public SinhVienDTO GetByMaTK(int maTK)
    {
        var sv = new SinhVienDTO();
        var listSv = SinhVienDao.GetAll();
        foreach (var item in listSv)
            if (item.MaTk == maTK)
                sv = item;

        return sv;
    }


    public string GetTenKhoa(int maSV)
    {
        var sinhVien = GetById(maSV);
        var lop = _lopController.GetLopById(sinhVien.MaLop);
        var nganhDto = _nganhController.GetNganhById(lop.MaNganh);
        var khoaDto = _khoaController.GetKhoaById(nganhDto.MaKhoa);
        return khoaDto.TenKhoa;
    }

    public string GetTenNganh(int maSV)
    {
        var sinhVien = GetById(maSV);
        var lop = _lopController.GetLopById(sinhVien.MaLop);
        var nganhDto = _nganhController.GetNganhById(lop.MaNganh);
        return nganhDto.TenNganh;
    }

    public string GetTrangThaiHocPhi(int maSV, int hocKy, string nam) //tạm
    {
        foreach (var hpsv in _HocPhiSVController._listHocPhiSV)
            if (hpsv.MaSV == maSV && hpsv.HocKy == hocKy && hpsv.Nam.Equals(nam))
                return hpsv.TrangThai;

        return "Chưa đóng";
    }
    
    public List<SinhVienDisplay> ConvertDtoToDisplay(List<SinhVienDTO> input)
    {
        List<SinhVienDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new SinhVienDisplay
        {
            MaSinhVien = x.MaSinhVien,
            TenSinhVien = x.TenSinhVien,
            NgaySinh = x.NgaySinh,
            GioiTinh = x.GioiTinh,
            TenLop = _lopDic.TryGetValue(x.MaLop, out var tenLop) ? tenLop : "",
            TenNganh = _nganhDic.TryGetValue(x.MaLop, out var tenNganh) ? tenNganh : "",
            TrangThai = x.TrangThai
        });
        return rs;
    }
}