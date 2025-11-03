using Newtonsoft.Json;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class NhomQuyenController
{
    private static NhomQuyenController _instance;
    private readonly NhomQuyenDao _nhomQuyenDao;
    private List<NhomQuyenDto> listNhomQuyen;

    private NhomQuyenController()
    {
        _nhomQuyenDao = NhomQuyenDao.GetInstance();
        listNhomQuyen = _nhomQuyenDao.GetAll();
    }

    public static NhomQuyenController GetInstance()
    {
        if (_instance == null) _instance = new NhomQuyenController();
        return _instance;
    }

    public List<NhomQuyenDto> GetAll()
    {
        return _nhomQuyenDao.GetAll();
    }

    public bool Insert(NhomQuyenDto nhomQuyenDto)
    {
        var result = _nhomQuyenDao.Insert(nhomQuyenDto);
        if (result)
            listNhomQuyen = _nhomQuyenDao.GetAll();
        return result;
    }

    public bool Update(NhomQuyenDto nhomQuyenDto)
    {
        var result = _nhomQuyenDao.Update(nhomQuyenDto);
        if (result)
            listNhomQuyen = _nhomQuyenDao.GetAll();
        return result;
    }

    public bool Delete(int idNhomQuyen)
    {
        var result = _nhomQuyenDao.Delete(idNhomQuyen);
        if (result)
            listNhomQuyen = _nhomQuyenDao.GetAll();
        return result;
    }

    public NhomQuyenDto GetById(int id)
    {
        return listNhomQuyen.First(nq => nq.MaNQ == id);
    }

    public string GetTenQuyenByID(int id)
    {
        var nhomQuyen = _nhomQuyenDao.GetById(id);
        return nhomQuyen.TenNhomQuyen;
    }

    public List<string> GetAllTenNhomQuyen()
    {
        listNhomQuyen = _nhomQuyenDao.GetAll();
        var listTenNhomQuyen = new List<string>();
        foreach (var item in listNhomQuyen) listTenNhomQuyen.Add(item.TenNhomQuyen);
        return listTenNhomQuyen;
    }

    public int GetMaNhomQuyenByTen(string ten)
    {
        foreach (var item in listNhomQuyen)
            if (ten.Equals(item.TenNhomQuyen))
                return item.MaNQ;

        return -1;
    }

    //Đọc ds quyền từ json
    public List<QuyenChucNangJS> GetListAllChucNang_HanhDong()
    {
        var json = File.ReadAllText("config/Permission.json");

        var list = JsonConvert.DeserializeObject<ListQuyenChucNangJS>(json);

        return list.Modules;
    }

    public int GetLastAutoIncrement()
    {
        return _nhomQuyenDao.GetLastAutoIncrement();
    }
}