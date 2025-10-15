using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class NavBar : Panel
{
    private String[] _labels;
    private String[] _imgText;

    public List<NavItem> ButtonArray;

    //item đang được chọn
    public NavItem SelectedItem;

    public TableLayoutPanel _mainLayout;

    public event Action<String> OnSelect1Item;

    public NavBar()
    {
        _labels = new[]
        {
            "Trang chủ", "Sinh viên", "Giảng viên", "Khoa", "Ngành", "Chương trình đào tạo", "Học phần", "Phòng học", "Chu kỳ đào tạo",
            "Tổ chức thi", "Nhập điểm", "Học phí", "Mở đăng ký học phần", "Quản lí tài khoản", "Phân quyền", "Thống kê"
        };
        _imgText = new[]
        {
            "trangchu", "sinhvien", "giangvien", "khoa", "nganh", "chuongtrinhdaotao", "hocphan", "phonghoc","chukydaotao",
            "tochucthi", "nhapdiem", "hocphi", "modangkyhocphan", "sinhvien", "phanquyen", "thongke"
        };
        ButtonArray = new List<NavItem>();
        this.Init();
    }

    void Init()
    {
        Dock = DockStyle.Fill;
        AutoScroll = true;

        _mainLayout = new TableLayoutPanel
        {
            BackColor = MyColor.GrayBackGround,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Margin = new Padding(0),
        };

        // _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        for (int i = 0; i < _labels.Length; i++)
        {
            NavItem navItem = new NavItem(i, _imgText[i] + ".svg", _labels[i]);
            navItem.OnClickThisItem += this.UpdateStatusNavBar;
            ButtonArray.Add(navItem);
            _mainLayout.Controls.Add(ButtonArray[i]);
        }

        this.Controls.Add(_mainLayout);
        
        UpdateSize();

        //Mặc định item đầu được chọn
        SelectedItem = ButtonArray[0];
        SelectedItem.ChangeToSelectStatus();
        
        
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