using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class SinhVienDialog : Form
{
    public enum DialogMode { View, Create, Edit, Delete }

    public SinhVienDTO ResultSinhVienDto { get; private set; }

    private TextBox txtMaSinhVien;
    private TextBox txtTenSinhVien;
    private DateTimePicker dtpNgaySinh;
    private ComboBox cbbNganh;
    private ComboBox cbbKhoaHoc;
    private ComboBox cbbLop;
    private RadioButton rbNam;
    private RadioButton rbNu;
    private TextBox txtSoDienThoai;
    private TextBox txtQueQuan;
    private TextBox txtEmail;
    private TextBox txtCCCD;
    private ComboBox cbbTrangThai;
    
    // DAO instances
    private NganhDao nganhDao;
    private KhoaHocDAO khoaHocDao;
    private LopDAO lopDao;

    public SinhVienDialog(DialogMode mode, SinhVienDTO sinhVienDto = null)
    {
        Text = string.Empty;
        Size = new Size(500, 750);
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        BackColor = ColorTranslator.FromHtml("#F3F4F6");

        // Initialize DAOs
        nganhDao = new NganhDao();
        khoaHocDao = new KhoaHocDAO();
        lopDao = new LopDAO();

        var headerPanel = CreateHeaderPanel(mode);
        var buttonPanel = CreateButtonPanel(mode);
        var contentPanel = CreateContentPanel(mode, sinhVienDto);

        Controls.Add(contentPanel);
        Controls.Add(buttonPanel);
        Controls.Add(headerPanel);
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

        bool ok = true;
        if(mode == DialogMode.Edit) SetControlsEnabled(ok);
        else if(mode == DialogMode.Delete) SetControlsEnabled2(!ok);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 12,
            Padding = new Padding(0),
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 270));

        for (int i = 0; i < 12; i++)
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

        cbbKhoaHoc = new ComboBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0),
            Enabled = false
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

        // Load initial data
        LoadNganhData();
        LoadKhoaHocData();

        // Add event handlers for cascading
        cbbNganh.SelectedIndexChanged += CbbNganh_SelectedIndexChanged;
        cbbKhoaHoc.SelectedIndexChanged += CbbKhoaHoc_SelectedIndexChanged;

        
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
        rbNam.Checked = true;

        
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
            var nganhList = nganhDao.GetAll();
            cbbNganh.Items.Clear();
            cbbNganh.DisplayMember = "TenNganh";
            cbbNganh.ValueMember = "MaNganh";
            cbbNganh.DataSource = nganhList;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải dữ liệu ngành: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadKhoaHocData()
    {
        try
        {
            var khoaHocList = khoaHocDao.GetAllWithDisplayText();
            cbbKhoaHoc.Items.Clear();
            cbbKhoaHoc.DisplayMember = "DisplayText";
            cbbKhoaHoc.ValueMember = "MaKhoaHoc";
            cbbKhoaHoc.DataSource = khoaHocList;
            cbbKhoaHoc.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải dữ liệu khóa học: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadLopData(int maNganh, string tenKhoaHoc)
    {
        try
        {
            var lopList = lopDao.GetByNganhAndKhoaHoc(maNganh, tenKhoaHoc);
            cbbLop.Items.Clear();
            cbbLop.DisplayMember = "DisplayText";
            cbbLop.ValueMember = "MaLop";
            cbbLop.DataSource = lopList;
            cbbLop.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải dữ liệu lớp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CbbNganh_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbbNganh.SelectedValue != null && cbbKhoaHoc.SelectedValue != null)
        {
            int maNganh = (int)cbbNganh.SelectedValue;
            string tenKhoaHoc = cbbKhoaHoc.SelectedItem?.GetType().GetProperty("DisplayText")?.GetValue(cbbKhoaHoc.SelectedItem)?.ToString() ?? "";
            
            // Extract khoa hoc name (e.g., "K21" from "K21 (2021-2025)")
            if (tenKhoaHoc.Contains("("))
            {
                tenKhoaHoc = tenKhoaHoc.Substring(0, tenKhoaHoc.IndexOf("(")).Trim();
            }
            
            LoadLopData(maNganh, tenKhoaHoc);
        }
    }

    private void CbbKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbbNganh.SelectedValue != null && cbbKhoaHoc.SelectedValue != null)
        {
            int maNganh = (int)cbbNganh.SelectedValue;
            string tenKhoaHoc = cbbKhoaHoc.SelectedItem?.GetType().GetProperty("DisplayText")?.GetValue(cbbKhoaHoc.SelectedItem)?.ToString() ?? "";
            
            // Extract khoa hoc name (e.g., "K21" from "K21 (2021-2025)")
            if (tenKhoaHoc.Contains("("))
            {
                tenKhoaHoc = tenKhoaHoc.Substring(0, tenKhoaHoc.IndexOf("(")).Trim();
            }
            
            LoadLopData(maNganh, tenKhoaHoc);
        }
    }

    private Label[] CreateLabels()
    {
        var labelTexts = new[] { "Mã SV:", "Tên SV:", "Ngày sinh:", "Ngành:", "Khóa học:", "Lớp:", "Giới tính:", "SĐT:", "Quê quán:", "Email:", "CCCD:", "Trạng thái:" };
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
            CreateControlPanel(cbbKhoaHoc),
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
        txtMaSinhVien.Text = sinhVienDto.MaSinhVien.ToString();
        txtTenSinhVien.Text = sinhVienDto.TenSinhVien;
        
        if (DateTime.TryParse(sinhVienDto.NgaySinh, out DateTime ngaySinh))
        {
            dtpNgaySinh.Value = ngaySinh;
        }

        if (!string.IsNullOrEmpty(sinhVienDto.Nganh))
        {
            var nganhList = nganhDao.GetAll();
            var selectedNganh = nganhList.FirstOrDefault(n => n.TenNganh == sinhVienDto.Nganh);
            if (selectedNganh != null)
            {
                cbbNganh.SelectedValue = selectedNganh.MaNganh;
                
                if (sinhVienDto.MaKhoaHoc > 0)
                {
                    cbbKhoaHoc.SelectedValue = sinhVienDto.MaKhoaHoc;
                    
                    if (sinhVienDto.MaLop > 0)
                    {
                        var khoaHoc = khoaHocDao.GetById(sinhVienDto.MaKhoaHoc);
                        if (khoaHoc != null)
                        {
                            string tenKhoaHoc = khoaHoc.TenKhoaHoc;
                            LoadLopData(selectedNganh.MaNganh, tenKhoaHoc);
                            cbbLop.SelectedValue = sinhVienDto.MaLop;
                        }
                    }
                }
            }
        }
                
        if (sinhVienDto.GioiTinh == "Nam")
        {
            rbNam.Checked = true;
        }
        else if (sinhVienDto.GioiTinh == "Nữ")
        {
            rbNu.Checked = true;
        }

        txtSoDienThoai.Text = sinhVienDto.SdtSinhVien ?? "";
        txtQueQuan.Text = sinhVienDto.QueQuanSinhVien ?? "";
        txtEmail.Text = sinhVienDto.Email ?? "";
        txtCCCD.Text = sinhVienDto.CCCD ?? "";

        
        for (int i = 0; i < cbbTrangThai.Items.Count; i++)
        {
            if (cbbTrangThai.Items[i].ToString() == sinhVienDto.TrangThai)
            {
                cbbTrangThai.SelectedIndex = i;
                break;
            }
        }
    }

    private void SetControlsEnabled(bool enabled)
    {
        txtTenSinhVien.Enabled = !enabled;
        dtpNgaySinh.Enabled = !enabled;
        cbbNganh.Enabled = !enabled;
        cbbKhoaHoc.Enabled = !enabled;
        cbbLop.Enabled = !enabled;
        rbNam.Enabled = !enabled;
        rbNu.Enabled = !enabled;
        txtSoDienThoai.Enabled = enabled;
        txtQueQuan.Enabled = !enabled;
        txtEmail.Enabled = enabled;
        txtCCCD.Enabled = !enabled;
        cbbTrangThai.Enabled = enabled;
    }

    private void SetControlsEnabled2(bool disable)
    {
        txtTenSinhVien.Enabled = disable;
        dtpNgaySinh.Enabled = disable;
        cbbNganh.Enabled = disable;
        cbbKhoaHoc.Enabled = disable;
        cbbLop.Enabled = disable;
        rbNam.Enabled = disable;
        rbNu.Enabled = disable;
        txtSoDienThoai.Enabled = disable;
        txtQueQuan.Enabled = disable;
        txtEmail.Enabled = disable;
        txtCCCD.Enabled = disable;
        cbbTrangThai.Enabled = disable;
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
        
        // Validate cascading dropdown
        if (cbbNganh.SelectedIndex < 0 || cbbNganh.SelectedValue == null)
        {
            MessageBox.Show("Vui lòng chọn ngành.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cbbNganh.Focus();
            return false;
        }
        
        if (cbbKhoaHoc.SelectedIndex < 0 || cbbKhoaHoc.SelectedValue == null)
        {
            MessageBox.Show("Vui lòng chọn khóa học.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cbbKhoaHoc.Focus();
            return false;
        }
        
        if (cbbLop.SelectedIndex < 0 || cbbLop.SelectedValue == null)
        {
            MessageBox.Show("Vui lòng chọn lớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cbbLop.Focus();
            return false;
        }
        
        return true;
    }

    private SinhVienDTO CreateSinhVienDto()
    {
        // Get nganh name from selected value
        string nganhName = "";
        if (cbbNganh.SelectedValue != null)
        {
            var selectedNganh = nganhDao.GetNganhById((int)cbbNganh.SelectedValue);
            nganhName = selectedNganh.TenNganh;
        }

        return new SinhVienDTO
        {
            MaSinhVien = string.IsNullOrEmpty(txtMaSinhVien.Text) ? 0 : int.Parse(txtMaSinhVien.Text),
            TenSinhVien = txtTenSinhVien.Text.Trim(),
            NgaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
            GioiTinh = rbNam.Checked ? "Nam" : "Nữ",
            Nganh = nganhName,
            MaKhoaHoc = cbbKhoaHoc.SelectedValue != null ? (int)cbbKhoaHoc.SelectedValue : 0,
            MaLop = cbbLop.SelectedValue != null ? (int)cbbLop.SelectedValue : 0,
            SdtSinhVien = txtSoDienThoai.Text.Trim(),
            QueQuanSinhVien = txtQueQuan.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            CCCD = txtCCCD.Text.Trim(),
            TrangThai = cbbTrangThai.SelectedItem?.ToString() ?? "Đang học"
        };
    }
}