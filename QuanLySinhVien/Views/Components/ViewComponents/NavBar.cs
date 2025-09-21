using System.Drawing.Printing;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class NavBar: TableLayoutPanel
{
    private String[] labels;
    private String[] imgText;
    
    private List<NavItem> buttonArray = new List<NavItem>();
    //item đang được chọn
    private NavItem selectedItem;
    public NavBar()
    {
        labels = new[]
        {
            "Trang chủ", "Sinh viên", "Giảng viên", "Khoa", "Ngành", "Chương trình đào tạo", "Học phần", "Phòng học",
            "Tổ chức thi", "Nhập điểm", "Học phí", "Mở đăng ký học phần", "Quản lí tài khoản", "Phân quyền", "Thống kê"
        };
        imgText = new[]
        {
            "trangchu" , "sinhvien" , "giangvien" , "khoa" , "nganh" , "chuongtrinhdaotao" , "hocphan" , "phonghoc",
            "tochucthi" , "nhapdiem" , "hocphi" , "modangkyhocphan" , "sinhvien" , "phanquyen" , "thongke"
        };
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
            NavItem navItem = new NavItem(i,imgText[i] + ".svg", labels[i]);
            navItem.OnClickThisItem += this.UpdateStatusNavBar;
            buttonArray.Add(navItem);
            this.Controls.Add(buttonArray[i]);
        }

        //Mặc định item đầu được chọn
        selectedItem = buttonArray[0];
        selectedItem.ChangeToSelectStatus();
    }
    
    //Hàm được gọi call back để caapj nhật lại item được chonj mỗi khi 1 item đươc click
    void UpdateStatusNavBar(int index)
    {
        if (selectedItem.Index != index)
        {
            selectedItem.ChangeToNormalStatus();
            buttonArray[index].ChangeToSelectStatus();
            selectedItem = buttonArray[index];
        }
    }
}