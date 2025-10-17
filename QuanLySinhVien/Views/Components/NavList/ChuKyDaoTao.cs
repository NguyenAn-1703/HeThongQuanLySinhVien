using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList;

public class ChuKyDaoTao : NavBase
{
    private string _title = "Chu kỳ đào tạo";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private ChuKyDaoTaoController _ChuKyDaoTaoController;
    private NhomQuyenController _nhomQuyenController;
    string[] _headerArray = new string[] { "Mã chu kỳ", "Năm bắt đầu", "Năm kết thúc" };
    List<string> _headerList;

    private TitleButton _insertButton;

    List<ChuKyDaoTaoDto> _rawData;
    List<object> _displayData;

    private ChuKyDaoTaoSearch _ChuKyDaoTaoSearch;

    private ChuKyDaoTaoDialog _ChuKyDaoTaoDialog;

    private List<InputFormItem> _listIFI;

    public ChuKyDaoTao()
    {
        _rawData = new List<ChuKyDaoTaoDto>();
        _displayData = new List<object>();
        _ChuKyDaoTaoController = ChuKyDaoTaoController.GetInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
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
        _rawData = _ChuKyDaoTaoController.GetAll();
        SetDisplayData();

        string[] columnNames = new[] { "MaCKDT", "NamBatDau", "NamKetThuc" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, true, true, true);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                MaCKDT = x.MaCKDT,
                NamBatDau = x.NamBatDau,
                NamKetThuc = x.NamKetThuc
            }
        );
    }


    void SetSearch()
    {
        _ChuKyDaoTaoSearch = new ChuKyDaoTaoSearch(_rawData);
    }

    void SetAction()
    {
        _ChuKyDaoTaoSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    void UpdateDataDisplay(List<ChuKyDaoTaoDto> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaCKDT = x.MaCKDT,
            NamBatDau = x.NamBatDau,
            NamKetThuc = x.NamKetThuc
        });
    }

    void Insert()
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Năm bắt đầu", TextFieldType.NormalText),
            new InputFormItem("Năm kết thúc", TextFieldType.NormalText),
        };
        
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);

        _ChuKyDaoTaoDialog = new ChuKyDaoTaoDialog("Thêm chu kỳ đào tạo", DialogType.Them, list, _ChuKyDaoTaoController);

        _ChuKyDaoTaoDialog.Finish += () =>
        {
            UpdateDataDisplay(_ChuKyDaoTaoController.GetAll());
            this._table.UpdateData(_displayData);
        };
        this._ChuKyDaoTaoDialog.ShowDialog();
    }

    void Update(int id)
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Năm bắt đầu", TextFieldType.NormalText),
            new InputFormItem("Năm kết thúc", TextFieldType.NormalText),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);
        _ChuKyDaoTaoDialog = new ChuKyDaoTaoDialog("Sửa chu kỳ đào tạo", DialogType.Sua, list, _ChuKyDaoTaoController, id);
        _ChuKyDaoTaoDialog.Finish += () =>
        {
            UpdateDataDisplay(_ChuKyDaoTaoController.GetAll());
            this._table.UpdateData(_displayData);
        };
        this._ChuKyDaoTaoDialog.ShowDialog();
    }

    void Detail(int id)
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Năm bắt đầu", TextFieldType.NormalText),
            new InputFormItem("Năm kết thúc", TextFieldType.NormalText),
        };
        List<InputFormItem> list = new List<InputFormItem>();
        list.AddRange(arr);
        _ChuKyDaoTaoDialog = new ChuKyDaoTaoDialog("Chi tiết chu kỳ đào tạo", DialogType.ChiTiet, list,
            _ChuKyDaoTaoController, id);
        this._ChuKyDaoTaoDialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }

        if (_ChuKyDaoTaoController.Delete(index))
        {
            MessageBox.Show("Xóa chu kỳ đào tạo thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            UpdateDataDisplay(_ChuKyDaoTaoController.GetAll());
            this._table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa chu kỳ đào tạo thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._ChuKyDaoTaoSearch.Search(txtSearch, filter);
    }
}