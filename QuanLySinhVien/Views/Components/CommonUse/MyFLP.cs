namespace QuanLySinhVien.Views.Components.CommonUse;

public class MyFLP : FlowLayoutPanel
{
    public MyFLP()
    {
        this.SetStyle(ControlStyles.UserPaint, true);
        this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        this.SetStyle(ControlStyles.ResizeRedraw, true);
        this.DoubleBuffered = true;
        this.UpdateStyles();
    }
}