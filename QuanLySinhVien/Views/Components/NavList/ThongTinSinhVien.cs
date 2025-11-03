using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components.NavList;

public class ThongTinSinhVien : NavBase
{
    private readonly CUse _cUse;

    private readonly string[] _listSelectionForComboBox = new[] { "" };

    private ThongTinSinhVienController _controller;
    private DiemSinhVienController _diemController;

    private ComboBox cboKyHoc;
    private string currentAvatarPath = ""; //todo lay tu trong database phan sinh vien anh dai dien

    private int currentMaSinhVien = 1;
    private DataGridView dgvDiem;
    private Label lblBacHeDaoTao;
    private Label lblCccdSv;
    private Label lblDiemTBHe10;
    private Label lblDiemTBHe4;
    private Label lblEmailCv;
    private Label lblEmailSv;
    private Label lblGioiTinhSv;
    private Label lblHoVaTenCv;
    private Label lblKhoa;

    private Label lblLop;

    // phần 1 thông tin sinh viên
    private Label lblMaSv;

    // phần 2 : thông tin khóa học
    private Label lblNganh;
    private Label lblNgaySinhSv;
    private Label lblNienKhoa;
    private Label lblNoiSinhSv;
    private Label lblSdtCv;
    private Label lblSdtSv;

    // Phần 3: Cố vấn học tập
    private Label lblTaiKhoanCv;
    private Label lblTenSv;
    private Label lblTinhTrangSv;
    private Label lblTongTinChi;

    private TableLayoutPanel leftInformation1;
    private TableLayoutPanel leftInformation2;
    private TableLayoutPanel leftInformation3;

    private PictureBox picAvatar;
    private Label txtBacHeDaoTao;
    private Label txtCccdSv;
    private Label txtEmailCv;
    private Label txtEmailSv;
    private Label txtGioiTinhSv;
    private Label txtHoVaTenCv;
    private Label txtKhoa;
    private Label txtLop;

    private Label txtMaSv;

    private Label txtNganh;
    private Label txtNgaySinhSv;
    private Label txtNienKhoa;
    private Label txtNoiSinhSv;
    private Label txtSdtCv;
    private Label txtSdtSv;

    private Label txtTaiKhoanCv;
    private Label txtTenSv;
    private Label txtTinhTrangSv;


    public ThongTinSinhVien(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _controller = new ThongTinSinhVienController();
        _diemController = new DiemSinhVienController();
        _cUse = new CUse();
        InitComponent();
        LoadSinhVienData(currentMaSinhVien);
    }

    private void InitComponent()
    {
        Console.WriteLine("InitComponent");
        var fullTable = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2
        };
        fullTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        fullTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

