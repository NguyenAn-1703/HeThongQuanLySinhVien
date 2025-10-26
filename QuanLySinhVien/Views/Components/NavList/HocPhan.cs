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
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Components.NavList;

public class HocPhan : NavBase
{
    private string _title = "Học phần";
    private string[] _headerArray = new string[] { "Mã HP", "Mã HP Trước", "Tên HP", "Số Tín Chỉ", "Hệ Số", "Số Tiết LT", "Số Tiết TH" };
    private List<string> _listSelectionForComboBox;
    private List<string> _headerList;
    
    private string ID = "HOCPHAN";

    private CustomTable _table;
    private Panel _tableContainer;
    private List<HocPhanDto> _rawData;
    private List<object> _displayData;

    private HocPhanSearch _hocPhanSearch;

    private HocPhanDao hocPhanDAO = HocPhanDao.GetInstance();

    private TitleButton _insertButton;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    
    private List<ChiTietQuyenDto> _listAccess;
    private bool them = false;
    private bool sua = false;
    private bool xoa = false;

    public HocPhan(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _rawData = new List<HocPhanDto>();
        _displayData = new List<object>();
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

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Padding = new Padding(20)
        };

        _tableContainer = new Panel
        {
            Dock = DockStyle.Fill
        };

        SetCombobox();
        SetDataTableFromDb();
        SetSearch();
        SetAction();

        mainBot.Controls.Add(_tableContainer);
        _tableContainer.Controls.Add(_table);
        return mainBot;
    }

    void SetCombobox()
    {
        _headerList = ConvertArray_ListString.ConvertArrayToListString(_headerArray);
        _listSelectionForComboBox = _headerList;
    }

    void SetDataTableFromDb()
    {
        try
        {
            _rawData = hocPhanDAO.GetAll() ?? new List<HocPhanDto>();
        }
        catch (Exception ex)
        {
            _rawData = new List<HocPhanDto>();
            MessageBox.Show($"Lỗi khi tải danh sách học phần: {ex.Message}");
        }

        SetDisplayData();

        var columnNames = new List<string> { "MaHP", "MaHPTruoc", "TenHP", "SoTinChi", "HeSoHocPhan", "SoTietLyThuyet", "SoTietThucHanh" };

        _table = new CustomTable(_headerList, columnNames, _displayData, true, true, true);
    }

    void SetDisplayData()
    {
        _displayData = ConvertObject.ConvertToDisplay(_rawData, x => new
        {
            MaHP = x.MaHP,
            MaHPTruoc = x.MaHPTruoc,
            TenHP = x.TenHP,
            SoTinChi = x.SoTinChi,
            HeSoHocPhan = x.HeSoHocPhan,
            SoTietLyThuyet = x.SoTietLyThuyet,
            SoTietThucHanh = x.SoTietThucHanh
        });
    }

    void SetSearch()
    {
        _hocPhanSearch = new HocPhanSearch(_rawData);
    }

    void SetAction()
    {
        _hocPhanSearch.FinishSearch += dtos =>
        {
            UpdateDataDisplay(dtos);
            this._table.UpdateData(_displayData);
        };

        _insertButton._mouseDown += () => { Insert(); };
        _table.OnEdit += id => { Update(id); };
        _table.OnDetail += id => { Detail(id); };
        _table.OnDelete += id => { Delete(id); };
    }

    void UpdateDataDisplay(List<HocPhanDto> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaHP = x.MaHP,
            MaHPTruoc = x.MaHPTruoc,
            TenHP = x.TenHP,
            SoTinChi = x.SoTinChi,
            HeSoHocPhan = x.HeSoHocPhan,
            SoTietLyThuyet = x.SoTietLyThuyet,
            SoTietThucHanh = x.SoTietThucHanh
        });
    }

    void Insert()
    {
        using (var dialog = new HocPhanDialog(DialogType.Them, null, hocPhanDAO))
        {
            dialog.Finish += () =>
            {
                UpdateDataDisplay(hocPhanDAO.GetAll());
                this._table.UpdateData(_displayData);
            };
            dialog.ShowDialog();
        }
    }

    void Update(int id)
    {
        var hocPhan = _rawData.FirstOrDefault(h => h.MaHP == id);
        if (hocPhan != null)
        {
            using (var dialog = new HocPhanDialog(DialogType.Sua, hocPhan, hocPhanDAO))
            {
                dialog.Finish += () =>
                {
                    UpdateDataDisplay(hocPhanDAO.GetAll());
                    this._table.UpdateData(_displayData);
                };
                dialog.ShowDialog();
            }
        }
    }

    void Detail(int id)
    {
        var hocPhan = _rawData.FirstOrDefault(h => h.MaHP == id);
        if (hocPhan != null)
        {
            using (var dialog = new HocPhanDialog(DialogType.ChiTiet, hocPhan, hocPhanDAO))
            {
                dialog.ShowDialog();
            }
        }
    }

    void Delete(int id)
    {
        var hocPhan = _rawData.FirstOrDefault(h => h.MaHP == id);
        if (hocPhan != null)
        {
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa học phần '{hocPhan.TenHP}'?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    hocPhanDAO.Delete(hocPhan.MaHP);
                    UpdateDataDisplay(hocPhanDAO.GetAll());
                    this._table.UpdateData(_displayData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa học phần: {ex.Message}");
                }
            }
        }
    }


    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox;
    }
    
    public override void onSearch(string txtSearch, string filter)
    {
        this._hocPhanSearch.Search(txtSearch, filter);
    }
}