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
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Components;

public class NganhPanel : NavBase
{
    
    private string ID = "NGANH";
    private string[] _listSelectionForComboBox = new[] { "Mã ngành", "Tên ngành", "Mã khoa", "Tên khoa" };

    private CustomTable _table;
    private Panel _tableContainer;
    private List<NganhDto> _currentNganhs;
    
    private NganhSearch _nganhSearch;

    private NganhDao nganhDAO = NganhDao.GetInstance();

    private TitleButton _insertButton;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    
    private List<ChiTietQuyenDto> _listAccess;
    private bool them = false;
    private bool sua = false;
    private bool xoa = false;

    public NganhPanel(NhomQuyenDto quyen) : base(quyen)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        Init();
        LoadData();
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
        

        _insertButton._mouseDown += () =>
        {
            using (var dialog = new NganhDialog(DialogType.Them, null, nganhDAO))
            {
                dialog.Finish += () => LoadData();
                dialog.ShowDialog();
            }
        };

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

        mainBot.Controls.Add(_tableContainer);
        return mainBot;
    }

    private void LoadData()
    {
        try
        {
            _currentNganhs = nganhDAO.GetAll() ?? new List<NganhDto>();
        }
        catch (Exception ex)
        {
            _currentNganhs = new List<NganhDto>();
            MessageBox.Show($"Lỗi khi tải danh sách ngành: {ex.Message}");
        }

        var nganhsAsObjectList = _currentNganhs.Cast<object>().ToList();

        if (_table == null)
        {
            string[] headerArray = new string[] { "Mã ngành", "Mã khoa", "Tên ngành" };
            List<string> headerList = ConvertArray_ListString.ConvertArrayToListString(headerArray);
            var columnNames = new List<string> { "MaNganh", "MaKhoa", "TenNganh" };

            _table = new CustomTable(headerList, columnNames, nganhsAsObjectList, sua || xoa, sua, xoa);

            _table.OnEdit += (id) =>
            {
                var nganh = _currentNganhs.FirstOrDefault(n => n.MaNganh == id);
                if (nganh != null)
                {
                    using (var dialog = new NganhDialog(DialogType.Sua, nganh, nganhDAO))
                    {
                        dialog.Finish += () => LoadData();
                        dialog.ShowDialog();
                    }
                }
            };

            _table.OnDelete += (id) =>
            {
                var nganh = _currentNganhs.FirstOrDefault(n => n.MaNganh == id);
                if (nganh != null)
                {
                    var confirm = MessageBox.Show($"Bạn có chắc muốn xóa ngành '{nganh.TenNganh}'?", "Xác nhận xóa",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        try
                        {
                            nganhDAO.Delete(nganh.MaNganh);
                            LoadData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi xóa ngành: {ex.Message}");
                        }
                    }
                }
            };

            _table.OnDetail += (id) =>
            {
                var nganh = _currentNganhs.FirstOrDefault(n => n.MaNganh == id);
                if (nganh != null)
                {
                    using (var dialog = new NganhDialog(DialogType.ChiTiet, nganh, nganhDAO))
                    {
                        dialog.ShowDialog();
                    }
                }
            };


            _tableContainer.Controls.Add(_table);
        }
        else
        {
            _table.UpdateData(nganhsAsObjectList);
        }
    }


    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    public override void onSearch(string txtSearch, string filter)
    { }
}