using System.Data;
using System.Runtime.InteropServices.JavaScript;
using QuanLyGiangVien.Views.Components.CommonUse;
using QuanLyGiangVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.GetFont;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;
using Svg;

namespace QuanLyGiangVien.Views.Components;

public class GiangVien : NavBase
{
    private string ID = "GiangVien";
    private string _title = "Giảng viên";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private GiangVienController _GiangVienController;
    private NhomQuyenController _nhomQuyenController;
    private KhoaController _khoaController;
    string[] _headerArray = new string[] { "Mã giảng viên", "Tên giảng viên", "Khoa", "Ngày sinh", "Giới tính", "Số điện thoại", "Email" };
    List<string> _headerList;

    private TitleButton _insertButton;

    List<GiangVienDto> _rawData;
    List<object> _displayData;

    private GiangVienSearch _GiangVienSearch;

    private GiangVienDialog _GiangVienDialog;

    private List<InputFormItem> _listIFI;

    private List<ChiTietQuyenDto> _listAccess;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private LopController _lopController;
    private NganhController _nganhController;

    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    

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
        _insertButton._label.Font = GetFont.GetMainFont(12, FontType.ExtraBold);
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
            Font = GetFont.GetMainFont(17, FontType.ExtraBold),
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
        _rawData = _GiangVienController.GetAll();
        SetDisplayData();

        string[] columnNames = new[] { "MaGV", "TenGV", "TenKhoa", "NgaySinhGV","GioiTinhGV", "SoDienThoai", "Email" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                MaGV = x.MaGV,
                TenGV = x.TenGV,
                TenKhoa = x.TenKhoa,
                NgaySinhGV = x.NgaySinhGV,
                GioiTinhGV = x.GioiTinhGV,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
            }
        );
    }


    void SetSearch()
    {
        _GiangVienSearch = new GiangVienSearch(ConvertDtoToDisplay(_rawData));
    }

    void SetAction()
    {
        _GiangVienSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    void UpdateDataDisplay(List<GiangVienDisplay> input)
    {
        this._displayData = ConvertObject.ConvertToDisplay(input, x => new
        {
            MaGV = x.MaGV,
            TenGV = x.TenGV,
            TenKhoa = x.TenKhoa,
            NgaySinhGV = x.NgaySinhGV,
            GioiTinhGV = x.GioiTinhGV,
            SoDienThoai = x.SoDienThoai,
            Email = x.Email,
        });
    }

    void Insert()
    { 
        _GiangVienDialog = new GiangVienDialog("Thêm giảng viên", DialogType.Them);
        
        _GiangVienDialog.Finish += () =>
        {
            List<GiangVienDisplay> listSv = ConvertDtoToDisplay(_GiangVienController.GetAll());
            UpdateDataDisplay(listSv);
            this._table.UpdateData(_displayData);
        };
        this._GiangVienDialog.ShowDialog();
    }

    void Update(int id)
    {
        _GiangVienDialog = new GiangVienDialog("Sửa giảng viên", DialogType.Sua, id);
        _GiangVienDialog.Finish += () =>
        {
            List<GiangVienDisplay> listSv = ConvertDtoToDisplay(_GiangVienController.GetAll());
            UpdateDataDisplay(listSv);
            this._table.UpdateData(_displayData);
        };
        this._GiangVienDialog.ShowDialog();
    }

    void Detail(int id)
    {
        _GiangVienDialog = new GiangVienDialog("Chi tiết giảng viên", DialogType.ChiTiet, id);
        this._GiangVienDialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }

        _GiangVienController.SoftDeleteById(index);
            MessageBox.Show("Xóa giảng viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(ConvertDtoToDisplay(_GiangVienController.GetAll()));
            this._table.UpdateData(_displayData);
    }

    List<GiangVienDisplay> ConvertDtoToDisplay(List<GiangVienDto> input)
    {
        List<GiangVienDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new GiangVienDisplay
        {
            MaGV = x.MaGV,
            TenGV = x.TenGV,
            TenKhoa = _khoaController.GetKhoaById(x.MaKhoa).TenKhoa,
            NgaySinhGV = x.NgaySinhGV,
            GioiTinhGV = x.GioiTinhGV,
            SoDienThoai = x.SoDienThoai,
            Email = x.Email,
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
        this._GiangVienSearch.Search(txtSearch, filter);
    }
}