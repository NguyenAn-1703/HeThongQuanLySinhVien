using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using Svg;

namespace QuanLySinhVien.Views.Components.NavList;

public class HocPhan : NavBase
{
    private string[] _listSelectionForComboBox = new[] { "Mã HP", "Tên HP", "Mã HP Trước", "Số Tín Chỉ" };

    private CustomTable _table;
    private Panel _tableContainer;
    private List<HocPhanDto> _currentHocPhans;
    

    private HocPhanDao hocPhanDAO = new HocPhanDao();

    int iconSize = 24;
    int spacing = 25;

    private Bitmap _addIcon;

    public HocPhan()
    {
        Init();
        LoadData();
    }

    private void Init()
    {
        Dock = DockStyle.Fill;
        Size = new Size(1200, 900);
        var borderTop = new Panel
        {
            Dock = DockStyle.Fill,
        };
        Controls.Add(Bottom());
        Controls.Add(Top());
    }

    private Panel Top()
    {
        var addIconPath = Path.Combine(AppContext.BaseDirectory, "img", "plus.svg");
        if (_addIcon == null && File.Exists(addIconPath))
        {
            var svgDoc = SvgDocument.Open(addIconPath);
            var rawBitmap = svgDoc.Draw();
            _addIcon = ResizeImage(rawBitmap, iconSize, iconSize);
            rawBitmap.Dispose();
        }

        Panel mainTop = new Panel
        {
            Dock = DockStyle.Top,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Height = 90,
            Padding = new Padding(20)
        };

        Label title = new Label
        {
            Text = "Học phần",
            Dock = DockStyle.Left,
            AutoSize = true,
            Height = 50,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Montserrat", 20, FontStyle.Bold)
        };

        Button addButton = new Button
        {
            Text = "Thêm",
            Font = new Font("Montserrat", 10, FontStyle.Bold),
            FlatAppearance = { BorderSize = 0 },
            Width = 150,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Right,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            Image = _addIcon,
            ImageAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 10, 0),
            TextImageRelation = TextImageRelation.ImageBeforeText
        };

        addButton.MouseEnter += (sender, e) =>
        {
            addButton.Cursor = Cursors.Hand;
            addButton.BackColor = ColorTranslator.FromHtml("#B0BEC5");
        };

        addButton.MouseLeave += (sender, e) => { addButton.BackColor = Color.White; };

