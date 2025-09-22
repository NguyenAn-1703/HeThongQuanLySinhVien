using System.Drawing.Drawing2D;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class RoundButton : Button
{
    public int BorderRadius { get; set; } = 5;

    protected override void OnPaint(PaintEventArgs pevent)
    {
        GraphicsPath path = new GraphicsPath();
        int radius = BorderRadius;
        path.AddArc(0, 0, radius, radius, 180, 90);
        path.AddArc(Width - radius, 0, radius, radius, 270, 90);
        path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90);
        path.AddArc(0, Height - radius, radius, radius, 90, 90);
        path.CloseAllFigures();
        this.Region = new Region(path);
        base.OnPaint(pevent);
    }
}