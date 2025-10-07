using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class QuanLiTaiKhoan : NavBase
{
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private TaiKhoanController _taiKhoanController;
    string[] _headerArray = new string[] { "Mã tài khoản", "Mã nhóm quyền", "Tên đăng nhập" };
    List<string> _headerList;
    
    List<TaiKhoanDto> _rawData;
    List<object> _displayData;

    private TaiKhoanSearch _taiKhoanSearch;
        
    public QuanLiTaiKhoan()
    {
        _rawData = new  List<TaiKhoanDto>();
        _displayData = new List<object>();
        _taiKhoanController = TaiKhoanController.getInstance();
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
        Panel panel = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = Color.Red,
            Height = 90,
        };
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

}