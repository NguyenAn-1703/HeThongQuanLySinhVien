using System.Drawing.Drawing2D;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components.ViewComponents;
//MyTLP thêm bo góc
public class RoundTLP : MyTLP
{
    public int BorderRadius { get; set; } = 10;
    public Boolean TopLeft, TopRight, BottomRight, BottomLeft;
    public Boolean Border;
    public int BorderSize { get; set; } = 2;
    public Color BorderColor { get; set; } = MyColor.GraySelectColor;

    public RoundTLP(Boolean topleft = true, Boolean topright = true, Boolean bottomright = true, Boolean bottomleft = true, bool border = false)
    {
        TopLeft = topleft;
        TopRight = topright;
        BottomRight = bottomright;
        BottomLeft = bottomleft;
        Border = border;
        Init();
        
    }

    void Init()
    {
        DoubleBuffered = true;
    }
        
    //override ghi đè thêm phương thức onPaint của lớp Control
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
        
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using (GraphicsPath path = new GraphicsPath())
        {
            if(TopLeft) path.AddArc(rect.X, rect.Y, BorderRadius, BorderRadius, 180, 90);
            else path.AddLine(rect.X, rect.Y, rect.X, rect.Y);
            
            if(TopRight) path.AddArc(rect.Right - BorderRadius, rect.Y, BorderRadius, BorderRadius, 270, 90);
            else path.AddLine(rect.Right, rect.Y, rect.Right, rect.Y);
            
            if(BottomRight) path.AddArc(rect.Right - BorderRadius, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 0, 90);
            else path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom);
            
            if(BottomLeft) path.AddArc(rect.X, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 90, 90);
            else path.AddLine(rect.X, rect.Bottom, rect.X, rect.Bottom);
            
            path.CloseFigure();
            
            this.Region = new Region(path);

            if (Border)
            {
                using (Pen pen = new Pen(BorderColor, BorderSize))
                {
                    pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset; 
                    e.Graphics.DrawPath(pen, path);
                }
            }

        }
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        

    }
}