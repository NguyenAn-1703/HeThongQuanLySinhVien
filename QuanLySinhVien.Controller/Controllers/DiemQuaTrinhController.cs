using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class DiemQuaTrinhController
{
    private static DiemQuaTrinhController _instance;
    private readonly DiemQuaTrinhDao _diemQuaTrinhDao;
    private List<DiemQuaTrinhDto> _listDiemQuaTrinh;

    private DiemQuaTrinhController()
    {
        _diemQuaTrinhDao = DiemQuaTrinhDao.GetInstance();
        _listDiemQuaTrinh = _diemQuaTrinhDao.GetAll();
    }

    public static DiemQuaTrinhController GetInstance()
    {
        if (_instance == null) _instance = new DiemQuaTrinhController();
        return _instance;
    }

    public List<DiemQuaTrinhDto> GetAll()
    {
        return _diemQuaTrinhDao.GetAll();
    }

    public bool Insert(DiemQuaTrinhDto dto)
    {
        return _diemQuaTrinhDao.Insert(dto);
    }

    public bool Update(DiemQuaTrinhDto dto)
    {
        return _diemQuaTrinhDao.Update(dto);
    }

    public bool Delete(int maDQT)
    {
        return _diemQuaTrinhDao.Delete(maDQT);
    }

    public DiemQuaTrinhDto GetById(int maDQT)
    {
        return _diemQuaTrinhDao.GetDiemQuaTrinhById(maDQT);
    }

    public DiemQuaTrinhDto GetByMaKQ(int maKQ)
    {
        var rs = new DiemQuaTrinhDto();
        _listDiemQuaTrinh = _diemQuaTrinhDao.GetAll();
        foreach (var dto in _listDiemQuaTrinh)
            if (dto.MaKQ == maKQ)
            {
                rs = dto;
                return rs;
            }

        return rs;
    }

    public bool ExistsByMaKQ(int maKQ)
    {
        _listDiemQuaTrinh = _diemQuaTrinhDao.GetAll();
        foreach (var dto in _listDiemQuaTrinh)
            if (dto.MaKQ == maKQ)
                return true;

        return false;
    }

    public bool HardDelete(int maDQT)
    {
        return _diemQuaTrinhDao.HardDelete(maDQT);
    }

    public int GetLastAutoIncrement()
    {
        _listDiemQuaTrinh = _diemQuaTrinhDao.GetAll();
        var lastindex = _listDiemQuaTrinh.Count - 1;
        return _listDiemQuaTrinh[lastindex].MaDQT;
    }
}