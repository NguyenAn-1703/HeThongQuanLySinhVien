using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Components;

public class NganhPanel : NavBase
{
    private string _title = "Ngành";
    
    private string ID = "NGANH";
  

    private CustomTable _table;
    private List<string> _listSelectionForComboBox;

    private NganhController _nganhController;
    private KhoaController _khoaController;
    string[] _headerArray = new[] { "Mã ngành", "Tên ngành", "Tên khoa" };
    List<string> _headerList;

    private TitleButton _insertButton;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    
    private List<ChiTietQuyenDto> _listAccess;
    private bool them = false;
    private bool sua = false;
    private bool xoa = false;

    List<NganhDto> _rawData;
    List<object> _displayData;

    private NganhSearch _nganhSearch;

    private NganhDialog _nganhDialog;

        
    public NganhPanel(NhomQuyenDto quyen) : base(quyen)
    {
        _rawData = new List<NganhDto>();
        _displayData = new List<object>();
        _nganhController = NganhController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
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
        Panel panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Padding = new Padding(20)
        };
        
        SetCombobox();

        SetDataTableFromDb();

        SetSearch();

        SetAction();

        panel.Controls.Add(_table);

        return panel;
    }

    Label getTitle()
    {
        Label titlePnl = new Label
        {
            Text = "Ngành",
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true,
        };
        return titlePnl;
    }

    void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }

    void SetDataTableFromDb()
    {
        _rawData = _nganhController.GetAll();
        SetDisplayData();

        string[] columnNames = new[] { "MaNganh", "TenNganh", "TenKhoa" };
        List<string> columnNamesList = columnNames.ToList();

        _table = new CustomTable(_headerList, columnNamesList, _displayData, true, true, true);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, dto => new
        {
            MaNganh = dto.MaNganh,
            TenNganh = dto.TenNganh,
            TenKhoa = _khoaController.GetKhoaById(dto.MaKhoa).TenKhoa
        });
    }

    void SetSearch()
    {
        _nganhSearch = new NganhSearch(_rawData);
    }

    void SetAction()
    {
        _nganhSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };
        
        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += index => { Update(index); };
        _table.OnDetail += index => { Detail(index); };
        _table.OnDelete += index => { Delete(index); };
    }
    
    void UpdateDataDisplay(List<NganhDto> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaNganh = x.MaNganh,
            TenNganh = x.TenNganh,
            TenKhoa = _khoaController.GetKhoaById(x.MaKhoa).TenKhoa
        });
    }

    void Insert()
    {
        _nganhDialog = new NganhDialog(DialogType.Them, new NganhDto(), NganhDao.GetInstance());
        _nganhDialog.Finish += () =>
        {
            UpdateDataDisplay(_nganhController.GetAll());
            this._table.UpdateData(_displayData);
        };
        _nganhDialog.ShowDialog();
    }

    void Update(int id)
    {
        var nganh = _rawData.FirstOrDefault(n => n.MaNganh == id);
        _nganhDialog = new NganhDialog(DialogType.Sua, nganh, NganhDao.GetInstance());
        
        _nganhDialog.Finish += () =>
        {
            UpdateDataDisplay(_nganhController.GetAll());
            this._table.UpdateData(_displayData);
        };
        _nganhDialog.ShowDialog();
    }
    
    void Detail(int id)
    {
        var nganh = _rawData.FirstOrDefault(n => n.MaNganh == id);
        _nganhDialog = new NganhDialog(DialogType.ChiTiet, nganh, NganhDao.GetInstance());
        
        _nganhDialog.ShowDialog();
    }

    void Delete(int id)
    {
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        if (_nganhController.Delete(id))
        {
            MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataDisplay(_nganhController.GetAll());
            this._table.UpdateData(_displayData);
        }
        else
        {
            MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    

    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }

    public override void onSearch(string txtSearch, string filter)
    {
        this._nganhSearch.Search(txtSearch, filter);
    }
}