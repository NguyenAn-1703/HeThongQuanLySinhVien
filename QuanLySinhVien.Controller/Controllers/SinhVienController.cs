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

    private Dictionary<int, LopDto> _lopDic;
    private Dictionary<int, NganhDto> _nganhDic;
    private Dictionary<int, string> _khoaDic;
    
    
    
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
        var listKhoa = _khoaController.GetDanhSachKhoa();
        
        _lopDic = listLop.ToDictionary(x => x.MaLop, x => x);
        _nganhDic = listNganh.ToDictionary(x => x.MaNganh, x => x);
        _khoaDic = listKhoa.ToDictionary(x => x.MaKhoa, x => x.TenKhoa);
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
    
    public List<SinhVienDisplay> ConvertDtoToDisplay(List<SinhVienDTO> input)
    {
        List<SinhVienDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new SinhVienDisplay
        {
            MaSinhVien = x.MaSinhVien,
            TenSinhVien = x.TenSinhVien,
            NgaySinh = x.NgaySinh,
            GioiTinh = x.GioiTinh,
            TenLop = _lopDic.TryGetValue(x.MaLop, out var lop) ? lop.TenLop : "",
            TenNganh = GetTenNganh(x),
            TrangThai = x.TrangThai
        });
        return rs;
    }
    
    public List<SVHocPhiDisplay> ConvertDtoToDisplayHocPhi(List<SinhVienDTO> input, int hky, string nam)
    {
        List<SVHocPhiDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new SVHocPhiDisplay
        {
            MaSV = x.MaSinhVien,
            TenSV = x.TenSinhVien,
            Khoa = GetTenKhoa(x),
            Nganh = GetTenNganh(x),
            TrangThaiHP = GetTrangThaiHocPhi(x.MaSinhVien, hky, nam),
        });
        return rs;
    }

    string GetTenNganh(SinhVienDTO x)
    {
        LopDto lopDto = _lopDic.TryGetValue(x.MaLop, out var lop) ? lop : new LopDto();
        return _nganhDic.TryGetValue(lopDto.MaNganh, out var nganh) ? nganh.TenNganh : "";
    }

    string GetTenKhoa(SinhVienDTO x)
    {
        LopDto lopDto = _lopDic.TryGetValue(x.MaLop, out var lop) ? lop : new LopDto();
        NganhDto nganhDto = _nganhDic.TryGetValue(lopDto.MaNganh, out var nganh) ? nganh : new NganhDto();
        return _khoaDic.TryGetValue(nganhDto.MaKhoa, out var tenKhoa) ? tenKhoa : "";
    }
    
    public string GetTrangThaiHocPhi(int maSV, int hocKy, string nam) //tạm
    {
        foreach (var hpsv in _HocPhiSVController._listHocPhiSV)
            if (hpsv.MaSV == maSV && hpsv.HocKy == hocKy && hpsv.Nam.Equals(nam))
                return hpsv.TrangThai;
        return "Chưa đóng";
    }
}