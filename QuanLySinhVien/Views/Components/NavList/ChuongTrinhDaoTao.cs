using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components;

public class ChuongTrinhDaoTao : NavBase
{
    
    private string _title = "Chương trình đào tạo";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private ChuongTrinhDaoTaoController _ChuongTrinhDaoTaoController;
    private NganhController _nganhController;
    private ChuKyDaoTaoController _chuKyDaoTaoController;
    string[] _headerArray = new string[] { "Mã chương trình đào tạo", "Tên ngành", "Chu kỳ","Loại hình đào tạo", "Trình độ" };
    List<string> _headerList;

    private TitleButton _insertButton;

    List<ChuongTrinhDaoTaoDto> _rawData;
    List<object> _displayData;

    private ChuongTrinhDaoTaoSearch _ChuongTrinhDaoTaoSearch;

    private ChuongTrinhDaoTaoDialog _ChuongTrinhDaoTaoDialog;

    private List<InputFormItem> _listIFI;

    public ChuongTrinhDaoTao()
    {
        _rawData = new List<ChuongTrinhDaoTaoDto>();
        _displayData = new List<object>();
        _ChuongTrinhDaoTaoController = ChuongTrinhDaoTaoController.GetInstance();
        _nganhController = NganhController.GetInstance();
        _chuKyDaoTaoController = ChuKyDaoTaoController.GetInstance();
        Init();
    }

    private void Init()
    {
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
        panel.Controls.Add(_insertButton);

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
        _rawData = _ChuongTrinhDaoTaoController.GetAll();
        SetDisplayData();

        
        string[] columnNames = new[] { "MaCTDT", "TenNganh", "ChuKy", "LoaiHinhDT", "TrinhDo" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, true, true, true);
    }

    void SetDisplayData()
    {
        _displayData = ConvertListCTDTToObj(_rawData);
    }


    void SetSearch()
    {
        List<ChuongTrinhDaoTaoDisplayObject> list = ConvertObject.ConvertDtoToDto(_rawData, x => new ChuongTrinhDaoTaoDisplayObject
            {
                MaCTDT = x.MaCTDT,
                TenNganh = _nganhController.GetNganhById(x.MaNganh).TenNganh,
                ChuKy = _chuKyDaoTaoController.GetById(x.MaNganh).NamBatDau + "-" + _chuKyDaoTaoController.GetById(x.MaNganh).NamKetThuc,
                LoaiHinhDT = x.LoaiHinhDT,
                TrinhDo = x.TrinhDo,
            }
        );
        
        _ChuongTrinhDaoTaoSearch = new ChuongTrinhDaoTaoSearch(list);
    }

    void SetAction()
    {
        _ChuongTrinhDaoTaoSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplaySearch(dtos);
            this._table.UpdateData(_displayData);
        };

        // _insertButton._mouseDown += () => { Insert(); };
        // _table.OnEdit += index => { Update(index); };
        // _table.OnDetail += index => { Detail(index); };
        // _table.OnDelete += index => { Delete(index); };
    }

    void UpdateDataDisplay(List<ChuongTrinhDaoTaoDto> dtos)
    {
        _displayData = ConvertListCTDTToObj(dtos);
    }

    void UpdateDataDisplaySearch(List<ChuongTrinhDaoTaoDisplayObject> listDisplay)
    {
        _displayData = listDisplay.Cast<object>().ToList();
    }

    List<object> ConvertListCTDTToObj(List<ChuongTrinhDaoTaoDto> dtos)
    {
        List<object> list = ConvertObject.ConvertToDisplay(dtos, x => new ChuongTrinhDaoTaoDisplayObject
            {
                MaCTDT = x.MaCTDT,
                TenNganh = _nganhController.GetNganhById(x.MaNganh).TenNganh,
                ChuKy = _chuKyDaoTaoController.GetById(x.MaNganh).NamBatDau + "-" + _chuKyDaoTaoController.GetById(x.MaNganh).NamKetThuc,
                LoaiHinhDT = x.LoaiHinhDT,
                TrinhDo = x.TrinhDo,
            }
        );
        return list;
    }
    


    void Insert()
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên tài khoản", TextFieldType.NormalText),
            new InputFormItem("Mật khẩu", TextFieldType.NormalText),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);

        _ChuongTrinhDaoTaoDialog = new ChuongTrinhDaoTaoDialog("Thêm tài khoản", DialogType.Them, list, _ChuongTrinhDaoTaoController);
        
        _ChuongTrinhDaoTaoDialog.Finish += () =>
        {
            UpdateDataDisplay(_ChuongTrinhDaoTaoController.GetAll());
            
            this._table.UpdateData(_displayData);
        };
        this._ChuongTrinhDaoTaoDialog.ShowDialog();
    }

    void Update(int id)
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên tài khoản", TextFieldType.NormalText),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);
        _ChuongTrinhDaoTaoDialog = new ChuongTrinhDaoTaoDialog("Sửa tài khoản", DialogType.Sua, list, _ChuongTrinhDaoTaoController, id);
        _ChuongTrinhDaoTaoDialog.Finish += () =>
        {
            UpdateDataDisplay(_ChuongTrinhDaoTaoController.GetAll());
            this._table.UpdateData(_displayData);
        };
        this._ChuongTrinhDaoTaoDialog.ShowDialog();
    }

    void Detail(int id)
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên tài khoản", TextFieldType.NormalText),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);
        _ChuongTrinhDaoTaoDialog = new ChuongTrinhDaoTaoDialog("Chi tiết tài khoản", DialogType.ChiTiet, list, _ChuongTrinhDaoTaoController, id);
        this._ChuongTrinhDaoTaoDialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        if (_ChuongTrinhDaoTaoController.Delete(index))
        {
            MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(_ChuongTrinhDaoTaoController.GetAll());
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
        this._ChuongTrinhDaoTaoSearch.Search(txtSearch, filter);
    }
}