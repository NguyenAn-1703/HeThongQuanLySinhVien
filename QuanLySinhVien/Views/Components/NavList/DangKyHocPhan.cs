using Accessibility;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList;

public class DangKyHocPhan : NavBase
{
    private string ID = "DANGKYHOCPHAN";
    private string _title = "Đăng ký học phần";
    private string[] _listSelectionForComboBox = new[] { "" };
    
    private CustomTable _tableMoDangKy;
    string[] _headerArray = new string[] { "Mã nhóm học phần", "Mã giảng viên" };
    List<string> _headerList;
    
    List<NhomHocPhanDto> _rawDataMoDangKy;
    List<object> _displayDataMoDangKy;
    
    
    private CustomTable _tableDangKy;
    
    List<NhomHocPhanDto> _rawDataDangKy;
    List<object> _displayDataDangKy;

    private NhomHocPhanController _nhomHocPhanController;
    
    private LichDangKyController _lichDangKyController;
    private LichDangKyDto _currentLichDangKy;
    
    public DangKyHocPhan(NhomQuyenDto quyen) : base(quyen)
    {
        _headerList = _headerArray.ToList();
        _rawDataMoDangKy = new List<NhomHocPhanDto>();
        _displayDataMoDangKy = new List<object>();
        _rawDataDangKy = new List<NhomHocPhanDto>();
        _displayDataDangKy = new List<object>();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _lichDangKyController = LichDangKyController.GetInstance();
        Init();
    }

    private MyTLP _mainLayout;
    private void Init()
    {
        Dock = DockStyle.Fill;

        _mainLayout = new MyTLP
        {
            RowCount = 3,
            Dock = DockStyle.Fill,
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetLich();
        SetTop();
        SetBottom();
        SetBtnExport();

        Controls.Add(_mainLayout);
    }
    private void SetTop()
    {
        MyTLP panel = new MyTLP
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

    private MyTLP _contentLayout;
    private void SetBottom()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
        };
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        SetDataTableMoDangKy();
        SetDataTableDangKy();
        SetAction();
        SetFilterContainer();
        SetHpMoDangKyContainer();
        SetHpDangKyContainer();
        
        _mainLayout.Controls.Add(_contentLayout);
    }

    //////////////////////////////SETTOP///////////////////////////////

    Label getLblHkNam()
    {
        string hknam = "học kỳ: " + _hocKy + " năm học: " + _nam;
        
        Label hknamlbl = new Label
        {
            Text = hknam,
            Font = GetFont.GetFont.GetMainFont(13, FontType.Bold),
            AutoSize = true,
            Anchor = AnchorStyles.None
        };
        return hknamlbl;
    }
    Label getTitle()
    {
        Label titlePnl = new Label
        {
            Text = _title,
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true,
        };
        return titlePnl;
    }
    //////////////////////////////SETTOP///////////////////////////////

    /// ///////////////////////////SETBOTTOM////////////////////////////////////


