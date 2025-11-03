namespace QuanLySinhVien.Views.Components.CommonUse;

public class MyTLP : TableLayoutPanel
{
    public MyTLP()
    {
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        DoubleBuffered = true;
        UpdateStyles();
    }
}