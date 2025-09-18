using QuanLySinhVien.Views.Components;
using QuanLySinhVien.Views.UserControls;

namespace QuanLySinhVien.Views.Forms;

public class FLogin : MyForm
{
    public UcLogin ucLogin = new  UcLogin();
    public FLogin():
        base("Đăng nhập", new Size(1440, 1024))
    {
        
        // Location
        ucLogin.Location = new Point(0, 0);
        
        // Add Component
        Controls.Add(ucLogin);
    }
}