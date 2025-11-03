using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.NavList;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;

namespace QuanLySinhVien.View.Views.Components;

public class PhanQuyen : NavBase
{
    private readonly string[] _headerArray = new[] { "Mã nhóm quyền", "Tên nhóm quyền" };
    private readonly string _title = "Phân quyền";
    private readonly string ID = "PHANQUYEN";
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<object> _displayData;
    private List<string> _headerList;

    private TitleButton _insertButton;

    private List<ChiTietQuyenDto> _listAccess;
    private List<string> _listSelectionForComboBox;
    private NhomQuyenController _nhomQuyenController;

    private NhomQuyenSearch _nhomQuyenSearch;

    private PhanQuyenDialog _phanQuyenDialog;

    private List<NhomQuyenDto> _rawData;
    private CustomTable _table;
    private bool sua;

    private bool them;
    private bool xoa;


    public PhanQuyen(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _rawData = new List<NhomQuyenDto>();
        _displayData = new List<object>();
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
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
        _rawData = _nhomQuyenController.GetAll();
        SetDisplayData();

        var columnNames = new[] { "MaNQ", "TenNhomQuyen" };
        var columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    private void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                x.MaNQ, x.TenNhomQuyen
            }
        );
    }


    private void SetSearch()
    {
        _nhomQuyenSearch = new NhomQuyenSearch(_rawData);
    }

    private void SetAction()
    {
        _nhomQuyenSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            _table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    private void UpdateDataDisplay(List<NhomQuyenDto> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaNQ, x.TenNhomQuyen
        });
    }

    private void Insert()
    {
        _phanQuyenDialog = new PhanQuyenDialog("Thêm nhóm quyền", DialogType.Them);
        _phanQuyenDialog.Finish += () =>
        {
            UpdateDataDisplay(_nhomQuyenController.GetAll());
            _table.UpdateData(_displayData);
        };
        _phanQuyenDialog.ShowDialog();
    }

    private void Update(int id)
    {
        _phanQuyenDialog = new PhanQuyenDialog("Sửa nhóm quyền", DialogType.Sua, id);
        _phanQuyenDialog.Finish += () =>
        {
            UpdateDataDisplay(_nhomQuyenController.GetAll());
            _table.UpdateData(_displayData);
        };
        _phanQuyenDialog.ShowDialog();
    }

    private void Detail(int id)
    {
        _phanQuyenDialog = new PhanQuyenDialog("Chi tiết nhóm quyền", DialogType.ChiTiet, id);
        _phanQuyenDialog.ShowDialog();
    }

    private void Delete(int index)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        if (_nhomQuyenController.Delete(index) && _chiTietQuyenController.DeleteAllCTQ(index))
        {
            MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(_nhomQuyenController.GetAll());
            _table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        _nhomQuyenSearch.Search(txtSearch, filter);
    }
}