using QuanLySinhVien.Model.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class LichSD_NhomHPController
{
    private static LichSD_NhomHPController _instance;
    private readonly LichSD_NhomHPDao _lichSDNhomHPDao;

    private List<LichSD_NhomHPDto> _listLichSDNhomHP;

    private LichSD_NhomHPController()
    {
        _lichSDNhomHPDao = LichSD_NhomHPDao.GetInstance();
        LoadData();
    }

    public static LichSD_NhomHPController GetInstance()
    {
        return _instance ?? (_instance = new LichSD_NhomHPController());
    }

    public void LoadData()
    {
        _listLichSDNhomHP = _lichSDNhomHPDao.GetAll();
    }

    public List<LichSD_NhomHPDto> GetAll()
    {
        return new List<LichSD_NhomHPDto>(_listLichSDNhomHP);
    }

    public bool Add(LichSD_NhomHPDto dto)
    {
        bool success = _lichSDNhomHPDao.Insert(dto);
        if (success)
        {
            _listLichSDNhomHP.Add(dto);
        }

        return success;
    }

    
    public bool Delete(int maLSD, int maNHP)
    {
        bool success = _lichSDNhomHPDao.Delete(maLSD, maNHP);
        if (success)
        {
            var item = _listLichSDNhomHP
                .FirstOrDefault(x => x.MaLSD == maLSD && x.MaNHP == maNHP);

            if (item != null)
                _listLichSDNhomHP.Remove(item);
        }

        return success;
    }


    public List<LichSD_NhomHPDto> GetByMaLSD(int maLSD)
    {
        return _listLichSDNhomHP
            .Where(x => x.MaLSD == maLSD)
            .ToList();
    }


    public List<LichSD_NhomHPDto> GetByMaNHP(int maNHP)
    {
        return _listLichSDNhomHP
            .Where(x => x.MaNHP == maNHP)
            .ToList();
    }
    
    public bool Exists(int maLSD, int maNHP)
    {
        return _listLichSDNhomHP
            .Any(x => x.MaLSD == maLSD && x.MaNHP == maNHP);
    }
}
