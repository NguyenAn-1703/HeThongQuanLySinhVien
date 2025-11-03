namespace QuanLySinhVien.View.Views.Components.ViewComponents;

//nút thu gonj navbar
public class ToggleButton : RoundTLP
{
    private PictureBox pb;

    public ToggleButton()
    {
        Init();
    }

    public string img { get; set; } = "toggle.svg";
    public event Action OnClick;

    private void Init()
    {
        BorderRadius = 6;
        Size = new Size(30, 30);
        BackColor = MyColor.LightGray;


        pb = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(12, 12),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap(img)
        };

        Controls.Add(pb);

        MouseEnter += (sender, args) => { BackColor = MyColor.GrayHoverColor; };
        MouseLeave += (sender, args) => { BackColor = MyColor.LightGray; };
        MouseDown += (sender, args) => { BackColor = MyColor.GraySelectColor; };
        MouseUp += (sender, args) =>
        {
            BackColor = MyColor.GrayHoverColor;
            OnClick?.Invoke();
        };

        Controls[0].MouseEnter += (sender, args) => { BackColor = MyColor.GrayHoverColor; };
        Controls[0].MouseLeave += (sender, args) => { BackColor = MyColor.LightGray; };
        Controls[0].MouseDown += (sender, args) => { BackColor = MyColor.GraySelectColor; };
        Controls[0].MouseUp += (sender, args) =>
        {
            BackColor = MyColor.GrayHoverColor;
            OnClick?.Invoke();
        };
    }

    public void ChangeImg(string file)
    {
        var btm = GetSvgBitmap.GetBitmap(file);
        pb.Image = btm;
    }
}