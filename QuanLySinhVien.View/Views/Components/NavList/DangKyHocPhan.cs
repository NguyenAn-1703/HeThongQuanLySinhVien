using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList;

public class DangKyHocPhan : NavBase
{
    private readonly string[] _headerArrayDK = new[] { "Nhóm", "Mã học phần", "Tên học phần", "Số TC", "Lớp" };

    private readonly string[] _headerArrayMDK = new[]
        { "Nhóm", "Mã học phần", "Tên học phần", "Số TC", "Lớp", "Số lượng", "Còn lại", "TKB" };

    private readonly List<string> _headerListDK;
    private readonly List<string> _headerListMDK;
    private readonly string[] _listSelectionForComboBox = new[] { "" };

    private readonly SinhVienDTO _sinhvien;
    private readonly string _title = "Đăng ký học phần";

    private MyTLP _contentLayout;
    private LichDangKyDto _currentLichDangKy;
    private List<NhomHocPhanDto> _currentListNhp;
    private DangKyController _dangKyController;
    private List<object> _displayDataDangKy;
    private List<object> _displayDataMoDangKy;

    private TitleButton _exportBtn;
    private LabelTextField _fieldHPMDK;
    private LabelTextField _fieldLop;

    private LabelTextField _fieldNganh;
    //////////////////////////////SETTOP///////////////////////////////

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    private MyTLP _filterContainer;

    private GiangVienController _giangVienController;


    private int _hocKy;
    private HocPhanController _hocPhanController;
    private HocPhiHocPhanController _hocPhiHocPhanController;
    private HocPhiTinChiController _hocPhiTinChiController;
    private KetQuaController _ketQuaController;
    private LichDangKyController _lichDangKyController;
    private LichHocController _lichHocController;
    private LopController _lopController;

    private MyTLP _mainLayout;
    private string _nam;
    private NganhController _nganhController;

    private NhomHocPhanController _nhomHocPhanController;
    private RoundTLP _panelHpDangKy;


    private RoundTLP _panelHpMoDangKy;
    private PhongHocController _phongHocController;

    private List<NhomHocPhanDto> _rawDataDangKy;
    private List<NhomHocPhanDto> _rawDataFilter;

    private List<NhomHocPhanDto> _rawDataMoDangKy;
    private SinhVienController _sinhVienController;


    private int _soMon;
    private int _soTC;
    private Label _soTCsoHP;

    private CustomTable _tableDangKy;

    private CustomTable _tableMoDangKy;

    private double hocPhiTrenTinChi;
    private string ID = "DANGKYHOCPHAN";

    public DangKyHocPhan(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _headerListMDK = _headerArrayMDK.ToList();
        _headerListDK = _headerArrayDK.ToList();
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
        _lichHocController = LichHocController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _dangKyController = DangKyController.GetInstance();
        _sinhVienController = SinhVienController.GetInstance();
        _ketQuaController = KetQuaController.GetInstance();
        _hocPhiHocPhanController = HocPhiHocPhanController.GetInstance();
        _hocPhiTinChiController = HocPhiTinChiController.GetInstance();
        _taiKhoan = taiKhoan;
        _sinhvien = _sinhVienController.GetByMaTK(taiKhoan.MaTK);
        Init();
    }

    private void Init()
    {
        Dock = DockStyle.Fill;

        _mainLayout = new MyTLP
        {
            RowCount = 3,
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 0, 0, 50)
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetLich();
        SetTop();
        SetBottom();
        SetupHocPhi();
        // SetBtnExport();


        Controls.Add(_mainLayout);
    }

    private void SetTop()
    {
        var panel = new MyTLP
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

    private void SetBottom()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3
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

    private Label getLblHkNam()
    {
        var hknam = "học kỳ: " + _hocKy + " năm học: " + _nam;

        var hknamlbl = new Label
        {
            Text = hknam,
            Font = GetFont.GetFont.GetMainFont(13, FontType.Bold),
            AutoSize = true,
            Anchor = AnchorStyles.None
        };
        return hknamlbl;
    }

    private Label getTitle()
    {
        var titlePnl = new Label
        {
            Text = _title,
            Font = GetFont.GetFont.GetMainFont(17, FontType.ExtraBold),
            AutoSize = true
        };
        return titlePnl;
    }

    private void SetFilterContainer()
    {
        _filterContainer = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.GrayBackGround,
            ColumnCount = 3
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

    private void SetHpMoDangKyContainer()
    {
        _panelHpMoDangKy = new RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 2,
            Border = true,
            Padding = new Padding(10)
        };
        _panelHpMoDangKy.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _panelHpMoDangKy.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _panelHpMoDangKy.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        _panelHpMoDangKy.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        _panelHpMoDangKy.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

        _fieldHPMDK = new LabelTextField("Học phần", TextFieldType.ListBoxHP);

        var boxHeader = getHeaderBox("Học phần mở đăng ký");
        _panelHpMoDangKy.Controls.Add(boxHeader);
        _panelHpMoDangKy.SetColumnSpan(boxHeader, 2);

        _panelHpMoDangKy.Controls.Add(_fieldHPMDK);
        _panelHpMoDangKy.Controls.Add(_tableMoDangKy, 0, 2);
        _panelHpMoDangKy.SetColumnSpan(_tableMoDangKy, 2);


        _contentLayout.Controls.Add(_panelHpMoDangKy);
    }

    private void SetHpDangKyContainer()
    {
        _panelHpDangKy = new RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 2,
            Border = true,
            Padding = new Padding(10)
        };

        var boxHeader = getHeaderBox("Học phần đã đăng ký");
        _panelHpDangKy.Controls.Add(boxHeader);

        SetTinChiContainer();
        _panelHpDangKy.Controls.Add(_tableDangKy);
        _panelHpDangKy.SetColumnSpan(_tableDangKy, 2);

        _contentLayout.Controls.Add(_panelHpDangKy);
    }

