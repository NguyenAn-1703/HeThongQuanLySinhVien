using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;

namespace QuanLySinhVien.Controller.Controllers;

public class NhomHocPhanController
{
    private static NhomHocPhanController _instance;
    private readonly NhomHocPhanDao _nhomHocPhanDao;
    private List<NhomHocPhanDto> _listNhomHocPhan;
    private HocPhanController _hocPhanController;
    private GiangVienController _giangVienController;
    private DangKyController _dangKyController;
    private KhoaController _khoaController;
    
    private Dictionary<int, HocPhanDto> hocPhanDic;
    private Dictionary<int, string> giangVienDic;
    private Dictionary<int, string> khoaDic;

    private NhomHocPhanController()
    {
        _nhomHocPhanDao = NhomHocPhanDao.GetInstance();
        _listNhomHocPhan = _nhomHocPhanDao.GetAll();
        _hocPhanController = HocPhanController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        InitLookUpData();
    }

    void InitLookUpData()
    {
        List<HocPhanDto> listHocPhan = _hocPhanController.GetAll();
        List<GiangVienDto> listGiangVien = _giangVienController.GetAll();
        List<KhoaDto> listKhoa = _khoaController.GetDanhSachKhoa();
        
        hocPhanDic = listHocPhan.ToDictionary(x => x.MaHP, x => x);
        giangVienDic = listGiangVien.ToDictionary(x => x.MaGV, x => x.TenGV);
        khoaDic = listKhoa.ToDictionary(x => x.MaKhoa, x => x.TenKhoa);

    }

    public static NhomHocPhanController GetInstance()
    {
        if (_instance == null) _instance = new NhomHocPhanController();
        return _instance;
    }

    public List<NhomHocPhanDto> GetAll()
    {
        return _nhomHocPhanDao.GetAll();
    }

    public bool Insert(NhomHocPhanDto dto)
    {
        return _nhomHocPhanDao.Insert(dto);
    }

    public bool Update(NhomHocPhanDto dto)
    {
        return _nhomHocPhanDao.Update(dto);
    }

    public bool Delete(int maNHP)
    {
        return _nhomHocPhanDao.Delete(maNHP);
    }

    public NhomHocPhanDto GetById(int maNHP)
    {
        return _nhomHocPhanDao.GetById(maNHP);
    }

    public List<NhomHocPhanDto> GetByLichMaDangKy(int maLichDk)
    {
        return _nhomHocPhanDao.GetByLichMaDangKy(maLichDk);
    }

    public List<NhomHocPhanDto> GetByHkyNamMaHP(int hky, string nam, int maHP)
    {
        return _nhomHocPhanDao.GetByHkyNamMaHP(hky, nam, maHP);
    }

    public List<NhomHocPhanDto> GetByHkyNam(int hky, string nam)
    {
        return _nhomHocPhanDao.GetByHkyNam(hky, nam);
    }

    public List<NhomHocPhanDto> GetByHkyNamMaGV(int hky, string nam, int maGV)
    {
        return _nhomHocPhanDao.GetByHkyNamMaGV(hky, nam, maGV);
    }
    
    //tuongw lai suar :<
    public List<NhomHocPhanDisplay> ConvertDtoToDisplayNhapDiem(List<NhomHocPhanDto> input)
    {
        List<NhomHocPhanDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new NhomHocPhanDisplay
        {
            MaNHP = x.MaNHP,
            TenHP = hocPhanDic.TryGetValue(x.MaHP, out var  hp) ? hp.TenHP : "",
            Siso = x.SiSo + "/" + x.SiSoToiDa,
            TenGiangVien = giangVienDic.TryGetValue(x.MaGV, out var teng) ? teng : ""
        });
        return rs;
    }

    public void UpdateSiso()
    {
        _listNhomHocPhan = _nhomHocPhanDao.GetAll();
        _dangKyController = DangKyController.GetInstance();
        List<DangKyDto> listDangKy = _dangKyController.GetAll();

        foreach (var item in _listNhomHocPhan)
        {
            int siSo = listDangKy.Count(x => x.MaNHP == item.MaNHP);
            item.SiSo = siSo;
            if (!Update(item))
            {
                throw new Exception("Sua nhp that bai");
            }
        }
    }

    public List<NhomHocPhanDto> GetByMaDotDky(int maDot)
    {
        _listNhomHocPhan = _nhomHocPhanDao.GetAll();

        List<NhomHocPhanDto> rs = new List<NhomHocPhanDto>();
        foreach (var item in _listNhomHocPhan)
        {
            if (item.MaDotDK == maDot) rs.Add(item);
        }

        return rs;
    }

    public List<NhomHocPhanDisplay> ConvertDtoToDisplay(List<NhomHocPhanDto> input)
    {
        List<NhomHocPhanDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new NhomHocPhanDisplay
        {
            MaNHP = x.MaNHP,
            TenHP = hocPhanDic.TryGetValue(x.MaHP, out var  hp) ? hp.TenHP : "",
            MaHP = x.MaHP,
            TenKhoa = khoaDic.TryGetValue(hocPhanDic.TryGetValue(x.MaHP, out var hp1) ? hp1.MaKhoa : 0, out var tenkh) ? tenkh : "",
            Siso = x.SiSo + "/" + x.SiSoToiDa,
            TenGiangVien = giangVienDic.TryGetValue(x.MaGV, out var teng) ? teng : "",
        });
        return rs;
    }
}