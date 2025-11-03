using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class CustomButton : RoundTLP
{
    public CustomButton(int width, int height, string svg, Color backColor, bool radius1 = true, bool radius2 = true,
        bool radius3 = true, bool radius4 = true, bool border = false)
    {
        PicWidth = width;
        PicHeight = height;
        Svg = svg;
        BackgroundColor = backColor;
        HoverColor = backColor; //mặc định
        SelectColor = backColor;
        TopLeft = radius1;
        TopRight = radius2;
        BottomRight = radius3;
        BottomLeft = radius4;
        Border = border;
        Init();
    }

    public int PicWidth { get; set; }
    public int PicHeight { get; set; }
    public string Svg { get; set; }
    public int Pad { get; set; } = 3;

    public Color BackgroundColor { get; set; }
    public Color HoverColor { get; set; }
    public Color SelectColor { get; set; }

    public event Action LeaveActionTable;


    public event Action _mouseDown;

    private void Init()
    {
        BackColor = BackgroundColor;
        Margin = new Padding(0);
        Size = new Size(PicWidth + Pad * 2, PicHeight + Pad * 2);
        SetPictureBox();
        SetAction();
    }

    private void SetPictureBox()
    {
        var pB = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(PicWidth, PicHeight),
            Image = GetSvgBitmap.GetBitmap(Svg),
            SizeMode = PictureBoxSizeMode.Zoom,
            Enabled = false
        };
        Controls.Add(pB);
    }

    private void SetAction()
    {
        MouseDown += (sender, args) => OnMouseDown();
        MouseUp += (sender, args) => OnMouseUp();
        MouseEnter += (sender, args) => OnMouseEnter();
        MouseLeave += (sender, args) => OnMouseLeave();
    }

    public virtual void OnMouseDown()
    {
        BackColor = SelectColor;
        _mouseDown?.Invoke();
    }

    private void OnMouseUp()
    {
        BackColor = BackgroundColor;
    }

    private void OnMouseEnter()
    {
        BackColor = HoverColor;
    }

    private void OnMouseLeave()
    {
        BackColor = BackgroundColor;
        LeaveActionTable?.Invoke();
    }

    public void SetBorder()
    {
    }
}