    private void SetTinChiContainer()
    {
        _soMon = 0;
        _soTC = 0;
        var txt = "Số môn: " + _soMon + " , Số tín chỉ: " + _soTC;
        _soTCsoHP = new Label
        {
            Text = txt,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            AutoSize = true,
            ForeColor = MyColor.Red,
            Margin = new Padding(15, 12, 3, 3),
            Dock = DockStyle.Left
        };
        _panelHpDangKy.Controls.Add(_soTCsoHP);
    }

    private void UpdateSoTC()
    {
        _soMon = 0;
        _soTC = 0;
        if (_rawDataDangKy.Count != 0)
            foreach (var nhp in _rawDataDangKy)
            {
                _soMon += 1;
                _soTC += _hocPhanController.GetHocPhanById(nhp.MaHP).SoTinChi;
            }

        var txt = "Số môn: " + _soMon + " , Số tín chỉ: " + _soTC;
        _soTCsoHP.Text = txt;
    }

    private RoundTLP getHeaderBox(string content)
    {
        var panel = new RoundTLP
        {
            // Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.GrayBackGround
        };
        var titlePnl = new Label
        {
            Text = content,
            Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold),
            AutoSize = true,
            Dock = DockStyle.Left
        };
        panel.Controls.Add(titlePnl);
        return panel;
    }

    private void SetBtnExport()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true
        };

        _exportBtn = new TitleButton("Xuất phiếu", "exportpdf.svg")
        {
            Anchor = AnchorStyles.Right
            // Dock = DockStyle.Right,
        };

        panel.Controls.Add(_exportBtn);
        _mainLayout.Controls.Add(panel);
    }

    private void SetDataTableMoDangKy()
    {
        _rawDataMoDangKy = _currentListNhp;
        SetDisplayDataMoDangKy();

        var columnNames = new[] { "MaNHP", "MaHP", "TenHP", "SoTC", "Lop", "SoLuong", "ConLai", "TKB" };
        var columnNamesList = columnNames.ToList();

        _tableMoDangKy = new CustomTable(_headerListMDK, columnNamesList, _displayDataMoDangKy);

        SetCheckBoxTblMoDK();
        SetUpTableMoDangKy();
    }

    private void SetCheckBoxTblMoDK()
    {
        _tableMoDangKy.AddColumn(ColumnType.CheckBox, "");
    }

    private void SetUpTableMoDangKy()
    {
        _tableMoDangKy.ConfigDKHP();
    }

    private void SetDataTableDangKy()
    {
        _rawDataDangKy = new List<NhomHocPhanDto>();
        SetDisplayDataDangKy();

        var columnNames = new[] { "MaNHP", "MaHP", "TenHP", "SoTC", "Lop", "SoLuong", "ConLai", "TKB" };
        var columnNamesList = columnNames.ToList();

        _tableDangKy = new CustomTable(_headerListDK, columnNamesList, _displayDataDangKy);
    }

    private void SetDisplayDataMoDangKy()
    {
        _displayDataMoDangKy = ConvertObject.ConvertToDisplay(_rawDataMoDangKy, x => new
            {
                x.MaNHP,
                x.MaHP,
                _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                SoTC = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
                Lop = _lopController.GetLopById(x.MaLop).TenLop,
                SoLuong = x.SiSo,
                ConLai = 0,
                TKB = GetTkbByNhp(x)
            }
        );

        _rawDataFilter = _rawDataMoDangKy;
    }


    private string GetTkbByNhp(NhomHocPhanDto nhp)
    {
        var rs = "";
        List<LichHocDto> lich = _lichHocController.GetByMaNhp(nhp.MaNHP);
        GiangVienDto GV = _giangVienController.GetById(nhp.MaGV);

        foreach (var d in lich)
        {
            PhongHocDto phong = _phongHocController.GetPhongHocById(d.MaPH);

            rs += d.Thu + " , tiết " + d.TietBatDau +
                  " đến " + d.TietKetThuc + " , PH " + phong.TenPH +
                  " , GV " + GV.TenGV + " ";
        }

        return rs;
    }

    private void SetDisplayDataDangKy()
    {
        _displayDataDangKy = ConvertObject.ConvertToDisplay(_rawDataDangKy, x => new
            {
                x.MaNHP,
                x.MaHP,
                _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
                SoTC = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
                Lop = _lopController.GetLopById(x.MaLop).TenLop,
                SoLuong = x.SiSo,
                ConLai = 0
            }
        );
    }

    private void SetStatusCB()
    {
        List<DangKyDto> listDk = _dangKyController.GetByMaSV(_sinhvien.MaSinhVien);
        var listDkHocKy = new List<DangKyDto>();

        foreach (var nhp in _currentListNhp)
            if (listDk.Any(x => x.MaNHP == nhp.MaNHP)) // Kiểm tra có nhp nào nằm trong current list không
            {
                var dk = new DangKyDto
                {
                    MaNHP = nhp.MaNHP,
                    MaSV = _sinhvien.MaSinhVien
                };
                listDkHocKy.Add(dk);
            }

        foreach (var dk in listDkHocKy) Console.WriteLine("sv " + _sinhvien.MaSinhVien + " nhoms: " + dk.MaNHP);

        if (listDkHocKy.Count == 0) return;
        foreach (var d in listDkHocKy)
        {
            NhomHocPhanDto nhp = _nhomHocPhanController.GetById(d.MaNHP);
            _tableMoDangKy.tickCB(nhp.MaNHP);

            _rawDataDangKy.Add(nhp);
            UpdateDataDisplayDangKy(_rawDataDangKy);
            _tableDangKy._dataGridView.DataSource = _displayDataDangKy;
        }
    }

    private void SetAction()
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

    private void FilterNganh()
    {
        var tenNganh = _fieldNganh.tbNganh.contentTextBox.Text;
        _rawDataFilter = _rawDataMoDangKy.Where(x =>
            _nganhController.GetNganhById(_lopController.GetLopById(x.MaLop).MaNganh).TenNganh.Equals(tenNganh)
        ).ToList();
        UpdateDataDisplayMoDangKy(_rawDataFilter);
        _tableMoDangKy._dataGridView.DataSource = _displayDataMoDangKy;
    }

    private void FilterLop()
    {
        var tenLop = _fieldLop.tbLop.contentTextBox.Text;
        _rawDataFilter = _rawDataMoDangKy.Where(x =>
            _lopController.GetLopById(x.MaLop).TenLop.Equals(tenLop)).ToList();
        UpdateDataDisplayMoDangKy(_rawDataFilter);
        _tableMoDangKy._dataGridView.DataSource = _displayDataMoDangKy;
    }

    private void FilterHp()
    {
        var tenHp = _fieldHPMDK.tb.contentTextBox.Text;
        _rawDataFilter = _rawDataMoDangKy.Where(x =>
            _hocPhanController.GetHocPhanById(x.MaHP).TenHP.Equals(tenHp)).ToList();
        UpdateDataDisplayMoDangKy(_rawDataFilter);
        _tableMoDangKy._dataGridView.DataSource = _displayDataMoDangKy;
    }

    private void SetupHocPhi()
    {
        LopDto lop = _lopController.GetLopById(_sinhvien.MaLop);
        NganhDto nganh = _nganhController.GetNganhById(lop.MaNganh);
        HocPhiTinChiDto hocPhi = _hocPhiTinChiController.GetNewestHocPhiTinChiByMaNganh(nganh.MaNganh);
        hocPhiTrenTinChi = hocPhi.SoTienMotTinChi;
    }

    private void OnClickCB(int index, bool b)
    {
        var nhp = _rawDataMoDangKy.First(x => x.MaNHP == index);
        if (b)
        {
            var maHpTrung = DuplicateLichHp(nhp);
            if (maHpTrung != -1)
            {
                MessageBox.Show("Lich hoc bị trùng với học phần có mã: " + maHpTrung + " !", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _tableMoDangKy.FailCb(index);
                return;
            }

            var dangKy = new DangKyDto
            {
                MaNHP = nhp.MaNHP,
                MaSV = _sinhvien.MaSinhVien
            };
            if (!_dangKyController.Insert(dangKy))
            {
                MessageBox.Show("Them dang ky that bai: " + maHpTrung + " !", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _rawDataDangKy.Add(nhp);

            //thêm luôn bảng kết quả, học phí học phần
            var ketQua = new KetQuaDto
            {
                MaHP = nhp.MaHP,
                MaSV = _sinhvien.MaSinhVien,
                HocKy = _hocKy,
                Nam = _nam
            };
            if (!_ketQuaController.Insert(ketQua))
            {
                MessageBox.Show("Them ket qua that bai: !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var hocPhiHocPhan = new HocPhiHocPhanDto
            {
                MaHP = nhp.MaHP,
                MaSV = _sinhvien.MaSinhVien,
                HocKy = _hocKy,
                Nam = _nam,
                TongTien = TinhTongTien(nhp)
            };
            if (!_hocPhiHocPhanController.Insert(hocPhiHocPhan))
            {
                MessageBox.Show("Them hoc phi hoc phan that bai: !", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
        }
        else
        {
            var dangKy = new DangKyDto
            {
                MaNHP = nhp.MaNHP,
                MaSV = _sinhvien.MaSinhVien
            };
            if (!_dangKyController.HardDelete(dangKy.MaNHP, dangKy.MaSV))
            {
                MessageBox.Show("Xoa dang ky that bai", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _rawDataDangKy.RemoveAll(x => x.MaNHP == index);


            //xóa bảng kết quả, học phí học phần
            KetQuaDto ketQua = _ketQuaController.GetByMaSVMaHP(_sinhvien.MaSinhVien, nhp.MaHP);
            if (!_ketQuaController.Delete(ketQua.MaKQ))
            {
                MessageBox.Show("Xoa ket qua that bai: !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_hocPhiHocPhanController.DeleteByMaSVMaHP(_sinhvien.MaSinhVien, nhp.MaHP))
            {
                MessageBox.Show("xoa hoc phi hoc phan that bai: !", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
        }

        UpdateSoTC();
        UpdateDataDisplayDangKy(_rawDataDangKy);
        _tableDangKy._dataGridView.DataSource = _displayDataDangKy;
    }

    private double TinhTongTien(NhomHocPhanDto nhomHp)
    {
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(nhomHp.MaHP);
        var tongTien = hocPhiTrenTinChi * hocPhan.SoTinChi;
        return tongTien;
    }


    //chỉ xét trùng lịch, không xét tới phòng
    private int DuplicateLichHp(NhomHocPhanDto nhp)
    {
        var rs = -1;
        if (_rawDataDangKy.Count == 0) return rs;
        List<LichHocDto> listLichThem = _lichHocController.GetByMaNhp(nhp.MaNHP);
        foreach (var item in _rawDataDangKy)
        {
            List<LichHocDto> listLichSan = _lichHocController.GetByMaNhp(item.MaNHP);
            foreach (var lich in listLichSan)
            foreach (var d in listLichThem)
                if (DuplicateLich(d, lich))
                {
                    rs = item.MaHP;
                    return rs;
                }
        }

        return rs;
    }

    private bool DuplicateLich(LichHocDto lich1, LichHocDto lich2)
    {
        if (lich1.Thu != lich2.Thu) return false;
        if (lich1.TietBatDau <= lich2.TietKetThuc && lich2.TietBatDau <= lich1.TietKetThuc) return true;

        return false;
    }


    private void UpdateDataDisplayMoDangKy(List<NhomHocPhanDto> dtos)
    {
        _displayDataMoDangKy = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaNHP,
            x.MaHP,
            _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            SoTC = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
            Lop = _lopController.GetLopById(x.MaLop).TenLop,
            SoLuong = x.SiSo,
            ConLai = 0,
            TKB = GetTkbByNhp(x)
        });
    }

    private void UpdateDataDisplayDangKy(List<NhomHocPhanDto> dtos)
    {
        _displayDataDangKy = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaNHP,
            x.MaHP,
            _hocPhanController.GetHocPhanById(x.MaHP).TenHP,
            SoTC = _hocPhanController.GetHocPhanById(x.MaHP).SoTinChi,
            Lop = _lopController.GetLopById(x.MaLop).TenLop,
            SoLuong = x.SiSo,
            ConLai = 0
        });
    }

    private void Detail(int index)
    {
    }

    private void SetLich()
    {
        List<LichDangKyDto> listLich = _lichDangKyController.GetAll();
        var now = DateTime.Now;
        foreach (var lich in listLich)
            if (now >= lich.ThoiGianBatDau && now <= lich.ThoiGianKetThuc)
            {
                _currentLichDangKy = lich;
                _currentListNhp = _nhomHocPhanController.GetByLichMaDangKy(lich.MaLichDK);
                _hocKy = _currentListNhp[0].HocKy;
                _nam = _currentListNhp[0].Nam;
                Console.WriteLine(_hocKy + "  " + _nam);
            }
    }

    /// ///////////////////////////SETBOTTOM////////////////////////////////////
    public override List<string> getComboboxList()
    {
        return _listSelectionForComboBox.ToList();
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}