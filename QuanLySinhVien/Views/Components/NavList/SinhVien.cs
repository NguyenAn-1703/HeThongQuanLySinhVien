using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Enums;
using Svg;
namespace QuanLySinhVien.Views.Components;

public class SinhVien : NavBase
{
    private String[] columns;
    private SinhVienController _controller = new SinhVienController();
    private string[] _listSelectionForComboBox = new []{"Mã sinh viên", "Tên sinh viên"};
    
    private DataGridView dataGridView;
    private DataTable table;
    private CUse _cUse;
    public SinhVien()
    {
        _cUse = new CUse();
        Init();
        SetupDataGridView();
        LoadSinhVienData();
    }
    
    private void Init()
    {   
        //BackColor = Color.Blue;
        Dock = DockStyle.Fill;
        Size = new Size(1200, 900);
        Controls.Add(Bottom());
        Controls.Add(Top());
    }
    
    private Label JLable(string txt)
    {
        Label lbl = new Label()
        {
            Text = txt,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Bold),
            Dock = DockStyle.Right,
            TextAlign = ContentAlignment.TopRight,
            Anchor = AnchorStyles.None
            // Width = 300,
            // AutoSize = true,
            // Padding = new  Padding(50 , 0 , 0 , 0),

        };
        lbl.Width += 50;
        return lbl;
    }

    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Top,
            BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            //BackColor = Color.Red,
            Height = 100,
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
        
        
        // add sinh vien 
        var fullDialog = new TableLayoutPanel()
        {
            ColumnCount = 1,
            RowCount = 3,
            Dock = DockStyle.Fill,
            // BackColor = Color.Red,
        };
        var textLabel = new Label
        {
            Text = "Thêm sinh viên",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = GetFont.GetFont.GetMainFont(13, FontType.Bold),
            Width = 300,
            Height = 60,
            Dock = DockStyle.Fill,
        };
        var topDialog = new Panel()
        {
            BackColor = Color.Aqua,
            Dock = DockStyle.Fill,
            // Height = 90
        };
        topDialog.Controls.Add(textLabel);
        
        var middleDialogFull = new TableLayoutPanel
        {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill,
        };

        var imgMiddleDialog = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            // BackColor = Color.Yellow,
            RowCount = 2,
        };
        var middleDialog = new Panel()
        {
            Dock = DockStyle.Fill,
        };
        var pathImgUpload = Path.Combine(AppContext.BaseDirectory, "img" , "upload.svg");
        var imgUpload = SvgDocument.Open(pathImgUpload).Draw(250, 250);
        var boxImg = new PictureBox
        {   
            Image = imgUpload,
            //BackColor = Color.Blue,
            Anchor = AnchorStyles.None,
            Width = 250,
            Height = 250,
        };
        
        var imgAddButton = new Button
        {
            Text = "Thêm ảnh",
            Anchor = AnchorStyles.None,
            Font = GetFont.GetFont.GetMainFont(12, FontType.Bold),
            ImageAlign = ContentAlignment.MiddleCenter,
            Width = 200,
            Height = 70,
        };  
        imgMiddleDialog.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        imgMiddleDialog.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
        imgMiddleDialog.Controls.Add(imgAddButton , 0 , 0);
        imgMiddleDialog.Controls.Add(boxImg , 0 , 1);
        
        // var deleteImgButton = new Button
        
        middleDialogFull.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        middleDialogFull.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        var borderMiddleLeft = new TableLayoutPanel()
        {
            ColumnCount = 1,
            RowCount = 5,   
            // BackColor = Color.Black,
            Dock = DockStyle.Fill,
            // Padding = new Padding(0, 10, 0, 0),
        };
        
        var borderMiddleRight = new TableLayoutPanel()
        {
            ColumnCount = 1,
            RowCount = 5,
            Dock = DockStyle.Fill,
            // BackColor = Color.White,
        };
        
        var borderMiddle = new TableLayoutPanel()
        {
            ColumnCount = 2,
            RowCount = 1,
            // BackColor = Color.Aqua,
            Dock = DockStyle.Fill,
            // Padding = new Padding(10 , 0 , 0 , 10),
        };
        borderMiddle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        borderMiddle.Controls.Add(borderMiddleLeft , 0 , 0);
        borderMiddle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
        borderMiddle.Controls.Add(borderMiddleRight , 1 , 0);
        // borderMiddle.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        middleDialog.Controls.Add(borderMiddle);
        
        middleDialogFull.Controls.Add(imgMiddleDialog , 0 , 0);
        middleDialogFull.Controls.Add(middleDialog , 0 , 1);
        
        
        var bottomDialog = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
        };
        bottomDialog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        bottomDialog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        bottomDialog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        var cancelButton = new Button
        {
            Text = "Hủy",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Red,
            Cursor = Cursors.Hand,
            // Dock = DockStyle.Left,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            Width = 140,
            Height = 50,
        };
        var addButton = new Button
        {
            Text = "Thêm",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Green,
            Cursor = Cursors.Hand,
            // Dock = DockStyle.Right,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            Width = 140,
            Height = 50,
        };
        var resetButton = new Button
        {
            Text = "Làm mới",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Cyan,
            Cursor = Cursors.Hand,
            // Dock = DockStyle.Fill,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            Width = 140,
            Height = 50,
        };
        cancelButton.Margin = new Padding(20, 0, 20, 0);
        resetButton.Margin = new Padding(20, 0, 20, 0);
        addButton.Margin   = new Padding(20, 0, 20, 0);
        bottomDialog.Controls.Add(cancelButton);   
        bottomDialog.Controls.Add(resetButton);   
        bottomDialog.Controls.Add(addButton);   
        //borderMiddleLeft.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        List<string> list = new List<string>{"Mã Sinh Viên: " ,"Tên Sinh Viên: " , "Ngày sinh: " , "Khoa: " , "Giới tính: "};
        float tile = 60f / list.Count;
        string[] cbb = new []{ "Cộng nghệ thông tin" };
        List<Control> rightComponents = new List<Control>();
        var radioNam = new RadioButton
        {
            Text = "Nam",
            Anchor = AnchorStyles.None,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true
        };
        var radioNu = new RadioButton
        {
            Text = "Nữ",
            Anchor = AnchorStyles.None,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true
        };
        var tableRadioButton = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
        };
        tableRadioButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableRadioButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableRadioButton.Controls.Add(radioNam, 0, 0);
        tableRadioButton.Controls.Add(radioNu, 1, 0);
        
        var panelRadioButton = new Panel()
        {
            Dock = DockStyle.Fill,
            Width = 300,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            // BackColor = Color.Red,
            Anchor = AnchorStyles.None,
        };
        panelRadioButton.Controls.Add(tableRadioButton);
        var combo = new ComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 300,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
            // Margin = new Padding(0 , 0 , 0 , 0),
        };
        combo.Items.AddRange(cbb);
        rightComponents.Add(new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
            // Margin = new Padding(5),
        });
        rightComponents.Add(new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
            //Margin = new Padding(5),
        });
        rightComponents.Add(new DateTimePicker()
        {
            Value = DateTime.Now,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            //Margin = new Padding(5),
        });
        rightComponents.Add(combo);
        rightComponents.Add(panelRadioButton);
        
        
        // Console.WriteLine(tile);
        for (int i = 1; i <= list.Count; i++)
        {
            borderMiddleLeft.RowStyles.Add(new RowStyle(SizeType.Percent,  tile));
            borderMiddleRight.RowStyles.Add(new RowStyle(SizeType.Percent, tile));
        }
        
        for (int i = 0; i < list.Count; i++)
        {
            Label lb = JLable(list[i]);
            Control rb = rightComponents[i];
            borderMiddleLeft.Controls.Add(lb , 0 , i);
            borderMiddleRight.Controls.Add(rb , 1 , i);
        }
        
        fullDialog.Controls.Add(topDialog , 0 , 0);
        fullDialog.Controls.Add(middleDialogFull , 0 , 1);
        fullDialog.Controls.Add(bottomDialog , 0 , 2);
        
        fullDialog.RowStyles.Clear();
        fullDialog.RowStyles.Add(new RowStyle(SizeType.Percent , 5F));
        fullDialog.RowStyles.Add(new RowStyle(SizeType.Percent , 80F));
        fullDialog.RowStyles.Add(new RowStyle(SizeType.Percent , 15F));
        
        btnAdd.Click += (s , e) =>
        {
            Form form = new Form()
            {
                Text = "Thêm sinh viên",
                Size = new Size(600, 1000),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.None,
                Controls = { fullDialog }
            };
            form.ShowDialog();
        };
        // Text Nav_List
        var textNavList = new Label
        {
            Text = "Sinh Viên",
            Font = GetFont.GetFont.GetMainFont(15, FontType.Bold),
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
        table.Columns.Add("Mã SV", typeof(int));
        table.Columns.Add("Tên Sinh Viên", typeof(string));
        table.Columns.Add("Ngày sinh", typeof(string));
        table.Columns.Add("Giới tính", typeof(string));
        table.Columns.Add("Ngành", typeof(string));
        table.Columns.Add("Trạng thái", typeof(string));
        // set Width
        dataGridView.AutoGenerateColumns = true;
        dataGridView.DataSource = table;
        dataGridView.RowTemplate.Height = 37;
        // delete border
        dataGridView.BorderStyle = BorderStyle.None;
        dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
        dataGridView.RowHeadersVisible = false;
        dataGridView.Dock = DockStyle.Fill;
        dataGridView.Font = GetFont.GetFont.GetMainFont(10, FontType.Regular);
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
            Font = GetFont.GetFont.GetMainFont(11, FontType.Bold),
            //ForeColor = Color.White,
            // Font = new Font("JetBrains Mono", 10f, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleCenter,
        };
    }
    
    private void LoadSinhVienData()
    {
        try
        {
            var list = _controller.LayDanhSachSinhVienTable();
            table.Rows.Clear();
    
            foreach (var sv in list)
            {
                table.Rows.Add(
                    sv.MaSinhVien,
                    sv.TenSinhVien,
                    sv.NgaySinh,
                    sv.GioiTinh,
                    sv.Nganh,
                    sv.TrangThai
                );
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }


    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
}