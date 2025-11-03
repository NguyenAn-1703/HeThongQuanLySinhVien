using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.NavList;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components;

public class MoDangKyHocPhan : NavBase
{
    private readonly string[] _headerArray = new[]
    {
        "Mã nhóm HP",
        "Tên HP",
        "Sĩ số",
        "Giảng viên"
    };

    private readonly List<string> _headerList;
    private readonly string _title = "Mở đăng ký học phần";
    private readonly string ID = "MODANGKYHOCPHAN";
    private RoundTLP _bottomTimePnl;

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;
    private GiangVienController _giangVienController;
    private LabelTextField _hocKyField;
    private HocPhanController _hocPhanController;

    private TitleButton _insertButton;
    private LichDangKyController _lichDangKyController;

    private List<ChiTietQuyenDto> _listAccess;

    private List<InputFormItem> _listIFI;

    private List<string> _listSelectionForComboBox;

    private MyTLP _mainLayout;
    private LabelTextField _namField;
    private NhomHocPhanController _nhomHocPhanController;

    private NhomHocPhanDialog _NhomHocPhanDialog;

    private NhomHocPhanSearch _nhomHocPhanSearch;
    private RoundTLP _panelBottom;

    private MyTLP _panelTop;

    private List<NhomHocPhanDto> _rawData;
    private List<NhomHocPhanDto> _rawDataFilter;

    private CustomTable _table;
    private TitleButton _updateTimeButton;
    private LabelTextField endDTField;

    private LabelTextField startDTField;
    private bool sua;
    private bool them;
    private bool xoa;

    public MoDangKyHocPhan(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
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
            Dock = DockStyle.Fill
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

    private void CheckQuyen()
    {
        int maCN = _chucNangController.GetByTen(ID).MaCN;
        _listAccess = _chiTietQuyenController.GetByMaNQMaCN(_quyen.MaNQ, maCN);
        foreach (var x in _listAccess) Console.WriteLine(x.HanhDong);

        if (_listAccess.Any(x => x.HanhDong.Equals("Them"))) them = true;

        if (_listAccess.Any(x => x.HanhDong.Equals("Sua"))) sua = true;

        if (_listAccess.Any(x => x.HanhDong.Equals("Xoa"))) xoa = true;
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
            Padding = new Padding(10)
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

        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;

        if (them) _panelBottom.Controls.Add(_insertButton);

        SetDataTableFromDb();
        _panelBottom.Controls.Add(_table);
        _panelBottom.SetColumnSpan(_table, 2);

        _mainLayout.Controls.Add(_panelBottom);
    }

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

    private void SetBottomTimePnl()
    {
        _bottomTimePnl = new RoundTLP
        {
            Border = true,
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 3,
            Padding = new Padding(10, 10, 10, 30)
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

    private void UpdateTimeField()
    {
        var now = DateTime.Now;

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
            _table.EnableActionColumn();
            return;
        }

        var nhp1 = _rawDataFilter[0];
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
            _table.EnableActionColumn();

            return;
        }

        var start = lichDangKy.ThoiGianBatDau;
        var end = lichDangKy.ThoiGianKetThuc;

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
            _table.DisapleActionColumn();
        }
    }


    private void SetCombobox()
    {
        _listSelectionForComboBox = _headerList;
    }


    private void SetDataTableFromDb()
    {
        _rawData = _nhomHocPhanController.GetAll();
        SetDisplayData();

        var columnNames = new[]
        {
            "MaNHP",
            "TenHP",
            "Siso",
            "TenGiangVien"
        };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    private void SetDisplayData()
    {
        var hocKy = _hocKyField.GetSelectionCombobox().Split(" ")[2];
        var nam = DateTime.Now.Year.ToString();

        _rawDataFilter = _rawData.Where(x =>
            x.HocKy.ToString().Equals(hocKy) &&
            x.Nam.Equals(nam)
        ).ToList();


        _displayData = ConvertListNhomHPoObj(_rawDataFilter);
    }

    private List<object> ConvertListNhomHPoObj(List<NhomHocPhanDto> dtos)
    {
        List<object> list = ConvertObject.ConvertToDisplay(dtos, x => new NhomHocPhanDisplay
            {
                MaNHP = x.MaNHP,
                TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                Siso = x.SiSo,
                TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV
            }
        );
        return list;
    }


    private void SetSearch()
    {
        List<NhomHocPhanDisplay> list = ConvertObject.ConvertDtoToDto(_rawData, x => new NhomHocPhanDisplay
            {
                MaNHP = x.MaNHP,
                TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                Siso = x.SiSo,
                TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV
            }
        );

        _nhomHocPhanSearch = new NhomHocPhanSearch(list);
    }

    private void SetAction()
    {
        _nhomHocPhanSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };

        _hocKyField.GetComboboxField().SelectedIndexChanged += (sender, args) => OnChangeNamHK();
        _namField._namField.ValueChanged += (sender, args) => OnChangeNamHK();

        _updateTimeButton._mouseDown += () => { OnClickBtnCapNhat(); };
    }

