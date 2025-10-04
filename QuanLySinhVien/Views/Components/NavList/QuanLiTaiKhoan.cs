using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class QuanLiTaiKhoan : NavBase
{
    private string[] _listSelectionForComboBox = new []{"Mã tài khoản", "Tên tài khoản"};
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

        object[,] datas =
        {
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            {"1","an","Nguyen An"},
            
            
        };
        List<List<object>> listData = ConvertArray_ListString.ConvertArrayToListObject(datas);
        
        CustomTable table = new CustomTable(headerList, listData, true);
        panel.Controls.Add(table);
        
        return panel;
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
}