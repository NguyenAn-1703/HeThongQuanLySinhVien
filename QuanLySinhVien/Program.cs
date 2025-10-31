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
        
        
        //ADMIN
        // NhomQuyenDto nhomQuyen = new NhomQuyenDto
        // {
        //     MaNQ = 2,
        //     TenNhomQuyen = "admin", 
        // };
        // TaiKhoanDto taiKhoan = new TaiKhoanDto
        // {
        //     MaTK = 1,
        //     TenDangNhap = "admin", 
        //     Type = "Quản trị viên"
        // };
        // Application.Run(new MyHome(nhomQuyen, taiKhoan));
        
        //SINHVIEN
        // NhomQuyenDto nhomQuyen = new NhomQuyenDto
        // {
        //     MaNQ = 1,
        //     TenNhomQuyen = "SinhVien", 
        // };
        // TaiKhoanDto taiKhoan = new TaiKhoanDto
        // {
        //     MaTK = 3,
        //     TenDangNhap = "sinhvien", 
        //     Type = "Sinh viên"
        // };
        // Application.Run(new MyHome(nhomQuyen, taiKhoan));
        //
        //GIANGVIEN
        NhomQuyenDto nhomQuyen = new NhomQuyenDto
        {
            MaNQ = 4,
            TenNhomQuyen = "GiangVien", 
        };
        TaiKhoanDto taiKhoan = new TaiKhoanDto
        {
            MaTK = 4,
            TenDangNhap = "giangvien", 
            Type = "Quản trị viên"
        };
        Application.Run(new MyHome(nhomQuyen, taiKhoan));

    }
}