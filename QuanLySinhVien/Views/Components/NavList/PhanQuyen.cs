using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components;

public class PhanQuyen : NavBase
{
    private string ID = "PHANQUYEN";
    private string _title = "Phân quyền";
    private List<string> _listSelectionForComboBox;
    private CustomTable _table;
    private NhomQuyenController _nhomQuyenController;
    private ChiTietQuyenController _chiTietQuyenController;
    string[] _headerArray = new string[] { "Mã nhóm quyền", "Tên nhóm quyền" };
    List<string> _headerList;

    private TitleButton _insertButton;

    List<NhomQuyenDto> _rawData;
    List<object> _displayData;

    private NhomQuyenSearch _nhomQuyenSearch;
    
    private PhanQuyenDialog _phanQuyenDialog;
    private ChucNangController _chucNangController;
    
    private List<ChiTietQuyenDto> _listAccess;
    
    private bool them = false;
    private bool sua = false;
    private bool xoa = false;


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
        if (them)
        {
            panel.Controls.Add(_insertButton);
        }
        

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
        _rawData = _nhomQuyenController.GetAll();
        SetDisplayData();

        string[] columnNames = new[] { "MaNQ", "TenNhomQuyen"};
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, sua || xoa, sua, xoa);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
            {
                MaNQ = x.MaNQ,
                TenNhomQuyen = x.TenNhomQuyen,
            }
        );
    }


    void SetSearch()
    {
        _nhomQuyenSearch = new NhomQuyenSearch(_rawData);
    }

    void SetAction()
    {
        _nhomQuyenSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }

    void UpdateDataDisplay(List<NhomQuyenDto> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaNQ = x.MaNQ,
            TenNhomQuyen = x.TenNhomQuyen,
        });
    }

    void Insert()
    {
        _phanQuyenDialog = new PhanQuyenDialog("Thêm nhóm quyền",DialogType.Them);
        _phanQuyenDialog.Finish += () =>
        {
            UpdateDataDisplay(_nhomQuyenController.GetAll());
            this._table.UpdateData(_displayData);
        };
        _phanQuyenDialog.ShowDialog();
    }

    void Update(int id)
    {
        _phanQuyenDialog = new PhanQuyenDialog("Sửa nhóm quyền", DialogType.Sua, id);
        _phanQuyenDialog.Finish += () =>
        {
            UpdateDataDisplay(_nhomQuyenController.GetAll());
            this._table.UpdateData(_displayData);
        };
        _phanQuyenDialog.ShowDialog();
    }

    void Detail(int id)
    {
        _phanQuyenDialog = new PhanQuyenDialog("Chi tiết nhóm quyền", DialogType.ChiTiet, id);
        _phanQuyenDialog.ShowDialog();
    }

    void Delete(int index)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        if (_nhomQuyenController.Delete(index) && _chiTietQuyenController.DeleteAllCTQ(index))
        {
            MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(_nhomQuyenController.GetAll());
            this._table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
        
    

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    
    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._nhomQuyenSearch.Search(txtSearch, filter);
    }
}