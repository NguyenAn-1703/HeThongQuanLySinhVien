using System.Drawing.Drawing2D;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class CTDTTable : CustomTable
{
    private readonly int _padding = 3;

    private readonly TableCTDTType _type;
    private readonly int actionRadius = 10;
    private readonly int ButtonHeight = 20;
    private readonly int ButtonWidth = 20;
    private readonly Bitmap minusIcon = GetSvgBitmap.GetBitmap("minus.svg");

    private readonly Bitmap plusIcon = GetSvgBitmap.GetBitmap("plus.svg");

    private CustomButton _Btn;
    private string _columnName;
    private int _margin = 10;
    private CustomButton _minusBtn;

    private Form _topForm;

    private bool flagBtn;
    private bool flagBtnMinus;

    public CTDTTable(List<string> headerContent, List<string> columnNames, List<object> cells, TableCTDTType type)
        : base(headerContent, columnNames, cells)
    {
        _type = type;
        Init();
    }


    public event Action<int> BtnClick;

    private void Init()
    {
        if (_type == TableCTDTType.Plus)
        {
            _columnName = "ActionPlus";
            SetActionColumn();
        }

        if (_type == TableCTDTType.Minus)
        {
            _columnName = "ActionMinus";
            SetActionColumn();
        }
    }

    private void SetActionColumn()
    {
        _dataGridView.CellPainting += (sender, args) => DrawBtn(sender, args);
        _dataGridView.CellMouseMove += (sender, args) => OnHoverCell(sender, args);
    }

    private void DrawBtn(object cell, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == _dataGridView.Columns[_columnName].Index)
        {
            //Cai dat cell trong nhu binh thuong
            e.PaintBackground(e.CellBounds, true);
            e.PaintContent(e.CellBounds);

            var CellBox = e.CellBounds;

            Rectangle rectBorder;
            Rectangle rect;

            SetRectLoactionAndSize(out rectBorder, out rect, CellBox);

            var g = e.Graphics;

            if (_type == TableCTDTType.Plus)
            {
                DrawRecByPath(g, rectBorder, MyColor.BlueButton);
                DrawIcon(g, plusIcon, rect);
            }
            else
            {
                DrawRecByPath(g, rectBorder, MyColor.Red);
                DrawIcon(g, minusIcon, rect);
            }


            e.Handled = true;
        }
    }

    private void SetRectLoactionAndSize(
        out Rectangle rectBorder,
        out Rectangle rect,
        Rectangle CellBox
    )
    {
        rectBorder = new Rectangle();
        rect = new Rectangle();
        rectBorder = new Rectangle(
            CellBox.Left + (CellBox.Width - (ButtonWidth + 2 * _padding)) / 2,
            CellBox.Top + (CellBox.Height - (ButtonHeight + 2 * _padding)) / 2,
            ButtonWidth + _padding * 2,
            ButtonHeight + _padding * 2
        );
        rect = new Rectangle(
            rectBorder.Left + _padding,
            rectBorder.Top + _padding,
            ButtonWidth,
            ButtonHeight
        );
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
        if (e.RowIndex >= 0 && e.ColumnIndex == _dataGridView.Columns[_columnName].Index)
        {
            var rowIndex = e.RowIndex;
            var columnIndex = e.ColumnIndex;

            var dgv = o as DataGridView;

            //Lấy display vì e kiểu DataGridViewCellEventArgs
            var cellRect = dgv.GetCellDisplayRectangle(columnIndex, rowIndex, false);

            Rectangle recPlus;

            SetActionRegion(out recPlus, cellRect);
            SetButtonAction(dgv, recPlus, e);
        }
    }

    private void SetActionRegion(out Rectangle recPlus, Rectangle cellRect)
    {
        recPlus = new Rectangle(
            cellRect.Left + (cellRect.Width - (ButtonWidth + 2 * _padding)) / 2,
            cellRect.Top + (cellRect.Height - (ButtonHeight + 2 * _padding)) / 2,
            ButtonWidth + _padding * 2,
            ButtonHeight + _padding * 2
        );
    }

    private void SetButtonAction(DataGridView dgv, Rectangle recPlus, DataGridViewCellMouseEventArgs e)
    {
        var index = e.RowIndex;
        //Kiểm tra vị trí chuột
        var mousePos = dgv.PointToClient(Cursor.Position);

        var screenPosPlus = dgv.PointToScreen(recPlus.Location);

        // xét chuột có nằm ở nút không
        if (recPlus.Contains(mousePos))
        {
            if (!flagBtn)
            {
                flagBtn = true;
                OnHoverBtn(screenPosPlus, index);
            }
        }
        else
        {
            if (flagBtn)
            {
                flagBtn = false;
                OnLeaveBtn();
            }
        }
    }

    private void OnHoverBtn(Point rec, int index)
    {
        var rowIndex = index;
        //Vẽ vào form, không phụ thuộc layout
        _topForm = FindForm();

        if (_type == TableCTDTType.Plus)
            _Btn = new CustomButton(20, 20, "plus.svg", MyColor.MainColor);
        else
            _Btn = new CustomButton(20, 20, "minus.svg", MyColor.RedHover);

        _topForm.Controls.Add(_Btn);
        _Btn.BringToFront();

        var myPoint = _topForm.PointToClient(rec);
        _Btn.Location = myPoint;

        var cursorPos = Cursor.Position;
        var cursorOnForm = _topForm.PointToClient(cursorPos);

        // Nếu chuột đang nằm trong vùng nút thì hiển thị, nếu không thì ẩn
        if (_Btn.Bounds.Contains(cursorOnForm))
            _Btn.Visible = true;
        else
            _Btn.Visible = false;
        _Btn.MouseLeave += (sender, args) => _Btn.Visible = false;


        _Btn.MouseDown += (sender, args) => OnClickBtn(index);
    }

    private void OnLeaveBtn()
    {
        _Btn.Dispose();
    }

    private void OnClickBtn(int index)
    {
        var Id = (int)_dataGridView.Rows[index].Cells[0].Value;
        _Btn.Dispose();
        flagBtn = false;
        BtnClick?.Invoke(Id);
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

    public void EnableActionColumn()
    {
        _dataGridView.Columns[_columnName].Visible = false;
        _header.Controls.RemoveAt(_header.Controls.Count - 1);
    }
}