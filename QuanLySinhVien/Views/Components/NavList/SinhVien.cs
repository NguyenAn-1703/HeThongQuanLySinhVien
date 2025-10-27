
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components;

public class SinhVien : NavBase
{
    private string ID = "SINHVIEN";
    private string _title = "Sinh viên";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private SinhVienController _SinhVienController;
    private NhomQuyenController _nhomQuyenController;
    string[] _headerArray = new string[] { "Mã sinh viên", "Ho tên", "Ngày sinh", "Giới tính", "Lớp", "Ngành", "Trạng thái" };
    List<string> _headerList;

    private TitleButton _insertButton;

    List<SinhVienDTO> _rawData;
    List<object> _displayData;

    private SinhVienSearch _SinhVienSearch;

    private SinhVienDialog _SinhVienDialog;

    private List<InputFormItem> _listIFI;

    private List<ChiTietQuyenDto> _listAccess;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private LopController _lopController;
    private NganhController _nganhController;

    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    

    public SinhVien(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<SinhVienDTO>();
        _displayData = new List<object>();
        _SinhVienController = SinhVienController.GetInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _lopController = LopController.GetInstance();
        _nganhController = NganhController.GetInstance();
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
        _rawData = _SinhVienController.GetAll();
        SetDisplayData();

        string[] columnNames = new[] { "MaSinhVien", "TenSinhVien", "NgaySinh", "GioiTinh","TenLop", "TenNganh", "TrangThai" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }
    
    // public int MaSinhVien { get; set; }
// public string TenSinhVien { get; set; }
// public string NgaySinh { get; set; }
// public string GioiTinh { get; set; }
// public string Nganh { get; set; }
// public string TrangThai { get; set; }
// public int MaKhoaHoc { get; set; }
// public int MaLop { get; set; }
// public int? MaTk { get; set; }
// public string SdtSinhVien { get; set; }
// public string QueQuanSinhVien { get; set; }
// public string Email { get; set; }
// public string CCCD { get; set; }
// public string AnhDaiDienSinhVien { get; set; }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                MaSinhVien = x.MaSinhVien,
                TenSinhVien = x.TenSinhVien,
                NgaySinh = x.NgaySinh,
                GioiTinh = x.GioiTinh,
                TenLop = x.TenLop,
                TenNganh = x.TenNganh,
                TrangThai = x.TrangThai
            }
        );
    }


    void SetSearch()
    {
        _SinhVienSearch = new SinhVienSearch(ConvertDtoToDisplay(_rawData));
    }

    void SetAction()
    {
        _SinhVienSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    void UpdateDataDisplay(List<SinhVienDisplay> input)
    {
        this._displayData = ConvertObject.ConvertToDisplay(input, x => new
        {
            MaSinhVien = x.MaSinhVien,
            TenSinhVien = x.TenSinhVien,
            NgaySinh = x.NgaySinh,
            GioiTinh = x.GioiTinh,
            TenLop = x.TenLop,
            TenNganh = x.TenNganh,
            TrangThai = x.TrangThai
        });
    }

    void Insert()
    { 
        _SinhVienDialog = new SinhVienDialog("Thêm sinh viên", DialogType.Them);
        
        _SinhVienDialog.Finish += () =>
        {
            List<SinhVienDisplay> listSv = ConvertDtoToDisplay(_SinhVienController.GetAll());
            UpdateDataDisplay(listSv);
            this._table.UpdateData(_displayData);
        };
        this._SinhVienDialog.ShowDialog();
    }

    void Update(int id)
    {
        _SinhVienDialog = new SinhVienDialog("Sửa sinh viên", DialogType.Sua, id);
        _SinhVienDialog.Finish += () =>
        {
            List<SinhVienDisplay> listSv = ConvertDtoToDisplay(_SinhVienController.GetAll());
            UpdateDataDisplay(listSv);
            this._table.UpdateData(_displayData);
        };
        this._SinhVienDialog.ShowDialog();
    }

    void Detail(int id)
    {
        _SinhVienDialog = new SinhVienDialog("Chi tiết sinh viên", DialogType.ChiTiet, id);
        this._SinhVienDialog.ShowDialog();
    }

    void Delete(int index)
    {
        // DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        // if (select == DialogResult.No)
        // {
        //     return;
        // }
        // if (_SinhVienController.Delete(index))
        // {
        //     MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //     UpdateDataDisplay(_SinhVienController.GetAll());
        //     this._table.UpdateData(_displayData);
        // }
        // else
        // {
        //     MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        // }
    }

    List<SinhVienDisplay> ConvertDtoToDisplay(List<SinhVienDTO> input)
    {
        List<SinhVienDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new SinhVienDisplay
        {
            MaSinhVien = x.MaSinhVien,
            TenSinhVien = x.TenSinhVien,
            NgaySinh = x.NgaySinh,
            GioiTinh = x.GioiTinh,
            TenLop = _lopController.GetLopById(x.MaLop).TenLop,
            TenNganh = _nganhController.GetNganhById(_lopController.GetLopById(x.MaLop).MaNganh).TenNganh,
            TrangThai = x.TrangThai
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
        this._SinhVienSearch.Search(txtSearch, filter);
    }
}