using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components;

public class NhapDiem : NavBase
{
    private readonly string[] _headerArray = new[]
    {
        "Mã nhóm HP",
        "Tên HP",
        "Sĩ số",
        "Giảng viên"
    };

    private readonly string _title = "Nhập điểm";
    private readonly string ID = "NHAPDIEM";
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
    private NhomHocPhanController _NhomHocPhanController;

    private NhomHocPhanSearch _NhomHocPhanSearch;
    private NhomQuyenController _nhomQuyenController;

    private MyTLP _panelBottom;


    private MyTLP _panelTop;

    private PhongHocController _phongHocController;
    // private TitleButton _insertButton;

    private List<NhomHocPhanDto> _rawData;
    private MyTLP _screen;
    private CustomTable _table;
    private bool sua;

    private bool them;
    private bool xoa;


    public NhapDiem(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<NhomHocPhanDto>();
        _displayData = new List<object>();
        _NhomHocPhanController = NhomHocPhanController.GetInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _giangVienController = GiangVienController.GetInstance();
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
            Text = "Nhóm học phần",
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

        // lấy ds nhóm học phần theo học kỳ và năm, theo giảng viên, nếu admin thì lấy toàn bộ giảng viên
        if (_quyen.TenNhomQuyen.Equals("admin"))
        {
            _rawData = _NhomHocPhanController.GetByHkyNam(hky, nam);
        }
        else
        {
            GiangVienDto GV = _giangVienController.GetByMaTK(_taiKhoan.MaTK);
            _rawData = _NhomHocPhanController.GetByHkyNamMaGV(hky, nam, GV.MaGV);
        }

        SetDisplayData();

        var columnNames = new[] { "MaNHP", "TenHP", "Siso", "TenGiangVien" };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                x.MaNHP,
                x.TenHP,
                x.Siso,
                x.TenGiangVien
            }
        );
    }


    private void SetSearch()
    {
        _NhomHocPhanSearch = new NhomHocPhanSearch(ConvertDtoToDisplay(_rawData));
    }

    private void SetAction()
    {
        _NhomHocPhanSearch.FinishSearch += dtos =>
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

        // lấy ds nhóm học phần theo học kỳ và năm, theo giảng viên, nếu admin thì lấy toàn bộ giảng viên
        if (_quyen.TenNhomQuyen.Equals("admin"))
        {
            _rawData = _NhomHocPhanController.GetByHkyNam(hky, nam);
        }
        else
        {
            GiangVienDto GV = _giangVienController.GetByMaTK(_taiKhoan.MaTK);
            _rawData = _NhomHocPhanController.GetByHkyNamMaGV(hky, nam, GV.MaGV);
        }

        UpdateDataDisplay(ConvertDtoToDisplay(_rawData));
        _table.UpdateData(_displayData);
    }

    private void UpdateDataDisplay(List<NhomHocPhanDisplay> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaNHP,
            x.TenHP,
            x.Siso,
            x.TenGiangVien
        });
    }

    private void ChangePanel(int id)
    {
        _screen.SuspendLayout();
        _screen.Controls.Clear();
        var nhapDiemDialog = new NhapDiemDialog("Nhập điểm", id);

        nhapDiemDialog.Back += () => ResetPanel();
        _screen.Controls.Add(nhapDiemDialog);
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

    private List<NhomHocPhanDisplay> ConvertDtoToDisplay(List<NhomHocPhanDto> input)
    {
        List<NhomHocPhanDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new NhomHocPhanDisplay
        {
            MaNHP = x.MaNHP,
            TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            Siso = x.SiSo,
            TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV
        });
        return rs;
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _NhomHocPhanSearch.Search(txtSearch, filter, ConvertDtoToDisplay(_rawData));
    }
}