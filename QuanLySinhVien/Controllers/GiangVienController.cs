using System.Data;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class GiangVienController
{
    private static GiangVienController _instance;
    private static List<GiangVienDto> giangVien;
    
    private GiangVienController()
    { }

    public static GiangVienController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GiangVienController();
        }
        return _instance;
    }
    public List<GiangVienDto> GetAll()
    {
        try
        {
            giangVien = GiangVienDao.GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return giangVien;
    }

    public void HardDeleteById(int id)
    {
        try
        {
            GiangVienDao.HardDeleteById(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public void SoftDeleteById(int id)
    {
        try
        {
            GiangVienDao.SoftDeleteById(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public GiangVienDto GetById(int id)
    {
        GiangVienDto giangVien = new GiangVienDto();
        try
        {
            giangVien = GiangVienDao.GetGVById(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return giangVien;
    }

    public void Update(GiangVienDto giangVien)
    {
        try
        {
            if (giangVien.TenGV == "") throw new Exception("Tên giảng viên không được để trống");
            GiangVienDao.UpdateGV(giangVien);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void Insert(GiangVienDto giangVien)
    {
        try
        {
            if (giangVien.TenGV == "") throw new Exception("Tên giảng viên không được để trống");
            GiangVienDao.InsertGV(giangVien);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public GiangVienDto GetByTen(string ten)
    {
        return GiangVienDao.GetGVByTen(ten);
    }
    
    public bool ExistByTen(string ten)
    {
        List<GiangVienDto> listPh = GetAll();
        foreach (GiangVienDto item in listPh)
        {
            if (item.TenGV.Equals(ten))
            {
                return true;
            }
        }

        return false;
    }
    
    public GiangVienDto GetByMaTK(int maTk)
    {
        GiangVienDto gv = new  GiangVienDto();
        List<GiangVienDto> listGV = GiangVienDao.GetAll();
        
        gv = listGV.First(x => x.MaTK == maTk);
        return gv;
    }
}