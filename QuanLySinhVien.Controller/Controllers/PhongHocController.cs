using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class PhongHocController
{
    private static PhongHocController _instance;
    private readonly PhongHocDao _dao;

    private PhongHocController()
    {
        _dao = new PhongHocDao();
    }

    public static PhongHocController getInstance()
    {
        if (_instance == null) _instance = new PhongHocController();
        return _instance;
    }

    public List<PhongHocDto> GetDanhSachPhongHoc()
    {
        return _dao.GetAll();
    }

    public PhongHocDto GetPhongHocById(int maPH)
    {
        return _dao.GetById(maPH);
    }

    public PhongHocDto GetByTen(string TenPH)
    {
        return _dao.GetByTen(TenPH);
    }

    public bool ThemPhongHoc(PhongHocDto phongHoc)
    {
        return _dao.Insert(phongHoc);
    }

    public bool SuaPhongHoc(PhongHocDto phongHoc)
    {
        return _dao.Update(phongHoc);
    }

    public bool XoaPhongHoc(int maPH)
    {
        return _dao.Delete(maPH);
    }

    public bool ExistByTen(string ten)
    {
        var listPh = GetDanhSachPhongHoc();
        foreach (var item in listPh)
            if (item.TenPH.Equals(ten))
                return true;

        return false;
    }
}