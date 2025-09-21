using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class NavItem : TableLayoutPanel
{
    private String path { get; set; }
    private String text { get; set; }
    public NavItem(string path, string text)
    {
        this.path = Path.Combine(AppContext.BaseDirectory, "img", path);
        this.text = text;
        this.Init();
    }

    void Init()
    {
        this.BackColor = MyColor.GrayBackGround;
        Margin = new Padding(0, 5, 0, 0);
        this.Padding = new Padding(5);
        this.Dock = DockStyle.Fill;
        this.AutoSize = true;
        this.ColumnCount = 2;
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        try
        {
            if (Path.GetExtension(path).ToLower() != ".svg")
                throw new Exception("File không phải SVG!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Lỗi không có file");
        }

        SvgDocument svgDocument = SvgDocument.Open(this.path);
        Bitmap btm = svgDocument.Draw();
        
        PictureBox pb = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(20, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = btm
        };

        Label content = new Label
        {
            Anchor = AnchorStyles.None,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Text = this.text,
            Font = new GetFont.GetFont().GetMainFont(14, FontType.SemiBold)
        };
        
        this.Controls.Add(pb);
        this.Controls.Add(content);

        this.MouseEnter += (sender, args) => this.onHover();
        this.MouseLeave += (sender, args) => this.onLeave();
        this.MouseClick += (sender, args) => this.onClick();
        foreach (Control c in this.Controls)
        {
            c.MouseEnter += (sender, args) => this.onHover();
            c.MouseLeave += (sender, args) => this.onLeave();
            c.MouseClick += (sender, args) => this.onClick();
        }
        
    }
    void onHover()
    {
        this.BackColor = MyColor.GrayHoverColor;
    }
    void onLeave()
    {
        this.BackColor = MyColor.GrayBackGround;
    }
    void onClick()
    {
        this.BackColor = MyColor.GraySelectColor;
    }



}