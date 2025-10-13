using System.Data;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Controllers;

public class GiangVienController
{
    public static List<GiangVienDTO> GetAll()
    {
        try
        {
            List<GiangVienDTO> giangVien = GiangVienModel.GetAll();
            return giangVien;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void HardDeleteById(int id)
    {
        try
        {
            GiangVienModel.HardDeleteById(id);
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
            GiangVienModel.SoftDeleteById(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
}