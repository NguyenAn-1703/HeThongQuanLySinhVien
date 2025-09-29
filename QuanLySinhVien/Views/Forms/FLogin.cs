using System.Drawing.Drawing2D;
using QuanLySinhVien.Views.Components;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.GetFont;
using QuanLySinhVien.Views.Components.Home;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Forms;

public class FLogin : Form
{
    private LabelTextField _usrnameTfl;
    private LabelTextField _passTfl;
    
    public FLogin()
    {
        Init();
    }

    void Init()
    {
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(1000, 600);
        this.FormBorderStyle = FormBorderStyle.None;
        
        this.BackColor = MyColor.White;
        
        TableLayoutPanel mainPanel = new TableLayoutPanel();
        mainPanel.BackColor = MyColor.White;
        mainPanel.ColumnCount = 2;
        mainPanel.Dock = DockStyle.Fill;
        mainPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        
        mainPanel.Controls.Add(GetImg());
        mainPanel.Controls.Add(GetControlPanel());
        this.Controls.Add(mainPanel);

    }
    
    //img
    public PictureBox GetImg()
    {
        PictureBox pic = new PictureBox
        {
            Image = GetPng.GetImage("img/png/Truong.png"),
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.StretchImage,
            Margin = new Padding(0),
            BorderStyle = BorderStyle.None,
        };
        return pic;
    }

    TableLayoutPanel GetBtnThoat()
    {
        TableLayoutPanel Button = new TableLayoutPanel
        {
            AutoSize = true,
        };
        PictureBox pb = new PictureBox
        {
            Image = GetSvgBitmap.GetBitmap("exitbutton.svg"),
            Size = new Size(30, 30),
            SizeMode = PictureBoxSizeMode.Zoom,
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

    TableLayoutPanel GetControlPanel()
    {
        TableLayoutPanel controlPanel = new TableLayoutPanel
        {
            RowCount = 3
        };
        controlPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        controlPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        controlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        controlPanel.Dock =  DockStyle.Fill;
        // controlPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        
        //Tiêu đề
        Label title = new Label();
        title.Text = "Hệ thống quản lý sinh viên SGU";
        title.Font = GetFont.GetMainFont(12, FontType.Black);
        title.AutoSize = true;
        title.Anchor = AnchorStyles.None;
        
        /////////////////////form điền
        TableLayoutPanel loginForm = new TableLayoutPanel
        {
            RowCount = 6,
        };
        loginForm.AutoSize = true;
        loginForm.Anchor = AnchorStyles.None;
        loginForm.Dock = DockStyle.Fill;
        loginForm.Padding = new Padding(120, 50, 120, 50);
        // loginForm.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        // loginForm.BackColor = MyColor.GrayHoverColor;
        
        Label titleWb = new Label();
        titleWb.Anchor = AnchorStyles.None;
        titleWb.AutoSize = true;
        titleWb.Text = "Welcome Back";
        titleWb.Font = GetFont.GetMainFont(20, FontType.Black);
        titleWb.ForeColor = MyColor.MainColor;
        titleWb.Margin = new Padding(3, 3, 3, 10);
        
        Label title2 = new Label();
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
        
        Label lblQmk = new Label();
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

    RoundTLP GetButtonDangNhap()
    {
        RoundTLP btnDangNhap = new RoundTLP();
        btnDangNhap.BorderRadius = 60;
        btnDangNhap.BackColor = MyColor.MainColor;
        btnDangNhap.Dock = DockStyle.Top;
        btnDangNhap.Height = 60;
        
        Label lblDangNhap = new Label();
        lblDangNhap.Anchor = AnchorStyles.None;
        lblDangNhap.Text = "Đăng nhập";
        lblDangNhap.Font = GetFont.GetMainFont(11, FontType.Black);
        lblDangNhap.ForeColor = MyColor.White;
        lblDangNhap.AutoSize = true;
        
        btnDangNhap.Controls.Add(lblDangNhap);
        
        btnDangNhap.MouseClick += (sender, args) => onClickBtnDangNhap(); 
        btnDangNhap.MouseEnter += (sender, args) => { btnDangNhap.BackColor = MyColor.GrayHoverColor; };
        btnDangNhap.MouseLeave +=  (sender, args) => { btnDangNhap.BackColor = MyColor.MainColor; };

        foreach (Control c in btnDangNhap.Controls)
        {
            c.MouseClick += (sender, args) => onClickBtnDangNhap();
            c.MouseEnter += (sender, args) => { btnDangNhap.BackColor = MyColor.GrayHoverColor; };
            c.MouseLeave +=  (sender, args) => { btnDangNhap.BackColor = MyColor.MainColor; };
        }
        
        return btnDangNhap;
    }

    void onClickBtnDangNhap()
    {
        this.Hide();
        Form home = new MyHome();
        home.FormClosed += (s, args) => this.Show();
        home.ShowDialog();
    }
}