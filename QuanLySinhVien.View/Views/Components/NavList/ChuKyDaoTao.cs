using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class ChuKyDaoTao : NavBase
{
    private readonly string[] _headerArray = new[] { "Mã chu kỳ", "Năm bắt đầu", "Năm kết thúc" };
    private readonly string _title = "Chu kỳ đào tạo";
    private readonly string ID = "CHUKYDAOTAO";

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private ChuKyDaoTaoController _ChuKyDaoTaoController;

    private ChuKyDaoTaoDialog _ChuKyDaoTaoDialog;

    private ChuKyDaoTaoSearch _ChuKyDaoTaoSearch;
    private List<object> _displayData;
    private List<string> _headerList;

    private TitleButton _insertButton;

    private List<ChiTietQuyenDto> _listAccess;

    private List<InputFormItem> _listIFI;
    private List<string> _listSelectionForComboBox;
    private NhomQuyenController _nhomQuyenController;

    private List<ChuKyDaoTaoDto> _rawData;
    private CustomTable _table;
    private bool sua;
    private bool them;
    private bool xoa;

    public ChuKyDaoTao(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _rawData = new List<ChuKyDaoTaoDto>();
        _displayData = new List<object>();
        _ChuKyDaoTaoController = ChuKyDaoTaoController.GetInstance();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        Init();
    }

    private void Init()
    {
        CheckQuyen();
        Dock = DockStyle.Fill;

        var mainLayout = new TableLayoutPanel
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
        var panel = new TableLayoutPanel
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
        if (them) panel.Controls.Add(_insertButton);

        return panel;
    }

    private Panel Bottom()
    {
        var panel = new TableLayoutPanel
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
        _rawData = _ChuKyDaoTaoController.GetAll();
        SetDisplayData();

        var columnNames = new[] { "MaCKDT", "NamBatDau", "NamKetThuc" };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                x.MaCKDT,
                x.NamBatDau,
                x.NamKetThuc
            }
        );
    }


    private void SetSearch()
    {
        _ChuKyDaoTaoSearch = new ChuKyDaoTaoSearch(_rawData);
    }

    private void SetAction()
    {
        _ChuKyDaoTaoSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    private void UpdateDataDisplay(List<ChuKyDaoTaoDto> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaCKDT,
            x.NamBatDau,
            x.NamKetThuc
        });
    }

    private void Insert()
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Năm bắt đầu", TextFieldType.NormalText),
            new InputFormItem("Năm kết thúc", TextFieldType.NormalText)
        };

        List<InputFormItem> list = new();
        list.AddRange(arr);

        _ChuKyDaoTaoDialog =
            new ChuKyDaoTaoDialog("Thêm chu kỳ đào tạo", DialogType.Them, list, _ChuKyDaoTaoController);

        _ChuKyDaoTaoDialog.Finish += () =>
        {
            UpdateDataDisplay(_ChuKyDaoTaoController.GetAll());
            _table.UpdateData(_displayData);
        };
        _ChuKyDaoTaoDialog.ShowDialog();
    }

    private void Update(int id)
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Năm bắt đầu", TextFieldType.NormalText),
            new InputFormItem("Năm kết thúc", TextFieldType.NormalText)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);
        _ChuKyDaoTaoDialog =
            new ChuKyDaoTaoDialog("Sửa chu kỳ đào tạo", DialogType.Sua, list, _ChuKyDaoTaoController, id);
        _ChuKyDaoTaoDialog.Finish += () =>
        {
            UpdateDataDisplay(_ChuKyDaoTaoController.GetAll());
            _table.UpdateData(_displayData);
        };
        _ChuKyDaoTaoDialog.ShowDialog();
    }

    private void Detail(int id)
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Năm bắt đầu", TextFieldType.NormalText),
            new InputFormItem("Năm kết thúc", TextFieldType.NormalText)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);
        _ChuKyDaoTaoDialog = new ChuKyDaoTaoDialog("Chi tiết chu kỳ đào tạo", DialogType.ChiTiet, list,
            _ChuKyDaoTaoController, id);
        _ChuKyDaoTaoDialog.ShowDialog();
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;

        if (_ChuKyDaoTaoController.Delete(index))
        {
            MessageBox.Show("Xóa chu kỳ đào tạo thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            UpdateDataDisplay(_ChuKyDaoTaoController.GetAll());
            _table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa chu kỳ đào tạo thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _ChuKyDaoTaoSearch.Search(txtSearch, filter);
    }
}