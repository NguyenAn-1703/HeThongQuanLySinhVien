using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;
using QuanLySinhVien.Views.Components.ViewComponents;
using Svg;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomDataGridView : DataGridView
{
    private bool _action;
    public event Action<Point> BtnHover;
    public event Action BtnLeave;
    
    private Bitmap editIcon = GetSvgBitmap.GetBitmap("fix.svg");
    private Bitmap deleteIcon = GetSvgBitmap.GetBitmap("trashbin.svg");
    
    //flag cho mousemove
    private bool flag = false;


    private int actionRadius = 10;
    public CustomDataGridView(bool action = false)
    {
        _action = action;
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
    }
    
    void SetActionColumn()
    {
        CellPainting += (sender, args) => DrawBtn(sender, args);
        CellMouseMove +=  (sender, args) => OnHoverCell(sender, args);
        // CellMouseLeave +=  (sender, args) => OnLeaveCell(sender, args);
    }
    
    void DrawBtn(object cell, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == Columns["Hành động"].Index)
        {
            int ButtonHeight = 20;
            int ButtonWidth = 20;
            int Margin = 10;
            int Padding = 3;
            
            //Cai dat cell trong nhu binh thuong
            e.PaintBackground(e.CellBounds, true);
            e.PaintContent(e.CellBounds);
            
            Rectangle rectEdit = new Rectangle(
                e.CellBounds.Left + (e.CellBounds.Width - 2*ButtonWidth - Margin) / 2, 
                e.CellBounds.Top + (e.CellBounds.Height - ButtonHeight) / 2, 
                ButtonWidth, 
                ButtonHeight
                ); 
            Rectangle rectDelete = new Rectangle( 
                rectEdit.Right + Margin, 
                rectEdit.Top, 
                ButtonWidth, 
                ButtonHeight
                );
            Rectangle rectEditBorder = new Rectangle(
                rectEdit.Left - Padding,
                rectEdit.Top - Padding,
                ButtonWidth +  Padding * 2, 
                ButtonHeight + Padding * 2
            ); 
            Rectangle rectDeleteBorder = new Rectangle( 
                rectDelete.Left - Padding,
                rectDelete.Top - Padding,
                ButtonWidth +  Padding * 2, 
                ButtonHeight + Padding * 2
            );

            
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            using (GraphicsPath path = GetRoundedRec(rectEditBorder, actionRadius))
            using (Brush backBrush = new SolidBrush(MyColor.BlueButton))
            {
                g.FillPath(backBrush, path);
            }
            
            //cho giải phóng tài nguyên hệ thống
            using (GraphicsPath path = GetRoundedRec(rectDeleteBorder, actionRadius))
            using (Brush backBrush = new SolidBrush(MyColor.Red))
            {
                g.FillPath(backBrush, path);
            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            
    
            e.Graphics.DrawImage(editIcon,rectEdit);
            e.Graphics.DrawImage(deleteIcon,rectDelete);
            
            e.Handled = true;
            
        }
    }

    void OnHoverCell(object o,  DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == Columns["Hành động"].Index)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            
            int ButtonHeight = 20;
            int ButtonWidth = 20;
            int Margin = 10;
            int Padding = 3;
            
            DataGridView dgv = o as DataGridView;

            //Lấy display vì e kiểu DataGridViewCellEventArgs
            Rectangle cellRect = dgv.GetCellDisplayRectangle(columnIndex, rowIndex, false);
            
            Rectangle rec = new Rectangle(                
                cellRect.Left + (cellRect.Width - 2*ButtonWidth - Margin) / 2 - Padding, 
                cellRect.Top + (cellRect.Height - ButtonHeight) / 2 - Padding, 
                ButtonWidth + Padding * 2, 
                ButtonHeight + Padding * 2);
            
            //Kiểm tra vị trí chuột
            Point mousePos = dgv.PointToClient(Cursor.Position);

            //Gửi tọa độ theo screen cho panel ngoài
            Point screenPos = dgv.PointToScreen(rec.Location);
            
            // xét chuột có nằm ở nút không
            if (rec.Contains(mousePos))
            {
                if (!flag)
                {
                    flag = true;
                    OnHoverButton(screenPos);
                }
            }
            else
            {
                if (flag)
                {
                    flag = false;
                    OnLeaveButton();
                }
            }
        }
    }

    void OnHoverButton(Point rec)
    {
        BtnHover?.Invoke(rec);
    }

    void OnLeaveButton()
    {
        BtnLeave?.Invoke();
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