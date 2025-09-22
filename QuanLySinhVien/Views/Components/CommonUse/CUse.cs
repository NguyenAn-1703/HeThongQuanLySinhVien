using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CUse
{
    public CUse(){}
    public DataGridView getDataView(int height, int width , int x , int y)
    {
        return new DataGridView
        {
            Location = new Point(x, y),
            Size = new Size(width, height),
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.Fixed3D,
            // Font = new Font("JetBrains Mono", 10f, FontStyle.Regular),
            RowHeadersVisible = false,
            GridColor = ColorTranslator.FromHtml("#2f4f4f"),
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#f5f5f5")
            },
            AllowUserToResizeColumns = false,
            AllowUserToResizeRows = false,
        };
    }

    public DataGridView GetTable(String[] columns)
    {
        DataGridView dgv = new DataGridView
        {
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.Fixed3D,
            RowHeadersVisible = false,
            GridColor = ColorTranslator.FromHtml("#2f4f4f"),
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#f5f5f5")
            },
            AllowUserToResizeColumns = false,
            AllowUserToResizeRows = false,
        };
        SetDefaultsDgvProperty(dgv);
        
        dgv.Dock = DockStyle.Fill;
        dgv.AutoSize = true;
        
        return dgv;
    }
    void SetDefaultsDgvProperty(DataGridView dgv)
    {
        dgv.AllowUserToAddRows = false;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.BackgroundColor = Color.White;
        dgv.BorderStyle = BorderStyle.Fixed3D;
        dgv.RowHeadersVisible = false;
        dgv.GridColor = ColorTranslator.FromHtml("#2f4f4f");
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = ColorTranslator.FromHtml("#f5f5f5")
        };
        dgv.AllowUserToResizeColumns = false;
        dgv.AllowUserToResizeRows = false;
    }
}