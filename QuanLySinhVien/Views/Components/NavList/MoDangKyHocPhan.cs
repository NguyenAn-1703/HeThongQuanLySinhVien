using System.Data;
using QuanLySinhVien.Views.Components.CommonUse;
using Svg;
namespace QuanLySinhVien.Views.Components;

public class MoDangKyHocPhan : Panel
{
    private DataGridView dataGridView;
    private DataTable table;
    private TableLayoutPanel tableLayout;
    private Panel topCenter, botCenter;
    private CUse _cUse; 
    public MoDangKyHocPhan()
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
            Text = "Mở đăng ký học phần",
            // Font = new Font("JetBrains Mono", 16f , FontStyle.Bold),
            Size = new Size(400, 40),
            Location = new Point(20 , 10),
        };
        // Học kỳ
        var textHk = new Label
        {
            Text = "Học Kỳ:",
            // Font = new Font("JetBrains Mono", 12f , FontStyle.Regular),
            Size = new Size(100, 60),
            Margin = new Padding(0, 12, 8, 0)
        };
        var labels = new[] { "1", "2", "3" };
        var cbHk = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            // Font = new Font("JetBrains Mono", 10f),
            Size = new Size(300, 0),
            DrawMode = DrawMode.Normal,
            Margin = new Padding(0, 12, 8, 0),
        };
        cbHk.Items.AddRange(labels);
        var pathIconHk = Path.Combine(AppContext.BaseDirectory, "img" , "plus_circle.svg");
        var img = SvgDocument.Open(pathIconHk).Draw(25, 25);
        var btnHk = new Button
        {       
            Image = img,
            FlatStyle = FlatStyle.Flat,
            Margin = new Padding(0),
            Size = new  Size(60, 50),
        };
        btnHk.FlatAppearance.BorderSize = 0;
        var panelFull = new FlowLayoutPanel
        {
            Location = new Point(500 , 10),
            FlowDirection = FlowDirection.LeftToRight,
            Size = new Size(1000, 100),
            AutoSize = true,
        };
        panelFull.Controls.Add(textHk);
        panelFull.Controls.Add(cbHk);
        panelFull.Controls.Add(btnHk);
        
        // Phần Button Thêm
        var pathIconPlus = Path.Combine(AppContext.BaseDirectory, "img" , "plus.svg");
        var imgPlus = SvgDocument.Open(pathIconPlus).Draw(25, 25);
        var btnAdd = new Button
        {
            Image = imgPlus,
            Text = "Thêm",
            FlatStyle = FlatStyle.Flat,
            Margin = new Padding(0),
            Size = new  Size(120, 50),
            Location = new Point(1040, 60),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
            // Font = new Font("JetBrains Mono", 10f , FontStyle.Regular),
        };
        btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
        btnAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
        
        // Text các lớp mở đăng ký
        var textClmdk = new Label()
        {
            Text = "Các lớp mở đăng ký",
            // Font = new Font("JetBrains Mono", 10f , FontStyle.Regular),
            Location = new Point(20 , 80),
            Size = new Size(200, 40),
        };
        
        // add components
        mainTop.Controls.Add(textClmdk);
        mainTop.Controls.Add(btnAdd);
        mainTop.Controls.Add(textNavList);
        mainTop.Controls.Add(panelFull);
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
        //TODO: Chia thành table layout có 2 hàng 1 cái của bảng 1 cái của lịch
        BuildTableLayout();
        topCenter = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };
        botCenter = new Panel { Dock = DockStyle.Fill, BackColor = Color.MistyRose };
        topCenter.Controls.Add(getDataGridView());
        tableLayout.Controls.Add(topCenter, 0 , 0);
        tableLayout.Controls.Add(botCenter,0 ,1);
        mainBot.Controls.Add(tableLayout);
        return mainBot;
    }

    private void BuildTableLayout()
    {
        tableLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(0),
        };
        tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
        tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
    }

    private DataGridView getDataGridView()
    {
        dataGridView = _cUse.getDataView(520 , 1140 , 0 , 0);
        dataGridView.ReadOnly = true;
        dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        return dataGridView;
    }

    private Panel CreateAction()
    {
        var pathIconFix = Path.Combine(AppContext.BaseDirectory, "img" , "plus.svg");
        var imgFix = SvgDocument.Open(pathIconFix).Draw(25, 25);
        var btnFix = new Button
        {
            FlatStyle = FlatStyle.Flat,
            Image = imgFix,
            ImageAlign = ContentAlignment.MiddleLeft,
            Size = new Size(30, 30),
        };
        var pathIconDel = Path.Combine(AppContext.BaseDirectory, "img" , "plus.svg");
        var imgDel = SvgDocument.Open(pathIconDel).Draw(25, 25);
        var btnDel = new Button
        {
            FlatStyle = FlatStyle.Flat,
            Image = imgDel,
            ImageAlign = ContentAlignment.MiddleRight,
            Size = new Size(30, 30),
        };
        Panel mainButton = new Panel
        {
            Dock = DockStyle.Fill,
        };
        mainButton.Controls.Add(btnFix);
        mainButton.Controls.Add(btnDel);
        return mainButton;
    }
    

    private void SetupDataGridView()
    {
        table = new DataTable();
        
        // add row table
        table.Columns.Add("Mã HP", typeof(string));
        table.Columns.Add("Tên Học phần", typeof(string));
        table.Columns.Add("Phòng", typeof(string));
        table.Columns.Add("Thứ", typeof(int));
        table.Columns.Add("Tiết BD", typeof(int));
        table.Columns.Add("Số tiết", typeof(int));
        table.Columns.Add("Giảng viên", typeof(string));
        table.Columns.Add("Sĩ số", typeof(int));
        //table.Columns.Add("Hành động", typeof(Panel));

        // add database
        for (int i = 0; i < 30; i++)
        {
            table.Rows.Add("3123410025", "Nguyễn âu gia bảo", 1 , 1 , 1 , 1, 1 , 1 );
        }
        
        // set Width
        dataGridView.AutoGenerateColumns = true;
        dataGridView.DataSource = table;
        dataGridView.RowTemplate.Height = 37;
        // delete border
        dataGridView.BorderStyle = BorderStyle.None;
        dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
        dataGridView.RowHeadersVisible = false;
        // change Width
        bool _actionsAdded = false;
        dataGridView.DataBindingComplete += (s, e) =>
        {
            if (_actionsAdded) return;
            _actionsAdded = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView.Columns[0].Width = 120;
            dataGridView.Columns[1].Width = 220;
            dataGridView.Columns[2].Width = 100;
            dataGridView.Columns[3].Width = 80;
            dataGridView.Columns[4].Width = 100;
            dataGridView.Columns[5].Width = 100;
            dataGridView.Columns[6].Width = 148;
            dataGridView.Columns[7].Width = 100;
            
            var editIcon = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory,"img","fix.svg")).Draw(20, 20);
            var delIcon  = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory,"img","trashbin.svg")).Draw(20, 20);
            var colEdit = new DataGridViewImageColumn {
                Name = "Edit",
                Image = editIcon,
                DataPropertyName = null,
                Width = 75
            };
            var colDel = new DataGridViewImageColumn {
                Name = "Delete",
                Image = delIcon,
                DataPropertyName = null,
                Width = 75
            };
            dataGridView.Columns.Add(colEdit);
            dataGridView.Columns.Add(colDel);
            dataGridView.Columns["Edit"].DisplayIndex   = 8;
            dataGridView.Columns["Delete"].DisplayIndex = 9;
            dataGridView.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex == dataGridView.Columns["Edit"]?.Index) { Console.WriteLine("sua"); }
                else if (e.ColumnIndex == dataGridView.Columns["Delete"]?.Index) { Console.WriteLine("xoa"); }
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
}