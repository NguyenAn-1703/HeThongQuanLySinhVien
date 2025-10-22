using System.Data;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;
using Svg;

namespace QuanLySinhVien.Views.Components;

public class MoDangKyHocPhan : NavBase
{
    private string ID = "MODANGKYHOCPHAN";

    private List<string> _listSelectionForComboBox;
    private string _title = "Mở đăng ký học phần";

    string[] _headerArray = new string[]
    {
        "Mã nhóm HP",
        "Tên HP",
        "Sĩ số",
        "Giảng viên",
    };

    private CustomTable _table;

    List<NhomHocPhanDto> _rawData;
    List<object> _displayData;
    List<string> _headerList;

    private TableLayoutPanel _mainLayout;

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private NhomHocPhanController _nhomHocPhanController;
    // private LichHocController _lichHocController;
    private HocPhanController _hocPhanController;
    private GiangVienController _giangVienController;
    // private PhongHocController _phongHocController;

    private TitleButton _insertButton;
    private TitleButton _updateTimeButton;

    private NhomHocPhanSearch _nhomHocPhanSearch;

    private NhomHocPhanDialog _NhomHocPhanDialog;

    private List<InputFormItem> _listIFI;

    private TableLayoutPanel _panelTop;
    private RoundTLP _panelBottom;
    private TableLayoutPanel _bottomTimePnl;
    private LabelTextField _hocKyField;
    private LabelTextField _namField;

    private List<ChiTietQuyenDto> _listAccess;
    private bool them = false;
    private bool sua = false;
    private bool xoa = false;

    public MoDangKyHocPhan(NhomQuyenDto quyen) : base(quyen)
    {
        _rawData = new List<NhomHocPhanDto>();
        _displayData = new List<object>();
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        // _lichHocController = LichHocController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        // _phongHocController = PhongHocController.getInstance();
        Init();
    }

    private void Init()
    {
        CheckQuyen();

        Dock = DockStyle.Fill;

        _mainLayout = new TableLayoutPanel
        {
            RowCount = 3,
            Dock = DockStyle.Fill,
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        setTop();
        setBottom();
        SetBottomTimePnl();
        
        SetCombobox();
        SetSearch();
        SetAction();
        

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


    private void setTop()
    {
        _panelTop = new TableLayoutPanel
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
        
        _mainLayout.Controls.Add(_panelTop);
    }

    private void SetHKyNamContainer()
    {
        TableLayoutPanel panel = new TableLayoutPanel
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
        
        
        panel.Controls.Add(_hocKyField);
        panel.Controls.Add(_namField);
        
        _panelTop.Controls.Add(panel);
    }
    
    

    private void setBottom()
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
            Text="Nhóm học phần",
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

        SetDataTableFromDb();
        _panelBottom.Controls.Add(_table);
        _panelBottom.SetColumnSpan(_table, 2);

        _mainLayout.Controls.Add(_panelBottom);
    }

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

    void SetBottomTimePnl()
    {
        _bottomTimePnl = new RoundTLP
        {
            Border = true,
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 3,
            Padding = new Padding(10, 10, 10, 30),
        };
        
        _bottomTimePnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _bottomTimePnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _bottomTimePnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        // Label lblStart = new Label
        // {
        //     Margin = new Padding(5),
        //     AutoSize = true,
        //     Text = "Thời gian bắt đầu",
        //     Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold),
        // };
        // Label lblEnd = new Label
        // {
        //     Margin = new Padding(5),
        //     AutoSize = true,
        //     Text = "Thời gian kết thúc",
        //     Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold),
        // };

        LabelTextField startDTField = new LabelTextField("Thời gian bắt đầu", TextFieldType.DateTime);
        _bottomTimePnl.Controls.Add(startDTField);
        LabelTextField endDTField = new LabelTextField("Thời gian kết thúc", TextFieldType.DateTime);
        _bottomTimePnl.Controls.Add(endDTField);
        
        
        _updateTimeButton = new TitleButton("Cập nhật", "reload.svg");
        _updateTimeButton.Margin = new Padding(3, 3, 20, 3);
        _updateTimeButton.Anchor = AnchorStyles.None;
        _updateTimeButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        
        _bottomTimePnl.Controls.Add(_updateTimeButton);
        
        _mainLayout.Controls.Add(_bottomTimePnl);
    }
    
    

    void SetCombobox()
    {
        _listSelectionForComboBox = _headerList;
    }


    void SetDataTableFromDb()
    {
        _rawData = _nhomHocPhanController.GetAll();
        SetDisplayData();

        string[] columnNames = new[]
        {
            "MaNHP",
            "TenHP",
            "Siso",
            "TenGiangVien",
        };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    void SetDisplayData()
    {
        _displayData = ConvertListNhomHPoObj(_rawData);
    }

    List<object> ConvertListNhomHPoObj(List<NhomHocPhanDto> dtos)
    {
        List<object> list = ConvertObject.ConvertToDisplay(dtos, x => new NhomHocPhanDisplay()
            {
                MaNHP = x.MaNHP,
                TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                Siso = x.SiSo,
                TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV,
            }
        );
        return list;
    }


    void SetSearch()
    {
        List<NhomHocPhanDisplay> list = ConvertObject.ConvertDtoToDto(_rawData, x => new NhomHocPhanDisplay
            {
                MaNHP = x.MaNHP,
                TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                Siso = x.SiSo,
                TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV,
            }
        );
        
        _nhomHocPhanSearch = new NhomHocPhanSearch(list);
    }

    void SetAction()
    {
        _nhomHocPhanSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
        
        
    }

    void UpdateDataDisplay(List<NhomHocPhanDisplay> list)
    {
        this._displayData = list.Cast<object>().ToList();
    }

    void Insert()
    {
        NhomHocPhanDialog dialog = new NhomHocPhanDialog(_title, DialogType.Them, _hocKyField.GetSelectionCombobox(), _namField.GetTextNam());
        dialog.ShowDialog();
    }

    void Update(int id)
    {
        
    }

    void Detail(int id)
    {
        
    }

    void Delete(int index)
    {
        
    }


    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._nhomHocPhanSearch.Search(txtSearch, filter);
    }
}