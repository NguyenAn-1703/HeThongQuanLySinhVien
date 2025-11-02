using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList;

public class HocPhi : NavBase
{
    private string ID = "SVHocPhi";
    private string _title = "Học phí";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private SinhVienController _sinhVienController;
    private NhomQuyenController _nhomQuyenController;
    string[] _headerArray = new string[] { "Mã sinh viên", "Tên sinh viên", "Khoa", "Ngành", "Trạng thái" };
    List<string> _headerList;

    private TitleButton _insertButton;

    List<SinhVienDTO> _rawData;
    List<object> _displayData;

    private SVHocPhiSearch _SVHocPhiSearch;


    private List<InputFormItem> _listIFI;

    private List<ChiTietQuyenDto> _listAccess;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private LopController _lopController;
    private NganhController _nganhController;
    private KhoaController _khoaController;

    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    

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

    private MyTLP mainLayout;
    private void Init()
    {
        CheckQuyen();
        
        Dock = DockStyle.Fill;

        mainLayout = new MyTLP
        {
            RowCount = 2,
            Dock = DockStyle.Fill,
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        SetTop();
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
        if (them)
        {
            _panelTop.Controls.Add(_insertButton);
        }
        mainLayout.Controls.Add(_panelTop);
        SetHKyNamContainer();
    }

    private Panel Bottom()
    {
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
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


    //////////////////////////////SETTOP///////////////////////////////


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }


    void SetDataTableFromDb()
    {
        _rawData = _sinhVienController.GetAll();
        SetDisplayData();

        string[] columnNames = new[] { "MaSV", "TenSV", "Khoa", "Nganh","TrangThaiHP" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                MaSV = x.MaSV,
                TenSV = x.TenSV,
                Khoa = x.Khoa,
                Nganh = x.Nganh,
                TrangThaiHP = x.TrangThaiHP,
            }
        );
    }


    void SetSearch()
    {
        _SVHocPhiSearch = new SVHocPhiSearch(ConvertDtoToDisplay(_rawData));
    }

    void SetAction()
    {
        _SVHocPhiSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
        
        _hocKyField._combobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeHkNam();
        _namField._namField.ValueChanged += (sender, args) => OnChangeHkNam();

        _table.MouseClickBtnCapNhat += i => UpdateHocPhi(i);
    }

    void UpdateDataDisplay(List<SVHocPhiDisplay> input)
    {
        this._displayData = ConvertObject.ConvertToDisplay(input, x => new
        {
            MaSV = x.MaSV,
            TenSV = x.TenSV,
            Khoa = x.Khoa,
            Nganh = x.Nganh,
            TrangThaiHP = x.TrangThaiHP,
        });
    }
    
    void UpdateHocPhi(int maSV)
    {
        int hky = int.Parse(_hocKyField.GetSelectionCombobox().Split(' ')[2]);
        string nam =  _namField._namField.Value.ToString("yyyy");
        HocPhiDialog hocPhiDialog = new HocPhiDialog("Cập nhật học phí", DialogType.Sua, hky, nam , maSV);
        hocPhiDialog.ShowDialog();
        UpdateTrangThaiHP();
    }
    
    void Detail(int maSV)
    {
        int hky = int.Parse(_hocKyField.GetSelectionCombobox().Split(' ')[2]);
        string nam =  _namField._namField.Value.ToString("yyyy");
        HocPhiDialog hocPhiDialog = new HocPhiDialog("Cập nhật học phí", DialogType.ChiTiet, hky, nam , maSV);
        hocPhiDialog.ShowDialog();
    }

    void Delete(int index)
    {

    }

    List<SVHocPhiDisplay> ConvertDtoToDisplay(List<SinhVienDTO> input)
    {
        int hky = int.Parse(_hocKyField.GetSelectionCombobox().Split(' ')[2]);
        string nam =  _namField._namField.Value.ToString("yyyy");
        List<SVHocPhiDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new SVHocPhiDisplay
        {
            MaSV = x.MaSinhVien,
            TenSV = x.TenSinhVien,
            Khoa = _sinhVienController.GetTenKhoa(x.MaSinhVien),
            Nganh = _sinhVienController.GetTenNganh(x.MaSinhVien),
            TrangThaiHP = _sinhVienController.GetTrangThaiHocPhi(x.MaSinhVien, hky, nam),
        });
        return rs;
    }

    void OnChangeHkNam()
    {
        UpdateTrangThaiHP();
    }
    
    void UpdateTrangThaiHP()
    {
        UpdateDataDisplay(ConvertDtoToDisplay(_rawData));
        _table.UpdateData(_displayData);
    }

    void SetTableHP()
    {
        _table.AddColumn(ColumnType.Button, "Hành động");
    }
    

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._SVHocPhiSearch.Search(txtSearch, filter);
    }
}