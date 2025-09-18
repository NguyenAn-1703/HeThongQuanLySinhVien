// using QuanLySinhVien.Views;

using System.Security.Policy;
using Svg;

namespace QuanLySinhVien.Views.Components.Home;

public class MyHome : Form
{
    public MyHome()
    {
        Init();
    }

    private void Init()
    {
        // Client
        Text = "Trang Chủ";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1600, 1000);

        // layout border
        var parLeft = new Panel
        {
            Dock = DockStyle.Left,
            BackColor = Color.White,
            Width = 400
        };

        var parRight = new Panel
        {
            Dock = DockStyle.Right,
            Width = 1200,
            BackColor = Color.White
        };

        // right panel
        var right = new Panel
        {
            Dock = DockStyle.Fill,
        };

        // left panel
        var left = new Panel
        {
            Dock = DockStyle.Fill,
            Width = 400,
        };
        
        // logo
        var logo = new Panel
        {
            Dock = DockStyle.Top,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Height = 200,
        };

        var logoSguPath = Path.Combine(Path.Combine(AppContext.BaseDirectory, "img" , "png" ,  "Logo ĐH Sài Gòn - SGU.png"));
        var iconLogo = Image.FromFile(logoSguPath);

        var logoPb = new PictureBox
        {
            Size = new Size(150, 150),
            Location = new Point(100 ,  0),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = iconLogo,
        };

        var logoText = new Label
        {
            Text = "Sài Gòn University",
            Font = new Font("JetBrains Mono", 17f, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1E40AF"),
            AutoSize = true,
            BackColor = Color.Transparent,
            Location = new Point(20, 150),
            BorderStyle = BorderStyle.None,
        };
        
        logo.Controls.Add(logoText);
        logo.Controls.Add(logoPb);
        // navList 
        var navList = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Height = 700,
            Width = 400,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            //AutoScroll = true,
            Padding = new Padding(30, 0 , 30 , 0),
            Margin = Padding.Empty
        };
        var labels = new[]
        {
            "Trang chủ", "Sinh viên", "Giảng viên", "Khoa", "Ngành", "Chương trình đào tạo", "Học phần", "Phòng học",
            "Tổ chức thi", "Nhập điểm", "Học phí", "Mở đăng ký học phần", "Quản lí tài khoản", "Phân quyền", "Thống kê"
        };
        var imgText = new[]
        {
            "trangchu" , "sinhvien" , "giangvien" , "khoa" , "nganh" , "chuongtrinhdaotao" , "hocphan" , "phonghoc",
            "tochucthi" , "nhapdiem" , "hocphi" , "modangkyhocphan" , "sinhvien" , "phanquyen" , "thongke"
        };
        for (int i = 0; i < labels.Length; i++)
        {
            var svgPath = Path.Combine(AppContext.BaseDirectory, "img", imgText[i] + ".svg");
            var icon = SvgDocument.Open(svgPath).Draw(20, 20);
            var btn = new Button
            {
                Text = labels[i], 
                AutoSize = false, 
                Height = 40, 
                Font = new Font("JetBrains Mono", 10f, FontStyle.Regular),
                Width = navList.ClientSize.Width - navList.Padding.Horizontal, 
                TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(12,0,0,0), 
                Margin = new Padding(0,5,0,0), 
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Image = icon;
            btn.ImageAlign = ContentAlignment.MiddleLeft;  
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            navList.Controls.Add(btn);
        }
        // logout
        var logout = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            // BackColor = Color.Red,
            Padding = new Padding(30, 0 , 30 , 0),
            Height = 100,
        };
        var pathLog = Path.Combine(AppContext.BaseDirectory, "img", "dangxuat.svg");
        var iconLog = SvgDocument.Open(pathLog).Draw(25, 25);
        var logoutButton = new Button
        {
            Text = "Đăng Xuất",
            AutoSize = true,
            Location = new Point(40 , 30),
            BackColor = Color.Red,
            Height = 40,
            Width = 300,
            Font =  new Font("JetBrains Mono", 12f, FontStyle.Regular),
            FlatStyle = FlatStyle.Flat,
            Image = iconLog,
            ImageAlign = ContentAlignment.MiddleLeft,
            TextImageRelation = TextImageRelation.ImageBeforeText,
            Padding = new  Padding(20,0,0,0),
        };
        logoutButton.FlatAppearance.BorderSize = 0;
        logout.Controls.Add(logoutButton);
        //taskbar
        var taskbar = new Panel
        {
            Dock = DockStyle.Top,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Height = 100,
        };
        // center
        var center = new Panel
        {
            Dock = DockStyle.Top,
            BackColor = Color.White,
            Height = 80,
        };
        
        //bottom
        var bottom = new Panel
        {
            Dock = DockStyle.Top,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Height = 820,
        };
        
        var functionTopRightLeft = new Panel
        {
            Dock = DockStyle.Left,
            Width = 800,
        };
        var searchWrap = new Panel {
            Size = new Size(480, 40),
            BackColor = Color.White,
            Padding = new Padding(40, 8, 12, 8) 
        };
        searchWrap.Location = new Point(30, 30);
        var iconSearchPath = Path.Combine(AppContext.BaseDirectory, "img", "search.svg");
        var iconSearch = new PictureBox {
            Image = SvgDocument.Open(iconSearchPath).Draw(30, 30),
            Size = new Size(30, 30),
            Location = new Point(8, 8),
            SizeMode = PictureBoxSizeMode.CenterImage,
            BackColor = Color.Transparent
        };
        var txtSearch = new TextBox {
            BorderStyle = BorderStyle.None,
            PlaceholderText = "Tìm kiếm...",
            Font = new Font("JetBrains Mono", 13f, FontStyle.Regular),
            Dock = DockStyle.Fill,
            BackColor = searchWrap.BackColor
        };
        searchWrap.Controls.Add(txtSearch);
        searchWrap.Controls.Add(iconSearch);
        functionTopRightLeft.Controls.Add(searchWrap);
        var comboList = new[] {"huhu" , "2" , "3" , "4"} ;
        var filter = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            Location = new Point(550, 30),
            Font = new Font("JetBrains Mono", 10f),
            Width = 200,
            DrawMode = DrawMode.OwnerDrawFixed,
            ItemHeight = 35,                
            DropDownHeight = 200,             
            IntegralHeight = false,
        };
        filter.DrawItem += (s, e) =>
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            var text = filter.Items[e.Index].ToString();
            TextRenderer.DrawText(
                e.Graphics,
                text,
                filter.Font,
                e.Bounds,
                SystemColors.ControlText,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left
            );
            e.DrawFocusRectangle();
        };
        filter.Items.AddRange(comboList);
        
        functionTopRightLeft.Controls.Add(filter);
        
        // user account
        var functionTopRightRight = new Panel
        {
            Dock = DockStyle.Right,
            BackColor = Color.Cyan,
            Width = 400,
        };
        
        // border
         parLeft.Padding = new Padding(10);
         parRight.Padding = new Padding(10);
        
        // add
        taskbar.Controls.Add(functionTopRightLeft);
        taskbar.Controls.Add(functionTopRightRight);
        
        right.Controls.Add(bottom);
        right.Controls.Add(center);
        right.Controls.Add(taskbar);
        
        left.Controls.Add(logout);
        left.Controls.Add(navList);
        left.Controls.Add(logo);
        
        parLeft.Controls.Add(left);
        parRight.Controls.Add(right);
        Controls.Add(parLeft);
        Controls.Add(parRight);
        
        ResumeLayout(performLayout: true);
    }
}