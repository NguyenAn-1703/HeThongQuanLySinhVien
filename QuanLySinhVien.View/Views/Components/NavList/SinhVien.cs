using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.utils;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.NavList;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.View.Views.Components;

public class SinhVien : NavBase
{
    private readonly string[] _headerArray = new[]
        { "Mã sinh viên", "Ho tên", "Ngày sinh", "Giới tính", "Lớp", "Ngành", "Trạng thái" };

    private readonly string _title = "Sinh viên";
    private readonly string ID = "SINHVIEN";
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;
    private List<string> _headerList;

    private TitleButton _insertButton;

    private List<ChiTietQuyenDto> _listAccess;

    private List<InputFormItem> _listIFI;
    private List<string> _listSelectionForComboBox;
    private LopController _lopController;
    private NganhController _nganhController;
    private NhomQuyenController _nhomQuyenController;

    private List<SinhVienDTO> _rawData;
    private SinhVienController _SinhVienController;

    private SinhVienDialog _SinhVienDialog;

    private SinhVienSearch _SinhVienSearch;
    private CustomTable _table;
    private bool sua;

    private bool them;
    private bool xoa;


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


    private TitleButton _exportExBtn;
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

        MyTLP container = new MyTLP
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            ColumnCount = 2
        };
        
        _exportExBtn = new TitleButton("Xuất", "exportexcel.svg")
        {
            BackgroundColor = MyColor.Green,
            HoverColor = MyColor.GreenHover,
            SelectColor = MyColor.GreenClick,
        };
        _exportExBtn.SetBackGroundColor(MyColor.Green);
        _exportExBtn.Margin = new Padding(3, 3, 10, 3);
        _exportExBtn._label.Font = GetFont.GetFont.GetMainFont(12, FontType.Bold);
        _exportExBtn.Anchor = AnchorStyles.Right;

        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        container.Controls.Add(_exportExBtn);
        if (them) container.Controls.Add(_insertButton);

        panel.Controls.Add(container);

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

    private List<string> columnNamesList;
    private void SetDataTableFromDb()
    {
        _rawData = _SinhVienController.GetAll();
        SetDisplayData();

        var columnNames = new[]
            { "MaSinhVien", "TenSinhVien", "NgaySinh", "GioiTinh", "TenLop", "TenNganh", "TrangThai" };
        columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }


    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_SinhVienController.ConvertDtoToDisplay(_rawData), x => new
            {
                x.MaSinhVien,
                x.TenSinhVien,
                x.NgaySinh,
                x.GioiTinh,
                x.TenLop,
                x.TenNganh,
                x.TrangThai
            }
        );
    }


    private void SetSearch()
    {
        _SinhVienSearch = new SinhVienSearch(_SinhVienController.ConvertDtoToDisplay(_rawData));
    }

    private void SetAction()
    {
        _SinhVienSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };

        _exportExBtn._mouseDown += () => ExportExcel(_SinhVienController.ConvertDtoToDisplay(_rawData));
    }

    void ExportExcel(List<SinhVienDisplay> list)
    {
        var header = new Dictionary<string, string>();
        for (int i = 0; i < _headerArray.Length; i++)
        {
            header.Add(columnNamesList[i], _headerArray[i]);
        }
        
        ExcelExporter.ExportToExcel(list, "sheet1","Danh sách sinh viên", header);
    }

    private void UpdateDataDisplay(List<SinhVienDisplay> input)
    {
        _displayData = ConvertObject.ConvertToDisplay(input, x => new
        {
            x.MaSinhVien,
            x.TenSinhVien,
            x.NgaySinh,
            x.GioiTinh,
            x.TenLop,
            x.TenNganh,
            x.TrangThai
        });
    }

    private void Insert()
    {
        _SinhVienDialog = new SinhVienDialog("Thêm sinh viên", DialogType.Them);

        _SinhVienDialog.Finish += () =>
        {
            var listSv = _SinhVienController.ConvertDtoToDisplay(_SinhVienController.GetAll());
            UpdateDataDisplay(listSv);
            _table.UpdateData(_displayData);
        };
        _SinhVienDialog.ShowDialog();
    }

    private void Update(int id)
    {
        _SinhVienDialog = new SinhVienDialog("Sửa sinh viên", DialogType.Sua, id);
        _SinhVienDialog.Finish += () =>
        {
            var listSv = _SinhVienController.ConvertDtoToDisplay(_SinhVienController.GetAll());
            UpdateDataDisplay(listSv);
            _table.UpdateData(_displayData);
        };
        _SinhVienDialog.ShowDialog();
    }

    private void Detail(int id)
    {
        _SinhVienDialog = new SinhVienDialog("Chi tiết sinh viên", DialogType.ChiTiet, id);
        _SinhVienDialog.ShowDialog();
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        if (_SinhVienController.DeleteSinhVien(index))
        {
            MessageBox.Show("Xóa sinh viên thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            UpdateDataDisplay(_SinhVienController.ConvertDtoToDisplay(_SinhVienController.GetAll()));
            _table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _SinhVienSearch.Search(txtSearch, filter);
    }
}