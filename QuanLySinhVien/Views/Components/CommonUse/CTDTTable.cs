using System.Drawing.Drawing2D;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CTDTTable : CustomTable
{
    int ButtonHeight = 20;
    int ButtonWidth = 20;
    int _margin = 10;
    int _padding = 3;
    private int actionRadius = 10;

    private Bitmap plusIcon = GetSvgBitmap.GetBitmap("plus.svg");
    private Bitmap minusIcon = GetSvgBitmap.GetBitmap("minus.svg");
    
    private Form _topForm;
    
    private CustomButton _Btn;
    private CustomButton _minusBtn;

    private bool flagBtn;
    private bool flagBtnMinus;

    private TableCTDTType _type;
    private string _columnName;


    public event Action<int> BtnClick;
    public CTDTTable(List<string> headerContent, List<string> columnNames, List<object> cells, TableCTDTType type)
        : base(headerContent, columnNames, cells,
            false, false, false)
    {
        _type = type;
        Init();
    }

    void Init()
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

    void SetActionColumn()
    {
        _dataGridView.CellPainting += (sender, args) => DrawBtn(sender, args);
        _dataGridView.CellMouseMove += (sender, args) => OnHoverCell(sender, args);
    }

    void DrawBtn(object cell, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == _dataGridView.Columns[_columnName].Index)
        {
            //Cai dat cell trong nhu binh thuong
            e.PaintBackground(e.CellBounds, true);
            e.PaintContent(e.CellBounds);

            Rectangle CellBox = e.CellBounds;

            Rectangle rectBorder;
            Rectangle rect;

            SetRectLoactionAndSize(out rectBorder, out rect, CellBox);

            Graphics g = e.Graphics;

            if (_type == TableCTDTType.Plus)
            {
                DrawRecByPath(g, rectBorder, MyColor.SkyBlue);
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

    void SetRectLoactionAndSize(
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

    void DrawRecByPath(Graphics g, Rectangle rect, Color color)
    {
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        using (GraphicsPath path = GetRoundedRec(rect, actionRadius))
        using (Brush backBrush = new SolidBrush(color))
        {
            g.FillPath(backBrush, path);
        }

        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
    }

    void DrawIcon(Graphics g, Bitmap svg, Rectangle rect)
    {
        g.DrawImage(svg, rect);
    }

    void OnHoverCell(object o, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == _dataGridView.Columns[_columnName].Index)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;

            DataGridView dgv = o as DataGridView;

            //Lấy display vì e kiểu DataGridViewCellEventArgs
            Rectangle cellRect = dgv.GetCellDisplayRectangle(columnIndex, rowIndex, false);

            Rectangle recPlus;

            SetActionRegion(out recPlus, cellRect);
            SetButtonAction(dgv, recPlus, e);
        }
    }

    void SetActionRegion(out Rectangle recPlus, Rectangle cellRect)
    {
        recPlus = new Rectangle(
            cellRect.Left + (cellRect.Width - (ButtonWidth + 2 * _padding)) / 2,
            cellRect.Top + (cellRect.Height - (ButtonHeight + 2 * _padding)) / 2,
            ButtonWidth + _padding * 2,
            ButtonHeight + _padding * 2
        );
    }

    void SetButtonAction(DataGridView dgv, Rectangle recPlus, DataGridViewCellMouseEventArgs e)
    {
        int index = e.RowIndex;
        //Kiểm tra vị trí chuột
        Point mousePos = dgv.PointToClient(Cursor.Position);

        Point screenPosPlus = dgv.PointToScreen(recPlus.Location);

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

    void OnHoverBtn(Point rec, int index)
    {
        int rowIndex = index;
        //Vẽ vào form, không phụ thuộc layout
        _topForm = this.FindForm();

        if (_type == TableCTDTType.Plus)
        {
            _Btn = new CustomButton(20, 20, "plus.svg", MyColor.MainColor);
        }
        else
        {
            _Btn = new CustomButton(20, 20, "minus.svg", MyColor.RedHover);
        }
        

        Point myPoint = _topForm.PointToClient(rec);

        _Btn.Location = myPoint;

        _topForm.Controls.Add(_Btn);

        _Btn.BringToFront();

        _Btn.MouseDown += (sender, args) => OnClickBtn(index);
    }

    void OnLeaveBtn()
    {
        _Btn.Dispose();
    }

    void OnClickBtn(int index)
    {
        int Id = (int)_dataGridView.Rows[index].Cells[0].Value;
        _Btn.Dispose();
        flagBtn = false;
        BtnClick?.Invoke(Id);
    } 


    GraphicsPath GetRoundedRec(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();

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