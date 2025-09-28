using LiveChartsCore.SkiaSharpView.WinForms;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Chart;
using QuanLySinhVien.Views.Enums;
using WinFormsSample.Pies.Doughnut;


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

        MyChart sideChart = GetSideStatistical();
        mainLayout.Controls.Add(sideChart);
        mainLayout.SetRowSpan(sideChart, 2);

        TableLayoutPanel bottomBoxContainer = this.GetBottomContainer();
        mainLayout.Controls.Add(bottomBoxContainer);
        mainLayout.SetColumnSpan(bottomBoxContainer, 3);
        
        
        this.Controls.Add(mainLayout);
    }

    CommonUse.Chart.View GetChart()
    {
        CommonUse.Chart.View chart = new CommonUse.Chart.View();
        chart.Dock = DockStyle.Fill;
        return chart;
    }

    MyChart GetSideStatistical()
    {
        MyChart chart =  new MyChart();
        chart.Dock = DockStyle.Fill;
        chart.BorderStyle = BorderStyle.FixedSingle;
        return chart;
    }

    //chứa 2 box top5
    TableLayoutPanel GetBottomContainer()
    {
        TableLayoutPanel panel = new TableLayoutPanel();
        panel.ColumnCount = 2;
        panel.Dock = DockStyle.Fill;
        panel.AutoSize = true;
        
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        
        string[] listTen1 = new string[] { "Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin"};
        float[] listPhanTram1 = new[] { 100f, 100f, 100f, 100f, 100f };
        StatisticalTop5Box box1 = new StatisticalTop5Box("Top 5 ngành có tỉ lệ tốt nghiệp cao nhất", listTen1, listPhanTram1);
        
        string[] listTen2 = new string[] { "Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin","Công nghệ thông tin"};
        float[] listPhanTram2 = new[] { 100f, 100f, 100f, 100f, 100f };
        StatisticalTop5Box box2 = new StatisticalTop5Box("Top 5 ngành có tỉ lệ tốt nghiệp thấp nhất", listTen2, listPhanTram2);
        
        panel.Controls.Add(box1);
        panel.Controls.Add(box2);
        return panel;
    }
    
    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    
}