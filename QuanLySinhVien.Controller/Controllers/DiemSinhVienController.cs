using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Controller.Controllers;

public class DiemSinhVienController
{
    private readonly DiemSinhVienDao _dao;

    public DiemSinhVienController()
    {
        _dao = DiemSinhVienDao.GetInstance();
    }

    public List<DiemSinhVienDto> GetDiemTheoKy(int maSinhVien, int hocKy, string nam)
    {
        try
        {
            return _dao.GetDiemTheoKy(maSinhVien, hocKy, nam);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi lấy điểm sinh viên: {ex.Message}");
            return new List<DiemSinhVienDto>();
        }
    }

    public List<(int HocKy, string Nam)> GetDanhSachKyHoc(int maSinhVien)
    {
        try
        {
            return _dao.GetDanhSachKyHoc(maSinhVien);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi lấy danh sách kỳ học: {ex.Message}");
            return new List<(int HocKy, string Nam)>();
        }
    }

    public (float DiemTBHe10, float DiemTBHe4, int TongTinChi) TinhDiemTrungBinh(int maSinhVien, int hocKy, string nam)
    {
        try
        {
            return _dao.TinhDiemTrungBinhTheoKy(maSinhVien, hocKy, nam);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi tính điểm trung bình: {ex.Message}");
            return (0, 0, 0);
        }
    }
}