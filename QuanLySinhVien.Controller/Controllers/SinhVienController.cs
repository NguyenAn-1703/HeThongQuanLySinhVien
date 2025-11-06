using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Structs;

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
    
    private ChuongTrinhDaoTao_HocPhanController _chuongTrinhDaoTao_HocPhanController;
    private KetQuaController _ketQuaController;
    private TaiKhoanController _taiKhoanController;
    
    
    
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
        _chuongTrinhDaoTao_HocPhanController = ChuongTrinhDaoTao_HocPhanController.GetInstance();
        _ketQuaController = KetQuaController.GetInstance();
        _taiKhoanController = TaiKhoanController.getInstance();

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


    public void UpdateTrangThaiSv()
    {
        Dictionary<int, int> MaSvMaCTDTDic = SinhVienDao.GetSinhVien_ChuongTrinhDaoTao();
        foreach (var item in MaSvMaCTDTDic)
        {
            //ds hp sv cần học để tốt nghiệp
            int maSV = item.Key;
            int maCTDT = item.Value;
            List<ChuongTrinhDaoTao_HocPhanDto> listCTDT = _chuongTrinhDaoTao_HocPhanController.GetByMaCTDT(maCTDT);
            List<int> listSVCanHoc = listCTDT.Select(x => x.MaHP).ToList();

            //ds hp đã học và không liệt
            List<KetQuaDto> listKetQua = _ketQuaController.GetListQuaMonByMaSV(maSV);
            List<int> listDaHoc = listKetQua.Select(x => x.MaHP).ToList();
            
            
            bool totNghiep = listSVCanHoc.All(hp => listDaHoc.Contains(hp));

            if (totNghiep)
            {
                SinhVienDTO sv =  GetById(maSV);
                sv.TrangThai = "Tốt nghiệp";
                if (!EditSinhVien(sv))
                {
                    throw new Exception("sua sv that bai");
                }
            }
            
            // ✅ Log ra nếu là sinh viên mã số 1
            if (maSV == 1)
            {
                Console.WriteLine($"Sinh viên {maSV}:");
                Console.WriteLine($"- Danh sách học phần cần học ({listSVCanHoc.Count}): {string.Join(", ", listSVCanHoc)}");
                Console.WriteLine($"- Danh sách học phần đã học qua môn ({listDaHoc.Count}): {string.Join(", ", listDaHoc)}");
                Console.WriteLine("------------------------------------------------------------");
            }
        }
    }
    
    public bool ExistByMaTk(int maTK)
    {
        var listSV = SinhVienDao.GetAll();
        foreach (var item in listSV)
        {
            if (item.MaTk == maTK)
            {
                return true;
            }
        }

        return false;
    }
    
    public ValidateResult Validate(
        string imgPath, string tenSV, string soDT, string ngaySinh,
        string email, string cccd, string queQuan,
        string tenLop, string tenTK)
    {
        ValidateResult rs = new ValidateResult
        {
            index = -1,
            message = "",
        };
        
        var homNay = DateTime.Now;
        DateTime ngaySinhDate = ConvertDate.ConvertStringToDate(ngaySinh);
        var tuoi = homNay.Year - ngaySinhDate.Year;

        if (homNay.Month < ngaySinhDate.Month ||
            (homNay.Month == ngaySinhDate.Month && homNay.Day < ngaySinhDate.Day))
            tuoi--;

        if (Shared.Validate.IsEmpty(imgPath))
        {
            rs.index = 0;
            rs.message = "Vui lòng thêm ảnh!";
            return rs;
        }

        if (Shared.Validate.IsEmpty(tenSV))
        {
            rs.index = 0;
            rs.message = "Tên sinh viên không được để trống!";
            return rs;
        }

        if (Shared.Validate.IsEmpty(soDT))
        {
            rs.index = 1;
            rs.message = "Số điện thoại không được để trống!";
            return rs;
        }

        if (!Shared.Validate.IsValidPhoneNumber(soDT))
        {
            rs.index = 1;
            rs.message = "Số điện thoại không hợp lệ!";
            return rs;
        }

        if (Shared.Validate.IsEmpty(ngaySinh))
        {
            rs.index = 2;
            rs.message = "Ngày sinh không được để trống!";
            return rs;
        }

        if (tuoi < 17)
        {
            rs.index = 2;
            rs.message = "Tuổi sinh viên phải lớn hơn 16!";
            return rs;
        }

        if (Shared.Validate.IsEmpty(email))
        {
            rs.index = 3;
            rs.message = "Email không được để trống!";
            return rs;
        }

        if (!Shared.Validate.IsValidEmail(email))
        {
            rs.index = 3;
            rs.message = "Email không hợp lệ!";
            return rs;

        }

        if (Shared.Validate.IsEmpty(cccd))
        {
            rs.index = 4;
            rs.message = "Căn cước công dân không được để trống!";
            return rs;
        }

        if (!Shared.Validate.IsValidCCCD(cccd))
        {
            rs.index = 4;
            rs.message = "Căn cước công dân không hợp lệ!";
            return rs;
        }

        if (Shared.Validate.IsEmpty(queQuan))
        {
            rs.index = 5;
            rs.message = "Quê quán không được để trống!";
            return rs;
        }

        if (Shared.Validate.IsEmpty(tenLop))
        {
            rs.index = 6;
            rs.message = "Tên lớp không được để trống!";
            return rs;
        }

        if (!_lopController.ExistByTen(tenLop))
        {
            rs.index = 6;
            rs.message = "Lớp không tồn tại!";
            return rs;
        }

        if (Shared.Validate.IsEmpty(tenTK))
        {
            rs.index = 7;
            rs.message = "Tên tài khoản không được để trống!";
            return rs;
        }

        if (!_taiKhoanController.ExistByTen(tenTK))
        {
            rs.index = 7;
            rs.message = "Tài khoản không tồn tại!";
            return rs;
        }

        return rs;
    }
    
}