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
            RowCount = 3,
            ColumnCount = 4,
            Dock = DockStyle.Fill,
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        mainLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        
        mainLayout.Controls.Add(new StatisticalBox("Tổng số sinh viên", 123, StatisticalIndex.first));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số giảng viên", 123, StatisticalIndex.second));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số ngành", 123, StatisticalIndex.third));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số học phí đã thu", 123, StatisticalIndex.fourth));

        CommonUse.Chart.View chart = GetChart();
        mainLayout.Controls.Add(chart);
        mainLayout.SetColumnSpan(chart, 3);
        
        mainLayout.Controls.Add(GetSideStatistical());

        
        string[] listTen = new string[] { "Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin"};
        float[] listPhanTram = new[] { 100f, 100f, 100f, 100f, 100f };
        StatisticalTop5Box box1 = new StatisticalTop5Box("Top 5 ngành có tỉ lệ tốt nghiệp cao nhất", listTen, listPhanTram);
        
        mainLayout.Controls.Add(box1);
        mainLayout.SetColumnSpan(box1, 3);
        
        
        this.Controls.Add(mainLayout);
    }

    CommonUse.Chart.View GetChart()
    {
        CommonUse.Chart.View chart = new CommonUse.Chart.View();
        chart.Dock = DockStyle.Fill;
        return chart;
    }

    TableLayoutPanel GetSideStatistical()
    {
        TableLayoutPanel panel = new TableLayoutPanel();
        panel.BackColor = MyColor.Red;

        return panel;
    }
    
    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    
}