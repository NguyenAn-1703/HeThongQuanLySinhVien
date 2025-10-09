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
    
}