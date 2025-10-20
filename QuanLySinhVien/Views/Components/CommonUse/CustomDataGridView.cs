using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;
using QuanLySinhVien.Views.Components.ViewComponents;
using Svg;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomDataGridView : DataGridView
{
    private bool _action;
    private bool _edit;
    private bool _delete;
    int ButtonHeight = 20;
    int ButtonWidth = 20;
    int _margin = 10;
    int _padding = 3;
    private int actionRadius = 10;
    public event Action<Point, int> BtnHoverEdit;
    public event Action<Point, int> BtnHoverDelete; 
    public event Action BtnEditLeave;
    public event Action BtnDeleteLeave;
    
    private Bitmap editIcon = GetSvgBitmap.GetBitmap("fix.svg");
    private Bitmap deleteIcon = GetSvgBitmap.GetBitmap("trashbin.svg");
    
    //flag cho mousemove
    private bool flagEdit = false;
    private bool flagDelete = false;
    
    
    public CustomDataGridView(bool action = false, bool edit = false, bool delete = false)
    {
        _action = action;
        _edit = edit;
        _delete = delete;
        Init();
    }

    void Init()
    {
        Configuration();
        if (_action)
        {
            SetActionColumn();
        }
    }

    void Configuration()
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
        this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        CellBorderStyle = DataGridViewCellBorderStyle.None;
        RowTemplate.Height = 35;
        DefaultCellStyle.SelectionBackColor = MyColor.GraySelectColor;
        DefaultCellStyle.SelectionForeColor = Color.Black; 
        BorderStyle = BorderStyle.None;
        ReadOnly = true;
        AutoGenerateColumns = false;

    }
    
    void SetActionColumn()
    {
        CellPainting += (sender, args) => DrawBtn(sender, args);
        CellMouseMove +=  (sender, args) => OnHoverCell(sender, args);
    }
    
    void DrawBtn(object cell, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == Columns["Action"].Index)
        {
            //Cai dat cell trong nhu binh thuong
            e.PaintBackground(e.CellBounds, true);
            e.PaintContent(e.CellBounds);

            Rectangle CellBox = e.CellBounds;

            Rectangle rectEditBorder;
            Rectangle rectDeleteBorder;

            Rectangle rectEdit;
            Rectangle rectDelete;

            SetRectLoactionAndSize(out rectEditBorder, out rectDeleteBorder, out rectEdit,out rectDelete, CellBox);
            
            Graphics g = e.Graphics;

            if (_edit && _delete)
            {
                DrawRecByPath(g,  rectEditBorder, MyColor.BlueButton);
                DrawRecByPath(g,  rectDeleteBorder, MyColor.Red);
            
                DrawIcon(g, editIcon, rectEdit);
                DrawIcon(g, deleteIcon, rectDelete);
            }
            else if (_edit && !_delete)
            {
                DrawRecByPath(g,  rectEditBorder, MyColor.BlueButton);
            
                DrawIcon(g, editIcon, rectEdit);
            }
            else
            {
                DrawRecByPath(g,  rectDeleteBorder, MyColor.Red);
            
                DrawIcon(g, deleteIcon, rectDelete);
            }
            
            // e.Graphics.DrawImage(editIcon,rectEdit);
            // e.Graphics.DrawImage(deleteIcon,rectDelete);
            
            e.Handled = true;
        }
    }

    void SetRectLoactionAndSize(
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
                CellBox.Left + (CellBox.Width - 2 * (ButtonWidth + 2*_padding) - _margin) / 2,
                CellBox.Top + (CellBox.Height - (ButtonHeight + 2*_padding)) / 2,
                ButtonWidth +  _padding * 2, 
                ButtonHeight + _padding * 2
            ); 
            rectDeleteBorder = new Rectangle( 
                rectEditBorder.Right + _margin,
                rectEditBorder.Top,
                ButtonWidth +  _padding * 2, 
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
                CellBox.Left + (CellBox.Width - (ButtonWidth + 2*_padding)) / 2,
                CellBox.Top + (CellBox.Height - (ButtonHeight + 2*_padding)) / 2,
                ButtonWidth +  _padding * 2, 
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
                CellBox.Left + (CellBox.Width - (ButtonWidth + 2*_padding)) / 2,
                CellBox.Top + (CellBox.Height - (ButtonHeight + 2*_padding)) / 2,
                ButtonWidth +  _padding * 2, 
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
        g.DrawImage(svg,rect);
    }

    void OnHoverCell(object o,  DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == Columns["Action"].Index)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            
            DataGridView dgv = o as DataGridView;
            
            //Lấy display vì e kiểu DataGridViewCellEventArgs
            Rectangle cellRect = dgv.GetCellDisplayRectangle(columnIndex, rowIndex, false);

            Rectangle recEdit;
            Rectangle recDelete;
            
            SetActionRegion(out recEdit, out  recDelete, cellRect);
            SetButtonAction(dgv, recEdit, recDelete, e);
        }
    }

    void SetActionRegion(out Rectangle recEdit, out Rectangle recDelete, Rectangle cellRect)
    {
        if (_edit && _delete)
        {
            recEdit = new Rectangle(
                cellRect.Left + (cellRect.Width - 2 * (ButtonWidth + 2*_padding) - _margin) / 2,
                cellRect.Top + (cellRect.Height - (ButtonHeight + 2*_padding)) / 2,
                ButtonWidth +  _padding * 2, 
                ButtonHeight + _padding * 2
            ); 
            recDelete = new Rectangle( 
                recEdit.Right + _margin,
                recEdit.Top,
                ButtonWidth +  _padding * 2, 
                ButtonHeight + _padding * 2
            );
        }
        else if (_edit && !_delete)
        {
            recEdit = new Rectangle(
                cellRect.Left + (cellRect.Width - (ButtonWidth + 2*_padding)) / 2,
                cellRect.Top + (cellRect.Height - (ButtonHeight + 2*_padding)) / 2,
                ButtonWidth +  _padding * 2, 
                ButtonHeight + _padding * 2
            );
            recDelete = new Rectangle();
        }
        else
        {
            recDelete = new Rectangle(
                cellRect.Left + (cellRect.Width - (ButtonWidth + 2*_padding)) / 2,
                cellRect.Top + (cellRect.Height - (ButtonHeight + 2*_padding)) / 2,
                ButtonWidth +  _padding * 2, 
                ButtonHeight + _padding * 2
            );
            recEdit = new Rectangle();
        }
    }

    void SetButtonAction(DataGridView dgv, Rectangle recEdit, Rectangle recDelete, DataGridViewCellMouseEventArgs e)
    {
        int index = e.RowIndex;
        //Kiểm tra vị trí chuột
        Point mousePos = dgv.PointToClient(Cursor.Position);

        //Gửi tọa độ theo screen cho panel ngoài
        Point screenPosEdit = dgv.PointToScreen(recEdit.Location);
        Point screenPosDelete = dgv.PointToScreen(recDelete.Location);
            
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

    void OnHoverButtonEdit(Point rec, int index)
    {
        BtnHoverEdit?.Invoke(rec, index);
    }
    
    void OnHoverButtonDelete(Point rec, int index)
    {
        BtnHoverDelete?.Invoke(rec, index);
    }

    void OnLeaveButtonEdit()
    {
        BtnEditLeave?.Invoke();
    }
    
    void OnLeaveButtonDelete()
    {
        BtnDeleteLeave?.Invoke();
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
    
    
}