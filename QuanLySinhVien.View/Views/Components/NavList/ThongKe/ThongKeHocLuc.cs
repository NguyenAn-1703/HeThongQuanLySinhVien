using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.DTO.ThongKe;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Chart;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class ThongKeHocLuc : MyTLP
{
    private MyTLP _content;
    private List<object> _displayData;
    private NganhController _nganhController;
    private List<ThongKeHocLucDto> _rawData;
    private ThongKeHocLucController _thongKeHocLucController;
    private KetQuaController _ketQuaController;

    public ThongKeHocLuc()
    {
        _rawData = new List<ThongKeHocLucDto>();
        _displayData = new List<object>();
        _nganhController = NganhController.GetInstance();
        _thongKeHocLucController = ThongKeHocLucController.GetInstance();
        _ketQuaController = KetQuaController.GetInstance();
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
        Padding = new Padding(5);
        RowCount = 2;

        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Percent, 100));
    }

    private void SetTitle()
    {
        var panel = new MyTLP
        {
            ColumnCount = 2,
            AutoSize = true,
            Dock = DockStyle.Fill,
        };
        var title = GetLabel("Học lực");

        var combobox = new CustomCombobox(new[] { "Ngành", "Khóa" }) {Margin = new Padding(3, 7, 400, 3)};
        combobox.Anchor = AnchorStyles.Right;
        combobox.SetSelectionCombobox("Ngành");

        panel.Controls.Add(title);
        // panel.Controls.Add(combobox);
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
        _content = new MyTLP
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
        var header = new[] { "Mã ngành", "Tên ngành", "Tổng sinh viên", "Số sinh viên giỏi, xuất sắc", "% Giỏi, Xuất sắc" };
        var ColumnName = new[] { "MaNganh", "TenNganh", "TongSV",  "SVGioiXS", "TiLe" };
        _rawData = _thongKeHocLucController.GetSinhVienThongKeHocLuc();

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
                x.TongSV,
                x.SVGioiXS,
                x.TiLe
            }
        );
    }
    

    private void UpdateDisplayData()
    {
        
    }

    private string[] percentPieChart;
    private void SetPieChart()
    {
        var panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.White,
            Margin = new Padding(6),
            Padding = new Padding(10),
            RowCount = 2
        };
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        Label title = new Label
        {
            Text = "Tỉ lệ học lực",
            AutoSize = true,
            Anchor = AnchorStyles.None,
            Font = GetFont.GetFont.GetMainFont(13,  FontType.SemiBold),
        };
        panel.Controls.Add(title);
        
        RoundTLP container = new RoundTLP
        {
            Dock = DockStyle.Top,
            Border = true,
            AutoSize = true,
        };
        
        var dsKhoaHoc = new[] { "Xuất sắc", "Giỏi", "Khá", "Trung bình", "Yếu" };
        var percentPieChart = _ketQuaController.SetPercentPiChart().ToArray();
        var chart = new CustomPieChart(dsKhoaHoc, percentPieChart);
        container.Controls.Add(chart);
        panel.Controls.Add(container);
        
        _content.Controls.Add(panel);
    }


    
}