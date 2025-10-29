using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components;

public class ToChucThi : NavBase
{
    private string ID = "TOCHUCTHI";
    private string _title = "Tổ chức thi";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private CaThiController _CaThiController;
    private NhomQuyenController _nhomQuyenController;
    private CaThi_SinhVienController _caThiSinhVienController;
    // public int MaCT { get; set; }
    // public string TenHP { get; set; }
    // public int TenPhong { get; set; }
    // public int Thu { get; set; }
    // public string ThoiGianBatDau { get; set; }
    // public int ThoiLuong { get; set; }
    // public int SiSo { get; set; }
    string[] _headerArray = new string[] { "Mã ca thi", "Học phần", "Phòng", "Thứ", "Thời gian bắt đầu" , "Thời lượng", "Sĩ số"};
    List<string> _headerList;

    private TitleButton _insertButton;

    List<CaThiDto> _rawData;
    List<object> _displayData;

    private CaThiSearch _CaThiSearch;

    private CaThiDialog _CaThiDialog;

    private List<InputFormItem> _listIFI;

    private List<ChiTietQuyenDto> _listAccess;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private HocPhanController _hocPhanController;
    private PhongHocController _phongHocController;

    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    

    public ToChucThi(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<CaThiDto>();
        _displayData = new List<object>();
        _CaThiController = CaThiController.GetInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _caThiSinhVienController = CaThi_SinhVienController.GetInstance();
        Init();
    }

    private void Init()
    {
        CheckQuyen();
        
        Dock = DockStyle.Fill;

        MyTLP mainLayout = new MyTLP
        {
            RowCount = 2,
            Dock = DockStyle.Fill,
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        mainLayout.Controls.Add(Top());
        mainLayout.Controls.Add(Bottom());

        Controls.Add(mainLayout);
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
    private Panel Bottom()
    {
        _panelBottom = new RoundTLP
        {
            Border = true,
            Dock = DockStyle.Fill,
            AutoSize = true,
            RowCount = 2,
            ColumnCount = 2,
            Padding = new Padding(10),
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
        
        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        
        if (them)
        {
            _panelBottom.Controls.Add(_insertButton);
        }

        SetCombobox();

        SetDataTableFromDb();

        SetSearch();

        SetAction();
        
        _panelBottom.Controls.Add(_table);
        _panelBottom.SetColumnSpan(_table, 2);

        return _panelBottom;
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
        _rawData = _CaThiController.GetByHocKyNam(hky, nam);
        
        Console.WriteLine("hky" + hky + "nam" + nam);
        SetDisplayData();
        
        string[] columnNames = new[] { "MaCT", "TenHP", "TenPhong", "Thu", "ThoiGianBatDau", "ThoiLuong", "Siso" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, true, true, true);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                MaCT =  x.MaCT,
                TenHP = x.TenHP,
                TenPhong = x.TenPhong,
                Thu = x.Thu,
                ThoiGianBatDau = x.ThoiGianBatDau,
                ThoiLuong = x.ThoiLuong,
                SiSo = x.SiSo
            }
        );
    }


    void SetSearch()
    {
        _CaThiSearch = new CaThiSearch(ConvertDtoToDisplay(_rawData));
    }

    void SetAction()
    {
        _CaThiSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };

        _hocKyField._combobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeHKNam();
        _namField._namField.ValueChanged += (sender, args) => OnChangeHKNam();

    }

    void OnChangeHKNam()
    {
        string hkyS = _hocKyField.GetSelectionCombobox();
        int hky = int.Parse(hkyS.Split(' ')[2]);

        string nam = _namField.GetTextNam();

        _rawData = _CaThiController.GetByHocKyNam(hky, nam);
        UpdateDataDisplay(ConvertDtoToDisplay(_rawData));
        _table.UpdateData(_displayData);
    }

    void UpdateDataDisplay(List<CaThiDisplay> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaCT =  x.MaCT,
            TenHP = x.TenHP,
            TenPhong = x.TenPhong,
            Thu = x.Thu,
            ThoiGianBatDau = x.ThoiGianBatDau,
            ThoiLuong = x.ThoiLuong,
            SiSo = x.SiSo
        });
    }

    void Insert()
    {
        string hkyS = _hocKyField.GetSelectionCombobox();
        int hky = int.Parse(hkyS.Split(' ')[2]);

        string nam = _namField.GetTextNam(); 
        
        _CaThiDialog = new CaThiDialog("Thêm ca thi", DialogType.Them, hky, nam);
        _CaThiDialog.Finish += () =>
        {
            UpdateDataDisplay(ConvertDtoToDisplay(_CaThiController.GetAll()));
            OnChangeHKNam();
            this._table.UpdateData(_displayData);
        };
        this._CaThiDialog.ShowDialog();
    }

    void Update(int id)
    {
        string hkyS = _hocKyField.GetSelectionCombobox();
        int hky = int.Parse(hkyS.Split(' ')[2]);

        string nam = _namField.GetTextNam(); 
        _CaThiDialog = new CaThiDialog("Sửa ca thi", DialogType.Sua,hky, nam, id);
        _CaThiDialog.Finish += () =>
        {
            UpdateDataDisplay(ConvertDtoToDisplay(_CaThiController.GetAll()));
            OnChangeHKNam();
            this._table.UpdateData(_displayData);
        };
        this._CaThiDialog.ShowDialog();
    }

    void Detail(int id)
    {
        string hkyS = _hocKyField.GetSelectionCombobox();
        int hky = int.Parse(hkyS.Split(' ')[2]);

        string nam = _namField.GetTextNam(); 
        _CaThiDialog = new CaThiDialog("Chi tiết ca thi", DialogType.ChiTiet,hky, nam, id);
        this._CaThiDialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        if (_CaThiController.Delete(index))
        {
            MessageBox.Show("Xóa ca thi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(ConvertDtoToDisplay(_CaThiController.GetAll()));
            OnChangeHKNam();
            this._table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa ca thi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    List<CaThiDisplay> ConvertDtoToDisplay(List<CaThiDto> input)
    {
        List<CaThiDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new CaThiDisplay
        {
            MaCT =  x.MaCT,
            TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            TenPhong = _phongHocController.GetPhongHocById(x.MaPH).TenPH,
            Thu = x.Thu,
            ThoiGianBatDau = x.ThoiGianBatDau,
            ThoiLuong = x.ThoiLuong,
            SiSo = _caThiSinhVienController.GetByMaCT(x.MaCT).Count
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
        this._CaThiSearch.Search(txtSearch, filter);
    }
}