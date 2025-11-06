using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.utils;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class Khoa : NavBase
{
    // public int MaKhoa { get; set; }
    // public string TenKhoa { get; set; }
    // public string Email { get; set; }
    // public string DiaChi { get; set; }
    // variable
    private readonly string[] _listSelectionForComboBox = new[] { "Mã khoa", "Tên khoa" };
    
    private string[] _headerArr = { "Mã khoa", "Tên khoa", "email", "Địa chỉ"};
    private string[] _columnNames = { "MaKhoa", "TenKhoa", "Email", "DiaChi"};

    private readonly string ID = "KHOA";

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;

    private KhoaController _kcontroller;

    private List<ChiTietQuyenDto> _listAccess;
    private Panel _mainBot;
    private SearchBar _searchBar;

    private CustomTable _table;
    private Button btnSua;

    //button
    private TitleButton _insertButton;
    private Button btnXoa;
    private List<KhoaDto> listKhoa;
    private bool sua;
    private bool them;
    private bool xoa;

    private string _title = "Khoa";


    public Khoa(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _kcontroller = new KhoaController();
        Init();
        loadData();
        setActionListener();
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
        if (them) container.Controls.Add(_insertButton);
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

    private void setActionListener()
    {
        if (_table != null)
        {
            _table.OnEdit += maKhoa => BtnSua_Click(maKhoa);
            _table.OnDelete += maKhoa => BtnXoa_Click(maKhoa);
            _table.OnDetail += maKhoa => BtnChiTiet_Click(maKhoa);
        }

        _insertButton._mouseDown += _insertButton_Click;
        _exportExBtn._mouseDown += () => ExportExcel(listKhoa);
    }
    
    void ExportExcel(List<KhoaDto> list)
    {
        var header = new Dictionary<string, string>();
        for (int i = 0; i < _columnNames.Length; i++)
        {
            header.Add(_columnNames[i], _headerArr[i]);
        }
        
        ExcelExporter.ExportToExcel(list, "sheet1","Danh sách khoa", header);
    }

    private new Panel Bottom()
    {
        var mainBot = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = MyColor.GrayBackGround,
            // Padding = new Padding(20, 0, 20, 0)
        };

        return mainBot;
    }

    public void loadData()
    {
        listKhoa = _kcontroller.GetDanhSachKhoa();

        //  == null -> create new table else ...
        if (_table == null)
        {
            var headerArray = new[] { "Mã khoa", "Tên khoa", "email", "Địa chỉ" };
            List<string> headerList = ConvertArray_ListString.ConvertArrayToListString(headerArray);
            var columnNames = new List<string> { "MaKhoa", "TenKhoa", "Email", "DiaChi" };

            var cells = listKhoa.Cast<object>().ToList();
            _table = new CustomTable(headerList, columnNames, cells, sua || xoa, sua, xoa);
            _table.Margin = new Padding(0);
            _mainBot.Controls.Add(_table);
        }
        else
        {
            _table.UpdateData(listKhoa.Cast<object>().ToList());
        }
    }

    // event
    private void _insertButton_Click()
    {
        try
        {
            using (var dialog = new KhoaDialog("Thêm Khoa mới", DialogType.Them))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _kcontroller.ThemKhoa(
                        dialog.TxtTenKhoa.Text.Trim(),
                        dialog.TxtEmail.Text.Trim(),
                        dialog.TxtDiaChi.Text.Trim()
                    );
                    MessageBox.Show("Thêm thành công!", "Thông báo");
                    loadData();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi thêm khoa: " + ex.Message);
        }
    }

    // edit button
    private void BtnSua_Click(int maKhoa)
    {
        try
        {
            // Tìm khoa theo MaKhoa
            var khoa = listKhoa.FirstOrDefault(k => k.MaKhoa == maKhoa);
            if (khoa == null)
            {
                MessageBox.Show("Không tìm thấy khoa!", "Thông báo");
                return;
            }

            using (var dialog = new KhoaDialog("Sửa thông tin Khoa", DialogType.Sua))
            {
                dialog.LoadData(khoa);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _kcontroller.SuaKhoa(
                        maKhoa,
                        dialog.TxtTenKhoa.Text.Trim(),
                        dialog.TxtEmail.Text.Trim(),
                        dialog.TxtDiaChi.Text.Trim()
                    );
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                    loadData();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi sửa khoa: " + ex.Message);
        }
    }

    // del button
    private void BtnXoa_Click(int maKhoa)
    {
        try
        {
            // Tìm khoa theo MaKhoa
            var khoa = listKhoa.FirstOrDefault(k => k.MaKhoa == maKhoa);
            if (khoa == null)
            {
                MessageBox.Show("Không tìm thấy khoa!", "Thông báo");
                return;
            }

            // Button y/n
            var rs = MessageBox.Show(
                $"Bạn chắc chắn muốn xóa khoa {khoa.TenKhoa}?",
                "Cảnh báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (rs == DialogResult.Yes)
            {
                _kcontroller.XoaKhoa(maKhoa);
                loadData();
            }
            else if (rs == DialogResult.No)
            {
                MessageBox.Show("Đã hủy xóa khoa", "Thông báo");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi xóa khoa: " + ex.Message);
        }
    }

    // detail button - xem chi tiết
    private void BtnChiTiet_Click(int maKhoa)
    {
        try
        {
            // Tìm khoa theo MaKhoa
            var khoa = listKhoa.FirstOrDefault(k => k.MaKhoa == maKhoa);
            if (khoa == null)
            {
                MessageBox.Show("Không tìm thấy khoa!", "Thông báo");
                return;
            }

            // Kiểm tra KhoaDialog có tồn tại không
            if (typeof(KhoaDialog) == null)
            {
                MessageBox.Show("KhoaDialog chưa được khởi tạo!", "Lỗi");
                return;
            }

            using (var dialog = new KhoaDialog("Chi tiết Khoa", DialogType.ChiTiet))
            {
                dialog.LoadData(khoa);
                dialog.ShowDialog();
            }
        }
        catch (NullReferenceException ex)
        {
            MessageBox.Show($"Lỗi null reference: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Lỗi");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi xem chi tiết: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Lỗi");
        }
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(_listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
        var f = (filter ?? string.Empty).Trim();
        var txt = (txtSearch ?? string.Empty).Trim();

        //  trả về tất cả
        if (string.IsNullOrWhiteSpace(txt) || string.Equals(f, "Tất cả", StringComparison.OrdinalIgnoreCase))
        {
            if (_table != null && listKhoa != null)
                _table.UpdateData(listKhoa.Cast<object>().ToList());
            return;
        }

        var searchTerm = txt.ToLowerInvariant();
        var filteredList = new List<KhoaDto>();

        if (string.Equals(f, "Tất cả", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(f))
            // Tìm trong tất cả cột
            filteredList = listKhoa.Where(k =>
                k.MaKhoa.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                || (!string.IsNullOrEmpty(k.TenKhoa) &&
                    k.TenKhoa.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(k.Email) &&
                    k.Email.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(k.DiaChi) &&
                    k.DiaChi.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();
        else
            // Lọc theo combobox đã chọn
            filteredList = listKhoa.Where(k =>
            {
                switch (f)
                {
                    case "Mã khoa":
                        return k.MaKhoa.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                    case "Tên khoa":
                        return !string.IsNullOrEmpty(k.TenKhoa) &&
                               k.TenKhoa.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                    default:
                        return k.MaKhoa.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                               || (!string.IsNullOrEmpty(k.TenKhoa) &&
                                   k.TenKhoa.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                               || (!string.IsNullOrEmpty(k.Email) &&
                                   k.Email.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                               || (!string.IsNullOrEmpty(k.DiaChi) &&
                                   k.DiaChi.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            }).ToList();
        // reaload
        if (_table != null)
            _table.UpdateData(filteredList.Cast<object>().ToList());
    }
}