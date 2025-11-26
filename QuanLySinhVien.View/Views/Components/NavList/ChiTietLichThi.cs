using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.NavList;

//Bên sinh viên
public class ChiTietLichThi : NavBase
{
    private readonly string _title = "Chi tiết lịch thi";

    private MyTLP _contentLayout;
    private MyTLP _filterContainer;
    private MyTLP _mainLayout;

    private SinhVienDTO _sinhvien;

    private SinhVienController _sinhVienController;
    private readonly string[] _listSelectionForComboBox = new[] { "" };
    
    private string _nam;
    private string _hocKy;
    
    private string ID = "CHITIETLICHTHI";

    public ChiTietLichThi(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _taiKhoan = taiKhoan;
        _sinhVienController = SinhVienController.GetInstance();
        _sinhvien = _sinhVienController.GetByMaTK(taiKhoan.MaTK);
        Init();
    }

    private void Init()
    {
        Dock = DockStyle.Fill;

        _mainLayout = new MyTLP
        {
            RowCount = 3,
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 0, 0, 50)
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTop();
        SetBottom();

        Controls.Add(_mainLayout);
    }

    private void SetTop()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Padding = new Padding(10),
            ColumnCount = 3,
            BackColor = MyColor.GrayBackGround
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.Controls.Add(getTitle());
        panel.Controls.Add(getLblHkNam());

        _mainLayout.Controls.Add(panel);
    }

    private void SetBottom()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3
        };
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));

        _mainLayout.Controls.Add(_contentLayout);
    }

    //////////////////////////////SETTOP///////////////////////////////

    private Label getLblHkNam()
    {
        var hknam = "học kỳ: " + _hocKy + " năm học: " + _nam;

        var hknamlbl = new Label
        {
            Text = hknam,
            Font = GetFont.GetFont.GetMainFont(13, FontType.Bold),
            AutoSize = true,
            Anchor = AnchorStyles.None
        };
        return hknamlbl;
    }

    private Label getTitle()
    {
        var titlePnl = new Label
        {
            Text = _title,
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true
        };
        return titlePnl;
    }

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox.ToList();
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}