using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList;

public class QuanLiLop : NavBase
{
    private string ID = "Lop";
    private string _title = "Lớp";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private LopController _LopController;
    private NhomQuyenController _nhomQuyenController;
    string[] _headerArray = new string[] { "Mã lớp", "Tên lớp", "Tên giảng viên", "Tên ngành"};
    List<string> _headerList;

    private TitleButton _insertButton;

    List<LopDto> _rawData;
    List<object> _displayData;

    private LopSearch _LopSearch;

    private LopDialog _LopDialog;

    private List<InputFormItem> _listIFI;

    private GiangVienController _giangVienController;
    private NganhController _nganhController;

    private List<ChiTietQuyenDto> _listAccess;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    

    private bool them = false;
    private bool sua = false;
    private bool xoa = false;
    

    public QuanLiLop(NhomQuyenDto quyen) : base(quyen)
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
        _rawData = _LopController.GetAll();
        SetDisplayData();

        string[] columnNames = new[] { "MaLop", "TenLop", "TenGV", "TenNganh" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    
    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                MaLop = x.MaLop,
                TenLop = x.TenLop,
                TenGV = _giangVienController.GetById(x.MaGV).TenGV,
                TenNganh = _nganhController.GetNganhById(x.MaNganh).TenNganh
            }
        );
    }


    void SetSearch()
    {
        _LopSearch = new LopSearch(convertToSearch(_rawData));
    }

    List<LopDisplay> convertToSearch(List<LopDto> input)
    {
        List<LopDisplay> list = ConvertObject.ConvertDtoToDto(input , x => new LopDisplay
        {
            MaLop = x.MaLop,
            TenLop = x.TenLop,
            TenGV = _giangVienController.GetById(x.MaGV).TenGV,
            TenNganh = _nganhController.GetNganhById(x.MaNganh).TenNganh
        });
        return list;
    }

    void SetAction()
    {
        _LopSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    void UpdateDataDisplay(List<LopDisplay> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaLop = x.MaLop,
            TenLop = x.TenLop,
            TenGV = x.TenGV,
            TenNganh = x.TenNganh
        });
    }

    void Insert()
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên lớp", TextFieldType.NormalText),
            new InputFormItem("Tên giảng viên", TextFieldType.Combobox),
            new InputFormItem("Tên ngành", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);

        _LopDialog = new LopDialog("Thêm lớp", DialogType.Them, list, _LopController);
        
        _LopDialog.Finish += () =>
        {
            UpdateDataDisplay(convertToSearch(_LopController.GetAll()));
            this._table.UpdateData(_displayData);
        };
        this._LopDialog.ShowDialog();
    }

    void Update(int id)
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên lớp", TextFieldType.NormalText),
            new InputFormItem("Tên giảng viên", TextFieldType.Combobox),
            new InputFormItem("Tên ngành", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);
        _LopDialog = new LopDialog("Sửa lớp", DialogType.Sua, list, _LopController, id);
        _LopDialog.Finish += () =>
        {
            UpdateDataDisplay(convertToSearch(_LopController.GetAll()));
            this._table.UpdateData(_displayData);
        };
        this._LopDialog.ShowDialog();
    }

    void Detail(int id)
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên lớp", TextFieldType.NormalText),
            new InputFormItem("Tên giảng viên", TextFieldType.Combobox),
            new InputFormItem("Tên ngành", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);
        _LopDialog = new LopDialog("Chi tiết lớp", DialogType.ChiTiet, list, _LopController, id);
        this._LopDialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        if (_LopController.Delete(index))
        {
            MessageBox.Show("Xóa lớp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(convertToSearch(_LopController.GetAll()));
            this._table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa lớp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._LopSearch.Search(txtSearch, filter);
    }
}