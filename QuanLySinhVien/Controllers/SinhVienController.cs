using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components;

namespace QuanLySinhVien.Controllers;

public class SinhVienController
{
    public SinhVienDAO SinhVienDao;
    public NganhDao NganhDao;
    public LopDAO LopDao;
    
    private LopController _lopController;
    private NganhController _nganhController;
    private KhoaController _khoaController;
    private HocPhiSVController _HocPhiSVController;
    private NhomHocPhanController _nhomHocPhanController;
    

    private static SinhVienController _instance;

    public static SinhVienController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SinhVienController();
        }
        return _instance;
    }
    private SinhVienController()
    {   
        SinhVienDao = new SinhVienDAO();
        NganhDao = NganhDao.GetInstance();
        LopDao = LopDAO.GetInstance();
        _lopController = LopController.GetInstance();
        _nganhController = NganhController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        _HocPhiSVController = HocPhiSVController.GetInstance();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
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
        SinhVienDTO sv = new  SinhVienDTO();
        List<SinhVienDTO> listSv = SinhVienDao.GetAll();
        foreach (SinhVienDTO item in listSv)
        {
            if (item.MaTk == maTK)
            {
                sv =  item;
            }
        }
        return sv;
    }
    
    
    public string GetTenKhoa(int maSV)
    {
        SinhVienDTO sinhVien = GetById(maSV);
        LopDto lop = _lopController.GetLopById(sinhVien.MaLop);
        NganhDto nganhDto = _nganhController.GetNganhById(lop.MaNganh);
        KhoaDto khoaDto = _khoaController.GetKhoaById(nganhDto.MaKhoa);
        return khoaDto.TenKhoa;
    }
    
    public string GetTenNganh(int maSV)
    {
        SinhVienDTO sinhVien = GetById(maSV);
        LopDto lop = _lopController.GetLopById(sinhVien.MaLop);
        NganhDto nganhDto = _nganhController.GetNganhById(lop.MaNganh);
        return nganhDto.TenNganh;
    }
    
    public string GetTrangThaiHocPhi(int maSV, int hocKy, string nam) //tạm
    {
        foreach (HocPhiSVDto hpsv in _HocPhiSVController._listHocPhiSV)
        {
            if (hpsv.MaSV == maSV && hpsv.HocKy == hocKy && hpsv.Nam.Equals(nam))
            {
                return hpsv.TrangThai;
            }
        }

        return "Chưa đóng";
    }
}