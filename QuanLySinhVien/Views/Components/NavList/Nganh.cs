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


        addButton.MouseClick += (sender, e) => { MessageBox.Show("Thêm"); };

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

                if (clickX >= (cellRect.Width - totalWidth) / 2 &&
                    clickX <= (cellRect.Width - totalWidth) / 2 + iconSize)
                    MessageBox.Show("Sửa");
                else if (clickX >= (cellRect.Width - totalWidth) / 2 + iconSize + spacing &&
                         clickX <= (cellRect.Width - totalWidth) / 2 + totalWidth)
                    MessageBox.Show("Xóa");
            }
        };

        mainBot.Controls.Add(dataGrid);

        return mainBot;
    }

    private void LoadData()
    {
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();

            // Nếu chưa có dữ liệu, thêm mẫu
            if (!db.nganh.Any())
            {
                db.nganh.Add(new Nganh { MaKhoa = 1, TenNganh = "Công nghệ thông tin" });
                db.nganh.Add(new Nganh { MaKhoa = 2, TenNganh = "Kế toán" });
                db.SaveChanges();
            }

            // Gán dữ liệu cho DataGridView
            dataGrid.DataSource = db.nganh.ToList();
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