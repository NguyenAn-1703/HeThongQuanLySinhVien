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
    private List<NhomHocPhanDto> _rawDataFilter;
    List<object> _displayData;
    List<string> _headerList;

    private MyTLP _mainLayout;

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private NhomHocPhanController _nhomHocPhanController;
    private HocPhanController _hocPhanController;
    private GiangVienController _giangVienController;
    private LichDangKyController _lichDangKyController;

    private TitleButton _insertButton;
    private TitleButton _updateTimeButton;

    private NhomHocPhanSearch _nhomHocPhanSearch;

    private NhomHocPhanDialog _NhomHocPhanDialog;

    private List<InputFormItem> _listIFI;

    private MyTLP _panelTop;
    private RoundTLP _panelBottom;
    private RoundTLP _bottomTimePnl;
    private LabelTextField _hocKyField;
    private LabelTextField _namField;
    
    private LabelTextField startDTField;
    private LabelTextField endDTField;

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
        
        _lichDangKyController = LichDangKyController.GetInstance();
        Init();
    }

    private void Init()
    {
        CheckQuyen();

        Dock = DockStyle.Fill;

        _mainLayout = new MyTLP
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
        
        _mainLayout.Controls.Add(_panelTop);
    }

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
        

        startDTField = new LabelTextField("Thời gian bắt đầu", TextFieldType.DateTime);
        _bottomTimePnl.Controls.Add(startDTField);
        endDTField = new LabelTextField("Thời gian kết thúc", TextFieldType.DateTime);
        _bottomTimePnl.Controls.Add(endDTField);
        
        
        _updateTimeButton = new TitleButton("Cập nhật", "reload.svg");
        _updateTimeButton.Margin = new Padding(3, 20, 20, 3);
        _updateTimeButton.Anchor = AnchorStyles.None;
        _updateTimeButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);



        UpdateTimeField();
        
        _bottomTimePnl.Controls.Add(_updateTimeButton);
        
        _mainLayout.Controls.Add(_bottomTimePnl);
    }

    void UpdateTimeField()
    {
        DateTime now = DateTime.Now;
        
        if (_rawDataFilter.Count == 0) // ds theo học kỳ và năm chưa đc tao
        {
            startDTField._dTNgayField.dateField.Value = now;
            startDTField._dTGioField.dateField.Value = now;
            endDTField._dTNgayField.dateField.Value = now;
            endDTField._dTGioField.dateField.Value = now;
            
            startDTField._dTNgayField.dateField.Enabled = true;
            startDTField._dTGioField.dateField.Enabled = true;
            endDTField._dTNgayField.dateField.Enabled = true;
            endDTField._dTGioField.dateField.Enabled = true;
            
            _updateTimeButton.Enabled = true;
            _insertButton.Enabled = true;
            return;
        }
        NhomHocPhanDto nhp1 = _rawDataFilter[0];
        LichDangKyDto lichDangKy = _lichDangKyController.GetById(nhp1.MaLichDK);

        if (lichDangKy.MaLichDK == 1) //chưa cập nhật lịch mới
        {
            startDTField._dTNgayField.dateField.Value = now;
            startDTField._dTGioField.dateField.Value = now;
            endDTField._dTNgayField.dateField.Value = now;
            endDTField._dTGioField.dateField.Value = now;
            
            startDTField._dTNgayField.dateField.Enabled = true;
            startDTField._dTGioField.dateField.Enabled = true;
            endDTField._dTNgayField.dateField.Enabled = true;
            endDTField._dTGioField.dateField.Enabled = true;
            _updateTimeButton.Enabled = true;
            _insertButton.Enabled = true;
            return;
        }
        
        DateTime start = lichDangKy.ThoiGianBatDau;
        DateTime end = lichDangKy.ThoiGianKetThuc;
        
        startDTField._dTNgayField.dateField.Value = start;
        startDTField._dTGioField.dateField.Value = start;
        endDTField._dTNgayField.dateField.Value = end;
        endDTField._dTGioField.dateField.Value = end;
        
        // nếu tgian đky đã qua ngày hiện tại thì không cho sửa lại
        if (now > start)
        {
            startDTField._dTNgayField.dateField.Enabled = false;
            startDTField._dTGioField.dateField.Enabled = false;
            endDTField._dTNgayField.dateField.Enabled = false;
            endDTField._dTGioField.dateField.Enabled = false;
            
            _updateTimeButton.Enabled = false;
            _insertButton.Enabled = false;
        }
        
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
        string hocKy = _hocKyField.GetSelectionCombobox().Split(" ")[2];
        string nam = DateTime.Now.Year.ToString();
        
        _rawDataFilter = _rawData.Where(x => 
            x.HocKy.ToString().Equals(hocKy) &&
            x.Nam.Equals(nam)
            ).ToList();
        
        
        _displayData = ConvertListNhomHPoObj(_rawDataFilter);
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

        _hocKyField.GetComboboxField().SelectedIndexChanged += (sender, args) => OnChangeNamHK();
        _namField._namField.ValueChanged += (sender, args) => OnChangeNamHK();

        _updateTimeButton._mouseDown += () => { OnClickBtnCapNhat(); };

    }

    void OnClickBtnCapNhat()
    {
        DateTime start = startDTField._dTNgayField.dateField.Value;
        DateTime end = endDTField._dTNgayField.dateField.Value;
        DateTime now = DateTime.Now;
        
        string startStr = start.ToString("HH:mm:ss ") + "Ngày " + start.ToString("dd/MM/yyyy");
        string endStr = end.ToString("HH:mm:ss ") + "Ngày " + end.ToString("dd/MM/yyyy");

        if (!Validate())
        {
            return;
        }
        
        if (start < now)
        {
            DialogResult rs = MessageBox.Show("Thời gian đăng ký học phần " +
                            _hocKyField.GetSelectionCombobox() + 
                            " Năm học: " + 
                            _namField.GetTextNam() +
                            "\n" +
                            "Sẽ được mở vào: " + 
                            startStr + "\n" + 
                            "Cho đến: " +
                            endStr + "\n" +
                            "Sau khi cập nhật sẽ không thể thay đổi!",
                "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (rs == DialogResult.No)
            {
                return;
            }
        }
        
        LichDangKyDto lichDk = new LichDangKyDto
        {
            ThoiGianBatDau = start,
            ThoiGianKetThuc = end
        };
        if (!_lichDangKyController.Insert(lichDk))
        {
            MessageBox.Show("Thêm lịch đăng ký thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        List<LichDangKyDto> listL = _lichDangKyController.GetAll();
        LichDangKyDto lich = listL[listL.Count - 1];
        
        foreach (NhomHocPhanDto item in _rawDataFilter)
        {
            item.MaLichDK = lich.MaLichDK;
            if (!_nhomHocPhanController.Update(item))
            {
                MessageBox.Show("Cập nhật nhóm học phần thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        
        MessageBox.Show("Cập nhật lịch đăng ký môn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        UpdateTimeField();
    }

    void OnChangeNamHK()
    {
        UpdateRawDataFilter();
        UpdateTimeField();
        
        List<NhomHocPhanDisplay> list = ConvertToDtoDisplay(_rawDataFilter);
        UpdateDataDisplay(list);
        this._table.UpdateData(_displayData);
    }

    void UpdateRawDataFilter()
    {
        string hocKy = _hocKyField.GetSelectionCombobox().Split(" ")[2];
        string nam = _namField.GetTextNam();
        _rawDataFilter = _rawData.Where(x => 
            x.HocKy.ToString().Equals(hocKy) &&
            x.Nam.Equals(nam)
        ).ToList();
    }

    List<NhomHocPhanDisplay> ConvertToDtoDisplay(List<NhomHocPhanDto> input)
    {
        List<NhomHocPhanDisplay> list = ConvertObject.ConvertDtoToDto(input, x => new NhomHocPhanDisplay
            {
                MaNHP = x.MaNHP,
                TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                Siso = x.SiSo,
                TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV,
            }
        );
        return list;
    }

    void UpdateDataDisplay(List<NhomHocPhanDisplay> list)
    {
        this._displayData = list.Cast<object>().ToList();
    }

    void Insert()
    {
        NhomHocPhanDialog dialog = new NhomHocPhanDialog("Thêm nhóm học phần", DialogType.Them, _hocKyField.GetSelectionCombobox(), _namField.GetTextNam(), _rawDataFilter);
        
        
        dialog.Finish += () =>
        {
            _rawData = _nhomHocPhanController.GetAll();
            UpdateRawDataFilter();
            List<NhomHocPhanDisplay> list = ConvertToDtoDisplay(_rawDataFilter);
            UpdateDataDisplay(list);
            this._table.UpdateData(_displayData);
        };
        
        dialog.ShowDialog();
    }

    void Update(int id)
    {
        NhomHocPhanDialog dialog = new NhomHocPhanDialog("Sửa nhóm học phần", DialogType.Sua, _hocKyField.GetSelectionCombobox(), _namField.GetTextNam(),_rawDataFilter, id);
        dialog.Finish += () =>
        {
            _rawData = _nhomHocPhanController.GetAll();
            UpdateRawDataFilter();
            List<NhomHocPhanDisplay> list = ConvertToDtoDisplay(_rawDataFilter);
            UpdateDataDisplay(list);
            this._table.UpdateData(_displayData);
        };
        dialog.ShowDialog();
    }

    void Detail(int id)
    {
        NhomHocPhanDialog dialog = new NhomHocPhanDialog("Chi tiết nhóm học phần", DialogType.ChiTiet, _hocKyField.GetSelectionCombobox(), _namField.GetTextNam(),_rawDataFilter, id);
        dialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        if (_nhomHocPhanController.Delete(index))
        {
            MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _rawData = _nhomHocPhanController.GetAll();
            UpdateRawDataFilter();
            UpdateDataDisplay(ConvertToDtoDisplay(_rawDataFilter));
            this._table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    bool Validate()
    {
        if (_rawDataFilter.Count == 0)
        {
            MessageBox.Show("Vui lòng thêm nhóm học phần!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }


    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._nhomHocPhanSearch.Search(txtSearch, filter, ConvertToDtoDisplay(_rawDataFilter));
    }
}

