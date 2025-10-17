using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class ChuongTrinhDaoTaoDialog : Form
{

    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private ChuongTrinhDaoTaoController _ChuongTrinhDaoTaoController;
    private int _idChuongTrinhDaoTao;
    public event Action Finish;
    
    private TableLayoutPanel _mainLayout;
    private TitleButton _btnLuu;
    
    private string _title;
    private DialogType _dialogType;

    private TableLayoutPanel _contentPanel;
    
    private HocPhanController _hocPhanController;


    private CTDTTable _leftTable;
    private List<HocPhanDto> _leftRawData;
    private List<object> _leftDisplayData;
    
    private CTDTTable _rightTable;
    private List<HocPhanDto> _rightRawData;
    private List<object> _rightDisplayData;

    private string[] _headerArray = new[] { "Mã học phần", "Tên học phần", "Hành động" };
    private List<string> _header;
    
    

    public ChuongTrinhDaoTaoDialog(string title, DialogType dialogType, List<InputFormItem> listIFI, ChuongTrinhDaoTaoController chuongTrinhDaoTaoController, int idChuongTrinhDaoTao = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _ChuongTrinhDaoTaoController = chuongTrinhDaoTaoController;
        _idChuongTrinhDaoTao = idChuongTrinhDaoTao;
        _title = title;
        _hocPhanController = HocPhanController.GetInstance();
        
        _leftRawData = new List<HocPhanDto>();
        _leftDisplayData = new List<object>();
        _rightRawData = new List<HocPhanDto>();
        _rightDisplayData = new List<object>();
        
        _header = _headerArray.ToList();
        Init();
    }

    void Init()
    {
        Width = 1200;
        Height = 900;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        
        _mainLayout = new  TableLayoutPanel
        {
            Dock =  DockStyle.Fill,
            RowCount = 4,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };
        
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        SetTopBar();
        SetTitleBar();
        SetContent();
        
        this.Controls.Add(_mainLayout);
        
        
        
        // if (_dialogType == DialogType.Them)
        // {
        //     SetupInsert();
        // }
        // else if (_dialogType == DialogType.Sua)
        // {
        //     SetupUpdate();
        // }
        // else
        // {
        //     SetupDetail();
        // }
        
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
        
        this._mainLayout.Controls.Add(panel);
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

    void SetContent()
    {
        _contentPanel = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 4,
            Dock = DockStyle.Fill,
            Margin = new Padding(0)
        };
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        SetTextBoxContainer();

        Label lblhp = GetLabel("Học phần: ");
        Label lblhpct = GetLabel("Học phần thuộc chương trình đào tạo: ");
        lblhp.Margin = new Padding(0, 20, 0, 0);
        lblhpct.Margin = new Padding(0, 20, 0, 0);
        
        lblhp.Anchor = AnchorStyles.None;
        lblhpct.Anchor = AnchorStyles.None;
        
        _contentPanel.Controls.Add(lblhp);
        _contentPanel.Controls.Add(lblhpct);
        
        CtdtSearchBar leftSearchBar = new CtdtSearchBar(){Dock = DockStyle.Right};
        CtdtSearchBar rightSearchBar = new CtdtSearchBar(){Dock = DockStyle.Right};
        _contentPanel.Controls.Add(leftSearchBar);
        _contentPanel.Controls.Add(rightSearchBar);
        
        SetTable();
        
        _mainLayout.Controls.Add(_contentPanel);
        
        
    }

    void SetTextBoxContainer()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0),
            BackColor = MyColor.LightGray,
            ColumnCount = 2,
            // Padding = new Padding(100, 10, 100, 10),
        };
        
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        
        for (int i = 0; i < _listIFI.Count; i++)
        {
            LabelTextField textField = new LabelTextField(_listIFI[i].content, _listIFI[i].type)
            {
                Margin = new Padding(100, 5, 100, 5),
            };
            _listTextBox.Add(textField);
            panel.Controls.Add(_listTextBox[i]);
        }
        _contentPanel.Controls.Add(panel);
        _contentPanel.SetColumnSpan (panel, 2);
    }

    Label GetLabel(string text)
    {
        Label lbl = new Label
        {
            Text = text,
            Font = GetFont.GetFont.GetMainFont(11, FontType.SemiBold),
            AutoSize = true,
        };
        return lbl;
    }

    void SetTable()
    {
        //mahp, tenhp, hành động
        //mahp, tenhp, hành động

        _leftRawData = _hocPhanController.GetAll();
        
        SetLeftDisplayData();
        string[] leftColumnNameArr = new[] { "MaHP", "TenHP", "ActionPlus" };
        _leftTable = new CTDTTable(_header, leftColumnNameArr.ToList(), _leftDisplayData, TableCTDTType.Plus);
        _contentPanel.Controls.Add(_leftTable);
        
        string[] rightColumnNameArr = new[] { "MaHP", "TenHP", "ActionMinus" };
        _rightTable = new CTDTTable(_header, rightColumnNameArr.ToList(), _leftDisplayData, TableCTDTType.Minus);
        _contentPanel.Controls.Add(_rightTable);
        
    }

    void SetLeftDisplayData()
    {
        _leftDisplayData = ConvertObject.ConvertToDisplay(_leftRawData, x => new
        {                          
            MaHP = x.MaHP,
            TenHP = x.TenHP,
        }); 
    }
    

    void Insert()
    {
        

    }

    void UpdateTK(ChuongTrinhDaoTaoDto ChuongTrinhDaoTao)
    {
        
    }

    bool Validate(TextBox TxtTenDangNhap, TextBox TxtMatKhau, string tenDangNhap, string matKhau)
    {
        return true;
    }
    
    
}