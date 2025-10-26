using Accessibility;
using Org.BouncyCastle.Tls;
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
    string[] _headerArrayMDK = new string[] {"Nhóm", "Mã học phần", "Tên học phần", "Số TC", "Lớp", "Số lượng", "Còn lại", "TKB" };
    List<string> _headerListMDK;
    
    string[] _headerArrayDK = new string[] {"Nhóm", "Mã học phần", "Tên học phần", "Số TC", "Lớp" };
    List<string> _headerListDK;
    
    List<NhomHocPhanDto> _rawDataMoDangKy;
    private List<NhomHocPhanDto> _rawDataFilter;
    List<object> _displayDataMoDangKy;
    
    private CustomTable _tableDangKy;
    
    List<NhomHocPhanDto> _rawDataDangKy;
    List<object> _displayDataDangKy;

    private SinhVienDTO _sinhvien;

    private NhomHocPhanController _nhomHocPhanController;
    private LichDangKyController _lichDangKyController;
    private HocPhanController _hocPhanController;
    private LopController _lopController;
    private NganhController _nganhController;
    private GiangVienController _giangVienController;
    private LichHocController _lichHocController;
    private PhongHocController _phongHocController;
    private DangKyController _dangKyController;
    private SinhVienController _sinhVienController;
    private LichDangKyDto _currentLichDangKy;
    private List<NhomHocPhanDto> _currentListNhp;
    
    public DangKyHocPhan(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _headerListMDK = _headerArrayMDK.ToList();
        _headerListDK =  _headerArrayDK.ToList();
        _rawDataMoDangKy = new List<NhomHocPhanDto>();
        _displayDataMoDangKy = new List<object>();
        _rawDataDangKy = new List<NhomHocPhanDto>();
        _displayDataDangKy = new List<object>();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _lichDangKyController = LichDangKyController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _lopController = LopController.GetInstance();
        _nganhController = NganhController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        _lichHocController =  LichHocController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _dangKyController = DangKyController.GetInstance();
        _sinhVienController = SinhVienController.GetInstance();
        _taiKhoan = taiKhoan;
        _sinhvien = _sinhVienController.GetByMaTK(taiKhoan.MaTK);
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
            Padding = new Padding(0, 0, 0, 50),
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetLich();
        SetTop();
        SetBottom();
        // SetBtnExport();
        

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
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
        
        SetDataTableMoDangKy();
        SetDataTableDangKy();
        SetFilterContainer();
        SetHpMoDangKyContainer();
        SetHpDangKyContainer();
        SetAction();


        
        
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
    private MyTLP _filterContainer;
    private LabelTextField _fieldNganh; 
    private LabelTextField _fieldLop; 
    void SetFilterContainer()
    {
        _filterContainer = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.GrayBackGround,
            ColumnCount = 3,
        };
        
        _filterContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        _filterContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        _filterContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        
        _fieldNganh = new LabelTextField("Ngành", TextFieldType.ListBoxNganh);
        _fieldLop = new LabelTextField("Lớp", TextFieldType.ListBoxLop);
        _fieldNganh.tbNganh.BackColor = MyColor.White;
        _fieldLop.tbLop.BackColor = MyColor.White;
        
        _filterContainer.Controls.Add(_fieldNganh);
        _filterContainer.Controls.Add(_fieldLop);
        
        _contentLayout.Controls.Add(_filterContainer);
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
            ColumnCount = 2,
            Border = true,
            Padding = new Padding(10),
        };
        
        RoundTLP boxHeader = getHeaderBox("Học phần đã đăng ký");
        _panelHpDangKy.Controls.Add(boxHeader);

        SetTinChiContainer();
        _panelHpDangKy.Controls.Add(_tableDangKy);
        _panelHpDangKy.SetColumnSpan(_tableDangKy, 2);
        
        _contentLayout.Controls.Add(_panelHpDangKy);
    }

    
    int _soMon;
    int _soTC;
    private Label _soTCsoHP;
    void SetTinChiContainer()
    {
        _soMon = 0;
        _soTC = 0;
        string txt = "Số môn: " + _soMon + " , Số tín chỉ: " + _soTC;
        _soTCsoHP = new Label
        {
            Text = txt,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            AutoSize = true,
            ForeColor = MyColor.Red,
            Margin = new Padding(15, 12, 3, 3),
            Dock = DockStyle.Left,
            
        };
        _panelHpDangKy.Controls.Add(_soTCsoHP);
    }

    void UpdateSoTC()
    {
        _soMon = 0;
        _soTC = 0;
        if (_rawDataDangKy.Count != 0)
        {
            foreach (NhomHocPhanDto nhp in _rawDataDangKy)
            {
                _soMon += 1;
                _soTC += _hocPhanController.GetHocPhanById(nhp.MaHP).SoTinChi;
            }
        }
        string txt = "Số môn: " + _soMon + " , Số tín chỉ: " + _soTC;
        _soTCsoHP.Text = txt;
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
        _rawDataMoDangKy = _currentListNhp;
        SetDisplayDataMoDangKy();

        string[] columnNames = new[] { "MaNHP","MaHP", "TenHP", "SoTC", "Lop", "SoLuong", "ConLai", "TKB"}; 
        List<string> columnNamesList = columnNames.ToList();
        
        _tableMoDangKy = new CustomTable(_headerListMDK, columnNamesList, _displayDataMoDangKy, false);

        SetCheckBoxTblMoDK();
        SetUpTableMoDangKy();
    }
    void SetCheckBoxTblMoDK()
    {
        _tableMoDangKy.AddColumn(ColumnType.CheckBox, "");
    }

    void SetUpTableMoDangKy()
    {
        _tableMoDangKy.ConfigDKHP();
    }

    void SetDataTableDangKy()
    {
        _rawDataDangKy = new List<NhomHocPhanDto>();
        SetDisplayDataDangKy();

        string[] columnNames = new[] { "MaNHP","MaHP", "TenHP", "SoTC", "Lop", "SoLuong", "ConLai", "TKB"}; 
        List<string> columnNamesList = columnNames.ToList();
        
        _tableDangKy = new CustomTable(_headerListDK, columnNamesList, _displayDataDangKy, false);
    }

    void SetDisplayDataMoDangKy()
    {
        _displayDataMoDangKy = ConvertObject.ConvertToDisplay(_rawDataMoDangKy, x => new
            {
                MaNHP = x.MaNHP,
                MaHP = x.MaHP,
                TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                SoTC = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
                Lop = _lopController.GetLopById(x.MaLop).TenLop,
                SoLuong = x.SiSo,
                ConLai = 0,
                TKB = GetTkbByNhp(x),
            }
        );

        _rawDataFilter = _rawDataMoDangKy;
    }


    string GetTkbByNhp(NhomHocPhanDto nhp)
    {
        string rs = "";
        List<LichHocDto> lich = _lichHocController.GetByMaNhp(nhp.MaNHP);
        GiangVienDto GV = _giangVienController.GetById(nhp.MaGV);
        
        foreach (LichHocDto d in lich)
        {
            PhongHocDto phong = _phongHocController.GetPhongHocById(d.MaPH);

            rs += d.Thu + " , tiết " + d.TietBatDau +
                  " đến " + d.TietKetThuc + " , PH " + phong.TenPH +
                  " , GV " + GV.TenGV + " ";
        }
        
        return rs;
    }
    
    void SetDisplayDataDangKy()
    {
        _displayDataDangKy = ConvertObject.ConvertToDisplay(_rawDataDangKy, x => new
            {
                MaNHP = x.MaNHP,
                MaHP = x.MaHP,
                TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                SoTC = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
                Lop = _lopController.GetLopById(x.MaLop).TenLop,
                SoLuong = x.SiSo,
                ConLai = 0,
            }
        );
    }

    void SetStatusCB()
    {
        List<DangKyDto> listDk = _dangKyController.GetByMaSV(_sinhvien.MaSinhVien);
        List<DangKyDto> listDkHocKy = new  List<DangKyDto>();
        
        foreach (NhomHocPhanDto nhp in _currentListNhp) 
        {
            if (listDk.Any(x => x.MaNHP == nhp.MaNHP)) // Kiểm tra có nhp nào nằm trong current list không
            {
                DangKyDto dk = new DangKyDto
                {
                    MaNHP = nhp.MaNHP,
                    MaSV = _sinhvien.MaSinhVien,
                };
                listDkHocKy.Add(dk);
            }
        }

        foreach (DangKyDto dk in listDkHocKy)
        {
            Console.WriteLine(dk.MaNHP);
        }
        
        if (listDkHocKy.Count == 0) return;
        foreach (DangKyDto d in listDkHocKy)
        {
            NhomHocPhanDto nhp = _nhomHocPhanController.GetById(d.MaNHP);
            _tableMoDangKy.tickCB(nhp.MaNHP);

            _rawDataDangKy.Add(nhp);
            UpdateDataDisplayDangKy(_rawDataDangKy);
            _tableDangKy._dataGridView.DataSource = _displayDataDangKy;
        }
    }

    void SetAction()
    {
        _tableMoDangKy.OnDetail += index => { Detail(index); };
        _tableMoDangKy.ClickCB += (i, b) => { OnClickCB(i, b); };
        _fieldNganh.tbNganh.contentTextBox.TextChanged += (sender, args) => FilterNganh();
        _fieldLop.tbLop.contentTextBox.TextChanged += (sender, args) => FilterLop();
        _fieldHPMDK.tb.contentTextBox.TextChanged += (sender, args) => FilterHp();
        _tableMoDangKy._dataGridView.DataBindingComplete += (sender, args) =>
        {
            SetStatusCB();
            UpdateSoTC();
        };
    }

    void FilterNganh()
    {
        string tenNganh = _fieldNganh.tbNganh.contentTextBox.Text;
        _rawDataFilter = _rawDataMoDangKy.Where(x => 
            _nganhController.GetNganhById(_lopController.GetLopById(x.MaLop).MaNganh).TenNganh.Equals(tenNganh)
                ).ToList();
        UpdateDataDisplayMoDangKy(_rawDataFilter);
        _tableMoDangKy._dataGridView.DataSource = _displayDataMoDangKy;
    }
    
    void FilterLop()
    {
        string tenLop = _fieldLop.tbLop.contentTextBox.Text;
        _rawDataFilter = _rawDataMoDangKy.Where(x => 
            _lopController.GetLopById(x.MaLop).TenLop.Equals(tenLop)).ToList();
        UpdateDataDisplayMoDangKy(_rawDataFilter);
        _tableMoDangKy._dataGridView.DataSource = _displayDataMoDangKy;
    }

    void FilterHp()
    {
        string tenHp = _fieldHPMDK.tb.contentTextBox.Text;
        _rawDataFilter = _rawDataMoDangKy.Where(x => 
            _hocPhanController.GetHocPhanById(x.MaHP).TenHP.Equals(tenHp)).ToList();
        UpdateDataDisplayMoDangKy(_rawDataFilter);
        _tableMoDangKy._dataGridView.DataSource = _displayDataMoDangKy;
    }

    void OnClickCB(int index, bool b)
    {
        NhomHocPhanDto nhp = _rawDataMoDangKy.First(x => x.MaNHP == index);
        if (b)
        {
            int maHpTrung = DuplicateLichHp(nhp);
            if (maHpTrung != -1)
            {
                MessageBox.Show("Lich hoc bị trùng với học phần có mã: " + maHpTrung + " !", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _tableMoDangKy.FailCb(index);
                return;
            }
            DangKyDto dangKy = new DangKyDto
            {
                MaNHP = nhp.MaNHP,
                MaSV = _sinhvien.MaSinhVien, 
            };
            if (!_dangKyController.Insert(dangKy))
            {
                MessageBox.Show("Them dang ky that bai: " + maHpTrung + " !", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            _rawDataDangKy.Add(nhp);
        }
        else
        {
            DangKyDto dangKy = new DangKyDto
            {
                MaNHP = nhp.MaNHP,
                MaSV = _sinhvien.MaSinhVien, 
            };
            if (!_dangKyController.HardDelete(dangKy.MaNHP, dangKy.MaSV))
            {
                MessageBox.Show("Xoa dang ky that bai", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            _rawDataDangKy.RemoveAll(x => x.MaNHP == index);
        }
        UpdateSoTC();
        UpdateDataDisplayDangKy(_rawDataDangKy);
        _tableDangKy._dataGridView.DataSource = _displayDataDangKy;
    }

    //chỉ xét trùng lịch, không xét tới phòng
    int DuplicateLichHp(NhomHocPhanDto nhp)
    {
        int rs = -1;
        if (_rawDataDangKy.Count == 0)
        {
            return rs;
        }
        List<LichHocDto> listLichThem = _lichHocController.GetByMaNhp(nhp.MaNHP);
        foreach (NhomHocPhanDto item in _rawDataDangKy)
        {
            List<LichHocDto> listLichSan =  _lichHocController.GetByMaNhp(item.MaNHP);
            foreach (LichHocDto lich in listLichSan)
            {
                foreach (LichHocDto d in listLichThem)
                {
                    if (DuplicateLich(d, lich))
                    {
                        rs = item.MaHP;
                        return rs;
                    }
                }
            }
        }

        return rs;
    }

    bool DuplicateLich(LichHocDto lich1, LichHocDto lich2)
    {
        if (lich1.Thu != lich2.Thu)
        {
            return false;
        }
        if (lich1.TietBatDau <= lich2.TietKetThuc && lich2.TietBatDau <= lich1.TietKetThuc)
        {
            return true;
        }

        return false;
    }
    
    
    void UpdateDataDisplayMoDangKy(List<NhomHocPhanDto> dtos)
    {
        this._displayDataMoDangKy = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaNHP = x.MaNHP,
            MaHP = x.MaHP,
            TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            SoTC = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
            Lop = _lopController.GetLopById(x.MaLop).TenLop,
            SoLuong = x.SiSo,
            ConLai = 0,
            TKB = GetTkbByNhp(x),
        });
    }
    
    void UpdateDataDisplayDangKy(List<NhomHocPhanDto> dtos)
    {
        this._displayDataDangKy = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaNHP = x.MaNHP,
            MaHP = x.MaHP,
            TenHP = _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            SoTC = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
            Lop = _lopController.GetLopById(x.MaLop).TenLop,
            SoLuong = x.SiSo,
            ConLai = 0,
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
                _currentListNhp = _nhomHocPhanController.GetByLichMaDangKy(lich.MaLichDK);
                _hocKy = _currentListNhp[0].HocKy;
                _nam = _currentListNhp[0].Nam;
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