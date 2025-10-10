using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomButton : RoundTLP
{
    public int PicWidth {get; set;}
    public int PicHeight {get; set;}
    public string Svg {get; set;}
    public int Pad { get; set; } = 3;
    

    public event Action _mouseDown;
    
    public Color BackgroundColor {get; set;}
    public Color HoverColor { get; set; }
    public Color SelectColor { get; set; }


    public CustomButton(int width, int height, string svg, Color backColor, bool radius1 = true, bool radius2 = true, bool radius3 = true, bool radius4 = true, bool border = false)
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

    void Init()
    {
        BackColor = BackgroundColor;
        this.Margin = new Padding(0);
        this.Size = new Size(PicWidth + Pad * 2, PicHeight + Pad * 2);
        SetPictureBox();
        SetAction();
    }

    void SetPictureBox()
    {
        PictureBox pB = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(PicWidth, PicHeight),
            Image = GetSvgBitmap.GetBitmap(Svg),
            SizeMode = PictureBoxSizeMode.Zoom,
            Enabled = false
        };
        this.Controls.Add(pB);
    }

    void SetAction()
    {
        this.MouseDown += (sender, args) => OnMouseDown();
        this.MouseUp += (sender, args) => OnMouseUp();
        this.MouseEnter +=  (sender, args) => OnMouseEnter();
        this.MouseLeave +=  (sender, args) => OnMouseLeave();
    }

    void OnMouseDown()
    {
        this.BackColor = this.SelectColor;
        _mouseDown?.Invoke();
    }

    void OnMouseUp()
    {
        this.BackColor = this.BackgroundColor;
    }

    void OnMouseEnter()
    {
        this.BackColor = this.HoverColor;
    }

    void OnMouseLeave()
    {
        this.BackColor = this.BackgroundColor;
    }

    public void SetBorder()
    {
        
    }
}