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

namespace QuanLySinhVien.View.Views.Components;

public class QuanLiTaiKhoan : NavBase
{
    private readonly string[] _headerArray = new[] { "Mã tài khoản", "Tên đăng nhập", "Tên nhóm quyền" };
    private readonly string _title = "Tài Khoản";
    private readonly string ID = "TAIKHOAN";
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;
    private List<string> _headerList;

    private TitleButton _insertButton;

    private List<ChiTietQuyenDto> _listAccess;

    private List<InputFormItem> _listIFI;
    private List<string> _listSelectionForComboBox;
    private NhomQuyenController _nhomQuyenController;

    private List<TaiKhoanDto> _rawData;
    private CustomTable _table;
    private TaiKhoanController _taiKhoanController;

    private TaiKhoanDialog _taiKhoanDialog;

    private TaiKhoanSearch _taiKhoanSearch;
    private bool sua;

    private bool them;
    private bool xoa;


    public QuanLiTaiKhoan(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<TaiKhoanDto>();
        _displayData = new List<object>();
        _taiKhoanController = TaiKhoanController.getInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
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
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
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
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
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
        _rawData = _taiKhoanController.GetAll();
        SetDisplayData();

        var columnNames = new[] { "MaTK", "TenDangNhap", "TenNhomQuyen" };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                x.MaTK,
                x.TenDangNhap,
                TenNhomQuyen = _nhomQuyenController.GetTenQuyenByID(x.MaNQ)
            }
        );
    }


    private void SetSearch()
    {
        _taiKhoanSearch = new TaiKhoanSearch(_taiKhoanController.ConvertDtoToDisplay(_rawData));
    }

    private void SetAction()
    {
        _taiKhoanSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    private void UpdateDataDisplay(List<TaiKhoanDisplay> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new TaiKhoanDisplay
        {
            MaTK = x.MaTK,
            TenDangNhap = x.TenDangNhap,
            TenNhomQuyen = x.TenNhomQuyen
        });
    }

    private void Insert()
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Tên đăng nhập", TextFieldType.NormalText),
            new InputFormItem("Mật khẩu", TextFieldType.NormalText),
            new InputFormItem("Loại tài khoản", TextFieldType.Combobox),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);

        _taiKhoanDialog = new TaiKhoanDialog("Thêm tài khoản", DialogType.Them, list, _taiKhoanController,
            _nhomQuyenController);

        _taiKhoanDialog.Finish += () =>
        {
            UpdateDataDisplay(_taiKhoanController.ConvertDtoToDisplay(_taiKhoanController.GetAll()));
            _table.UpdateData(_displayData);
        };
        _taiKhoanDialog.ShowDialog();
    }

    private void Update(int id)
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Tên tài khoản", TextFieldType.NormalText),
            new InputFormItem("Loại tài khoản", TextFieldType.Combobox),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);
        _taiKhoanDialog = new TaiKhoanDialog("Sửa tài khoản", DialogType.Sua, list, _taiKhoanController,
            _nhomQuyenController, id);
        _taiKhoanDialog.Finish += () =>
        {
            UpdateDataDisplay(_taiKhoanController.ConvertDtoToDisplay(_taiKhoanController.GetAll()));
            _table.UpdateData(_displayData);
        };
        _taiKhoanDialog.ShowDialog();
    }



    private void Detail(int id)
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Tên tài khoản", TextFieldType.NormalText),
            new InputFormItem("Loại tài khoản", TextFieldType.Combobox),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);
        _taiKhoanDialog = new TaiKhoanDialog("Chi tiết tài khoản", DialogType.ChiTiet, list, _taiKhoanController,
            _nhomQuyenController, id);
        _taiKhoanDialog.ShowDialog();
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        if (_taiKhoanController.Delete(index))
        {
            MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            UpdateDataDisplay(_taiKhoanController.ConvertDtoToDisplay(_taiKhoanController.GetAll()));
            _table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _taiKhoanSearch.Search(txtSearch, filter);
    }
}