using System.Data;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;
using Svg;
namespace QuanLySinhVien.Views.Components;

public class MoDangKyHocPhan : NavBase
{
    private string ID = "MODANGKYHOCPHAN";
    
    private List<string> _listSelectionForComboBox;
    private string _title = "Tài Khoản";
    
    private CustomTable _table;
    private DataGridView dataGridView;
    private DataTable table;
    private TableLayoutPanel tableLayout;
    private Panel topCenter, botCenter;
    private CUse _cUse; 
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private NhomHocPhanController _nhomHocPhanController;
    private HocPhanController _hocPhanController;
    private GiangVienController _giangVienController;
    // private Lic
    
    List<NhomHocPhanDto> _rawData;
    List<object> _displayData;
    
    string[] _headerArray = new string[]
    {
        "Mã nhóm học phần", 
        "Tên học phần", 
        "Sĩ số",
        "Giảng viên",
        "Loại",
        "Thứ",
        "Tiết bắt đầu",
        "Số tiết",
        "Phòng",
    };// ///////
    List<string> _headerList;

    private TitleButton _insertButton;
    
    private NhomHocPhanSearch _NhomHocPhanSearch;

    private NhomHocPhanDialog _NhomHocPhanDialog;

    private List<InputFormItem> _listIFI;
    
    private List<ChiTietQuyenDto> _listAccess;
    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    public MoDangKyHocPhan(NhomQuyenDto quyen) : base(quyen)
    {
        _rawData = new List<NhomHocPhanDto>();
        _displayData = new List<object>();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
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
        _rawData = _nhomHocPhanController.GetAll();
        SetDisplayData();

        string[] columnNames = new[]
        {
            "MaNHP", 
            "TenHP", 
            "Siso",
            "TenGV",
            "Type",
            "TenPhong",
            "Thu",
            "TietBatDau",
            "SoTiet"
        };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    void SetDisplayData()
    {
        // _displayData = ConvertListCTDTToObj(_rawData);
    }
    
    List<object> ConvertListNhomHPoObj(List<NhomHocPhanDto> dtos)
    {
        List<object> list = ConvertObject.ConvertToDisplay(dtos, x => new NhomHocPhanDisplay()
            {
                MaNHP = x.MaNHP,
                // TenHP = vb
            }
        );
        return list;
    }


    void SetSearch()
    {
        // _NhomHocPhanSearch = new NhomHocPhanSearch(_rawData);
    }

    void SetAction()
    {
        _NhomHocPhanSearch.FinishSearch += dtos =>
        {
            // UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    void UpdateDataDisplay(List<NhomHocPhanDto> dtos)
    {
        // this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        // {
        //     MaTK = x.MaTK,
        //     TenDangNhap = x.TenDangNhap,
        //     TenNhomQuyen = _nhomQuyenController.GetTenQuyenByID(x.MaNQ),
        // });
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

        // _NhomHocPhanDialog = new NhomHocPhanDialog("Thêm tài khoản", DialogType.Them, list, _NhomHocPhanController,
        //     _nhomQuyenController);
        //
        // _NhomHocPhanDialog.Finish += () =>
        // {
        //     UpdateDataDisplay(_NhomHocPhanController.GetAll());
        //     this._table.UpdateData(_displayData);
        // };
        this._NhomHocPhanDialog.ShowDialog();
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
        // _NhomHocPhanDialog = new NhomHocPhanDialog("Sửa tài khoản", DialogType.Sua, list, _NhomHocPhanController,
        //     _nhomQuyenController, id);
        // _NhomHocPhanDialog.Finish += () =>
        // {
        //     UpdateDataDisplay(_NhomHocPhanController.GetAll());
            this._table.UpdateData(_displayData);
        // };
        // this._NhomHocPhanDialog.ShowDialog();
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
        // _NhomHocPhanDialog = new NhomHocPhanDialog("Chi tiết tài khoản", DialogType.ChiTiet, list, _NhomHocPhanController,
            // _nhomQuyenController, id);
        // this._NhomHocPhanDialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        // if (_NhomHocPhanController.Delete(index))
        // {
        //     MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //     UpdateDataDisplay(_NhomHocPhanController.GetAll());
        //     this._table.UpdateData(_displayData);
        // }
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
        this._NhomHocPhanSearch.Search(txtSearch, filter);
    }
}