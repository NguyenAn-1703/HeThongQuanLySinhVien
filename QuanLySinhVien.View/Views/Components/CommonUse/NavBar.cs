using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.ViewComponents;

public class NavBar : Panel
{
    private readonly NavItemValue[] _arrDataNavItem;
    private readonly NavItemValue[] _arrDataNavItemForSV;
    private readonly List<ChucNangDto> _listAccess = new();
    private readonly List<NavItemValue> _listDataNavItemForSV;

    private readonly NhomQuyenDto _nhomQuyen;

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;

    private LichDangKyController _lichDangKyController;

    public MyTLP _mainLayout;

    private string _roleType = "admin";
    // private String[] _labels;
    // private String[] _imgText;

    public List<NavItem> ButtonArray;

    //item đang được chọn
    public NavItem SelectedItem;

    public NavBar(NhomQuyenDto nhomQuyen)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _lichDangKyController = LichDangKyController.GetInstance();
        _nhomQuyen = nhomQuyen;

        //     "Trang chủ", "Sinh viên", "Giảng viên", "Khoa", "Ngành", "Chương trình đào tạo", "Học phần", "Phòng học", "Chu kỳ đào tạo","Khóa học",
        //     "Tổ chức thi", "Nhập điểm", "Học phí", "Mở đăng ký học phần", "Quản lí tài khoản", "Phân quyền", "Thống kê"


        //     "trangchu", "sinhvien", "giangvien", "khoa", "nganh", "chuongtrinhdaotao", "hocphan", "phonghoc","chukydaotao","khoahoc",
        //     "tochucthi", "nhapdiem", "hocphi", "modangkyhocphan", "taikhoan", "phanquyen", "thongke"


        _arrDataNavItem = new[]
        {
            new NavItemValue { ID = "TRANGCHU", Svg = "trangchu", Name = "Trang chủ" },
            new NavItemValue { ID = "SINHVIEN", Svg = "sinhvien", Name = "Sinh viên" },
            new NavItemValue { ID = "GIANGVIEN", Svg = "giangvien", Name = "Giảng viên" },
            new NavItemValue { ID = "KHOA", Svg = "khoa", Name = "Khoa" },
            new NavItemValue { ID = "NGANH", Svg = "nganh", Name = "Ngành" },
            new NavItemValue { ID = "LOP", Svg = "lop", Name = "Lớp" },
            new NavItemValue { ID = "CHUONGTRINHDAOTAO", Svg = "chuongtrinhdaotao", Name = "Chương trình đào tạo" },
            new NavItemValue { ID = "HOCPHAN", Svg = "hocphan", Name = "Học phần" },
            new NavItemValue { ID = "PHONGHOC", Svg = "phonghoc", Name = "Phòng học" },
            new NavItemValue { ID = "CHUKYDAOTAO", Svg = "chukydaotao", Name = "Chu kỳ đào tạo" },
            new NavItemValue { ID = "KHOAHOC", Svg = "khoahoc", Name = "Khóa học" },
            new NavItemValue { ID = "TOCHUCTHI", Svg = "tochucthi", Name = "Tổ chức thi" },
            new NavItemValue { ID = "NHAPDIEM", Svg = "nhapdiem", Name = "Nhập điểm" },
            new NavItemValue { ID = "NHAPDIEMTHI", Svg = "nhapdiemthi", Name = "Nhập điểm thi" },
            new NavItemValue { ID = "HOCPHI", Svg = "hocphi", Name = "Học phí" },
            new NavItemValue { ID = "MODANGKYHOCPHAN", Svg = "modangkyhocphan", Name = "Mở đăng ký học phần" },
            new NavItemValue { ID = "TAIKHOAN", Svg = "taikhoan", Name = "Quản lí tài khoản" },
            new NavItemValue { ID = "PHANQUYEN", Svg = "phanquyen", Name = "Phân quyền" },
            new NavItemValue { ID = "THONGKE", Svg = "thongke", Name = "Thống kê" }
        };

