using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.ThongKe;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Chart;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class ThongKeDoanhThu : MyTLP
{
    private MyTLP _content;
    private List<object> _displayData;
    private ThongKeDoanhThuController _thongKeDoanhThuController;
    private List<ThongKeDoanhThuDto> _rawData;
    

    public ThongKeDoanhThu()
    {
        _rawData = new List<ThongKeDoanhThuDto>();
        _displayData = new List<object>();
        _thongKeDoanhThuController = ThongKeDoanhThuController.GetInstance();
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
        var title = GetLabel("Doanh thu");

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
        var header = new[] { "Mã ngành", "Tên ngành", "Tổng tiền đã thu" };
        var ColumnName = new[] { "MaNganh", "TenNganh", "TongTien" };
        _rawData = _thongKeDoanhThuController.GetListThongKeDoanhThu();
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
                TongTien = FormatMoney.formatVN(x.TongTien)
            }
        );
    }

    private void UpdateDisplayData()
    {
        
    }

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
            Text = "Top 5 ngành",
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


        List<string> dsKhoaHoc = new List<string>();
        List<float> percent =  new List<float>();
        
        List<NganhDoanhThu> listNganhDoanhThu = _thongKeDoanhThuController.GetListPieChart();
        foreach (var item in listNganhDoanhThu)
        {
            dsKhoaHoc.Add(item.ten);
            percent.Add((float)item.phanTram);
        }
        

        var chart = new CustomPieChart(dsKhoaHoc.ToArray(), percent.ToArray());
        container.Controls.Add(chart);
        panel.Controls.Add(container);


        _content.Controls.Add(panel);
    }
}