namespace QuanLySinhVien.Views.Components;

public class MyUc : UserControl
{
    public MyUc(Size size, Color backColor)
    {
        base.BackColor = backColor;
        Size = size;
    }
}