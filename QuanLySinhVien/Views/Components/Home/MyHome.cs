// using QuanLySinhVien.Views;

using System.Drawing.Text;
using QuanLySinhVien.Controllers;
using Svg;
using QuanLySinhVien.Views.Components.GetFont;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using System.Windows.Forms;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using Timer = System.Windows.Forms.Timer;

namespace QuanLySinhVien.Views.Components.Home;

public class MyHome : Form
{
    private int IndexButton = 0;
    private string ButtonClickNavList = "";
    private NavListController navListController = new NavListController();
    //Trang bắt đầu
    NavBase rightBottomChange = new TrangChu();
    private Panel rightBottomHost;
    private NavBar navBar;
    //Cho phan thu gon navbar
    private Boolean _isToggle = false;
    private Label logoText;
    private PictureBox logoPb;
    private ToggleButton toggleButton;
    private TableLayoutPanel navListContainer;
    private Panel parLeft;
    private TableLayoutPanel left;
    private Panel logout;
    private LogoutButton logoutButton;
    private SearchBar _searchBar;
    
    public MyHome()
    {
        Init();
    }

    private void Init()
    {
        #region mainLayout
        // Chia layout co dãn theo kích thước window
        // Bố cục 2 phần trái, phải, kích thước theo component bên trong
        #endregion
        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        
        // Client
        Text = "Hệ thống quản lí sinh viên";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1600, 1000);

        // layout border
        parLeft = new Panel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.GrayBackGround
        };
        
        // left panel
        left = new TableLayoutPanel()
        {
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
        var logo = new TableLayoutPanel()
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
        
        navBar = new NavBar();
        
        navListContainer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Padding = new Padding(0, 15, 0, 0)
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
        
        //taskbar
        // var taskbar = new Panel
        // {
        //     Dock = DockStyle.Top,
        //     BackColor = ColorTranslator.FromHtml("#E5E7EB"),
        //     Height = 100,
        // };
        
        
        //Panel Phải

        
        // var txtSearch = new TextBox {
        //     BorderStyle = BorderStyle.None,
        //     PlaceholderText = "Tìm kiếm...",
        //     Font = new Font("JetBrains Mono", 13f, FontStyle.Regular),
        //     Dock = DockStyle.Fill,
        //     BackColor = searchWrap.BackColor
        // };
        // searchWrap.Controls.Add(txtSearch);
        // searchWrap.Controls.Add(iconSearch);
        // functionTopRightLeft.Controls.Add(searchWrap);
        
        // var comboList = new[] {"1" , "2" , "3" , "4"} ;
        // var filter = new ComboBox
        // {
        //     DropDownStyle = ComboBoxStyle.DropDown,
        //     AutoCompleteMode = AutoCompleteMode.SuggestAppend,
        //     Location = new Point(550, 35),
        //     Font = new Font("JetBrains Mono", 10f),
        //     Size = new Size(200, 0),
        //     DrawMode = DrawMode.Normal,
        //     ItemHeight = 35,                
        //     //DropDownHeight = 200,             
        //     IntegralHeight = false,
        //     SelectedText = "Lọc",
        // };
        // filter.Items.AddRange(comboList);
        
        var parRight = new TableLayoutPanel     //////////////////////ddang lamf
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            RowCount = 2,
        };
        
        parRight.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        
        parRight.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        parRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        /* right panel => chia thành 2 thành phần top và bottom
         * mục đích xử lí navList khi chọn vào button (các phần còn lại để nguyên chỉ có thành phần bottom là bị thay đổi)
         * TODO: Sử dụng cái gì để xử lí? dùng Map để lưu các thành phần?
         */ 
        var rightTop = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = MyColor.GrayBackGround,
            ColumnCount = 2
        };

        rightTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        rightTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        
        _searchBar = new SearchBar();
        TableLayoutPanel accountInfo = getAcountInfo();
            

        // border
         parLeft.Padding = new Padding(5);
         parRight.Padding = new Padding(10);

        //add
         rightBottomHost = new Panel
         {
             Dock = DockStyle.Fill,
         };
         
         rightBottomChange.Dock = DockStyle.Fill;
         rightBottomHost.Controls.Add(rightBottomChange);
         
         rightTop.Controls.Add(_searchBar);
         rightTop.Controls.Add(accountInfo);
         
         rightTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
         
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
        
         ResumeLayout(performLayout: true);
        
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
        rightBottomHost.SuspendLayout();
        rightBottomHost.Controls.Clear();
        
        String change = navListController.getDataButton(function);
        Console.WriteLine(change);
        rightBottomChange = navListController.update(change);
        Console.WriteLine(rightBottomChange);
        
        rightBottomChange.Dock = DockStyle.Fill;
        rightBottomHost.Controls.Add(rightBottomChange);
        rightBottomHost.ResumeLayout(true);
        rightBottomHost.Invalidate();
        rightBottomHost.Refresh();
    }

    void UpdateSearchCombobox(string function)
    {
        String change = navListController.getDataButton(function);
        rightBottomChange = navListController.update(change);
        
        List<string> t = rightBottomChange.getComboboxList();
        _searchBar.UpdateListCombobox(t);

    }

    public void LogOut()
    {
        this.Dispose();
    }

    TableLayoutPanel getAcountInfo()
    {
        TableLayoutPanel AccountInfo = new TableLayoutPanel
        {
            ColumnCount = 2,
        };
        AccountInfo.Anchor = AnchorStyles.None;
        AccountInfo.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        AccountInfo.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        AccountInfo.AutoSize = true;
        
        AccountInfo.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;        
        
        
        var userIcon = new PictureBox
        {
            Image = GetSvgBitmap.GetBitmap("user.svg"),
            Margin = new Padding(0),
            Size = new Size(45, 45),
            SizeMode = PictureBoxSizeMode.Zoom,
        };
        
        var textAccount = new TableLayoutPanel {
            RowCount = 2,
            Margin = new Padding(8, 0, 0, 0),
            AutoSize = true
        };
        
        textAccount.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textAccount.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        var userTextName = new Label
        {
            Text = "truy vấn sql",  
            Font = GetFont.GetFont.GetMainFont(12, FontType.Regular),
            AutoSize = true
        };
        
        var userTextGmail = new Label
        {   
            Text = "truyvấnsql@gmail.com",
            Font = GetFont.GetFont.GetMainFont(12, FontType.Regular),
            AutoSize = true,
            ForeColor = Color.Gray,
        };
        
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
        navBar.SuspendLayout();
        foreach(NavItem item in navBar.ButtonArray)
        {
            item.Controls[2].Visible = false;
            item.Dock = DockStyle.None;
        }

        logoPb.Size = new Size(40, 40);
        logoText.Visible = false;

        toggleButton.Location = new Point(logoPb.Location.X + 20, 40);
        toggleButton.ChangeImg("toggle2.svg");
        
        logoutButton.Controls[1].Visible = false;
        
        navBar.ResumeLayout();
        navBar.Refresh();
    }
    
    public void UnToggleNavbar()
    {
        navBar.SuspendLayout();
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
        
        navBar.ResumeLayout();
        navBar.Refresh();
        
    }

}