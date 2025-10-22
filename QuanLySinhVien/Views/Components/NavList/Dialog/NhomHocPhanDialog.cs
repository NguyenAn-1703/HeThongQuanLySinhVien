using System.Diagnostics.PerformanceData;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NhomHocPhanDialog : Form
{
    
    private LichHocController _lichHocController;
    private HocPhanController _hocPhanController;
    private GiangVienController _giangVienController;
    private PhongHocController _phongHocController;
    
    private CustomButton _exitButton;
    private TableLayoutPanel _mainLayout;
    private string _title;
    private DialogType _dialogType;
    protected TitleButton _btnLuu;
    private TitleButton _insertButton;
    private CustomTable _tableLichHoc;
    private RoundTLP _lichContainer;

    private TableLayoutPanel _contentPanel;

    private List<LabelTextField> listField;
    
    private List<object> _lichDisplay;

    private string _hocKy;
    private string _nam;

    private Form _lichDialog;
    
    public NhomHocPhanDialog(string title, DialogType dialogType, string hocKy, string nam)
    {
        _lichDisplay = new List<object>();
        listField = new List<LabelTextField>();
        _lichHocController = LichHocController.GetInstance();
        _hocPhanController =  HocPhanController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _title = title;
        _dialogType = dialogType;
        _hocKy = hocKy;
        _nam = nam;
        Init();
    }

    void Init()
    {
        Size = new Size(1200, 600);

        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        
        _mainLayout = new TableLayoutPanel
        {
            // AutoSize = true,
            RowCount = 4,
            Dock = DockStyle.Fill,
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
        
        
        this.Controls.Add(_mainLayout);

        
    }

 

    void SetContent()
    {
        _contentPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
        };
        
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        SetFieldContainer();
        SetLichHocContainer();
        
        _mainLayout.Controls.Add(_contentPanel);
        
    }

    void SetFieldContainer()
    {
        listField.Add(new LabelTextField("Học kỳ", TextFieldType.NormalText));
        listField.Add(new LabelTextField("Năm", TextFieldType.NormalText));
        listField.Add(new LabelTextField("Học phần", TextFieldType.ListBoxHP));
        listField.Add(new LabelTextField("Giảng viên",  TextFieldType.ListBoxGV));
        listField.Add(new LabelTextField("Sĩ số", TextFieldType.NormalText));
        
        
        RoundTLP panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            Border = true,
            Margin = new Padding(7),
            Padding = new Padding(7),
        };

        foreach (LabelTextField field in listField)
        {
            panel.Controls.Add(field);
        }
        
        listField[0].SetText(_hocKy + "");
        listField[1].SetText(_nam + "");
        
        listField[0]._field.Enable = false;
        listField[1]._field.Enable = false;
        _contentPanel.Controls.Add(panel);
    }

    void SetLichHocContainer()
    {
        _lichContainer = new RoundTLP
        {
            Border = true,
            Dock = DockStyle.Fill,
            Margin = new Padding(7),
            RowCount = 3,
            Padding = new Padding(7),
        };

        Label lblLich = new Label
        {
            Text = "Lịch học",
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(13, FontType.Black),
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 10, 0, 0),
        };
        
        _lichContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _lichContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _lichContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        _lichContainer.Controls.Add(lblLich);

        _insertButton = new TitleButton("Thêm", "plus.svg");
        // _insertButton.Margin = new Padding(3, 3, 10, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(11, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        _lichContainer.Controls.Add(_insertButton);

        SetTableLichHoc();
        
        
        
        
        _contentPanel.Controls.Add(_lichContainer);
        
    }

    private void SetTableLichHoc()
    {
        string[] header = new[] { "Thứ", "Tiết bắt đầu", "Tiết kết thúc" };
        string[] columnNames = new[] { "Thu", "TietBatDau", "TietKetThuc" };
        List<string> columnNamesList = columnNames.ToList();
        _tableLichHoc = new CustomTable(header.ToList(), columnNamesList, _lichDisplay, true, true, true);

        _lichContainer.Controls.Add(_tableLichHoc);
    }

    void UpdateDisplayDataLich(List<LichHocDto> dtos)
    {
        _lichDisplay = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            Thu = x.Thu,
            TietBatDau = x.TietBatDau,
            TietKetThuc = x.TietKetThuc
        });
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
        
        _exitButton.MouseDown +=  (sender, args) => this.Close(); 
        panel.Controls.Add(_exitButton);
        
        _mainLayout.Controls.Add(panel);
    }
    
    void SetTitleBar()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            BackColor = MyColor.MainColor,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0),
            Padding = new Padding(0, 10, 0 , 10),
        };
        
        
        Label lbl = GetTitle();
        panel.Controls.Add(lbl);
        _mainLayout.Controls.Add(panel);
    }
    
    Label GetTitle()
    {
        Label title = new Label
        {
            Text = _title,
            AutoSize = true,
            Dock = DockStyle.Fill,
            Anchor = AnchorStyles.None,
            Font = GetFont.GetFont.GetMainFont(16,  FontType.ExtraBold),
            ForeColor = Color.White
        };
        return title;
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
            panel.Controls.Add(new Panel{Height = 0});
        
            _btnLuu = new TitleButton("Lưu");
            panel.Controls.Add(_btnLuu);
        
            TitleButton btnHuy = new TitleButton("Hủy");
            btnHuy._mouseDown += () => this.Close();
            
            panel.Controls.Add(btnHuy);
        }
        else
        {
            panel.Controls.Add(new Panel{Height = 0});
        
            TitleButton btnThoat = new TitleButton("Thoát");
            btnThoat._mouseDown += () => this.Close();
            panel.Controls.Add(btnThoat, 2, 0);
        }

        
        this._mainLayout.Controls.Add(panel, 0, 3);
    }

    void SetAction()
    {
        _insertButton._mouseDown += () => OnMouseDown();
    }

    void OnMouseDown()
    {
        
        LichHocDialog _lichDialog = new LichHocDialog("Lịch học", DialogType.Them);
        _lichDialog.ShowDialog();
        
    }
    
    
}