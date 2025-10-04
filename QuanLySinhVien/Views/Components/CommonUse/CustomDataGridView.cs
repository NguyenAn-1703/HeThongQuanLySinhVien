using System.Windows.Forms.VisualStyles;
using QuanLySinhVien.Views.Components.ViewComponents;
using Svg;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomDataGridView : DataGridView
{
    private bool _action;
    private Bitmap editIcon = GetSvgBitmap.GetBitmap("fix.svg");
    private Bitmap deleteIcon = GetSvgBitmap.GetBitmap("trashbin.svg");
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
        ReadOnly = true;
    }
    
    void SetActionColumn()
    {
        CellPainting += (sender, args) => DrawBtn(args);
    }
    
    void DrawBtn(DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == Columns["Hành động"].Index)
        {
            int ButtonHeight = 20;
            int ButtonWidth = 20;
            int Margin = 5;
            
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
    
            e.Graphics.DrawImage(editIcon,rectEdit);
            e.Graphics.DrawImage(deleteIcon,rectDelete);
            
            e.Handled = true;
            
        }
    }
    
    
}