    private void OnClickBtnCapNhat()
    {
        var start = startDTField._dTNgayField.dateField.Value;
        var end = endDTField._dTNgayField.dateField.Value;
        var now = DateTime.Now;

        var startStr = start.ToString("HH:mm:ss ") + "Ngày " + start.ToString("dd/MM/yyyy");
        var endStr = end.ToString("HH:mm:ss ") + "Ngày " + end.ToString("dd/MM/yyyy");

        if (!Validate()) return;

        if (start < now)
        {
            var rs = MessageBox.Show("Thời gian đăng ký học phần " +
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
            if (rs == DialogResult.No) return;
        }

        var lichDk = new LichDangKyDto
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
        var lich = listL[listL.Count - 1];

        foreach (var item in _rawDataFilter)
        {
            item.MaLichDK = lich.MaLichDK;
            if (!_nhomHocPhanController.Update(item))
            {
                MessageBox.Show("Cập nhật nhóm học phần thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        MessageBox.Show("Cập nhật lịch đăng ký môn thành công!", "Thành công", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        UpdateTimeField();
    }

    private void OnChangeNamHK()
    {
        UpdateRawDataFilter();
        UpdateTimeField();

        var list = ConvertToDtoDisplay(_rawDataFilter);
        UpdateDataDisplay(list);
        _table.UpdateData(_displayData);
    }

    private void UpdateRawDataFilter()
    {
        var hocKy = _hocKyField.GetSelectionCombobox().Split(" ")[2];
        var nam = _namField.GetTextNam();
        _rawDataFilter = _rawData.Where(x =>
            x.HocKy.ToString().Equals(hocKy) &&
            x.Nam.Equals(nam)
        ).ToList();
    }

    private List<NhomHocPhanDisplay> ConvertToDtoDisplay(List<NhomHocPhanDto> input)
    {
        List<NhomHocPhanDisplay> list = ConvertObject.ConvertDtoToDto(input, x => new NhomHocPhanDisplay
            {
                MaNHP = x.MaNHP,
                TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                Siso = x.SiSo,
                TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV
            }
        );
        return list;
    }

    private void UpdateDataDisplay(List<NhomHocPhanDisplay> list)
    {
        _displayData = list.Cast<object>().ToList();
    }

    private void Insert()
    {
        var dialog = new NhomHocPhanDialog("Thêm nhóm học phần", DialogType.Them, _hocKyField.GetSelectionCombobox(),
            _namField.GetTextNam(), _rawDataFilter);


        dialog.Finish += () =>
        {
            _rawData = _nhomHocPhanController.GetAll();
            UpdateRawDataFilter();
            var list = ConvertToDtoDisplay(_rawDataFilter);
            UpdateDataDisplay(list);
            _table.UpdateData(_displayData);
        };

        dialog.ShowDialog();
    }

    private void Update(int id)
    {
        var dialog = new NhomHocPhanDialog("Sửa nhóm học phần", DialogType.Sua, _hocKyField.GetSelectionCombobox(),
            _namField.GetTextNam(), _rawDataFilter, id);
        dialog.Finish += () =>
        {
            _rawData = _nhomHocPhanController.GetAll();
            UpdateRawDataFilter();
            var list = ConvertToDtoDisplay(_rawDataFilter);
            UpdateDataDisplay(list);
            _table.UpdateData(_displayData);
        };
        dialog.ShowDialog();
    }

    private void Detail(int id)
    {
        var dialog = new NhomHocPhanDialog("Chi tiết nhóm học phần", DialogType.ChiTiet,
            _hocKyField.GetSelectionCombobox(), _namField.GetTextNam(), _rawDataFilter, id);
        dialog.ShowDialog();
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        if (_nhomHocPhanController.Delete(index))
        {
            MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _rawData = _nhomHocPhanController.GetAll();
            UpdateRawDataFilter();
            UpdateDataDisplay(ConvertToDtoDisplay(_rawDataFilter));
            _table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool Validate()
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
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _nhomHocPhanSearch.Search(txtSearch, filter, ConvertToDtoDisplay(_rawDataFilter));
    }
}