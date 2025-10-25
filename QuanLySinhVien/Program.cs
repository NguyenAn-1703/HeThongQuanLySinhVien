using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.Home;
using QuanLySinhVien.Views.Forms;

namespace QuanLySinhVien;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        
        ApplicationConfiguration.Initialize();
        
        // Application.Run(new FLogin());
        
        // NhomQuyenDto nhomQuyen = new NhomQuyenDto*
        
        NhomQuyenDto nhomQuyen = new NhomQuyenDto
        {
            MaNQ = 2,
            TenNhomQuyen = "admin", 
        };
        Application.Run(new MyHome(nhomQuyen));
        
        // NhomQuyenDto nhomQuyen = new NhomQuyenDto
        // {
        //     MaNQ = 1,
        //     TenNhomQuyen = "SinhVien", 
        // };
        // Application.Run(new MyHome(nhomQuyen));

    }
}