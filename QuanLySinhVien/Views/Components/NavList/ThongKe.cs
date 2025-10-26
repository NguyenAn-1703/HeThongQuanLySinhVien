using LiveChartsCore.SkiaSharpView.WinForms;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Chart;
// using QuanLySinhVien.Views.Components.CommonUse.Chart;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;


namespace QuanLySinhVien.Views.Components.NavList;

public class ThongKe : NavBase
{
    private string[] _listSelectionForComboBox = new []{""};
    private ThongKeTongQuan _panelThongKeTongQuan;
    private ThongKeHocLuc _panelThongKeHocLuc;
    private string[] items = new[] { "Tổng quan", "Học lực" };
    private string selectedItem;
    private TableLayoutPanel mainLayout;
    public ThongKe(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        selectedItem = items[0];
        Init();
    }

    void Init()
    {
        mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
        };
        
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        _panelThongKeTongQuan = new ThongKeTongQuan();

        TableLayoutPanel bar = GetChangeButtonBar();
        
        mainLayout.Controls.Add(bar);
        mainLayout.Controls.Add(_panelThongKeTongQuan);
        
        this.Controls.Add(mainLayout);
    }


    TableLayoutPanel GetChangeButtonBar()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = items.Length,
            Margin = new Padding(0, 0, 0, 0),
        };
        foreach (string i in items)
        {
            ButtonChangeThongKe btn = new ButtonChangeThongKe(i);
            btn.mouseClick += (text) => changePanel(text);
            panel.Controls.Add(btn);
            
        }
        return panel;
    }

    //làm tạm
    void changePanel(string text)
    {
        // "Tổng quan", "Học lực"
        if (selectedItem.Equals(text))
        {
            return;
        }

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
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    public override void onSearch(string txtSearch, string filter)
    { }
    
}