using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using QuanLySinhVien.Controllers;
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
    public event Action Finish;

    public LichHocDialog(string title, DialogType dialogType)
    {
        _title = title;
        _dialogType = dialogType;
        Init();
    }

    void Init()
    {
        Width = 600;
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

        this.Controls.Add(_mainLayout);
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

        LabelTextField fieldCa = _listTextBox[1];
        LabelTextField fieldThu = _listTextBox[2];
        LabelTextField fieldTietBd = _listTextBox[3];
        LabelTextField fieldTietKt = _listTextBox[4];
        LabelTextField fieldTuNgay = _listTextBox[5];
        LabelTextField fieldDenNgay = _listTextBox[6];
        LabelTextField fieldSoTuan = _listTextBox[7];

        string[] ca = new[] { "Thực hành", "Lý thuyết" };
        fieldCa.SetComboboxList(ca.ToList());
        fieldCa.SetComboboxSelection(ca[0]);

        string[] thu = new[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
        fieldThu.SetComboboxList(thu.ToList());
        fieldThu.SetComboboxSelection(thu[0]);

        fieldTietBd.SetComboboxList(_listAll);
        fieldTietBd.SetComboboxSelection(_listAll[0]);

        fieldTietKt.SetComboboxList(_listAll);
        fieldTietKt.SetComboboxSelection(_listAll[0]);

        SetActionTiet(fieldTietBd, fieldTietKt);
    }

    void SetActionTiet(LabelTextField fieldTietBd, LabelTextField fieldTietKt)
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
}