using OpenTK.Graphics.OpenGL;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class StatisticalTop5Box : TableLayoutPanel
{
    private Label _title;
    private string[] _listTen;
    private float[] _listPhanTram;
    
    public StatisticalTop5Box(string title, string[] dsTen, float[] dsPhanTram)
    {
        _title = new Label();
        _title.Text = title;
        _listTen = dsTen;
        _listPhanTram = dsPhanTram;
        this.Init();
    }

    void Init()
    {
        this.Dock = DockStyle.Fill;
        this.AutoSize = true;
        
        this.ColumnCount = 2;
        this.RowCount = 6;

        _title.AutoSize = true;
        _title.Font = GetFont.GetFont.GetMainFont(13, FontType.Bold);
        
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        this.Controls.Add(_title);
        this.SetColumnSpan(_title, 2);
        
        for (int i = 0; i < _listPhanTram.Length; i++)
        {
            Label lbl1 = new Label();
            Label lbl2 = new Label();
            lbl1.AutoSize = true;
            lbl2.AutoSize = true;
            
            lbl1.Text = _listTen[i];
            lbl2.Text = _listPhanTram[i] + "";

            lbl1.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
            lbl2.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
            
            
            this.Controls.Add(lbl1);
            this.Controls.Add(lbl2);
        }

        this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;


    }
    
}