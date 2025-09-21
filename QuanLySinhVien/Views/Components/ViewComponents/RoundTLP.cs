using System.Drawing.Drawing2D;

namespace QuanLySinhVien.Views.Components.ViewComponents;
//TableLayoutPanel thêm bo góc
public class RoundTLP : TableLayoutPanel
{
    public int BorderRadius { get; set; } = 10;
    public Boolean TopLeft, TopRight, BottomRight, BottomLeft;

    public RoundTLP(Boolean topleft = true, Boolean topright = true, Boolean bottomright = true, Boolean bottomleft = true)
    {
        TopLeft = topleft;
        TopRight = topright;
        BottomRight = bottomright;
        BottomLeft = bottomleft;
    }
        
    //override ghi đè thêm phương thức onPaint của lớp Control
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

        using (GraphicsPath path = new GraphicsPath())
        {
            if(TopLeft) path.AddArc(rect.X, rect.Y, BorderRadius, BorderRadius, 180, 90);
            else path.AddLine(rect.X, rect.Y, rect.X, rect.Y);
            
            if(TopRight) path.AddArc(rect.Right - BorderRadius, rect.Y, BorderRadius, BorderRadius, 270, 90);
            else path.AddLine(rect.Right, rect.Y, rect.Right, rect.Y);
            
            if(BottomRight) path.AddArc(rect.Right - BorderRadius, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 0, 90);
            else path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom);
            
            if(BottomLeft) path.AddArc(rect.X, rect.Bottom - BorderRadius - 1, BorderRadius, BorderRadius, 90, 90);
            else path.AddLine(rect.X, rect.Bottom, rect.X, rect.Bottom);
            
            path.CloseFigure();
            
            this.Region = new Region(path);
        }
    }
}