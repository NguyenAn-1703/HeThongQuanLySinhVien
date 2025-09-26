using QuanLySinhVien.DAO;
using QuanLySinhVien.DB;
using QuanLySinhVien.Models;
using Svg;

using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class NganhPanel : NavBase
{
    private DataGridView dataGrid;
    private TableLayoutPanel tableLayout;
    
    private NganhDAO nganhDAO = new NganhDAO();

    int iconSize = 24;
    int spacing = 25;
    
    private string[] _listSelectionForComboBox = new []{"Mã ngành", "Tên ngành"};
    public NganhPanel()
    {
        Init();
        LoadData();
    }

    private void Init()
    {
        //BackColor = Color.Blue;
        Dock = DockStyle.Bottom;
        Size = new Size(1200, 900);
        var borderTop = new Panel
        {
            Dock = DockStyle.Fill,
            // Padding = new  Padding(0 , 30 , 0 , 0),
        };
        Controls.Add(Top());
        // Controls.Add(borderTop);
        Controls.Add(Bottom());
    }

    private Panel Top()
    {
        var addIconPath =
            Path.Combine(Path.Combine(AppContext.BaseDirectory, "img", "plus.svg"));

        Bitmap addIcon = null;

        if (File.Exists(addIconPath))
        {
            var svgDoc = SvgDocument.Open(addIconPath);
            addIcon = svgDoc.Draw();
            addIcon = ResizeImage(addIcon, iconSize, iconSize);
        }

        Panel mainTop = new Panel
        {
            Dock = DockStyle.Bottom,
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
            Image = addIcon,
            ImageAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(10, 0, 10, 0),
            TextImageRelation = TextImageRelation.ImageBeforeText
        };

        addButton.MouseEnter += (sender, e) =>
        {
            addButton.Cursor = Cursors.Hand;
            addButton.BackColor = ColorTranslator.FromHtml("#B0BEC5");
        };

        addButton.MouseLeave += (sender, e) =>
        {
            addButton.BackColor = Color.White;
        };

        addButton.MouseClick += (sender, e) =>
        {
            var newNganh = ShowNganhDialog(null);
            if (newNganh != null)
            {
                try
                {
                    nganhDAO.addNganh(newNganh);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm ngành: {ex.Message}");
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
            Dock = DockStyle.Bottom,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Height = 780,
            Padding = new Padding(20)
        };

        var editIconPath =
            Path.Combine(Path.Combine(AppContext.BaseDirectory, "img", "fix.svg"));
        var deleteIconPath =
            Path.Combine(Path.Combine(AppContext.BaseDirectory, "img", "trashbin.svg"));

        Bitmap editIcon = null;
        Bitmap deleteIcon = null;

        if (File.Exists(editIconPath))
        {
            var svgDoc = SvgDocument.Open(editIconPath);
            editIcon = svgDoc.Draw();
            editIcon = ResizeImage(editIcon, iconSize, iconSize);
        }

        if (File.Exists(deleteIconPath))
        {
            var svgDoc = SvgDocument.Open(deleteIconPath);
            deleteIcon = svgDoc.Draw();
            deleteIcon = ResizeImage(deleteIcon, iconSize, iconSize);
        }

        dataGrid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
            BorderStyle = BorderStyle.None,
            Margin = new Padding(20),
            MultiSelect = false,
            BackgroundColor = Color.White,
            EnableHeadersVisualStyles = false,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.None,
            GridColor = Color.White,
            AllowUserToResizeColumns = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AllowUserToResizeRows = false,
            AutoGenerateColumns = false,
            RowHeadersVisible = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#07689F"),
                ForeColor = Color.White,
                Font = new Font("Montserrat", 10, FontStyle.Bold),
                SelectionBackColor = ColorTranslator.FromHtml("#07689F"),
                SelectionForeColor = Color.White,
            },
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = ColorTranslator.FromHtml("#B0BEC5"),
                SelectionForeColor = Color.Black,
            },
        };

        dataGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "MaNganh",
            HeaderText = "Mã Ngành",
            DataPropertyName = "MaNganh"
        });
        dataGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "MaKhoa",
            HeaderText = "Mã Khoa",
            DataPropertyName = "MaKhoa"
        });
        dataGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "TenNganh",
            HeaderText = "Tên Nghành",
            DataPropertyName = "TenNganh"
        });

        int totalWidth = iconSize * 2 + spacing;

        var actionColumn = new DataGridViewImageColumn
        {
            Name = "Action",
            HeaderText = "Hành động",
            Width = totalWidth,
            ImageLayout = DataGridViewImageCellLayout.Normal
        };

        dataGrid.Columns.Add(actionColumn);

        dataGrid.RowTemplate.Height = iconSize + 10;

        dataGrid.CellPainting += (s, e) =>
        {
            if (e.RowIndex < 0) return;
            if (dataGrid.Columns[e.ColumnIndex].Name == "Action")
            {
                e.PaintBackground(e.CellBounds, true);

                int startX = e.CellBounds.X + (e.CellBounds.Width - totalWidth) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - iconSize) / 2;

                e.Graphics.DrawImage(editIcon, new Rectangle(startX, y, iconSize, iconSize));

                e.Graphics.DrawImage(deleteIcon, new Rectangle(startX + iconSize + spacing, y, iconSize, iconSize));

                e.Handled = true;
            }
        };

        dataGrid.CellMouseMove += (s, e) =>
        {
            if (e.RowIndex < 0) return;
            if (dataGrid.Columns[e.ColumnIndex].Name == "Action")
            {
                var cellRect = dataGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int startX = (cellRect.Width - totalWidth) / 2;

                // Khoanh vùng ảnh Sửa
                Rectangle editRect = new Rectangle(cellRect.X + startX, cellRect.Y + (cellRect.Height - iconSize) / 2,
                    iconSize, iconSize);
                // Khoanh vùng ảnh Xóa
                Rectangle deleteRect = new Rectangle(cellRect.X + startX + iconSize + spacing,
                    cellRect.Y + (cellRect.Height - iconSize) / 2, iconSize, iconSize);

                var mousePos = dataGrid.PointToClient(Cursor.Position);

                if (editRect.Contains(mousePos) || deleteRect.Contains(mousePos))
                    dataGrid.Cursor = Cursors.Hand;
                else
                    dataGrid.Cursor = Cursors.Default;
            }
            else
            {
                dataGrid.Cursor = Cursors.Default;
            }
        };

        dataGrid.CellClick += (s, e) =>
        {
            if (e.RowIndex < 0) return;
            if (dataGrid.Columns[e.ColumnIndex].Name == "Action")
            {
                var cellRect = dataGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                var clickX = dataGrid.PointToClient(Cursor.Position).X - cellRect.X;
                int totalWidth = iconSize * 2 + spacing;

                if (clickX >= (cellRect.Width - totalWidth) / 2 &&
                    clickX <= (cellRect.Width - totalWidth) / 2 + iconSize)
                {
                    // Edit action
                    var row = dataGrid.Rows[e.RowIndex];
                    var nganh = row.DataBoundItem as QuanLySinhVien.Models.Nganh;
                    if (nganh != null)
                    {
                        var updatedNganh = ShowNganhDialog(nganh);
                        if (updatedNganh != null)
                        {
                            try
                            {
                                nganhDAO.updateNganh(updatedNganh);
                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi cập nhật ngành: {ex.Message}");
                            }
                        }
                    }
                }
                else if (clickX >= (cellRect.Width - totalWidth) / 2 + iconSize + spacing &&
                         clickX <= (cellRect.Width - totalWidth) / 2 + totalWidth)
                {
                    // Delete action
                    var row = dataGrid.Rows[e.RowIndex];
                    var nganh = row.DataBoundItem as QuanLySinhVien.Models.Nganh;
                    if (nganh != null)
                    {
                        var confirm = MessageBox.Show($"Bạn có chắc muốn xóa ngành '{nganh.TenNganh}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                }
            }
        };

        mainBot.Controls.Add(dataGrid);

        return mainBot;
    }

    private void LoadData()
    {
        var nganhs = nganhDAO.getAllNganh();
        dataGrid.DataSource = nganhs;
    }

    // Dialog for add/edit Nganh
    private Nganh ShowNganhDialog(Nganh nganh)
    {
        var dialog = new Form
        {
            Text = string.Empty,
            Size = new Size(420, 350),
            FormBorderStyle = FormBorderStyle.None,
            StartPosition = FormStartPosition.CenterParent,
            // BackColor = Color.FromArgb(0, 0, 0, 0), // Transparent for rounded effect
            ShowInTaskbar = false
        };

        // Header panel
        var headerPanel = new Panel
        {
            BackColor = ColorTranslator.FromHtml("#07689F"),
            Height = 50,
            Dock = DockStyle.Top,
            Padding = new Padding(0, 0, 0, 0)
        };
        
        var lblTitle = new Label
        {
            Text = nganh == null ? "Thêm ngành" : "Cập nhật ngành",
            ForeColor = Color.White,
            Font = new Font("Montserrat", 13, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        
        headerPanel.Controls.Add(lblTitle);

        // Main card panel (rounded)
        var cardPanel = new Panel
        {
            BackColor = ColorTranslator.FromHtml("#F3F4F6"),
            Size = new Size(420, 300),
            Location = new Point(0, 50),
            Padding = new Padding(30, 50, 30, 50)
        };
        // cardPanel.Region = System.Drawing.Region.FromHrgn(
        //     NativeMethods.CreateRoundRectRgn(0, 0, cardPanel.Width, cardPanel.Height, 18, 18));

        // Labels and textboxes
        var lblMaNganh = new Label { Text = "Mã Ngành", Font = new Font("Montserrat", 10), AutoSize = true };
        var txtMaNganh = new TextBox { Width = 250, Font = new Font("Montserrat", 10), BorderStyle = BorderStyle.FixedSingle };
        var lblMaKhoa = new Label { Text = "Mã Khoa", Font = new Font("Montserrat", 10), AutoSize = true };
        var txtMaKhoa = new TextBox { Width = 250, Font = new Font("Montserrat", 10), BorderStyle = BorderStyle.FixedSingle };
        var lblTenNganh = new Label { Text = "Tên Ngành", Font = new Font("Montserrat", 10), AutoSize = true };
        var txtTenNganh = new TextBox { Width = 250, Font = new Font("Montserrat", 10), BorderStyle = BorderStyle.FixedSingle };

        if (nganh != null)
        {
            txtMaNganh.Text = nganh.MaNganh.ToString();
            txtMaNganh.Enabled = false;
            txtMaKhoa.Text = nganh.MaKhoa.ToString();
            txtTenNganh.Text = nganh.TenNganh;
        }

        // Layout for fields
        var layout = new TableLayoutPanel
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
        // layout.Controls.Add(lblMaNganh, 0, 0);
        // layout.Controls.Add(txtMaNganh, 1, 0);
        layout.Controls.Add(lblMaKhoa, 0, 1);
        layout.Controls.Add(txtMaKhoa, 1, 1);
        layout.Controls.Add(lblTenNganh, 0, 2);
        layout.Controls.Add(txtTenNganh, 1, 2);

        // Buttons
        var btnCancel = new Button
        {
            Text = "Hủy",
            DialogResult = DialogResult.Cancel,
            Width = 90,
            Height = 32,
            Font = new Font("Montserrat", 10, FontStyle.Regular),
            BackColor = Color.White,
            ForeColor = Color.Black,
            FlatStyle = FlatStyle.Flat,
            Margin = new Padding(0, 20, 10, 0)
        };
        btnCancel.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#B0BEC5");
        btnCancel.FlatAppearance.BorderSize = 1;

        var btnOK = new Button
        {
            Text = nganh == null ? "Thêm" : "Cập nhật",
            DialogResult = DialogResult.OK,
            Width = 90,
            Height = 32,
            Font = new Font("Montserrat", 10, FontStyle.Bold),
            BackColor = ColorTranslator.FromHtml("#07689F"),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Margin = new Padding(10, 20, 0, 0)
        };
        btnOK.FlatAppearance.BorderSize = 0;

        // Button panel
        var buttonPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Bottom,
            // Padding = new Padding(0, 10, 0, 0),
            Height = 50
        };
        buttonPanel.Controls.Add(btnOK);
        buttonPanel.Controls.Add(btnCancel);

        cardPanel.Controls.Add(layout);
        cardPanel.Controls.Add(buttonPanel);
        buttonPanel.BringToFront();

        dialog.Controls.Add(headerPanel);
        dialog.Controls.Add(cardPanel);

        dialog.AcceptButton = btnOK;
        dialog.CancelButton = btnCancel;

        // Center cardPanel vertically
        // cardPanel.Top = (dialog.ClientSize.Height - cardPanel.Height) / 2 + 10;

        // Handle rounded corners for the dialog itself
        dialog.Load += (s, e) =>
        {
            dialog.Region = System.Drawing.Region.FromHrgn(
                NativeMethods.CreateRoundRectRgn(0, 0, dialog.Width, dialog.Height, 18, 18));
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            int maNganh, maKhoa;
            if (!int.TryParse(txtMaNganh.Text.Trim(), out maNganh))
            {
                MessageBox.Show("Mã Ngành phải là số nguyên.");
                return null;
            }
            if (!int.TryParse(txtMaKhoa.Text.Trim(), out maKhoa))
            {
                MessageBox.Show("Mã Khoa phải là số nguyên.");
                return null;
            }
            var tenNganh = txtTenNganh.Text.Trim();
            if (string.IsNullOrEmpty(tenNganh))
            {
                MessageBox.Show("Tên Ngành không được để trống.");
                return null;
            }
            return new Nganh
            {
                MaNganh = maNganh,
                MaKhoa = maKhoa,
                TenNganh = tenNganh
            };
        }
        return null;
    }

    // Native method for rounded corners
    private static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
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