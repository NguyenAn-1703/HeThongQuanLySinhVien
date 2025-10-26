// using QuanLySinhVien.Views;

using System.Drawing.Text;
using QuanLySinhVien.Controllers;
using Svg;
using QuanLySinhVien.Views.Components.GetFont;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using System.Windows.Forms;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using Timer = System.Windows.Forms.Timer;

namespace QuanLySinhVien.Views.Components.Home;

public class MyHome : Form
{
    private int IndexButton = 0;
    private string ButtonClickNavList = "";
    private NavListController navListController;
    //Trang bắt đầu
    NavBase rightBottomChange;
    private Panel rightBottomHost;
    private NavBar navBar;
    //Cho phan thu gon navbar
    private Boolean _isToggle = false;
    private Label logoText;
    private PictureBox logoPb;
    private ToggleButton toggleButton;
    private MyTLP navListContainer;
    private Panel parLeft;
    private MyTLP parRight;
    private MyTLP left;
    private Panel logout;
    private LogoutButton logoutButton;
    private SearchBar _searchBar;
    private MyTLP rightTop;
    private MyTLP mainLayout;
    //panel trống cho chức năng không cần đến rightTop
    private MyTLP _emptyForUnTopBar;
    
    private SinhVienController _sinhVienController;
    private GiangVienController _giangVienController;

    private NhomQuyenDto _quyen;
    private TaiKhoanDto _taiKhoan;

    // private NhomQuyenDto _quyen;
    
    public MyHome(NhomQuyenDto nhomQuyen, TaiKhoanDto taiKhoan)
    {
        _quyen =  nhomQuyen;
        navBar = new NavBar(_quyen);
        rightBottomChange = new TrangChu(_quyen, taiKhoan);
        navListController = new NavListController(_quyen, taiKhoan);
        _sinhVienController = SinhVienController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
            
        _taiKhoan = taiKhoan;
        Init();
    }

    private void Init()
    {

        mainLayout = new MyTLP
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        mainLayout.SuspendLayout();

        
        // Client
        Text = "Hệ thống quản lí sinh viên";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1600, 950);

        // layout border
        parLeft = new Panel
        {
            Margin = new Padding(0),
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.GrayBackGround
        };
        
        // left panel
        left = new MyTLP()
        {
            Margin = new Padding(0),
            Dock = DockStyle.Fill,
            AutoSize = true,
            RowCount = 3
        };
        // left.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)),
        
        //hàng 1, 3 auto, hàng 2 chiếm tâst cả
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        // logo
        var logo = new MyTLP()
        {
            Dock = DockStyle.Top,
            BackColor = MyColor.GrayBackGround,
            RowCount = 2,
            AutoSize = true,
        };
        
        logo.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        logo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        logo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var logoSguPath = Path.Combine(Path.Combine(AppContext.BaseDirectory, "img" , "png" ,  "Logo ĐH Sài Gòn - SGU.png"));
        var iconLogo = Image.FromFile(logoSguPath);

        logoPb = new PictureBox
        {
            Size = new Size(70, 70),
            SizeMode = PictureBoxSizeMode.Zoom,
            Anchor = AnchorStyles.None,
            Image = iconLogo,
        };

        logoText = new Label
        {
            Text = "Sài Gòn University",
            Font = GetFont.GetFont.GetMainFont(13, FontType.Black),
            ForeColor = ColorTranslator.FromHtml("#07689F"),
            BackColor = Color.Transparent,
            BorderStyle = BorderStyle.None,
            AutoSize = true,
            Anchor = AnchorStyles.None
        };
        
