using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class HocPhiDialog : Form
{
    private readonly DialogType _dialogType;

    private readonly int _hky;

    private readonly int _idSinhVien;
    private readonly string _nam;
    private readonly string _title;
    private readonly SinhVienDTO SelectedSv;
    private TitleButton _btnLuu;


    private MyTLP _contentLayout;
    private List<object> _displayData;
    private CustomButton _exitButton;
    private HocPhanController _hocPhanController;

    private HocPhiHocPhanController _hocPhiHocPhanController;
    private HocPhiSVController _hocPhiSVController;
    private HocPhiTinChiController _hocPhiTinChiController;
    private List<LabelTextField> _listTextBox;
    private LopController _lopController;
    private MyTLP _mainLayout;
    private NganhController _nganhController;
    private List<HocPhiHocPhanDto> _rawData;

    private SinhVienController _sinhVienController;

    private CustomTable _tableHp;

    private CustomTextBox fieldDaThu;

    private double hocPhiTrenTinChi;

    //Tổng tiền tất cả học phần
    private double tongCong;

    public HocPhiDialog(string title, DialogType dialogType, int hky, string nam, int idSinhVien)
    {
        _listTextBox = new List<LabelTextField>();
        _sinhVienController = SinhVienController.GetInstance();
        _hocPhiHocPhanController = HocPhiHocPhanController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _lopController = LopController.GetInstance();
        _nganhController = NganhController.GetInstance();
        _hocPhiTinChiController = HocPhiTinChiController.GetInstance();
        _hocPhiSVController = HocPhiSVController.GetInstance();
        _idSinhVien = idSinhVien;
        _title = title;
        _dialogType = dialogType;
        _hky = hky;
        _nam = nam;
        SelectedSv = _sinhVienController.GetById(_idSinhVien);

        Init();
    }

    public event Action Finish;

    private void Init()
    {
        Width = 700;
        Height = 500;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            BorderStyle = BorderStyle.FixedSingle
        };

        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));


        SetHocPhiTinChi();
        SetTopBar();
        SetTitleBar();
        SetContent();
        SetTongTienLbl();
        SetPanelDatThu();
        SetBottom();

        Controls.Add(_mainLayout);

        SetAction();

        if (_dialogType == DialogType.Sua)
            SetupUpdate();
        else
            SetupDetail();
    }

    private void SetHocPhiTinChi()
    {
        LopDto lop = _lopController.GetLopById(SelectedSv.MaLop);
        NganhDto nganh = _nganhController.GetNganhById(lop.MaNganh);
        hocPhiTrenTinChi = _hocPhiTinChiController.GetNewestHocPhiTinChiByMaNganh(nganh.MaNganh).SoTienMotTinChi;
    }


    private void SetTopBar()
    {
        var panel = new MyTLP
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0)
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


        var topTitle = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.Left,
            BackColor = MyColor.GrayBackGround,
            Dock = DockStyle.Fill,
            Margin = new Padding(0)
        };
        panel.Controls.Add(topTitle);

        _exitButton = new CustomButton(25, 25, "exitbutton.svg", MyColor.GrayBackGround, false, false, false, false);
        _exitButton.HoverColor = MyColor.GrayHoverColor;
        _exitButton.SelectColor = MyColor.GraySelectColor;
        _exitButton.Margin = new Padding(0);
        _exitButton.Anchor = AnchorStyles.Right;

        _exitButton.MouseDown += (sender, args) => Close();
        panel.Controls.Add(_exitButton);

        _mainLayout.Controls.Add(panel);
    }

    private void SetTitleBar()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.MainColor,
            Margin = new Padding(0),
            Padding = new Padding(0, 10, 0, 10)
        };

        var title = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.None,
            AutoSize = true,
            ForeColor = MyColor.White,
            Font = GetFont.GetFont.GetMainFont(16, FontType.Black)
        };

        panel.Controls.Add(title);
        _mainLayout.Controls.Add(panel);
    }

    private void SetContent()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(10)
        };

        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var lblNam = new Label
        {
            Text = "Năm: " + _nam,
            Font = GetFont.GetFont.GetMainFont(9, FontType.Bold),
            AutoSize = true
        };

        var lblHocKy = new Label
        {
            Text = "Học kỳ: " + _hky,
            Font = GetFont.GetFont.GetMainFont(9, FontType.Bold),
            AutoSize = true
        };

        _contentLayout.Controls.Add(lblNam);
        _contentLayout.Controls.Add(lblHocKy);
        SetTableHP();

        _mainLayout.Controls.Add(_contentLayout);
    }

    private void SetTableHP()
    {
        var headers = new[] { "Mã học phần", "Học phần", "Tín chỉ", "Tổng tiền" };
        var columnNames = new[] { "MaHP", "TenHP", "SoTinChi", "TongTien" };

        _rawData = _hocPhiHocPhanController.GetByMaSVHocKyNam(_idSinhVien, _hky, _nam);
        UpdateDataDisplay(_rawData);

        _tableHp = new CustomTable(headers.ToList(), columnNames.ToList(), _displayData);
        _contentLayout.Controls.Add(_tableHp);
    }

    private void UpdateDataDisplay(List<HocPhiHocPhanDto> dtos)
    {
        _displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            _hocPhanController.GetHocPhanById(x.MaHP).MaHP,
            _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
            x.TongTien
        });
    }


    private void SetBottom()
    {
        //Thêm có Đặt lại, Lưu, Hủy
        var panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            panel.Controls.Add(new Panel { Height = 0 });

            _btnLuu = new TitleButton("Lưu");
            panel.Controls.Add(_btnLuu);

            var btnHuy = new TitleButton("Hủy");
            btnHuy._mouseDown += () => Close();

            panel.Controls.Add(btnHuy);
        }
        else
        {
            panel.Controls.Add(new Panel { Height = 0 });

            var btnThoat = new TitleButton("Thoát");
            btnThoat._mouseDown += () => Close();
            panel.Controls.Add(btnThoat, 2, 0);
        }

        _mainLayout.Controls.Add(panel);
    }

    private void SetTongTienLbl()
    {
        foreach (var hphp in _rawData) tongCong += hphp.TongTien;

        var lblTongTien = new Label
        {
            Text = "Tổng cộng: " + FormatMoney.formatVN(tongCong),
            Font = GetFont.GetFont.GetMainFont(10, FontType.Bold),
            Dock = DockStyle.Right,
            AutoSize = true
        };

        _mainLayout.Controls.Add(lblTongTien);
    }

    private void SetPanelDatThu()
    {
        var panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Right,
            ColumnCount = 2
        };
        fieldDaThu = new CustomTextBox();
        fieldDaThu.contentTextBox.Width = 200;
        fieldDaThu.contentTextBox.KeyPress += (sender, args) =>
        {
            if (!char.IsControl(args.KeyChar) && !char.IsDigit(args.KeyChar)) args.Handled = true;
        };
        var lblDaThu = new Label
        {
            Text = "Đã thu: ",
            Font = GetFont.GetFont.GetMainFont(10, FontType.Bold),
            Anchor = AnchorStyles.None,
            AutoSize = true
        };
        panel.Controls.Add(lblDaThu);
        panel.Controls.Add(fieldDaThu);


        _mainLayout.Controls.Add(panel);
    }

    private void SetupUpdate()
    {
        if (!_hocPhiSVController.ExistByMaSVHKyNam(_idSinhVien, _hky, _nam)) return;

        double daThu = _hocPhiSVController.GetByMaSVHKyNam(_idSinhVien, _hky, _nam).DaThu;
        fieldDaThu.contentTextBox.Text = daThu.ToString();
    }

    private void SetupDetail()
    {
        if (!_hocPhiSVController.ExistByMaSVHKyNam(_idSinhVien, _hky, _nam)) return;

        double daThu = _hocPhiSVController.GetByMaSVHKyNam(_idSinhVien, _hky, _nam).DaThu;
        fieldDaThu.contentTextBox.Text = daThu.ToString();
        fieldDaThu.Enable = false;
    }

    private void SetAction()
    {
        if (_dialogType == DialogType.Sua) _btnLuu._mouseDown += () => Update();
    }


    private void Update()
    {
        var daThuS = fieldDaThu.contentTextBox.Text;

        if (!Validate(daThuS)) return;

        var daThu = double.Parse(daThuS);
        bool isExist = _hocPhiSVController.ExistByMaSVHKyNam(_idSinhVien, _hky, _nam);

        //chưa có -> tạo mới, có -> update
        if (!isExist)
        {
            var hocPhiSV = new HocPhiSVDto
            {
                MaSV = _idSinhVien,
                HocKy = _hky,
                Nam = _nam,
                DaThu = daThu,
                TrangThai = GetTrangThai(daThu)
            };

            if (!_hocPhiSVController.Insert(hocPhiSV))
            {
                MessageBox.Show("Thêm hpsv that bai!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        else
        {
            HocPhiSVDto hocPhiSV = _hocPhiSVController.GetByMaSVHKyNam(_idSinhVien, _hky, _nam);
            hocPhiSV.DaThu = daThu;
            hocPhiSV.TrangThai = GetTrangThai(daThu);
            if (!_hocPhiSVController.Update(hocPhiSV))
            {
                MessageBox.Show("Sua hpsv that bai!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }

    private string GetTrangThai(double daThu)
    {
        Console.WriteLine(tongCong + " " + daThu);
        return tongCong > daThu ? "Còn nợ" : "Đã đóng";
    }

    public bool Validate(string daThu)
    {
        if (Shared.Validate.IsEmpty(daThu))
        {
            MessageBox.Show("Số tiền thu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }
}