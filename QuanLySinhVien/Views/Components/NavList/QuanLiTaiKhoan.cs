using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components;

public class QuanLiTaiKhoan : NavBase
{
    private string ID = "TAIKHOAN";
    private string _title = "Tài Khoản";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private TaiKhoanController _taiKhoanController;
    private NhomQuyenController _nhomQuyenController;
    string[] _headerArray = new string[] { "Mã tài khoản", "Tên đăng nhập", "Tên nhóm quyền" };
    List<string> _headerList;

    private TitleButton _insertButton;

    List<TaiKhoanDto> _rawData;
    List<object> _displayData;

    private TaiKhoanSearch _taiKhoanSearch;

    private TaiKhoanDialog _taiKhoanDialog;

    private List<InputFormItem> _listIFI;

    private List<ChiTietQuyenDto> _listAccess;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;

    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    

    public QuanLiTaiKhoan(NhomQuyenDto quyen) : base(quyen)
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

        TableLayoutPanel mainLayout = new TableLayoutPanel
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
        TableLayoutPanel panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
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
        TableLayoutPanel panel = new TableLayoutPanel
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
        _rawData = _taiKhoanController.GetAll();
        SetDisplayData();

        string[] columnNames = new[] { "MaTK", "TenDangNhap", "TenNhomQuyen" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                MaTK = x.MaTK,
                TenDangNhap = x.TenDangNhap,
                TenNhomQuyen = _nhomQuyenController.GetTenQuyenByID(x.MaNQ),
            }
        );
    }


    void SetSearch()
    {
        _taiKhoanSearch = new TaiKhoanSearch(_rawData);
    }

    void SetAction()
    {
        _taiKhoanSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    void UpdateDataDisplay(List<TaiKhoanDto> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaTK = x.MaTK,
            TenDangNhap = x.TenDangNhap,
            TenNhomQuyen = _nhomQuyenController.GetTenQuyenByID(x.MaNQ),
        });
    }

    void Insert()
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên đăng nhập", TextFieldType.NormalText),
            new InputFormItem("Mật khẩu", TextFieldType.NormalText),
            new InputFormItem("Loại tài khoản", TextFieldType.Combobox),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);

        _taiKhoanDialog = new TaiKhoanDialog("Thêm tài khoản", DialogType.Them, list, _taiKhoanController,
            _nhomQuyenController);
        
        _taiKhoanDialog.Finish += () =>
        {
            UpdateDataDisplay(_taiKhoanController.GetAll());
            this._table.UpdateData(_displayData);
        };
        this._taiKhoanDialog.ShowDialog();
    }

    void Update(int id)
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên tài khoản", TextFieldType.NormalText),
            new InputFormItem("Loại tài khoản", TextFieldType.Combobox),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);
        _taiKhoanDialog = new TaiKhoanDialog("Sửa tài khoản", DialogType.Sua, list, _taiKhoanController,
            _nhomQuyenController, id);
        _taiKhoanDialog.Finish += () =>
        {
            UpdateDataDisplay(_taiKhoanController.GetAll());
            this._table.UpdateData(_displayData);
        };
        this._taiKhoanDialog.ShowDialog();
    }

    void Detail(int id)
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên tài khoản", TextFieldType.NormalText),
            new InputFormItem("Loại tài khoản", TextFieldType.Combobox),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);
        _taiKhoanDialog = new TaiKhoanDialog("Chi tiết tài khoản", DialogType.ChiTiet, list, _taiKhoanController,
            _nhomQuyenController, id);
        this._taiKhoanDialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        if (_taiKhoanController.Delete(index))
        {
            MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(_taiKhoanController.GetAll());
            this._table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._taiKhoanSearch.Search(txtSearch, filter);
    }
}