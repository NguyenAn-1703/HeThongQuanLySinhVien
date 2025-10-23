using System.Runtime.InteropServices.JavaScript;
using ExCSS;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Mysqlx.Crud;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class LichHocDialog : Form
{
    private string _title;
    private DialogType _dialogType;

    private TableLayoutPanel _mainLayout;
    private CustomButton _exitButton;
    private TitleButton _btnLuu;
    private List<LabelTextField> _listTextBox;

    private LabelTextField fieldPhong;
    LabelTextField fieldCa;
    LabelTextField fieldThu;
    LabelTextField fieldTietBd;
    LabelTextField fieldTietKt;
    LabelTextField fieldTuNgay;
    LabelTextField fieldDenNgay;
    LabelTextField fieldSoTuan;


    public event Action<LichHocDto> Finish;

    private PhongHocController _phongController;

    private LichHocDto selectedItem;


    public LichHocDialog(string title, DialogType dialogType, LichHocDto lich = null)
    {
        _title = title;
        _dialogType = dialogType;
        _phongController = PhongHocController.getInstance();
        selectedItem = lich;
        Init();
    }

    void Init()
    {
        Width = 800;
        Height = 700;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            BorderStyle = BorderStyle.FixedSingle,
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

        this.Controls.Add(_mainLayout);

        if (_dialogType == DialogType.ChiTiet)
        {
            SetupDetail();
        }
        else if (_dialogType == DialogType.Sua)
        {
            SetupUpdate();
        }
    }

    void SetTopBar()
    {
        TableLayoutPanel panel = new TableLayoutPanel
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

        _mainLayout.Controls.Add(panel);
    }

    void SetTitleBar()
    {
        TableLayoutPanel panel = new TableLayoutPanel
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

    private TableLayoutPanel _contentPanel;

    void SetContent()
    {
        _contentPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(7),
        };

        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));


        RoundTLP pnl1 = new RoundTLP { Dock = DockStyle.Fill, AutoSize = true };
        RoundTLP pnl2 = new RoundTLP
            { Dock = DockStyle.Fill, Border = true, Padding = new Padding(7), Margin = new Padding(3, 20, 6, 3) };
        RoundTLP pnl3 = new RoundTLP
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

    string[] temps = new[] { "Tiết 1", "Tiết 2", "Tiết 3", "Tiết 4", "Tiết 5" };
    private List<string> _listSang;

    string[] tempc = new[] { "Tiết 6", "Tiết 7", "Tiết 8", "Tiết 9", "Tiết 10" };
    private List<string> _listChieu;

    string[] tempa = new[]
        { "Tiết 1", "Tiết 2", "Tiết 3", "Tiết 4", "Tiết 5", "Tiết 6", "Tiết 7", "Tiết 8", "Tiết 9", "Tiết 10" };

    private List<string> _listAll;

    void ConfigCombobox()
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

        string[] ca = new[] { "Lý thuyết", "Thực hành" };
        fieldCa.SetComboboxList(ca.ToList());
        fieldCa.SetComboboxSelection(ca[0]);

        string[] thu = new[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
        fieldThu.SetComboboxList(thu.ToList());
        fieldThu.SetComboboxSelection(thu[0]);

        fieldTietBd.SetComboboxList(_listAll);
        fieldTietBd.SetComboboxSelection(_listAll[0]);

        fieldTietKt.SetComboboxList(_listAll);
        fieldTietKt.SetComboboxSelection(_listAll[0]);

        SetActionTiet();
        SetActionNgay();
    }

    void SetActionTiet()
    {
        fieldTietBd._combobox.combobox.SelectedIndexChanged += (sender, args) =>
        {
            int bd = GetTietInt(fieldTietBd.GetSelectionCombobox());

            if (bd <= 5)
            {
                List<string> kt = _listSang.ToList();
                for (int i = 0; i < bd - 1; i++)
                {
                    kt.RemoveAt(0);
                }

                fieldTietKt.SetComboboxList(kt);
                fieldTietKt.SetComboboxSelection(kt[0]);
            }
            else
            {
                List<string> kt = _listChieu.ToList();
                for (int i = 0; i < bd - 6; i++)
                {
                    kt.RemoveAt(0);
                }

                fieldTietKt.SetComboboxList(kt);
                fieldTietKt.SetComboboxSelection(kt[0]);
            }
        };
    }

    void SetActionNgay()
    {
        fieldTuNgay.GetDField().TextChanged += (sender, args) =>
        {
            DateTime startDate = fieldTuNgay.GetDField().Value;
            DateTime endDate = fieldDenNgay.GetDField().Value;

            if (startDate > endDate)
            {
                fieldDenNgay.GetDField().Value = startDate;
                endDate = startDate;
            }

            TimeSpan timeSpan = endDate - startDate;
            int days = timeSpan.Days;
            int week = days / 7;

            fieldSoTuan._numberField.contentTextBox.Text = week + "";
        };

        fieldDenNgay.GetDField().TextChanged += (sender, args) =>
        {
            DateTime startDate = fieldTuNgay.GetDField().Value;
            DateTime endDate = fieldDenNgay.GetDField().Value;

            if (startDate > endDate)
            {
                fieldDenNgay.GetDField().Value = startDate;
                endDate = startDate;
            }

            TimeSpan timeSpan = endDate - startDate;
            int days = timeSpan.Days;
            int week = days / 7;

            fieldSoTuan._numberField.contentTextBox.Text = week + "";
        };

        fieldSoTuan._numberField.contentTextBox.TextChanged += (sender, args) =>
        {
            DateTime startDate = fieldTuNgay.GetDField().Value;
            if (fieldSoTuan._numberField.contentTextBox.Focused == true)
            {
                string ngayS = fieldSoTuan._numberField.contentTextBox.Text;
                if (!ngayS.Equals(""))
                {
                    int days = int.Parse(ngayS) * 7;
                    fieldDenNgay.GetDField().Value = startDate.AddDays(days);
                }
            }
        };
    }

    void SetupDetail()
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

        TimeSpan timeSpan = selectedItem.DenNgay - selectedItem.TuNgay;
        int days = timeSpan.Days;
        int week = days / 7;

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
    void SetupUpdate()
    {
        fieldPhong.tbPH.contentTextBox.Text = _phongController.GetPhongHocById(selectedItem.MaPH).TenPH;
        fieldCa.SetComboboxSelection(selectedItem.Type);
        fieldThu.SetComboboxSelection(selectedItem.Thu);
        fieldTietBd.SetComboboxSelection("Tiết " + selectedItem.TietBatDau);
        fieldTietKt.SetComboboxSelection("Tiết " + selectedItem.TietKetThuc);

        fieldTuNgay.GetDField().Value = selectedItem.TuNgay;
        fieldDenNgay.GetDField().Value = selectedItem.DenNgay;

        TimeSpan timeSpan = selectedItem.DenNgay - selectedItem.TuNgay;
        int days = timeSpan.Days;
        int week = days / 7;

        fieldSoTuan._numberField.contentTextBox.Text = week + "";
    }

    void SetAction()
    {
        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            _btnLuu._mouseDown += () => { OnMouseDown(); };
        }
    }

    void OnMouseDown()
    {
        if (_dialogType == DialogType.Them)
        {
            Insert();
        }   
        else if (_dialogType == DialogType.Sua)
        {
            Update();
        }
    }

    void Insert()
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

        string tenPhong = fieldPhong.tbPH.contentTextBox.Text;

        if (!Validate(tenPhong))
        {
            return;
        }
        
        PhongHocDto phong = _phongController.GetByTen(tenPhong);

        int tietBd = int.Parse(fieldTietBd.GetSelectionCombobox().Split(" ")[1]);
        int tietKt = int.Parse(fieldTietKt.GetSelectionCombobox().Split(" ")[1]);

        LichHocDto lich = new LichHocDto
        {
            MaPH = phong.MaPH,
            Thu = fieldThu.GetSelectionCombobox(),
            TietBatDau = tietBd,
            TietKetThuc = tietKt,
            TuNgay = fieldTuNgay.GetDField().Value,
            DenNgay = fieldDenNgay.GetDField().Value,
            SoTiet = tietKt - tietBd,
            Type = fieldCa.GetSelectionCombobox(),
        };

        Finish?.Invoke(lich);
        this.Close();
    }
    
    void Update()
    {
        string tenPhong = fieldPhong.tbPH.contentTextBox.Text;

        if (!Validate(tenPhong))
        {
            return;
        }
        
        PhongHocDto phong = _phongController.GetByTen(tenPhong);

        int tietBd = int.Parse(fieldTietBd.GetSelectionCombobox().Split(" ")[1]);
        int tietKt = int.Parse(fieldTietKt.GetSelectionCombobox().Split(" ")[1]);

        LichHocDto lich = new LichHocDto
        {
            MaPH = phong.MaPH,
            Thu = fieldThu.GetSelectionCombobox(),
            TietBatDau = tietBd,
            TietKetThuc = tietKt,
            TuNgay = fieldTuNgay.GetDField().Value,
            DenNgay = fieldDenNgay.GetDField().Value,
            SoTiet = tietKt - tietBd,
            Type = fieldCa.GetSelectionCombobox(),
        };

        Finish?.Invoke(lich);
        this.Close();
    }

    int GetTietInt(string tiet)
    {
        string[] temp = tiet.Split(' ');
        return int.Parse(temp[1]);
    }

    void SetBottom()
    {
        //Thêm có Đặt lại, Lưu, Hủy
        TableLayoutPanel panel = new TableLayoutPanel
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

        this._mainLayout.Controls.Add(panel, 0, 3);
    }

    bool Validate(string tenPhong)
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

        return true;
    }
}