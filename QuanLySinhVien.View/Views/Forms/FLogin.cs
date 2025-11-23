using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.utils;
using QuanLySinhVien.View.Views.Components;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.GetFont;
using QuanLySinhVien.View.Views.Components.Home;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Forms;

public class FLogin : Form
{
    private readonly NhomQuyenController _nhomQuyenController;
    private readonly TaiKhoanController _taiKhoanController;
    private LabelTextField _passTfl;

    private TaiKhoanDto _taiKhoan;
    private LabelTextField _usrnameTfl;
    private RoundTLP btnDangNhap;

    public FLogin()
    {
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _taiKhoanController = TaiKhoanController.getInstance();
        Init();
    }

    private void Init()
    {
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1000, 600);
        FormBorderStyle = FormBorderStyle.None;

        BackColor = MyColor.White;

        var mainPanel = new TableLayoutPanel();
        mainPanel.BackColor = MyColor.White;
        mainPanel.ColumnCount = 2;
        mainPanel.Dock = DockStyle.Fill;
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

        mainPanel.Controls.Add(GetImg());
        mainPanel.Controls.Add(GetControlPanel());


        _usrnameTfl.SetText("admin");
        // _usrnameTfl.SetText("sinhvien");
        _passTfl.SetPassword("123456");

        Controls.Add(mainPanel);

