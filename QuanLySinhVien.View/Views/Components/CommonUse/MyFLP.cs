namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class MyFLP : FlowLayoutPanel
{
    public MyFLP()
    {
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        DoubleBuffered = true;
        UpdateStyles();
    }
}