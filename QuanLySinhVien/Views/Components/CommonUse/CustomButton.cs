using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomButton : RoundTLP
{
    public int PicWidth {get; set;}
    public int PicHeight {get; set;}
    public string Svg {get; set;}
    public int Pad { get; set; } = 3;
    
    public Color BackgroundColor {get; set;}

    public CustomButton(int width, int height, string svg, Color backColor)
    {
        PicWidth = width;
        PicHeight = height;
        Svg = svg;
        BackColor = backColor;
        Init();
    }

    void Init()
    {
        this.Margin = new Padding(0);
        this.Size = new Size(PicWidth + Pad * 2, PicHeight + Pad * 2);
        SetPictureBox();
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
}