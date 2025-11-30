using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.utils;
using QuanLySinhVien.View.Views.Components.CommonUse;
using System.Data;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class ChiTietLichThi : NavBase
{
    private readonly string _title = "Chi tiết lịch thi";
    private readonly string ID = "CHITIETLICHTHI";
    
    private MyTLP _mainLayout;
    private MyTLP _contentLayout; // Panel chứa bảng
    private SinhVienDTO _sinhvien;
    private SinhVienController _sinhVienController;
    
    private LichThiController _lichThiController;
    private List<LichThiSVDto> _fullList;
    private List<LichThiSVDto> _displayList;
    
    // Components
    private CustomTable _table;
    private CustomCombobox _cbHocKy;
    private CustomCombobox _cbLoaiLich;
    private TitleButton _exportExBtn;

    // Header bảng
    private string[] _headerArr = { "STT", "Mã MH", "Tên môn học", "Sĩ số", "Ngày thi", "Giờ bắt đầu", "Phòng thi", "Cơ sở" };
    private string[] _columnNames = { "STT", "MaMH", "TenMonHoc", "SiSo", "NgayThi", "GioBatDau", "PhongThi", "CoSo" };

    public ChiTietLichThi(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _taiKhoan = taiKhoan;
        
        // Init Controllers
        _sinhVienController = SinhVienController.GetInstance();
        _lichThiController = LichThiController.GetInstance();
        
        // Lấy thông tin sinh viên
        _sinhvien = _sinhVienController.GetByMaTK(taiKhoan.MaTK);

        Init();
        LoadData(); // Gọi hàm load dữ liệu
    }

    private void Init()
    {
        Dock = DockStyle.Fill;

        _mainLayout = new MyTLP
        {
            RowCount = 3,
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 0, 0, 50)
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Top
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content (Table)
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Footer

        SetTop();
        SetBottom();

        Controls.Add(_mainLayout);
    }

    // Top: lọc + title + export
    private void SetTop()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Padding = new Padding(10),
            ColumnCount = 2, // title + ( lọc và export )
            BackColor = MyColor.GrayBackGround
        };

        // Title
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.Controls.Add(getTitle());

        //  Lọc + export
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        
        MyTLP rightContainer = new MyTLP
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            ColumnCount = 3
        };

        // Combobox lọc theo học kỳ ( tất cả )
        List<string> listHocKy = new List<string> { "Tất cả học kỳ" };
        if (_sinhvien != null && _sinhvien.MaSinhVien > 0)
        {
            listHocKy.AddRange(_lichThiController.GetListHocKyFilter(_sinhvien.MaSinhVien));
        }

        _cbHocKy = new CustomCombobox(listHocKy.ToArray());
        _cbHocKy.cbxWidth = 200;
        _cbHocKy.combobox.SelectedIndexChanged += (s, e) => FilterData();
        _cbHocKy.combobox.SelectedIndex = 0;
        rightContainer.Controls.Add(_cbHocKy);

        // Combobox lọc theo thứ tự ( tgian, a-z, lịch cá nhân )
        List<string> listLoaiLich = new List<string> { 
            "Lịch thi cá nhân", 
            "Theo môn học (A-Z)", 
            "Theo ngày thi (Tăng dần)" 
        };
        _cbLoaiLich = new CustomCombobox(listLoaiLich.ToArray());
        _cbLoaiLich.cbxWidth = 180;
        _cbLoaiLich.Margin = new Padding(10, 4, 10, 0);
        _cbLoaiLich.combobox.SelectedIndexChanged += (s, e) => FilterData();
        _cbLoaiLich.combobox.SelectedIndex = 0;
        rightContainer.Controls.Add(_cbLoaiLich);

        // export excel
        _exportExBtn = new TitleButton("Xuất Excel", "exportexcel.svg")
        {
            BackgroundColor = MyColor.Green,
            HoverColor = MyColor.GreenHover,
            SelectColor = MyColor.GreenClick,
        };
        _exportExBtn._label.Font = GetFont.GetFont.GetMainFont(12, FontType.Bold);
        _exportExBtn._mouseDown += ExportExcel;
        rightContainer.Controls.Add(_exportExBtn);

        // add
        panel.Controls.Add(rightContainer);

        _mainLayout.Controls.Add(panel);
    }

    // Content (table)
    private void SetBottom()
    {
        // Layout
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 1,
            ColumnCount = 1
        };
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _mainLayout.Controls.Add(_contentLayout);
    }

    private Label getTitle()
    {
        var titlePnl = new Label
        {
            Text = _title,
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true,
            Anchor = AnchorStyles.Left
        };
        return titlePnl;
    }

    // function
    private void LoadData()
    {
        if (_sinhvien == null || _sinhvien.MaSinhVien == 0) return;
        
        _fullList = _lichThiController.GetLichThiCaNhan(_sinhvien.MaSinhVien);
        
        // load data
        FilterData();
    }

    // Các bộ lọc
    private void FilterData()
    {
        if (_fullList == null) return;
        var listTemp = new List<LichThiSVDto>(_fullList);

        // Filter Học kỳ
        string selectedHK = _cbHocKy.combobox.SelectedItem?.ToString() ?? "Tất cả học kỳ";
        if (selectedHK != "Tất cả học kỳ")
        {
            listTemp = listTemp.Where(x => $"Học kỳ {x.HocKy} năm {x.Nam}" == selectedHK).ToList();
        }

        // Sort Loại lịch
        string sortType = _cbLoaiLich.combobox.SelectedItem?.ToString() ?? "";
        switch (sortType)
        {
            case "Theo môn học (A-Z)":
                listTemp = listTemp.OrderBy(x => x.TenMonHoc).ToList();
                break;
            case "Theo ngày thi (Tăng dần)":
                listTemp = listTemp.OrderBy(x => x.ThoiGianThuc).ToList();
                break;
            default:
                listTemp = listTemp.OrderBy(x => x.ThoiGianThuc).ToList();
                break;
        }

        // STT
        for (int i = 0; i < listTemp.Count; i++) listTemp[i].STT = i + 1;

        _displayList = listTemp;
        RenderTable();
    }

    private void RenderTable()
    {
        // clean
        _contentLayout.Controls.Clear();
        
        var headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArr);
        var cells = _displayList.Cast<object>().ToList();

        // View Only
        _table = new CustomTable(headerList, _columnNames.ToList(), cells, false, false, false);
        _table.Dock = DockStyle.Fill;
        _table.Margin = new Padding(0);
        
        _contentLayout.Controls.Add(_table);
    }

    // export...
    private void ExportExcel()
    {
        if (_displayList == null || _displayList.Count == 0)
        {
            MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
            return;
        }

        var header = new Dictionary<string, string>();
        for (int i = 0; i < _columnNames.Length; i++)
        {
            header.Add(_columnNames[i], _headerArr[i]);
        }
        
        string sheetName = _cbHocKy.combobox.Text;
        if (string.IsNullOrEmpty(sheetName)) sheetName = "LichThi";
        
        ExcelExporter.ExportToExcel(_displayList, "LichThi", $"Lịch thi - {sheetName}", header);
    }

    // Override NavBase
    public override List<string> getComboboxList() { return new List<string>(); }
    public override void onSearch(string txtSearch, string filter) { }
}