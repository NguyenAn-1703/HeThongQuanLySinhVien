using System.Data;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using Svg;
namespace QuanLySinhVien.Views.Components;

public class MoDangKyHocPhan : NavBase
{
    private string[] _listSelectionForComboBox = new []{""};
    
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
            RowCount = 2,
            Padding = new Padding(20, 10, 20, 10),
        };
        header.ColumnStyles.Clear();
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.RowStyles.Clear();
        header.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
        header.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        // Text Nav_List
        var textNavList = new Label
        {
            Text = "Mở đăng ký học phần",
            // Font = new Font("JetBrains Mono", 16f , FontStyle.Bold),
            AutoSize = true,
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
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            WrapContents = false,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Margin = new Padding(0, 30, 15, 0),
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
            Margin = new Padding(0 , 30 , 0 , 0),
            Size = new  Size(120, 50),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
            // Font = new Font("JetBrains Mono", 10f , FontStyle.Regular),
            Anchor = AnchorStyles.None
        };
        btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
        btnAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
        
        // Text các lớp mở đăng ký
        var textClmdk = new Label()
        {
            Text = "Các lớp mở đăng ký",
            // Font = new Font("JetBrains Mono", 10f , FontStyle.Regular),
            Size = new Size(200, 40),
            Margin = new Padding(0, 8, 0, 0),
        };
        
        // add components using layout
        header.Controls.Add(textNavList, 0, 0);
        header.Controls.Add(panelFull, 1, 0);
        header.Controls.Add(btnAdd, 2, 0);
        header.Controls.Add(textClmdk, 0, 1);
        header.SetColumnSpan(textClmdk, 3);
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
        dataGridView.Dock = DockStyle.Fill;
        // change Width
        bool _actionsAdded = false;
        dataGridView.DataBindingComplete += (s, e) =>
        {
            if (_actionsAdded) return;
            _actionsAdded = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            int[] weights = {150, 250, 120, 80, 120, 120, 148 , 120};
            for (int i = 0; i < dataGridView.Columns.Count && i < weights.Length; i++)
            {
                var col = dataGridView.Columns[i];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.FillWeight = weights[i];
                col.MinimumWidth = 80;
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
            dataGridView.Columns["Edit"].DisplayIndex   = 8;
            dataGridView.Columns["Del"].DisplayIndex = 9;
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
}