using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
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
    
    private string[] _arrLHDT = new []{"Chính quy","Văn bằng 2","Vừa học vừa làm"};
    private List<string> _listLHDT;
    private string[] _arrTDDT = new []{"Cử nhân","Thạc sĩ","Tiến sĩ"};
    private List<string> _listTDDT;

    private List<HocPhanDto> _oldListUpdate;
    private List<HocPhanDto> _newListUpdate;
    

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
        _listLHDT = _arrLHDT.ToList();
        _listTDDT = _arrTDDT.ToList();
        
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
            RowCount = 2,
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

        _leftTable.OnDetail += i =>
        {
            new HocPhanDialog(DialogType.ChiTiet, _hocPhanController.GetHocPhanById(i), HocPhanDao.GetInstance()).ShowDialog();
        };
        
        _rightTable.OnDetail += i =>
        {
            new HocPhanDialog(DialogType.ChiTiet, _hocPhanController.GetHocPhanById(i), HocPhanDao.GetInstance()).ShowDialog();
        };

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
        SetupCombobox();
    }

    void SetupUpdate()
    {
        SetupCombobox();
        
        ChuongTrinhDaoTaoDto chuongTrinh = _chuongTrinhDaoTaoController.GetById(_idChuongTrinhDaoTao);
        
        NganhDto nganh = _nganhController.GetNganhById(chuongTrinh.MaNganh);
        ChuKyDaoTaoDto chuKy = _chuKyDaoTaoController.GetById(chuongTrinh.MaCKDT);
        
        _listTextBox[0].SetComboboxSelection(nganh.TenNganh);
        _listTextBox[1].SetComboboxSelection(chuKy.NamBatDau + " - " + chuKy.NamKetThuc);
        _listTextBox[2].SetComboboxSelection(chuongTrinh.LoaiHinhDT);
        _listTextBox[3].SetComboboxSelection(chuongTrinh.TrinhDo);
        

        List<ChuongTrinhDaoTao_HocPhanDto> listCTDT_HP = _chuongTrinhDaoTao_HocPhanController.GetByMaCTDT(_idChuongTrinhDaoTao);
        
        List<HocPhanDto> listHocPhan = new List<HocPhanDto>();
        foreach (ChuongTrinhDaoTao_HocPhanDto item in listCTDT_HP)
        {
            listHocPhan.Add(_hocPhanController.GetHocPhanById(item.MaHP));
        }
        _rightRawData = listHocPhan;
        _leftRawData.RemoveAll(a => listHocPhan.Any(b => a.MaHP == b.MaHP));
        UpdateTableWNewRawData();
        
        //chỉ cần lấy mã để so sánh
        _oldListUpdate = _rightRawData
            .Select(x => new HocPhanDto
            {
                MaHP = x.MaHP,
            })
            .ToList();;
    }

    void SetupDetail()
    {
        SetupCombobox();
        ChuongTrinhDaoTaoDto chuongTrinh = _chuongTrinhDaoTaoController.GetById(_idChuongTrinhDaoTao);
        NganhDto nganh = _nganhController.GetNganhById(chuongTrinh.MaNganh);
        ChuKyDaoTaoDto chuKy = _chuKyDaoTaoController.GetById(chuongTrinh.MaCKDT);
        
        _listTextBox[0].SetComboboxSelection(nganh.TenNganh);
        _listTextBox[1].SetComboboxSelection(chuKy.NamBatDau + " - " + chuKy.NamKetThuc);
        _listTextBox[2].SetComboboxSelection(chuongTrinh.LoaiHinhDT);
        _listTextBox[3].SetComboboxSelection(chuongTrinh.TrinhDo);

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
        _listTextBox[2].GetComboboxField().Enabled = false;
        _listTextBox[3].GetComboboxField().Enabled = false;

    }

    void SetupCombobox()
    {
        List<NganhDto> listNganh = _nganhController.GetAll();
        List<string> listTenNganh = listNganh.Select(x => x.TenNganh).ToList();
        _listTextBox[0].SetComboboxList(listTenNganh);
        _listTextBox[0].SetComboboxSelection(listTenNganh[0]);
        
        List<ChuKyDaoTaoDto> listChuKy = _chuKyDaoTaoController.GetAll();
        List<string> listTenChuKy = listChuKy.Select(x => (x.NamBatDau + " - " + x.NamKetThuc)).ToList();
        _listTextBox[1].SetComboboxList(listTenChuKy);
        _listTextBox[1].SetComboboxSelection(listTenChuKy[0]);
        
        _listTextBox[2].SetComboboxList(_listLHDT);
        _listTextBox[2].SetComboboxSelection(_listLHDT[0]);
        
        _listTextBox[3].SetComboboxList(_listTDDT);
        _listTextBox[3].SetComboboxSelection(_listTDDT[0]);
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
        
        string tenNganh = _listTextBox[0].GetSelectionCombobox();
        NganhDto nganh = _nganhController.GetByTen(tenNganh);
        
        string loaiHinhDaoTao = _listTextBox[2].GetSelectionCombobox();
        string trinhDoDaoTao = _listTextBox[2].GetSelectionCombobox();

        if (!_chuongTrinhDaoTaoController.ValidateNganhChuKy(nganh.MaNganh, chuky.MaCKDT))
        {
            MessageBox.Show("Đã tồn tại chương trình đào tạo của ngành này thuộc chu kỳ này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        List<HocPhanDto> listSelected = _rightRawData;
        if (listSelected.Count == 0)
        {
            MessageBox.Show("Chưa có học phần nào được chọn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_chuongTrinhDaoTaoController.Insert(new ChuongTrinhDaoTaoDto
            {
                MaCKDT = chuky.MaCKDT,
                MaNganh = nganh.MaNganh,
                LoaiHinhDT = loaiHinhDaoTao,
                TrinhDo = trinhDoDaoTao,
            }))
        {
            MessageBox.Show("Lỗi thêm chương trình đào tạo!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int MaCTDT = _chuongTrinhDaoTaoController.GetLastAutoIncrement();

        foreach (HocPhanDto dto in listSelected)
        {
            if (
                !_chuongTrinhDaoTao_HocPhanController.Insert(new ChuongTrinhDaoTao_HocPhanDto
                {
                    MaHP = dto.MaHP,
                    MaCTDT = MaCTDT,
                }))
            {
                MessageBox.Show("Lỗi thêm chi tiết chương trình đào tạo!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        
        MessageBox.Show("Thêm chương trình đào tạo thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        this.Close();
        Finish?.Invoke();
    }
    
    void Update()
    {
        _newListUpdate = _rightRawData;
        
        string startendYear = _listTextBox[1].GetSelectionCombobox();
        ChuKyDaoTaoDto chuky = _chuKyDaoTaoController.GetByStartYearEndYear(startendYear);
        
        string tenNganh = _listTextBox[0].GetSelectionCombobox();
        NganhDto nganh = _nganhController.GetByTen(tenNganh);
        
        string loaiHinhDaoTao = _listTextBox[2].GetSelectionCombobox();
        string trinhDoDaoTao = _listTextBox[2].GetSelectionCombobox();

        if (!_chuongTrinhDaoTaoController.ValidateNganhChuKy(_idChuongTrinhDaoTao,nganh.MaNganh, chuky.MaCKDT))
        {
            MessageBox.Show("Đã tồn tại chương trình đào tạo của ngành này thuộc chu kỳ này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        List<HocPhanDto> listSelected = _rightRawData;
        if (listSelected.Count == 0)
        {
            MessageBox.Show("Chưa có học phần nào được chọn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        if (!_chuongTrinhDaoTaoController.Update(new ChuongTrinhDaoTaoDto
            {
                MaCTDT = _idChuongTrinhDaoTao,
                MaCKDT = chuky.MaCKDT,
                MaNganh = nganh.MaNganh,
                LoaiHinhDT = loaiHinhDaoTao,
                TrinhDo = trinhDoDaoTao,
            }))
        {
            MessageBox.Show("Lỗi thêm chương trình đào tạo!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        

        //List cũ có, mới không có -> xóa
        foreach (HocPhanDto dto in _oldListUpdate)
        {
            if (!_newListUpdate.Any(x => x.MaHP == dto.MaHP))
            {
                _chuongTrinhDaoTao_HocPhanController.HardDelete(_idChuongTrinhDaoTao, dto.MaHP);
            }
        }
        //List cũ không có, mới có -> thêm
        foreach (HocPhanDto dto in _newListUpdate)
        {
            if (!_oldListUpdate.Any(x => x.MaHP == dto.MaHP))
            {
                _chuongTrinhDaoTao_HocPhanController.Insert(new ChuongTrinhDaoTao_HocPhanDto()
                {
                    MaCTDT = _idChuongTrinhDaoTao,
                    MaHP = dto.MaHP,
                });
            }
        }
        
        MessageBox.Show("Sửa chương trình đào tạo thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        this.Close();
        Finish?.Invoke();
    }

    void UpdateTK(ChuongTrinhDaoTaoDto ChuongTrinhDaoTao)
    {
        
    }

    bool Validate(TextBox TxtTenDangNhap, TextBox TxtMatKhau, string tenDangNhap, string matKhau)
    {
        return true;
    }
    
    
}