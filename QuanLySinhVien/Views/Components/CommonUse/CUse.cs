namespace QuanLySinhVien.Views.Components.CommonUse;

public class CUse
{
    public CUse(){}
    public DataGridView getDataView(int height , int width , int x , int y)
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
}