using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.View.Views.Components.CommonUse;

// using QuanLySinhVien.View.Views.Components.CommonUse.Chart;


namespace QuanLySinhVien.View.Views.Components.NavList;

public class ThongKe : NavBase
{
    private readonly string[] _listSelectionForComboBox = new[] { "" };
    
    private ThongKeHocLuc _panelThongKeHocLuc;
    private ThongKeTongQuan _panelThongKeTongQuan;
    private ThongKeDoanhThu _panelThongKeDoanhThu;
    private MyTLP mainLayout;


    public ThongKe(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        selectedItem = items[0];
        Init();
    }

    private void Init()
    {
        mainLayout = new MyTLP
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

    
    private List<ButtonChangeThongKe> listBtnChange = new ();
    private readonly string[] items = new[] { "Tổng quan", "Học lực", "Doanh thu" };
    private int selectedIndex = 0;
    private string selectedItem;
    private MyTLP GetChangeButtonBar()
    {
        
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = items.Length,
            Margin = new Padding(0, 0, 0, 0)
        };
        
        for (int i = 0; i < items.Length; i++)
        {
            var btn = new ButtonChangeThongKe(i, items[i]);
            listBtnChange.Add(btn);
            btn.mouseClick += (index, text) => changePanel(index, text);
            panel.Controls.Add(btn);
        }

        return panel;
    }
    

    private void changePanel(int index, string text)
    {
        // "Tổng quan", "Học lực", "Doanh thu"
        if (selectedItem.Equals(text)) return;

        if (selectedItem.Equals("Tổng quan"))
        {
            _panelThongKeTongQuan.Dispose();
            listBtnChange[0].UnSelected();
        }
        else if (selectedItem.Equals("Học lực"))
        {
            _panelThongKeHocLuc.Dispose();
            listBtnChange[1].UnSelected();
        }
        else if (selectedItem.Equals("Doanh thu"))
        {
            _panelThongKeDoanhThu.Dispose();
            listBtnChange[2].UnSelected();
        }
        
        if (text.Equals("Tổng quan"))
        {
            _panelThongKeTongQuan = new ThongKeTongQuan();
            mainLayout.Controls.Add(_panelThongKeTongQuan);

        }
        else if (text.Equals("Học lực"))
        {
            _panelThongKeHocLuc = new ThongKeHocLuc();
            mainLayout.Controls.Add(_panelThongKeHocLuc);
        }
        else if (text.Equals("Doanh thu"))
        {
            _panelThongKeDoanhThu = new ThongKeDoanhThu();
            mainLayout.Controls.Add(_panelThongKeDoanhThu);
        }
        
        listBtnChange[index].SetSelected();
        
        selectedItem = text;
        selectedIndex = index;
    }


    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(_listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}