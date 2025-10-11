namespace QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
public class SinhVienController
{
    public SinhVienDAO sinhVienDAO;
    public SinhVienController()
    {   
        sinhVienDAO = new SinhVienDAO();
    }

    public List<SinhVienDTO> LayDanhSachSinhVienTable()
    {
        return sinhVienDAO.getTableSinhVien();
    }

    public void EditSinhVien(SinhVienDTO sinhVien)
    {
        
        sinhVienDAO.Update(sinhVien);
    }

    public void DeleteSinhVien(int id)
    {
        sinhVienDAO.Delete(id);
    }

    public SinhVienDTO GetSinhVienById(int id)
    {
        return sinhVienDAO.GetSinhVienById(id);
    }

    public void AddSinhVien(SinhVienDTO sinhVien)
    {
        sinhVienDAO.Add(sinhVien);
    }

    public List<SinhVienDTO> Search(string text, string filter)
    {
        return sinhVienDAO.Search(text, filter);
    }
}