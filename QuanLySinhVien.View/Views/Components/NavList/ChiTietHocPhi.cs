using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.NavList;

//Bên sinh viên
public class ChiTietHocPhi : NavBase
{
    private readonly string _title = "Chi tiết học phí";

    private MyTLP _contentLayout;
    private MyTLP _filterContainer;
    private MyTLP _mainLayout;

    private SinhVienDTO _sinhvien;

    private SinhVienController _sinhVienController;
    private HocPhiSVController _hocPhiSVController;
    private HocPhiHocPhanController _hocPhiHocPhanController;
    private CustomTable _table;
    private LabelTextField _semesterComboBox;
    
    private List<HocPhiSVDto> _rawData;
    private List<object> _displayData;
    private List<string> _comboboxOptions;
    
    private readonly string[] _listSelectionForComboBox = new[] { "" };
    
    private string _nam;
    private string _hocKy;
    
    private string ID = "CHITIETHOCPHI";

    public ChiTietHocPhi(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _taiKhoan = taiKhoan;
        _sinhVienController = SinhVienController.GetInstance();
        _hocPhiSVController = HocPhiSVController.GetInstance();
        _hocPhiHocPhanController = HocPhiHocPhanController.GetInstance();
        _sinhvien = _sinhVienController.GetByMaTK(taiKhoan.MaTK);
        _rawData = new List<HocPhiSVDto>();
        _displayData = new List<object>();
        _comboboxOptions = new List<string>();
        Init();
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
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTop();
        SetBottom();

        Controls.Add(_mainLayout);
    }

    private void SetTop()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Padding = new Padding(10),
            ColumnCount = 3,
            BackColor = MyColor.GrayBackGround
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.Controls.Add(getTitle());
        panel.Controls.Add(getSemesterComboBox());

