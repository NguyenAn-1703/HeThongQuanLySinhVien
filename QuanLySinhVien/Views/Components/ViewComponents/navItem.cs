using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class NavItem : RoundTLP
{
    public int Index { get; set; }
    public Boolean IsSelected { get; set; } = false;
    private String path { get; set; }
    private String text { get; set; }

    public event Action<int> OnClickThisItem;
    
    // thanh màu xanh nhor bên trái mooix item
    private RoundTLP tag = new RoundTLP(false, true, true, false);
    public NavItem(int index, string path, string text)
    {
        this.Index = index;        
        this.path = Path.Combine(AppContext.BaseDirectory, "img", path);
        this.text = text;
        this.Init();
    }

    void Init()
    {
        this.BackColor = MyColor.GrayBackGround;
        Margin = new Padding(0, 5, 0, 0);
        this.Padding = new Padding(0, 5, 5, 5);
        this.Dock = DockStyle.Fill;
        this.AutoSize = true;
        this.ColumnCount = 3;
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
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

        tag.BackColor = MyColor.GrayBackGround;
        tag.Size = new Size(7, 24);
        tag.Margin = new Padding(0, 0, 0, 0);
        tag.Anchor = AnchorStyles.Left;
        
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
        
        this.Controls.Add(tag);
        this.Controls.Add(pb);
        this.Controls.Add(content);

        this.MouseEnter += (sender, args) => this.OnHover();
        this.MouseLeave += (sender, args) => this.OnLeave();
        
        this.MouseClick += (sender, args) => this.OnClick();
        foreach (Control c in this.Controls)
        {
            c.MouseEnter += (sender, args) => this.OnHover();
            c.MouseLeave += (sender, args) => this.OnLeave();
            c.MouseClick += (sender, args) => this.OnClick();
        }
    }

    void OnHover()
    {
        if (!this.IsSelected) ChangeToHoverStatus();
    }

    void OnLeave()
    {
        if (!this.IsSelected) ChangeToNormalStatus();
    }

    void OnClick()
    {
        OnClickThisItem?.Invoke(this.Index);
    }
    public void ChangeToHoverStatus()
    {
        this.IsSelected = false;
        this.tag.BackColor = MyColor.GrayHoverColor;
        this.BackColor = MyColor.GrayHoverColor;
    }
    public void ChangeToNormalStatus()
    {
        this.IsSelected = false;
        this.tag.BackColor = MyColor.GrayBackGround;
        this.BackColor = MyColor.GrayBackGround;
    }
    public void ChangeToSelectStatus()
    {
        this.tag.BackColor = MyColor.MainColor;
        this.BackColor = MyColor.GraySelectColor;
        this.IsSelected = true;
    }

}