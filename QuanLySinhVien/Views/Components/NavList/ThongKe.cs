using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Views.Components.CommonUse;

// using QuanLySinhVien.Views.Components.CommonUse.Chart;


namespace QuanLySinhVien.Views.Components.NavList;

public class ThongKe : NavBase
{
    private readonly string[] _listSelectionForComboBox = new[] { "" };
    private readonly string[] items = new[] { "Tổng quan", "Học lực" };
    private ThongKeHocLuc _panelThongKeHocLuc;
    private ThongKeTongQuan _panelThongKeTongQuan;
    private TableLayoutPanel mainLayout;
    private string selectedItem;

    public ThongKe(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        selectedItem = items[0];
        Init();
    }

    private void Init()
    {
        mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2
        };

        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        _panelThongKeTongQuan = new ThongKeTongQuan();

        var bar = GetChangeButtonBar();

        mainLayout.Controls.Add(bar);
        mainLayout.Controls.Add(_panelThongKeTongQuan);

        Controls.Add(mainLayout);
    }


    private TableLayoutPanel GetChangeButtonBar()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = items.Length,
            Margin = new Padding(0, 0, 0, 0)
        };
        foreach (var i in items)
        {
            var btn = new ButtonChangeThongKe(i);
            btn.mouseClick += text => changePanel(text);
            panel.Controls.Add(btn);
        }

        return panel;
    }

    //làm tạm
    private void changePanel(string text)
    {
        // "Tổng quan", "Học lực"
        if (selectedItem.Equals(text)) return;

        if (selectedItem.Equals("Tổng quan"))
        {
            _panelThongKeTongQuan.Dispose();
            _panelThongKeHocLuc = new ThongKeHocLuc();
            mainLayout.Controls.Add(_panelThongKeHocLuc);
            selectedItem = "Học lực";
        }
        else if (selectedItem.Equals("Học lực"))
        {
            _panelThongKeHocLuc.Dispose();
            _panelThongKeTongQuan = new ThongKeTongQuan();
            mainLayout.Controls.Add(_panelThongKeTongQuan);
            selectedItem = "Tổng quan";
        }
    }


    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(_listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}