        addButton.MouseClick += (sender, e) =>
        {
            using (var dialog = new HocPhanDialog(HocPhanDialog.DialogMode.Create, null))
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultHocPhanDto != null)
                {
                    try
                    {
                        hocPhanDAO.Insert(dialog.ResultHocPhanDto);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi thêm học phần: {ex.Message}");
                    }
                }
            }
        };

        mainTop.Controls.Add(title);
        mainTop.Controls.Add(addButton);

        return mainTop;
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
            _currentHocPhans = hocPhanDAO.GetAll() ?? new List<HocPhanDto>();
        }
        catch (Exception ex)
        {
            _currentHocPhans = new List<HocPhanDto>();
            MessageBox.Show($"Lỗi khi tải danh sách học phần: {ex.Message}");
        }

        var hocPhansAsObjectList = _currentHocPhans.Cast<object>().ToList();

        if (_table == null)
        {
            string[] headerArray = new string[] { "Mã HP", "Mã HP Trước", "Tên HP", "Số Tín Chỉ", "Hệ Số", "Số Tiết LT", "Số Tiết TH" };
            List<string> headerList = ConvertArray_ListString.ConvertArrayToListString(headerArray);
            var columnNames = new List<string> { "MaHP", "MaHPTruoc", "TenHP", "SoTinChi", "HeSoHocPhan", "SoTietLyThuyet", "SoTietThucHanh" };

            _table = new CustomTable(headerList, columnNames, hocPhansAsObjectList, true, true, true);

            _table.OnEdit += (index) =>
            {
                if (index >= 0 && index < _currentHocPhans.Count)
                {
                    var hocPhan = _currentHocPhans[index];
                    using (var dialog = new HocPhanDialog(HocPhanDialog.DialogMode.Edit, hocPhan))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultHocPhanDto != null)
                        {
                            try
                            {
                                hocPhanDAO.Update(dialog.ResultHocPhanDto);
                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi cập nhật học phần: {ex.Message}");
                            }
                        }
                    }
                }
            };

            _table.OnDelete += (index) =>
            {
                if (index >= 0 && index < _currentHocPhans.Count)
                {
                    var hocPhan = _currentHocPhans[index];
                    var confirm = MessageBox.Show($"Bạn có chắc muốn xóa học phần '{hocPhan.TenHP}'?", "Xác nhận xóa",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        try
                        {
                            hocPhanDAO.Delete(hocPhan.MaHP);
                            LoadData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi xóa học phần: {ex.Message}");
                        }
                    }
                }
            };

            _table.OnDetail += (index) =>
            {
                if (index >= 0 && index < _currentHocPhans.Count)
                {
                    var hocPhan = _currentHocPhans[index];
                    using (var dialog = new HocPhanDialog(HocPhanDialog.DialogMode.View, hocPhan))
                    {
                        dialog.ShowDialog();
                    }
                }
            };

            _tableContainer.Controls.Add(_table);
        }
        else
        {
            _table.UpdateData(hocPhansAsObjectList);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_addIcon != null)
            {
                _addIcon.Dispose();
                _addIcon = null;
            }
        }
        base.Dispose(disposing);
    }

    private class HocPhanDialog : Form
    {
        public enum DialogMode { View, Create, Edit }

        public HocPhanDto ResultHocPhanDto { get; private set; }

        private Label _maHPLabel;
        private TextBox _maHPTextBox;
        private Label _maHPTruocLabel;
        private ComboBox _maHPTruocComboBox;
        private Label _tenHPLabel;
        private TextBox _tenHPTextBox;
        private Label _soTinChiLabel;
        private TextBox _soTinChiTextBox;
        private Label _heSoHocPhanLabel;
        private TextBox _heSoHocPhanTextBox;
        private Label _soTietLyThuyetLabel;
        private TextBox _soTietLyThuyetTextBox;
        private Label _soTietThucHanhLabel;
        private TextBox _soTietThucHanhTextBox;

        public HocPhanDialog(DialogMode mode, HocPhanDto hocPhanDto)
        {
            Text = string.Empty;
            Size = new Size(420, 350);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            BackColor = ColorTranslator.FromHtml("#F3F4F6");

            var headerPanel = new Panel
            {
                BackColor = ColorTranslator.FromHtml("#07689F"),
                Height = 50,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 0)
            };
            
            var lblTitle = new Label
            {
                Text = mode == DialogMode.Create ? "Thêm học phần" : mode == DialogMode.Edit ? "Cập nhật học phần" : "Chi tiết học phần",
                ForeColor = Color.White,
                Font = new Font("Montserrat", 13, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 0, 0)
            };

            var btnClose = new Button
            {
                Text = "✕",
                Width = 50,
                Height = 50,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTranslator.FromHtml("#07689F"),
                ForeColor = Color.White,
                Font = new Font("Arial", 16, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = ColorTranslator.FromHtml("#055a7a");
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = ColorTranslator.FromHtml("#07689F");

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnClose);

            var cardPanel = new Panel
            {
                BackColor = ColorTranslator.FromHtml("#F3F4F6"),
                Size = new Size(420, 300),
                Location = new Point(0, 50),
                Padding = new Padding(30, 50, 30, 50)
            };

            // Create standard controls
            _maHPLabel = new Label
            {
                Text = "Mã HP:",
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            _maHPTextBox = new TextBox
            {
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                Size = new Size(250, 25),
                Location = new Point(100, 0),
                ReadOnly = true,
                BackColor = Color.LightGray
            };

            _maHPTruocLabel = new Label
            {
                Text = "Mã HP Trước:",
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 30)
            };
            _maHPTruocComboBox = new ComboBox
            {
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                Size = new Size(250, 25),
                Location = new Point(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _tenHPLabel = new Label
            {
                Text = "Tên HP:",
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 60)
            };
            _tenHPTextBox = new TextBox
            {
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                Size = new Size(250, 25),
                Location = new Point(100, 60)
            };

            _soTinChiLabel = new Label
            {
                Text = "Số Tín Chỉ:",
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 90)
            };
            _soTinChiTextBox = new TextBox
            {
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                Size = new Size(250, 25),
                Location = new Point(100, 90)
            };

            _heSoHocPhanLabel = new Label
            {
                Text = "Hệ Số HP:",
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 120)
            };
            _heSoHocPhanTextBox = new TextBox
            {
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                Size = new Size(250, 25),
                Location = new Point(100, 120)
            };

            _soTietLyThuyetLabel = new Label
            {
                Text = "Số Tiết LT:",
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 150)
            };
            _soTietLyThuyetTextBox = new TextBox
            {
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                Size = new Size(250, 25),
                Location = new Point(100, 150)
            };

            _soTietThucHanhLabel = new Label
            {
                Text = "Số Tiết TH:",
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 180)
            };
            _soTietThucHanhTextBox = new TextBox
            {
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                Size = new Size(250, 25),
                Location = new Point(100, 180)
            };

            // Set TabIndex for editable fields
            if (mode != DialogMode.View)
            {
                _maHPTruocComboBox.TabIndex = 1;
                _tenHPTextBox.TabIndex = 2;
                _soTinChiTextBox.TabIndex = 3;
                _heSoHocPhanTextBox.TabIndex = 4;
                _soTietLyThuyetTextBox.TabIndex = 5;
                _soTietThucHanhTextBox.TabIndex = 6;
            }

            // Populate ComboBox for MaHPTruoc
            var hocPhanDao = new HocPhanDao();
            var hocPhanComboList = hocPhanDao.GetAllForCombobox();
            hocPhanComboList.Insert(0, "(Không có)");
            _maHPTruocComboBox.Items.AddRange(hocPhanComboList.ToArray());

            // Set values if editing/viewing
            if (hocPhanDto != null)
            {
                _maHPTextBox.Text = hocPhanDto.MaHP.ToString();
                _tenHPTextBox.Text = hocPhanDto.TenHP;
                _soTinChiTextBox.Text = hocPhanDto.SoTinChi.ToString();
                _heSoHocPhanTextBox.Text = hocPhanDto.HeSoHocPhan.ToString();
                _soTietLyThuyetTextBox.Text = hocPhanDto.SoTietLyThuyet.ToString();
                _soTietThucHanhTextBox.Text = hocPhanDto.SoTietThucHanh.ToString();

                // Set MaHPTruoc selection
                if (hocPhanDto.MaHPTruoc.HasValue)
                {
                    string targetValue = $"{hocPhanDto.MaHPTruoc.Value} - ";
                    foreach (string item in _maHPTruocComboBox.Items)
                    {
                        if (item.StartsWith(targetValue))
                        {
                            _maHPTruocComboBox.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    _maHPTruocComboBox.SelectedItem = "(Không có)";
                }
            }

            bool isView = mode == DialogMode.View;
            bool isAdd = mode == DialogMode.Create;

            // Set field visibility and enable state
            _maHPLabel.Visible = isView; // Hide in Add/Edit, show in View
            _maHPTextBox.Visible = isView;
            _maHPTextBox.Enabled = false; // Always disabled

            _maHPTruocComboBox.Enabled = !isView;
            _tenHPTextBox.Enabled = !isView;
            _soTinChiTextBox.Enabled = !isView;
            _heSoHocPhanTextBox.Enabled = !isView;
            _soTietLyThuyetTextBox.Enabled = !isView;
            _soTietThucHanhTextBox.Enabled = !isView;

            // Add controls to cardPanel
            if (isView)
            {
                cardPanel.Controls.Add(_maHPLabel);
                cardPanel.Controls.Add(_maHPTextBox);
            }

            cardPanel.Controls.Add(_maHPTruocLabel);
            cardPanel.Controls.Add(_maHPTruocComboBox);
            cardPanel.Controls.Add(_tenHPLabel);
            cardPanel.Controls.Add(_tenHPTextBox);
            cardPanel.Controls.Add(_soTinChiLabel);
            cardPanel.Controls.Add(_soTinChiTextBox);
            cardPanel.Controls.Add(_heSoHocPhanLabel);
            cardPanel.Controls.Add(_heSoHocPhanTextBox);
            cardPanel.Controls.Add(_soTietLyThuyetLabel);
            cardPanel.Controls.Add(_soTietLyThuyetTextBox);
            cardPanel.Controls.Add(_soTietThucHanhLabel);
            cardPanel.Controls.Add(_soTietThucHanhTextBox);

            var btnCancel = new Button
            {
                Text = isView ? "Đóng" : "Hủy",
                DialogResult = DialogResult.Cancel,
                Width = 90,
                Height = 32,
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 20, 10, 0),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#B0BEC5");
            btnCancel.FlatAppearance.BorderSize = 1;

            var btnOk = new Button
            {
                Text = mode == DialogMode.Create ? "Thêm" : "Cập nhật",
                DialogResult = DialogResult.OK,
                Width = 100,
                Height = 32,
                Font = new Font("Montserrat", 10, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#07689F"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(10, 20, 0, 0),
                Cursor = Cursors.Hand,
                Visible = !isView
            };
            btnOk.FlatAppearance.BorderSize = 0;

            btnOk.Click += (s, e) =>
            {
                // Validation
                if (string.IsNullOrWhiteSpace(_tenHPTextBox.Text))
                {
                    MessageBox.Show("Tên HP không được để trống.");
                    DialogResult = DialogResult.None;
                    return;
                }

                if (!int.TryParse(_soTinChiTextBox.Text, out _))
                {
                    MessageBox.Show("Số tín chỉ phải là số.");
                    DialogResult = DialogResult.None;
                    return;
                }

                if (!float.TryParse(_heSoHocPhanTextBox.Text, out _))
                {
                    MessageBox.Show("Hệ số học phần phải là số.");
                    DialogResult = DialogResult.None;
                    return;
                }

                if (!int.TryParse(_soTietLyThuyetTextBox.Text, out _))
                {
                    MessageBox.Show("Số tiết lý thuyết phải là số.");
                    DialogResult = DialogResult.None;
                    return;
                }

                if (!int.TryParse(_soTietThucHanhTextBox.Text, out _))
                {
                    MessageBox.Show("Số tiết thực hành phải là số.");
                    DialogResult = DialogResult.None;
                    return;
                }

                // Parse MaHPTruoc
                int? maHPTruoc = null;
                string selectedHPTruoc = _maHPTruocComboBox.SelectedItem?.ToString();
                if (selectedHPTruoc != "(Không có)" && !string.IsNullOrEmpty(selectedHPTruoc))
                {
                    maHPTruoc = int.Parse(selectedHPTruoc.Split('-')[0].Trim());
                }

                if (mode == DialogMode.Create)
                {
                    ResultHocPhanDto = new HocPhanDto 
                    { 
                        MaHPTruoc = maHPTruoc,
                        TenHP = _tenHPTextBox.Text,
                        SoTinChi = int.Parse(_soTinChiTextBox.Text),
                        HeSoHocPhan = float.Parse(_heSoHocPhanTextBox.Text),
                        SoTietLyThuyet = int.Parse(_soTietLyThuyetTextBox.Text),
                        SoTietThucHanh = int.Parse(_soTietThucHanhTextBox.Text)
                    };
                }
                else if (mode == DialogMode.Edit && hocPhanDto != null)
                {
                    ResultHocPhanDto = new HocPhanDto 
                    { 
                        MaHP = hocPhanDto.MaHP,
                        MaHPTruoc = maHPTruoc,
                        TenHP = _tenHPTextBox.Text,
                        SoTinChi = int.Parse(_soTinChiTextBox.Text),
                        HeSoHocPhan = float.Parse(_heSoHocPhanTextBox.Text),
                        SoTietLyThuyet = int.Parse(_soTietLyThuyetTextBox.Text),
                        SoTietThucHanh = int.Parse(_soTietThucHanhTextBox.Text)
                    };
                }
            };

            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Height = 50
            };
            buttonPanel.Controls.Add(btnCancel);
            if (!isView) buttonPanel.Controls.Add(btnOk);

            cardPanel.Controls.Add(buttonPanel);
            buttonPanel.BringToFront();

            Controls.Add(headerPanel);
            Controls.Add(cardPanel);

            if (!isView)
            {
                AcceptButton = btnOk;
                CancelButton = btnCancel;
            }
            else
            {
                CancelButton = btnCancel;
            }
        }
    }

    Bitmap ResizeImage(Bitmap original, int width, int height)
    {
        var bmp = new Bitmap(width, height);
        using (var g = Graphics.FromImage(bmp))
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(original, 0, 0, width, height);
        }

        return bmp;
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    public override void onSearch(string txtSearch, string filter)
    { }
}