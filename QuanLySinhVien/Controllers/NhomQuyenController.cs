using Newtonsoft.Json;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

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
        if (_instance == null)
        {
            _instance = new NhomQuyenController();
        }
        return _instance;
    }

    public List<NhomQuyenDto> GetAll()
    {
        return _nhomQuyenDao.GetAll();
    }

    public bool Insert(NhomQuyenDto nhomQuyenDto)
    {
        return _nhomQuyenDao.Insert(nhomQuyenDto);
    }

    public bool Update(NhomQuyenDto nhomQuyenDto)
    {
        return _nhomQuyenDao.Update(nhomQuyenDto);
    }

    public bool Delete(int idNhomQuyen)
    {
        return _nhomQuyenDao.Delete(idNhomQuyen);
    }

    public NhomQuyenDto GetById(int id)
    {
        return _nhomQuyenDao.GetById(id);
    }

    public string GetTenQuyenByID(int id)
    {
        NhomQuyenDto nhomQuyen = _nhomQuyenDao.GetById(id);
        return nhomQuyen.TenNhomQuyen;
    }

    public List<string> GetAllTenNhomQuyen()
    {
        List<string> listTenNhomQuyen = new List<string>();
        foreach (var item in listNhomQuyen)
        {
            listTenNhomQuyen.Add(item.TenNhomQuyen);
        }
        return listTenNhomQuyen;
    }

    public int GetMaNhomQuyenByTen(string ten)
    {
        foreach (var item in listNhomQuyen)
        {
            if (ten.Equals(item.TenNhomQuyen))
                return item.MaNQ;
        }

        return -1;
    }

    //Đọc ds quyền từ json
    public List<QuyenChucNangJS> GetListAllChucNang_HanhDong()
    {
        string json = File.ReadAllText("config/Permission.json");
        ListQuyenChucNangJS list = JsonConvert.DeserializeObject<ListQuyenChucNangJS>(json);

        return list.Modules;
    }

    public int GetLastAutoIncrement()
    {
        return _nhomQuyenDao.GetLastAutoIncrement();
    }
}
