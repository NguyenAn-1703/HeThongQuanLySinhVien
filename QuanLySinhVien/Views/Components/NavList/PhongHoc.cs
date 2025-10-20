using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLySinhVien.Views.Components;

public class PhongHoc : NavBase
{
        // variable
        private string ID = "PHONGHOC";
        private ChiTietQuyenController _chiTietQuyenController;
        private ChucNangController _chucNangController;
        private List<ChiTietQuyenDto> _listAccess;
        private readonly string[] _listSelectionForComboBox = { "Mã phòng", "Tên phòng", "Loại phòng", "Cơ sở" };
        private PhongHocController _phongHocController;
        private CustomTable _table;
        private SearchBar _searchBar;
        
        private List<PhongHocDto> _listPhongHoc;
        
        private Panel _mainBot;
        private Button _btnThem;

        public PhongHoc(NhomQuyenDto quyen) : base(quyen)
        {
            _chiTietQuyenController = ChiTietQuyenController.getInstance();
            _chucNangController = ChucNangController.getInstance();
            _phongHocController = PhongHocController.getInstance();
            Init();
            LoadData();
            SetActionListener();
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
            CheckQuyen();
            this.Dock = DockStyle.Fill;
            this.Size = new Size(1200, 900);
            _mainBot = Bottom();
            this.Controls.Add(_mainBot);
            this.Controls.Add(Top());
        }
    
    
    
    void CheckQuyen()
    {
        int maCN = _chucNangController.GetByTen(ID).MaCN;
        _listAccess = _chiTietQuyenController.GetByMaNQMaCN(_quyen.MaNQ, maCN);
        foreach (ChiTietQuyenDto x in _listAccess)
        {
            Console.WriteLine(x.HanhDong);
        }
    }


        private new Panel Top()
        {
            Panel mainTop = new Panel 
            { 
                Dock = DockStyle.Top, 
                BackColor = MyColor.GrayBackGround, 
                Height = 90 
            };
            
            _btnThem = new Button 
            { 
                Text = "Thêm phòng học", 
                Size = new Size(150, 40), 
                Location = new Point(30, 25) 
            };
            _btnThem.Click += BtnThem_Click;
            mainTop.Controls.Add(_btnThem);
            
            return mainTop;
        }

        private new Panel Bottom()
        {
            return new Panel 
            { 
                Dock = DockStyle.Fill, 
                BackColor = MyColor.GrayBackGround, 
                Padding = new Padding(20, 0, 20, 20) 
            };
        }
        private void SetActionListener()
        {
            if (_table != null)
            {
                _table.OnEdit += maPH => BtnSua_Click(maPH);
                _table.OnDelete += maPH => BtnXoa_Click(maPH);
                _table.OnDetail += maPH => BtnChiTiet_Click(maPH);
            }
        }

        private void ShowSchedule(int maPh)
        {
            throw new NotImplementedException();
        }
        
        public void LoadData()
        {
            _listPhongHoc = _phongHocController.GetDanhSachPhongHoc();
            
            if (_table == null)
            {
                string[] headerArray = { "Mã PH", "Tên phòng", "Loại phòng", "Cơ sở", "Sức chứa", "Tình trạng" };
                var headerList = headerArray.ToList();
                var columnNames = new List<string> { "MaPH", "TenPH", "LoaiPH", "CoSo", "SucChua", "TinhTrang" };
                
                var cells = _listPhongHoc.Cast<object>().ToList();
                _table = new CustomTable(headerList, columnNames, cells, true, true, true);
                _mainBot.Controls.Add(_table);
            }
            else
            {
                _table.UpdateData(_listPhongHoc.Cast<object>().ToList());
            }
        }

        // event
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                using (var dialog = new PhongHocDialog("Thêm Phòng học mới", DialogType.Them))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var resultDto = dialog.GetResult();
                        _phongHocController.ThemPhongHoc(resultDto);
                        MessageBox.Show("Thêm thành công!", "Thông báo");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phòng học: " + ex.Message);
            }
        }

        private void BtnSua_Click(int maPH)
        {
            try
            {
                var selectedItem = _listPhongHoc.FirstOrDefault(p => p.MaPH == maPH);
                
                if (selectedItem == null)
                {
                    MessageBox.Show("Không tìm thấy phòng học!", "Thông báo");
                    return;
                }
                
                using (var dialog = new PhongHocDialog("Sửa thông tin phòng học", DialogType.Sua))
                {
                    dialog.LoadData(selectedItem);
                    
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var resultDto = dialog.GetResult();
                        _phongHocController.SuaPhongHoc(resultDto);
                        MessageBox.Show("Cập nhật thành công!", "Thông báo");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa phòng học: " + ex.Message);
            }
        }

        private void BtnXoa_Click(int maPH)
        {
            try
            {
                var selectedItem = _listPhongHoc.FirstOrDefault(p => p.MaPH == maPH);
                
                if (selectedItem == null)
                {
                    MessageBox.Show("Không tìm thấy phòng học!", "Thông báo");
                    return;
                }
                
                // Button y/n
                DialogResult rs = MessageBox.Show(
                    $"Bạn có chắc muốn xóa phòng '{selectedItem.TenPH}'?", 
                    "Cảnh báo",
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (rs == DialogResult.Yes)
                {
                    bool success = _phongHocController.XoaPhongHoc(maPH);
                    
                    if (success)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!", "Lỗi");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa phòng học: " + ex.Message);
            }
        }

        private void BtnChiTiet_Click(int maPH)
        {
            try
            {
                // Tìm phòng theo MaPH
                var selectedItem = _listPhongHoc.FirstOrDefault(p => p.MaPH == maPH);
                
                if (selectedItem == null)
                {
                    MessageBox.Show("Không tìm thấy phòng học!", "Thông báo");
                    return;
                }
                
                // Mở dialog lịch học
                using (var dialog = new PhongHocScheduleDialog(selectedItem))
                {
                    dialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem lịch học: " + ex.Message);
            }
        }

        // override
        public override List<string> getComboboxList()
        {
            return _listSelectionForComboBox.ToList();
        }
        
        public override void onSearch(string txtSearch, string filter)
        {
            // check null -> gán giá trị
            var f = (filter ?? string.Empty).Trim();
            var txt = txtSearch ?? string.Empty;

            // tất cả -> trả về tất cả
            if (string.IsNullOrWhiteSpace(txt) || string.Equals(f, "Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                _table.UpdateData(_listPhongHoc.Cast<object>().ToList());
                return;
            }

            // lọc khoảng trắng
            var searchTerm = txt.ToLower().Trim();

            // lọc tìm kiếm
            var filteredList = _listPhongHoc.Where(ph =>
            {
                switch (f)
                {
                    case "Mã phòng":
                        return ph.MaPH.ToString().Contains(searchTerm);
                    case "Tên phòng":
                        return ph.TenPH?.ToLower().Contains(searchTerm) == true;
                    case "Loại phòng":
                        return ph.LoaiPH?.ToLower().Contains(searchTerm) == true;
                    case "Cơ sở":
                        return ph.CoSo?.ToLower().Contains(searchTerm) == true;
                    default:
                        return (ph.TenPH?.ToLower().Contains(searchTerm) == true)
                               || (ph.LoaiPH?.ToLower().Contains(searchTerm) == true)
                               || (ph.CoSo?.ToLower().Contains(searchTerm) == true)
                               || ph.MaPH.ToString().Contains(searchTerm);
                }
            }).ToList();

            _table.UpdateData(filteredList.Cast<object>().ToList());
        }

    }
