// using QuanLySinhVien.Views;

using System.Security.Policy;
using QuanLySinhVien.Models;
using Svg;

namespace QuanLySinhVien.Views.Components.Home;

public class MyHome : Form
{
    private int IndexButton = 0;
    private NavListController navListController = new NavListController();
    public MyHome()
    {
        Init();
    }

    private void Init()
    {
        // Client
        Text = "Hệ thống quản lí sinh viên";
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

        /* right panel => chia thành 2 thành phần top và bottom
         * mục đích xử lí navList khi chọn vào button (các phần còn lại để nguyên chỉ có thành phần bottom là bị thay đổi)
         * TODO: Sử dụng cái gì để xử lí? dùng Map để lưu các thành phần?
        */ 
        var right = new Panel
        {
            Dock = DockStyle.Fill,
        };
        var rightTop = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
        };
        /* TODO: rightBottomChange = new TrangChu() hay 1 thành phần nào đó được button click
         * vậy thì trong cái button phải lưu trử 1 cái gì đó để truy xuất đến được <Button text = Trang Chủ -> TrangChu>
         * 
         */
        var rightBottomChange = new TrangChu();
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
        Button[] buttonArray = new Button[labels.Length];
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
            buttonArray[i] = btn;
        }
        buttonArray[0].BackColor = MyColor.Gray;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            int pos = i;
            buttonArray[pos].Click += (s, e) =>
            {
                buttonArray[IndexButton].BackColor = Color.Transparent;
                IndexButton = pos;
                buttonArray[IndexButton].BackColor = MyColor.Gray;
            };
        }
        

        //navListController.getDataButton();
        
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
            Cursor = Cursors.Hand
        };
        logoutButton.Click += (s, e) =>
        {
            this.Dispose();
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
        var comboList = new[] {"1" , "2" , "3" , "4"} ;
        var filter = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            Location = new Point(550, 35),
            Font = new Font("JetBrains Mono", 10f),
            Size = new Size(200, 0),
            DrawMode = DrawMode.Normal,
            ItemHeight = 35,                
            //DropDownHeight = 200,             
            IntegralHeight = false,
            SelectedText = "Lọc",
        };
        filter.Items.AddRange(comboList);
        
        functionTopRightLeft.Controls.Add(filter);
        
        // user account
        var functionTopRightRight = new Panel
        {
            Dock = DockStyle.Right,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Width = 400,
        };
        
        var pathIconAccount = Path.Combine(AppContext.BaseDirectory, "img", "user.svg");
        var userIcon = new PictureBox
        {
            Image = SvgDocument.Open(pathIconAccount).Draw(50, 50),
            AutoSize = true,
            Dock = DockStyle.Left,
            Margin = new Padding(0),
            Padding = new Padding(0),
        };

        var textAccount = new TableLayoutPanel {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill,
            Padding = new Padding(0),
            Margin = new Padding(8, 0, 0, 0),
        };
        textAccount.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textAccount.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var userTextName = new Label
        {
            Text = "truy vấn sql",  
            Font = new Font("JetBrains Mono", 10f, FontStyle.Bold),
            AutoSize = true
        };

        var userTextGmail = new Label
        {   
            Text = "truyvấnsql@gmail.com",
            Font = new Font("JetBrains Mono",10f, FontStyle.Regular),
            AutoSize = true,
            ForeColor = Color.Gray,
        };
        
        textAccount.Controls.Add(userTextName, 0, 0);
        textAccount.Controls.Add(userTextGmail, 0, 1);
        
        var fullUserAccount = new Panel
        {
            Size = new Size(400, 50),
            Padding = new Padding(8, 4, 8, 4)
            //BackColor = Color.Red,
        };
        fullUserAccount.Controls.Add(textAccount);
        fullUserAccount.Controls.Add(userIcon);
        fullUserAccount.Location = new Point(30, 25);
        functionTopRightRight.Controls.Add(fullUserAccount);
        // border
         parLeft.Padding = new Padding(10);
         parRight.Padding = new Padding(10);
        
        // add
        taskbar.Controls.Add(functionTopRightLeft);
        taskbar.Controls.Add(functionTopRightRight);
        
        // right.Controls.Add(bottom);
        // right.Controls.Add(center);
        rightTop.Controls.Add(taskbar);
        right.Controls.Add(rightTop);
        right.Controls.Add(rightBottomChange);
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