    void SetFilterContainer()
    {
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
        };
        LabelTextField field = new LabelTextField("Lớp", TextFieldType.NormalText);
        panel.Controls.Add(field);
        _contentLayout.Controls.Add(panel);
    }
    
    
    private RoundTLP _panelHpMoDangKy;
    private RoundTLP _panelHpDangKy;
    private LabelTextField _fieldHPMDK;
    
    void SetHpMoDangKyContainer()
    {
        _panelHpMoDangKy = new RoundTLP()
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 2,
            Border = true,
            Padding = new Padding(10),
        };
        _panelHpMoDangKy.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _panelHpMoDangKy.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _panelHpMoDangKy.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        _panelHpMoDangKy.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        _panelHpMoDangKy.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

        _fieldHPMDK = new LabelTextField("Học phần", TextFieldType.ListBoxHP);

        RoundTLP boxHeader = getHeaderBox("Học phần mở đăng ký");
        _panelHpMoDangKy.Controls.Add(boxHeader);
        _panelHpMoDangKy.SetColumnSpan(boxHeader,2);
        
        _panelHpMoDangKy.Controls.Add(_fieldHPMDK);
        _panelHpMoDangKy.Controls.Add(_tableMoDangKy, 0, 2);
        _panelHpMoDangKy.SetColumnSpan(_tableMoDangKy,2);
        
        
        _contentLayout.Controls.Add(_panelHpMoDangKy);
    }
    
    void SetHpDangKyContainer()
    {
        _panelHpDangKy = new  RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            Border = true,
            Padding = new Padding(10),
        };
        
        RoundTLP boxHeader = getHeaderBox("Học phần đã đăng ký");
        _panelHpDangKy.Controls.Add(boxHeader);
        
        _panelHpDangKy.Controls.Add(_tableDangKy);
        _contentLayout.Controls.Add(_panelHpDangKy);
    }
    
    RoundTLP getHeaderBox(string content)
    {
        RoundTLP panel = new RoundTLP
        {
            // Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.GrayBackGround,
        };
        Label titlePnl = new Label
        {
            Text = content,
            Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold),
            AutoSize = true,
            Dock = DockStyle.Left
        };
        panel.Controls.Add(titlePnl);
        return panel;
    }

    private TitleButton _exportBtn;
    void SetBtnExport()
    {
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
        };

        _exportBtn = new TitleButton("Xuất phiếu", "exportpdf.svg")
        {
            Anchor = AnchorStyles.Right,
            // Dock = DockStyle.Right,
        };
        
        panel.Controls.Add(_exportBtn);
        _mainLayout.Controls.Add(panel);
    }
    
    void SetDataTableMoDangKy()
    {
        _rawDataMoDangKy = _nhomHocPhanController.GetAll();
        SetDisplayDataMoDangKy();

        string[] columnNames = new[] { "MaNHP", "MaGV" };
        List<string> columnNamesList = columnNames.ToList();
        
        _tableMoDangKy = new CustomTable(_headerList, columnNamesList, _displayDataMoDangKy, false);
    }

    void SetDataTableDangKy()
    {
        _rawDataDangKy = new List<NhomHocPhanDto>(); //tam
        SetDisplayDataDangKy();

        string[] columnNames = new[] { "MaNHP", "MaGV" };
        List<string> columnNamesList = columnNames.ToList();
        
        _tableDangKy = new CustomTable(_headerList, columnNamesList, _displayDataDangKy, false);
    }

    void SetDisplayDataMoDangKy()
    {
        _displayDataMoDangKy = ConvertObject.ConvertToDisplay(_rawDataMoDangKy, x => new
            {
                MaNHP = x.MaNHP,
                MaGV = x.MaGV,
            }
        );
    }
    
    void SetDisplayDataDangKy()
    {
        _displayDataDangKy = ConvertObject.ConvertToDisplay(_rawDataDangKy, x => new
            {
                MaNHP = x.MaNHP,
                MaGV = x.MaGV,
            }
        );
    }

    void SetAction()
    {
        _tableMoDangKy.OnDetail += index => { Detail(index); };
    }

    void UpdateDataDisplay(List<NhomHocPhanDto> dtos)
    {
        this._displayDataMoDangKy = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaNHP = x.MaNHP,
            MaGV = x.MaGV,
        });
    }
    
    void Detail(int index){}


    private int _hocKy;
    private string _nam;
    
    void SetLich()
    {
        List<LichDangKyDto> listLich = _lichDangKyController.GetAll();
        DateTime now = DateTime.Now;
        foreach (LichDangKyDto lich in listLich)
        {
            if (now >= lich.ThoiGianBatDau && now <= lich.ThoiGianKetThuc)
            {
                _currentLichDangKy =  lich;
                List<NhomHocPhanDto> listNhp =  _nhomHocPhanController.GetByLichMaDangKy(lich.MaLichDK);
                _hocKy = listNhp[0].HocKy;
                _nam = listNhp[0].Nam;
                Console.WriteLine(_hocKy + "  "+ _nam);
            }
        }
        
    } 
        
    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return this._listSelectionForComboBox.ToList();
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}