        _mainLayout.Controls.Add(panel);
    }

    private void SetBottom()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 1
        };
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        LoadData();
        BuildComboboxOptions();
        SetupTable();
        UpdateTableData();

        _mainLayout.Controls.Add(_contentLayout);
    }

    //////////////////////////////SETTOP///////////////////////////////

    private LabelTextField getSemesterComboBox()
    {
        _semesterComboBox = new LabelTextField("Lọc theo", TextFieldType.Combobox);
        _semesterComboBox._combobox.Font = GetFont.GetFont.GetMainFont(10, FontType.Regular);
        _semesterComboBox._combobox.cbxWidth = 350;
        _semesterComboBox._combobox.combobox.Width = _semesterComboBox._combobox.cbxWidth;
        
        _semesterComboBox._combobox.combobox.SelectedIndexChanged += (sender, args) => UpdateTableData();
        
        return _semesterComboBox;
    }

    private Label getTitle()
    {
        var titlePnl = new Label
        {
            Text = _title,
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true
        };
        return titlePnl;
    }

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    
    private void LoadData()
    {
        _rawData = _hocPhiSVController.GetByMaSV(_sinhvien.MaSinhVien);
    }
    
    private void BuildComboboxOptions()
    {
        _comboboxOptions.Clear();
        _comboboxOptions.Add("Tổng hợp học phí tất cả học kì");
        
        // Get distinct and ordered semesters
        var uniqueSemesters = _hocPhiHocPhanController
            .GetNamHocKySVDaDK(maSV: _sinhvien.MaSinhVien)
            .Distinct() // record equality is structural by default
            .OrderBy(s => s.HocKy)
            .ThenBy(s => s.Nam);

        // Add formatted strings to combobox
        foreach (var sem in uniqueSemesters)
        {
            _comboboxOptions.Add($"Học kỳ {sem.HocKy} - Năm học {sem.Nam}");
        }
        
        _semesterComboBox.SetComboboxList(_comboboxOptions);
        _semesterComboBox.SetComboboxSelection("Tổng hợp học phí tất cả học kì");
    }
    
    private void SetupTable()
    {
        var headerList = new List<string> { "STT", "Học kỳ", "Năm", "Tổng học phí", "Đã thu", "Trạng thái" };
        var columnNames = new List<string> { "STT", "HocKy", "Nam", "TongHocPhi", "DaThu", "TrangThai" };
        
        _table = new CustomTable(headerList, columnNames, _displayData, false, false, false);
        _table.Dock = DockStyle.Fill;
        
        _contentLayout.Controls.Add(_table);
    }
    
    private void SetupDetailTable()
    {
        _contentLayout.Controls.Remove(_table);
        _table?.Dispose();
        
        var headerList = new List<string> { "STT", "Mã HP", "Tên HP", "Số TC", "Số tiền"};
        var columnNames = new List<string> { "STT", "MaHP", "TenHP", "SoTC", "SoTien" };
        
        _table = new CustomTable(headerList, columnNames, _displayData, false, false, false);
        _table.Dock = DockStyle.Fill;
        
        _contentLayout.Controls.Add(_table);
    }
    
    private void SetupSummaryTable()
    {
        _contentLayout.Controls.Remove(_table);
        _table?.Dispose();
        
        var headerList = new List<string> { "STT", "Học kỳ", "Năm", "Tổng học phí", "Đã thu", "Trạng thái" };
        var columnNames = new List<string> { "STT", "HocKy", "Nam", "TongHocPhi", "DaThu", "TrangThai" };
        
        _table = new CustomTable(headerList, columnNames, _displayData, false, false, false);
        _table.Dock = DockStyle.Fill;
        
        _contentLayout.Controls.Add(_table);
    }
    
    private void UpdateTableData()
    {
        if (_semesterComboBox == null || _rawData == null || _table == null)
        {
            return;
        }
        
        var selectedOption = _semesterComboBox.GetSelectionCombobox();
        if (string.IsNullOrEmpty(selectedOption))
        {
            selectedOption = "Tổng hợp học phí tất cả học kì";
        }
        
        if (selectedOption == "Tổng hợp học phí tất cả học kì")
        {
            UpdateSummaryView();
        }
        else
        {
            UpdateDetailView(selectedOption);
        }
    }
    
    private bool _isDetailView = false;
    
    private void UpdateSummaryView()
    {
        if (_rawData.Count == 0) return;
        
        if (_isDetailView)
        {
            SetupSummaryTable();
            _isDetailView = false;
        }
        
        var filteredData = _rawData.ToList();
        
        var rows = new List<List<object>>();
        var stt = 1;
        var totalTongHocPhi = 0.0;
        var totalDaThu = 0.0;
        
        foreach (var item in filteredData)
        {
            rows.Add(new List<object>
            {
                stt++,
                item.HocKy.ToString(),
                item.Nam,
                FormatMoney.formatVN(item.TongHocPhi),
                FormatMoney.formatVN(item.DaThu),
                item.TrangThai
            });
            totalTongHocPhi += item.TongHocPhi;
            totalDaThu += item.DaThu;
        }
        
        if (filteredData.Count > 0)
        {
            var tongNo = totalDaThu - totalTongHocPhi;
            rows.Add(new List<object>
            {
                "Tổng cộng",
                string.Empty,
                string.Empty,
                FormatMoney.formatVN(totalTongHocPhi),
                FormatMoney.formatVN(totalDaThu),
                FormatMoney.formatVN(tongNo)
            });
        }
        
        _table.UpdateData(rows);
        
        if (_table._dataGridView.Rows.Count > 0 && filteredData.Count > 0)
        {
            var lastRowIndex = _table._dataGridView.Rows.Count - 1;
            var lastRow = _table._dataGridView.Rows[lastRowIndex];
            lastRow.DefaultCellStyle.Font = GetFont.GetFont.GetMainFont(9, FontType.Bold);
            lastRow.DefaultCellStyle.BackColor = MyColor.GrayBackGround;
        }
    }
    
    private void UpdateDetailView(string selectedOption)
    {
        var parts = selectedOption.Split(new[] { " - " }, StringSplitOptions.None);
        if (parts.Length != 2)
        {
            UpdateSummaryView();
            return;
        }
        
        var hockyStr = parts[0].Replace("Học kỳ ", "");
        var namStr = parts[1].Replace("Năm học ", "");
        
        if (!int.TryParse(hockyStr, out var hocky))
        {
            UpdateSummaryView();
            return;
        }
        
        var detailData = _hocPhiSVController.GetDetailsByMaSVHocKyNam(_sinhvien.MaSinhVien, hocky, namStr);
        
        if (detailData == null || detailData.Count == 0)
        {
            SetupDetailTable();
            _table.UpdateData(new List<List<object>>());
            return;
        }
        
        if (!_isDetailView)
        {
            SetupDetailTable();
            _isDetailView = true;
        }
        
        var summaryEntry = _rawData?
            .FirstOrDefault(x => x.HocKy == hocky && x.Nam == namStr);
        var summaryDaThu = summaryEntry?.DaThu ?? 0.0;
        
        var rows = new List<List<object>>();
        var stt = 1;
        var totalSoTC = 0;
        var totalSoTien = 0.0;
        
        foreach (var item in detailData)
        {
            rows.Add(new List<object>
            {
                stt++,
                item.MaHP.ToString(),
                item.TenHP,
                item.SoTinChi.ToString(),
                FormatMoney.formatVN(item.SoTien),
            });
            totalSoTC += item.SoTinChi;
            totalSoTien += item.SoTien;
        }
        
        if (detailData.Count > 0)
        {
            var summaryTongHocPhi = summaryEntry?.TongHocPhi ?? totalSoTien;
            var summaryConNo = summaryTongHocPhi - summaryDaThu;
            if (summaryConNo < 0) summaryConNo = 0;
            
            rows.Add(new List<object>
            {
                "Tổng cộng",
                string.Empty,
                string.Empty,
                totalSoTC.ToString(),
                FormatMoney.formatVN(totalSoTien),
            });
            
            rows.Add(new List<object>
            {
                "Đã thu",
                string.Empty,
                string.Empty,
                string.Empty,
                FormatMoney.formatVN(summaryDaThu)
            });
            
            rows.Add(new List<object>
            {
                "Còn nợ",
                string.Empty,
                string.Empty,
                string.Empty,
                FormatMoney.formatVN(summaryConNo)
            });
        }
        
        _table.UpdateData(rows);
        
        if (_table._dataGridView.Rows.Count > 0 && detailData.Count > 0)
        {
            var lastRowIndex = _table._dataGridView.Rows.Count - 1;
            var lastRow = _table._dataGridView.Rows[lastRowIndex];
            lastRow.DefaultCellStyle.Font = GetFont.GetFont.GetMainFont(9, FontType.Bold);
            lastRow.DefaultCellStyle.BackColor = MyColor.GrayBackGround;
        }
    }

    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox.ToList();
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}