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

namespace QuanLySinhVien.Views.Components;
public class NhapDiem : NavBase
{
    private string ID = "NHAPDIEM";
    private string _title = "Nhập điểm";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private NhomQuyenController _nhomQuyenController;
    string[] _headerArray = new string[]
    {
        "Mã nhóm HP",
        "Tên HP",
        "Sĩ số",
        "Giảng viên",
    };
    List<string> _headerList;
    // private TitleButton _insertButton;

    List<NhomHocPhanDto> _rawData;
    List<object> _displayData;

    private NhomHocPhanSearch _NhomHocPhanSearch;
    private NhomHocPhanController _NhomHocPhanController;
    
    private List<ChiTietQuyenDto> _listAccess;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private HocPhanController _hocPhanController;
    private PhongHocController _phongHocController;
    
    private GiangVienController _giangVienController;

    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    

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
            Text="Nhóm học phần",
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
        
        string[] columnNames = new[] { "MaNHP", "TenHP", "Siso", "TenGiangVien"};
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, false);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                MaNHP = x.MaNHP,
                TenHP = x.TenHP,
                Siso = x.Siso,
                TenGiangVien = x.TenGiangVien,
            }
        );
    }


    void SetSearch()
    {
        _NhomHocPhanSearch = new NhomHocPhanSearch(ConvertDtoToDisplay(_rawData));
    }

    void SetAction()
    {
        _NhomHocPhanSearch.FinishSearch += dtos =>
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

    void UpdateDataDisplay(List<NhomHocPhanDisplay> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaNHP = x.MaNHP,
            TenHP = x.TenHP,
            Siso = x.Siso,
            TenGiangVien = x.TenGiangVien,
        });
    }
    
    void ChangePanel(int id)
    {
        _screen.SuspendLayout();
        _screen.Controls.Clear();
        NhapDiemDialog nhapDiemDialog = new NhapDiemDialog("Nhập điểm", id);

        nhapDiemDialog.Back += () => ResetPanel();
        _screen.Controls.Add(nhapDiemDialog);
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
    
    List<NhomHocPhanDisplay> ConvertDtoToDisplay(List<NhomHocPhanDto> input)
    {
        List<NhomHocPhanDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new NhomHocPhanDisplay
        {
            MaNHP = x.MaNHP,
            TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            Siso = x.SiSo,
            TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV,
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
        this._NhomHocPhanSearch.Search(txtSearch, filter, ConvertDtoToDisplay(_rawData));
    }
}