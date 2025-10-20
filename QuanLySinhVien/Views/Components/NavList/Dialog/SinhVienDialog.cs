using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Controllers;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class SinhVienDialog : Form
{
    public enum DialogMode { View, Create, Edit, Delete }

    public SinhVienDTO ResultSinhVienDto { get; private set; }

    private TextBox txtMaSinhVien;
    private TextBox txtTenSinhVien;
    private DateTimePicker dtpNgaySinh;
    private ComboBox cbbNganh;
    private ComboBox cbbLop;
    private RadioButton rbNam;
    private RadioButton rbNu;
    private TextBox txtSoDienThoai;
    private TextBox txtQueQuan;
    private TextBox txtEmail;
    private TextBox txtCCCD;
    private ComboBox cbbTrangThai;
    
    private LopDAO lopDao;
    private SinhVienController controller;
    private bool isLoadingData = false;
    private int initialMaLop = 0;

    public SinhVienDialog(DialogMode mode, SinhVienDTO sinhVienDto = null)
    {
        Text = string.Empty;
        Size = new Size(500, 750);
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        BackColor = ColorTranslator.FromHtml("#F3F4F6");
        
        lopDao = new LopDAO();
        controller = new SinhVienController();

        var headerPanel = CreateHeaderPanel(mode);
        var buttonPanel = CreateButtonPanel(mode);
        var contentPanel = CreateContentPanel(mode, sinhVienDto);

        Controls.Add(contentPanel);
        Controls.Add(buttonPanel);
        Controls.Add(headerPanel);
        
        this.Load += (s, e) => 
        {
            if (sinhVienDto == null)
            {
                LoadNganhData();
            }
        };
    }

    private Panel CreateHeaderPanel(DialogMode mode)
    {
        var headerPanel = new Panel
        {
            BackColor = ColorTranslator.FromHtml("#07689F"),
            Height = 50,
            Dock = DockStyle.Top
        };
        
        var lblTitle = new Label
        {
            Text = mode == DialogMode.Create ? "Thêm sinh viên" : 
                   mode == DialogMode.Edit ? "Cập nhật sinh viên" : 
                   mode == DialogMode.Delete ? "Xóa sinh viên" : "Chi tiết sinh viên",
            ForeColor = Color.White,
            Font = new Font("Montserrat", 13, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill,
            Padding = new Padding(20, 0, 0, 0)
        };

        var btnClose = new Button
        {
            Text = "✕",
            Width = 50,
            Height = 50,
            Dock = DockStyle.Right,
            FlatStyle = FlatStyle.Flat,
            BackColor = ColorTranslator.FromHtml("#07689F"),
            ForeColor = Color.White,
            Font = new Font("Arial", 16, FontStyle.Regular),
            Cursor = Cursors.Hand
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        btnClose.MouseEnter += (s, e) => btnClose.BackColor = ColorTranslator.FromHtml("#055a7a");
        btnClose.MouseLeave += (s, e) => btnClose.BackColor = ColorTranslator.FromHtml("#07689F");

        headerPanel.Controls.Add(lblTitle);
        headerPanel.Controls.Add(btnClose);
        return headerPanel;
    }

    private Panel CreateContentPanel(DialogMode mode, SinhVienDTO sinhVienDto)
    {
        var cardPanel = new Panel
        {
            BackColor = ColorTranslator.FromHtml("#F3F4F6"),
            Dock = DockStyle.Fill,
            Padding = new Padding(30, 20, 30, 20),
            AutoScroll = true
        };

        CreateControls();

        if (sinhVienDto != null)
        {
            LoadData(sinhVienDto);
        }
        else
        {
            // Trường hợp tạo mới - load ngành ngay từ đầu
            LoadNganhData();
        }

        // Thiết lập trạng thái enable/disable của controls theo mode
        if(mode == DialogMode.View)
        {
            SetControlsEnabled(false); // Chỉ xem, không cho sửa
        }
        else if(mode == DialogMode.Delete)
        {
            SetControlsEnabled(false); // Xóa cũng không cho sửa
        }
        else // Create hoặc Edit
        {
            SetControlsEnabled(true); // Cho phép nhập/sửa
        }

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 11,
            Padding = new Padding(0),
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 270));

        for (int i = 0; i < 11; i++)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        }

        var labels = CreateLabels();
        var controls = CreateControlPanels();

        for (int i = 0; i < labels.Length; i++)
        {
            layout.Controls.Add(labels[i], 0, i);
            layout.Controls.Add(controls[i], 1, i);
        }

        cardPanel.Controls.Add(layout);
        return cardPanel;
    }

    private void CreateControls()
    {
        
        txtMaSinhVien = new TextBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            BorderStyle = BorderStyle.FixedSingle,
            Enabled = false,
            BackColor = ColorTranslator.FromHtml("#F5F5F5"),
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };

        
        txtTenSinhVien = new TextBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };

        
        dtpNgaySinh = new DateTimePicker
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            Format = DateTimePickerFormat.Short,
            Value = DateTime.Now.AddYears(-18),
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };

        
        cbbNganh = new ComboBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };


        cbbLop = new ComboBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0),
            Enabled = false
        };
        
        // KHÔNG load ngành ở đây nữa, sẽ load trong LoadData hoặc sau khi dialog hiển thị
        // LoadNganhData(); // <-- XÓA DÒNG NÀY

        cbbNganh.SelectedIndexChanged += CbbNganh_SelectedIndexChanged;

        
        rbNam = new RadioButton
        {
            Text = "Nam",
            Font = new Font("Montserrat", 10),
            AutoSize = true,
            UseVisualStyleBackColor = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 2, 20, 0)
        };
        rbNu = new RadioButton
        {
            Text = "Nữ",
            Font = new Font("Montserrat", 10),
            AutoSize = true,
            UseVisualStyleBackColor = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 2, 0, 0)
        };

        
        txtSoDienThoai = new TextBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };

        
        txtQueQuan = new TextBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };

        
        txtEmail = new TextBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };

        
        txtCCCD = new TextBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };

        
        cbbTrangThai = new ComboBox()
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };
        cbbTrangThai.Items.AddRange(new[] { "Đang học", "Tạm nghỉ", "Tốt nghiệp", "Bị đuổi học" });
        cbbTrangThai.SelectedIndex = 0;
    }

    private void LoadNganhData()
    {
        try
        {
            isLoadingData = true; // Set flag to prevent event triggering
            
            var nganhList = controller.GetAllNganh();
            if (nganhList == null || nganhList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu ngành trong hệ thống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            cbbNganh.Items.Clear();
            cbbNganh.DisplayMember = "TenNganh";
            cbbNganh.ValueMember = "MaNganh";
            
            foreach (var nganh in nganhList)
            {
                cbbNganh.Items.Add(nganh);
            }
            
            // Enable combobox if data is loaded
            cbbNganh.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải dữ liệu ngành: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            isLoadingData = false; // Reset flag after loading
        }
    }

    private void LoadLopData(int maNganh)
    {
        try
        {
            isLoadingData = true; // Set flag to prevent event triggering
            
            var lopList = controller.GetLopByNganh(maNganh);
            cbbLop.Items.Clear();
            
            if (lopList == null || lopList.Count == 0)
            {
                cbbLop.Enabled = false;
                MessageBox.Show("Không có lớp nào thuộc ngành này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            cbbLop.DisplayMember = "TenLop";
            cbbLop.ValueMember = "MaLop";
            
            foreach (var lop in lopList)
            {
                cbbLop.Items.Add(lop);
            }
            
            if (cbbLop.Items.Count > 0)
            {
                cbbLop.SelectedIndex = 0;
            }
            
            cbbLop.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải dữ liệu lớp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            cbbLop.Enabled = false;
        }
        finally
        {
            isLoadingData = false; // Reset flag after loading
        }
    }

    private void CbbNganh_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Chỉ xử lý khi KHÔNG đang load data
        if (isLoadingData) return;
        
        try
        {
            if (cbbNganh.SelectedItem is NganhDto selectedNganh)
            {
                LoadLopData(selectedNganh.MaNganh);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi chọn ngành: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    private Label[] CreateLabels()
    {
        var labelTexts = new[] { "Mã SV:", "Tên SV:", "Ngày sinh:", "Ngành:", "Lớp:", "Giới tính:", "SĐT:", "Quê quán:", "Email:", "CCCD:", "Trạng thái:" };
        var labels = new Label[labelTexts.Length];

        for (int i = 0; i < labelTexts.Length; i++)
        {
            labels[i] = new Label
            {
                Text = labelTexts[i],
                Font = new Font("Montserrat", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };
        }
        return labels;
    }

    private Control[] CreateControlPanels()
    {
        
        var genderPanel = new FlowLayoutPanel 
        { 
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(5, 15, 5, 15),
            Margin = new Padding(0)
        };
        genderPanel.Controls.Add(rbNam);
        genderPanel.Controls.Add(rbNu);

        return new Control[]
        {
            CreateControlPanel(txtMaSinhVien),
            CreateControlPanel(txtTenSinhVien),
            CreateControlPanel(dtpNgaySinh),
            CreateControlPanel(cbbNganh),
            CreateControlPanel(cbbLop),
            genderPanel,
            CreateControlPanel(txtSoDienThoai),
            CreateControlPanel(txtQueQuan),
            CreateControlPanel(txtEmail),
            CreateControlPanel(txtCCCD),
            CreateControlPanel(cbbTrangThai)
        };
    }

    private Panel CreateControlPanel(Control control)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(5, 15, 5, 15)
        };
        control.Dock = DockStyle.Fill;
        panel.Controls.Add(control);
        return panel;
    }

    private void LoadData(SinhVienDTO sinhVienDto)
    {
        // Bật flag để tránh trigger event
        isLoadingData = true;
        
        try
        {
            txtMaSinhVien.Text = sinhVienDto.MaSinhVien.ToString();
            txtTenSinhVien.Text = sinhVienDto.TenSinhVien;
            
            if (DateTime.TryParse(sinhVienDto.NgaySinh, out DateTime ngaySinh))
            {
                dtpNgaySinh.Value = ngaySinh;
            }

            // Set giới tính - đảm bảo chỉ 1 được chọn
            if (sinhVienDto.GioiTinh == "Nam")
            {
                rbNam.Checked = true;
                rbNu.Checked = false;
            }
            else if (sinhVienDto.GioiTinh == "Nữ")
            {
                rbNu.Checked = true;
                rbNam.Checked = false;
            }

             // Load ngành và lớp dựa trên MaLop từ DB
             if (sinhVienDto.MaLop > 0)
             {
                 // Lấy thông tin lớp hiện tại từ DB
                 var lopHienTai = lopDao.GetById(sinhVienDto.MaLop);
                 
                 if (lopHienTai != null && lopHienTai.MaNganh > 0)
                 {
                     // Load danh sách ngành và đảm bảo ngành hiện tại có trong list
                    var nganhList = controller.GetAllNganh() ?? new List<NganhDto>();
                    var nganhHienTai = controller.GetNganhById(lopHienTai.MaNganh);
                    
                    if (nganhHienTai != null && nganhList.All(x => x.MaNganh != nganhHienTai.MaNganh))
                    {
                        nganhList.Insert(0, nganhHienTai);
                    }
                    
                    if (nganhList.Count > 0)
                    {
                        // TẮT event handler trước khi bind
                        cbbNganh.SelectedIndexChanged -= CbbNganh_SelectedIndexChanged;
                        
                        // Dùng Items.AddRange thay vì DataSource
                        cbbNganh.Items.Clear();
                        cbbNganh.DisplayMember = "TenNganh";
                        cbbNganh.ValueMember = "MaNganh";
                        
                        foreach (var nganh in nganhList)
                        {
                            cbbNganh.Items.Add(nganh);
                        }
                        
                        cbbNganh.Enabled = true;
                        
                        // Đợi bind xong
                        Application.DoEvents();
                        
                        // Chọn ngành bằng SelectedIndex
                        int nganhIndex = nganhList.FindIndex(x => x.MaNganh == lopHienTai.MaNganh);
                        
                        if (nganhIndex >= 0 && nganhIndex < cbbNganh.Items.Count)
                        {
                            cbbNganh.SelectedIndex = nganhIndex;
                        }
                        
                        // BẬT lại event handler
                        cbbNganh.SelectedIndexChanged += CbbNganh_SelectedIndexChanged;
                    }

                     // Load danh sách lớp theo ngành và đảm bảo lớp hiện tại có trong list
                     var lopList = controller.GetLopByNganh(lopHienTai.MaNganh) ?? new List<LopDto>();
                     
                     // Kiểm tra xem lớp hiện tại có trong danh sách không
                     if (lopList.All(x => x.MaLop != lopHienTai.MaLop))
                     {
                         // Nếu không có (bị lọc Status), thêm vào đầu danh sách
                         lopList.Insert(0, lopHienTai);
                     }

                    // Bind danh sách lớp bằng Items.Add
                    cbbLop.Items.Clear();
                    cbbLop.DisplayMember = "TenLop";
                    cbbLop.ValueMember = "MaLop";
                    
                    foreach (var lop in lopList)
                    {
                        cbbLop.Items.Add(lop);
                    }
                    
                    cbbLop.Enabled = true;

                    // Đợi bind xong
                    Application.DoEvents();

                    // Chọn lớp bằng SelectedIndex
                    int lopIndex = lopList.FindIndex(x => x.MaLop == sinhVienDto.MaLop);
                    
                    if (lopIndex >= 0 && lopIndex < cbbLop.Items.Count)
                    {
                        cbbLop.SelectedIndex = lopIndex;
                    }
                 }
                 else
                 {
                     // Nếu không tìm thấy lớp, vẫn load danh sách ngành
                     LoadNganhData();
                     MessageBox.Show("Không tìm thấy thông tin lớp của sinh viên này trong hệ thống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 }
             }
             else
             {
                 // Trường hợp tạo mới - load ngành
                 LoadNganhData();
             }

            txtSoDienThoai.Text = sinhVienDto.SdtSinhVien ?? "";
            txtQueQuan.Text = sinhVienDto.QueQuanSinhVien ?? "";
            txtEmail.Text = sinhVienDto.Email ?? "";
            txtCCCD.Text = sinhVienDto.CCCD ?? "";

            // Set trạng thái
            for (int i = 0; i < cbbTrangThai.Items.Count; i++)
            {
                if (cbbTrangThai.Items[i].ToString() == sinhVienDto.TrangThai)
                {
                    cbbTrangThai.SelectedIndex = i;
                    break;
                }
            }
        }
        finally
        {
            // Tắt flag sau khi load xong
            isLoadingData = false;
        }
    }

    private void SetControlsEnabled(bool enabled)
    {
        txtTenSinhVien.Enabled = enabled;
        dtpNgaySinh.Enabled = enabled;
        cbbNganh.Enabled = enabled;
        cbbLop.Enabled = enabled;
        rbNam.Enabled = enabled;
        rbNu.Enabled = enabled;
        txtSoDienThoai.Enabled = enabled;
        txtQueQuan.Enabled = enabled;
        txtEmail.Enabled = enabled;
        txtCCCD.Enabled = enabled;
        cbbTrangThai.Enabled = enabled;
    }

    private Panel CreateButtonPanel(DialogMode mode)
    {
        var buttonPanel = new Panel
        {
            Height = 60,
            Dock = DockStyle.Bottom,
            BackColor = ColorTranslator.FromHtml("#F3F4F6"),
            Padding = new Padding(20, 15, 20, 15)
        };

        var btnCancel = new Button
        {
            Text = mode == DialogMode.View ? "Đóng" : "Hủy",
            DialogResult = DialogResult.Cancel,
            Width = 90,
            Height = 32,
            Font = new Font("Montserrat", 10, FontStyle.Regular),
            BackColor = Color.White,
            ForeColor = Color.Black,
            FlatStyle = FlatStyle.Flat,
            Dock = DockStyle.Right,
            Cursor = Cursors.Hand
        };
        btnCancel.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#B0BEC5");
        btnCancel.FlatAppearance.BorderSize = 1;

        var btnOk = new Button
        {
            Text = mode == DialogMode.Create ? "Thêm" : 
                   mode == DialogMode.Delete ? "Xóa" : "Cập nhật",
            DialogResult = DialogResult.OK,
            Width = 100,
            Height = 32,
            Font = new Font("Montserrat", 10, FontStyle.Bold),
            BackColor = mode == DialogMode.Delete ? ColorTranslator.FromHtml("#FF6B6B") : ColorTranslator.FromHtml("#07689F"),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Dock = DockStyle.Right,
            Margin = new Padding(0, 0, 10, 0),
            Cursor = Cursors.Hand,
            Visible = mode != DialogMode.View
        };
        btnOk.FlatAppearance.BorderSize = 0;

        btnOk.Click += (s, e) =>
        {
            if (mode == DialogMode.Delete || ValidateInput())
            {
                ResultSinhVienDto = CreateSinhVienDto();
            }
            else
            {
                DialogResult = DialogResult.None;
            }
        };

        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Controls.Add(btnOk);

        return buttonPanel;
    }

    private bool ValidateInput()
    {
        if (!rbNam.Checked && !rbNu.Checked)
        {
            MessageBox.Show("Vui lòng chọn giới tính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
        {
            MessageBox.Show("Số điện thoại không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtSoDienThoai.Focus();
            return false;
        }
        
        var sdt = txtSoDienThoai.Text.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^(0[3|5|7|8|9])+([0-9]{8})$"))
        {
            MessageBox.Show("Số điện thoại không hợp lệ. Phải là 10 số và bắt đầu bằng 03, 05, 07, 08, 09.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtSoDienThoai.Focus();
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            MessageBox.Show("Email không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtEmail.Focus();
            return false;
        }
        
        var email = txtEmail.Text.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtEmail.Focus();
            return false;
        }
        
        if (cbbTrangThai.SelectedIndex < 0)
        {
            MessageBox.Show("Vui lòng chọn trạng thái.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cbbTrangThai.Focus();
            return false;
        }
        
        if (cbbNganh.SelectedIndex < 0 || cbbNganh.SelectedItem == null)
        {
            MessageBox.Show("Vui lòng chọn ngành.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cbbNganh.Focus();
            return false;
        }
        
        
        if (cbbLop.SelectedIndex < 0 || cbbLop.SelectedItem == null)
        {
            MessageBox.Show("Vui lòng chọn lớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cbbLop.Focus();
            return false;
        }
        
        return true;
    }

    private SinhVienDTO CreateSinhVienDto()
    {
        string nganhName = "";
        int maLop = 0;
        
        // Lấy ngành từ SelectedItem (vì không dùng DataSource)
        if (cbbNganh.SelectedItem is NganhDto selectedNganh)
        {
            nganhName = selectedNganh.TenNganh;
        }

        // Lấy lớp từ SelectedItem (vì không dùng DataSource)
        if (cbbLop.SelectedItem is LopDto selectedLop)
        {
            maLop = selectedLop.MaLop;
        }
        else
        {
            throw new Exception("Không thể lấy mã lớp. Vui lòng chọn lại lớp.");
        }

        var dto = new SinhVienDTO
        {
            MaSinhVien = string.IsNullOrEmpty(txtMaSinhVien.Text) ? 0 : int.Parse(txtMaSinhVien.Text),
            TenSinhVien = txtTenSinhVien.Text.Trim(),
            NgaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
            GioiTinh = rbNam.Checked ? "Nam" : "Nữ",
            Nganh = nganhName,
            MaLop = maLop,
            SdtSinhVien = txtSoDienThoai.Text.Trim(),
            QueQuanSinhVien = txtQueQuan.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            CCCD = txtCCCD.Text.Trim(),
            TrangThai = cbbTrangThai.SelectedItem?.ToString() ?? "Đang học"
        };

        // Lấy MaKhoaHoc từ sinh viên hiện tại (khi edit) hoặc mặc định là 1 (khi tạo mới)
        if (!string.IsNullOrEmpty(txtMaSinhVien.Text))
        {
            var currentSinhVien = controller.GetSinhVienById(int.Parse(txtMaSinhVien.Text));
            dto.MaKhoaHoc = currentSinhVien.MaKhoaHoc;
        }
        else
        {
            dto.MaKhoaHoc = 1; // Mặc định cho sinh viên mới
        }

        return dto;
    }
}
