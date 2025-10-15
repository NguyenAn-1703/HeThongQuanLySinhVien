using System.Data;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Controllers;

public class GiangVienController
{
    private static List<GiangVienDto> giangVien;
    public static List<GiangVienDto> GetAll()
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

    public static void HardDeleteById(int id)
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
    public static void SoftDeleteById(int id)
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

    public static GiangVienDto GetGVById(int id)
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

    public static void UpdateGV(GiangVienDto giangVien)
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

    public static void InsertGV(GiangVienDto giangVien)
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
}