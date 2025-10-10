using System.Drawing.Drawing2D;
using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CUse
{
    public CUse(){}
    public DataGridView getDataView(int height, int width , int x , int y)
    {
        return new DataGridView
        {
            Location = new Point(x, y),
            Size = new Size(width, height),
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.Fixed3D,
            // Font = new Font("JetBrains Mono", 10f, FontStyle.Regular),
            RowHeadersVisible = false,
            GridColor = ColorTranslator.FromHtml("#2f4f4f"),
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#f5f5f5")
            },
            AllowUserToResizeColumns = false,
            AllowUserToResizeRows = false,
        };
    }

    public DataGridView GetTable(String[] columns)
    {
        DataGridView dgv = new DataGridView
        {
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.Fixed3D,
            RowHeadersVisible = false,
            GridColor = ColorTranslator.FromHtml("#2f4f4f"),
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#f5f5f5")
            },
            AllowUserToResizeColumns = false,
            AllowUserToResizeRows = false,
        };
        SetDefaultsDgvProperty(dgv);
        
        dgv.Dock = DockStyle.Fill;
        dgv.AutoSize = true;
        
        return dgv;
    }
    void SetDefaultsDgvProperty(DataGridView dgv)
    {
        dgv.AllowUserToAddRows = false;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.BackgroundColor = Color.White;
        dgv.BorderStyle = BorderStyle.Fixed3D;
        dgv.RowHeadersVisible = false;
        dgv.GridColor = ColorTranslator.FromHtml("#2f4f4f");
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = ColorTranslator.FromHtml("#f5f5f5")
        };
        dgv.AllowUserToResizeColumns = false;
        dgv.AllowUserToResizeRows = false;
    }

    public Bitmap CreateIconWithBackground(string svgPath, Color iconColor, Color bgColor, int canvas, int cornerRadius,
        int padding)
    {
        var svg = SvgDocument.Open(svgPath);
        foreach (var v in svg.Descendants().OfType<SvgVisualElement>())
        {
            v.Fill = new SvgColourServer(iconColor);
            v.Stroke = new SvgColourServer(iconColor);
        }

        int inner = canvas - padding * 2;
        var iconBmp = svg.Draw(inner, inner);

        var bmp = new Bitmap(canvas, canvas);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new SolidBrush(bgColor))
            using (var path = new GraphicsPath())
            {
                float r = cornerRadius * 2f;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(canvas - r, 0, r, r, 270, 90);
                path.AddArc(canvas - r, canvas - r, r, r, 0, 90);
                path.AddArc(0, canvas - r, r, r, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }

            g.DrawImage(iconBmp, padding, padding, inner, inner);
        }

        return bmp;
    }
}