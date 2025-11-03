using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.Views.Components.NavList;

public class HocPhi : NavBase
{
    private readonly string[] _headerArray = new[] { "Mã sinh viên", "Tên sinh viên", "Khoa", "Ngành", "Trạng thái" };
    private readonly string _title = "Học phí";
    private readonly string ID = "SVHocPhi";
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;
    private List<string> _headerList;

    private LabelTextField _hocKyField;

    private TitleButton _insertButton;
    private KhoaController _khoaController;

    private List<ChiTietQuyenDto> _listAccess;


    private List<InputFormItem> _listIFI;
    private List<string> _listSelectionForComboBox;
    private LopController _lopController;
    private LabelTextField _namField;
    private NganhController _nganhController;
    private NhomQuyenController _nhomQuyenController;

    private MyTLP _panelTop;

    private List<SinhVienDTO> _rawData;
    private SinhVienController _sinhVienController;

    private SVHocPhiSearch _SVHocPhiSearch;
    private CustomTable _table;

    private MyTLP mainLayout;
    private bool sua;

    private bool them;
    private bool xoa;


    public HocPhi(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<SinhVienDTO>();
        _displayData = new List<object>();
        _sinhVienController = SinhVienController.GetInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _lopController = LopController.GetInstance();
        _nganhController = NganhController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        Init();
    }

    private void Init()
    {
        CheckQuyen();

        Dock = DockStyle.Fill;

        mainLayout = new MyTLP
        {
            RowCount = 2,
            Dock = DockStyle.Fill
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        SetTop();
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

    private void SetTop()
    {
        _panelTop = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            // CellBorderStyle = MyTLPCellBorderStyle.Single,
            Padding = new Padding(10),
            ColumnCount = 3,
            BackColor = MyColor.GrayBackGround
        };

        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _panelTop.Controls.Add(getTitle());

        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        if (them) _panelTop.Controls.Add(_insertButton);
        mainLayout.Controls.Add(_panelTop);
        SetHKyNamContainer();
    }

    private Panel Bottom()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill
        };

        SetCombobox();

        SetDataTableFromDb();

        SetSearch();

        SetAction();
        SetTableHP();

        panel.Controls.Add(_table);

        return panel;
    }

    //////////////////////////////SETTOP///////////////////////////////
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

    private void SetHKyNamContainer()
    {
        var panel = new MyTLP
        {
            ColumnCount = 2,
            AutoSize = true
        };

        _hocKyField = new LabelTextField("Học kỳ", TextFieldType.Combobox);
        _hocKyField._combobox.Font = GetFont.GetFont.GetMainFont(10, FontType.Regular);
        var listHK = new[] { "Học kỳ 1", "Học kỳ 2" };
        _hocKyField.SetComboboxList(listHK.ToList());
        _hocKyField.SetComboboxSelection("Học kỳ 1");

        _namField = new LabelTextField("Năm", TextFieldType.Year);
        _namField.Font = GetFont.GetFont.GetMainFont(14, FontType.Regular);
        _namField._namField.Value = DateTime.Now;


        panel.Controls.Add(_hocKyField);
        panel.Controls.Add(_namField);

        _panelTop.Controls.Add(panel);
    }


    //////////////////////////////SETTOP///////////////////////////////


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    private void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }


    private void SetDataTableFromDb()
    {
        _rawData = _sinhVienController.GetAll();
        SetDisplayData();

        var columnNames = new[] { "MaSV", "TenSV", "Khoa", "Nganh", "TrangThaiHP" };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                x.MaSV,
                x.TenSV,
                x.Khoa,
                x.Nganh,
                x.TrangThaiHP
            }
        );
    }


    private void SetSearch()
    {
        _SVHocPhiSearch = new SVHocPhiSearch(ConvertDtoToDisplay(_rawData));
    }

    private void SetAction()
    {
        _SVHocPhiSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };

        _hocKyField._combobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeHkNam();
        _namField._namField.ValueChanged += (sender, args) => OnChangeHkNam();

        _table.MouseClickBtnCapNhat += i => UpdateHocPhi(i);
    }

    private void UpdateDataDisplay(List<SVHocPhiDisplay> input)
    {
        _displayData = ConvertObject.ConvertToDisplay(input, x => new
        {
            x.MaSV,
            x.TenSV,
            x.Khoa,
            x.Nganh,
            x.TrangThaiHP
        });
    }

    private void UpdateHocPhi(int maSV)
    {
        var hky = int.Parse(_hocKyField.GetSelectionCombobox().Split(' ')[2]);
        var nam = _namField._namField.Value.ToString("yyyy");
        var hocPhiDialog = new HocPhiDialog("Cập nhật học phí", DialogType.Sua, hky, nam, maSV);
        hocPhiDialog.ShowDialog();
        UpdateTrangThaiHP();
    }

    private void Detail(int maSV)
    {
        var hky = int.Parse(_hocKyField.GetSelectionCombobox().Split(' ')[2]);
        var nam = _namField._namField.Value.ToString("yyyy");
        var hocPhiDialog = new HocPhiDialog("Cập nhật học phí", DialogType.ChiTiet, hky, nam, maSV);
        hocPhiDialog.ShowDialog();
    }

    private void Delete(int index)
    {
    }

    private List<SVHocPhiDisplay> ConvertDtoToDisplay(List<SinhVienDTO> input)
    {
        var hky = int.Parse(_hocKyField.GetSelectionCombobox().Split(' ')[2]);
        var nam = _namField._namField.Value.ToString("yyyy");
        List<SVHocPhiDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new SVHocPhiDisplay
        {
            MaSV = x.MaSinhVien,
            TenSV = x.TenSinhVien,
            Khoa = _sinhVienController.GetTenKhoa(x.MaSinhVien),
            Nganh = _sinhVienController.GetTenNganh(x.MaSinhVien),
            TrangThaiHP = _sinhVienController.GetTrangThaiHocPhi(x.MaSinhVien, hky, nam)
        });
        return rs;
    }

    private void OnChangeHkNam()
    {
        UpdateTrangThaiHP();
    }

    private void UpdateTrangThaiHP()
    {
        UpdateDataDisplay(ConvertDtoToDisplay(_rawData));
        _table.UpdateData(_displayData);
    }

    private void SetTableHP()
    {
        _table.AddColumn(ColumnType.Button, "Hành động");
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _SVHocPhiSearch.Search(txtSearch, filter);
    }
}