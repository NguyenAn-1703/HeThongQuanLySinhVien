using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class LichHocDialog : Form
{
    private readonly List<NhomHocPhanDto> _currentListNhp;
    private readonly DialogType _dialogType;

    private readonly List<LichHocDto> _listTam;
    private readonly string _title;

    private readonly LichHocDto selectedItem;

    private readonly string[] tempa = new[]
        { "Tiết 1", "Tiết 2", "Tiết 3", "Tiết 4", "Tiết 5", "Tiết 6", "Tiết 7", "Tiết 8", "Tiết 9", "Tiết 10" };

    private readonly string[] tempc = new[] { "Tiết 6", "Tiết 7", "Tiết 8", "Tiết 9", "Tiết 10" };

    private readonly string[] temps = new[] { "Tiết 1", "Tiết 2", "Tiết 3", "Tiết 4", "Tiết 5" };
    private TitleButton _btnLuu;

    private TableLayoutPanel _contentPanel;
    private CustomButton _exitButton;
    private LichHocController _lichController;

    private List<string> _listAll;
    private List<string> _listChieu;
    private List<string> _listSang;
    private List<LabelTextField> _listTextBox;

    private TableLayoutPanel _mainLayout;

    private PhongHocController _phongController;
    private LabelTextField fieldCa;
    private LabelTextField fieldDenNgay;

    private LabelTextField fieldPhong;
    private LabelTextField fieldSoTuan;
    private LabelTextField fieldThu;
    private LabelTextField fieldTietBd;
    private LabelTextField fieldTietKt;
    private LabelTextField fieldTuNgay;


    public LichHocDialog(string title, DialogType dialogType, List<LichHocDto> listTam,
        List<NhomHocPhanDto> currentLisNhp, LichHocDto lich = null)
    {
        _title = title;
        _dialogType = dialogType;
        _phongController = PhongHocController.getInstance();
        selectedItem = lich;
        _listTam = listTam;
        _currentListNhp = currentLisNhp;
        _lichController = LichHocController.GetInstance();
        Init();
    }


    public event Action<LichHocDto> Finish;

    private void Init()
    {
        Width = 800;
        Height = 600;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            BorderStyle = BorderStyle.FixedSingle
        };

        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTopBar();
        SetTitleBar();
        SetContent();
        SetBottom();

        SetAction();

        Controls.Add(_mainLayout);

        if (_dialogType == DialogType.ChiTiet)
            SetupDetail();
        else if (_dialogType == DialogType.Sua) SetupUpdate();
    }

    private void SetTopBar()
    {
        var panel = new TableLayoutPanel
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
        var panel = new TableLayoutPanel
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
        _contentPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(7)
        };

        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));


        var pnl1 = new RoundTLP { Dock = DockStyle.Fill, AutoSize = true };
        var pnl2 = new RoundTLP
            { Dock = DockStyle.Fill, Border = true, Padding = new Padding(7), Margin = new Padding(3, 20, 6, 3) };
        var pnl3 = new RoundTLP
            { Dock = DockStyle.Fill, Border = true, Padding = new Padding(7), Margin = new Padding(3, 20, 3, 3) };

        _listTextBox = new List<LabelTextField>();

        _listTextBox.Add(new LabelTextField("Phòng học", TextFieldType.ListBoxPH));
        _listTextBox.Add(new LabelTextField("Ca", TextFieldType.Combobox));
        _listTextBox.Add(new LabelTextField("Thứ", TextFieldType.Combobox));
        _listTextBox.Add(new LabelTextField("Tiết bắt đầu", TextFieldType.Combobox));
        _listTextBox.Add(new LabelTextField("Tiết kết thúc", TextFieldType.Combobox));
        _listTextBox.Add(new LabelTextField("Từ ngày", TextFieldType.Date));
        _listTextBox.Add(new LabelTextField("Đến ngày", TextFieldType.Date));
        _listTextBox.Add(new LabelTextField("Số tuần", TextFieldType.Number));

        //Phong, ca
        pnl1.Controls.Add(_listTextBox[0]);
        pnl1.Controls.Add(_listTextBox[1]);

        //Thu, tiet bat dau, tiet ket thuc
        pnl2.Controls.Add(_listTextBox[2]);
        pnl2.Controls.Add(_listTextBox[3]);
        pnl2.Controls.Add(_listTextBox[4]);

        //Tu ngay, den ngay, so tuan
        pnl3.Controls.Add(_listTextBox[5]);
        pnl3.Controls.Add(_listTextBox[6]);
        pnl3.Controls.Add(_listTextBox[7]);

        _contentPanel.Controls.Add(pnl1);
        _contentPanel.SetColumnSpan(pnl1, 2);
        _contentPanel.Controls.Add(pnl2);
        _contentPanel.Controls.Add(pnl3);

        _mainLayout.Controls.Add(_contentPanel);

        ConfigCombobox();
    }

    private void ConfigCombobox()
    {
        _listSang = temps.ToList();
        _listChieu = tempc.ToList();
        _listAll = tempa.ToList();

        fieldPhong = _listTextBox[0];
        fieldCa = _listTextBox[1];
        fieldThu = _listTextBox[2];
        fieldTietBd = _listTextBox[3];
        fieldTietKt = _listTextBox[4];
        fieldTuNgay = _listTextBox[5];
        fieldDenNgay = _listTextBox[6];
        fieldSoTuan = _listTextBox[7];

        var ca = new[] { "Lý thuyết", "Thực hành" };
        fieldCa.SetComboboxList(ca.ToList());
        fieldCa.SetComboboxSelection(ca[0]);

        var thu = new[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
        fieldThu.SetComboboxList(thu.ToList());
        fieldThu.SetComboboxSelection(thu[0]);

        fieldTietBd.SetComboboxList(_listAll);
        fieldTietBd.SetComboboxSelection(_listAll[0]);

        fieldTietKt.SetComboboxList(_listSang);
        fieldTietKt.SetComboboxSelection(_listSang[0]);

        SetActionTiet();
        SetActionNgay();
    }

    private void SetActionTiet()
    {
        fieldTietBd._combobox.combobox.SelectedIndexChanged += (sender, args) =>
        {
            var bd = GetTietInt(fieldTietBd.GetSelectionCombobox());

            if (bd <= 5)
            {
                var kt = _listSang.ToList();
                for (var i = 0; i < bd - 1; i++) kt.RemoveAt(0);

                fieldTietKt.SetComboboxList(kt);
                fieldTietKt.SetComboboxSelection(kt[0]);
            }
            else
            {
                var kt = _listChieu.ToList();
                for (var i = 0; i < bd - 6; i++) kt.RemoveAt(0);

                fieldTietKt.SetComboboxList(kt);
                fieldTietKt.SetComboboxSelection(kt[0]);
            }
        };
    }

    private void SetActionNgay()
    {
        fieldTuNgay.GetDField().TextChanged += (sender, args) =>
        {
            var startDate = fieldTuNgay.GetDField().Value;
            var endDate = fieldDenNgay.GetDField().Value;

            if (startDate > endDate)
            {
                fieldDenNgay.GetDField().Value = startDate;
                endDate = startDate;
            }

            var timeSpan = endDate - startDate;
            var days = timeSpan.Days;
            var week = days / 7;

            fieldSoTuan._numberField.contentTextBox.Text = week + "";
        };

        fieldDenNgay.GetDField().TextChanged += (sender, args) =>
        {
            var startDate = fieldTuNgay.GetDField().Value;
            var endDate = fieldDenNgay.GetDField().Value;

            if (startDate > endDate)
            {
                fieldDenNgay.GetDField().Value = startDate;
                endDate = startDate;
            }

            var timeSpan = endDate - startDate;
            var days = timeSpan.Days;
            var week = days / 7;

            fieldSoTuan._numberField.contentTextBox.Text = week + "";
        };

        fieldSoTuan._numberField.contentTextBox.TextChanged += (sender, args) =>
        {
            var startDate = fieldTuNgay.GetDField().Value;
            if (fieldSoTuan._numberField.contentTextBox.Focused)
            {
                var ngayS = fieldSoTuan._numberField.contentTextBox.Text;
                if (!ngayS.Equals(""))
                {
                    var days = int.Parse(ngayS) * 7;
                    fieldDenNgay.GetDField().Value = startDate.AddDays(days);
                }
            }
        };
    }

    private void SetupDetail()
    {
        // public int MaLH { get; set; }
        // public int MaPH { get; set; }
        // public int MaNHP { get; set; }
        // public string Thu { get; set; }
        // public int TietBatDau { get; set; }
        // public DateTime TuNgay { get; set; }
        // public DateTime DenNgay { get; set; }
        // public int TietKetThuc { get; set; }
        // public int SoTiet { get; set; }
        // public string Type { get; set; }
        fieldPhong.tbPH.contentTextBox.Text = _phongController.GetPhongHocById(selectedItem.MaPH).TenPH;
        fieldCa.SetComboboxSelection(selectedItem.Type);
        fieldThu.SetComboboxSelection(selectedItem.Thu);
        fieldTietBd.SetComboboxSelection("Tiết " + selectedItem.TietBatDau);
        fieldTietKt.SetComboboxSelection("Tiết " + selectedItem.TietKetThuc);

        fieldTuNgay.GetDField().Value = selectedItem.TuNgay;
        fieldDenNgay.GetDField().Value = selectedItem.DenNgay;

        var timeSpan = selectedItem.DenNgay - selectedItem.TuNgay;
        var days = timeSpan.Days;
        var week = days / 7;

        fieldSoTuan._numberField.contentTextBox.Text = week + "";

        fieldPhong.tbPH.Enable = false;
        fieldCa._combobox.Enable = false;
        fieldThu._combobox.Enable = false;
        fieldTietBd._combobox.Enable = false;
        fieldTietKt._combobox.Enable = false;
        fieldTuNgay._dField.Enabled = false;
        fieldDenNgay._dField.Enabled = false;
        fieldSoTuan._numberField.Enable = false;
    }

    private void SetupUpdate()
    {
        fieldPhong.tbPH.contentTextBox.Text = _phongController.GetPhongHocById(selectedItem.MaPH).TenPH;
        fieldCa.SetComboboxSelection(selectedItem.Type);
        fieldThu.SetComboboxSelection(selectedItem.Thu);
        fieldTietBd.SetComboboxSelection("Tiết " + selectedItem.TietBatDau);
        fieldTietKt.SetComboboxSelection("Tiết " + selectedItem.TietKetThuc);

        fieldTuNgay.GetDField().Value = selectedItem.TuNgay;
        fieldDenNgay.GetDField().Value = selectedItem.DenNgay;

        var timeSpan = selectedItem.DenNgay - selectedItem.TuNgay;
        var days = timeSpan.Days;
        var week = days / 7;

        fieldSoTuan._numberField.contentTextBox.Text = week + "";
    }

    private void SetAction()
    {
        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
            _btnLuu._mouseDown += () => { OnMouseDown(); };
    }

    private void OnMouseDown()
    {
        if (_dialogType == DialogType.Them)
            Insert();
        else if (_dialogType == DialogType.Sua) Update();
    }

    private void Insert()
    {
        // public int MaLH { get; set; }
        // public int MaPH { get; set; }
        // public int MaNHP { get; set; }
        // public string Thu { get; set; }
        // public int TietBatDau { get; set; }
        // public DateTime TuNgay { get; set; }
        // public DateTime DenNgay { get; set; }
        // public int TietKetThuc { get; set; }
        // public int SoTiet { get; set; }
        // public string Type { get; set; }

        var tenPhong = fieldPhong.tbPH.contentTextBox.Text;

        if (!Validate(tenPhong)) return;

        PhongHocDto phong = _phongController.GetByTen(tenPhong);

        var tietBd = int.Parse(fieldTietBd.GetSelectionCombobox().Split(" ")[1]);
        var tietKt = int.Parse(fieldTietKt.GetSelectionCombobox().Split(" ")[1]);

        var lich = new LichHocDto
        {
            MaPH = phong.MaPH,
            Thu = fieldThu.GetSelectionCombobox(),
            TietBatDau = tietBd,
            TietKetThuc = tietKt,
            TuNgay = fieldTuNgay.GetDField().Value,
            DenNgay = fieldDenNgay.GetDField().Value,
            SoTiet = tietKt - tietBd,
            Type = fieldCa.GetSelectionCombobox()
        };

        Finish?.Invoke(lich);
        Close();
    }

    private void Update()
    {
        var tenPhong = fieldPhong.tbPH.contentTextBox.Text;

        if (!Validate(tenPhong)) return;

        PhongHocDto phong = _phongController.GetByTen(tenPhong);

        var tietBd = int.Parse(fieldTietBd.GetSelectionCombobox().Split(" ")[1]);
        var tietKt = int.Parse(fieldTietKt.GetSelectionCombobox().Split(" ")[1]);

        var lich = new LichHocDto
        {
            MaPH = phong.MaPH,
            Thu = fieldThu.GetSelectionCombobox(),
            TietBatDau = tietBd,
            TietKetThuc = tietKt,
            TuNgay = fieldTuNgay.GetDField().Value,
            DenNgay = fieldDenNgay.GetDField().Value,
            SoTiet = tietKt - tietBd,
            Type = fieldCa.GetSelectionCombobox()
        };

        Finish?.Invoke(lich);
        Close();
    }

    private int GetTietInt(string tiet)
    {
        var temp = tiet.Split(' ');
        return int.Parse(temp[1]);
    }

    private void SetBottom()
    {
        //Thêm có Đặt lại, Lưu, Hủy
        var panel = new TableLayoutPanel
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

        _mainLayout.Controls.Add(panel, 0, 3);
    }

    private bool Validate(string tenPhong)
    {
        if (tenPhong.Equals(""))
        {
            MessageBox.Show("Phòng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            fieldPhong.tbPH.contentTextBox.Focus();
            return false;
        }

        if (!_phongController.ExistByTen(tenPhong))
        {
            MessageBox.Show("Phòng không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            fieldPhong.tbPH.contentTextBox.Focus();
            return false;
        }

        if (DuplicateLichHoc(fieldPhong.tbPH.contentTextBox.Text, fieldThu.GetSelectionCombobox(),
                fieldTietBd.GetSelectionCombobox(), fieldTietKt.GetSelectionCombobox()))
        {
            MessageBox.Show("Lịch học bị trùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        var maNhpTrung = DuplicateLichHocNhp(fieldPhong.tbPH.contentTextBox.Text, fieldThu.GetSelectionCombobox(),
            fieldTietBd.GetSelectionCombobox(), fieldTietKt.GetSelectionCombobox());
        if (maNhpTrung != -1)
        {
            MessageBox.Show("Lịch học bị trùng với lịch của nhóm học phần: " + maNhpTrung + " !", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private bool DuplicateLichHoc(string tenPhong, string thu, string tietBatDau, string tietKetThuc)
    {
        // int maPhong = _phongController.GetByTen(tenPhong).MaPH;
        var tietBd = int.Parse(tietBatDau.Split(" ")[1]);
        var tietKt = int.Parse(tietKetThuc.Split(" ")[1]);
        //trùng ở nhóm học phần hiện tại
        foreach (var item in _listTam)
            if (item.Thu.Equals(thu))
                if (item.TietBatDau <= tietKt && tietBd <= item.TietKetThuc)
                    return true;

        return false;
    }

    private int DuplicateLichHocNhp(string tenPhong, string thu, string tietBatDau, string tietKetThuc)
    {
        var rs = -1;
        int maPhong = _phongController.GetByTen(tenPhong).MaPH;
        var tietBd = int.Parse(tietBatDau.Split(" ")[1]);
        var tietKt = int.Parse(tietKetThuc.Split(" ")[1]);

        //trùng ở các nhóm học phần khác cùng đợt đăng ký
        foreach (var item in _currentListNhp)
        {
            List<LichHocDto> listLichOfNhp = _lichController.GetByMaNhp(item.MaNHP);

            foreach (var lich in listLichOfNhp)
                //(a1, a2) //(b1, b2) //dk: a1 < b2 && b1 < a2
                if (maPhong == lich.MaPH && thu.Equals(lich.Thu))
                    if (lich.TietBatDau <= tietKt && tietBd <= lich.TietKetThuc)
                    {
                        Console.WriteLine("lich cua nhp : " + lich.MaPH + " " + lich.Thu + " " + lich.TietBatDau + " " +
                                          lich.TietKetThuc);
                        Console.WriteLine("lich chen : " + maPhong + " " + thu + " " + tietBd + " " + tietKt);
                        rs = item.MaNHP;
                        return rs;
                    }
        }

        return rs;
    }
}