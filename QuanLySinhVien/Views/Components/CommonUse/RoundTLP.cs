using System.Drawing.Drawing2D;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components.ViewComponents;

//MyTLP thêm bo góc
public class RoundTLP : MyTLP
{
    public bool Border;
    public bool TopLeft, TopRight, BottomRight, BottomLeft;

    public RoundTLP(bool topleft = true, bool topright = true, bool bottomright = true, bool bottomleft = true,
        bool border = false)
    {
        TopLeft = topleft;
        TopRight = topright;
        BottomRight = bottomright;
        BottomLeft = bottomleft;
        Border = border;
        Init();
    }

    public int BorderRadius { get; set; } = 10;
    public int BorderSize { get; set; } = 2;
    public Color BorderColor { get; set; } = MyColor.GraySelectColor;

    private void Init()
    {
        DoubleBuffered = true;
    }

    //override ghi đè thêm phương thức onPaint của lớp Control
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var rect = new Rectangle(0, 0, Width, Height);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


        using (var path = new GraphicsPath())
        {
            if (TopLeft) path.AddArc(rect.X, rect.Y, BorderRadius, BorderRadius, 180, 90);
            else path.AddLine(rect.X, rect.Y, rect.X, rect.Y);

            if (TopRight) path.AddArc(rect.Right - BorderRadius, rect.Y, BorderRadius, BorderRadius, 270, 90);
            else path.AddLine(rect.Right, rect.Y, rect.Right, rect.Y);

            if (BottomRight)
                path.AddArc(rect.Right - BorderRadius, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 0, 90);
            else path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom);

            if (BottomLeft) path.AddArc(rect.X, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 90, 90);
            else path.AddLine(rect.X, rect.Bottom, rect.X, rect.Bottom);

            path.CloseFigure();

            using (Brush brush = new SolidBrush(BackColor))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillPath(brush, path);
            }

            if (Border)
                using (var pen = new Pen(BorderColor, BorderSize))
                {
                    pen.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawPath(pen, path);
                }

            Region = new Region(path);
        }

        e.Graphics.SmoothingMode = SmoothingMode.None;
    }
}