        _arrDataNavItemForSV = new[]
        {
            new NavItemValue { ID = "TRANGCHU", Svg = "trangchu", Name = "Trang chủ" },
            new NavItemValue { ID = "THONGTINSINHVIEN", Svg = "sinhvien", Name = "Thông tin cá nhân" },
            new NavItemValue { ID = "CHITIETLICHTHI", Svg = "chitietlichthi", Name = "Chi tiết lịch thi" },
            new NavItemValue { ID = "CHITIETHOCPHI", Svg = "chitiethocphi", Name = "Chi tiết học phí" },
        };
        _listDataNavItemForSV = _arrDataNavItemForSV.ToList();
        if (ValidateDangKyHP())
            _listDataNavItemForSV.Add(new NavItemValue
                { ID = "DANGKYHOCPHAN", Svg = "dangkyhocphan", Name = "Đăng ký học phần" });

        ButtonArray = new List<NavItem>();
        Init();
    }

    public event Action<string> OnSelect1Item;

    private void Init()
    {
        Dock = DockStyle.Fill;
        AutoScroll = true;

        _mainLayout = new MyTLP
        {
            BackColor = MyColor.GrayBackGround,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Margin = new Padding(0)
        };

        CheckRole();

        if (_roleType.Equals("admin"))
        {
            var index = 0;
            for (var i = 0; i < _arrDataNavItem.Length; i++)
                if (_listAccess.Any(x =>
                        x.TenCN.Equals(_arrDataNavItem[i].ID) || _arrDataNavItem[i].ID.Equals("TRANGCHU")))
                {
                    var navItem = new NavItem(index, _arrDataNavItem[i].Svg + ".svg", _arrDataNavItem[i].Name);
                    index++;
                    navItem.OnClickThisItem += UpdateStatusNavBar;
                    ButtonArray.Add(navItem);
                }

            foreach (var item in ButtonArray) _mainLayout.Controls.Add(item);
        }
        else
        {
            for (var i = 0; i < _listDataNavItemForSV.Count; i++)
            {
                var navItem = new NavItem(i, _listDataNavItemForSV[i].Svg + ".svg", _listDataNavItemForSV[i].Name);
                navItem.OnClickThisItem += UpdateStatusNavBar;
                ButtonArray.Add(navItem);
            }

            foreach (var item in ButtonArray) _mainLayout.Controls.Add(item);
        }


        Controls.Add(_mainLayout);

        UpdateSize();

        //Mặc định item đầu được chọn
        SelectedItem = ButtonArray[0];
        SelectedItem.ChangeToSelectStatus();
    }

    private void CheckRole()
    {
        if (_nhomQuyen.TenNhomQuyen.Equals("SinhVien"))
        {
            _roleType = "SinhVien";
            return;
        }

        List<ChiTietQuyenDto> listCTQ = _chiTietQuyenController.GetByMaNQ(_nhomQuyen.MaNQ);
        foreach (var ct in listCTQ) _listAccess.Add(_chucNangController.GetById(ct.MaCN));
    }

    //Hàm được gọi call back để caapj nhật lại item được chonj mỗi khi 1 item đươc click
    private void UpdateStatusNavBar(int index)
    {
        if (SelectedItem.Index != index)
        {
            SelectedItem.ChangeToNormalStatus();
            ButtonArray[index].ChangeToSelectStatus();
            SelectedItem = ButtonArray[index];
            //call back ra MyHome
            OnSelect1Item?.Invoke(SelectedItem.Text);
        }
    }

    public void UpdateSize()
    {
        var w = _mainLayout.Width + 6;
        Width = w;
    }

    private bool ValidateDangKyHP()
    {
        List<LichDangKyDto> listLichDk = _lichDangKyController.GetAll();
        var now = DateTime.Now;
        foreach (var lich in listLichDk)
            if (now >= lich.ThoiGianBatDau && now <= lich.ThoiGianKetThuc)
            {
                Console.WriteLine(lich.MaLichDK + " " + lich.ThoiGianBatDau + " " + lich.ThoiGianKetThuc);
                Console.WriteLine(now);
                return true;
            }

        return false;
    }
}