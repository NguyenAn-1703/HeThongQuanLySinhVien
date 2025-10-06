using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class QuanLiTaiKhoan : NavBase
{
    private string[] _listSelectionForComboBox;
    private CustomTable _table;
    public QuanLiTaiKhoan()
    {
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

        string[] headerArray = new string[] { "Mã tài khoản", "Tên tài khoản", "Người dùng" };
        List<string> headerList = ConvertArray_ListString.ConvertArrayToListString(headerArray);
        _listSelectionForComboBox = headerList.ToArray();

        
        object[] people = new[]
        {
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            new { MaTK = "1", TenTK = "An", NgDung = "Nguyen An" },
            
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
            new { MaTK = "2", TenTK = "An", NgDung = "Nguyen Nguyen" },
        };

        string[] columnNames = new[] { "MaTK", "TenTK", "NgDung" };
        List<string> columnNamesList = columnNames.ToList();
        
        List<object> listData = people.ToList();
        
        _table = new CustomTable(headerList, columnNamesList, listData, true, true, true);
        panel.Controls.Add(_table);
        
        return panel;
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._table.Search(txtSearch, filter);
    }

}