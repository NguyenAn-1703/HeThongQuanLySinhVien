using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class ChuongTrinhDaoTaoDialog : Form
{

    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private ChuongTrinhDaoTaoController _chuongTrinhDaoTaoController;
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

    private CtdtSearchBar _leftSearchBar;
    private CtdtSearchBar _rightSearchBar;

    private string[] _headerArray = new[] { "Mã học phần", "Tên học phần", "Hành động" };
    private List<string> _header;
    
    private string[] _headerDetailArray = new[] { "Mã học phần", "Tên học phần"};
    private List<string> _headerDetail;
    
    private NganhController _nganhController;
    private ChuKyDaoTaoController _chuKyDaoTaoController;
    private ChuongTrinhDaoTao_HocPhanController _chuongTrinhDaoTao_HocPhanController;
    
    

    public ChuongTrinhDaoTaoDialog(string title, DialogType dialogType, List<InputFormItem> listIFI, ChuongTrinhDaoTaoController chuongTrinhDaoTaoController, int idChuongTrinhDaoTao = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _chuongTrinhDaoTaoController = chuongTrinhDaoTaoController;
        _idChuongTrinhDaoTao = idChuongTrinhDaoTao;
        _title = title;
        _hocPhanController = HocPhanController.GetInstance();
        
        _leftRawData = new List<HocPhanDto>();
        _leftDisplayData = new List<object>();
        _rightRawData = new List<HocPhanDto>();
        _rightDisplayData = new List<object>();
        _dialogType = dialogType;
        
        _header = _headerArray.ToList();
        _headerDetail = _headerDetailArray.ToList();
        
        _nganhController = NganhController.GetInstance();
        _chuKyDaoTaoController = ChuKyDaoTaoController.GetInstance();
        
        _chuongTrinhDaoTao_HocPhanController = ChuongTrinhDaoTao_HocPhanController.GetInstance();
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
            BorderStyle = BorderStyle.FixedSingle,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
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
            RowCount = 2,
            Dock = DockStyle.Fill,
            Margin = new Padding(0)
        };
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        // _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        // _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        SetTextBoxContainer();


        

        
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

        RoundTLP leftPnl = new RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new  Padding(15, 0, 15, 15),
            Border = true,
            BorderColor = MyColor.GraySelectColor,
            Margin = new Padding(15),
        };
        RoundTLP rightPnl = new RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new  Padding(15, 0, 15, 15),
            Border = true,
            BorderColor = MyColor.GraySelectColor,
            Margin = new Padding(15),
        };
        leftPnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        leftPnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        leftPnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        rightPnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        rightPnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        rightPnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        
        Label lblhp = GetLabel("Học phần: ");
        Label lblhpct = GetLabel("Học phần thuộc chương trình đào tạo: ");
        lblhp.Margin = new Padding(0, 20, 0, 0);
        lblhpct.Margin = new Padding(0, 20, 0, 0);
        lblhp.Anchor = AnchorStyles.None;
        lblhpct.Anchor = AnchorStyles.None;
        
        _leftSearchBar = new CtdtSearchBar(){Dock = DockStyle.Right};
        _rightSearchBar = new CtdtSearchBar(){Dock = DockStyle.Right};
        

        

        _leftRawData = _hocPhanController.GetAll();
        
        SetLeftDisplayData();
        string[] leftColumnNameArr = new[] { "MaHP", "TenHP", "ActionPlus" };
        _leftTable = new CTDTTable(_header, leftColumnNameArr.ToList(), _leftDisplayData, TableCTDTType.Plus);
        // _contentPanel.Controls.Add(_leftTable);

        SetRightDisplayData();
        string[] rightColumnNameArr = new[] { "MaHP", "TenHP", "ActionMinus" };
        _rightTable = new CTDTTable(_header, rightColumnNameArr.ToList(), _rightDisplayData, TableCTDTType.Minus);

        
        
        leftPnl.Controls.Add(lblhp);
        leftPnl.Controls.Add(_leftSearchBar);
        leftPnl.Controls.Add(_leftTable);
        
        rightPnl.Controls.Add(lblhpct);
        rightPnl.Controls.Add(_rightSearchBar);
        rightPnl.Controls.Add(_rightTable);
        
        _contentPanel.Controls.Add(leftPnl);
        _contentPanel.Controls.Add(rightPnl);
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
        _leftSearchBar.KeyDown += s => OnLeftSearch(s);
        _rightSearchBar.KeyDown += s => OnRightSearch(s);

        _leftTable.BtnClick += id => InsertHP(id);
        _rightTable.BtnClick += id => DeleteHP(id);

        if (_dialogType == DialogType.Them)
        {
            _btnLuu._mouseDown += () => Insert();
        }

        if (_dialogType == DialogType.Sua)
        {
            _btnLuu._mouseDown += () => Update();
        }
    }

    void SetLeftDisplayData()
    {
        _leftDisplayData = ConvertObject.ConvertToDisplay(_leftRawData, x => new
        {                          
            MaHP = x.MaHP,
            TenHP = x.TenHP,
        }); 
    }

    void SetupInsert()
    {
        List<NganhDto> listNganh = _nganhController.GetAll();
        List<string> listTenNganh = listNganh.Select(x => x.TenNganh).ToList();
        _listTextBox[0].SetComboboxList(listTenNganh);
        _listTextBox[0].SetComboboxSelection(listTenNganh[0]);
        
        List<ChuKyDaoTaoDto> listChuKy = _chuKyDaoTaoController.GetAll();
        List<string> listTenChuKy = listChuKy.Select(x => (x.NamBatDau + " - " + x.NamKetThuc)).ToList();
        _listTextBox[1].SetComboboxList(listTenChuKy);
        _listTextBox[1].SetComboboxSelection(listTenChuKy[0]);
    }

    void SetupUpdate()
    {
        ChuongTrinhDaoTaoDto chuongTrinh = _chuongTrinhDaoTaoController.GetById(_idChuongTrinhDaoTao);
        NganhDto nganh = _nganhController.GetNganhById(chuongTrinh.MaNganh);
        ChuKyDaoTaoDto chuKy = _chuKyDaoTaoController.GetById(chuongTrinh.MaCKDT);
        
        List<NganhDto> listNganh = _nganhController.GetAll();
        List<string> listTenNganh = listNganh.Select(x => x.TenNganh).ToList();
        _listTextBox[0].SetComboboxList(listTenNganh);
        _listTextBox[0].SetComboboxSelection(nganh.TenNganh);
        
        List<ChuKyDaoTaoDto> listChuKy = _chuKyDaoTaoController.GetAll();
        List<string> listTenChuKy = listChuKy.Select(x => (x.NamBatDau + " - " + x.NamKetThuc)).ToList();
        _listTextBox[1].SetComboboxList(listTenChuKy);
        _listTextBox[1].SetComboboxSelection(chuKy.NamBatDau + " - " + chuKy.NamKetThuc);

        List<ChuongTrinhDaoTao_HocPhanDto> listCTDT_HP = _chuongTrinhDaoTao_HocPhanController.GetByMaCTDT(_idChuongTrinhDaoTao);
        
        List<HocPhanDto> listHocPhan = new List<HocPhanDto>();
        foreach (ChuongTrinhDaoTao_HocPhanDto item in listCTDT_HP)
        {
            listHocPhan.Add(_hocPhanController.GetHocPhanById(item.MaHP));
        }
        _rightRawData = listHocPhan;
        _leftRawData.RemoveAll(a => listHocPhan.Any(b => a.MaHP == b.MaHP));
        UpdateTableWNewRawData();
    }

    void SetupDetail()
    {
        Console.WriteLine("chitiet");
        ChuongTrinhDaoTaoDto chuongTrinh = _chuongTrinhDaoTaoController.GetById(_idChuongTrinhDaoTao);
        NganhDto nganh = _nganhController.GetNganhById(chuongTrinh.MaNganh);
        ChuKyDaoTaoDto chuKy = _chuKyDaoTaoController.GetById(chuongTrinh.MaCKDT);
        
        List<NganhDto> listNganh = _nganhController.GetAll();
        List<string> listTenNganh = listNganh.Select(x => x.TenNganh).ToList();
        _listTextBox[0].SetComboboxList(listTenNganh);
        _listTextBox[0].SetComboboxSelection(nganh.TenNganh);
        
        List<ChuKyDaoTaoDto> listChuKy = _chuKyDaoTaoController.GetAll();
        List<string> listTenChuKy = listChuKy.Select(x => (x.NamBatDau + " - " + x.NamKetThuc)).ToList();
        _listTextBox[1].SetComboboxList(listTenChuKy);
        _listTextBox[1].SetComboboxSelection(chuKy.NamBatDau + " - " + chuKy.NamKetThuc);

        List<ChuongTrinhDaoTao_HocPhanDto> listCTDT_HP = _chuongTrinhDaoTao_HocPhanController.GetByMaCTDT(_idChuongTrinhDaoTao);
        
        List<HocPhanDto> listHocPhan = new List<HocPhanDto>();
        foreach (ChuongTrinhDaoTao_HocPhanDto item in listCTDT_HP)
        {
            listHocPhan.Add(_hocPhanController.GetHocPhanById(item.MaHP));
        }
        _rightRawData = listHocPhan;
        _leftRawData.RemoveAll(a => listHocPhan.Any(b => a.MaHP == b.MaHP));
        
        
        UpdateTableWNewRawData();

        _leftTable.EnableActionColumn();
        _rightTable.EnableActionColumn();
        _listTextBox[0].GetComboboxField().Enabled = false;
        _listTextBox[1].GetComboboxField().Enabled = false;


    }
    
    void SetRightDisplayData()
    {
        _rightDisplayData = ConvertObject.ConvertToDisplay(_rightRawData, x => new
        {                          
            MaHP = x.MaHP,
            TenHP = x.TenHP,
        }); 
    }

    void InsertHP(int maHP)
    {
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(maHP);
        _rightRawData.Add(hocPhan);
        _leftRawData.RemoveAll(a => a.MaHP == maHP);
        UpdateTableWNewRawData();
    }
    
    void DeleteHP(int maHP)
    {
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(maHP);
        _leftRawData.Add(hocPhan);
        _rightRawData.RemoveAll(a => a.MaHP == maHP);
        UpdateTableWNewRawData();
    }

    void UpdateTableWNewRawData()
    {
        _leftRawData = _leftRawData.OrderBy(x => x.MaHP).ToList();
        _rightRawData = _rightRawData.OrderBy(x => x.MaHP).ToList();
        SetRightDisplayData();
        SetLeftDisplayData();
        
        _leftTable._dataGridView.DataSource=_leftDisplayData;
        _rightTable._dataGridView.DataSource=_rightDisplayData;
        
    }

    void UpdateLeftDisplayData(List<HocPhanDto> dtos)
    {
        this._leftDisplayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaHP = x.MaHP,
            TenHP = x.TenHP,
        });
    }
    
    void UpdateRightDisplayData(List<HocPhanDto> dtos)
    {
        this._rightDisplayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaHP = x.MaHP,
            TenHP = x.TenHP,
        });
    }


    void OnLeftSearch(string input)
    {
        string keyword = input.ToLower().Trim();
        List<HocPhanDto> result = _leftRawData
            .Where(x => x.MaHP.ToString().ToLower().Contains(keyword) || 
                        x.TenHP.ToString().ToLower().Contains(keyword)
            )
            .ToList();
 
        UpdateLeftDisplayData(result);
        _leftTable._dataGridView.DataSource = _leftDisplayData;
    }
    
    void OnRightSearch(string input)
    {
        string keyword = input.ToLower().Trim();
        List<HocPhanDto> result = _rightRawData
            .Where(x => x.MaHP.ToString().ToLower().Contains(keyword) || 
                        x.TenHP.ToString().ToLower().Contains(keyword)
            )
            .ToList();

        UpdateRightDisplayData(result);
        _rightTable._dataGridView.DataSource = _rightDisplayData;
    }
    

    void Insert()
    {
        string startendYear = _listTextBox[1].GetSelectionCombobox();
        ChuKyDaoTaoDto chuky = _chuKyDaoTaoController.GetByStartYearEndYear(startendYear);
        Console.WriteLine("machuky: " + chuky.MaCKDT);
        
        string tenNganh = _listTextBox[0].GetSelectionCombobox();
        NganhDto nganh = _nganhController.GetByTen(tenNganh);
        Console.WriteLine("maNganh: " + nganh.MaNganh);
    }

    void UpdateTK(ChuongTrinhDaoTaoDto ChuongTrinhDaoTao)
    {
        
    }

    bool Validate(TextBox TxtTenDangNhap, TextBox TxtMatKhau, string tenDangNhap, string matKhau)
    {
        return true;
    }
    
    
}