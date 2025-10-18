using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class SinhVienController
{
    public SinhVienDAO SinhVienDao;
    public NganhDao NganhDao;
    public LopDAO LopDao;
    
    public SinhVienController()
    {   
        SinhVienDao = new SinhVienDAO();
        NganhDao = NganhDao.GetInstance();
        LopDao = new LopDAO();
    }

    public List<SinhVienDTO> LayDanhSachSinhVienTable()
    {
        return SinhVienDao.getTableSinhVien();
    }

    public void EditSinhVien(SinhVienDTO sinhVien)
    {
        
        SinhVienDao.Update(sinhVien);
    }

    public void DeleteSinhVien(int id)
    {
        SinhVienDao.Delete(id);
    }

    public SinhVienDTO GetSinhVienById(int id)
    {
        return SinhVienDao.GetSinhVienById(id);
    }

    public void AddSinhVien(SinhVienDTO sinhVien)
    {
        SinhVienDao.Add(sinhVien);
    }

    public List<SinhVienDTO> Search(string text, string filter)
    {
        return SinhVienDao.Search(text, filter);
    }
    
    public List<NganhDto> GetAllNganh()
    {
        return NganhDao.GetAll();
    }

    public List<LopDto> GetLopByNganh(int maNganh)
    {
        return LopDao.GetByNganh(maNganh);
    }


    public NganhDto GetNganhById(int maNganh)
    {
        return NganhDao.GetNganhById(maNganh);
    }
}