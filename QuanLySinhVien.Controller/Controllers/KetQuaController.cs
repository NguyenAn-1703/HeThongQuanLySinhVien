using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class KetQuaController
{
    private static KetQuaController _instance;
    private readonly KetQuaDao _ketQuaDao;
    private List<KetQuaDto> _listKetQua;

    private KetQuaController()
    {
        _ketQuaDao = KetQuaDao.GetInstance();
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
}