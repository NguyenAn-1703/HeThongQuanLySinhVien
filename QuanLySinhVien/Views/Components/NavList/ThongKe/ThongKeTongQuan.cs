using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Chart;
using LiveChartsCore.SkiaSharpView.WinForms;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Chart;
// using QuanLySinhVien.Views.Components.CommonUse.Chart;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList;

public class ThongKeTongQuan : TableLayoutPanel

{
    SinhVienDAO sinhVienDao = new SinhVienDAO();
    GiangVienDao giangVienDao = new GiangVienDao();
    NganhDao nganhDao = NganhDao.GetInstance();

    public ThongKeTongQuan()
    {
        Init();
    }

    void Init()
    {
        this.Dock = DockStyle.Fill;
        this.Margin = new Padding(0);
        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            RowCount = 3,
            ColumnCount = 4,
            Dock = DockStyle.Fill,
            BackColor = MyColor.GrayBackGround,
            Padding = new Padding(0, 10, 0, 0),
            Margin = new Padding(0),
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));


        mainLayout.Controls.Add(new StatisticalBox("Tổng số sinh viên",
            sinhVienDao.CountSinhVienByStatus(TrangThaiSV.DangHoc), StatisticalIndex.first));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số giảng viên",
            giangVienDao.CountGiangVienByStatus(TrangThaiGV.DangCongTac), StatisticalIndex.second));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số ngành", nganhDao.CountNganhByStatus(1),
            StatisticalIndex.third));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số học phí đã thu", sinhVienDao.TongHocPhiDaThu(), StatisticalIndex.fourth));

        OverviewChart chart = GetOverViewChart();
        chart.BackColor = MyColor.White;
        mainLayout.Controls.Add(chart);
        mainLayout.SetColumnSpan(chart, 3);

        TableLayoutPanel sideContainer = GetPieChartContainer();
        mainLayout.Controls.Add(sideContainer);
        mainLayout.SetRowSpan(sideContainer, 2);

        TableLayoutPanel bottomBoxContainer = this.GetBottomContainer();
        mainLayout.Controls.Add(bottomBoxContainer);
        mainLayout.SetColumnSpan(bottomBoxContainer, 3);


        this.Controls.Add(mainLayout);
    }

    OverviewChart GetOverViewChart()
    {
        // int[] soSVTheoNam = new[] { 100, 150, 110, 90, 88, 120 };
        Dictionary<string, double> result = sinhVienDao.SoLuongSinhVienTheoNamNhapHoc();
        
        result = result.Take(7)
            .OrderBy(kv => kv.Key)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        
        string[] namNhapHoc = result.Keys.ToArray();
        double[] soSVTheoNam = result.Values.ToArray();

        OverviewChart chart = new OverviewChart(namNhapHoc, soSVTheoNam);
        chart.Dock = DockStyle.Fill;
        return chart;
    }

    RoundTLP GetPieChartContainer()
    {
        RoundTLP panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            AutoSize = true,
            Padding = new Padding(10),
            Margin = new Padding(10)
        };
        panel.BackColor = MyColor.White;
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Label title = new Label();
        title.AutoSize = true;
        title.Font = GetFont.GetFont.GetMainFont(13, FontType.SemiBold);
        title.Text = "Số sinh viên theo khóa";

        panel.Controls.Add(title);
        panel.Controls.Add(GetPieChart());
        panel.Controls.Add(new Panel());
        // panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        return panel;
    }

    CustomPieChart GetPieChart()
    {
        Dictionary<string, int> data = sinhVienDao.GetSinhVienCountByKhoaHoc();

        string[] dsKhoaHoc = data.Keys.ToArray();
        int total = data.Values.Sum();
        float[] percent = data.Values
            .Select(count => (float)Math.Round((float)count / total * 100, 2))
            .ToArray();

        CustomPieChart chart = new CustomPieChart(dsKhoaHoc, percent);
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

        Dictionary<string, float> data = sinhVienDao.TyLeSinhVienTotNghiepTheoNganh();
        
        var top5High = data
            .OrderByDescending(kv => kv.Value)
            .Take(5)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        
        string[] dsNganhHigh = top5High.Keys.ToArray();
        float[] tyLeHigh = top5High.Values.ToArray();
        
        StatisticalTop5Box box1 =
            new StatisticalTop5Box("Top 5 ngành có tỉ lệ tốt nghiệp cao nhất", dsNganhHigh, tyLeHigh);

        var top5Low = data
            .OrderBy(kv => kv.Value)
            .Take(5)
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        string[] dsNganhLow = top5Low.Keys.ToArray();
        float[] tyLeLow = top5Low.Values.ToArray();
        
        StatisticalTop5Box box2 =
            new StatisticalTop5Box("Top 5 ngành có tỉ lệ tốt nghiệp thấp nhất", dsNganhLow, tyLeLow);

        panel.Controls.Add(box1);
        panel.Controls.Add(box2);
        return panel;
    }
}