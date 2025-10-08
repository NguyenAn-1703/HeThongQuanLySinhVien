using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components;

public class QuanLiTaiKhoan : NavBase
{
    private string _title = "Tài Khoản";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private TaiKhoanController _taiKhoanController;
    string[] _headerArray = new string[] { "Mã tài khoản", "Mã nhóm quyền", "Tên đăng nhập" };
    List<string> _headerList;

    private TitleButton _insertButton;
    
    List<TaiKhoanDto> _rawData;
    List<object> _displayData;

    private TaiKhoanSearch _taiKhoanSearch;
    private TaiKhoanDialog _taiKhoanDialog;
        
    public QuanLiTaiKhoan()
    {
        _rawData = new  List<TaiKhoanDto>();
        _displayData = new List<object>();
        _taiKhoanController = TaiKhoanController.getInstance();
        _taiKhoanDialog = new TaiKhoanDialog(DialogType.Them, "Thêm tài khoản");
        Init();
    }
        
    private void Init()
    {
        Dock = DockStyle.Fill;
        
        TableLayoutPanel mainLayout = new  TableLayoutPanel
        {
            RowCount = 2,
            Dock = DockStyle.Fill,
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        mainLayout.Controls.Add(Top());
        mainLayout.Controls.Add(Bottom());

        Controls.Add(mainLayout);
    }

    private Panel Top()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            Padding = new Padding(10),
            ColumnCount = 2,
            BackColor = MyColor.GrayBackGround
        };
        
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        
        panel.Controls.Add(getTitle());
        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12,  FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        panel.Controls.Add(_insertButton);
        
        return panel;
    }

    private Panel Bottom()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
        };

        SetCombobox();
        
        SetDataTable();

        SetSearch();

        SetAction();
        
        panel.Controls.Add(_table);
        
        return panel;
    }
    //////////////////////////////SETTOP///////////////////////////////
    Label getTitle()
    {
        Label titlePnl = new Label
        {
            Text = _title,
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true,
        };
        return titlePnl;
    }


    
    
    //////////////////////////////SETTOP///////////////////////////////
    
    
    
/// ///////////////////////////SETBOTTOM////////////////////////////////////
    void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }

    //hàm gọi 1 lần duy nhất khi khởi tạo
    void SetDataTable()
    {
        _rawData = _taiKhoanController.GetAll();
        SetDisplayData();
        string[] columnNames = new[] { "MaTK", "MaNQ", "TenDangNhap" };
        List<string> columnNamesList = columnNames.ToList();
        
        _table = new CustomTable(_headerList, columnNamesList, _displayData, true, true, true);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new {x.MaTK, x.MaNQ, x.TenDangNhap});
    }

    void SetSearch()
    {
        _taiKhoanSearch = new TaiKhoanSearch(_rawData);
    }

    void SetAction()
    {
        _taiKhoanSearch.FinishSearch += dtos =>
        {
            this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new { x.MaTK, x.MaNQ, x.TenDangNhap });
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
    }

    void Insert()
    {
        this._taiKhoanDialog.ShowDialog();
    }

    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._taiKhoanSearch.Search(txtSearch, filter);
            
        // this._table.Search(txtSearch, filter);
        
    } 
    /// ///////////////////////////SETBOTTOM////////////////////////////////////

}