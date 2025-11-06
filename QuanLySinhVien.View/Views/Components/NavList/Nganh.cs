using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.utils;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.NavList;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.View.Views.Components;

public class NganhPanel : NavBase
{
    private readonly string[] _headerArray = new[] { "Mã ngành", "Tên ngành", "Tên khoa" };

    private readonly string ID = "NGANH";
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;
    private List<string> _headerList;

    private TitleButton _insertButton;
    private KhoaController _khoaController;

    private List<ChiTietQuyenDto> _listAccess;
    private List<string> _listSelectionForComboBox;

    private NganhController _nganhController;

    private NganhDialog _nganhDialog;

    private NganhSearch _nganhSearch;

    private List<NganhDto> _rawData;


    private CustomTable _table;
    private string _title = "Ngành";
    private bool sua;
    private bool them;
    private bool xoa;


    public NganhPanel(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<NganhDto>();
        _displayData = new List<object>();
        _nganhController = NganhController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        Init();
    }


    private void Init()
    {
        CheckQuyen();
        Dock = DockStyle.Fill;

        var mainLayout = new TableLayoutPanel
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
        var panel = new TableLayoutPanel
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

    private Panel Bottom()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
        };

        SetCombobox();

        SetDataTableFromDb();

        SetSearch();

        SetAction();

        panel.Controls.Add(_table);

        return panel;
    }

    private Label getTitle()
    {
        var titlePnl = new Label
        {
            Text = "Ngành",
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true
        };
        return titlePnl;
    }

    private void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }

    private List<string> columnNamesList;
    private void SetDataTableFromDb()
    {
        _rawData = _nganhController.GetAll();
        SetDisplayData();

        var columnNames = new[] { "MaNganh", "TenNganh", "TenKhoa" };
        columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa)
        {
            Margin = new Padding(0),
        };
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, dto => new
        {
            dto.MaNganh,
            dto.TenNganh,
            _khoaController.GetKhoaById(dto.MaKhoa).TenKhoa
        });
    }

    private void SetSearch()
    {
        _nganhSearch = new NganhSearch(_rawData);
    }

    private void SetAction()
    {
        _nganhSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
        _exportExBtn._mouseDown += () => ExportExcel(_rawData);
    }
    
    void ExportExcel(List<NganhDto> list)
    {
        var header = new Dictionary<string, string>();
        for (int i = 0; i < _headerArray.Length; i++)
        {
            header.Add(columnNamesList[i], _headerArray[i]);
        }
        
        ExcelExporter.ExportToExcel(list, "sheet1","Danh sách ngành", header);
    }

    private void UpdateDataDisplay(List<NganhDto> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaNganh,
            x.TenNganh,
            _khoaController.GetKhoaById(x.MaKhoa).TenKhoa
        });
    }

    private void Insert()
    {
        _nganhDialog = new NganhDialog(DialogType.Them, new NganhDto(), NganhDao.GetInstance());
        _nganhDialog.Finish += () =>
        {
            UpdateDataDisplay(_nganhController.GetAll());
            _table.UpdateData(_displayData);
        };
        _nganhDialog.ShowDialog();
    }

    private void Update(int id)
    {
        var nganh = _nganhController.GetNganhById(id);
        _nganhDialog = new NganhDialog(DialogType.Sua, nganh, NganhDao.GetInstance());

        _nganhDialog.Finish += () =>
        {
            UpdateDataDisplay(_nganhController.GetAll());
            _table.UpdateData(_displayData);
        };
        _nganhDialog.ShowDialog();
    }

    private void Detail(int id)
    {
        var nganh = _nganhController.GetNganhById(id);
        _nganhDialog = new NganhDialog(DialogType.ChiTiet, nganh, NganhDao.GetInstance());

        _nganhDialog.ShowDialog();
    }

    private void Delete(int id)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        if (_nganhController.Delete(id))
        {
            MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            UpdateDataDisplay(_nganhController.GetAll());
            _table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _nganhSearch.Search(txtSearch, filter);
    }
}