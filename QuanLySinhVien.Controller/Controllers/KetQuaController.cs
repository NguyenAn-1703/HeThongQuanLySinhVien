using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class KetQuaController
{
    private static KetQuaController _instance;
    private readonly KetQuaDao _ketQuaDao;
    private List<KetQuaDto> _listKetQua;
    
    private HocPhanController _hocPhanController;
    private DiemQuaTrinhController _diemQuaTrinhController;

    private KetQuaController()
    {
        _ketQuaDao = KetQuaDao.GetInstance();
        _diemQuaTrinhController = DiemQuaTrinhController.GetInstance();
        _hocPhanController =  HocPhanController.GetInstance();
        _listKetQua = _ketQuaDao.GetAll();
    }

    public static KetQuaController GetInstance()
    {
        if (_instance == null) _instance = new KetQuaController();
        return _instance;
    }

    public List<KetQuaDto> GetAll()
    {
        return _ketQuaDao.GetAll();
    }

    public bool Insert(KetQuaDto ketQuaDto)
    {
        return _ketQuaDao.Insert(ketQuaDto);
    }

    public bool Update(KetQuaDto ketQuaDto)
    {
        return _ketQuaDao.Update(ketQuaDto);
    }

    public bool Delete(int maKQ)
    {
        return _ketQuaDao.Delete(maKQ);
    }

    public KetQuaDto GetKetQuaById(int maKQ)
    {
        return _ketQuaDao.GetKetQuaById(maKQ);
    }

    public bool ExistByMaSVMaHP(int MaSV, int MaHP)
    {
        _listKetQua = _ketQuaDao.GetAll();
        foreach (var item in _listKetQua)
            if (item.MaSV == MaSV && item.MaHP == MaHP)
                return true;

        return false;
    }

    public KetQuaDto GetByMaSVMaHP(int MaSV, int MaHP)
    {
        _listKetQua = _ketQuaDao.GetAll();
        var ketQua = new KetQuaDto();
        foreach (var item in _listKetQua)
            if (item.MaSV == MaSV && item.MaHP == MaHP)
            {
                ketQua = item;
                return ketQua;
            }

        return ketQua;
    }

    public List<KetQuaDto> GetByMaSV(int maSV)
    {
        _listKetQua = _ketQuaDao.GetAll();
        List<KetQuaDto> ketQua = new ();
        
        foreach (var item in _listKetQua)
            if (item.MaSV == maSV)
            {
                ketQua.Add(item);
            }

        return ketQua;
    }
    
    public List<KetQuaDto> GetListQuaMonByMaSV(int maSV)
    {
        _listKetQua = _ketQuaDao.GetAll();
        List<KetQuaDto> ketQua = new ();
        
        foreach (var item in _listKetQua)
            if (item.MaSV == maSV && item.DiemHe4 >= 2)
            {
                ketQua.Add(item);
            }

        return ketQua;
    }
    
    public void UpdateDiemHeSoSV(List<SinhVienDTO> listSV, HocPhanDto hocPhan)
    {
        foreach (SinhVienDTO item in listSV)
        {
            KetQuaDto ketQua = GetByMaSVMaHP(item.MaSinhVien, hocPhan.MaHP);

            DiemQuaTrinhDto diemQuaTrinhDto = (_diemQuaTrinhController.ExistsByMaKQ(ketQua.MaKQ))
                ? _diemQuaTrinhController.GetByMaKQ(ketQua.MaKQ)
                : new DiemQuaTrinhDto();
            
            
            string[] heso = hocPhan.HeSoHocPhan.Split(':');

            double diemQuaTrinh = diemQuaTrinhDto.DiemSo;
            double diemThi = ketQua.DiemThi;
            
            int hesoThi = int.Parse(heso[0]);
            int hesoQt = int.Parse(heso[1]);
            
            double diemHe10 = (diemThi * hesoThi +  diemQuaTrinh * hesoQt) / (hesoThi + hesoQt);
            double diemHe4;
            switch (diemHe10)
            {
                case >= 8.5 and <= 10.0:
                    diemHe4 = 4.0;
                    break;
                case >= 7.0 and < 8.5:
                    diemHe4 = 3.0;
                    break;
                case >= 5.5 and < 7.0:
                    diemHe4 = 2.0;
                    break;
                case >= 4.0 and < 5.5:
                    diemHe4 = 1.0;
                    break;
                default:
                    diemHe4 = 0.0;
                    break;
            }
            
            diemHe10 = Math.Round(diemHe10, 2);
            diemHe4 = Math.Round(diemHe4, 2);
            
            ketQua.DiemHe4 = (float)diemHe4;
            ketQua.DiemHe10 = (float)diemHe10;

            if (!Update(ketQua))
            {
                throw new Exception("loi sua ket qua");
            }
            
        }
    }
    
    public List<float> SetPercentPiChart()
    {
        // { "Xuất sắc", "Giỏi", "Khá", "Trung bình", "Yếu" };
        _listKetQua =  _ketQuaDao.GetAll();

        var listKq = _listKetQua
            .GroupBy(kq => kq.MaSV)
            .Select(g => new
            {
                MaSV = g.Key,
                DiemTb = g.Average(kq => kq.DiemHe4),
            }).ToList();

        int svxs = listKq.Where(x => x.DiemTb >= 3.6 && x.DiemTb <= 4.0).Count();
        int svg  = listKq.Where(x => x.DiemTb >= 3.2 && x.DiemTb < 3.6).Count();
        int svk  = listKq.Where(x => x.DiemTb >= 2.5 && x.DiemTb < 3.2).Count();
        int svtb = listKq.Where(x => x.DiemTb >= 2.0 && x.DiemTb < 2.5).Count();
        int svy  = listKq.Where(x => x.DiemTb < 2.0).Count();

        int tongsv = svxs + svg + svk + svtb + svy;

        float pt1 = (float)Math.Round(((double)svxs / tongsv * 100), 2);
        float pt2 = (float)Math.Round(((double)svg / tongsv * 100), 2);
        float pt3 = (float)Math.Round(((double)svk / tongsv * 100), 2);
        float pt4 = (float)Math.Round(((double)svtb / tongsv * 100), 2);
        float pt5 = (float)Math.Round(((double)svy / tongsv * 100), 2);
        
        List<float> rs = new List<float>();
        rs.Add(pt1);
        rs.Add(pt2);
        rs.Add(pt3);
        rs.Add(pt4);
        rs.Add(pt5);
        return rs;

    }
}