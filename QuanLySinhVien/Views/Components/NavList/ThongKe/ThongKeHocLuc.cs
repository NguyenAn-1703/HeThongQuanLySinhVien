namespace QuanLySinhVien.Views.Components.NavList;

public class ThongKeHocLuc : TableLayoutPanel
{
    public ThongKeHocLuc()
    {
        Init();
    }

    void Init()
    {
        Dock = DockStyle.Fill;
        BackColor = MyColor.GrayBackGround;
        Margin = new Padding(0);
        
        
    }
}