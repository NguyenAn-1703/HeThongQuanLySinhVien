using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class NhapDiemThi : NavBase
{
    private readonly string[] _headerArray = new[]
    {
        "Mã ca thi",
        "Tên HP",
        "Ngày thi",
        "Phòng"
    };

    private readonly string _title = "Nhập điểm thi";
    private readonly string ID = "NhapDiemThi";
    private CaThiController _caThiController;

    private CaThiNhapDiemSearch _caThiNhapDiemSearch;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;

    private GiangVienController _giangVienController;

    private List<string> _headerList;

    private LabelTextField _hocKyField;
    private HocPhanController _hocPhanController;


    private List<ChiTietQuyenDto> _listAccess;
    private List<string> _listSelectionForComboBox;

    private MyTLP _mainLayout;
    private LabelTextField _namField;
    private NhomQuyenController _nhomQuyenController;

    private MyTLP _panelBottom;


    private MyTLP _panelTop;

    private PhongHocController _phongHocController;
    // private TitleButton _insertButton;

    private List<CaThiDto> _rawData;
    private MyTLP _screen;
    private CustomTable _table;
    private bool sua;

    private bool them;
    private bool xoa;


    public NhapDiemThi(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<CaThiDto>();
        _displayData = new List<object>();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _giangVienController = GiangVienController.GetInstance();
        _caThiController = CaThiController.GetInstance();
        Init();
    }

    private void Init()
    {
        CheckQuyen();

        Dock = DockStyle.Fill;

        _mainLayout = new MyTLP
        {
            RowCount = 2,
            Dock = DockStyle.Fill
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _mainLayout.Controls.Add(Top());
        SetBottom();


        Controls.Add(_mainLayout);
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

    private Panel Top()
    {
        _panelTop = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Padding = new Padding(10, 7, 10, 0),
            ColumnCount = 3,
            BackColor = MyColor.GrayBackGround
        };

        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _panelTop.Controls.Add(getTitle());
        SetHKyNamContainer();

        return _panelTop;
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

    private void SetBottom()
    {
        _screen = new MyTLP { Margin = new Padding(0), Dock = DockStyle.Fill };
        _panelBottom = new RoundTLP
        {
            Border = true,
            Dock = DockStyle.Fill,
            AutoSize = true,
            RowCount = 2,
            ColumnCount = 2,
            Padding = new Padding(10),
            Margin = new Padding(0)
        };

        _panelBottom.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _panelBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var lblNhomHocPhan = new Label
        {
            Margin = new Padding(5),
            AutoSize = true,
            Text = "Ca thi",
            Font = GetFont.GetFont.GetMainFont(13, FontType.Black)
        };

        _panelBottom.Controls.Add(lblNhomHocPhan);


        SetCombobox();

        SetDataTableFromDb();

        SetSearch();

        SetAction();

        _panelBottom.Controls.Add(_table);
        _panelBottom.SetColumnSpan(_table, 2);

        _screen.Controls.Add(_panelBottom);
        _mainLayout.Controls.Add(_screen);
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


    //////////////////////////////SETTOP///////////////////////////////


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    private void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }


    private void SetDataTableFromDb()
    {
        var hkyS = _hocKyField.GetSelectionCombobox();
        var hky = int.Parse(hkyS.Split(' ')[2]);
        var nam = DateTime.Now.ToString("yyyy");

        // Lấy ca thi theo hky nam
        List<CaThiDto> listCaThi = _caThiController.GetByHocKyNam(hky, nam);
        _rawData = listCaThi;


        SetDisplayData();

        var columnNames = new[] { "MaCT", "TenHP", "NgayThi", "TenPhong" };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_caThiController.ConvertDtoToDisplay(_rawData), x => new
            {
                x.MaCT,
                x.TenHP,
                x.NgayThi,
                x.TenPhong
            }
        );
    }


    private void SetSearch()
    {
        _caThiNhapDiemSearch = new CaThiNhapDiemSearch(_caThiController.ConvertDtoToDisplay(_rawData));
    }

    private void SetAction()
    {
        _caThiNhapDiemSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };


        _table.OnDetail += index => { ChangePanel(index); };

        _hocKyField._combobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeHKNam();
        _namField._namField.ValueChanged += (sender, args) => OnChangeHKNam();
    }

    private void OnChangeHKNam()
    {
        var hkyS = _hocKyField.GetSelectionCombobox();
        var hky = int.Parse(hkyS.Split(' ')[2]);

        var nam = _namField.GetTextNam();

        List<CaThiDto> listCaThi = _caThiController.GetByHocKyNam(hky, nam);
        _rawData = listCaThi;

        UpdateDataDisplay(_caThiController.ConvertDtoToDisplay(_rawData));
        _table.UpdateData(_displayData);
    }

    private void UpdateDataDisplay(List<CaThiNhapDiemDisplay> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaCT,
            x.TenHP,
            x.NgayThi,
            x.TenPhong
        });
    }

    private void ChangePanel(int id)
    {
        _screen.SuspendLayout();
        _screen.Controls.Clear();
        var NhapDiemThiDialog = new NhapDiemThiDialog("Nhập điểm", id);

        NhapDiemThiDialog.Back += () => ResetPanel();
        _screen.Controls.Add(NhapDiemThiDialog);
        _screen.ResumeLayout();

        _namField.Enabled = false;
        _hocKyField.Enabled = false;
    }

    private void ResetPanel()
    {
        _screen.Controls.Clear();
        _screen.Controls.Add(_panelBottom);
        _namField.Enabled = true;
        _hocKyField.Enabled = true;
    }

    private void Delete(int index)
    {
    }




    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _caThiNhapDiemSearch.Search(txtSearch, filter, _caThiController.ConvertDtoToDisplay(_rawData));
    }
}