namespace QuanLySinhVien.Views.Components.NavList;

public abstract class NavBase : Panel
{
    //Định nghĩa những hàm dùng chung của các NavItem trong folder NavList/
    public abstract List<string> getComboboxList();
    public abstract void onSearch(string txtSearch);
}