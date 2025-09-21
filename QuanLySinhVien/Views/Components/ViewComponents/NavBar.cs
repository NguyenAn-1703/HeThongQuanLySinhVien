namespace QuanLySinhVien.Views.Components.ViewComponents;

public class NavBar: TableLayoutPanel
{
    private String[] _labels;
    private String[] _imgText;
    
    private List<NavItem> _buttonArray;
    //item đang được chọn
    public NavItem SelectedItem;

    public event Action<String> OnSelect1Item;
    public NavBar()
    {
        _labels = new[]
        {
            "Trang chủ", "Sinh viên", "Giảng viên", "Khoa", "Ngành", "Chương trình đào tạo", "Học phần", "Phòng học",
            "Tổ chức thi", "Nhập điểm", "Học phí", "Mở đăng ký học phần", "Quản lí tài khoản", "Phân quyền", "Thống kê"
        };
        _imgText = new[]
        {
            "trangchu" , "sinhvien" , "giangvien" , "khoa" , "nganh" , "chuongtrinhdaotao" , "hocphan" , "phonghoc",
            "tochucthi" , "nhapdiem" , "hocphi" , "modangkyhocphan" , "sinhvien" , "phanquyen" , "thongke"
        };
        _buttonArray = new List<NavItem>();
        this.Init();
    }

    void Init()
    {
        BackColor = MyColor.GrayBackGround;
        AutoSize = true;
        ColumnCount = 1;
        Margin = new Padding(0);
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        
        for (int i = 0; i < _labels.Length; i++)
        {
            NavItem navItem = new NavItem(i,_imgText[i] + ".svg", _labels[i]);
            navItem.OnClickThisItem += this.UpdateStatusNavBar;
            _buttonArray.Add(navItem);
            this.Controls.Add(_buttonArray[i]);
        }

        //Mặc định item đầu được chọn
        SelectedItem = _buttonArray[0];
        SelectedItem.ChangeToSelectStatus();
    }
    
    //Hàm được gọi call back để caapj nhật lại item được chonj mỗi khi 1 item đươc click
    void UpdateStatusNavBar(int index)
    {
        if (SelectedItem.Index != index)
        {
            SelectedItem.ChangeToNormalStatus();
            _buttonArray[index].ChangeToSelectStatus();
            SelectedItem = _buttonArray[index];
            //call back ra MyHome
            OnSelect1Item?.Invoke(SelectedItem.Text);
        }
    }
}