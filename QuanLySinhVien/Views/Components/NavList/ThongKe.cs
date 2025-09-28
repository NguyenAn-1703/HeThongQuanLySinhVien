using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList;

public class ThongKe : NavBase
{
    private string[] _listSelectionForComboBox = new []{""};
    public ThongKe()
    {
        Init();
    }

    void Init()
    {
        this.Dock = DockStyle.Fill;
        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            RowCount = 2,
            ColumnCount = 4,
            Dock = DockStyle.Fill,
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        mainLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        
        mainLayout.Controls.Add(new StatisticalBox("Tổng số sinh viên", 123, StatisticalIndex.first));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số giảng viên", 123, StatisticalIndex.second));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số ngành", 123, StatisticalIndex.third));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số học phí đã thu", 123, StatisticalIndex.fourth));
        
        this.Controls.Add(mainLayout);
    }
    
    
    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
}