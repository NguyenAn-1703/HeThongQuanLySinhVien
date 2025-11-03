using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.Views.Components;

public class ChuongTrinhDaoTao : NavBase
{
    private readonly string[] _headerArray = new[]
        { "Mã chương trình đào tạo", "Tên ngành", "Chu kỳ", "Loại hình đào tạo", "Trình độ" };

    private readonly string _title = "Chương trình đào tạo";
    private readonly string ID = "CHUONGTRINHDAOTAO";

    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private ChuKyDaoTaoController _chuKyDaoTaoController;
    private ChuongTrinhDaoTao_HocPhanController _chuongTrinhDaoTao_HocPhanController;
    private ChuongTrinhDaoTaoController _ChuongTrinhDaoTaoController;

    private ChuongTrinhDaoTaoDialog _ChuongTrinhDaoTaoDialog;

    private ChuongTrinhDaoTaoSearch _ChuongTrinhDaoTaoSearch;
    private List<object> _displayData;
    private List<string> _headerList;

    private TitleButton _insertButton;

    private List<ChiTietQuyenDto> _listAccess;

    private List<InputFormItem> _listIFI;
    private List<string> _listSelectionForComboBox;
    private NganhController _nganhController;

    private List<ChuongTrinhDaoTaoDto> _rawData;
    private CustomTable _table;
    private bool sua;
    private bool them;
    private bool xoa;

    public ChuongTrinhDaoTao(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _rawData = new List<ChuongTrinhDaoTaoDto>();
        _displayData = new List<object>();
        _ChuongTrinhDaoTaoController = ChuongTrinhDaoTaoController.GetInstance();
        _nganhController = NganhController.GetInstance();
        _chuKyDaoTaoController = ChuKyDaoTaoController.GetInstance();
        _chuongTrinhDaoTao_HocPhanController = ChuongTrinhDaoTao_HocPhanController.GetInstance();
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
        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        if (them) panel.Controls.Add(_insertButton);

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


    private void SetDataTableFromDb()
    {
        _rawData = _ChuongTrinhDaoTaoController.GetAll();
        SetDisplayData();


        var columnNames = new[] { "MaCTDT", "TenNganh", "ChuKy", "LoaiHinhDT", "TrinhDo" };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertListCTDTToObj(_rawData);
    }


    private void SetSearch()
    {
        List<ChuongTrinhDaoTaoDisplayObject> list = ConvertObject.ConvertDtoToDto(_rawData, x =>
            new ChuongTrinhDaoTaoDisplayObject
            {
                MaCTDT = x.MaCTDT,
                TenNganh = _nganhController.GetNganhById(x.MaNganh).TenNganh,
                ChuKy = _chuKyDaoTaoController.GetById(x.MaCKDT).NamBatDau + "-" +
                        _chuKyDaoTaoController.GetById(x.MaCKDT).NamKetThuc,
                LoaiHinhDT = x.LoaiHinhDT,
                TrinhDo = x.TrinhDo
            }
        );

        _ChuongTrinhDaoTaoSearch = new ChuongTrinhDaoTaoSearch(list);
    }

    private void SetAction()
    {
        _ChuongTrinhDaoTaoSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplaySearch(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    private void UpdateDataDisplay(List<ChuongTrinhDaoTaoDto> dtos)
    {
        _displayData = ConvertListCTDTToObj(dtos);
    }

    private void UpdateDataDisplaySearch(List<ChuongTrinhDaoTaoDisplayObject> listDisplay)
    {
        _displayData = listDisplay.Cast<object>().ToList();
    }

    private List<object> ConvertListCTDTToObj(List<ChuongTrinhDaoTaoDto> dtos)
    {
        List<object> list = ConvertObject.ConvertToDisplay(dtos, x => new ChuongTrinhDaoTaoDisplayObject
            {
                MaCTDT = x.MaCTDT,
                TenNganh = _nganhController.GetNganhById(x.MaNganh).TenNganh,
                ChuKy = _chuKyDaoTaoController.GetById(x.MaCKDT).NamBatDau + "-" +
                        _chuKyDaoTaoController.GetById(x.MaCKDT).NamKetThuc,
                LoaiHinhDT = x.LoaiHinhDT,
                TrinhDo = x.TrinhDo
            }
        );
        return list;
    }


    private void Insert()
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Ngành", TextFieldType.Combobox),
            new InputFormItem("Chu kỳ", TextFieldType.Combobox),
            new InputFormItem("Loại hình đào tạo", TextFieldType.Combobox),
            new InputFormItem("Trình độ đào tạo", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);

        _ChuongTrinhDaoTaoDialog = new ChuongTrinhDaoTaoDialog("Thêm chương trình đào tạo", DialogType.Them, list,
            _ChuongTrinhDaoTaoController);

        _ChuongTrinhDaoTaoDialog.Finish += () =>
        {
            UpdateDataDisplay(_ChuongTrinhDaoTaoController.GetAll());

            _table.UpdateData(_displayData);
        };
        _ChuongTrinhDaoTaoDialog.ShowDialog();
    }

    private void Update(int id)
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Ngành", TextFieldType.Combobox),
            new InputFormItem("Chu kỳ", TextFieldType.Combobox),
            new InputFormItem("Loại hình đào tạo", TextFieldType.Combobox),
            new InputFormItem("Trình độ đào tạo", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);
        _ChuongTrinhDaoTaoDialog = new ChuongTrinhDaoTaoDialog("Sửa chương trình đào tạo", DialogType.Sua, list,
            _ChuongTrinhDaoTaoController, id);
        _ChuongTrinhDaoTaoDialog.Finish += () =>
        {
            UpdateDataDisplay(_ChuongTrinhDaoTaoController.GetAll());
            _table.UpdateData(_displayData);
        };
        _ChuongTrinhDaoTaoDialog.ShowDialog();
    }

    private void Detail(int id)
    {
        InputFormItem[] arr = new[]
        {
            new InputFormItem("Ngành", TextFieldType.Combobox),
            new InputFormItem("Chu kỳ", TextFieldType.Combobox),
            new InputFormItem("Loại hình đào tạo", TextFieldType.Combobox),
            new InputFormItem("Trình độ đào tạo", TextFieldType.Combobox)
        };
        List<InputFormItem> list = new();
        list.AddRange(arr);
        _ChuongTrinhDaoTaoDialog = new ChuongTrinhDaoTaoDialog("Chi tiết chương trình đào tạo", DialogType.ChiTiet,
            list, _ChuongTrinhDaoTaoController, id);
        _ChuongTrinhDaoTaoDialog.ShowDialog();
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        if (_ChuongTrinhDaoTaoController.Delete(index) && _chuongTrinhDaoTao_HocPhanController.DeleteAllByMaCTDT(index))
        {
            MessageBox.Show("Xóa chương trình đào tạo thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            UpdateDataDisplay(_ChuongTrinhDaoTaoController.GetAll());
            _table.UpdateData(_displayData);
        }

        else
        {
            MessageBox.Show("Xóa chương trình đào tạo thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _ChuongTrinhDaoTaoSearch.Search(txtSearch, filter);
    }
}