        logo.Controls.Add(logoPb);
        logo.Controls.Add(logoText);
        
        
        navListContainer = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Padding = new Padding(0, 15, 0, 0),
        };

        
        
        //Nút thu gọn
        toggleButton = new ToggleButton();
        toggleButton.Location = new Point(logoPb.Location.X + 130, 40);
        toggleButton.OnClick += UpdateToggleNavbar;
        
        /* TODO: rightBottomChange = new TrangChu() hay 1 thành phần nào đó được button click
         * vậy thì trong cái button phải lưu trử 1 cái gì đó để truy xuất đến được <Button text = Trang Chủ -> TrangChu>
         *
         */
        
        //set Panel bên trái
        navBar.OnSelect1Item += this.OnChangeItemNavBar;
        navListContainer.Controls.Add(navBar);
        
        logoutButton = new LogoutButton();
        logoutButton.OnClick += LogOut;
        
        // panel phải
        
        parRight = new MyTLP   
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            RowCount = 2,
        };
        
        
        parRight.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        parRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        rightTop = new MyTLP
        {
            Dock = DockStyle.Fill,
            BackColor = MyColor.GrayBackGround,
            Margin = new Padding(10),
            ColumnCount = 2,
        };


        rightTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        rightTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        
        _searchBar = new SearchBar();
        MyTLP accountInfo = getAcountInfo();
            

        // border
         parLeft.Padding = new Padding(5);
         
         
        //add
         rightBottomHost = new Panel
         {
             Dock = DockStyle.Fill,
         };

         if (rightBottomChange is TrangChu)
         {
             _emptyForUnTopBar = new MyTLP { AutoSize = true };
             _emptyForUnTopBar.Margin = new Padding(0);
             _emptyForUnTopBar.BackColor = MyColor.GrayBackGround;
             rightTop.Visible = false;
             parRight.Controls.Add(_emptyForUnTopBar);
             rightBottomHost.Padding = new Padding(0);
         }
         
         rightBottomChange.Dock = DockStyle.Fill;
         rightBottomHost.Margin = new Padding(0);
         parRight.Margin = new Padding(0);
         
         rightBottomHost.Controls.Add(rightBottomChange);
         
         rightTop.Controls.Add(_searchBar);
         rightTop.Controls.Add(accountInfo);
         
         parRight.Controls.Add(rightTop);
         parRight.Controls.Add(rightBottomHost);
        
         Controls.Add(toggleButton);
         
         left.Controls.Add(logo);
         left.Controls.Add(navListContainer);
         left.Controls.Add(logoutButton); 
        
         parLeft.Controls.Add(left);
         
         mainLayout.Controls.Add(parLeft);
         mainLayout.Controls.Add(parRight);
         Controls.Add(mainLayout);
        
         mainLayout.ResumeLayout(true);

         this._searchBar.KeyDown += (txtSearch, selectedItem) =>
         {
             this.rightBottomChange.onSearch(txtSearch, selectedItem);
         };
    }
    
    //update khi 1 item khác được chọn
    public void OnChangeItemNavBar(string function)
    {
        ChangePanel(function);
        UpdateSearchCombobox(function);
    }
    
    //Đổi rightbottom sang bảng chức năng khác
    void ChangePanel(string function)
    {
        mainLayout.SuspendLayout();
        
        rightBottomHost.SuspendLayout();
        rightBottomChange.SuspendLayout();
        
        rightBottomHost.Controls.Clear();
        String change = navListController.getDataButton(function);
        Console.WriteLine(change);
        rightBottomChange = navListController.update(change);
        
        rightBottomChange.Dock = DockStyle.Fill;
        rightBottomHost.Controls.Add(rightBottomChange);


        // if (rightBottomChange is DangKyHocPhan)
        // {
        //     _searchBar.Visible = false;
        // }
        // else
        // {
        //     _searchBar.Visible = true;
        // }
        

        if (rightBottomChange is TrangChu || rightBottomChange is DangKyHocPhan)
        {
            rightTop.Visible = false;
            _emptyForUnTopBar.Visible = true;
            this.rightBottomHost.Padding = new Padding(0);
        }
        else
        {
            rightTop.Visible = true;
            _emptyForUnTopBar.Visible = false;
            this.rightBottomHost.Padding = new Padding(10);
        }
        
        parRight.ResumeLayout();
        rightBottomChange.ResumeLayout();
        rightBottomHost.ResumeLayout();
        mainLayout.ResumeLayout(true);
    }

    void UpdateSearchCombobox(string function)
    {
        List<string> t = rightBottomChange.getComboboxList();
        _searchBar.UpdateListCombobox(t);

    }

    public void LogOut()
    {
        this.Dispose();
    }

    private MyTLP AccountInfo;
    MyTLP getAcountInfo()
    {
        AccountInfo = new MyTLP
        {
            ColumnCount = 2,
            Margin = new Padding(3, 3 ,40 ,3),
        };
        AccountInfo.Anchor = AnchorStyles.Right;
        AccountInfo.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        AccountInfo.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        AccountInfo.AutoSize = true;
        
        // AccountInfo.CellBorderStyle = MyTLPCellBorderStyle.Single;        
        var userIcon = new PictureBox
        {
            Image = GetSvgBitmap.GetBitmap("user.svg"),
            Margin = new Padding(0),
            Size = new Size(45, 45),
            SizeMode = PictureBoxSizeMode.Zoom,
        };
        
        var textAccount = new MyTLP {
            RowCount = 2,
            Margin = new Padding(8, 0, 0, 0),
            AutoSize = true
        };
        
        textAccount.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textAccount.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        string gmail;
        if (_taiKhoan.Type.Equals("Quản trị viên"))
        {
            gmail = _giangVienController.GetByMaTK(_taiKhoan.MaTK).Email;
        }
        else
        {
            gmail = _sinhVienController.GetByMaTK(_taiKhoan.MaTK).Email;
        }
        
        var userTextName = new Label
        {
            Text = _taiKhoan.TenDangNhap,  
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            AutoSize = true
        };
        
        var userTextGmail = new Label
        {   
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            AutoSize = true,
            ForeColor = Color.Gray,
        };
        userTextGmail.Text = gmail;
        
        
        textAccount.Controls.Add(userTextName);
        textAccount.Controls.Add(userTextGmail);
        
        
        AccountInfo.Controls.Add(userIcon);
        AccountInfo.Controls.Add(textAccount);
        
        return AccountInfo;
    }
    
    
    public void UpdateToggleNavbar()
    {
        if (_isToggle)
            UnToggleNavbar();
        else
            ToggleNavbar();
        _isToggle = !_isToggle;
    }
    
    public void ToggleNavbar()
    {
        SuspendForToggle();
        foreach(NavItem item in navBar.ButtonArray)
        {
            item.Controls[2].Visible = false;
            item.Dock = DockStyle.None;
        }

        logoPb.Size = new Size(40, 40);
        logoText.Visible = false;

        toggleButton.Location = new Point(logoPb.Location.X - 50, 40);
        toggleButton.ChangeImg("toggle2.svg");
        
        logoutButton.Controls[1].Visible = false;
        
        
        navBar.UpdateSize();
        ResumeForToggle();

    }
    
    public void UnToggleNavbar()
    {
        SuspendForToggle();
        foreach(NavItem item in navBar.ButtonArray)
        {
            item.Controls[2].Visible = true;
            item.Dock = DockStyle.Fill;
        }
        
        logoPb.Size = new Size(70, 70);
        logoText.Visible = true;
        
        toggleButton.Location = new Point(logoPb.Location.X + 130, 40);
        toggleButton.ChangeImg("toggle.svg");
        
        logoutButton.Controls[1].Visible = true;
        
        
        navBar.UpdateSize();
        ResumeForToggle();

    }

    void SuspendForToggle()
    {
        foreach(NavItem item in navBar.ButtonArray)
        {
            item.Controls[2].SuspendLayout();
        }

        
        mainLayout.SuspendLayout();
    }

    void ResumeForToggle()
    {
        foreach(NavItem item in navBar.ButtonArray)
        {
            item.Controls[2].ResumeLayout();
        }

        mainLayout.ResumeLayout();
    }


    // void SuspendLayoutStart()
    // {
    //     this.SuspendLayout();
    //     this.
    // }
}