namespace QuanLySinhVien.Views.Components;

public class MyPanel : Panel
{
    public MyPanel(){}
    public MyPanel(Size size, Color backColor, DockStyle dockStyle)
    {
        base.BackColor = backColor;
        base.Dock = dockStyle;
        base.Size = size;
    }
}