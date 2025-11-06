using OpenTK.Platform.Windows;
using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.utils;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.NavList;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.View.Views.Components;

public class PhongHoc : NavBase
{
    // public int MaPH { get; set; }
    // public string TenPH { get; set; }
    // public string LoaiPH { get; set; }
    // public string CoSo { get; set; }
    // public int SucChua { get; set; }
    // public string TinhTrang { get; set; }
    
    private readonly string[] _listSelectionForComboBox = { "Mã phòng", "Tên phòng", "Loại phòng", "Cơ sở" };
    private string[] _headerArr = { "Mã phòng", "Tên phòng", "Loại phòng", "Cơ sở" , "Sức chứa", "Tình trạng"};
    private string[] _columnNames = { "MaPH", "TenPH", "LoaiPH", "CoSo", "SucChua", "TinhTrang"};

    // variable
    private readonly string ID = "PHONGHOC";
    private TitleButton _insertButton;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<ChiTietQuyenDto> _listAccess;

    private List<PhongHocDto> _listPhongHoc;

    private Panel _mainBot;
    private PhongHocController _phongHocController;
    private SearchBar _searchBar;
    private CustomTable _table;

    private string _title = "Phòng học";
    
    private bool sua;

    private bool them;
    private bool xoa;

    public PhongHoc(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _phongHocController = PhongHocController.getInstance();
        Init();
        LoadData();
        SetActionListener();
        try
        {
            if (_searchBar != null)
            {
                _searchBar.UpdateListCombobox(_listSelectionForComboBox.ToList());
                _searchBar.KeyDown += (txt, filter) => onSearch(txt, filter);
            }
        }
        catch
        {
            // khong co gi..
        }
    }

    private void Init()
    {
        CheckQuyen();
        
        Dock = DockStyle.Fill;

        var mainLayout = new MyTLP
        {
            RowCount = 2,
            Dock = DockStyle.Fill
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _mainBot = Bottom();
        mainLayout.Controls.Add(Top());
        mainLayout.Controls.Add(_mainBot);

        Controls.Add(mainLayout);
    }


    private void CheckQuyen()
    {
        int maCN = _chucNangController.GetByTen(ID).MaCN;
        _listAccess = _chiTietQuyenController.GetByMaNQMaCN(_quyen.MaNQ, maCN);
        foreach (var x in _listAccess) Console.WriteLine(x.HanhDong);
        
        if (_listAccess.Any(x => x.HanhDong.Equals("Them"))) them = true;
        if (_listAccess.Any(x => x.HanhDong.Equals("Sua"))) sua = true;
        if (_listAccess.Any(x => x.HanhDong.Equals("Xoa"))) xoa = true;
    }


    private TitleButton _exportExBtn;
    private new Panel Top()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            // CellBorderStyle = MyTLPCellBorderStyle.Single,
            Padding = new Padding(10),
            ColumnCount = 2,
            BackColor = MyColor.GrayBackGround
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        panel.Controls.Add(getTitle());
        
        MyTLP container = new MyTLP
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            ColumnCount = 2
        };
        
        _exportExBtn = new TitleButton("Xuất", "exportexcel.svg")
        {
            BackgroundColor = MyColor.Green,
            HoverColor = MyColor.GreenHover,
            SelectColor = MyColor.GreenClick,
        };
        _exportExBtn.SetBackGroundColor(MyColor.Green);
        _exportExBtn.Margin = new Padding(3, 3, 10, 3);
        _exportExBtn._label.Font = GetFont.GetFont.GetMainFont(12, FontType.Bold);
        _exportExBtn.Anchor = AnchorStyles.Right;

        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        container.Controls.Add(_exportExBtn);
        if (them)
        {
            container.Controls.Add(_insertButton);
        }
        
        _insertButton._mouseDown += BtnThem_Click;
        
        panel.Controls.Add(container);

        return panel;
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

