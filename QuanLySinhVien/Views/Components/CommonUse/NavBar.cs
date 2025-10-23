using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class NavBar : Panel
{
    // private String[] _labels;
    // private String[] _imgText;

    public List<NavItem> ButtonArray;

    private NavItemValue[] _arrDataNavItem;
    private NavItemValue[] _arrDataNavItemForSV;

    //item đang được chọn
    public NavItem SelectedItem;

    public MyTLP _mainLayout;

    public event Action<String> OnSelect1Item;

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;

    private NhomQuyenDto _nhomQuyen;
    private List<ChucNangDto> _listAccess = new List<ChucNangDto>();

    private string _roleType = "admin";


    public NavBar(NhomQuyenDto nhomQuyen)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
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
            new NavItemValue { ID = "CHUONGTRINHDAOTAO", Svg = "chuongtrinhdaotao", Name = "Chương trình đào tạo" },
            new NavItemValue { ID = "HOCPHAN", Svg = "hocphan", Name = "Học phần" },
            new NavItemValue { ID = "PHONGHOC", Svg = "phonghoc", Name = "Phòng học" },
            new NavItemValue { ID = "CHUKYDAOTAO", Svg = "chukydaotao", Name = "Chu kỳ đào tạo" },
            new NavItemValue { ID = "KHOAHOC", Svg = "khoahoc", Name = "Khóa học" },
            new NavItemValue { ID = "TOCHUCTHI", Svg = "tochucthi", Name = "Tổ chức thi" },
            new NavItemValue { ID = "NHAPDIEM", Svg = "nhapdiem", Name = "Nhập điểm" },
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
        };

        ButtonArray = new List<NavItem>();
        this.Init();
    }

    void Init()
    {
        Dock = DockStyle.Fill;
        AutoScroll = true;

        _mainLayout = new MyTLP
        {
            BackColor = MyColor.GrayBackGround,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Margin = new Padding(0),
        };

        CheckRole();

        if (_roleType.Equals("admin"))
        {
            for (int i = 0; i < _arrDataNavItem.Length; i++)
            {
                if (_listAccess.Any(x =>
                        x.TenCN.Equals(_arrDataNavItem[i].ID) || _arrDataNavItem[i].ID.Equals("TRANGCHU")))
                {
                    NavItem navItem = new NavItem(i, _arrDataNavItem[i].Svg + ".svg", _arrDataNavItem[i].Name);
                    navItem.OnClickThisItem += this.UpdateStatusNavBar;
                    ButtonArray.Add(navItem);
                    _mainLayout.Controls.Add(ButtonArray[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < _arrDataNavItemForSV.Length; i++)
            {
                NavItem navItem = new NavItem(i, _arrDataNavItemForSV[i].Svg + ".svg", _arrDataNavItemForSV[i].Name);
                navItem.OnClickThisItem += this.UpdateStatusNavBar;
                ButtonArray.Add(navItem);
                _mainLayout.Controls.Add(ButtonArray[i]);
            }
        }


        this.Controls.Add(_mainLayout);

        UpdateSize();

        //Mặc định item đầu được chọn
        SelectedItem = ButtonArray[0];
        SelectedItem.ChangeToSelectStatus();
    }

    void CheckRole()
    {
        if (_nhomQuyen.TenNhomQuyen.Equals("SinhVien"))
        {
            _roleType = "SinhVien";
            return;
        }

        List<ChiTietQuyenDto> listCTQ = _chiTietQuyenController.GetByMaNQ(_nhomQuyen.MaNQ);
        foreach (ChiTietQuyenDto ct in listCTQ)
        {
            _listAccess.Add(_chucNangController.GetById(ct.MaCN));
        }
    }

    //Hàm được gọi call back để caapj nhật lại item được chonj mỗi khi 1 item đươc click
    void UpdateStatusNavBar(int index)
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
        int w = _mainLayout.Width + 6;
        this.Width = w;
    }
}