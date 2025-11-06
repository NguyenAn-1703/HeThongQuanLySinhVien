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
    
    private Dictionary<int, string> hocPhanDic;
    private Dictionary<int, string> giangVienDic;

    private NhomHocPhanController()
    {
        _nhomHocPhanDao = NhomHocPhanDao.GetInstance();
        _listNhomHocPhan = _nhomHocPhanDao.GetAll();
        _hocPhanController = HocPhanController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        InitLookUpData();
    }

    void InitLookUpData()
    {
        List<HocPhanDto> listHocPhan = _hocPhanController.GetAll();
        List<GiangVienDto> listGiangVien = _giangVienController.GetAll();
        
        hocPhanDic = listHocPhan.ToDictionary(x => x.MaHP, x => x.TenHP);
        giangVienDic = listGiangVien.ToDictionary(x => x.MaGV, x => x.TenGV);
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
    
    
    public List<NhomHocPhanDisplay> ConvertDtoToDisplayNhapDiem(List<NhomHocPhanDto> input)
    {
        List<NhomHocPhanDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new NhomHocPhanDisplay
        {
            MaNHP = x.MaNHP,
            TenHP = hocPhanDic.TryGetValue(x.MaHP, out var  ten) ? ten : "",
            Siso = x.SiSo,
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
}