    private new Panel Bottom()
    {
        return new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = MyColor.GrayBackGround,
            // Padding = new Padding(20, 0, 20, 20)
        };
    }

    private void SetActionListener()
    {
        if (_table != null)
        {
            _table.OnEdit += maPH => BtnSua_Click(maPH);
            _table.OnDelete += maPH => BtnXoa_Click(maPH);
            _table.OnDetail += maPH => BtnChiTiet_Click(maPH);
        }
        _exportExBtn._mouseDown += () => ExportExcel(_listPhongHoc);
    }
    void ExportExcel(List<PhongHocDto> list)
    {
        var header = new Dictionary<string, string>();
        for (int i = 0; i < _columnNames.Length; i++)
        {
            header.Add(_columnNames[i], _headerArr[i]);
        }
        
        ExcelExporter.ExportToExcel(list, "sheet1","Danh sách phòng học", header);
    }

    private void ShowSchedule(int maPh)
    {
        throw new NotImplementedException();
    }

    public void LoadData()
    {
        _listPhongHoc = _phongHocController.GetDanhSachPhongHoc();

        if (_table == null)
        {
            string[] headerArray = { "Mã PH", "Tên phòng", "Loại phòng", "Cơ sở", "Sức chứa", "Tình trạng" };
            var headerList = headerArray.ToList();
            var columnNames = new List<string> { "MaPH", "TenPH", "LoaiPH", "CoSo", "SucChua", "TinhTrang" };

            var cells = _listPhongHoc.Cast<object>().ToList();
            _table = new CustomTable(headerList, columnNames, cells, sua || xoa, sua, xoa)
            {
                Margin = new Padding(0),
            };
            _mainBot.Controls.Add(_table);
        }
        else
        {
            _table.UpdateData(_listPhongHoc.Cast<object>().ToList());
        }
    }

    // event
    private void BtnThem_Click()
    {
        try
        {
            using (var dialog = new PhongHocDialog("Thêm Phòng học mới", DialogType.Them))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var resultDto = dialog.GetResult();
                    _phongHocController.ThemPhongHoc(resultDto);
                    MessageBox.Show("Thêm thành công!", "Thông báo");
                    LoadData();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi thêm phòng học: " + ex.Message);
        }
    }

    private void BtnSua_Click(int maPH)
    {
        try
        {
            var selectedItem = _listPhongHoc.FirstOrDefault(p => p.MaPH == maPH);

            if (selectedItem == null)
            {
                MessageBox.Show("Không tìm thấy phòng học!", "Thông báo");
                return;
            }

            using (var dialog = new PhongHocDialog("Sửa thông tin phòng học", DialogType.Sua))
            {
                dialog.LoadData(selectedItem);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var resultDto = dialog.GetResult();
                    _phongHocController.SuaPhongHoc(resultDto);
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                    LoadData();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi sửa phòng học: " + ex.Message);
        }
    }

    private void BtnXoa_Click(int maPH)
    {
        try
        {
            var selectedItem = _listPhongHoc.FirstOrDefault(p => p.MaPH == maPH);

            if (selectedItem == null)
            {
                MessageBox.Show("Không tìm thấy phòng học!", "Thông báo");
                return;
            }

            // Button y/n
            var rs = MessageBox.Show(
                $"Bạn có chắc muốn xóa phòng '{selectedItem.TenPH}'?",
                "Cảnh báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (rs == DialogResult.Yes)
            {
                bool success = _phongHocController.XoaPhongHoc(maPH);

                if (success)
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Lỗi");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi xóa phòng học: " + ex.Message);
        }
    }

    private void BtnChiTiet_Click(int maPH)
    {
        try
        {
            // Tìm phòng theo MaPH
            var selectedItem = _listPhongHoc.FirstOrDefault(p => p.MaPH == maPH);

            if (selectedItem == null)
            {
                MessageBox.Show("Không tìm thấy phòng học!", "Thông báo");
                return;
            }

            // Mở dialog lịch học
            using (var dialog = new PhongHocScheduleDialog(selectedItem))
            {
                dialog.ShowDialog();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi xem lịch học: " + ex.Message);
        }
    }

    // override
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox.ToList();
    }

    public override void onSearch(string txtSearch, string filter)
    {
        // check null -> gán giá trị
        var f = (filter ?? string.Empty).Trim();
        var txt = txtSearch ?? string.Empty;

        // tất cả -> trả về tất cả
        if (string.IsNullOrWhiteSpace(txt) || string.Equals(f, "Tất cả", StringComparison.OrdinalIgnoreCase))
        {
            _table.UpdateData(_listPhongHoc.Cast<object>().ToList());
            return;
        }

        // lọc khoảng trắng
        var searchTerm = txt.ToLower().Trim();

        // lọc tìm kiếm
        var filteredList = _listPhongHoc.Where(ph =>
        {
            switch (f)
            {
                case "Mã phòng":
                    return ph.MaPH.ToString().Contains(searchTerm);
                case "Tên phòng":
                    return ph.TenPH?.ToLower().Contains(searchTerm) == true;
                case "Loại phòng":
                    return ph.LoaiPH?.ToLower().Contains(searchTerm) == true;
                case "Cơ sở":
                    return ph.CoSo?.ToLower().Contains(searchTerm) == true;
                default:
                    return ph.TenPH?.ToLower().Contains(searchTerm) == true
                           || ph.LoaiPH?.ToLower().Contains(searchTerm) == true
                           || ph.CoSo?.ToLower().Contains(searchTerm) == true
                           || ph.MaPH.ToString().Contains(searchTerm);
            }
        }).ToList();

        _table.UpdateData(filteredList.Cast<object>().ToList());
    }
}