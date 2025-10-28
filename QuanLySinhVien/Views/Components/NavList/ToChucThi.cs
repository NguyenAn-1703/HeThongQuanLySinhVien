using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
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
    

    private Panel Top()
    {
        MyTLP panel = new MyTLP
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
        
        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        if (them)
        {
            panel.Controls.Add(_insertButton);
        }
        

        return panel;
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
        _rawData = _CaThiController.GetAll();
        SetDisplayData();
        
    // public int MaCT { get; set; }
    // public string TenHP { get; set; }
    // public string TenPhong { get; set; }
    // public string Thu { get; set; }
    // public string ThoiGianBatDau { get; set; }
    // public string ThoiLuong { get; set; }
    // public int SiSo { get; set; }

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
        _CaThiDialog = new CaThiDialog("Thêm ca thi", DialogType.Them);
        _CaThiDialog.Finish += () =>
        {
            UpdateDataDisplay(ConvertDtoToDisplay(_CaThiController.GetAll()));
            this._table.UpdateData(_displayData);
        };
        this._CaThiDialog.ShowDialog();
    }

    void Update(int id)
    {
        _CaThiDialog = new CaThiDialog("Sửa ca thi", DialogType.Sua, id);
        _CaThiDialog.Finish += () =>
        {
            UpdateDataDisplay(ConvertDtoToDisplay(_CaThiController.GetAll()));
            this._table.UpdateData(_displayData);
        };
        this._CaThiDialog.ShowDialog();
    }

    void Detail(int id)
    {
        _CaThiDialog = new CaThiDialog("Chi tiết ca thi", DialogType.ChiTiet, id);
        _CaThiDialog.Finish += () =>
        {
            UpdateDataDisplay(ConvertDtoToDisplay(_CaThiController.GetAll()));
            this._table.UpdateData(_displayData);
        };
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