using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components;

public class TrangChu : NavBase
{
    private string[] _listSelectionForComboBox = new[] { "" };
    private BackgroundPic _image;
    private int _imgWidth = 364;
    private int _imgHeight = 626;
    private Label _lbl1, _lbl2, _lbl3, _lbl4;
    public TrangChu()
    {
        Init();
    }

    private void Init()
    {
        this.SuspendLayout();
        BackColor = MyColor.GrayBackGround;
        Dock = DockStyle.Fill;
        Margin = new Padding(0);

        TableLayoutPanel mainLayout = new TableLayoutPanel();
        mainLayout.Margin = new Padding(0);
        mainLayout.Dock = DockStyle.Fill;

        mainLayout.ColumnCount = 2;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        mainLayout.Controls.Add(GetLeftPanel());

        // new BoxHome("Tiện lợi", "Quản lý thông tin nhanh gọn")
        
        SetImage();
        mainLayout.Controls.Add(_image);
        this.Controls.Add(mainLayout);
        
        SetContentInFrontOfImg();
        
        this.Resize += (sender, args) => { updateSize(); };
        this.ResumeLayout();
    }

    void updateSize()
    {
        this.SuspendLayout();
        _image.SuspendLayout();
        _image.Height = this.Height;
        _image.Width = this.Height * _imgWidth / _imgHeight;
        _lbl2.Location = new Point(50, _image.Bottom - 250);
        _lbl3.Location = new Point(50, _image.Bottom - 130);
        _lbl4.Location = new Point(50, _image.Bottom - 100);
        this.ResumeLayout();
        _image.ResumeLayout();
    }

    void SetImage()
    {
        _image = new BackgroundPic
        {
            Margin = new Padding(0),
            BackgroundImage = GetPng.GetImage("img/jpg/homeimg.jpg"),
            Dock = DockStyle.Right,
            BackgroundImageLayout = ImageLayout.Zoom,
        };
        _image.Paint += (sender, args) => Overlay(args);
    }
    
    private void Overlay(PaintEventArgs e)
    {
        Color overlayColor = Color.FromArgb(100, 0, 0, 0); 
        using (SolidBrush brush = new SolidBrush(overlayColor))
        {
            e.Graphics.FillRectangle(brush, _image.ClientRectangle);
        }
    }

    void SetContentInFrontOfImg()
    {
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
    }

    PictureBox GetStar()
    {
        PictureBox pb = new PictureBox
        {
            Location = new Point(_image.Location.X + 10, _image.Location.Y + 10),
            BackColor = Color.Transparent,
            Size = new Size(40, 40),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap("star.svg")
        };
        return pb;
    }

    TableLayoutPanel GetLeftPanel()
    {
        TableLayoutPanel panel = new TableLayoutPanel();
        panel.Margin = new Padding(0);
        panel.Padding = new Padding(40);
        panel.Dock = DockStyle.Fill;
        panel.RowCount = 5;
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        
        Label name = new Label();
        name.Text = "Hệ thống quản lý sinh viên \nSGU";
        name.AutoSize = true;
        name.Font = GetFont.GetFont.GetPlayFairFont(25, FontType.Regular);
        name.ForeColor = MyColor.MainColor;

        BoxHome box1 = new BoxHome("Tiện lợi", "Quản lý thông tin nhanh gọn.");
        BoxHome box2 = new BoxHome("Bảo mật", "Hệ thống bảo mật an toàn.");
        BoxHome box3 = new BoxHome("Nhanh chóng", "Lưu trữ và tải dữ liệu nhanh chóng.");
        BoxHome box4 = new BoxHome("Thân thiện", "Thao tác dễ dàng.");

        box1.Anchor = AnchorStyles.Left;
        box3.Anchor = AnchorStyles.Left;
        box2.Anchor = AnchorStyles.Right;
        box4.Anchor = AnchorStyles.Right;
        
        panel.Controls.Add(name);
        panel.Controls.Add(box1);
        panel.Controls.Add(box2);
        panel.Controls.Add(box3);
        panel.Controls.Add(box4);
        
        return panel;

    }


    public override List<string> getComboboxList()
        {
            return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
        }
    
    }