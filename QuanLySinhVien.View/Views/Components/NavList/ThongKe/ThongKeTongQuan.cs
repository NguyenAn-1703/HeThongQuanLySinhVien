using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Chart;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class ThongKeTongQuan : MyTLP
{
    #region Constructor
    
    SinhVienController _sinhVienController;

    public ThongKeTongQuan()
    {
        _sinhVienController = SinhVienController.GetInstance();
        _sinhVienController.UpdateTrangThaiSv();
        InitializeComponent();
    }

    #endregion

    #region Initialization

    private void InitializeComponent()
    {
        Dock = DockStyle.Fill;
        Margin = new Padding(0);

        var mainLayout = new MyTLP
        {
            RowCount = ROW_COUNT,
            ColumnCount = COLUMN_COUNT,
            Dock = DockStyle.Fill,
            BackColor = MyColor.GrayBackGround,
            Padding = new Padding(0, PADDING_VALUE, 0, 0),
            Margin = new Padding(0)
        };

        for (var i = 0; i < COLUMN_COUNT; i++)
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, COLUMN_PERCENTAGE));

        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        // Add statistical boxes
        mainLayout.Controls.Add(new StatisticalBox("Tổng số sinh viên",
            _sinhVienDao.CountSinhVienByStatus(TrangThaiSV.DangHoc), StatisticalIndex.first));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số giảng viên",
            GiangVienDao.GetAll().Count, StatisticalIndex.second));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số ngành", _nganhDao.CountNganhByStatus(ACTIVE_STATUS),
            StatisticalIndex.third));
        mainLayout.Controls.Add(new StatisticalBox("Tổng số học phí đã thu", _sinhVienDao.TongHocPhiDaThu(),
            StatisticalIndex.fourth));

        // Add overview chart
        var overviewChart = CreateOverviewChart();
        overviewChart.BackColor = MyColor.White;
        mainLayout.Controls.Add(overviewChart);
        mainLayout.SetColumnSpan(overviewChart, 3);

        // Add pie chart container
        var pieChartContainer = CreatePieChartContainer();
        mainLayout.Controls.Add(pieChartContainer);
        mainLayout.SetRowSpan(pieChartContainer, 2);

        // Add bottom container
        var bottomContainer = CreateBottomContainer();
        mainLayout.Controls.Add(bottomContainer);
        mainLayout.SetColumnSpan(bottomContainer, 3);

        Controls.Add(mainLayout);
    }

    #endregion

    #region Constants

    private const int OVERVIEW_CHART_ITEM_LIMIT = 7;
    private const int TOP_5_ITEM_LIMIT = 5;
    private const int ACTIVE_STATUS = 1;
    private const int COLUMN_COUNT = 4;
    private const int ROW_COUNT = 3;
    private const int COLUMN_PERCENTAGE = 25;
    private const int BOTTOM_CONTAINER_COLUMN_COUNT = 2;
    private const int BOTTOM_CONTAINER_COLUMN_PERCENTAGE = 50;
    private const int PIE_CHART_ROW_COUNT = 2;
    private const int PADDING_VALUE = 10;

    #endregion

    #region Fields

    private readonly SinhVienDAO _sinhVienDao = new();

    // private readonly GiangVienDao _giangVienDa;
    private readonly NganhDao _nganhDao = NganhDao.GetInstance();

    #endregion

    #region Chart Creation Methods

    // Tong so sinh vien nhap hoc 7 nam gan nhat
    private OverviewChart CreateOverviewChart()
    {
        var studentData = _sinhVienDao.SoLuongSinhVienTheoNamNhapHoc();

        var processedData = GetTopNItems(studentData, OVERVIEW_CHART_ITEM_LIMIT, false, true);
        var (years, counts) = ExtractKeysAndValues(processedData);

        return new OverviewChart(years, counts)
        {
            Dock = DockStyle.Fill,
        };
    }

    // So sinh vien theo khoa hoc panel
    private RoundTLP CreatePieChartContainer()
    {
        var panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            // RowCount = PIE_CHART_ROW_COUNT,
            // AutoSize = true,
            // Anchor =AnchorStyles.None,
            Padding = new Padding(PADDING_VALUE),
            Margin = new Padding(PADDING_VALUE),
            BackColor = MyColor.White,
        };

        // Configure row styles
        // for (var i = 0; i < PIE_CHART_ROW_COUNT; i++) panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var title = new Label
        {
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(13, FontType.SemiBold),
            Anchor = AnchorStyles.None,
            Text = "Số sinh viên theo khóa",
            Margin = new Padding(3, 5, 3, 3),
        };

        RoundTLP container = new RoundTLP
        {
            AutoSize = true,
            Border = true,
            Dock = DockStyle.Top,
            Margin = new Padding(3, 3,3,50),
        };
        
        container.Controls.Add(title);
        container.Controls.Add(CreatePieChart());

        panel.Controls.Add(container);

        return panel;
    }

    // Pie Chart Ti le so sinh vien theo khoa hoc
    private CustomPieChart CreatePieChart()
    {
        var courseData = _sinhVienDao.GetSinhVienCountByKhoaHoc();
        var (courseNames, counts) = ExtractKeysAndValues(courseData);

        var total = counts.Sum();
        var percentages = counts
            .Select(count => (float)Math.Round((float)count / total * 100, 2))
            .ToArray();

        return new CustomPieChart(courseNames, percentages)
        {
            Dock = DockStyle.Top,
        };
    }

    private MyTLP CreateBottomContainer()
    {
        var panel = new MyTLP
        {
            ColumnCount = BOTTOM_CONTAINER_COLUMN_COUNT,
            Dock = DockStyle.Fill,
            AutoSize = true
        };

        // Configure column styles
        for (var i = 0; i < BOTTOM_CONTAINER_COLUMN_COUNT; i++)
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, BOTTOM_CONTAINER_COLUMN_PERCENTAGE));

        var graduationData = _sinhVienDao.TyLeSinhVienTotNghiepTheoNganh();

        // Top 5 nganh co ti le tot nghiep cao nhat
        var top5HighData = GetTopNItems(graduationData, TOP_5_ITEM_LIMIT, true);
        var (highMajors, highRates) = ExtractKeysAndValues(top5HighData);
        var highBox = new StatisticalTop5Box("Top 5 ngành có tỉ lệ tốt nghiệp cao nhất", highMajors, highRates);

        // Top 5 nganh co ti le tot nghiep thap nhat
        var top5LowData = GetTopNItems(graduationData, TOP_5_ITEM_LIMIT, false);
        var (lowMajors, lowRates) = ExtractKeysAndValues(top5LowData);
        var lowBox = new StatisticalTop5Box("Top 5 ngành có tỉ lệ tốt nghiệp thấp nhất", lowMajors, lowRates);

        panel.Controls.Add(highBox);
        panel.Controls.Add(lowBox);
        return panel;
    }

    #endregion

    #region Helper Methods

    private Dictionary<TKey, TValue> GetTopNItems<TKey, TValue>(Dictionary<TKey, TValue> data, int count,
        bool descending, bool sortByKey = false)
        where TKey : IComparable<TKey>
        where TValue : IComparable<TValue>
    {
        var orderedData = sortByKey
            ? descending ? data.OrderByDescending(kv => kv.Key) : data.OrderBy(kv => kv.Key)
            : descending
                ? data.OrderByDescending(kv => kv.Value)
                : data.OrderBy(kv => kv.Value);

        return orderedData
            .Take(count)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    private (TKey[] keys, TValue[] values) ExtractKeysAndValues<TKey, TValue>(Dictionary<TKey, TValue> data)
    {
        return (data.Keys.ToArray(), data.Values.ToArray());
    }

    #endregion
}