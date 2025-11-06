using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.utils;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class HocPhan : NavBase
{
    private readonly string[] _headerArray = new[]
        { "Mã HP", "Mã HP Trước", "Tên HP", "Số Tín Chỉ", "Hệ Số", "Số Tiết LT", "Số Tiết TH" };

    private readonly string _title = "Học phần";

    private readonly HocPhanDao hocPhanDAO = HocPhanDao.GetInstance();

    private readonly string ID = "HOCPHAN";
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;
    private List<string> _headerList;

    private HocPhanSearch _hocPhanSearch;

    private TitleButton _insertButton;

    private List<ChiTietQuyenDto> _listAccess;
    private List<string> _listSelectionForComboBox;
    private List<HocPhanDto> _rawData;

    private CustomTable _table;
    private Panel _tableContainer;
    private bool sua;
    private bool them;
    private bool xoa;

    public HocPhan(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<HocPhanDto>();
        _displayData = new List<object>();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        Init();
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

        mainLayout.Controls.Add(Top());
        mainLayout.Controls.Add(Bottom());

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
    private Panel Top()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
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

    private Panel Bottom()
    {
        var mainBot = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
        };

        _tableContainer = new Panel
        {
            Dock = DockStyle.Fill
        };

        SetCombobox();
        SetDataTableFromDb();
        SetSearch();
        SetAction();

        mainBot.Controls.Add(_tableContainer);
        _tableContainer.Controls.Add(_table);
        return mainBot;
    }

    private void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }

    private List<string> columnNames;
    private void SetDataTableFromDb()
    {
        try
        {
            _rawData = hocPhanDAO.GetAll() ?? new List<HocPhanDto>();
        }
        catch (Exception ex)
        {
            _rawData = new List<HocPhanDto>();
            MessageBox.Show($"Lỗi khi tải danh sách học phần: {ex.Message}");
        }

        SetDisplayData();

        columnNames = new List<string>
            { "MaHP", "MaHPTruoc", "TenHP", "SoTinChi", "HeSoHocPhan", "SoTietLyThuyet", "SoTietThucHanh" };

        _table = new CustomTable(_headerList, columnNames, _displayData, sua || xoa, sua, xoa)
        {
            Margin = new Padding(0),
        };
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
        {
            x.MaHP,
            x.MaHPTruoc,
            x.TenHP,
            x.SoTinChi,
            x.HeSoHocPhan,
            x.SoTietLyThuyet,
            x.SoTietThucHanh
        });
    }

    private void SetSearch()
    {
        _hocPhanSearch = new HocPhanSearch(_rawData);
    }

    private void SetAction()
    {
        _hocPhanSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += id => { Update(id); };
        _table.OnDetail += id => { Detail(id); };
        _table.OnDelete += id => { Delete(id); };

        _exportExBtn._mouseDown += () => ExportExcel(_rawData);
    }
    
    void ExportExcel(List<HocPhanDto> list)
    {
        var header = new Dictionary<string, string>();
        for (int i = 0; i < _headerArray.Length; i++)
        {
            header.Add(columnNames[i], _headerArray[i]);
        }
        
        ExcelExporter.ExportToExcel(list, "sheet1","Danh sách lớp", header);
    }

    private void UpdateDataDisplay(List<HocPhanDto> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaHP,
            x.MaHPTruoc,
            x.TenHP,
            x.SoTinChi,
            x.HeSoHocPhan,
            x.SoTietLyThuyet,
            x.SoTietThucHanh
        });
    }

    private void Insert()
    {
        using (var dialog = new HocPhanDialog(DialogType.Them, null, hocPhanDAO))
        {
            dialog.Finish += () =>
            {
                UpdateDataDisplay(hocPhanDAO.GetAll());
                _table.UpdateData(_displayData);
            };
            dialog.ShowDialog();
        }
    }

    private void Update(int id)
    {
        var hocPhan = _rawData.FirstOrDefault(h => h.MaHP == id);
        if (hocPhan != null)
            using (var dialog = new HocPhanDialog(DialogType.Sua, hocPhan, hocPhanDAO))
            {
                dialog.Finish += () =>
                {
                    UpdateDataDisplay(hocPhanDAO.GetAll());
                    _table.UpdateData(_displayData);
                };
                dialog.ShowDialog();
            }
    }

    private void Detail(int id)
    {
        var hocPhan = _rawData.FirstOrDefault(h => h.MaHP == id);
        if (hocPhan != null)
            using (var dialog = new HocPhanDialog(DialogType.ChiTiet, hocPhan, hocPhanDAO))
            {
                dialog.ShowDialog();
            }
    }

    private void Delete(int id)
    {
        var hocPhan = _rawData.FirstOrDefault(h => h.MaHP == id);
        if (hocPhan != null)
        {
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa học phần '{hocPhan.TenHP}'?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
                try
                {
                    hocPhanDAO.Delete(hocPhan.MaHP);
                    UpdateDataDisplay(hocPhanDAO.GetAll());
                    _table.UpdateData(_displayData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa học phần: {ex.Message}");
                }
        }
    }


    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _hocPhanSearch.Search(txtSearch, filter);
    }
}