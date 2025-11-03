using QuanLyGiangVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.GetFont;
using QuanLySinhVien.View.Views.Components.NavList;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLyGiangVien.Views.Components;

public class GiangVien : NavBase
{
    private readonly string[] _headerArray = new[]
        { "Mã giảng viên", "Tên giảng viên", "Khoa", "Ngày sinh", "Giới tính", "Số điện thoại", "Email" };

    private readonly string _title = "Giảng viên";
    private readonly string ID = "GiangVien";
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;
    private GiangVienController _GiangVienController;

    private GiangVienDialog _GiangVienDialog;

    private GiangVienSearch _GiangVienSearch;
    private List<string> _headerList;

    private TitleButton _insertButton;
    private KhoaController _khoaController;

    private List<ChiTietQuyenDto> _listAccess;

    private List<InputFormItem> _listIFI;
    private List<string> _listSelectionForComboBox;
    private LopController _lopController;
    private NganhController _nganhController;
    private NhomQuyenController _nhomQuyenController;

    private List<GiangVienDto> _rawData;
    private CustomTable _table;
    private bool sua;

    private bool them;
    private bool xoa;


    public GiangVien(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<GiangVienDto>();
        _displayData = new List<object>();
        _GiangVienController = GiangVienController.GetInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _lopController = LopController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        _nganhController = NganhController.GetInstance();
        Init();
    }

    private void Init()
    {
        CheckQuyen();

        Dock = DockStyle.Fill;

        var mainLayout = new MyTLP
        {
            RowCount = 2,
            Dock = DockStyle.Fill
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        mainLayout.Controls.Add(Top());
        mainLayout.Controls.Add(Bottom());

        Controls.Add(mainLayout);
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
        var panel = new MyTLP
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
        _insertButton._label.Font = GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        if (them) panel.Controls.Add(_insertButton);


        return panel;
    }

    private Panel Bottom()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill
        };

        SetCombobox();

        SetDataTableFromDb();

        SetSearch();

        SetAction();

        panel.Controls.Add(_table);

        return panel;
    }

    //////////////////////////////SETTOP///////////////////////////////
    private Label getTitle()
    {
        var titlePnl = new Label
        {
            Text = _title,
            Font = GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true
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
        _rawData = _GiangVienController.GetAll();
        SetDisplayData();

        var columnNames = new[] { "MaGV", "TenGV", "TenKhoa", "NgaySinhGV", "GioiTinhGV", "SoDienThoai", "Email" };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                x.MaGV,
                x.TenGV,
                x.TenKhoa,
                x.NgaySinhGV,
                x.GioiTinhGV,
                x.SoDienThoai,
                x.Email
            }
        );
    }


    private void SetSearch()
    {
        _GiangVienSearch = new GiangVienSearch(ConvertDtoToDisplay(_rawData));
    }

    private void SetAction()
    {
        _GiangVienSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    private void UpdateDataDisplay(List<GiangVienDisplay> input)
    {
        _displayData = ConvertObject.ConvertToDisplay(input, x => new
        {
            x.MaGV,
            x.TenGV,
            x.TenKhoa,
            x.NgaySinhGV,
            x.GioiTinhGV,
            x.SoDienThoai,
            x.Email
        });
    }

    private void Insert()
    {
        _GiangVienDialog = new GiangVienDialog("Thêm giảng viên", DialogType.Them);

        _GiangVienDialog.Finish += () =>
        {
            var listSv = ConvertDtoToDisplay(_GiangVienController.GetAll());
            UpdateDataDisplay(listSv);
            _table.UpdateData(_displayData);
        };
        _GiangVienDialog.ShowDialog();
    }

    private void Update(int id)
    {
        _GiangVienDialog = new GiangVienDialog("Sửa giảng viên", DialogType.Sua, id);
        _GiangVienDialog.Finish += () =>
        {
            var listSv = ConvertDtoToDisplay(_GiangVienController.GetAll());
            UpdateDataDisplay(listSv);
            _table.UpdateData(_displayData);
        };
        _GiangVienDialog.ShowDialog();
    }

    private void Detail(int id)
    {
        _GiangVienDialog = new GiangVienDialog("Chi tiết giảng viên", DialogType.ChiTiet, id);
        _GiangVienDialog.ShowDialog();
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;

        _GiangVienController.SoftDeleteById(index);
        MessageBox.Show("Xóa giảng viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        UpdateDataDisplay(ConvertDtoToDisplay(_GiangVienController.GetAll()));
        _table.UpdateData(_displayData);
    }

    private List<GiangVienDisplay> ConvertDtoToDisplay(List<GiangVienDto> input)
    {
        List<GiangVienDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new GiangVienDisplay
        {
            MaGV = x.MaGV,
            TenGV = x.TenGV,
            TenKhoa = _khoaController.GetKhoaById(x.MaKhoa).TenKhoa,
            NgaySinhGV = x.NgaySinhGV,
            GioiTinhGV = x.GioiTinhGV,
            SoDienThoai = x.SoDienThoai,
            Email = x.Email
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
        _GiangVienSearch.Search(txtSearch, filter);
    }
}