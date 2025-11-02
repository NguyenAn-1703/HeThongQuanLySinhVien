using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class HocPhiDialog : Form
{
    private string _title;
    private MyTLP _mainLayout;
    private CustomButton _exitButton;
    private List<LabelTextField> _listTextBox;
    private DialogType _dialogType;

    private SinhVienController _sinhVienController;

    private int _idSinhVien;
    private SinhVienDTO SelectedSv;
    private TitleButton _btnLuu;

    private int _hky;
    private string _nam;
    public event Action Finish;
    
    private HocPhiHocPhanController _hocPhiHocPhanController;
    private HocPhanController _hocPhanController;
    private LopController _lopController;
    private NganhController _nganhController;
    private HocPhiTinChiController _hocPhiTinChiController;
    private HocPhiSVController _hocPhiSVController;

    private double hocPhiTrenTinChi;

    public HocPhiDialog(string title, DialogType dialogType, int hky, string nam, int idSinhVien)
    {
        _listTextBox = new List<LabelTextField>();
        _sinhVienController = SinhVienController.GetInstance();
        _hocPhiHocPhanController = HocPhiHocPhanController.GetInstance();
        _hocPhanController =  HocPhanController.GetInstance();
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

    void Init()
    {
        Width = 700;
        Height = 500;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new MyTLP()
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            BorderStyle = BorderStyle.FixedSingle,
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

        this.Controls.Add(_mainLayout);

        SetAction();
        
        if (_dialogType == DialogType.Sua)
        {
            SetupUpdate();
        }
        else
        {
            SetupDetail();
        }
    }

    void SetHocPhiTinChi()
    {
        LopDto lop = _lopController.GetLopById(SelectedSv.MaLop);
        NganhDto nganh = _nganhController.GetNganhById(lop.MaNganh);
        hocPhiTrenTinChi = _hocPhiTinChiController.GetNewestHocPhiTinChiByMaNganh(nganh.MaNganh).SoTienMotTinChi;
    }
    

    void SetTopBar()
    {
        MyTLP panel = new MyTLP
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0),
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


        Label topTitle = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.Left,
            BackColor = MyColor.GrayBackGround,
            Dock = DockStyle.Fill,
            Margin = new Padding(0),
        };
        panel.Controls.Add(topTitle);

        _exitButton = new CustomButton(25, 25, "exitbutton.svg", MyColor.GrayBackGround, false, false, false, false);
        _exitButton.HoverColor = MyColor.GrayHoverColor;
        _exitButton.SelectColor = MyColor.GraySelectColor;
        _exitButton.Margin = new Padding(0);
        _exitButton.Anchor = AnchorStyles.Right;

        _exitButton.MouseDown += (sender, args) => this.Close();
        panel.Controls.Add(_exitButton);

        this._mainLayout.Controls.Add(panel);
    }

    void SetTitleBar()
    {
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.MainColor,
            Margin = new Padding(0),
            Padding = new Padding(0, 10, 0, 10),
        };

        Label title = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.None,
            AutoSize = true,
            ForeColor = MyColor.White,
            Font = GetFont.GetFont.GetMainFont(16, FontType.Black),
        };

        panel.Controls.Add(title);
        _mainLayout.Controls.Add(panel);
    }
    

    private MyTLP _contentLayout;

    void SetContent()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(10),
        };
        
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Label lblNam = new Label
        {
            Text = "Năm: " + _nam,
            Font = GetFont.GetFont.GetMainFont(9,  FontType.Bold),
            AutoSize = true,
        };
        
        Label lblHocKy = new Label
        {
            Text = "Học kỳ: " + _hky,
            Font = GetFont.GetFont.GetMainFont(9,  FontType.Bold),
            AutoSize = true,
        };
        
        _contentLayout.Controls.Add(lblNam);
        _contentLayout.Controls.Add(lblHocKy);
        SetTableHP();
        
        this._mainLayout.Controls.Add(_contentLayout);
    }

    private CustomTable _tableHp;
    private List<HocPhiHocPhanDto> _rawData;
    private List<object> _displayData;
    void SetTableHP()
    {
        string[] headers = new[] {"Mã học phần", "Học phần", "Tín chỉ", "Tổng tiền" };
        string[] columnNames = new[] {"MaHP", "TenHP","SoTinChi","TongTien" };
            
        _rawData = _hocPhiHocPhanController.GetByMaSVHocKyNam(_idSinhVien, _hky, _nam);
        UpdateDataDisplay(_rawData);
        
        _tableHp = new CustomTable(headers.ToList(), columnNames.ToList(), _displayData);
        _contentLayout.Controls.Add(_tableHp);
    }

    void UpdateDataDisplay(List<HocPhiHocPhanDto> dtos)
    {
        this._displayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaHP = _hocPhanController.GetHocPhanById(x.MaHP).MaHP,
            TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            SoTinChi = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
            TongTien = x.TongTien,
        });
    }


    void SetBottom()
    {
        //Thêm có Đặt lại, Lưu, Hủy
        MyTLP panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3,
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            panel.Controls.Add(new Panel { Height = 0 });

            _btnLuu = new TitleButton("Lưu");
            panel.Controls.Add(_btnLuu);

            TitleButton btnHuy = new TitleButton("Hủy");
            btnHuy._mouseDown += () => this.Close();

            panel.Controls.Add(btnHuy);
        }
        else
        {
            panel.Controls.Add(new Panel { Height = 0 });

            TitleButton btnThoat = new TitleButton("Thoát");
            btnThoat._mouseDown += () => this.Close();
            panel.Controls.Add(btnThoat, 2, 0);
        }

        this._mainLayout.Controls.Add(panel);
    }

    //Tổng tiền tất cả học phần
    private double tongCong = 0;
    void SetTongTienLbl()
    {
        foreach (HocPhiHocPhanDto hphp in _rawData)
        {
            tongCong += hphp.TongTien;
        }

        Label lblTongTien = new Label
        {
            Text = "Tổng cộng: " + FormatMoney.formatVN(tongCong),
            Font = GetFont.GetFont.GetMainFont(10, FontType.Bold),
            Dock = DockStyle.Right,
            AutoSize = true,
        };
        
        _mainLayout.Controls.Add(lblTongTien);
    }

    private CustomTextBox fieldDaThu;
    void SetPanelDatThu()
    {
        MyTLP panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Right,
            ColumnCount = 2,
        };
        fieldDaThu = new CustomTextBox();
        fieldDaThu.contentTextBox.Width = 200;
        fieldDaThu.contentTextBox.KeyPress += (sender, args) =>
        {
            if (!char.IsControl(args.KeyChar) && !char.IsDigit(args.KeyChar))
            {
                args.Handled = true;
            }
        };
        Label lblDaThu = new Label
        {
            Text = "Đã thu: ",
            Font = GetFont.GetFont.GetMainFont(10, FontType.Bold),
            Anchor = AnchorStyles.None,
            AutoSize = true,
        };
        panel.Controls.Add(lblDaThu);
        panel.Controls.Add(fieldDaThu);
        
        
        _mainLayout.Controls.Add(panel);
    }

    void SetupUpdate()
    {
        if (!_hocPhiSVController.ExistByMaSVHKyNam(_idSinhVien, _hky, _nam))
        {
            return;
        }

        double daThu = _hocPhiSVController.GetByMaSVHKyNam(_idSinhVien, _hky, _nam).DaThu;
        fieldDaThu.contentTextBox.Text = daThu.ToString();
    }

    void SetupDetail()
    {
        if (!_hocPhiSVController.ExistByMaSVHKyNam(_idSinhVien, _hky, _nam))
        {
            return;
        }

        double daThu = _hocPhiSVController.GetByMaSVHKyNam(_idSinhVien, _hky, _nam).DaThu;
        fieldDaThu.contentTextBox.Text = daThu.ToString();
        fieldDaThu.Enable = false;
    }

    void SetAction()
    {
        if (_dialogType == DialogType.Sua)
        {
            _btnLuu._mouseDown += () => Update();
        }
    }

    
    void Update()
    {
        string daThuS = fieldDaThu.contentTextBox.Text;
        
        if (!Validate(daThuS))
        {
            return;
        }
        
        double daThu = double.Parse(daThuS);
        //chưa có -> tạo mới, có -> update
        if (!_hocPhiSVController.ExistByMaSVHKyNam(_idSinhVien,  _hky, _nam))
        {
            HocPhiSVDto hocPhiSV = new HocPhiSVDto
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
        this.Close();
    }

    string GetTrangThai(double daThu)
    {
        Console.WriteLine(tongCong + " " +  daThu);
        return tongCong > daThu ? "Còn nợ" : "Đã đóng";
    }

    public bool Validate(string daThu)
    {
        if (CommonUse.Validate.IsEmpty(daThu))
        {
            MessageBox.Show("Số tiền thu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }
}