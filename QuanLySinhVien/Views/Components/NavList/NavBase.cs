using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Views.Components.NavList;

public abstract class NavBase : Panel
{
    //Định nghĩa những hàm dùng chung của các NavItem trong folder NavList/
    public NhomQuyenDto _quyen;
    public TaiKhoanDto _taiKhoan;

    public NavBase(NhomQuyenDto quyen, TaiKhoanDto taiKhoan)
    {
        _quyen = quyen;
        _taiKhoan = taiKhoan;
    }

    public abstract List<string> getComboboxList();
    public abstract void onSearch(string txtSearch, string filter);
}