        SetAciton();
    }

    //img
    public PictureBox GetImg()
    {
        var pic = new PictureBox
        {
            Image = GetPng.GetImage("img/png/Truong.png"),
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.StretchImage,
            Margin = new Padding(0),
            BorderStyle = BorderStyle.None
        };
        return pic;
    }

    private TableLayoutPanel GetBtnThoat()
    {
        var Button = new TableLayoutPanel
        {
            Margin = new Padding(0),
            AutoSize = true
        };
        var pb = new PictureBox
        {
            Margin = new Padding(0),
            Image = GetSvgBitmap.GetBitmap("exitbutton.svg"),
            Size = new Size(40, 40),
            SizeMode = PictureBoxSizeMode.Zoom,
            BorderStyle = BorderStyle.None
        };
        Button.Dock = DockStyle.Right;

        Button.Controls.Add(pb);

        Button.MouseEnter += (sender, args) => { Button.BackColor = MyColor.GrayHoverColor; };
        Button.MouseLeave += (sender, args) => { Button.BackColor = MyColor.White; };
        Button.MouseClick += (sender, args) => { Application.Exit(); };

        foreach (Control c in Button.Controls)
        {
            c.MouseEnter += (sender, args) => { Button.BackColor = MyColor.GrayHoverColor; };
            c.MouseLeave += (sender, args) => { Button.BackColor = MyColor.White; };
            c.MouseClick += (sender, args) => { Application.Exit(); };
        }

        return Button;
    }

    private TableLayoutPanel GetControlPanel()
    {
        var controlPanel = new TableLayoutPanel
        {
            RowCount = 3,
            Margin = new Padding(0)
        };
        controlPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        controlPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        controlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        controlPanel.Dock = DockStyle.Fill;
        // controlPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

        //Tiêu đề
        var title = new Label();
        title.Text = "Hệ thống quản lý sinh viên SGU";
        title.Font = GetFont.GetMainFont(12, FontType.Black);
        title.AutoSize = true;
        title.Anchor = AnchorStyles.None;

        /////////////////////form điền
        var loginForm = new TableLayoutPanel
        {
            RowCount = 6
        };
        loginForm.AutoSize = true;
        loginForm.Anchor = AnchorStyles.None;
        loginForm.Dock = DockStyle.Fill;
        loginForm.Padding = new Padding(120, 50, 120, 50);
        // loginForm.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        // loginForm.BackColor = MyColor.GrayHoverColor;

        var titleWb = new Label();
        titleWb.Anchor = AnchorStyles.None;
        titleWb.AutoSize = true;
        titleWb.Text = "Welcome Back";
        titleWb.Font = GetFont.GetMainFont(20, FontType.Black);
        titleWb.ForeColor = MyColor.MainColor;
        titleWb.Margin = new Padding(3, 3, 3, 10);

        var title2 = new Label();
        title2.Anchor = AnchorStyles.None;
        title2.AutoSize = true;
        title2.Text = "Vui lòng đăng nhập vào ứng dụng";
        title2.Font = GetFont.GetMainFont(8, FontType.Regular);
        title2.Margin = new Padding(3, 3, 3, 20);


        loginForm.Controls.Add(titleWb);
        loginForm.Controls.Add(title2);

        _usrnameTfl = new LabelTextField("Tên đăng nhập", TextFieldType.NormalText);
        _passTfl = new LabelTextField("Mật khẩu", TextFieldType.Password);

        loginForm.Controls.Add(_usrnameTfl);
        loginForm.Controls.Add(_passTfl);

        var lblQmk = new Label();
        lblQmk.Dock = DockStyle.Right;
        lblQmk.AutoSize = true;
        lblQmk.Text = "Quên mật khẩu ?";
        lblQmk.Font = GetFont.GetMainFont(10, FontType.SemiBoldItalic);

        loginForm.Controls.Add(lblQmk);

        loginForm.Controls.Add(GetButtonDangNhap());
        /////////////////////form điền


        //thanh điều khiển
        controlPanel.Controls.Add(GetBtnThoat());

        controlPanel.Controls.Add(title);

        controlPanel.Controls.Add(loginForm);


        return controlPanel;
    }

    private RoundTLP GetButtonDangNhap()
    {
        btnDangNhap = new RoundTLP();
        btnDangNhap.BorderRadius = 60;
        btnDangNhap.BackColor = MyColor.MainColor;
        btnDangNhap.Dock = DockStyle.Top;
        btnDangNhap.Height = 60;

        var lblDangNhap = new Label();
        lblDangNhap.Anchor = AnchorStyles.None;
        lblDangNhap.Text = "Đăng nhập";
        lblDangNhap.Font = GetFont.GetMainFont(11, FontType.Black);
        lblDangNhap.ForeColor = MyColor.White;
        lblDangNhap.AutoSize = true;

        btnDangNhap.Controls.Add(lblDangNhap);
        btnDangNhap.Cursor = Cursors.Hand;


        return btnDangNhap;
    }

    private void SetAciton()
    {
        btnDangNhap.MouseClick += (sender, args) => onClickBtnDangNhap();
        btnDangNhap.MouseEnter += (sender, args) => { btnDangNhap.BackColor = MyColor.GrayHoverColor; };
        btnDangNhap.MouseLeave += (sender, args) => { btnDangNhap.BackColor = MyColor.MainColor; };

        foreach (Control c in btnDangNhap.Controls)
        {
            c.Cursor = Cursors.Hand;
            c.MouseClick += (sender, args) => onClickBtnDangNhap();
            c.MouseEnter += (sender, args) => { btnDangNhap.BackColor = MyColor.GrayHoverColor; };
            c.MouseLeave += (sender, args) => { btnDangNhap.BackColor = MyColor.MainColor; };
        }

        _usrnameTfl.GetTextField().KeyDown += (sender, args) => OnKeyDown(sender, args);
        _passTfl.GetPasswordField().KeyDown += (sender, args) => OnKeyDown(sender, args);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) onClickBtnDangNhap();
    }

    private void onClickBtnDangNhap()
    {
        var username = _usrnameTfl.GetTextTextField();
        var password = _passTfl.GetTextPasswordField();

        _taiKhoan = _taiKhoanController.GetTaiKhoanByUsrName(username);

        if (_taiKhoan == null)
        {
            Console.WriteLine("hi");
            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác", "Lỗi", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (!PasswordHasher.VerifyPassword(password, _taiKhoan.MatKhau))
        {
            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác", "Lỗi", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var nhomQuyen = _nhomQuyenController.GetById(_taiKhoan.MaNQ);

        Hide();

        Form home = new MyHome(nhomQuyen, _taiKhoan);
        home.FormClosed += (s, args) => Show();
        home.ShowDialog();
    }
}