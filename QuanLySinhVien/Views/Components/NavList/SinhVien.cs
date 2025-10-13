using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.NavList.Dialog;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Components;

public class SinhVien : NavBase
{
    private String[] columns;
    private SinhVienController _controller;
    private string[] _listSelectionForComboBox = new []{"Mã sinh viên", "Tên sinh viên"};
    private SinhVienDAO sinhVienDao;
    private DataGridView dataGridView;
    private DataTable table;
    private CUse _cUse;

    private TextBox txtTenSinhVien;
    private TextBox txtNgaySinh;
    private ComboBox cbbNganh;
    private RadioButton rbNam;
    private RadioButton rbNu;
    private TextBox txtSoDienThoai;
    private TextBox txtQueQuan;
    private TextBox txtEmail;
    private TextBox txtCCCD;
    private ComboBox cbbTrangThai;
    private TextBox txtKhoaHoc;
    private TextBox txtMaLop;
    private DateTimePicker dtpNgaySinh;
    
    public SinhVien()
    {
        _cUse = new CUse();
        sinhVienDao = new SinhVienDAO();
        _controller = new SinhVienController();
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
            ForeColor = Color.White,
        };
        var topDialog = new Panel()
        {
            // BackColor = Color.Aqua,
            BackColor = ColorTranslator.FromHtml("#07689F"),
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
        
        
        var bottomDialog = new Panel()
        {
            Dock = DockStyle.Fill,
        };
        
        var buttonContainer = new FlowLayoutPanel()
        {
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoSize = true,
            // Anchor = AnchorStyles.None,
        };
        
        var cancelButton = new Button
        {
            Text = "Hủy",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Red,
            Cursor = Cursors.Hand,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Width = 140,
            Height = 50,
            Margin = new Padding(120, 20, 30, 0),
        };
        var addButton = new Button
        {
            Text = "Thêm",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Green,
            Cursor = Cursors.Hand,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Width = 140,
            Height = 50,
            Margin = new Padding(30, 20, 0, 0),
        };
        
        buttonContainer.Controls.Add(cancelButton);   
        buttonContainer.Controls.Add(addButton);
        bottomDialog.Controls.Add(buttonContainer);   
        //borderMiddleLeft.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        List<string> list = new List<string>{"Tên Sinh Viên: " , "Ngày sinh: " , "Ngành: " , "Giới tính: " , "Số điện thoại" , "Quê Quán" , "Email" , "CCCD" , "Trạng thái" , "Khóa Học"};
        float tile = 60f / list.Count;
        string[] cbb = new []{"--Chọn--", "Công nghệ thông tin", "Mạng máy tính", "Hệ thống thông tin" };
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
        combo.SelectedIndex = 0;
        
        combo.SelectedIndexChanged += (s, e) =>
        {
            if (combo.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn một ngành.");
            }
        };
        
        txtTenSinhVien = new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        
        dtpNgaySinh = new DateTimePicker()
        {
            Value = DateTime.Now.AddYears(-18),
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        
        txtSoDienThoai = new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        
        txtQueQuan = new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        
        txtEmail = new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        
        txtCCCD = new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        
        cbbTrangThai = new ComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 300,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        cbbTrangThai.Items.AddRange(new[] {"--Chọn--" , "Đang học", "Tạm nghỉ", "Tốt nghiệp", "Bị đuổi học" });
        cbbTrangThai.SelectedIndex = 0;
        
        
        txtKhoaHoc = new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        
        rightComponents.Add(txtTenSinhVien);
        rightComponents.Add(dtpNgaySinh);
        rightComponents.Add(combo);
        rightComponents.Add(panelRadioButton);
        rightComponents.Add(txtSoDienThoai);
        rightComponents.Add(txtQueQuan);
        rightComponents.Add(txtEmail);
        rightComponents.Add(txtCCCD);
        rightComponents.Add(cbbTrangThai);
        rightComponents.Add(txtKhoaHoc);
        
        
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
            
            cancelButton.Click += (sender, args) =>
            {
                form.Close();
            };
            
            addButton.Click += (sender, args) =>
            {
                try
                {
                    // Validate tên sinh viên
                    if (string.IsNullOrWhiteSpace(txtTenSinhVien.Text))
                    {
                        MessageBox.Show("Tên sinh viên không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTenSinhVien.Focus();
                        return;
                    }
                    
                    if (txtTenSinhVien.Text.Trim().Length < 3)
                    {
                        MessageBox.Show("Tên sinh viên phải có ít nhất 3 ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTenSinhVien.Focus();
                        return;
                    }
                    
                    // Validate ngày sinh (>= 18 tuổi)
                    var age = DateTime.Now.Year - dtpNgaySinh.Value.Year;
                    if (dtpNgaySinh.Value > DateTime.Now.AddYears(-age)) age--;
                    
                    if (age < 16)
                    {
                        MessageBox.Show("Sinh viên phải từ 16 tuổi trở lên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    if (age > 100)
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    // Validate ngành
                    if (combo.SelectedIndex == 0)
                    {
                        MessageBox.Show("Vui lòng chọn ngành.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        combo.Focus();
                        return;
                    }
                    
                    // Validate giới tính
                    if (!radioNam.Checked && !radioNu.Checked)
                    {
                        MessageBox.Show("Vui lòng chọn giới tính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    // Validate số điện thoại
                    if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
                    {
                        MessageBox.Show("Số điện thoại không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoDienThoai.Focus();
                        return;
                    }
                    
                    var sdt = txtSoDienThoai.Text.Trim();
                    if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^(0[3|5|7|8|9])+([0-9]{8})$"))
                    {
                        MessageBox.Show("Số điện thoại không hợp lệ. Phải là 10 số và bắt đầu bằng 03, 05, 07, 08, 09.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoDienThoai.Focus();
                        return;
                    }
                    
                    // Validate quê quán
                    if (string.IsNullOrWhiteSpace(txtQueQuan.Text))
                    {
                        MessageBox.Show("Quê quán không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtQueQuan.Focus();
                        return;
                    }
                    
                    // Validate email
                    if (string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        MessageBox.Show("Email không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return;
                    }
                    
                    var email = txtEmail.Text.Trim();
                    if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return;
                    }
                    
                    // Validate CCCD
                    if (string.IsNullOrWhiteSpace(txtCCCD.Text))
                    {
                        MessageBox.Show("CCCD không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCCCD.Focus();
                        return;
                    }
                    
                    var cccd = txtCCCD.Text.Trim();
                    if (!System.Text.RegularExpressions.Regex.IsMatch(cccd, @"^\d{12}$"))
                    {
                        MessageBox.Show("CCCD phải là 12 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCCCD.Focus();
                        return;
                    }
                    
                    // Validate trạng thái
                    if (cbbTrangThai.SelectedIndex == 0)
                    {
                        MessageBox.Show("Vui lòng chọn trạng thái.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbTrangThai.Focus();
                        return;
                    }
                    
                    // Validate khóa học
                    if (string.IsNullOrWhiteSpace(txtKhoaHoc.Text))
                    {
                        MessageBox.Show("Khóa học không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtKhoaHoc.Focus();
                        return;
                    }
                    
                    var sinhVien = new SinhVienDTO
                    {
                        TenSinhVien = txtTenSinhVien.Text.Trim(),
                        NgaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
                        GioiTinh = radioNam.Checked ? "Nam" : "Nữ",
                        SdtSinhVien = txtSoDienThoai.Text.Trim(),
                        QueQuanSinhVien = txtQueQuan.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        CCCD = txtCCCD.Text.Trim(),
                        TrangThai = cbbTrangThai.SelectedItem?.ToString() ?? "Đang học",
                        // MaLop = combo.SelectedValue?.ToString(),
                        // MaKhoaHoc = txtKhoaHoc.Text,
                    };
                    
                    _controller.AddSinhVien(sinhVien);
                    
                    MessageBox.Show("Thêm sinh viên thành công!");
                    txtTenSinhVien.Clear();
                    txtSoDienThoai.Clear();
                    txtQueQuan.Clear();
                    txtEmail.Clear();
                    txtCCCD.Clear();
                    txtKhoaHoc.Clear();
                    dtpNgaySinh.Value = DateTime.Now.AddYears(-18);
                    cbbTrangThai.SelectedIndex = 0;

                    form.Close();
                    LoadSinhVienData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm sinh viên: {ex.Message}");
                }
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
                if (e.ColumnIndex == dataGridView.Columns["Edit"]?.Index) { 
                    var row = dataGridView.Rows[e.RowIndex];
                    var sinhVienDto = new SinhVienDTO
                    {
                        MaSinhVien = Convert.ToInt32(row.Cells["Mã SV"].Value),
                    };
                    sinhVienDto = _controller.GetSinhVienById(sinhVienDto.MaSinhVien);
                    
                    using (var dialog = new SinhVienDialog(SinhVienDialog.DialogMode.Edit, sinhVienDto))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultSinhVienDto != null)
                        {
                            try
                            {
                                _controller.EditSinhVien(dialog.ResultSinhVienDto);
                                LoadSinhVienData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi cập nhật sinh viên: {ex.Message}");
                            }
                        }
                    }
                 }
                else if (e.ColumnIndex == dataGridView.Columns["Del"]?.Index) { 
                    var row = dataGridView.Rows[e.RowIndex];
                    var sinhVienDto = new SinhVienDTO
                    {
                        MaSinhVien = Convert.ToInt32(row.Cells["Mã SV"].Value),
                    };
                    sinhVienDto = _controller.GetSinhVienById(sinhVienDto.MaSinhVien);
                    
                    using (var dialog = new SinhVienDialog(SinhVienDialog.DialogMode.Delete, sinhVienDto))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                _controller.DeleteSinhVien(sinhVienDto.MaSinhVien);
                                LoadSinhVienData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi xóa sinh viên: {ex.Message}");
                            }
                        }
                    }
                }
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

    public override void onSearch(string txtSearch, string filter)
    {
        try
        {
            List<SinhVienDTO> list;
        
            filter = filter.Trim();
            
            if (string.IsNullOrWhiteSpace(txtSearch))
            {
                list = _controller.LayDanhSachSinhVienTable();
            }
            else
            {
                // Tìm kiếm theo text và filter
                list = _controller.Search(txtSearch, filter);
            }

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
            MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
        }
    }
}