using System.Drawing.Printing;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class NavBar: TableLayoutPanel
{
    private String[] labels = new[]
    {
        "Trang chủ", "Sinh viên", "Giảng viên", "Khoa", "Ngành", "Chương trình đào tạo", "Học phần", "Phòng học",
        "Tổ chức thi", "Nhập điểm", "Học phí", "Mở đăng ký học phần", "Quản lí tài khoản", "Phân quyền", "Thống kê"
    };
    private String[] imgText = new[]
    {
        "trangchu" , "sinhvien" , "giangvien" , "khoa" , "nganh" , "chuongtrinhdaotao" , "hocphan" , "phonghoc",
        "tochucthi" , "nhapdiem" , "hocphi" , "modangkyhocphan" , "sinhvien" , "phanquyen" , "thongke"
    };
    
    private List<NavItem> buttonArray = new List<NavItem>();
    
    public NavBar()
    {
        this.Init();
    }

    void Init()
    {
        BackColor = MyColor.GrayBackGround;
        AutoSize = true;
        ColumnCount = 1;
        Margin = new Padding(0);
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        for (int i = 0; i < labels.Length; i++)
        {
            NavItem navItem = new NavItem(imgText[i] + ".svg", labels[i]);
            buttonArray.Add(navItem);
            this.Controls.Add(buttonArray[i]);
        }
        
    }
}