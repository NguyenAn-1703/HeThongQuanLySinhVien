using Mysqlx.Crud;
using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.NavList;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components;

public class MoDangKyHocPhan : NavBase
{
    private readonly string _title = "Mở đăng ký học phần";
    private readonly string ID = "MODANGKYHOCPHAN";

    private readonly string[] _headerArray = new[] { "Mã đợt", "Năm", "Học kỳ", "Thời gian mở", "Thời gian kết thúc" };
    private List<string> _headerList;

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private DotDangKyController _dotDangKyController;
    private TitleButton _insertButton;
    private DotDKSearch _dotDKSearch;


    private List<ChiTietQuyenDto> _listAccess;

    private List<string> _listSelectionForComboBox;

    private MyTLP _mainLayout;
    private NhomHocPhanController _nhomHocPhanController;
    private RoundTLP _panelBottom;
    private MyTLP _panelTop;
    private MyTLP _screen;

    private List<DotDangKyDto> _rawData;
    private List<object> _displayData;

    private CustomTable _table;
    private bool sua;
    private bool them;
    private bool xoa;

    public MoDangKyHocPhan(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<DotDangKyDto>();
        _displayData = new List<object>();
        _headerList = _headerArray.ToList();
        _dotDangKyController = DotDangKyController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
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
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new ColumnStyle(SizeType.Percent, 100));


        setTop();
        SetRootBar();
        setBottom();
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
            Padding = new Padding(10),
            ColumnCount = 3,
            BackColor = MyColor.GrayBackGround
        };

        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _panelTop.Controls.Add(getTitle());

        _mainLayout.Controls.Add(_panelTop);
    }

    private RootBar rootBar;

    private void SetRootBar()
    {
        rootBar = new RootBar();
        rootBar.AddItem("Đợt đăng ký");
        _mainLayout.Controls.Add(rootBar);
    }


    private void setBottom()
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
        };

        _panelBottom.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _panelBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var lblNhomHocPhan = new Label
        {
            Margin = new Padding(5),
            AutoSize = true,
            Text = "Đợt đăng ký",
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
        _screen.Controls.Add(_panelBottom);
        _mainLayout.Controls.Add(_screen);
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

    private void SetCombobox()
    {
        _listSelectionForComboBox = _headerList;
    }


    private void SetDataTableFromDb()
    {
        // _rawData = _nhomHocPhanController.GetAll();
        SetDisplayData();
        var columnNames = new[]
        {
            "MaDotDK",
            "Nam",
            "HocKy",
            "ThoiGianBatDau",
            "ThoiGianKetThuc"
        };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    private void SetDisplayData()
    {
        _rawData = _dotDangKyController.GetAll();
        _displayData = _rawData.Cast<object>().ToList();
    }

    private void UpdateDisplayData(List<DotDangKyDto> list)
    {
        _displayData = list.Cast<object>().ToList();
    }


    private void SetSearch()
    {
        _dotDKSearch = new DotDKSearch(_rawData);
    }

    private void SetAction()
    {
        _dotDKSearch.FinishSearch += dtos =>
        {
            UpdateDisplayData(dtos);
            _table.UpdateData(_displayData);
        };
        _insertButton._mouseDown += () => { Insert(); };

        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    void Insert()
    {
        NamHocKyDialog namHocKyDialog = new NamHocKyDialog("Thêm đợt đăng ký", DialogType.Them);
        namHocKyDialog.Finish += () =>
        {
            List<DotDangKyDto> list = _dotDangKyController.GetAll();
            UpdateDisplayData(list);
            _table.UpdateData(_displayData);

            DotDangKyDto dotDK = list[list.Count - 1];

            ChangePanel(DialogType.Them, dotDK.MaDotDK);
        };
        namHocKyDialog.ShowDialog();
    }

    // private List<NhomHocPhanDisplay> ConvertToDtoDisplay(List<NhomHocPhanDto> input)
    // {
    //     List<NhomHocPhanDisplay> list = ConvertObject.ConvertDtoToDto(input, x => new NhomHocPhanDisplay
    //         {
    //             MaNHP = x.MaNHP,
    //             TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
    //             Siso = x.SiSo,
    //             TenGiangVien = _giangVienController.GetById(x.MaGV).TenGV
    //         }
    //     );
    //     return list;
    // }

    // private void UpdateDataDisplay(List<NhomHocPhanDisplay> list)
    // {
    //     _displayData = list.Cast<object>().ToList();
    // }

    // private void Insert()
    // {
    //     var dialog = new NhomHocPhanDialog("Thêm nhóm học phần", DialogType.Them, _hocKyField.GetSelectionCombobox(),
    //         _namField.GetTextNam(), _rawDataFilter);
    //
    //
    //     dialog.Finish += () =>
    //     {
    //         _rawData = _nhomHocPhanController.GetAll();
    //         UpdateRawDataFilter();
    //         var list = ConvertToDtoDisplay(_rawDataFilter);
    //         UpdateDataDisplay(list);
    //         _table.UpdateData(_displayData);
    //     };
    //
    //     dialog.ShowDialog();
    // }

    private void Update(int id)
    {
        ChangePanel(DialogType.Sua, id);
    }

    private void Detail(int id)
    {
        ChangePanel(DialogType.ChiTiet, id);
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        if (_dotDangKyController.Delete(index))
        {
            MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _rawData = _dotDangKyController.GetAll();
            UpdateDisplayData(_rawData);
            _table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    //
    // private bool Validate()
    // {
    //     if (_rawDataFilter.Count == 0)
    //     {
    //         MessageBox.Show("Vui lòng thêm nhóm học phần!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //         return false;
    //     }
    //
    //     return true;
    // }


    private void ChangePanel(DialogType type, int id = -1)
    {
        _screen.SuspendLayout();
        _screen.Controls.Clear();

        DotDangKyDto dotDangKyDto = _dotDangKyController.GetById(id);
        var NhomHocPhanScreen = new NhomHocPhanScreen("DS Nhóm học phần",
            dotDangKyDto.HocKy, dotDangKyDto.Nam, id, type);
        NhomHocPhanScreen.Back += () => ResetPanel();
        _screen.Controls.Add(NhomHocPhanScreen);
        _screen.ResumeLayout();

        rootBar.AddItem($"Đợt đăng ký {dotDangKyDto.MaDotDK} học kỳ {dotDangKyDto.HocKy} - năm {dotDangKyDto.Nam}");
    }

    private void ResetPanel()
    {
        _screen.Controls.Clear();
        _screen.Controls.Add(_panelBottom);
        rootBar.RemoveItem();
    }

    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _dotDKSearch.Search(txtSearch, filter);
    }
}