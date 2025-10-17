using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Chart;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList;

public class ThongKeHocLuc : TableLayoutPanel
{
    List<NganhDto> _rawData;
    List<object> _displayData;
    TableLayoutPanel _content;
    private NganhController _nganhController;
    public ThongKeHocLuc()
    {
        _rawData = new List<NganhDto>();
        _displayData = new List<object>();
        _nganhController = NganhController.GetInstance();
        Init();
    }

    void Init()
    {
        Configuration();
        SetTitle();
        SetContent();
    }

    void Configuration()
    {
        Dock = DockStyle.Fill;
        BackColor = MyColor.GrayBackGround;
        Margin = new Padding(0);
        RowCount = 2;
        
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
    }

    void SetTitle()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            ColumnCount = 2,
            AutoSize = true,
        };
        Label title = GetLabel("Học lực");

        CustomCombobox combobox = new CustomCombobox(new string[] {"Ngành", "Khóa"});
        combobox.Anchor = AnchorStyles.None;
        combobox.SetSelectionCombobox("Ngành");
        
        panel.Controls.Add(title);
        panel.Controls.Add(combobox);
        this.Controls.Add(panel);
    }

    Label GetLabel(string i)
    {
        Label lbl = new Label
        {
            Text = i,
            Font = GetFont.GetFont.GetMainFont(14, FontType.Black),
            AutoSize = true,
            Margin = new Padding(30, 10, 10, 10),
        };

        return lbl;
    }

    void SetContent()
    {
        _content = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
        };
        _content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _content.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        // Panel box1 = new Panel{BackColor = Color.Black};
        // Panel box2 = new Panel{BackColor = Color.Blue};


        SetTable();
        SetPieChart();
        
        // panel.Controls.Add(box1);
        // _content.Controls.Add(box2);

        this.Controls.Add(_content);
    }

    void SetTable()
    {
        string[] header = new[] { "Mã ngành", "Tên ngành", "Số sinh viên giỏi, xuất sắc"};
        string[] ColumnName = new[] { "MaNganh", "TenNganh", "SoSV"};
        _rawData = _nganhController.GetAll();
        SetDisplayData();
        CustomTable table = new CustomTable(header.ToList(), ColumnName.ToList(), _displayData.ToList(), false, false, false);
        table.Margin = new Padding(6);
        _content.Controls.Add(table);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                MaNganh = x.MaNganh,
                TenNganh = x.TenNganh,
                SoSV = 100
            }
        );
    }

    void SetPieChart()
    {
        RoundTLP container = new RoundTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.White,
            Margin = new Padding(6),
        };
        string[] dsKhoaHoc = new[] { "Xuất sắc","Giỏi","Khá","Trung bình","Yếu" };
        float[] percent = new[] { 20f, 20f, 25f, 25f, 10f };
        CustomPieChart chart = new CustomPieChart(dsKhoaHoc,  percent);
        chart.Dock = DockStyle.Top;
        container.Controls.Add(chart);

        
        _content.Controls.Add(container);
    }
}