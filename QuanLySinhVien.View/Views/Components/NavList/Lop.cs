using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.utils;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class QuanLiLop : NavBase
{
    private readonly string[] _headerArray = new[] { "Mã lớp", "Tên lớp", "Tên giảng viên", "Tên ngành" };
    private List<string> _headerList;
    private readonly string _title = "Lớp";
    private readonly string ID = "Lop";
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;

    private GiangVienController _giangVienController;


    private TitleButton _insertButton;

    private List<ChiTietQuyenDto> _listAccess;

    private List<InputFormItem> _listIFI;
    private List<string> _listSelectionForComboBox;
    private LopController _LopController;

    private LopDialog _LopDialog;

    private LopSearch _LopSearch;
    private NganhController _nganhController;
    private NhomQuyenController _nhomQuyenController;

    private List<LopDto> _rawData;
    private CustomTable _table;
    private bool sua;


    private bool them;
    private bool xoa;


    public QuanLiLop(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<LopDto>();
        _displayData = new List<object>();
        _LopController = LopController.GetInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _giangVienController = GiangVienController.GetInstance();
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
        _rawData = _LopController.GetAll();
        SetDisplayData();

        var columnNames = new[] { "MaLop", "TenLop", "TenGV", "TenNganh" };
        columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }


    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                x.MaLop,
                x.TenLop,
                _giangVienController.GetById(x.MaGV).TenGV,
                _nganhController.GetNganhById(x.MaNganh).TenNganh
            }
        );
    }


    private void SetSearch()
    {
        _LopSearch = new LopSearch(convertToSearch(_rawData));
    }

    private List<LopDisplay> convertToSearch(List<LopDto> input)
    {
        List<LopDisplay> list = ConvertObject.ConvertDtoToDto(input, x => new LopDisplay
        {
            MaLop = x.MaLop,
            TenLop = x.TenLop,
            TenGV = _giangVienController.GetById(x.MaGV).TenGV,
            TenNganh = _nganhController.GetNganhById(x.MaNganh).TenNganh
        });
        return list;
    }

    private void SetAction()
    {
        _LopSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };

        _exportExBtn._mouseDown += () => ExportExcel(convertToSearch(_rawData));
    }
    
    void ExportExcel(List<LopDisplay> list)
    {
        var header = new Dictionary<string, string>();
        for (int i = 0; i < _headerArray.Length; i++)
        {
            header.Add(columnNamesList[i], _headerArray[i]);
        }
        
        ExcelExporter.ExportToExcel(list, "sheet1","Danh sách lớp", header);
    }

    private void UpdateDataDisplay(List<LopDisplay> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaLop,
            x.TenLop,
            x.TenGV,
            x.TenNganh
        });
    }

    private void Insert()
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Tên lớp", TextFieldType.NormalText),
            new InputFormItem("Tên giảng viên", TextFieldType.Combobox),
            new InputFormItem("Tên ngành", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);

        _LopDialog = new LopDialog("Thêm lớp", DialogType.Them, list, _LopController);

        _LopDialog.Finish += () =>
        {
            UpdateDataDisplay(convertToSearch(_LopController.GetAll()));
            _table.UpdateData(_displayData);
        };
        _LopDialog.ShowDialog();
    }

    private void Update(int id)
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Tên lớp", TextFieldType.NormalText),
            new InputFormItem("Tên giảng viên", TextFieldType.Combobox),
            new InputFormItem("Tên ngành", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);
        _LopDialog = new LopDialog("Sửa lớp", DialogType.Sua, list, _LopController, id);
        _LopDialog.Finish += () =>
        {
            UpdateDataDisplay(convertToSearch(_LopController.GetAll()));
            _table.UpdateData(_displayData);
        };
        _LopDialog.ShowDialog();
    }

    private void Detail(int id)
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Tên lớp", TextFieldType.NormalText),
            new InputFormItem("Tên giảng viên", TextFieldType.Combobox),
            new InputFormItem("Tên ngành", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);
        _LopDialog = new LopDialog("Chi tiết lớp", DialogType.ChiTiet, list, _LopController, id);
        _LopDialog.ShowDialog();
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        if (_LopController.Delete(index))
        {
            MessageBox.Show("Xóa lớp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(convertToSearch(_LopController.GetAll()));
            _table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa lớp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _LopSearch.Search(txtSearch, filter);
    }
}