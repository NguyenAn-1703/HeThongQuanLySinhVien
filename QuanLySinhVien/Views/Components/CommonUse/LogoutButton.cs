using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class LogoutButton : RoundTLP
{
    public event Action OnClick;
    public LogoutButton()
    {
        Init();

    }
    public void Init()
    {
        this.ColumnCount = 2;
        this.AutoSize = true;
        this.Dock = DockStyle.Fill;
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        this.BackColor = MyColor.Red;
        this.Margin = new Padding(3, 3, 3, 40);
        this.Padding = new Padding(5);
        this.Cursor = Cursors.Hand;

        PictureBox pb = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(20, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap("dangxuat.svg")
        };

        Label text = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Đăng xuất",
            Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold),
        };
        
        this.Controls.Add(pb);
        this.Controls.Add(text);


        this.MouseEnter += (sender, args) => { this.BackColor = MyColor.RedHover; };
        this.MouseLeave += (sender, args) => { this.BackColor = MyColor.Red; };
        this.MouseDown += (sender, args) => { this.BackColor = MyColor.RedClick; OnClick?.Invoke(); };
        this.MouseUp += (sender, args) => { this.BackColor = MyColor.RedHover;};
        
        foreach (Control c in this.Controls)
        {
            c.MouseEnter += (sender, args) => { this.BackColor = MyColor.RedHover; };
            c.MouseLeave += (sender, args) => { this.BackColor = MyColor.Red; };
            c.MouseDown += (sender, args) => { this.BackColor = MyColor.RedClick; OnClick?.Invoke(); };
            c.MouseUp += (sender, args) => { this.BackColor = MyColor.RedHover; };
        }
    }
    
}