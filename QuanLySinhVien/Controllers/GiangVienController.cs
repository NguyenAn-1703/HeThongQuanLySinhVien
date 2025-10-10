using System.Data;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Controllers;

public class GiangVienController
{
    public static List<GiangVien> GetAll()
    {
        return GiangVienModel.GetAll();
    }

}