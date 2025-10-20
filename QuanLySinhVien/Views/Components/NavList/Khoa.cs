using System.Data;
using System.Diagnostics;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components;

public class Khoa : NavBase
{
    // variable
    private string[] _listSelectionForComboBox = new[] { "Mã khoa", "Tên khoa" };

    private KhoaController _kcontroller;
    private SearchBar _searchBar;
    
    private CustomTable _table;
    private List<KhoaDto> listKhoa;
    private Panel _mainBot;

    //button
    private Button btnThem;

    public Khoa()
    {
        _kcontroller = new KhoaController();
        Init();
        loadData();
        setActionListener();
        try
        {
            if (this._searchBar != null)
            {
                this._searchBar.UpdateListCombobox(_listSelectionForComboBox.ToList());
                this._searchBar.KeyDown += (txt, filter) => onSearch(txt, filter);
            }
        }
        catch
        {
            // khong co gi..
        }
    }

    private void Init()
    {
        Dock = DockStyle.Fill;
        Size = new Size(1200, 900);
        _mainBot = Bottom();
        Controls.Add(_mainBot);
        Controls.Add(Top());
    }

    private new Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Top,
            BackColor = MyColor.GrayBackGround,
            Height = 90
        };

        // edit button
        btnThem = new Button
        {
            Text = "Thêm khoa",
            Size = new Size(120, 40),
            Location = new Point(30, 25)
        };
        btnThem.Click += BtnThem_Click;
        mainTop.Controls.Add(btnThem);
        
        return mainTop;
    }

    void setActionListener()
    {
        if (_table != null)
        {
            this._table.OnEdit += maKhoa => BtnSua_Click(maKhoa);
            this._table.OnDelete += maKhoa => BtnXoa_Click(maKhoa);
            this._table.OnDetail += maKhoa => BtnChiTiet_Click(maKhoa);
        }
    }

    private new Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = MyColor.GrayBackGround,
            Padding = new Padding(20, 0, 20, 0)
        };
        
        return mainBot;
    }

    public void loadData()
    {
        listKhoa = _kcontroller.GetDanhSachKhoa();
        
        //  == null -> create new table else ...
        if (_table == null)
        {
            string[] headerArray = new string[] { "Mã khoa", "Tên khoa", "email", "Địa chỉ" };
            List<string> headerList = ConvertArray_ListString.ConvertArrayToListString(headerArray);
            var columnNames = new List<string> { "MaKhoa", "TenKhoa", "Email", "DiaChi" };
            
            var cells = listKhoa.Cast<object>().ToList();
            _table = new CustomTable(headerList, columnNames, cells, true, true, true);
            _mainBot.Controls.Add(_table);
        }
        else
        {
            _table.UpdateData(listKhoa.Cast<object>().ToList());
        }
    }

    // event
    private void BtnThem_Click(object? sender, EventArgs e)
    {
        try
        {
            using (var dialog = new KhoaDialog("Thêm Khoa mới", DialogType.Them))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _kcontroller.ThemKhoa(
                        dialog.TxtTenKhoa.Text.Trim(), 
                        dialog.TxtEmail.Text.Trim(), 
                        dialog.TxtDiaChi.Text.Trim()
                    );
                    MessageBox.Show("Thêm thành công!", "Thông báo");
                    loadData();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi thêm khoa: " + ex.Message);
        }
    }

    // edit button
    private void BtnSua_Click(int maKhoa)
    {
        try
        {
            // Tìm khoa theo MaKhoa
            KhoaDto khoa = listKhoa.FirstOrDefault(k => k.MaKhoa == maKhoa);
            if (khoa == null)
            {
                MessageBox.Show("Không tìm thấy khoa!", "Thông báo");
                return;
            }
    
            using (var dialog = new KhoaDialog("Sửa thông tin Khoa", DialogType.Sua))
            {
                dialog.LoadData(khoa);
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _kcontroller.SuaKhoa(
                        maKhoa, 
                        dialog.TxtTenKhoa.Text.Trim(), 
                        dialog.TxtEmail.Text.Trim(), 
                        dialog.TxtDiaChi.Text.Trim()
                    );
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                    loadData();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi sửa khoa: " + ex.Message);
        }
    }

    // del button
    private void BtnXoa_Click(int maKhoa)
    {
        try
        {
            // Tìm khoa theo MaKhoa
            KhoaDto khoa = listKhoa.FirstOrDefault(k => k.MaKhoa == maKhoa);
            if (khoa == null)
            {
                MessageBox.Show("Không tìm thấy khoa!", "Thông báo");
                return;
            }
    
            // Button y/n
            DialogResult rs = MessageBox.Show(
                $"Bạn chắc chắn muốn xóa khoa {khoa.TenKhoa}?",
                "Cảnh báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
                
            if (rs == DialogResult.Yes)
            {
                _kcontroller.XoaKhoa(maKhoa);
                loadData();
            }
            else if (rs == DialogResult.No)
            {
                MessageBox.Show("Đã hủy xóa khoa", "Thông báo");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi xóa khoa: " + ex.Message);
        }
    }

    // detail button - xem chi tiết
    private void BtnChiTiet_Click(int maKhoa)
    {
        try
        {
            // Tìm khoa theo MaKhoa
            KhoaDto khoa = listKhoa.FirstOrDefault(k => k.MaKhoa == maKhoa);
            if (khoa == null)
            {
                MessageBox.Show("Không tìm thấy khoa!", "Thông báo");
                return;
            }

            // Kiểm tra KhoaDialog có tồn tại không
            if (typeof(KhoaDialog) == null)
            {
                MessageBox.Show("KhoaDialog chưa được khởi tạo!", "Lỗi");
                return;
            }

            using (var dialog = new KhoaDialog("Chi tiết Khoa", DialogType.ChiTiet))
            {
                dialog.LoadData(khoa);
                dialog.ShowDialog();
            }
        }
        catch (NullReferenceException ex)
        {
            MessageBox.Show($"Lỗi null reference: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Lỗi");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi xem chi tiết: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Lỗi");
        }
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
        var f = (filter ?? string.Empty).Trim();
        var txt = (txtSearch ?? string.Empty).Trim();

        //  trả về tất cả
        if (string.IsNullOrWhiteSpace(txt) || string.Equals(f, "Tất cả", StringComparison.OrdinalIgnoreCase))
        {
            if (_table != null && listKhoa != null)
                _table.UpdateData(listKhoa.Cast<object>().ToList());
            return;
        }
        
        var searchTerm = txt.ToLowerInvariant();
        var filteredList = new List<KhoaDto>();

        if (string.Equals(f, "Tất cả", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(f))
        {
            // Tìm trong tất cả cột
            filteredList = listKhoa.Where(k =>
                k.MaKhoa.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                || (!string.IsNullOrEmpty(k.TenKhoa) && k.TenKhoa.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(k.Email) && k.Email.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(k.DiaChi) && k.DiaChi.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();
        }
        else
        {
            // Lọc theo combobox đã chọn
            filteredList = listKhoa.Where(k =>
            {
                switch (f)
                {
                    case "Mã khoa":
                        return k.MaKhoa.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                    case "Tên khoa":
                        return !string.IsNullOrEmpty(k.TenKhoa) && k.TenKhoa.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                    default:
                        return k.MaKhoa.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                            || (!string.IsNullOrEmpty(k.TenKhoa) && k.TenKhoa.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                            || (!string.IsNullOrEmpty(k.Email) && k.Email.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                            || (!string.IsNullOrEmpty(k.DiaChi) && k.DiaChi.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            }).ToList();
        }
        // reaload
        if (_table != null)
            _table.UpdateData(filteredList.Cast<object>().ToList());
    }
}