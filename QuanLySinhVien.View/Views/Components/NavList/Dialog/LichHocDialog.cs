using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class LichHocDialog : Form
{
    private readonly List<NhomHocPhanDto> _currentListNhp;
    private readonly DialogType _dialogType;

    private readonly List<LichHocDto> _listTam;
    private readonly string _title;

    private readonly LichHocDto selectedLich;

    private string[] tempCa = new[] { "Lý thuyết", "Thực hành", "Lý thuyết + Thực hành" };
    private string[] tempThu = new[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };

    private readonly string[] tempa = new[]
        { "Tiết 1", "Tiết 2", "Tiết 3", "Tiết 4", "Tiết 5", "Tiết 6", "Tiết 7", "Tiết 8", "Tiết 9", "Tiết 10" };
    private readonly string[] tempc = new[] { "Tiết 6", "Tiết 7", "Tiết 8", "Tiết 9", "Tiết 10" };
    private readonly string[] temps = new[] { "Tiết 1", "Tiết 2", "Tiết 3", "Tiết 4", "Tiết 5" };
    
    private TitleButton _btnLuu;

    private MyTLP _contentPanel;
    private CustomButton _exitButton;
    private LichHocController _lichController;

    private List<string> _listAll;
    private List<string> _listChieu;
    private List<string> _listSang;
    private List<LabelTextField> _listTextBox;

    private MyTLP _mainLayout;

    private HocPhanController _hocPhanController;
    private LabelTextField fieldCa;

    private LabelTextField fieldPhong;
    private LabelTextField fieldSoTiet;
    private LabelTextField fieldThu;
    private LabelTextField fieldTietBd;
    private LabelTextField fieldTietKt;
    private LabelTextField fieldTuNgay;
    private LabelTextField fieldDenNgay;

    private PhongHocController _phongController;
    private LichSuDungController _lichSuDungController;
    private CTDTTable _tablePhong;
    private List<PhongHocDto> _rawDataPhong;
    private List<object> _displayDataPhong;

    
    private HocPhanDto _hocPhan;
    private List<LichSuDungDto> _listLichSuDungThem;
    private List<LichSuDungDto> _listLichSuDungForChecking;

    private List<LichHocDto> _listLichHocNhp;

    //list lịch sử dụng thêm là list cache tạm bên ngoài nhóm học phần, để check phòng trống, cần check thêm list này
    public LichHocDialog(string title, DialogType dialogType, HocPhanDto hocPhan, LichHocDto selectedLich, List<LichSuDungDto> listLichSuDungThem, List<LichHocDto> listLichHocNhp)
    {
        _title = title;
        _dialogType = dialogType;
        _phongController = PhongHocController.getInstance();
        this.selectedLich = selectedLich;
        _listTam = listLichHocNhp;
        _lichController = LichHocController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _lichSuDungController = LichSuDungController.GetInstance();
        _hocPhan = hocPhan;
        _listLichSuDungThem = listLichSuDungThem;
        _listLichSuDungForChecking = new List<LichSuDungDto>();
        Init();
    }


    public event Action<LichHocDto, List<LichSuDungDto>> Finish;

    private void Init()
    {
        Width = 1000;
        Height = 900;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        _mainLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
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

        Controls.Add(_mainLayout);

        if (_dialogType == DialogType.ChiTiet)
            SetupDetail();
        else if (_dialogType == DialogType.Sua) SetupUpdate();
    }

    private void SetTopBar()
    {
        var panel = new MyTLP
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0)
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


        var topTitle = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.Left,
            BackColor = MyColor.GrayBackGround,
            Dock = DockStyle.Fill,
            Margin = new Padding(0)
        };
        panel.Controls.Add(topTitle);


        _exitButton = new CustomButton(25, 25, "exitbutton.svg", MyColor.GrayBackGround, false, false, false, false);
        _exitButton.HoverColor = MyColor.GrayHoverColor;
        _exitButton.SelectColor = MyColor.GraySelectColor;
        _exitButton.Margin = new Padding(0);
        _exitButton.Anchor = AnchorStyles.Right;

        _exitButton.MouseDown += (sender, args) => Close();
        panel.Controls.Add(_exitButton);

        _mainLayout.Controls.Add(panel);
    }

    private void SetTitleBar()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.MainColor,
            Margin = new Padding(0),
            Padding = new Padding(0, 10, 0, 10)
        };

        var title = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.None,
            AutoSize = true,
            ForeColor = MyColor.White,
            Font = GetFont.GetFont.GetMainFont(16, FontType.Black)
        };

        panel.Controls.Add(title);
        _mainLayout.Controls.Add(panel);
    }

    private void SetContent()
    {
        _contentPanel = new MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(7),
        };

        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        SetFieldContainer();
        SetSearchRoomContainer();
        
        _mainLayout.Controls.Add(_contentPanel);
    }

    void SetFieldContainer()
    {
        RoundTLP panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            Border = true,
            AutoSize = true,
            RowCount = 8,
            Margin = new Padding(3, 3, 15, 3),
            Padding = new Padding(5),
        };
        fieldCa = new LabelTextField("Ca", TextFieldType.Combobox){Dock = DockStyle.None};
        fieldCa._combobox.combobox.Width = 300;
        fieldCa.SetComboboxList(tempCa.ToList());
        fieldCa.SetComboboxSelection(tempCa[0]);
        
        fieldSoTiet = new LabelTextField("Số tiết", TextFieldType.Number);
        
        fieldThu = new LabelTextField("Thứ", TextFieldType.Combobox);
        fieldThu.SetComboboxList(tempThu.ToList());
        fieldThu.SetComboboxSelection(tempThu[0]);
        
        fieldTietBd = new LabelTextField("Tiết bắt đầu", TextFieldType.Combobox);
        fieldTietBd.SetComboboxList(tempa.ToList());
        fieldTietBd.SetComboboxSelection(tempa[0]);
        
        fieldTietKt = new LabelTextField("Tiết kết thúc", TextFieldType.Combobox);
        fieldTietKt.SetComboboxList(temps.ToList());
        fieldTietKt.SetComboboxSelection(temps[0]);
        
        fieldTuNgay = new LabelTextField("Ngày bắt đầu", TextFieldType.Date);
        fieldDenNgay = new LabelTextField("Ngày kết thúc", TextFieldType.Date);
        fieldDenNgay._dField.Enabled = false;
        
        fieldPhong = new LabelTextField("Phòng", TextFieldType.NormalText);
        fieldPhong._field.Enable = false;
        
        panel.Controls.Add(fieldCa);
        panel.Controls.Add(fieldSoTiet);
        panel.Controls.Add(fieldThu);
        panel.Controls.Add(fieldTietBd);
        panel.Controls.Add(fieldTietKt);
        panel.Controls.Add(fieldTuNgay);
        panel.Controls.Add(fieldDenNgay);
        panel.Controls.Add(fieldPhong);
        
        _contentPanel.Controls.Add(panel);
        SetupField();
    }

    void SetupField()
    {
        fieldSoTiet._numberField.Enable = false;
        fieldSoTiet._numberField.contentTextBox.Text = _hocPhan.SoTietLyThuyet + "";

        SetActionField();
        SetActionNgay();
        
        UpdateDenNgay();
    }
    

    void SetActionField()
    {
        fieldCa._combobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeCa();
        fieldTietBd._combobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeTietBd();
        
        fieldTietBd._combobox.combobox.SelectedIndexChanged +=  (sender, args) => UpdateDenNgay();
        fieldTietKt._combobox.combobox.SelectedIndexChanged +=  (sender, args) => UpdateDenNgay();
    }

    void OnChangeCa()
    {
        string ca = fieldCa.GetSelectionCombobox();
        if (ca.Equals("Lý thuyết"))
        {
            fieldSoTiet._numberField.contentTextBox.Text = _hocPhan.SoTietLyThuyet + "";
        }
        else if (ca.Equals("Thực hành"))
        {
            fieldSoTiet._numberField.contentTextBox.Text = _hocPhan.SoTietThucHanh + "";
        }
        else
        {
            fieldSoTiet._numberField.contentTextBox.Text = (_hocPhan.SoTietThucHanh + _hocPhan.SoTietLyThuyet) + "";
        }
    }

    void OnChangeTietBd()
    {
        int tietBd = int.Parse(fieldTietBd.GetSelectionCombobox().Split(' ')[1]);
        List<string> listkt = new List<string>();
            
        if (tietBd < 6)
        {
            for (int i = tietBd - 1; i < temps.Length; i++)
            {
                listkt.Add(temps[i]);
            }
            
            fieldTietKt.SetComboboxList(listkt);
            fieldTietKt.SetComboboxSelection(listkt[0]);
        }
        else
        {
            for (int i = tietBd - 6; i < tempc.Length; i++)
            {
                listkt.Add(tempc[i]);
            }
            
            fieldTietKt.SetComboboxList(listkt);
            fieldTietKt.SetComboboxSelection(listkt[0]);
        }
        
    }

    private TitleButton _searchRoomBtn;
    private Label lblDsPhongTrong;
    private CtdtSearchBar _phongSearchBar;
    private CustomCombobox _phongCombobox;
    void SetSearchRoomContainer()
    {
        SetTable();
        string[] trangThaiPhong = new string[]
        {
            "Trống",
            "Tất cả",
            "Bận"
        };
        
        _searchRoomBtn = new TitleButton("Tìm phòng");
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 4
        };

        Label lblTitlePhongTrong = new Label(){Text = "Danh sách phòng trống ở:",AutoSize = true,Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold)};
        lblDsPhongTrong = new Label(){Text = "",AutoSize = true, Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold)};
        _phongSearchBar = new CtdtSearchBar();
        _phongCombobox = new CustomCombobox(trangThaiPhong);
        _phongCombobox.SetSelectionCombobox(trangThaiPhong[0]);
        _phongCombobox.combobox.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
        
        panel.Controls.Add(_searchRoomBtn);
        panel.Controls.Add(lblTitlePhongTrong);
        panel.Controls.Add(lblDsPhongTrong);

        RoundTLP tableContainer = new RoundTLP
        {
            Border = true,
            ColumnCount = 2, 
            RowCount = 2,
            Dock = DockStyle.Fill, 
            AutoSize = true,
            Padding = new Padding(10),
        };
        
        tableContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tableContainer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tableContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tableContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        
        tableContainer.Controls.Add(_phongSearchBar);
        tableContainer.Controls.Add(_phongCombobox);
        tableContainer.Controls.Add(_tablePhong);
        tableContainer.SetColumnSpan(_tablePhong, 2);

        panel.Controls.Add(tableContainer);
            
        _contentPanel.Controls.Add(panel);
    }

    void SetTable()
    {
        _rawDataPhong = _phongController.GetDanhSachPhongHoc();
        UpdateDisplayData(new List<PhongHocDto>());
        string[] headerContent = new[] { "Mã PH", "Tên PH", "Cơ sở", "Sức chứa", "Tình trạng", "Thêm" };
        string[] columnNames = new[] {"MaPH","TenPH","CoSo","SucChua","TinhTrang", "ActionPlus"};
        _tablePhong = new CTDTTable(headerContent.ToList(), columnNames.ToList(), _displayDataPhong, TableCTDTType.Plus);
    }

    void UpdateDisplayData(List<PhongHocDto> input)
    {
        _displayDataPhong = ConvertObject.ConvertToDisplay(input, x => new
        {
            x.MaPH,
            x.TenPH,
            x.CoSo,
            x.SucChua,
            x.TinhTrang,
        });
    }
    

    private void SetActionNgay()
    {
        fieldTuNgay.GetDField().TextChanged += (sender, args) => UpdateDenNgay();
        fieldSoTiet._numberField.contentTextBox.TextChanged += (sender, args) => UpdateDenNgay();
    }

    void UpdateDenNgay()
    {
        var startDate = fieldTuNgay.GetDField().Value;
            
        int soTiet = int.Parse(fieldSoTiet._numberField.contentTextBox.Text);
            
        int tbd = int.Parse(fieldTietBd.GetSelectionCombobox().Split(' ')[1]);
        int tkt = int.Parse(fieldTietKt.GetSelectionCombobox().Split(' ')[1]);
        
        int soTietTrenTuan = tkt - tbd + 1;

        int soTuan = (int)Math.Ceiling((double)soTiet / soTietTrenTuan); //lam tron len
        int soNgay = soTuan * 7;

        var endDate = startDate.AddDays(soNgay);
        
        fieldDenNgay._dField.dateField.Value = endDate;
    }

    void UpdateLblPhongTrong()
    {
        // Thứ 2, tiết 1 -> 5, ngày 20/12/2025 đến ngày 20/12/2026
        string thu = fieldThu.GetSelectionCombobox();
        string tietBd = fieldTietBd.GetSelectionCombobox();
        string tietKt = fieldTietKt.GetSelectionCombobox();
        string ngayBd = fieldTuNgay.GetDField().Value.ToString("dd/MM/yyyy");
        string ngayKt = fieldDenNgay.GetDField().Value.ToString("dd/MM/yyyy");

        lblDsPhongTrong.Text = $"{thu}, {tietBd} -> {tietKt}, ngày {ngayBd} đến ngày {ngayKt}";

        int tietBdN = int.Parse(tietBd.Split(' ')[1]);
        int tietKtN = int.Parse(tietKt.Split(' ')[1]);
        DateTime ngayBdT = fieldTuNgay.GetDField().Value;
        DateTime ngayKtT = fieldDenNgay.GetDField().Value;

        //Lấy ds lịch sử dụng của nhóm học phần nếu chọn các thông tin trên
        List<LichSuDungDto> lichSuDungOfNhp = GetListLichSd(
                thu,
                tietBdN,
                tietKtN,
                ngayBdT,
                ngayKtT,
                -1
            );
        UpdateListPhongTrong(lichSuDungOfNhp);
        OnChangeCbx();
    }

    private List<PhongHocDto> listPhongDisplay = new List<PhongHocDto>();
    private void UpdateListPhongTrong(List<LichSuDungDto> listOfNhp)
    {
        listPhongDisplay = new List<PhongHocDto>();
        foreach (PhongHocDto phong in _rawDataPhong)
        {
            if (!ExistLichByPh(phong))
            {
                phong.TinhTrang = "Trống";
                listPhongDisplay.Add(phong);
            }
            else
            {
                if (CheckLichSuDung(phong, listOfNhp))
                {
                    phong.TinhTrang = "Bận";
                }
                else
                {
                    phong.TinhTrang = "Trống";
                }
                listPhongDisplay.Add(phong);
            }
        }
        UpdateDisplayData(listPhongDisplay);
        _tablePhong.UpdateData(_displayDataPhong);
    }
    
    //Kiểm tra xem phòng có lịch nào không
    bool ExistLichByPh(PhongHocDto phong)
    {
        if (_lichSuDungController.ExistByMaPhong(phong.MaPH))
        {
            return true;
        }

        foreach (LichSuDungDto lich in _listLichSuDungThem)
        {
            if (lich.MaPH == phong.MaPH)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckLichSuDung(PhongHocDto phong, List<LichSuDungDto> listOfNhp)
    {
        // Lấy danh sách lịch phòng học hiện có
        SetCheckingList(phong);

        foreach (var lichPhong in _listLichSuDungForChecking)
        {
            foreach (var lichNhp in listOfNhp)
            {
                // Kiểm tra trùng thời gian
                bool isOverlap =
                    lichPhong.ThoiGianBatDau < lichNhp.ThoiGianKetThuc &&
                    lichNhp.ThoiGianBatDau < lichPhong.ThoiGianKetThuc;

                if (isOverlap)
                {
                    // Log chi tiết để debug
                    Console.WriteLine(
                        $"TRÙNG LỊCH: " +
                        $"Phòng {phong.MaPH} | " +
                        $"{lichPhong.ThoiGianBatDau:dd/MM HH:mm}-{lichPhong.ThoiGianKetThuc:HH:mm} " +
                        $"VS Nhóm HP {lichNhp.ThoiGianBatDau:dd/MM HH:mm}-{lichNhp.ThoiGianKetThuc:HH:mm}"
                    );

                    return true; // có trùng -> trả về ngay
                }
            }
        }
        return false; // không trùng
    }

    void SetCheckingList(PhongHocDto phong)
    {
        _listLichSuDungForChecking = new List<LichSuDungDto>();
        //thêm ở controller
        _listLichSuDungForChecking = _lichSuDungController.GetDsLichSdByMaPh(phong.MaPH);
        //thêm ở list cache
        foreach (LichSuDungDto lich in _listLichSuDungThem)
        {
            if (lich.MaPH == phong.MaPH)
            {
                _listLichSuDungForChecking.Add(lich);
            }
        }
        
        Console.WriteLine("-----------listlichcheck-----------");
        int index = 0;
        foreach (LichSuDungDto lich in _listLichSuDungForChecking)
        {
            Console.WriteLine($"{index++} lich phong {lich.MaPH} {lich.ThoiGianBatDau} - {lich.ThoiGianKetThuc}");
        }
    }
    
    public List<LichSuDungDto> GetListLichSd(
        string thu,
        int tietBatDau,
        int tietKetThuc,
        DateTime ngayBatDau,
        DateTime ngayKetThuc,
        int maPhong)
    {
        List<LichSuDungDto> list = new List<LichSuDungDto>();

        DayOfWeek targetDay = ConvertThu.ConvertToDayOfWeek(thu);

        // Tìm ngày đầu tiên trùng với thứ
        DateTime current = ngayBatDau;
        while (current.DayOfWeek != targetDay)
            current = current.AddDays(1);

        // Mỗi tiết sẽ add riêng 1 lịch luôn
        while (current <= ngayKetThuc)
        {
            for (int tiet = tietBatDau; tiet <= tietKetThuc; tiet++)
            {
                var tg = ConvertTietToGio.GetThoiGianTiet(tiet, current);

                list.Add(new LichSuDungDto
                {
                    MaPH = maPhong,
                    ThoiGianBatDau = tg.Start,
                    ThoiGianKetThuc = tg.End,
                });
            }

            current = current.AddDays(7);
        }

        return list;
    }



    private void SetupDetail()
    {
        fieldCa.SetComboboxSelection(selectedLich.Type);
        fieldThu.SetComboboxSelection(selectedLich.Thu);
        fieldTietBd.SetComboboxSelection("Tiết " + selectedLich.TietBatDau);
        fieldTietKt.SetComboboxSelection("Tiết " + selectedLich.TietKetThuc);

        fieldTuNgay.GetDField().Value = selectedLich.TuNgay;

        // if (selectedLich.Type.Equals("Thực hành"))
        // {
        //     fieldSoTiet._numberField.contentTextBox.Text = _hocPhan.SoTietThucHanh + "";
        // }
        // else
        // {
        //     fieldSoTiet._numberField.contentTextBox.Text = _hocPhan.SoTietLyThuyet + "";
        // }
        fieldPhong._field.contentTextBox.Text = _phongController.GetPhongHocById(selectedLich.MaPH).TenPH;

        fieldPhong._field.Enable = false;
        fieldCa._combobox.Enable = false;
        fieldThu._combobox.Enable = false;
        fieldTietBd._combobox.Enable = false;
        fieldTietKt._combobox.Enable = false;
        fieldTuNgay._dField.Enabled = false;
        fieldDenNgay._dField.Enabled = false;
        fieldSoTiet._numberField.Enable = false;
        
        _searchRoomBtn.Enabled = false;
        _phongSearchBar.Enabled = false;
        _phongCombobox.Enabled = false;
    }

    private void SetupUpdate()
    {
        fieldCa.SetComboboxSelection(selectedLich.Type);
        fieldThu.SetComboboxSelection(selectedLich.Thu);
        fieldTietBd.SetComboboxSelection("Tiết " + selectedLich.TietBatDau);
        fieldTietKt.SetComboboxSelection("Tiết " + selectedLich.TietKetThuc);

        fieldTuNgay.GetDField().Value = selectedLich.TuNgay;

        // if (selectedLich.Type.Equals("Thực hành"))
        // {
        //     fieldSoTiet._numberField.contentTextBox.Text = _hocPhan.SoTietThucHanh + "";
        // }
        // else
        // {
        //     fieldSoTiet._numberField.contentTextBox.Text = _hocPhan.SoTietLyThuyet + "";
        // }
        fieldPhong._field.contentTextBox.Text = _phongController.GetPhongHocById(selectedLich.MaPH).TenPH;
    }

    private void SetAction()
    {
        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
            _btnLuu._mouseDown += () => { Luu(); };

        _searchRoomBtn._mouseDown += () => UpdateLblPhongTrong();

        _phongSearchBar.KeyDown += s => SearchPhong(s);
        _phongCombobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeCbx();

        fieldThu._combobox.combobox.SelectedIndexChanged += (sender, args) => UnEnableTablePhong();
        fieldTietBd._combobox.combobox.SelectedIndexChanged += (sender, args) => UnEnableTablePhong();
        fieldTietKt._combobox.combobox.SelectedIndexChanged += (sender, args) => UnEnableTablePhong();
        fieldTuNgay._dField.dateField.TextChanged += (sender, args) => UnEnableTablePhong();
        fieldDenNgay._dField.dateField.TextChanged += (sender, args) => UnEnableTablePhong();
        _tablePhong.BtnClick += i => AddPhong(i);
    }

    void AddPhong(int maPhong)
    {
        PhongHocDto phong = _phongController.GetPhongHocById(maPhong);
        if (GetStatusPhongInListPhong(phong.MaPH).Equals("Trống"))
        {
            fieldPhong._field.contentTextBox.Text = phong.TenPH;
        }
        else
        {
            MessageBox.Show("Phòng học không khả dụng", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    string GetStatusPhongInListPhong(int maPhong)
    {
        foreach (PhongHocDto phong in listPhongDisplay)
        {
            if (phong.MaPH == maPhong) return phong.TinhTrang;
        }

        return "Bận";
    }
    
    void UnEnableTablePhong()
    {
        listPhongDisplay = new List<PhongHocDto>();
        UpdateDisplayData(listPhongDisplay);
        _tablePhong.UpdateData(_displayDataPhong);
        lblDsPhongTrong.Text = "";
        fieldPhong._field.contentTextBox.Text = "";
    }

    void OnChangeCbx()
    {
        string status = _phongCombobox.combobox.Text;
        List<PhongHocDto> result;
        if (status.Equals("Tất cả"))
        {
            result = listPhongDisplay;
        }
        else
        {
            result = listPhongDisplay
                .Where(x => x.TinhTrang.Equals(status)).ToList();
        }

        UpdateDisplayData(result);
        _tablePhong.UpdateData(_displayDataPhong);
    }
    private void SearchPhong(string keyword)
    {
        var result = listPhongDisplay
            .Where(x => x.MaPH.ToString().ToLower().Contains(keyword) ||
                        x.TenPH.ToString().ToLower().Contains(keyword) ||
                        x.TinhTrang.ToString().ToLower().Contains(keyword) ||
                        x.LoaiPH.ToString().ToLower().Contains(keyword) ||
                        x.SucChua.ToString().ToLower().Contains(keyword) ||
                        x.CoSo.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        UpdateDisplayData(result);
        _tablePhong.UpdateData(_displayDataPhong);
    }

    private void Luu()
    {
        if (_dialogType == DialogType.Them)
            Insert();
        else if (_dialogType == DialogType.Sua) Update();
    }

    private void Insert()
    {
        string tenPhong = fieldPhong._field.contentTextBox.Text;

        if (!Validate(tenPhong)) return;

        PhongHocDto phong = _phongController.GetByTen(tenPhong);

        var tietBd = int.Parse(fieldTietBd.GetSelectionCombobox().Split(" ")[1]);
        var tietKt = int.Parse(fieldTietKt.GetSelectionCombobox().Split(" ")[1]);
        if (DuplicateLichHoc(fieldThu.GetSelectionCombobox(), tietBd + "", tietKt + ""))
        {
            MessageBox.Show("Lịch học trùng", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var lich = new LichHocDto
        {
            MaPH = phong.MaPH,
            Thu = fieldThu.GetSelectionCombobox(),
            TietBatDau = tietBd,
            TietKetThuc = tietKt,
            TuNgay = fieldTuNgay.GetDField().Value,
            DenNgay = fieldDenNgay.GetDField().Value,
            SoTiet = tietKt - tietBd,
            Type = fieldCa.GetSelectionCombobox()
        };
        
        List<LichSuDungDto> lichSuDungOfNhp = GetListLichSd(
            lich.Thu,
            lich.TietBatDau,
            lich.TietKetThuc,
            lich.TuNgay,
            lich.DenNgay,
            phong.MaPH
        );

        Finish?.Invoke(lich, lichSuDungOfNhp);
        Close();
    }

    private void Update()
    {
        var tenPhong = fieldPhong._field.contentTextBox.Text;
    
        if (!Validate(tenPhong)) return;
    
        PhongHocDto phong = _phongController.GetByTen(tenPhong);
    
        var tietBd = int.Parse(fieldTietBd.GetSelectionCombobox().Split(" ")[1]);
        var tietKt = int.Parse(fieldTietKt.GetSelectionCombobox().Split(" ")[1]);
    
        var lich = new LichHocDto
        {
            MaLH = selectedLich.MaLH,
            MaPH = phong.MaPH,
            Thu = fieldThu.GetSelectionCombobox(),
            TietBatDau = tietBd,
            TietKetThuc = tietKt,
            TuNgay = fieldTuNgay.GetDField().Value,
            DenNgay = fieldDenNgay.GetDField().Value,
            SoTiet = tietKt - tietBd,
            Type = fieldCa.GetSelectionCombobox()
        };
        
        List<LichSuDungDto> lichSuDungOfNhp = GetListLichSd(
            lich.Thu,
            lich.TietBatDau,
            lich.TietKetThuc,
            lich.TuNgay,
            lich.DenNgay,
            phong.MaPH
        );
    
        Finish?.Invoke(lich, lichSuDungOfNhp);
        Close();
    }

    private int GetTietInt(string tiet)
    {
        var temp = tiet.Split(' ');
        return int.Parse(temp[1]);
    }

    private void SetBottom()
    {
        //Thêm có Đặt lại, Lưu, Hủy
        var panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            panel.Controls.Add(new Panel { Height = 0 });

            _btnLuu = new TitleButton("Lưu");
            panel.Controls.Add(_btnLuu);

            var btnHuy = new TitleButton("Hủy");
            btnHuy._mouseDown += () => Close();

            panel.Controls.Add(btnHuy);
        }
        else
        {
            panel.Controls.Add(new Panel { Height = 0 });

            var btnThoat = new TitleButton("Thoát");
            btnThoat._mouseDown += () => Close();
            panel.Controls.Add(btnThoat, 2, 0);
        }

        _mainLayout.Controls.Add(panel, 0, 3);
    }

    private bool Validate(string tenPhong)
    {
        if (tenPhong.Equals(""))
        {
            MessageBox.Show("Phòng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            fieldPhong._field.contentTextBox.Focus();
            return false;
        }

        if (!_phongController.ExistByTen(tenPhong))
        {
            MessageBox.Show("Phòng không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            fieldPhong._field.contentTextBox.Focus();
            return false;
        }
        //
        // if (DuplicateLichHoc(fieldPhong._field.contentTextBox.Text, fieldThu.GetSelectionCombobox(),
        //         fieldTietBd.GetSelectionCombobox(), fieldTietKt.GetSelectionCombobox()))
        // {
        //     MessageBox.Show("Lịch học bị trùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //     return false;
        // }

        // var maNhpTrung = DuplicateLichHocNhp(fieldPhong._field.contentTextBox.Text, fieldThu.GetSelectionCombobox(),
        //     fieldTietBd.GetSelectionCombobox(), fieldTietKt.GetSelectionCombobox());
        // if (maNhpTrung != -1)
        // {
        //     MessageBox.Show("Lịch học bị trùng với lịch của nhóm học phần: " + maNhpTrung + " !", "Lỗi",
        //         MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //     return false;
        // }

        return true;
    }

    private bool DuplicateLichHoc(string thu, string tietBatDau, string tietKetThuc)
    {
        // int maPhong = _phongController.GetByTen(tenPhong).MaPH;
        var tietBd = int.Parse(tietBatDau);
        var tietKt = int.Parse(tietKetThuc);
        //trùng ở nhóm học phần hiện tại
        foreach (var item in _listTam)
            if (item.Thu.Equals(thu))
                if (item.TietBatDau <= tietKt && tietBd <= item.TietKetThuc)
                    return true;

        return false;
    }

    // private int DuplicateLichHocNhp(string tenPhong, string thu, string tietBatDau, string tietKetThuc)
    // {
    //     var rs = -1;
    //     int maPhong = _phongController.GetByTen(tenPhong).MaPH;
    //     var tietBd = int.Parse(tietBatDau.Split(" ")[1]);
    //     var tietKt = int.Parse(tietKetThuc.Split(" ")[1]);
    //
    //     //trùng ở các nhóm học phần khác cùng đợt đăng ký
    //     foreach (var item in _currentListNhp)
    //     {
    //         List<LichHocDto> listLichOfNhp = _lichController.GetByMaNhp(item.MaNHP);
    //
    //         foreach (var lich in listLichOfNhp)
    //             //(a1, a2) //(b1, b2) //dk: a1 < b2 && b1 < a2
    //             if (maPhong == lich.MaPH && thu.Equals(lich.Thu))
    //                 if (lich.TietBatDau <= tietKt && tietBd <= lich.TietKetThuc)
    //                 {
    //                     Console.WriteLine("lich cua nhp : " + lich.MaPH + " " + lich.Thu + " " + lich.TietBatDau + " " +
    //                                       lich.TietKetThuc);
    //                     Console.WriteLine("lich chen : " + maPhong + " " + thu + " " + tietBd + " " + tietKt);
    //                     rs = item.MaNHP;
    //                     return rs;
    //                 }
    //     }
    //
    //     return rs;
    // }
}