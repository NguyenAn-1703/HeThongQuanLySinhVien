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
            Image = GetBitmapBySvg(img)
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

    public Bitmap GetBitmapBySvg(string file)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "img", file);
        try
        {
            if (Path.GetExtension(path).ToLower() != ".svg")
                throw new Exception("File không phải SVG!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Lỗi không có file");
        }
        
        SvgDocument svgDocument = SvgDocument.Open(path);
        Bitmap btm = svgDocument.Draw();
        return btm;
    }

    public void ChangeImg(string file)
    {
        Bitmap btm = GetBitmapBySvg(file);
        this.pb.Image = btm;
    }
}