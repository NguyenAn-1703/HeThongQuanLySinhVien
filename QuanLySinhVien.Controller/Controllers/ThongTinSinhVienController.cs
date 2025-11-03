using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class ThongTinSinhVienController
{
    private readonly ThongTinSinhVienDao _dao;

    public ThongTinSinhVienController()
    {
        _dao = ThongTinSinhVienDao.GetInstance();
    }


    public ThongTinSinhVienDto? GetThongTinSinhVien(int maSinhVien)
    {
        try
        {
            return _dao.GetByMaSinhVien(maSinhVien);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi lấy thông tin sinh viên: {ex.Message}");
            return null;
        }
    }
}