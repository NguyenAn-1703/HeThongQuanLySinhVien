using System.Data;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components;
public class NhapDiem : Panel
{
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
        Dock = DockStyle.Bottom;
        Size = new Size(1200, 900);
        var borderTop = new Panel
        {
            Dock = DockStyle.Fill,
            // Padding = new  Padding(0 , 30 , 0 , 0),
        };
        borderTop.Controls.Add(Top());
        Controls.Add(borderTop);
        Controls.Add(Bottom());
    }

    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            //BackColor = Color.Red,
            Height = 120,
        };
        
        // Text Nav_List
        var textNavList = new Label
        {
            Text = "Nhập Điểm",
            // Font = new Font("JetBrains Mono", 16f , FontStyle.Bold),
            Size = new Size(200, 40),
            Location = new Point(20 , 10),
            UseCompatibleTextRendering = false,
        };
        
        // Text Box học phần
        var textHp = new Label
        {
            Text = "Học phần",
            // Font = new Font("JetBrains Mono", 13f, FontStyle.Regular),
            Width = 120,
            Dock = DockStyle.Left,
        };
        var textBoxHp = new TextBox
        {
            // Font = new Font("JetBrains Mono", 13f, FontStyle.Regular),
            Dock = DockStyle.Right,
            Width = 250,
        };

        var textFullHp = new Panel
        {
            Size = new Size(400, 100),
            Location = new Point(90, 60),
            Padding = new  Padding(0,0,0,0),
        };
        
        textFullHp.Controls.Add(textBoxHp);
        textFullHp.Controls.Add(textHp);
        // TextBox lớp
        var textLop = new Label
        {
            Text = "Lớp",
            // Font = new Font("JetBrains Mono", 13f, FontStyle.Regular),
            Width = 80,
            Dock = DockStyle.Left,
        };

        var textBoxLop = new TextBox
        {   
            // Font = new Font("JetBrains Mono", 13f, FontStyle.Regular),
            Dock = DockStyle.Right,
            Width = 240,
        };
        
        var textFullLop = new Panel
        {
            Size = new Size(320, 100),
            Location = new Point(670, 60),
            Padding = new  Padding(0,0,0,0),
        };
        
        textFullLop.Controls.Add(textBoxLop);
        textFullLop.Controls.Add(textLop);
        
        mainTop.Controls.Add(textNavList);
        mainTop.Controls.Add(textFullHp);
        mainTop.Controls.Add(textFullLop);
        return mainTop;
    }

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Bottom,
            // BackColor = Color.Green,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            Height = 750,
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
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.Columns[0]!.Width = 150;
            dataGridView1.Columns[1]!.Width = 250;
            dataGridView1.Columns[2]!.Width = 170;
            dataGridView1.Columns[3]!.Width = 150;
            dataGridView1.Columns[4]!.Width = 140;
            dataGridView1.Columns[5]!.Width = 130;
            dataGridView1.Columns[6]!.Width = 128;
        };
        // change header
        dataGridView1.EnableHeadersVisualStyles = false;
        dataGridView1.ColumnHeadersHeight = 45;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = ColorTranslator.FromHtml("#07689F"),
            ForeColor = ColorTranslator.FromHtml("#f5f5f5"),
            //ForeColor = Color.White,
            // Font = new Font("JetBrains Mono", 10f, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleCenter,
        };
    }
}