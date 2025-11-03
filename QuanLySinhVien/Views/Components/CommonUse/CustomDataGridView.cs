using System.Drawing.Drawing2D;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomDataGridView : DataGridView
{
    private readonly bool _action;
    private readonly bool _delete;
    private readonly bool _edit;
    private readonly int _margin = 10;
    private readonly int _padding = 3;
    private readonly int actionRadius = 10;
    private readonly int ButtonHeight = 20;
    private readonly int ButtonWidth = 20;
    private readonly Bitmap deleteIcon = GetSvgBitmap.GetBitmap("trashbin.svg");

    private readonly Bitmap editIcon = GetSvgBitmap.GetBitmap("fix.svg");
    private bool flagDelete;

    //flag cho mousemove
    private bool flagEdit;

    private int i = 0;


    public CustomDataGridView(bool action = false, bool edit = false, bool delete = false)
    {
        _action = action;
        _edit = edit;
        _delete = delete;
        Init();
    }

    public event Action<Point, int> BtnHoverEdit;
    public event Action<Point, int> BtnHoverDelete;
    public event Action BtnEditLeave;
    public event Action BtnDeleteLeave;

    private void Init()
    {
        Configuration();
        if (_action) SetActionColumn();
    }

    private void Configuration()
    {
        Margin = new Padding(0);
        DoubleBuffered = true;
        AllowUserToAddRows = false;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        AllowUserToResizeColumns = false;
        AllowUserToResizeRows = false;
        Dock = DockStyle.Fill;
        BackgroundColor = Color.White;
        RowHeadersVisible = false;
        ColumnHeadersVisible = false;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = ColorTranslator.FromHtml("#f5f5f5")
        };
        DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        CellBorderStyle = DataGridViewCellBorderStyle.None;
        RowTemplate.Height = 35;
        DefaultCellStyle.SelectionBackColor = MyColor.GraySelectColor;
        DefaultCellStyle.SelectionForeColor = Color.Black;
        BorderStyle = BorderStyle.None;
        ReadOnly = true;
        AutoGenerateColumns = false;
        ShowCellToolTips = false;
    }

    private void SetActionColumn()
    {
        CellPainting += (sender, args) => DrawBtn(sender, args);
        CellMouseMove += (sender, args) => OnHoverCell(sender, args);
        CellMouseLeave += (sender, args) => OnLeaveCell(args.RowIndex, args.ColumnIndex);
        MouseLeave += (sender, args) => OnLeaveDgv();
    }

    private void DrawBtn(object cell, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == Columns["Action"].Index)
        {
            //Cai dat cell trong nhu binh thuong
            e.PaintBackground(e.CellBounds, true);
            e.PaintContent(e.CellBounds);

            var CellBox = e.CellBounds;

            Rectangle rectEditBorder;
            Rectangle rectDeleteBorder;

            Rectangle rectEdit;
            Rectangle rectDelete;

            SetRectLoactionAndSize(out rectEditBorder, out rectDeleteBorder, out rectEdit, out rectDelete, CellBox);

            var g = e.Graphics;

            if (_edit && _delete)
            {
                DrawRecByPath(g, rectEditBorder, MyColor.BlueButton);
                DrawRecByPath(g, rectDeleteBorder, MyColor.Red);

                DrawIcon(g, editIcon, rectEdit);
                DrawIcon(g, deleteIcon, rectDelete);
            }
            else if (_edit && !_delete)
            {
                DrawRecByPath(g, rectEditBorder, MyColor.BlueButton);

                DrawIcon(g, editIcon, rectEdit);
            }
            else
            {
                DrawRecByPath(g, rectDeleteBorder, MyColor.Red);

                DrawIcon(g, deleteIcon, rectDelete);
            }

            // e.Graphics.DrawImage(editIcon,rectEdit);
            // e.Graphics.DrawImage(deleteIcon,rectDelete);

            e.Handled = true;
        }
    }

    private void SetRectLoactionAndSize(
        out Rectangle rectEditBorder,
        out Rectangle rectDeleteBorder,
        out Rectangle rectEdit,
        out Rectangle rectDelete,
        Rectangle CellBox
    )
    {
        if (_edit && _delete)
        {
            rectEditBorder = new Rectangle(
                CellBox.Left + (CellBox.Width - 2 * (ButtonWidth + 2 * _padding) - _margin) / 2,
                CellBox.Top + (CellBox.Height - (ButtonHeight + 2 * _padding)) / 2,
                ButtonWidth + _padding * 2,
                ButtonHeight + _padding * 2
            );
            rectDeleteBorder = new Rectangle(
                rectEditBorder.Right + _margin,
                rectEditBorder.Top,
                ButtonWidth + _padding * 2,
                ButtonHeight + _padding * 2
            );

            rectEdit = new Rectangle(
                rectEditBorder.Left + _padding,
                rectEditBorder.Top + _padding,
                ButtonWidth,
                ButtonHeight
            );
            rectDelete = new Rectangle(
                rectDeleteBorder.Left + _padding,
                rectDeleteBorder.Top + _padding,
                ButtonWidth,
                ButtonHeight
            );
        }
        else if (_edit && !_delete)
        {
            rectDeleteBorder = new Rectangle();
            rectDelete = new Rectangle();
            rectEditBorder = new Rectangle(
                CellBox.Left + (CellBox.Width - (ButtonWidth + 2 * _padding)) / 2,
                CellBox.Top + (CellBox.Height - (ButtonHeight + 2 * _padding)) / 2,
                ButtonWidth + _padding * 2,
                ButtonHeight + _padding * 2
            );
            rectEdit = new Rectangle(
                rectEditBorder.Left + _padding,
                rectEditBorder.Top + _padding,
                ButtonWidth,
                ButtonHeight
            );
        }
        else
        {
            rectEditBorder = new Rectangle();
            rectEdit = new Rectangle();
            rectDeleteBorder = new Rectangle(
                CellBox.Left + (CellBox.Width - (ButtonWidth + 2 * _padding)) / 2,
                CellBox.Top + (CellBox.Height - (ButtonHeight + 2 * _padding)) / 2,
                ButtonWidth + _padding * 2,
                ButtonHeight + _padding * 2
            );
            rectDelete = new Rectangle(
                rectDeleteBorder.Left + _padding,
                rectDeleteBorder.Top + _padding,
                ButtonWidth,
                ButtonHeight
            );
        }
    }

    private void DrawRecByPath(Graphics g, Rectangle rect, Color color)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = GetRoundedRec(rect, actionRadius))
        using (Brush backBrush = new SolidBrush(color))
        {
            g.FillPath(backBrush, path);
        }

        g.SmoothingMode = SmoothingMode.None;
    }

    private void DrawIcon(Graphics g, Bitmap svg, Rectangle rect)
    {
        g.DrawImage(svg, rect);
    }

    private void OnHoverCell(object o, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == Columns["Action"].Index)
        {
            var rowIndex = e.RowIndex;
            var columnIndex = e.ColumnIndex;

            var dgv = o as DataGridView;

            //Lấy display vì e kiểu DataGridViewCellEventArgs
            var cellRect = dgv.GetCellDisplayRectangle(columnIndex, rowIndex, false);

            Rectangle recEdit;
            Rectangle recDelete;

            SetActionRegion(out recEdit, out recDelete, cellRect);
            SetButtonAction(dgv, recEdit, recDelete, e);
        }
    }


    private void SetActionRegion(out Rectangle recEdit, out Rectangle recDelete, Rectangle cellRect)
    {
        if (_edit && _delete)
        {
            recEdit = new Rectangle(
                cellRect.Left + (cellRect.Width - 2 * (ButtonWidth + 2 * _padding) - _margin) / 2,
                cellRect.Top + (cellRect.Height - (ButtonHeight + 2 * _padding)) / 2,
                ButtonWidth + _padding * 2,
                ButtonHeight + _padding * 2
            );
            recDelete = new Rectangle(
                recEdit.Right + _margin,
                recEdit.Top,
                ButtonWidth + _padding * 2,
                ButtonHeight + _padding * 2
            );
        }
        else if (_edit && !_delete)
        {
            recEdit = new Rectangle(
                cellRect.Left + (cellRect.Width - (ButtonWidth + 2 * _padding)) / 2,
                cellRect.Top + (cellRect.Height - (ButtonHeight + 2 * _padding)) / 2,
                ButtonWidth + _padding * 2,
                ButtonHeight + _padding * 2
            );
            recDelete = new Rectangle();
        }
        else
        {
            recDelete = new Rectangle(
                cellRect.Left + (cellRect.Width - (ButtonWidth + 2 * _padding)) / 2,
                cellRect.Top + (cellRect.Height - (ButtonHeight + 2 * _padding)) / 2,
                ButtonWidth + _padding * 2,
                ButtonHeight + _padding * 2
            );
            recEdit = new Rectangle();
        }
    }

    private void SetButtonAction(DataGridView dgv, Rectangle recEdit, Rectangle recDelete,
        DataGridViewCellMouseEventArgs e)
    {
        var index = e.RowIndex;
        //Kiểm tra vị trí chuột
        var mousePos = dgv.PointToClient(Cursor.Position);

        //Gửi tọa độ theo screen cho panel ngoài
        var screenPosEdit = dgv.PointToScreen(recEdit.Location);
        var screenPosDelete = dgv.PointToScreen(recDelete.Location);

        // xét chuột có nằm ở nút không
        if (recEdit.Contains(mousePos))
        {
            if (!flagEdit)
            {
                flagEdit = true;
                OnHoverButtonEdit(screenPosEdit, index);
            }
        }
        else
        {
            if (flagEdit)
            {
                flagEdit = false;
                OnLeaveButtonEdit();
            }
        }

        if (recDelete.Contains(mousePos))
        {
            if (!flagDelete)
            {
                flagDelete = true;
                OnHoverButtonDelete(screenPosDelete, index);
            }
        }
        else
        {
            if (flagDelete)
            {
                flagDelete = false;
                OnLeaveButtonDelete();
            }
        }
    }

    private void OnLeaveCell(int rowIndex, int columnIndex)
    {
        var cellRect = GetCellDisplayRectangle(columnIndex, rowIndex, false);
        var cellScreenLocation = PointToScreen(cellRect.Location);
        var cellScreenRect = new Rectangle(cellScreenLocation, cellRect.Size);

        // Lấy tọa độ con trỏ chuột hiện tại trên màn hình
        var cursorPos = Cursor.Position;

        // Chỉ thực hiện khi chuột thật sự rời cell
        if (!cellScreenRect.Contains(cursorPos))
        {
            if (flagEdit)
            {
                flagEdit = false;
                OnLeaveButtonEdit();
            }

            if (flagDelete)
            {
                flagDelete = false;
                OnLeaveButtonDelete();
            }
        }
    }

    private void OnLeaveDgv()
    {
        // Lấy vị trí chuột hiện tại trên màn hình
        var cursorPos = Cursor.Position;

        // Lấy vùng CustomDataGridView trên màn hình
        var dgvScreenRect = RectangleToScreen(ClientRectangle);

        // Nếu chuột vẫn còn trong vùng dgv thì không làm gì
        if (!dgvScreenRect.Contains(cursorPos))
        {
            if (flagEdit)
            {
                flagEdit = false;
                OnLeaveButtonEdit();
            }

            if (flagDelete)
            {
                flagDelete = false;
                OnLeaveButtonDelete();
            }
        }
    }

    private void OnHoverButtonEdit(Point rec, int index)
    {
        BtnHoverEdit?.Invoke(rec, index);
    }

    private void OnHoverButtonDelete(Point rec, int index)
    {
        BtnHoverDelete?.Invoke(rec, index);
    }

    private void OnLeaveButtonEdit()
    {
        BtnEditLeave?.Invoke();
    }

    private void OnLeaveButtonDelete()
    {
        BtnDeleteLeave?.Invoke();
    }


    private GraphicsPath GetRoundedRec(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();

        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
        path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
        path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

        path.CloseFigure();
        return path;
    }
}