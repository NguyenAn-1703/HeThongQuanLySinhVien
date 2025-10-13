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

namespace QuanLySinhVien.Views.Components
{
    public class PhongHoc : NavBase
    {
        // variable
        private readonly string[] _listSelectionForComboBox = { "Mã phòng", "Tên phòng", "Loại phòng", "Cơ sở" };
        private PhongHocController _phongHocController = default!;
        private CustomTable _table = default!;
        private List<PhongHocDto> _listPhongHoc = default!;
        private Panel _mainBot = default!;
        private Button _btnThem = default!;

        public PhongHoc()
        {
            _phongHocController = new PhongHocController();
            Init();
            LoadData();
            // 1. setActionListener được gọi ở cuối constructor, sau khi _table đã được tạo
            SetActionListener(); 
        }

        private void Init()
        {
            this.Dock = DockStyle.Fill;
            this.Size = new Size(1200, 900);
            _mainBot = Bottom();
            this.Controls.Add(_mainBot);
            this.Controls.Add(Top());
        }

        private new Panel Top()
        {
            Panel mainTop = new Panel { Dock = DockStyle.Top, BackColor = MyColor.GrayBackGround, Height = 90 };
            _btnThem = new Button { Text = "Thêm phòng học", Size = new Size(150, 40), Location = new Point(30, 25) };
            _btnThem.Click += BtnThem_Click;
            mainTop.Controls.Add(_btnThem);
            return mainTop;
        }

        private new Panel Bottom()
        {
            return new Panel { Dock = DockStyle.Fill, BackColor = MyColor.GrayBackGround, Padding = new Padding(20, 0, 20, 20) };
        }

        private void SetActionListener()
        {
            if (_table != null)
            {
                _table.OnEdit += index => BtnSua_Click(index);
                _table.OnDelete += index => BtnXoa_Click(index);
                _table.OnDetail += index => BtnChiTiet_Click(index);
            }
        }

        public void LoadData()
        {
            _listPhongHoc = _phongHocController.GetDanhSachPhongHoc();
            if (_table == null)
            {
                string[] headerArray = { "Mã PH", "Tên phòng", "Loại phòng", "Cơ sở", "Sức chứa", "Tình trạng" };
                var columnNames = new List<string> { "MaPH", "TenPH", "LoaiPH", "CoSo", "SucChua", "TinhTrang" };
                _table = new CustomTable(headerArray.ToList(), columnNames, _listPhongHoc.Cast<object>().ToList(), true, true, true);
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
            // 2. Sử dụng Dialog mới theo đúng mẫu
            using (var dialog = new PhongHocDialog("Thêm Phòng học mới", DialogType.Them))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _phongHocController.ThemPhongHoc(dialog.GetResult());
                    MessageBox.Show("Thêm thành công!", "Thông báo");
                    LoadData();
                }
            }
        }

        private void BtnSua_Click(int index)
        {
            var selectedItem = _listPhongHoc[index];
            using (var dialog = new PhongHocDialog("Sửa thông tin phòng học", DialogType.Sua))
            {
                dialog.LoadData(selectedItem); // Nạp dữ liệu vào dialog
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _phongHocController.SuaPhongHoc(dialog.GetResult());
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                    LoadData();
                }
            }
        }

        private void BtnXoa_Click(int index)
        {
            var selectedItem = _listPhongHoc[index];
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa phòng '{selectedItem.TenPH}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                _phongHocController.XoaPhongHoc(selectedItem.MaPH);
                MessageBox.Show("Xóa thành công!", "Thông báo");
                LoadData();
            }
        }
        
        private void BtnChiTiet_Click(int index)
        {
            var selectedItem = _listPhongHoc[index];
            using (var dialog = new PhongHocDialog("Chi tiết phòng học", DialogType.ChiTiet))
            {
                dialog.LoadData(selectedItem);
                // Bạn có thể cần thêm logic trong PhongHocDialog để xử lý mode ChiTiet
                // ví dụ: vô hiệu hóa các textbox, ẩn nút Lưu
                dialog.ShowDialog();
            }
        }

        // override
        public override List<string> getComboboxList()
        {
            return _listSelectionForComboBox.ToList();
        }

        public override void onSearch(string txtSearch, string filter)
        {
            if (string.IsNullOrWhiteSpace(txtSearch))
            {
                _table.UpdateData(_listPhongHoc.Cast<object>().ToList());
                return;
            }
            var searchTerm = txtSearch.ToLower().Trim();
            var filteredList = _listPhongHoc.Where(ph =>
            {
                switch (filter)
                {
                    case "Mã phòng": return ph.MaPH.ToString().Contains(searchTerm);
                    case "Tên phòng": return ph.TenPH?.ToLower().Contains(searchTerm) == true;
                    case "Loại phòng": return ph.LoaiPH?.ToLower().Contains(searchTerm) == true;
                    case "Cơ sở": return ph.CoSo?.ToLower().Contains(searchTerm) == true;
                    default: return false;
                }
            }).ToList();
            _table.UpdateData(filteredList.Cast<object>().ToList());
        }
    }
}