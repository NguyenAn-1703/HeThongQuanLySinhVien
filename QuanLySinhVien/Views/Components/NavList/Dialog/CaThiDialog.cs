using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.AddImg;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class CaThiDialog : Form
{
    private string _title;
    private DialogType _dialogType;
    private MyTLP _mainLayout;
    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    
    private CaThiController _CaThiController;
    private int _idCaThi;
    private TitleButton _btnLuu;
    public event Action Finish;

    public CaThiDialog(string title, DialogType dialogType, int idCaThi = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _CaThiController = CaThiController.GetInstance();
        _idCaThi = idCaThi;
        _title = title;
        _dialogType = dialogType;
        Init();
    }

    void Init()
    {
        Width = 1400;
        Height = 750;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new MyTLP()
        {
            Dock = DockStyle.Fill,
            RowCount = 5,
            BorderStyle = BorderStyle.FixedSingle,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };

        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTopBar();
        SetTitleBar();
        SetFilterBar();
        SetContent();
        SetBottom();

        this.Controls.Add(_mainLayout);

        SetAction();


        if (_dialogType == DialogType.Them)
        {
            SetupInsert();
        }
        else if (_dialogType == DialogType.Sua)
        {
            SetupUpdate();
        }
        else
        {
            SetupDetail();
        }
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

    private LabelTextField _hocKyField;
    private LabelTextField _namField;
    private void SetFilterBar()
    {
        MyTLP panel = new MyTLP
        {
            ColumnCount = 2,
            AutoSize = true,
            Anchor = AnchorStyles.Right,
        };

        _hocKyField = new LabelTextField("Học kỳ", TextFieldType.Combobox);
        _hocKyField._combobox.Font = GetFont.GetFont.GetMainFont(10, FontType.Regular);
        string[] listHK = new[] { "Học kỳ 1", "Học kỳ 2" };
        _hocKyField.SetComboboxList(listHK.ToList());
        _hocKyField.SetComboboxSelection("Học kỳ 1");
        
        _namField = new LabelTextField("Năm", TextFieldType.Year);
        _namField.Font =  GetFont.GetFont.GetMainFont(14, FontType.Regular);
        
        
        panel.Controls.Add(_hocKyField);
        panel.Controls.Add(_namField);
        
        _mainLayout.Controls.Add(panel);
    }
    

    private MyTLP _contentLayout;

    void SetContent()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
        };

        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        SetFieldContainer();
        SetTableSV();
        SetTableExam();
        _mainLayout.Controls.Add(_contentLayout);
    }

    void SetFieldContainer()
    {
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Top,
            AutoSize = true,
        };
        
        List<LabelTextField> list = new List<LabelTextField>();
        list.Add(new LabelTextField("Học phần", TextFieldType.ListBoxHP));
        list.Add(new LabelTextField("Phòng", TextFieldType.ListBoxPH));
        list.Add(new LabelTextField("Thứ", TextFieldType.Combobox));
        list.Add(new LabelTextField("Thời gian bắt đầu", TextFieldType.Time));
        list.Add(new LabelTextField("Thời lượng", TextFieldType.Timehhmm));
        
        foreach (LabelTextField item in list)
        {
            _listTextBox.Add(item);
            panel.Controls.Add(item);
        }
        
        string[] arrThu = new[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
        _listTextBox[2].SetComboboxList(arrThu.ToList());
        _listTextBox[2].SetComboboxSelection(arrThu[0]);
        
        _contentLayout.Controls.Add(panel);
    }


    private CustomTable _tableSinhVien;
    private List<SinhVienDTO> _rawDataSV;
    private List<object> _displayDatSV;
    
    void SetTableSV()
    {
        RoundTLP panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(10),
            Padding = new Padding(10),
            Border = true,
            RowCount = 2,
        };
        
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Label title = new Label
        {
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold),
            Text = "DS sinh viên học học phần",
        };
        
        _rawDataSV = new  List<SinhVienDTO>();
        _displayDatSV = new List<object>();
        string[] columnNameArr = new[] { "MaSV", "TenSV"};
        string[] headerArr = new[] { "Mã sinh viên", "Tên sinh viên" };
        _tableSinhVien = new CustomTable(headerArr.ToList(), columnNameArr.ToList(), _displayDatSV, false);
        
        panel.Controls.Add(title);
        panel.Controls.Add(_tableSinhVien);
        
        _contentLayout.Controls.Add(panel);
    }
    
    private CustomTable _tableSinhVienEx;
    private List<SinhVienDTO> _rawDataSVEx;
    private List<object> _displayDatSVEx;
    
    void SetTableExam()
    {
        RoundTLP panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(10),
            Padding = new Padding(10),
            Border = true,
            RowCount = 2,
        };
        
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        Label title = new Label
        {
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold),
            Text = "DS thi",
        };
        
        _rawDataSVEx = new  List<SinhVienDTO>();
        _displayDatSVEx = new List<object>();
        string[] columnNameArr = new[] { "MaSV", "TenSV"};
        string[] headerArr = new[] { "Mã sinh viên", "Tên sinh viên" };
        _tableSinhVienEx = new CustomTable(headerArr.ToList(), columnNameArr.ToList(), _displayDatSV, false);
        
        panel.Controls.Add(title);
        panel.Controls.Add(_tableSinhVienEx);
        _contentLayout.Controls.Add(panel);
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
    void SetupInsert()
    {
    }

    void SetupUpdate()
    {
        
    }

    void SetupDetail()
    {
        
    }

    void SetAction()
    {
        if (_dialogType == DialogType.Them)
        {
            _btnLuu._mouseDown += () => Insert();
        }

        if (_dialogType == DialogType.Sua)
        {
            _btnLuu._mouseDown += () => Update();
        }

    }

    void Insert()
    {

        
    }

    void Update()
    {

    }


    public bool Validate()
    {
        return true;
    }
}