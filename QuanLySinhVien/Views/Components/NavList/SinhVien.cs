using System.Data;
using System.Drawing.Drawing2D;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using Svg;

namespace QuanLySinhVien.Views.Components;

public class SinhVien : NavBase
{
    private String[] columns;
    
    private string[] _listSelectionForComboBox = new []{"Mã sinh viên", "Tên sinh viên"};
    
    private DataGridView dataGridView;
    private DataTable table;
    private CUse _cUse;
    public SinhVien()
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
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(20, 10, 20, 10),
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // Phần Button Thêm
        var pathIconPlus = Path.Combine(AppContext.BaseDirectory, "img" , "plus.svg");
        var imgPlus = SvgDocument.Open(pathIconPlus).Draw(25, 25);
        var btnAdd = new Button
        {
            Image = imgPlus,
            Text = "Thêm",
            FlatStyle = FlatStyle.Flat,
            Margin = new Padding(10 , 30 , 0 , 10),
            Size = new  Size(120, 50),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
            // Font = new Font("JetBrains Mono", 10f , FontStyle.Regular),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
        btnAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
        // Text Nav_List
        var textNavList = new Label
        {
            Text = "Sinh Viên",
            // Font = new Font("JetBrains Mono", 16f , FontStyle.Bold),
            Size = new Size(200, 40),
            UseCompatibleTextRendering = false,
        };
        header.Controls.Add(textNavList, 0, 0);
        header.Controls.Add(btnAdd, 1, 0);
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
        dataGridView = _cUse.getDataView(710 , 1140 , 20 , 20);
        dataGridView.ReadOnly = false;
        dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        
        return dataGridView;
    }
    
    private void SetupDataGridView()
    {
        table = new DataTable();
        
        // add row table
        table.Columns.Add("Mã SV", typeof(string));
        table.Columns.Add("Tên Sinh Viên", typeof(string));
        table.Columns.Add("Ngày Sinh", typeof(string));
        table.Columns.Add("Giới tính", typeof(string));
        table.Columns.Add("Ngành", typeof(string));
        table.Columns.Add("Trạng thái", typeof(string));

        // add database
        for (int i = 0; i < 30; i++)
        {
            table.Rows.Add("3123410025", "Nguyễn âu gia bảo", 1 , 1 , 1 , 1);
        }
        
        // set Width
        dataGridView.AutoGenerateColumns = true;
        dataGridView.DataSource = table;
        dataGridView.RowTemplate.Height = 37;
        // delete border
        dataGridView.BorderStyle = BorderStyle.None;
        dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
        dataGridView.RowHeadersVisible = false;
        dataGridView.Dock = DockStyle.Fill;
        // change Width
        bool _actionsAdded = false;
        dataGridView.DataBindingComplete += (s, e) =>
        {
            if (_actionsAdded) return;
            _actionsAdded = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            int[] weights = {120, 220, 100, 80, 100 , 100};
            for (int i = 0; i < dataGridView.Columns.Count && i < weights.Length; i++)
            {
                var col = dataGridView.Columns[i];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.FillWeight = weights[i];
                col.MinimumWidth = 60;
            }
            
            var editIcon = _cUse.CreateIconWithBackground(
                Path.Combine(AppContext.BaseDirectory,"img","fix.svg"),
                Color.Black,
                ColorTranslator.FromHtml("#6DB7E3"),
                28,
                6,
                4
            );
            var delIcon  = _cUse.CreateIconWithBackground(
                Path.Combine(AppContext.BaseDirectory,"img","trashbin.svg"),
                Color.Black,
                ColorTranslator.FromHtml("#FF6B6B"),
                28,
                6,
                4
            );
            var colEdit = new DataGridViewImageColumn {
                Name = "Edit",
                Image = editIcon,
                DataPropertyName = null,
                Width = 60,
                ImageLayout = DataGridViewImageCellLayout.Normal
            };
            
            var colDel = new DataGridViewImageColumn {
                Name = "Del",
                Image = delIcon,
                DataPropertyName = null,
                Width = 60,
                ImageLayout = DataGridViewImageCellLayout.Normal
            };
            dataGridView.Columns.Add(colEdit);
            dataGridView.Columns.Add(colDel);
            dataGridView.Columns["Edit"]!.DisplayIndex   = 6;
            dataGridView.Columns["Del"]!.DisplayIndex = 7;
            dataGridView.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex == dataGridView.Columns["Edit"]?.Index) { Console.WriteLine("sua"); }
                else if (e.ColumnIndex == dataGridView.Columns["Del"]?.Index) { Console.WriteLine("xoa"); }
            };
        };
        
        // change header
        dataGridView.EnableHeadersVisualStyles = false;
        dataGridView.ColumnHeadersHeight = 45;
        dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
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
    
    public override void onSearch(string txtSearch, string filter)
    { }
    
}