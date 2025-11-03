using QuanLySinhVien.Shared.Enums;
using Svg;

namespace QuanLySinhVien.View.Views.Components.ViewComponents;

public class NavItem : RoundTLP
{
    // thanh màu xanh nhor bên trái mooix item
    private readonly RoundTLP tag = new(false, true, true, false);

    public NavItem(int index, string path, string text)
    {
        Index = index;
        this.path = Path.Combine(AppContext.BaseDirectory, "Views", "img", path);
        Text = text;
        Init();
    }

    public int Index { get; set; }
    public bool IsSelected { get; set; }
    private string path { get; }
    public string Text { get; set; }

    public event Action<int> OnClickThisItem;

    private void Init()
    {
        TopLeft = TopRight = BottomLeft = BottomRight = false;
        BackColor = MyColor.GrayBackGround;
        Margin = new Padding(0, 5, 0, 0);
        // this.Padding = new Padding(0, 5, 0, 5);
        Dock = DockStyle.Fill;
        AutoSize = true;
        ColumnCount = 3;
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        Cursor = Cursors.Hand;
        try
        {
            if (Path.GetExtension(path).ToLower() != ".svg")
                throw new Exception("File không phải SVG!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Lỗi không có file");
        }

        var svgDocument = SvgDocument.Open(path);
        var btm = svgDocument.Draw();

        tag.BackColor = MyColor.GrayBackGround;
        tag.Size = new Size(5, 27);
        tag.Margin = new Padding(0, 0, 5, 0);
        tag.Anchor = AnchorStyles.Left;

        var pb = new PictureBox
        {
            Margin = new Padding(7, 7, 10, 7),
            Anchor = AnchorStyles.None,
            Size = new Size(20, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = btm
        };

        var content = new Label
        {
            MinimumSize = new Size(200, 0),
            Margin = new Padding(0, 3, 3, 3),
            Anchor = AnchorStyles.None,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Text = Text,
            Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold)
        };

        Controls.Add(tag);
        Controls.Add(pb);
        Controls.Add(content);

        MouseEnter += (sender, args) => OnHover();
        MouseLeave += (sender, args) => OnLeave();

        MouseClick += (sender, args) => OnClick();
        foreach (Control c in Controls)
        {
            c.MouseEnter += (sender, args) => OnHover();
            c.MouseLeave += (sender, args) => OnLeave();
            c.MouseClick += (sender, args) => OnClick();
        }
    }

    private void OnHover()
    {
        if (!IsSelected) ChangeToHoverStatus();
    }

    private void OnLeave()
    {
        if (!IsSelected) ChangeToNormalStatus();
    }

    private void OnClick()
    {
        OnClickThisItem?.Invoke(Index);
    }

    public void ChangeToHoverStatus()
    {
        IsSelected = false;
        tag.BackColor = MyColor.GrayHoverColor;
        BackColor = MyColor.GrayHoverColor;
    }

    public void ChangeToNormalStatus()
    {
        IsSelected = false;
        tag.BackColor = MyColor.GrayBackGround;
        BackColor = MyColor.GrayBackGround;
    }

    public void ChangeToSelectStatus()
    {
        tag.BackColor = MyColor.MainColor;
        BackColor = MyColor.GraySelectColor;
        IsSelected = true;
    }
}