        var leftTable = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3
        };

        var parrentInformation1 = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2
        };
        parrentInformation1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
        parrentInformation1.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));

        var parrentInformation2 = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2
        };
        parrentInformation2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        parrentInformation2.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));

        var parrentInformation3 = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2
        };
        parrentInformation3.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        parrentInformation3.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));

        leftInformation1 = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 9,
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        leftInformation1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
        leftInformation1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));

        for (var i = 0; i < 9; i++) leftInformation1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 9));

        leftInformation2 = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 5,
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        leftInformation2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
        leftInformation2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));

        for (var i = 0; i < 5; i++) leftInformation2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 5));

        leftInformation3 = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 4,
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        leftInformation3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
        leftInformation3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));

        for (var i = 0; i < 4; i++) leftInformation3.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 4));

        var headerTtsv = new Label
        {
            Text = "Thông tin sinh viên",
            Font = GetFont.GetFont.GetMainFont(13, FontType.Bold),
            Dock = DockStyle.Fill,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(0, 135, 202),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var headerTtkh = new Label
        {
            Text = "Thông tin khóa học",
            Font = GetFont.GetFont.GetMainFont(13, FontType.Bold),
            Dock = DockStyle.Fill,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(0, 135, 202),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var headerCvht = new Label
        {
            Text = "Cố vấn học tập",
            Font = GetFont.GetFont.GetMainFont(13, FontType.Bold),
            Dock = DockStyle.Fill,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(0, 135, 202),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var rightTable = new TableLayoutPanel
        {
            RowCount = 3,
            Dock = DockStyle.Fill
        };

        var rightInformation1 = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(15)
        };

        picAvatar = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.WhiteSmoke
        };

        try
        {
            var defaultImage = new Bitmap(200, 200);
            using (var g = Graphics.FromImage(defaultImage))
            {
                g.Clear(Color.LightGray);
                g.DrawString("Ảnh sinh viên",
                    new Font("Arial", 14, FontStyle.Bold),
                    Brushes.DarkGray,
                    new PointF(20, 90));
            }

            picAvatar.Image = defaultImage;
        }
        catch
        {
            picAvatar.BackColor = Color.LightGray;
        }

        rightInformation1.Controls.Add(picAvatar);

        var rightInformation2 = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            RowCount = 2,
            ColumnCount = 1,
            Padding = new Padding(10)
        };
        rightInformation2.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        rightInformation2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var panelKyHoc = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(5)
        };

        var lblKyHoc = new Label
        {
            Text = "Kỳ học:",
            Font = GetFont.GetFont.GetMainFont(11f, FontType.Bold),
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0, 8, 10, 0)
        };

        cboKyHoc = new ComboBox
        {
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = GetFont.GetFont.GetMainFont(11f, FontType.Regular)
        };
        cboKyHoc.SelectedIndexChanged += CboKyHoc_SelectedIndexChanged;

        panelKyHoc.Controls.Add(lblKyHoc);
        panelKyHoc.Controls.Add(cboKyHoc);

        var panelDiemTB = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 2,
            BackColor = Color.FromArgb(240, 248, 255),
            Padding = new Padding(5)
        };

        for (var i = 0; i < 3; i++) panelDiemTB.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        panelDiemTB.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        panelDiemTB.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

        lblDiemTBHe10 = CreateDiemLabel("Điểm TB (Hệ 10):", "0.00");
        lblDiemTBHe4 = CreateDiemLabel("Điểm TB (Hệ 4):", "0.00");
        lblTongTinChi = CreateDiemLabel("Tổng tín chỉ:", "0");

        panelDiemTB.Controls.Add(
            new Label
            {
                Text = "Điểm TB (Hệ 10):", Font = GetFont.GetFont.GetMainFont(10f, FontType.Bold),
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);
        panelDiemTB.Controls.Add(lblDiemTBHe10, 1, 0);

        panelDiemTB.Controls.Add(
            new Label
            {
                Text = "Điểm TB (Hệ 4):", Font = GetFont.GetFont.GetMainFont(10f, FontType.Bold), Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 1);
        panelDiemTB.Controls.Add(lblDiemTBHe4, 1, 1);

        panelDiemTB.Controls.Add(
            new Label
            {
                Text = "Tổng tín chỉ:", Font = GetFont.GetFont.GetMainFont(10f, FontType.Bold), Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 2);
        panelDiemTB.Controls.Add(lblTongTinChi, 1, 2);

        rightInformation2.Controls.Add(panelKyHoc, 0, 0);
        rightInformation2.Controls.Add(panelDiemTB, 0, 1);

        var rightInformation3 = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        dgvDiem = _cUse.getDataView(0, 0, 0, 0);
        dgvDiem.Dock = DockStyle.Fill;
        dgvDiem.AllowUserToDeleteRows = false;
        dgvDiem.ReadOnly = true;
        dgvDiem.ColumnHeadersHeight = 40;
        dgvDiem.RowTemplate.Height = 35;

        dgvDiem.Columns.Add("TenHP", "Học phần");
        dgvDiem.Columns.Add("SoTC", "TC");
        dgvDiem.Columns.Add("DiemHe10", "H10");
        dgvDiem.Columns.Add("KetQua", "KQ");

        dgvDiem.Columns["SoTC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvDiem.Columns["DiemHe10"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvDiem.Columns["KetQua"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        dgvDiem.Columns["TenHP"].FillWeight = 150;
        dgvDiem.Columns["SoTC"].FillWeight = 40;
        dgvDiem.Columns["DiemHe10"].FillWeight = 50;
        dgvDiem.Columns["KetQua"].FillWeight = 50;

        dgvDiem.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 135, 202);
        dgvDiem.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvDiem.ColumnHeadersDefaultCellStyle.Font = GetFont.GetFont.GetMainFont(10f, FontType.Bold);
        dgvDiem.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        dgvDiem.EnableHeadersVisualStyles = false;

        rightInformation3.Controls.Add(dgvDiem);

        lblMaSv = CreateLabelTitle("Mã sinh viên:");
        lblTenSv = CreateLabelTitle("Tên sinh viên:");
        lblNgaySinhSv = CreateLabelTitle("Ngày sinh:");
        lblGioiTinhSv = CreateLabelTitle("Giới tính:");
        lblSdtSv = CreateLabelTitle("Số điện thoại:");
        lblCccdSv = CreateLabelTitle("Số CCCD:");
        lblEmailSv = CreateLabelTitle("Email:");
        lblNoiSinhSv = CreateLabelTitle("Nơi sinh:");
        lblTinhTrangSv = CreateLabelTitle("Tình trạng:");

        txtMaSv = CreateLabelValue("");
        txtTenSv = CreateLabelValue("");
        txtNgaySinhSv = CreateLabelValue("");
        txtGioiTinhSv = CreateLabelValue("");
        txtSdtSv = CreateLabelValue("");
        txtCccdSv = CreateLabelValue("");
        txtEmailSv = CreateLabelValue("");
        txtNoiSinhSv = CreateLabelValue("");
        txtTinhTrangSv = CreateLabelValue("");

        leftInformation1.Controls.Add(lblMaSv, 0, 0);
        leftInformation1.Controls.Add(txtMaSv, 1, 0);

        leftInformation1.Controls.Add(lblTenSv, 0, 1);
        leftInformation1.Controls.Add(txtTenSv, 1, 1);

        leftInformation1.Controls.Add(lblNgaySinhSv, 0, 2);
        leftInformation1.Controls.Add(txtNgaySinhSv, 1, 2);

        leftInformation1.Controls.Add(lblGioiTinhSv, 0, 3);
        leftInformation1.Controls.Add(txtGioiTinhSv, 1, 3);

        leftInformation1.Controls.Add(lblSdtSv, 0, 4);
        leftInformation1.Controls.Add(txtSdtSv, 1, 4);

        leftInformation1.Controls.Add(lblCccdSv, 0, 5);
        leftInformation1.Controls.Add(txtCccdSv, 1, 5);

        leftInformation1.Controls.Add(lblEmailSv, 0, 6);
        leftInformation1.Controls.Add(txtEmailSv, 1, 6);

        leftInformation1.Controls.Add(lblNoiSinhSv, 0, 7);
        leftInformation1.Controls.Add(txtNoiSinhSv, 1, 7);

        leftInformation1.Controls.Add(lblTinhTrangSv, 0, 8);
        leftInformation1.Controls.Add(txtTinhTrangSv, 1, 8);

        lblNganh = CreateLabelTitle("Ngành:");
        lblLop = CreateLabelTitle("Lớp:");
        lblKhoa = CreateLabelTitle("Khoa:");
        lblBacHeDaoTao = CreateLabelTitle("Bậc đào tạo:");
        lblNienKhoa = CreateLabelTitle("Niên khóa:");

        txtNganh = CreateLabelValue("");
        txtLop = CreateLabelValue("");
        txtKhoa = CreateLabelValue("");
        txtBacHeDaoTao = CreateLabelValue("");
        txtNienKhoa = CreateLabelValue("");

        leftInformation2.Controls.Add(lblNganh, 0, 0);
        leftInformation2.Controls.Add(txtNganh, 1, 0);

        leftInformation2.Controls.Add(lblLop, 0, 1);
        leftInformation2.Controls.Add(txtLop, 1, 1);

        leftInformation2.Controls.Add(lblKhoa, 0, 2);
        leftInformation2.Controls.Add(txtKhoa, 1, 2);

        leftInformation2.Controls.Add(lblBacHeDaoTao, 0, 3);
        leftInformation2.Controls.Add(txtBacHeDaoTao, 1, 3);

        leftInformation2.Controls.Add(lblNienKhoa, 0, 4);
        leftInformation2.Controls.Add(txtNienKhoa, 1, 4);

        lblTaiKhoanCv = CreateLabelTitle("Tài khoản:");
        lblHoVaTenCv = CreateLabelTitle("Họ và tên:");
        lblEmailCv = CreateLabelTitle("Email:");
        lblSdtCv = CreateLabelTitle("Số điện thoại:");

        txtTaiKhoanCv = CreateLabelValue("");
        txtHoVaTenCv = CreateLabelValue("");
        txtEmailCv = CreateLabelValue("");
        txtSdtCv = CreateLabelValue("");

        leftInformation3.Controls.Add(lblTaiKhoanCv, 0, 0);
        leftInformation3.Controls.Add(txtTaiKhoanCv, 1, 0);

        leftInformation3.Controls.Add(lblHoVaTenCv, 0, 1);
        leftInformation3.Controls.Add(txtHoVaTenCv, 1, 1);

        leftInformation3.Controls.Add(lblEmailCv, 0, 2);
        leftInformation3.Controls.Add(txtEmailCv, 1, 2);

        leftInformation3.Controls.Add(lblSdtCv, 0, 3);
        leftInformation3.Controls.Add(txtSdtCv, 1, 3);

        parrentInformation1.Controls.Add(headerTtsv, 0, 0);
        parrentInformation1.Controls.Add(leftInformation1, 0, 1);

        parrentInformation2.Controls.Add(headerTtkh, 0, 0);
        parrentInformation2.Controls.Add(leftInformation2, 0, 1);

        parrentInformation3.Controls.Add(headerCvht, 0, 0);
        parrentInformation3.Controls.Add(leftInformation3, 0, 1);

        leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
        leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        leftTable.Controls.Add(parrentInformation1, 0, 0);
        leftTable.Controls.Add(parrentInformation2, 0, 1);
        leftTable.Controls.Add(parrentInformation3, 0, 2);

        rightTable.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
        rightTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        rightTable.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        rightTable.Controls.Add(rightInformation1, 0, 0);
        rightTable.Controls.Add(rightInformation2, 0, 1);
        rightTable.Controls.Add(rightInformation3, 0, 2);

        fullTable.Controls.Add(leftTable, 0, 0);
        fullTable.Controls.Add(rightTable, 1, 0);

        Controls.Add(fullTable);
    }


    private Label CreateLabelTitle(string text)
    {
        return new Label
        {
            Text = text,
            Font = GetFont.GetFont.GetMainFont(11f, FontType.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(5),
            ForeColor = Color.DarkBlue
        };
    }

    private Label CreateLabelValue(string text)
    {
        return new Label
        {
            Text = text,
            Font = GetFont.GetFont.GetMainFont(11f, FontType.Regular),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(5),
            ForeColor = Color.Black,
            AutoEllipsis = true
        };
    }

    private Label CreateDiemLabel(string title, string value)
    {
        return new Label
        {
            Text = value,
            Font = GetFont.GetFont.GetMainFont(11f, FontType.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(0, 100, 180)
        };
    }

    public void LoadSinhVienData(int maSinhVien)
    {
        try
        {
            currentMaSinhVien = maSinhVien;
            var thongTin = _controller.GetThongTinSinhVien(maSinhVien);

            if (thongTin != null)
            {
                txtMaSv.Text = thongTin.MaSinhVien.ToString();
                txtTenSv.Text = thongTin.TenSinhVien ?? "Chưa cập nhật";
                txtNgaySinhSv.Text = thongTin.NgaySinh ?? "Chưa cập nhật";
                txtGioiTinhSv.Text = thongTin.GioiTinh ?? "Chưa cập nhật";
                txtSdtSv.Text = thongTin.SdtSinhVien ?? "Chưa cập nhật";
                txtCccdSv.Text = thongTin.Cccd ?? "Chưa cập nhật";
                txtEmailSv.Text = thongTin.Email ?? "Chưa cập nhật";
                txtNoiSinhSv.Text = thongTin.QueQuanSinhVien ?? "Chưa cập nhật";
                txtTinhTrangSv.Text = thongTin.TrangThai ?? "Chưa cập nhật";

                txtNganh.Text = thongTin.Nganh ?? "Chưa xác định";
                txtLop.Text = thongTin.Lop ?? "Chưa xác định";
                txtKhoa.Text = thongTin.Khoa ?? "Chưa xác định";
                txtBacHeDaoTao.Text = thongTin.BacDaoTao ?? "Chưa xác định";
                txtNienKhoa.Text = thongTin.NienKhoa ?? "Chưa xác định";

                txtTaiKhoanCv.Text = thongTin.TaiKhoanCoVanHocTap ?? "Chưa có";
                txtHoVaTenCv.Text = thongTin.TenCoVanHocTap ?? "Chưa có";
                txtEmailCv.Text = thongTin.EmailCoVanHocTap ?? "Chưa có";
                txtSdtCv.Text = thongTin.SdtCoVanHocTap ?? "Chưa có";
                LoadAvatar(thongTin.AnhDaiDienSinhVien);

                LoadDanhSachKyHoc(maSinhVien);
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin sinh viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải thông tin sinh viên: {ex.Message}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadAvatar(string avatarPath)
    {
        try
        {
            if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
            {
                currentAvatarPath = avatarPath;
                using (var tempImage = Image.FromFile(avatarPath))
                {
                    picAvatar.Image = new Bitmap(tempImage);
                }
            }
            else
            {
                var defaultImage = new Bitmap(200, 200);
                using (var g = Graphics.FromImage(defaultImage))
                {
                    g.Clear(Color.LightGray);
                    g.DrawString("Không có ảnh",
                        new Font("Arial", 14, FontStyle.Bold),
                        Brushes.DarkGray,
                        new PointF(20, 90));
                }

                picAvatar.Image = defaultImage;
                currentAvatarPath = "";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi load ảnh: {ex.Message}");
            var errorImage = new Bitmap(200, 200);
            using (var g = Graphics.FromImage(errorImage))
            {
                g.Clear(Color.LightGray);
                g.DrawString("Lỗi tải ảnh",
                    new Font("Arial", 14, FontStyle.Bold),
                    Brushes.Red,
                    new PointF(20, 90));
            }

            picAvatar.Image = errorImage;
        }
    }

    private void LoadDanhSachKyHoc(int maSinhVien)
    {
        try
        {
            cboKyHoc.Items.Clear();
            var danhSachKy = _diemController.GetDanhSachKyHoc(maSinhVien);

            if (danhSachKy.Count > 0)
            {
                foreach (var (hocKy, nam) in danhSachKy) cboKyHoc.Items.Add($"Kỳ {hocKy} - Năm {nam}");
                cboKyHoc.SelectedIndex = 0;
            }
            else
            {
                cboKyHoc.Items.Add("Chưa có dữ liệu");
                cboKyHoc.SelectedIndex = 0;
                dgvDiem.Rows.Clear();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi load danh sách kỳ học: {ex.Message}");
        }
    }


    private void CboKyHoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboKyHoc.SelectedIndex < 0) return;

        var selectedText = cboKyHoc.SelectedItem?.ToString() ?? "";
        if (selectedText == "Chưa có dữ liệu") return;

        try
        {
            var parts = selectedText.Split('-');
            var hocKy = int.Parse(parts[0].Replace("Kỳ", "").Trim());
            var nam = parts[1].Replace("Năm", "").Trim();

            LoadDiemSinhVien(currentMaSinhVien, hocKy, nam);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi parse kỳ học: {ex.Message}");
        }
    }


    private void LoadDiemSinhVien(int maSinhVien, int hocKy, string nam)
    {
        try
        {
            var danhSachDiem = _diemController.GetDiemTheoKy(maSinhVien, hocKy, nam);

            dgvDiem.Rows.Clear();
            foreach (var diem in danhSachDiem)
            {
                var rowIndex = dgvDiem.Rows.Add(
                    diem.TenHocPhan,
                    diem.SoTinChi,
                    diem.DiemHe10.ToString("0.0"),
                    diem.KetQua
                );


                var row = dgvDiem.Rows[rowIndex];
                if (diem.KetQua == "Đạt")
                {
                    row.Cells["KetQua"].Style.ForeColor = Color.Green;
                    row.Cells["KetQua"].Style.Font = GetFont.GetFont.GetMainFont(10f, FontType.Bold);
                }
                else if (diem.KetQua == "Không đạt")
                {
                    row.Cells["KetQua"].Style.ForeColor = Color.Red;
                    row.Cells["KetQua"].Style.Font = GetFont.GetFont.GetMainFont(10f, FontType.Bold);
                }
                else
                {
                    row.Cells["KetQua"].Style.ForeColor = Color.Orange;
                }
            }

            var (diemTBHe10, diemTBHe4, tongTinChi) = _diemController.TinhDiemTrungBinh(maSinhVien, hocKy, nam);
            lblDiemTBHe10.Text = diemTBHe10.ToString("0.00");
            lblDiemTBHe4.Text = diemTBHe4.ToString("0.00");
            lblTongTinChi.Text = tongTinChi.ToString();

            if (diemTBHe4 >= 3.6)
                lblDiemTBHe4.ForeColor = Color.Green;
            else if (diemTBHe4 >= 2.0)
                lblDiemTBHe4.ForeColor = Color.Orange;
            else
                lblDiemTBHe4.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi load điểm sinh viên: {ex.Message}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(_listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}