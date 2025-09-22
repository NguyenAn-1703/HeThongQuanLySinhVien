using Svg;

namespace QuanLySinhVien.Views.Components.ViewComponents;

//nút thu gonj navbar
public class ToggleButton : RoundTLP
{
    public event Action OnClick;
    public String img { get; set; } = "toggle.svg";
    private PictureBox pb;
    public ToggleButton()
    {
        Init();
    }

    void Init()
    {
        this.BorderRadius = 6;
        this.Size = new Size(30, 30);
        this.BackColor = MyColor.LightGray;
        
        
        pb = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(12, 12),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap(img)
        };
        
        this.Controls.Add(pb);

        this.MouseEnter += (sender, args) => { this.BackColor = MyColor.GrayHoverColor; }; 
        this.MouseLeave += (sender, args) => { this.BackColor = MyColor.LightGray; }; 
        this.MouseDown += (sender, args) => { this.BackColor = MyColor.GraySelectColor; }; 
        this.MouseUp += (sender, args) => { this.BackColor = MyColor.GrayHoverColor; OnClick?.Invoke(); };
        
        this.Controls[0].MouseEnter += (sender, args) => { this.BackColor = MyColor.GrayHoverColor; }; 
        this.Controls[0].MouseLeave += (sender, args) => { this.BackColor = MyColor.LightGray; }; 
        this.Controls[0].MouseDown += (sender, args) => { this.BackColor = MyColor.GraySelectColor; }; 
        this.Controls[0].MouseUp += (sender, args) => { this.BackColor = MyColor.GrayHoverColor; OnClick?.Invoke(); };
        
    }

    public void ChangeImg(string file)
    {
        Bitmap btm = GetSvgBitmap.GetBitmap(file);
        this.pb.Image = btm;
    }
}