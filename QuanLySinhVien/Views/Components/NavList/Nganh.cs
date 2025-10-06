using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuanLySinhVien.DAO;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using Svg;

namespace QuanLySinhVien.Views.Components;

public class NganhPanel : NavBase
{
    private string[] _listSelectionForComboBox = new[] { "Mã ngành", "Tên ngành", "Mã khoa", "Tên khoa" };

    private CustomTable _table;
    private Panel _tableContainer;
    private List<Nganh> _currentNganhs;

    private NganhDAO nganhDAO = new NganhDAO();

    int iconSize = 24;
    int spacing = 25;

    private Bitmap _addIcon;

    public NganhPanel()
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
            Text = "Ngành",
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
            using (var dialog = new NganhDialog(NganhDialog.DialogMode.Create, null))
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultNganh != null)
                {
                    try
                    {
                        nganhDAO.addNganh(dialog.ResultNganh);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi thêm ngành: {ex.Message}");
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
            _currentNganhs = nganhDAO.getAllNganh() ?? new List<Nganh>();
        }
        catch (Exception ex)
        {
            _currentNganhs = new List<Nganh>();
            MessageBox.Show($"Lỗi khi tải danh sách ngành: {ex.Message}");
        }

        var nganhsAsObjectList = _currentNganhs.Cast<object>().ToList();

        if (_table == null)
        {
            string[] headerArray = new string[] { "Mã ngành", "Mã khoa", "Tên ngành" };
            List<string> headerList = ConvertArray_ListString.ConvertArrayToListString(headerArray);
            var columnNames = new List<string> { "MaNganh", "MaKhoa", "TenNganh" };

            _table = new CustomTable(headerList, columnNames, nganhsAsObjectList, true, true, true);

            _table.OnEdit += (index) =>
            {
                if (index >= 0 && index < _currentNganhs.Count)
                {
                    var nganh = _currentNganhs[index];
                    using (var dialog = new NganhDialog(NganhDialog.DialogMode.Edit, nganh))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultNganh != null)
                        {
                            try
                            {
                                nganhDAO.updateNganh(dialog.ResultNganh);
                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi cập nhật ngành: {ex.Message}");
                            }
                        }
                    }
                }
            };

            _table.OnDelete += (index) =>
            {
                if (index >= 0 && index < _currentNganhs.Count)
                {
                    var nganh = _currentNganhs[index];
                    var confirm = MessageBox.Show($"Bạn có chắc muốn xóa ngành '{nganh.TenNganh}'?", "Xác nhận xóa",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        try
                        {
                            nganhDAO.deleteNganh(nganh.MaNganh);
                            LoadData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi xóa ngành: {ex.Message}");
                        }
                    }
                }
            };

            _table.OnDetail += (index) =>
            {
                if (index >= 0 && index < _currentNganhs.Count)
                {
                    var nganh = _currentNganhs[index];
                    using (var dialog = new NganhDialog(NganhDialog.DialogMode.View, nganh))
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

    private class NganhDialog : Form
    {
        public enum DialogMode { View, Create, Edit }

        public Nganh ResultNganh { get; private set; }

        public NganhDialog(DialogMode mode, Nganh nganh)
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
                Text = mode == DialogMode.Create ? "Thêm ngành" : mode == DialogMode.Edit ? "Cập nhật ngành" : "Chi tiết ngành",
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

            var lblMaNganh = new Label { Text = "Mã Ngành", Font = new Font("Montserrat", 10), AutoSize = true };
            var txtMaNganh = new TextBox { Width = 250, Font = new Font("Montserrat", 10), BorderStyle = BorderStyle.FixedSingle, Enabled = false };

            var lblMaKhoa = new Label { Text = "Mã Khoa", Font = new Font("Montserrat", 10), AutoSize = true };
            var txtMaKhoa = new TextBox { Width = 250, Font = new Font("Montserrat", 10), BorderStyle = BorderStyle.FixedSingle };
            var lblTenNganh = new Label { Text = "Tên Ngành", Font = new Font("Montserrat", 10), AutoSize = true };
            var txtTenNganh = new TextBox { Width = 250, Font = new Font("Montserrat", 10), BorderStyle = BorderStyle.FixedSingle };

            if (nganh != null)
            {
                txtMaNganh.Text = nganh.MaNganh.ToString();
                txtMaKhoa.Text = nganh.MaKhoa.ToString();
                txtTenNganh.Text = nganh.TenNganh;
            }

            bool isView = mode == DialogMode.View;
            txtMaKhoa.Enabled = !isView;
            txtTenNganh.Enabled = !isView;

            var layout = new RoundTLP()
            {
                RowCount = 3,
                ColumnCount = 2,
                Dock = DockStyle.Top,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(0, 0, 0, 0)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            layout.Controls.Add(lblMaNganh, 0, 0);
            layout.Controls.Add(txtMaNganh, 1, 0);
            layout.Controls.Add(lblMaKhoa, 0, 1);
            layout.Controls.Add(txtMaKhoa, 1, 1);
            layout.Controls.Add(lblTenNganh, 0, 2);
            layout.Controls.Add(txtTenNganh, 1, 2);

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
                int maKhoa;
                if (!int.TryParse(txtMaKhoa.Text.Trim(), out maKhoa))
                {
                    MessageBox.Show("Mã Khoa phải là số nguyên.");
                    DialogResult = DialogResult.None;
                    return;
                }

                var tenNganh = txtTenNganh.Text.Trim();
                if (string.IsNullOrEmpty(tenNganh))
                {
                    MessageBox.Show("Tên Ngành không được để trống.");
                    DialogResult = DialogResult.None;
                    return;
                }

                if (mode == DialogMode.Create)
                {
                    ResultNganh = new Nganh { MaKhoa = maKhoa, TenNganh = tenNganh };
                }
                else if (mode == DialogMode.Edit && nganh != null)
                {
                    ResultNganh = new Nganh { MaNganh = nganh.MaNganh, MaKhoa = maKhoa, TenNganh = tenNganh };
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

            cardPanel.Controls.Add(layout);
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