using System.Drawing.Drawing2D;
using QuanLySinhVien.Views.Components;
using QuanLySinhVien.Views.Components.Home;
using Svg;

namespace QuanLySinhVien.Views.Forms;

public class FLogin : Form
{
    public FLogin()
    {
        Form Screen = new Form();
        Screen = this;
        BackColor = MyColor.SkyBlue;
        Text = "Đăng nhập";
        Size = new Size(1440, 1024);
        StartPosition = FormStartPosition.CenterScreen;
        this.MaximizeBox = false;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.WindowState = FormWindowState.Normal;

        
        Label lbHeading = new Label()
        {
            Text = "Hệ thống quản lý sinh viên",
            Font = new Font("Arial", 32, FontStyle.Regular),
            AutoSize = false,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.BottomCenter,
            Height = 100
        };

        Label lbLogin = new Label()
        {
            Text = "Đăng nhập",
            Font = new Font("Arial", 16, FontStyle.Regular),
            AutoSize = false,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.BottomCenter,
            Height = 150
        };
        
        
        TextBox txtBoxUserName = new TextBox()
        {
            BorderStyle = BorderStyle.None,
            Font = new Font("Arial", 12),
            Text = "Tên đăng nhập...",
            ForeColor = Color.Gray,
            Dock = DockStyle.Fill,
            BackColor = MyColor.Gray,
            SelectionStart = 0,
            
        };
        txtBoxUserName.Enter  += (sender, e) =>
        {
            if (txtBoxUserName.Text == "Tên đăng nhập...")
            {
                txtBoxUserName.Text = "";
                txtBoxUserName.ForeColor = MyColor.Black;
            }
        };
        txtBoxUserName.Leave += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(txtBoxUserName.Text))
            {
                txtBoxUserName.Text = "Tên đăng nhập...";
                txtBoxUserName.ForeColor = Color.Gray;
            }
        };
        
        TextBox txtBoxPassWord = new TextBox()
        {
            BorderStyle = BorderStyle.None,
            Font = new Font("Arial", 12),
            Text = "Mật khẩu...",
            ForeColor = Color.Gray,
            Dock = DockStyle.Fill,
            BackColor = MyColor.Gray,
            SelectionStart = 0,
        };
        txtBoxPassWord.Enter  += (sender, e) =>
        {
            if (txtBoxPassWord.Text == "Mật khẩu...")
            {
                txtBoxPassWord.Text = "";
                txtBoxPassWord.ForeColor = MyColor.Black;
            }
        };
        txtBoxPassWord.Leave += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(txtBoxPassWord.Text))
            {
                txtBoxPassWord.Text = "Mật khẩu...";
                txtBoxPassWord.ForeColor = Color.Gray;
            }
        };

        Image iconsUserName = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "img", "taikhoan.svg")).Draw(20, 20);
        PictureBox picUserName = new PictureBox()
        {
            Image = iconsUserName,
            SizeMode = PictureBoxSizeMode.CenterImage,
            Size = new Size(iconsUserName.Width, iconsUserName.Height),
            BackColor = Color.Transparent,
            Top = (60 - iconsUserName.Width)/2 - 2,
            Left = (50 - iconsUserName.Height)/2 + 2,
        };
        Panel panelUserName = new Panel()
        {
            
            Size = new Size(700, 60),
            BackColor = MyColor.Gray,
            Padding = new Padding(50,  15, 20, 0),
            Left = (1200 - 700) / 2,
            Top = lbLogin.Bottom + 200,
            Controls = { picUserName, txtBoxUserName }
        };
        
        
        Image iconsPassWord = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "img", "phanquyen.svg")).Draw(20, 20);
        PictureBox picPassWord = new PictureBox()
        {
            Image = iconsPassWord,
            SizeMode = PictureBoxSizeMode.CenterImage,
            Size = new Size(iconsPassWord.Width, iconsPassWord.Height),
            BackColor = Color.Transparent,
            Top = (60 - iconsPassWord.Width)/2 - 2,
            Left = (50 - iconsPassWord.Height)/2 + 2,
        };
        Panel panelPassWord = new Panel()
        {
            Size = new Size(700, 60),
            BackColor = MyColor.Gray,
            Padding = new Padding(50, 15, 20, 0),
            Left = (1200 - 700) / 2,
            Top = panelUserName.Bottom + 50,
            Controls = { picPassWord, txtBoxPassWord }
        };
        
        
        Button btnLogin = new Button()
        {
            FlatStyle = FlatStyle.Flat,
            TabStop = false,
            Text = "Đăng nhập",
            Font = new Font("Arial", 10),
            Size = new  Size(700, 60),
            BackColor = MyColor.Orange,
            Left = panelPassWord.Left,
            Top = panelPassWord.Bottom + 50,
            Cursor = Cursors.Hand,
        };
        btnLogin.FlatAppearance.BorderSize = 0;
        
        Panel pnCenter = new Panel()
        {
            Size = new Size(1200, 800),
            BackColor = MyColor.White,
            Left = (Screen.Width - 1200)/2,
            Top = (Screen.Height - 800)/2,
            Controls = { btnLogin, panelPassWord, panelUserName, lbLogin, lbHeading }
        };
        
        Controls.Add(pnCenter);


        btnLogin.Click += (sender, e) =>
        {
            Screen.Hide();
            Form home = new MyHome();
            home.FormClosed += (s, args) => Screen.Show();
            home.ShowDialog();
        };
        btnLogin.Select();
    }
}