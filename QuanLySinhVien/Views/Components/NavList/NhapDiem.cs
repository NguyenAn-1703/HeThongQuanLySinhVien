using System.Data;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Enums;
namespace QuanLySinhVien.Views.Components;
public class NhapDiem : NavBase
{
    private string[] _listSelectionForComboBox = new []{""};
    
    private DataGridView dataGridView1;
    private DataTable table;
    private CUse _cUse;
    public NhapDiem()
    {
        _cUse = new CUse();
        Init();
        SetupDataGridView();
    }
        
    private void Init()
    {
        //BackColor = Color.Blue;
        Dock = DockStyle.Fill;
        Size = new Size(1200, 900);
        Controls.Add(Bottom());
        Controls.Add(Top());
    }

    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Top,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            //BackColor = Color.Red,
            Height = 120,
        };
        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
            Padding = new Padding(20, 10, 20, 10),
        };
        header.ColumnStyles.Clear();
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.RowStyles.Clear();
        header.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        // Title
        var textNavList = new Label
        {
            Text = "Nhập Điểm",
            AutoSize = true,
        };
        
        var filters = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            WrapContents = false,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Margin = new Padding(0, 8, 12, 0),
        };
        Panel MakeLabeledInput(string labelText, int labelWidth, int inputWidth)
        {
            var panel = new Panel { Width = labelWidth + inputWidth, Height = 36, Margin = new Padding(8, 30, 8, 0) };
            var lbl = new Label { Text = labelText, Width = labelWidth, Dock = DockStyle.Left, TextAlign = ContentAlignment.MiddleLeft };
            var tb  = new TextBox { Width = inputWidth, Dock = DockStyle.Right };
            panel.Controls.Add(tb);
            panel.Controls.Add(lbl);
            return panel;
        }
        filters.Controls.Add(MakeLabeledInput("Học phần", 90, 220));
        filters.Controls.Add(MakeLabeledInput("Lớp", 60, 200));

        // Action button
        var btnAction = new Button
        {
            Text = "Tìm",
            AutoSize = true,
            Padding = new Padding(16, 10, 16, 10),
            Margin = new Padding(0),
            Anchor = AnchorStyles.None,
        };

        // Add to header
        header.Controls.Add(textNavList, 0, 0);
        header.Controls.Add(filters, 1, 0);
        header.Controls.Add(btnAction, 2, 0);
        mainTop.Controls.Add(header);
        return mainTop;
    }

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Fill,
            // BackColor = Color.Green,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Padding = new Padding(20 , 0 , 20 , 0),
        };
        
        
        mainBot.Controls.Add(getDataGridView());
        
        return mainBot;
    }

    private DataGridView getDataGridView()
    {
        dataGridView1 = _cUse.getDataView(710 , 1140 , 20 , 20);
        dataGridView1.ReadOnly = false;
        dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        
        return dataGridView1;
    }
    
    private void SetupDataGridView()
    {
        table = new DataTable();
        
        // add row table
        table.Columns.Add("Mã SV", typeof(string));
        table.Columns.Add("Họ và tên", typeof(string));
        table.Columns.Add("Điểm quá trình", typeof(double));
        table.Columns.Add("Điểm giữa kỳ", typeof(double));
        table.Columns.Add("Điểm cuối kỳ", typeof(double));
        table.Columns.Add("Điểm hệ 10", typeof(double));
        table.Columns.Add("Điểm hệ 4", typeof(double));

        // add database
        for (int i = 0; i < 30; i++)
        {
            table.Rows.Add("3123410025", "Nguyễn âu gia bảo", 1 , 1 , 1 , 1, 1);
        }
        
        // set Width
        dataGridView1.AutoGenerateColumns = true;
        dataGridView1.DataSource = table;
        dataGridView1.RowTemplate.Height = 37;
        // delete border
        dataGridView1.BorderStyle = BorderStyle.None;
        dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
        dataGridView1.RowHeadersVisible = false;
        
        // change Width
        dataGridView1.DataBindingComplete += (s, e) =>
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            int[] weights = {150, 250, 170, 150, 140, 130, 128};
            for (int i = 0; i < dataGridView1.Columns.Count && i < weights.Length; i++)
            {
                var col = dataGridView1.Columns[i];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.FillWeight = weights[i];
                col.MinimumWidth = 60;
            }
        };
        // change header
        dataGridView1.EnableHeadersVisualStyles = false;
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.ColumnHeadersHeight = 45;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = ColorTranslator.FromHtml("#07689F"),
            ForeColor = ColorTranslator.FromHtml("#f5f5f5"),
            //ForeColor = Color.White,
            // Font = new Font("JetBrains Mono", 10f, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleCenter,
        };
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    public override void onSearch(string txtSearch){}

}