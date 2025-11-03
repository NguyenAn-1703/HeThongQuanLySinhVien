using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Chart;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.NavList;

public class ThongKeHocLuc : TableLayoutPanel
{
    private TableLayoutPanel _content;
    private List<object> _displayData;
    private NganhController _nganhController;
    private List<NganhDto> _rawData;

    public ThongKeHocLuc()
    {
        _rawData = new List<NganhDto>();
        _displayData = new List<object>();
        _nganhController = NganhController.GetInstance();
        Init();
    }

    private void Init()
    {
        Configuration();
        SetTitle();
        SetContent();
    }

    private void Configuration()
    {
        Dock = DockStyle.Fill;
        BackColor = MyColor.GrayBackGround;
        Margin = new Padding(0);
        RowCount = 2;

        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Percent, 100));
    }

    private void SetTitle()
    {
        var panel = new TableLayoutPanel
        {
            ColumnCount = 2,
            AutoSize = true
        };
        var title = GetLabel("Học lực");

        var combobox = new CustomCombobox(new[] { "Ngành", "Khóa" });
        combobox.Anchor = AnchorStyles.None;
        combobox.SetSelectionCombobox("Ngành");

        panel.Controls.Add(title);
        panel.Controls.Add(combobox);
        Controls.Add(panel);
    }

    private Label GetLabel(string i)
    {
        var lbl = new Label
        {
            Text = i,
            Font = GetFont.GetFont.GetMainFont(14, FontType.Black),
            AutoSize = true,
            Margin = new Padding(30, 10, 10, 10)
        };

        return lbl;
    }

    private void SetContent()
    {
        _content = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill
        };
        _content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _content.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        // Panel box1 = new Panel{BackColor = Color.Black};
        // Panel box2 = new Panel{BackColor = Color.Blue};


        SetTable();
        SetPieChart();

        // panel.Controls.Add(box1);
        // _content.Controls.Add(box2);

        Controls.Add(_content);
    }

    private void SetTable()
    {
        var header = new[] { "Mã ngành", "Tên ngành", "Số sinh viên giỏi, xuất sắc" };
        var ColumnName = new[] { "MaNganh", "TenNganh", "SoSV" };
        _rawData = _nganhController.GetAll();
        SetDisplayData();
        var table = new CustomTable(header.ToList(), ColumnName.ToList(), _displayData.ToList());
        table.Margin = new Padding(6);
        _content.Controls.Add(table);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                x.MaNganh,
                x.TenNganh,
                SoSV = 100
            }
        );
    }

    private void SetPieChart()
    {
        var container = new RoundTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.White,
            Margin = new Padding(6)
        };
        var dsKhoaHoc = new[] { "Xuất sắc", "Giỏi", "Khá", "Trung bình", "Yếu" };
        var percent = new[] { 20f, 20f, 25f, 25f, 10f };
        var chart = new CustomPieChart(dsKhoaHoc, percent);
        chart.Dock = DockStyle.Top;
        container.Controls.Add(chart);


        _content.Controls.Add(container);
    }
}