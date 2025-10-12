using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Svg;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components
{
    public class HocPhan : NavBase
    {
        private string[] _listSelectionForComboBox = new[] { "MaHP", "TenHP" };
        private DataGridView dataGridView;
        private DataTable table;
        private CUse _cUse;

        private string connectionString = "Server=localhost;Port=3306;Database=QuanLySinhVien;Uid=root;Pwd=123456;";

        public HocPhan()
        {
            _cUse = new CUse();
            Init();
            SetupDataGridView();
            LoadDataFromDatabase();
        }

        // ==============================
        // === GIAO DIỆN CHÍNH ==========
        // ==============================
        private void Init()
        {
            Dock = DockStyle.Fill;
            Size = new Size(1200, 900);
            Controls.Add(Bottom());
            Controls.Add(Top());
        }

        private Panel Top()
        {
            Panel mainTop = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = ColorTranslator.FromHtml("#E5E7EB"),
                Height = 120,
            };

            var header = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(20, 10, 20, 10),
            };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var textNavList = new Label
            {
                Text = "Học Phần",
                Size = new Size(200, 40),
                UseCompatibleTextRendering = false,
            };

            var pathIconPlus = Path.Combine(AppContext.BaseDirectory, "img", "plus.svg");
            var imgPlus = SvgDocument.Open(pathIconPlus).Draw(25, 25);
            var btnAdd = new Button
            {
                Image = imgPlus,
                Text = "Thêm",
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(10, 30, 0, 10),
                Size = new Size(120, 50),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
            btnAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => ShowPopupForm(null);

            header.Controls.Add(textNavList, 0, 0);
            header.Controls.Add(btnAdd, 1, 0);
            mainTop.Controls.Add(header);
            return mainTop;
        }

        private Panel Bottom()
        {
            Panel mainBot = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#E5E7EB"),
                Padding = new Padding(20, 0, 20, 0),
            };
            mainBot.Controls.Add(GetDataGridView());
            return mainBot;
        }

        private DataGridView GetDataGridView()
        {
            dataGridView = _cUse.getDataView(710, 1140, 20, 20);
            dataGridView.ReadOnly = true;
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            return dataGridView;
        }

        // ==============================
        // ======= HIỂN THỊ DỮ LIỆU =====
        // ==============================
        private void SetupDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.RowTemplate.Height = 37;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView.RowHeadersVisible = false;
            dataGridView.Dock = DockStyle.Fill;

            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersHeight = 45;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#07689F"),
                ForeColor = Color.White,
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            // === Thứ tự hiển thị cột ===
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaHP", HeaderText = "Mã HP", DataPropertyName = "MaHP" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenHP", HeaderText = "Tên Học Phần", DataPropertyName = "TenHP" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoTinChi", HeaderText = "Số Tín Chỉ", DataPropertyName = "SoTinChi" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "HeSoHocPhan", HeaderText = "Hệ Số HP", DataPropertyName = "HeSoHocPhan" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoTietLyThuyet", HeaderText = "Số Tiết LT", DataPropertyName = "SoTietLyThuyet" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoTietThucHanh", HeaderText = "Số Tiết TH", DataPropertyName = "SoTietThucHanh" });
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "SELECT MaHP, TenHP, SoTinChi, HeSoHocPhan, SoTietLyThuyet, SoTietThucHanh FROM HocPhan";
                using var adapter = new MySqlDataAdapter(query, conn);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView.DataSource = table;
                AddEditDeleteColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        // ==============================
        // ======= NÚT SỬA / XÓA ========
        // ==============================
        private void AddEditDeleteColumns()
        {
            if (dataGridView.Columns.Contains("Edit")) return;

            var editIcon = _cUse.CreateIconWithBackground(
                Path.Combine(AppContext.BaseDirectory, "img", "fix.svg"),
                Color.Black,
                ColorTranslator.FromHtml("#6DB7E3"),
                28, 6, 4
            );
            var delIcon = _cUse.CreateIconWithBackground(
                Path.Combine(AppContext.BaseDirectory, "img", "trashbin.svg"),
                Color.Black,
                ColorTranslator.FromHtml("#FF6B6B"),
                28, 6, 4
            );

            var colEdit = new DataGridViewImageColumn { Name = "Edit", HeaderText = "", Image = editIcon, Width = 50 };
            var colDel = new DataGridViewImageColumn { Name = "Del", HeaderText = "", Image = delIcon, Width = 50 };
            dataGridView.Columns.Add(colEdit);
            dataGridView.Columns.Add(colDel);

            dataGridView.Columns["Edit"].DisplayIndex = dataGridView.Columns.Count - 2;
            dataGridView.Columns["Del"].DisplayIndex = dataGridView.Columns.Count - 1;

            dataGridView.CellClick += DataGridView_CellClick;
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string maHP = dataGridView.Rows[e.RowIndex].Cells["MaHP"].Value.ToString();

            if (e.ColumnIndex == dataGridView.Columns["Edit"].Index)
                ShowPopupForm(maHP);
            else if (e.ColumnIndex == dataGridView.Columns["Del"].Index)
                DeleteHocPhan(maHP);
        }

        // ==============================
        // ======= POPUP FORM ===========
        // ==============================
        private void ShowPopupForm(string? maHP)
        {
            bool isEdit = !string.IsNullOrEmpty(maHP);

            Form popup = new Form
            {
                Text = isEdit ? "Sửa học phần" : "Thêm học phần",
                Size = new Size(400, 420),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackColor = Color.White,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblTen = new Label { Text = "Tên học phần:", Location = new Point(30, 30) };
            var txtTen = new TextBox { Location = new Point(150, 30), Width = 200 };

            var lblTinChi = new Label { Text = "Số tín chỉ:", Location = new Point(30, 70) };
            var txtTinChi = new TextBox { Location = new Point(150, 70), Width = 200 };

            var lblHeSo = new Label { Text = "Hệ số HP:", Location = new Point(30, 110) };
            var txtHeSo = new TextBox { Location = new Point(150, 110), Width = 200 };

            var lblLT = new Label { Text = "Số tiết LT:", Location = new Point(30, 150) };
            var txtLT = new TextBox { Location = new Point(150, 150), Width = 200 };

            var lblTH = new Label { Text = "Số tiết TH:", Location = new Point(30, 190) };
            var txtTH = new TextBox { Location = new Point(150, 190), Width = 200 };

            if (isEdit)
            {
                DataRow row = table.AsEnumerable().FirstOrDefault(r => r["MaHP"].ToString() == maHP);
                if (row != null)
                {
                    txtTen.Text = row["TenHP"].ToString();
                    txtTinChi.Text = row["SoTinChi"].ToString();
                    txtHeSo.Text = row["HeSoHocPhan"].ToString();
                    txtLT.Text = row["SoTietLyThuyet"].ToString();
                    txtTH.Text = row["SoTietThucHanh"].ToString();
                }
            }

            var btnSave = new Button
            {
                Text = "Lưu",
                Size = new Size(100, 40),
                Location = new Point(250, 300),
                BackColor = ColorTranslator.FromHtml("#07689F"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                if (isEdit)
                    UpdateHocPhan(maHP, txtTen.Text, txtTinChi.Text, txtHeSo.Text, txtLT.Text, txtTH.Text);
                else
                    InsertHocPhan(txtTen.Text, txtTinChi.Text, txtHeSo.Text, txtLT.Text, txtTH.Text);

                popup.Close();
                LoadDataFromDatabase();
            };

            popup.Controls.AddRange(new Control[] { lblTen, txtTen, lblTinChi, txtTinChi, lblHeSo, txtHeSo, lblLT, txtLT, lblTH, txtTH, btnSave });
            popup.ShowDialog();
        }

        // ==============================
        // ======= CRUD DATABASE ========
        // ==============================
        private void InsertHocPhan(string ten, string tinChi, string heSo, string lt, string th)
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = @"INSERT INTO HocPhan (TenHP, SoTinChi, HeSoHocPhan, SoTietLyThuyet, SoTietThucHanh)
                                 VALUES (@ten, @tin, @he, @lt, @th)";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@tin", tinChi);
                cmd.Parameters.AddWithValue("@he", heSo);
                cmd.Parameters.AddWithValue("@lt", lt);
                cmd.Parameters.AddWithValue("@th", th);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm học phần thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm: {ex.Message}");
            }
        }

        private void UpdateHocPhan(string maHP, string ten, string tinChi, string heSo, string lt, string th)
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = @"UPDATE HocPhan 
                                 SET TenHP=@ten, SoTinChi=@tin, HeSoHocPhan=@he, SoTietLyThuyet=@lt, SoTietThucHanh=@th 
                                 WHERE MaHP=@id";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", maHP);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@tin", tinChi);
                cmd.Parameters.AddWithValue("@he", heSo);
                cmd.Parameters.AddWithValue("@lt", lt);
                cmd.Parameters.AddWithValue("@th", th);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật học phần thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật: {ex.Message}");
            }
        }

        private void DeleteHocPhan(string maHP)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa học phần này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using var conn = new MySqlConnection(connectionString);
                    conn.Open();
                    string query = "DELETE FROM HocPhan WHERE MaHP=@id";
                    using var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", maHP);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa học phần thành công!");
                    LoadDataFromDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa: {ex.Message}");
                }
            }
        }

        // ==============================
        // ======= OVERRIDE NAVBASE =====
        // ==============================
        public override List<string> getComboboxList()
        {
            return ConvertArray_ListString.ConvertArrayToListString(_listSelectionForComboBox);
        }

        public override void onSearch(string txtSearch, string filter)
        {
            if (table == null || table.Rows.Count == 0) return;
            try
            {
                string expression = $"{filter} LIKE '%{txtSearch}%'";
                DataRow[] foundRows = table.Select(expression);
                dataGridView.DataSource = foundRows.Length > 0 ? foundRows.CopyToDataTable() : table;
            }
            catch
            {
                dataGridView.DataSource = table;
            }
        }
    }
}
