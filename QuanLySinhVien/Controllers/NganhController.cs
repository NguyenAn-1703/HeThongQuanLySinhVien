using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;

public class NganhController
{
    private static NganhController _instance;
    private readonly NganhDao _nganhDao;
    private List<NganhDto> _listNganh;

    // Constructor riêng tư (Singleton)
    private NganhController()
    {
        _nganhDao = NganhDao.GetInstance();
        _listNganh = _nganhDao.GetAll();
    }

    // Lấy instance duy nhất
    public static NganhController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new NganhController();
        }

        return _instance;
    }

    // Lấy danh sách tất cả ngành
    public List<NganhDto> GetAll()
    {
        return _nganhDao.GetAll();
    }

    // Thêm ngành mới
    public bool Insert(NganhDto nganhDto)
    {
        return _nganhDao.Insert(nganhDto);
    }

    // Cập nhật ngành
    public bool Update(NganhDto nganhDto)
    {
        return _nganhDao.Update(nganhDto);
    }

    // Xóa ngành theo ID
    public bool Delete(int idNganh)
    {
        return _nganhDao.Delete(idNganh);
    }

    // Lấy thông tin ngành theo ID
    public NganhDto GetNganhById(int id)
    {
        return _nganhDao.GetNganhById(id);
    }
}
