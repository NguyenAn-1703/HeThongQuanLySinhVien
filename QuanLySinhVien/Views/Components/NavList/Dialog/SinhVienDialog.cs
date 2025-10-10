using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class SinhVienDialog : Form
{
    public enum DialogMode { View, Create, Edit }

    public SinhVienDTO ResultSinhVienDto { get; private set; }

    private TextBox txtMaSinhVien;
    private TextBox txtTenSinhVien;
    private DateTimePicker dtpNgaySinh;
    private ComboBox cbbKhoa;
    private RadioButton rbNam;
    private RadioButton rbNu;
    private TextBox txtSoDienThoai;
    private TextBox txtQueQuan;
    private TextBox txtEmail;
    private TextBox txtCCCD;
    private ComboBox cbbTrangThai;
    private TextBox txtMaKhoaHoc;

    public SinhVienDialog(DialogMode mode, SinhVienDTO sinhVienDto = null)
    {
        Text = string.Empty;
        Size = new Size(500, 750);
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        BackColor = ColorTranslator.FromHtml("#F3F4F6");

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
                   mode == DialogMode.Edit ? "Cập nhật sinh viên" : "Chi tiết sinh viên",
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

        bool isView = mode == DialogMode.View;
        SetControlsEnabled(!isView);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 10,
            Padding = new Padding(0),
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 270));

        for (int i = 0; i < 10; i++)
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

        
        cbbKhoa = new ComboBox
        {
            Width = 300,
            Height = 32,
            Font = new Font("Montserrat", 10),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Padding = new Padding(6, 6, 6, 6),
            Margin = new Padding(0)
        };
        cbbKhoa.Items.AddRange(new[] {"Công nghệ thông tin"});

        
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

    private Label[] CreateLabels()
    {
        var labelTexts = new[] { "Mã SV:", "Tên SV:", "Ngày sinh:", "Khoa:", "Giới tính:", "SĐT:", "Quê quán:", "Email:", "CCCD:", "Trạng thái:" };
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
            CreateControlPanel(cbbKhoa),
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
        txtTenSinhVien.Enabled = enabled;
        dtpNgaySinh.Enabled = enabled;
        cbbKhoa.Enabled = enabled;
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
            Text = mode == DialogMode.Create ? "Thêm" : "Cập nhật",
            DialogResult = DialogResult.OK,
            Width = 100,
            Height = 32,
            Font = new Font("Montserrat", 10, FontStyle.Bold),
            BackColor = ColorTranslator.FromHtml("#07689F"),
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
            if (ValidateInput())
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
        if (string.IsNullOrWhiteSpace(txtTenSinhVien.Text))
        {
            MessageBox.Show("Tên sinh viên không được để trống.");
            txtTenSinhVien.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
        {
            MessageBox.Show("Số điện thoại không được để trống.");
            txtSoDienThoai.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            MessageBox.Show("Email không được để trống.");
            txtEmail.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtCCCD.Text))
        {
            MessageBox.Show("CCCD không được để trống.");
            txtCCCD.Focus();
            return false;
        }

        return true;
    }

    private SinhVienDTO CreateSinhVienDto()
    {
        return new SinhVienDTO
        {
            MaSinhVien = string.IsNullOrEmpty(txtMaSinhVien.Text) ? 0 : int.Parse(txtMaSinhVien.Text),
            TenSinhVien = txtTenSinhVien.Text.Trim(),
            NgaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
            GioiTinh = rbNam.Checked ? "Nam" : "Nữ",
            SdtSinhVien = txtSoDienThoai.Text.Trim(),
            QueQuanSinhVien = txtQueQuan.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            CCCD = txtCCCD.Text.Trim(),
            TrangThai = cbbTrangThai.SelectedItem?.ToString() ?? "Đang học"
        };
    }
    
    public void setEdit(SinhVienDTO sinhVienDto)
    {
        using (var dialog = new SinhVienDialog(DialogMode.Edit, sinhVienDto))
        {
            if (dialog.ShowDialog() == DialogResult.OK && dialog.ResultSinhVienDto != null)
            {
                
            }
        }
    }

    public void setDelete()
    {
        
    }
}