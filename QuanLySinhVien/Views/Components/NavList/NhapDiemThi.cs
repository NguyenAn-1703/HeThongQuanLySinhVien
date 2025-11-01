using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList;

public class NhapDiemThi : NavBase
{
    private string ID = "NhapDiemThi";
    private string _title = "Nhập điểm thi";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private NhomQuyenController _nhomQuyenController;
    string[] _headerArray = new string[]
    {
        "Mã ca thi",
        "Tên HP",
        "Ngày thi",
        "Phòng",
    };
    List<string> _headerList;
    // private TitleButton _insertButton;

    List<CaThiDto> _rawData;
    List<object> _displayData;

    
    private List<ChiTietQuyenDto> _listAccess;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private HocPhanController _hocPhanController;
    private PhongHocController _phongHocController;
    
    private GiangVienController _giangVienController;
    private CaThiController _caThiController;
    
    private CaThiNhapDiemSearch _caThiNhapDiemSearch;

    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    

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
        _caThiController =  CaThiController.GetInstance();
        Init();
    }

    private MyTLP _mainLayout;
    private void Init()
    {
        CheckQuyen();
        
        Dock = DockStyle.Fill;

        _mainLayout = new MyTLP
        {
            RowCount = 2,
            Dock = DockStyle.Fill,
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _mainLayout.Controls.Add(Top());
        SetBottom();
        

        Controls.Add(_mainLayout);
    }

    void CheckQuyen()
    {
        int maCN = _chucNangController.GetByTen(ID).MaCN;
        _listAccess = _chiTietQuyenController.GetByMaNQMaCN(_quyen.MaNQ, maCN);
        foreach (ChiTietQuyenDto x in _listAccess)
        {
            Console.WriteLine(x.HanhDong);
        }

        if (_listAccess.Any(x => x.HanhDong.Equals("Them")))
        {
            them = true;
        }
        if (_listAccess.Any(x => x.HanhDong.Equals("Sua")))
        {
            sua = true;
        }
        if (_listAccess.Any(x => x.HanhDong.Equals("Xoa")))
        {
            xoa = true;
        }
        
    }


    private MyTLP _panelTop;
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

    private LabelTextField _hocKyField;
    private LabelTextField _namField;
    private void SetHKyNamContainer()
    {
        MyTLP panel = new MyTLP
        {
            ColumnCount = 2,
            AutoSize = true,
        };

        _hocKyField = new LabelTextField("Học kỳ", TextFieldType.Combobox);
        _hocKyField._combobox.Font = GetFont.GetFont.GetMainFont(10, FontType.Regular);
        string[] listHK = new[] { "Học kỳ 1", "Học kỳ 2" };
        _hocKyField.SetComboboxList(listHK.ToList());
        _hocKyField.SetComboboxSelection("Học kỳ 1");
        
        _namField = new LabelTextField("Năm", TextFieldType.Year);
        _namField.Font =  GetFont.GetFont.GetMainFont(14, FontType.Regular);
        _namField._namField.Value = DateTime.Now;

        
        panel.Controls.Add(_hocKyField);
        panel.Controls.Add(_namField);
        
        _panelTop.Controls.Add(panel);
    }

    MyTLP _panelBottom;
    private MyTLP _screen;
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
            Margin = new Padding(0),
        };

        _panelBottom.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _panelBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        Label lblNhomHocPhan = new Label
        {
            Margin = new Padding(5),
            AutoSize = true,
            Text="Ca thi",
            Font = GetFont.GetFont.GetMainFont(13, FontType.Black),
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
    Label getTitle()
    {
        Label titlePnl = new Label
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
    void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }


    void SetDataTableFromDb()
    {
        string hkyS = _hocKyField.GetSelectionCombobox();
        int hky = int.Parse(hkyS.Split(' ')[2]);
        string nam = DateTime.Now.ToString("yyyy");
        
        // Lấy ca thi theo hky nam
        List<CaThiDto> listCaThi = _caThiController.GetByHocKyNam(hky, nam);
        _rawData = listCaThi;
        
        
        SetDisplayData();
        
        string[] columnNames = new[] { "MaCT", "TenHP", "NgayThi", "TenPhong"};
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, false);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                MaCT = x.MaCT,
                TenHP = x.TenHP,
                NgayThi = x.NgayThi,
                TenPhong = x.TenPhong,
            }
        );
    }


    void SetSearch()
    {
        _caThiNhapDiemSearch = new CaThiNhapDiemSearch(ConvertDtoToDisplay(_rawData));
    }

    void SetAction()
    {
        _caThiNhapDiemSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        
        _table.OnDetail += index => { ChangePanel(index); };

        _hocKyField._combobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeHKNam();
        _namField._namField.ValueChanged += (sender, args) => OnChangeHKNam();
    }

    void OnChangeHKNam()
    {
        string hkyS = _hocKyField.GetSelectionCombobox();
        int hky = int.Parse(hkyS.Split(' ')[2]);

        string nam = _namField.GetTextNam();
        
        List<CaThiDto> listCaThi = _caThiController.GetByHocKyNam(hky, nam);
        _rawData = listCaThi;

        UpdateDataDisplay(ConvertDtoToDisplay(_rawData));
        _table.UpdateData(_displayData);
    }

    void UpdateDataDisplay(List<CaThiNhapDiemDisplay> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaCT = x.MaCT,
            TenHP = x.TenHP,
            NgayThi = x.NgayThi,
            TenPhong = x.TenPhong,
        });
    }
    
    void ChangePanel(int id)
    {
        _screen.SuspendLayout();
        _screen.Controls.Clear();
        NhapDiemThiDialog NhapDiemThiDialog = new NhapDiemThiDialog("Nhập điểm", id);
        
        NhapDiemThiDialog.Back += () => ResetPanel();
        _screen.Controls.Add(NhapDiemThiDialog);
        _screen.ResumeLayout();

        _namField.Enabled = false;
        _hocKyField.Enabled = false;
    }

    void ResetPanel()
    {
        _screen.Controls.Clear();
        _screen.Controls.Add(_panelBottom);
        _namField.Enabled = true;
        _hocKyField.Enabled = true;
    }

    void Delete(int index)
    {
        
    }
    
    List<CaThiNhapDiemDisplay> ConvertDtoToDisplay(List<CaThiDto> input)
    {
        List<CaThiNhapDiemDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new CaThiNhapDiemDisplay
        {
            MaCT = x.MaCT,
            TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            NgayThi = x.ThoiGianBatDau,
            TenPhong = _phongHocController.GetPhongHocById(x.MaPH).TenPH,
        });
        return rs;
    }
    

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._caThiNhapDiemSearch.Search(txtSearch, filter, ConvertDtoToDisplay(_rawData));
    }
}