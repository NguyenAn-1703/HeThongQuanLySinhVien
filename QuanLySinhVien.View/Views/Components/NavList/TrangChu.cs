using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.NavList;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components;

public class TrangChu : NavBase
{
    private readonly int _imgHeight = 626;
    private readonly int _imgWidth = 364;
    private readonly string[] _listSelectionForComboBox = new[] { "" };
    private BackgroundPic _image;
    private Label _lbl1, _lbl2, _lbl3, _lbl4;
    private MyTLP _leftPanel;

    public TrangChu(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        Init();
    }

    private void Init()
    {
        SuspendLayout();
        BackColor = MyColor.GrayBackGround;
        Dock = DockStyle.Fill;
        Margin = new Padding(0);

        var mainLayout = new MyTLP();
        mainLayout.SuspendLayout();
        mainLayout.Margin = new Padding(0);
        mainLayout.Dock = DockStyle.Fill;

        mainLayout.ColumnCount = 2;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _leftPanel = GetLeftPanel();
        mainLayout.Controls.Add(_leftPanel);

        // new BoxHome("Tiện lợi", "Quản lý thông tin nhanh gọn")

        SetImage();
        mainLayout.Controls.Add(_image);
        mainLayout.ResumeLayout();

        Controls.Add(mainLayout);

        SetContentInFrontOfImg();

        Resize += (sender, args) => { updateSize(); };

        ResumeLayout();
    }

    private void updateSize()
    {
        _image.SuspendLayout();
        _image.Height = Height;
        _image.Width = Height * _imgWidth / _imgHeight;
        _lbl2.Location = new Point(50, _image.Bottom - 250);
        _lbl3.Location = new Point(50, _image.Bottom - 130);
        _lbl4.Location = new Point(50, _image.Bottom - 100);
    }

    private void SetImage()
    {
        _image = new BackgroundPic
        {
            Margin = new Padding(0),
            BackgroundImage = GetPng.GetImage("img/jpg/homeimg.jpg"),
            Dock = DockStyle.Right,
            BackgroundImageLayout = ImageLayout.Zoom
        };
        _image.Paint += (sender, args) => Overlay(args);
    }

    private void Overlay(PaintEventArgs e)
    {
        var overlayColor = Color.FromArgb(100, 0, 0, 0);
        using (var brush = new SolidBrush(overlayColor))
        {
            e.Graphics.FillRectangle(brush, _image.ClientRectangle);
        }
    }

    private void SetContentInFrontOfImg()
    {
        _image.SuspendLayout();
        _lbl1 = new Label();
        _lbl1.Text = "SGU";
        _lbl1.AutoSize = true;
        _lbl1.Font = GetFont.GetFont.GetMainFont(13, FontType.ExtraBold);
        _lbl1.ForeColor = MyColor.White;
        _lbl1.BackColor = Color.Transparent;
        _lbl1.Location = new Point(_image.Location.X + 50, _image.Location.Y + 15);

        _lbl2 = new Label();
        _lbl2.Text = "“An investment in knowledge \npays the best interest.”";
        _lbl2.AutoSize = true;
        _lbl2.Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold);
        _lbl2.ForeColor = MyColor.White;
        _lbl2.BackColor = Color.Transparent;
        _lbl2.Location = new Point(50, _image.Bottom - 250);

        _lbl3 = new Label();
        _lbl3.Text = "Benjamin Franklin";
        _lbl3.AutoSize = true;
        _lbl3.Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold);
        _lbl3.ForeColor = MyColor.White;
        _lbl3.BackColor = Color.Transparent;
        _lbl3.Location = new Point(50, _image.Bottom - 150);

        _lbl4 = new Label();
        _lbl4.Text = "one of the Founding Fathers of the United States.";
        _lbl4.AutoSize = true;
        _lbl4.Font = GetFont.GetFont.GetMainFont(9, FontType.Regular);
        _lbl4.ForeColor = MyColor.White;
        _lbl4.BackColor = Color.Transparent;
        _lbl4.Location = new Point(50, _image.Bottom - 100);


        _image.Controls.Add(GetStar());
        _image.Controls.Add(_lbl1);
        _image.Controls.Add(_lbl2);
        _image.Controls.Add(_lbl3);
        _image.Controls.Add(_lbl4);

        _image.ResumeLayout();
    }

    private PictureBox GetStar()
    {
        var pb = new PictureBox
        {
            Location = new Point(_image.Location.X + 10, _image.Location.Y + 10),
            BackColor = Color.Transparent,
            Size = new Size(40, 40),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap("star.svg")
        };
        return pb;
    }

    private MyTLP GetLeftPanel()
    {
        var panel = new MyTLP();
        panel.SuspendLayout();
        panel.Margin = new Padding(0);
        panel.Padding = new Padding(40);
        panel.Dock = DockStyle.Fill;
        panel.RowCount = 5;
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

        var name = new Label();
        name.Text = "Hệ thống quản lý sinh viên \nSGU";
        name.AutoSize = true;
        name.Font = GetFont.GetFont.GetPlayFairFont(25, FontType.Regular);
        name.ForeColor = MyColor.MainColor;

        var box1 = new BoxHome("Tiện lợi", "Quản lý thông tin nhanh gọn. \nTối ưu thao tác người dùng.");
        var box2 = new BoxHome("Bảo mật", "Hệ thống bảo mật an toàn. \nCông nghệ xác thực tiên tiến nhất.");
        var box3 = new BoxHome("Nhanh chóng", "Lưu trữ và tải dữ liệu nhanh chóng. \nTối ưu hóa quy trình tải dữ liệu.");
        var box4 = new BoxHome("Thân thiện", "Thao tác dễ dàng. \nBiểu tượng gần gủi.");

        box1.Anchor = AnchorStyles.Left;
        box3.Anchor = AnchorStyles.Left;
        box2.Anchor = AnchorStyles.Right;
        box4.Anchor = AnchorStyles.Right;

        panel.Controls.Add(name);
        panel.Controls.Add(box1);
        panel.Controls.Add(box2);
        panel.Controls.Add(box3);
        panel.Controls.Add(box4);
        panel.ResumeLayout();
        return panel;
    }


    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(_listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}