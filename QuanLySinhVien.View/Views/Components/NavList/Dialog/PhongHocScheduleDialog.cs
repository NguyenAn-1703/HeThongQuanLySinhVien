using System.Globalization;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;
using System.Diagnostics;

// file này chỉ để hiển thị chi tiết lịch học ..., nếu có thể update thông báo ...
namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class PhongHocScheduleDialog : Form
{
    private readonly LichHocDao _lichHocDao;

    // var
    private readonly PhongHocDto _phongHoc;
    private List<LichHocDto> _lichHocList;
    private Panel _mainPanel;
    private FlowLayoutPanel _scheduleContainer;
    private Label _titleLabel;

    // constructor
    public PhongHocScheduleDialog(PhongHocDto phongHoc)
    {
        _phongHoc = phongHoc;
        _lichHocDao = LichHocDao.GetInstance();
        InitializeDialog();
        LoadScheduleData();
    }

    private void InitializeDialog()
    {
        // Cấu hình Form
        Text = $"Lịch học - {_phongHoc.TenPH}";
        Size = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = MyColor.GrayBackGround;

        // Main Panel
        _mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20)
        };

        // Title
        var titlePanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = MyColor.MainColor
        };


        // lấy thứ hiện tại ( thời gian thực ) 
        var dayOfWeek = DateTime.Now.ToString("dddd", new CultureInfo("vi-VN"));
        _titleLabel = new Label
        {
            Text = $"LỊCH HỌC PHÒNG {_phongHoc.TenPH}\n{dayOfWeek}, {DateTime.Now:dd/MM/yyyy}",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.White
        };
        titlePanel.Controls.Add(_titleLabel);

        // Info Panel
        var infoPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = Color.White,
            Padding = new Padding(20, 10, 20, 10)
        };

        var infoLabel = new Label
        {
            Text =
                $"Loại: {_phongHoc.LoaiPH} | Cơ sở: {_phongHoc.CoSo} | Sức chứa: {_phongHoc.SucChua} | Tình trạng: {_phongHoc.TinhTrang}",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 11, FontStyle.Regular),
            ForeColor = Color.DarkSlateGray,
            TextAlign = ContentAlignment.MiddleLeft
        };
        infoPanel.Controls.Add(infoLabel);

        // container
        _scheduleContainer = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(10)
        };

        // button Close
        var btnClose = new Button
        {
            Text = "Đóng",
            Size = new Size(100, 40),
            Dock = DockStyle.Bottom,
            BackColor = MyColor.MainColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (s, e) => Close();

        // Add controls
        _mainPanel.Controls.Add(_scheduleContainer);
        _mainPanel.Controls.Add(infoPanel);
        _mainPanel.Controls.Add(titlePanel);
        _mainPanel.Controls.Add(btnClose);

        Controls.Add(_mainPanel);
    }

    private void LoadScheduleData()
    {
        _scheduleContainer.Controls.Clear();

        try
        {
            // lấy lịch học hiện tại
            var today = DateTime.Now;
            _lichHocList = _lichHocDao.GetLichHocByPhongAndDate(_phongHoc.MaPH, today);

            // if null
            if (_lichHocList == null || _lichHocList.Count == 0)
            {
                var emptyPanel = new Panel
                {
                    Width = _scheduleContainer.Width - 40,
                    Height = 200,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5)
                };

                var emptyLabel = new Label
                {
                    Text = $"📅 Hôm nay ({today:dd/MM/yyyy}) phòng này không có lịch học.",
                    Font = new Font("Segoe UI", 14, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                emptyPanel.Controls.Add(emptyLabel);
                _scheduleContainer.Controls.Add(emptyPanel);
            
                // // Debug
                // Console.WriteLine($"Không tìm thấy lịch học nào cho phòng {_phongHoc.TenPH} vào ngày {today:dd/MM/yyyy}");
                // return;
            }

            Console.WriteLine($"Tìm thấy {_lichHocList.Count} tiết học hôm nay.");
        
            // Nếu có lịch -> Hiển thị danh sách
            foreach (var lichHoc in _lichHocList)
            {
                var scheduleCard = CreateScheduleCard(lichHoc);
                _scheduleContainer.Controls.Add(scheduleCard);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi tải lịch học: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // panel chi tiết lịch học
    private Panel CreateScheduleCard(LichHocDto lichHoc)
    {
        var card = new Panel
        {
            Width = _scheduleContainer.Width - 40,
            Height = 120,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(5),
            Padding = new Padding(0)
        };

        // Thời gian (bên trái)
        var timePanel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 120,
            BackColor = MyColor.MainColor
        };

        var timeLabel = new Label
        {
            Text = $"{lichHoc.ThoiGianBatDau}\n-\n{lichHoc.ThoiGianKetThuc}",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.White
        };

        var tietLabel = new Label
        {
            Text = $"Tiết {lichHoc.TietBatDau} - {lichHoc.TietKetThuc}",
            Dock = DockStyle.Bottom,
            Height = 30,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9, FontStyle.Regular),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(200, MyColor.MainColor.R, MyColor.MainColor.G, MyColor.MainColor.B)
        };

        timePanel.Controls.Add(timeLabel);
        timePanel.Controls.Add(tietLabel);

        // Thông tin môn học (bên phải)
        var infoPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15, 10, 10, 10)
        };

        var monHocLabel = new Label
        {
            Text = lichHoc.TenHP,
            AutoSize = true,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = MyColor.MainColor,
            Location = new Point(0, 5)
        };

        var giangVienLabel = new Label
        {
            Text = $"👤 Giảng viên: {lichHoc.TenGV}",
            AutoSize = true,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            ForeColor = Color.DarkSlateGray,
            Location = new Point(0, 35)
        };

        var siSoLabel = new Label
        {
            Text = $"👥 Sĩ số: {lichHoc.SiSo} sinh viên",
            AutoSize = true,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            ForeColor = Color.DarkSlateGray,
            Location = new Point(0, 60)
        };

        var soTietLabel = new Label
        {
            Text = $"📚 Số tiết: {lichHoc.SoTiet} tiết",
            AutoSize = true,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            ForeColor = Color.DarkSlateGray,
            Location = new Point(0, 85)
        };

        infoPanel.Controls.Add(monHocLabel);
        infoPanel.Controls.Add(giangVienLabel);
        infoPanel.Controls.Add(siSoLabel);
        infoPanel.Controls.Add(soTietLabel);

        card.Controls.Add(infoPanel);
        card.Controls.Add(timePanel);

        return card;
    }
}