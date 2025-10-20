using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.NavList;

public abstract class NavBase : Panel
{
    //Định nghĩa những hàm dùng chung của các NavItem trong folder NavList/
    public NhomQuyenDto _quyen;
    public NavBase(NhomQuyenDto quyen)
    {
        _quyen =  quyen;
    }
    public abstract List<string> getComboboxList();
    public abstract void onSearch(string txtSearch, string filter);
}