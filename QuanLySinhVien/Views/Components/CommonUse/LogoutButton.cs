using QuanLySinhVien.Shared.Enums;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class LogoutButton : RoundTLP
{
    public LogoutButton()
    {
        Init();
    }

    public event Action OnClick;

    public void Init()
    {
        ColumnCount = 2;
        AutoSize = true;
        Dock = DockStyle.Fill;
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        BackColor = MyColor.Red;
        Margin = new Padding(3, 3, 3, 40);
        Padding = new Padding(5);
        Cursor = Cursors.Hand;

        var pb = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(20, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap("dangxuat.svg")
        };

        var text = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Đăng xuất",
            Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold)
        };

        Controls.Add(pb);
        Controls.Add(text);


        MouseEnter += (sender, args) => { BackColor = MyColor.RedHover; };
        MouseLeave += (sender, args) => { BackColor = MyColor.Red; };
        MouseDown += (sender, args) =>
        {
            BackColor = MyColor.RedClick;
            OnClick?.Invoke();
        };
        MouseUp += (sender, args) => { BackColor = MyColor.RedHover; };

        foreach (Control c in Controls)
        {
            c.MouseEnter += (sender, args) => { BackColor = MyColor.RedHover; };
            c.MouseLeave += (sender, args) => { BackColor = MyColor.Red; };
            c.MouseDown += (sender, args) =>
            {
                BackColor = MyColor.RedClick;
                OnClick?.Invoke();
            };
            c.MouseUp += (sender, args) => { BackColor = MyColor.RedHover; };
        }
    }
}