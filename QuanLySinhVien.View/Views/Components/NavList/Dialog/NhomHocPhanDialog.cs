using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class NhomHocPhanDialog : Form
{
    private readonly DialogType _dialogType;

    private readonly string _hocKy;
    private readonly string _nam;

    private readonly int _selectedId;
    private readonly string _title;

    private readonly List<LabelTextField> listField;

    private readonly List<LichHocDto> listLichHocTam;
    private List<LichSuDungDto> listLichSuDungTam;
    protected TitleButton _btnLuu;

    private MyTLP _contentPanel;

    private CustomButton _exitButton;
    private GiangVienController _giangVienController;
    private HocPhanController _hocPhanController;
    private TitleButton _insertButton;
    private RoundTLP _lichContainer;

    private LichHocDialog _lichDialog;

    private List<object> _lichDisplay;

    private LichHocController _lichHocController;
    private LopController _lopController;
    private MyTLP _mainLayout;
    private NhomHocPhanController _nhomHocPhanController;
    private PhongHocController _phongHocController;
    private KhoaController _khoaController;
    private DotDangKyController _dotDangKyController;
    private CustomTable _tableLichHoc;
    private LichSD_NhomHPController _lichSD_NhomHPController;

    private LichSuDungController _lichSuDungController;
    private int _maDotDK;


    public NhomHocPhanDialog(string title, DialogType dialogType, string hocKy, string nam, int maDotDK, int index = -1)
    {
        _lichDisplay = new List<object>();
        listField = new List<LabelTextField>();
        listLichHocTam = new List<LichHocDto>();
        listLichSuDungTam = new List<LichSuDungDto>();
        _lichHocController = LichHocController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        _lopController = LopController.GetInstance();
        _lichSuDungController = LichSuDungController.GetInstance();
        _dotDangKyController = DotDangKyController.GetInstance();
        _lichSD_NhomHPController = LichSD_NhomHPController.GetInstance();
        _title = title;
        _dialogType = dialogType;
        _hocKy = hocKy;
        _nam = nam;
        _selectedId = index;
        _maDotDK = maDotDK;
        Init();
    }

    public event Action Finish;

    private void Init()
    {
        Size = new Size(900, 900);

        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new MyTLP
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

        Controls.Add(_mainLayout);

        if (_dialogType == DialogType.Sua)
            SetupUpdate();
        else if (_dialogType == DialogType.ChiTiet) SetupDetail();
        else if (_dialogType == DialogType.Them) SetupInsert();
    }


    private void SetContent()
    {
        _contentPanel = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 5,
            Padding = new Padding(5),
        };

        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        SetHkyNamContainer();
        SetFieldKhoa();
        SetHocPhanContainer();
        SetFieldContainer();
        SetLichHocContainer();

        _mainLayout.Controls.Add(_contentPanel);
    }

    void SetHkyNamContainer()
    {
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            AutoSize = true,
            Margin = new Padding(3, 3, 3, 15)
        };
        LabelValue lblnam = new LabelValue("Năm:", "2025");
        LabelValue lblhky = new LabelValue("Học kỳ:", "1");
        lblnam.lblTitle.Font = GetFont.GetFont.GetMainFont(11, FontType.SemiBold);
        lblnam.lblValue.Font = GetFont.GetFont.GetMainFont(11, FontType.SemiBold);
        lblhky.lblTitle.Font = GetFont.GetFont.GetMainFont(11, FontType.SemiBold);
        lblhky.lblValue.Font = GetFont.GetFont.GetMainFont(11, FontType.SemiBold);
        lblhky.Margin = new Padding(15, 3, 3, 3);
        panel.Controls.Add(lblnam);
        panel.Controls.Add(lblhky);
        _contentPanel.Controls.Add(panel);
    }

    private LabelTextField fieldKhoa;

    void SetFieldKhoa()
    {
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Padding = new Padding(5)
        };

        List<KhoaDto> listKhoa = _khoaController.GetDanhSachKhoa();
        listKhoa.RemoveAt(0);
        List<string> listCbxKhoa = listKhoa.Select(x => x.TenKhoa).ToList();
        fieldKhoa = new LabelTextField("Khoa", TextFieldType.Combobox) { Dock = DockStyle.None };
        fieldKhoa._combobox.combobox.Width = 410;
        fieldKhoa.SetComboboxList(listCbxKhoa);
        fieldKhoa.SetComboboxSelection(listCbxKhoa[0]);
        _contentPanel.Controls.Add(fieldKhoa);
    }

    private LabelTextField _hocPhanField;
    private LabelValue lblTinChi;
    private LabelValue lblHsHp;
    private LabelValue lblLT;
    private LabelValue lblTH;

    private void SetHocPhanContainer()
    {
        _hocPhanField = new LabelTextField("Học phần", TextFieldType.ListBoxHP) { Dock = DockStyle.None };
        _hocPhanField.tb.contentTextBox.Width = 400;

        lblTinChi = new LabelValue("Số tín chỉ:", "0");
        lblHsHp = new LabelValue("Hệ số học phần:", "0");
        lblLT = new LabelValue("Số tiết LT:", "0");
        lblTH = new LabelValue("Số tiết TH:", "0");


        RoundTLP panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Border = true,
            ColumnCount = 2,
            RowCount = 3,
            Padding = new Padding(5)
        };
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        panel.Controls.Add(_hocPhanField);
        panel.SetColumnSpan(_hocPhanField, 2);
        panel.Controls.Add(lblTinChi);
        panel.Controls.Add(lblHsHp);
        panel.Controls.Add(lblLT);
        panel.Controls.Add(lblTH);
        _contentPanel.Controls.Add(panel);
    }

    private LabelTextField fieldGV;
    private LabelTextField fieldLop;
    private LabelTextField fieldSiSoToiDa;

    private void SetFieldContainer()
    {
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 2,
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        fieldGV = new LabelTextField("Giảng viên", TextFieldType.ListBoxGV) { Dock = DockStyle.None };
        fieldLop = new LabelTextField("Lớp", TextFieldType.ListBoxLop) { Dock = DockStyle.None };
        fieldSiSoToiDa = new LabelTextField("Sĩ số tối đa", TextFieldType.Number) { Dock = DockStyle.None };

        fieldGV.tbGV.contentTextBox.Width = 300;
        fieldLop.tbLop.contentTextBox.Width = 300;
        fieldSiSoToiDa._numberField.contentTextBox.Width = 300;

        panel.Controls.Add(fieldGV);
        panel.Controls.Add(fieldLop);
        panel.Controls.Add(fieldSiSoToiDa);
        _contentPanel.Controls.Add(panel);
    }


    private void SetLichHocContainer()
    {
        SetTableLichHoc();

        Label titleLich = new Label
        {
            Text = "Lịch học",
            Font = GetFont.GetFont.GetMainFont(11, FontType.ExtraBold),
            AutoSize = true,
        };

        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;

        RoundTLP panel = new RoundTLP
        {
            Border = true,
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 2,
            Padding = new Padding(10),
            Margin = new Padding(3, 30, 3, 3),
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));


        panel.Controls.Add(titleLich);
        panel.Controls.Add(_insertButton);
        panel.Controls.Add(_tableLichHoc);
        panel.SetColumnSpan(_tableLichHoc, 2);

        _contentPanel.Controls.Add(panel);
    }

    private List<LichHocDto> listLichHoc;

    private void SetTableLichHoc()
    {
        listLichHoc = new List<LichHocDto>();
        _lichDisplay = listLichHoc.Cast<object>().ToList();

        var header = new[] { "ID", "Thứ", "Tiết BD", "Tiết KT", "Phòng" };
        var columnNames = new[] { "ID", "Thu", "TietBatDau", "TietKetThuc", "TenPH" };
        var columnNamesList = columnNames.ToList();

        if (_dialogType == DialogType.ChiTiet)
        {
            _tableLichHoc = new CustomTable(header.ToList(), columnNamesList, _lichDisplay);
        }
        else
        {
            _tableLichHoc = new CustomTable(header.ToList(), columnNamesList, _lichDisplay, true, true, true);
        }
    }

    private void UpdateDisplayDataLich()
    {
        var index = 1;
        foreach (LichHocDto dto in listLichHocTam)
        {
            dto.MaLH = index++;
        }

        _lichDisplay = ConvertObject.ConvertToDisplay(listLichHocTam, x => new
        {
            ID = x.MaLH,
            x.Thu,
            x.TietBatDau,
            x.TietKetThuc,
            _phongHocController.GetPhongHocById(x.MaPH).TenPH
        });
        _tableLichHoc._dataGridView.DataSource = _lichDisplay;
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
            BackColor = MyColor.MainColor,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0),
            Padding = new Padding(0, 10, 0, 10)
        };


        var lbl = GetTitle();
        panel.Controls.Add(lbl);
        _mainLayout.Controls.Add(panel);
    }

    private Label GetTitle()
    {
        var title = new Label
        {
            Text = _title,
            AutoSize = true,
            Dock = DockStyle.Fill,
            Anchor = AnchorStyles.None,
            Font = GetFont.GetFont.GetMainFont(16, FontType.ExtraBold),
            ForeColor = Color.White
        };
        return title;
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

    private void SetAction()
    {
        _insertButton._mouseDown += () => InsertLich();
        _tableLichHoc.OnDetail += i => DetailLich(i);
        _tableLichHoc.OnEdit += i => UpdateLich(i);
        _tableLichHoc.OnDelete += i => DeleteLich(i);
        //
        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
            _btnLuu._mouseDown += () => OnClickBtnLuu();

        // listField[2].tb.contentTextBox.TextChanged += (sender, args) => OnChangeHP();

        _hocPhanField.tb.contentTextBox.TextChanged += (sender, args) => OnChangeHP();
        fieldKhoa._combobox.combobox.SelectedIndexChanged += (sender, args) => OnChangeKhoa();

        fieldSiSoToiDa._numberField.contentTextBox.TextChanged += (sender, args) => OnChangeSiSoToiDa();
    }

    void SetSiSoMax()
    {
        fieldSiSoToiDa._numberField.contentTextBox.Text = GetSiSoPhongMax() + "";
    }

    void OnChangeSiSoToiDa()
    {
        string sisotoidas = fieldSiSoToiDa._numberField.contentTextBox.Text;
        if (sisotoidas.Equals("")) return;
        int sisotoida = int.Parse(sisotoidas);
        int sisotoidaphong = GetSiSoPhongMax();

        if (sisotoida > sisotoidaphong)
        {
            fieldSiSoToiDa._numberField.contentTextBox.Text = sisotoidaphong + "";
        }
    }

    int GetSiSoPhongMax()
    {
        if (listLichHocTam.Count == 0) return 0;

        int max = 0;
        foreach (var item in listLichHocTam)
        {
            PhongHocDto phong = _phongHocController.GetPhongHocById(item.MaPH);
            if (max < phong.SucChua)
            {
                max = phong.SucChua;
            }
        }

        return max;
    }

    void OnChangeKhoa()
    {
        string tenKhoa = fieldKhoa.GetSelectionCombobox();
        KhoaDto khoa = _khoaController.GetByTen(tenKhoa);
        //getListHocPhanByMaKhoa
        List<HocPhanDto> listHp = _hocPhanController.GetListHocPhanByMaKhoa(khoa.MaKhoa);
        _hocPhanField.tb.UpdateList(listHp);
    }

    void OnChangeHP()
    {
        string tenHP = _hocPhanField.tb.contentTextBox.Text;
        if (_hocPhanController.ExistByTen(tenHP))
        {
            _insertButton.Enabled = true;
            HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHP);
            UpdateDataHP(hocPhan);
        }
        else
        {
            _insertButton.Enabled = false;
        }
    }

    void UpdateDataHP(HocPhanDto hocPhan)
    {
        lblTinChi.lblValue.Text = hocPhan.SoTinChi + "";
        lblHsHp.lblValue.Text = hocPhan.HeSoHocPhan + "";
        lblLT.lblValue.Text = hocPhan.SoTietLyThuyet + "";
        lblTH.lblValue.Text = hocPhan.SoTietThucHanh + "";
    }


    /// //////////////////////LICH/////////////////////////
    private void InsertLich()
    {
        string tenHP = _hocPhanField.tb.contentTextBox.Text;
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHP);
        _lichDialog = new LichHocDialog("Thêm lịch học", DialogType.Them, hocPhan, new LichHocDto(), listLichSuDungTam, listLichHocTam);
        _lichDialog.Finish += (dto, listLichSuDung) =>
        {
            listLichSuDungTam.AddRange(listLichSuDung);
            listLichHocTam.Add(dto);
            UpdateDisplayDataLich();
            _hocPhanField.tb.Enable = false;

            Console.WriteLine("--------lich su dung hien tai cua nhom hoc phan---------");
            for (int i = 0; i < listLichSuDungTam.Count; i++)
            {
                Console.WriteLine(
                    $"lichsd {i} : phong {listLichSuDungTam[i].MaPH} {listLichSuDungTam[i].ThoiGianBatDau} {listLichSuDungTam[i].ThoiGianKetThuc}");
            }

            SetSiSoMax();
        };
        _lichDialog.ShowDialog();
    }

    private void DetailLich(int index)
    {
        string tenHP = _hocPhanField.tb.contentTextBox.Text;
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHP);
        var lich = GetLichById(index);
        _lichDialog = new LichHocDialog("Chi tiết lịch học", DialogType.ChiTiet, hocPhan, lich,
            new List<LichSuDungDto>(), listLichHocTam);
        _lichDialog.ShowDialog();
    }

    private void UpdateLich(int index)
    {
        string tenHP = _hocPhanField.tb.contentTextBox.Text;
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHP);
        var lich = GetLichById(index);

        //nếu update thì không cần check lại lịch sử dụng hiện có của lịch học nữa
        //xóa lịch sử dụng của lịch học được chọn trước khi truyền tham số

        List<LichSuDungDto> listLichThem = GetListLichSdThemForUpdate(lich);
        _lichDialog = new LichHocDialog("Sửa lịch học", DialogType.Sua, hocPhan, lich, listLichThem, listLichHocTam);

        _lichDialog.Finish += (dto, listLichSuDung) =>
        {
            //update LichSd
            DeleteLichSdOfLh(lich);
            listLichSuDungTam.AddRange(listLichSuDung);

            //update LichHoc
            foreach (var item in listLichHocTam)
                if (item.MaLH == dto.MaLH)
                {
                    item.MaPH = dto.MaPH;
                    item.Thu = dto.Thu;
                    item.TietBatDau = dto.TietBatDau;
                    item.TietKetThuc = dto.TietKetThuc;
                    item.TuNgay = dto.TuNgay;
                    item.DenNgay = dto.DenNgay;
                    item.SoTiet = dto.SoTiet;
                    item.Type = dto.Type;
                    break;
                }

            UpdateDisplayDataLich();
            SetSiSoMax();
        };
        _lichDialog.ShowDialog();
    }

    List<LichSuDungDto> GetListLichSdThemForUpdate(LichHocDto lich)
    {
        //copy list
        List<LichSuDungDto> rs = listLichSuDungTam
            .Select(x => new LichSuDungDto
            {
                MaLSD = x.MaLSD,
                MaPH = x.MaPH,
                ThoiGianBatDau = x.ThoiGianBatDau,
                ThoiGianKetThuc = x.ThoiGianKetThuc,
                GhiChu = x.GhiChu
            })
            .ToList();

        List<LichSuDungDto> lichSdOfLh = GetListLichSd(
            lich.Thu,
            lich.TietBatDau,
            lich.TietKetThuc,
            lich.TuNgay,
            lich.DenNgay,
            lich.MaPH
        );

        foreach (LichSuDungDto x in lichSdOfLh)
        {
            rs.RemoveAll(item => item.ThoiGianBatDau == x.ThoiGianBatDau
                                 && item.ThoiGianKetThuc == x.ThoiGianKetThuc
                                 && item.MaPH == x.MaPH
            );
        }

        return rs;
    }

    public void DeleteLich(int index)
    {
        var lich = GetLichById(index);

        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;


        //xóa lịch sử dụng ứng vs lịch học
        DeleteLichSdOfLh(lich);

        //xóa lịch học
        foreach (var item in listLichHocTam)
            if (item.MaLH == lich.MaLH)
            {
                listLichHocTam.Remove(item);
                break;
            }

        UpdateDisplayDataLich();
        SetSiSoMax();
    }

    void DeleteLichSdOfLh(LichHocDto lich)
    {
        List<LichSuDungDto> lichSdOfLh = GetListLichSd(
            lich.Thu,
            lich.TietBatDau,
            lich.TietKetThuc,
            lich.TuNgay,
            lich.DenNgay,
            lich.MaPH
        );

        foreach (LichSuDungDto x in lichSdOfLh)
        {
            listLichSuDungTam.RemoveAll(item => item.ThoiGianBatDau == x.ThoiGianBatDau
                                                && item.ThoiGianKetThuc == x.ThoiGianKetThuc
                                                && item.MaPH == x.MaPH
            );
        }

        Console.WriteLine("--------lich su dung hien tai cua nhom hoc phan---------");
        for (int i = 0; i < listLichSuDungTam.Count; i++)
        {
            Console.WriteLine(
                $"lichsd {i} : phong {listLichSuDungTam[i].MaPH} {listLichSuDungTam[i].ThoiGianBatDau} {listLichSuDungTam[i].ThoiGianKetThuc}");
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

    /// //////////////////////LICH/////////////////////////
    /// //////////////////////NHOMHOCPHAN/////////////////////////
    private void SetupInsert()
    {
        _insertButton.Enabled = false;
    }

    private void SetupUpdate()
    {
        //Không cho sửa học phần do ảnh hưởng lịch
        _hocPhanField.tb.Enable = false;
        
        NhomHocPhanDto nhomHP = _nhomHocPhanController.GetById(_selectedId);
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(nhomHP.MaHP);
        GiangVienDto giangVien = _giangVienController.GetById(nhomHP.MaGV);
        LopDto lop = _lopController.GetLopById(nhomHP.MaLop);
        
        _hocPhanField.tb.contentTextBox.Text = hocPhan.TenHP;
        fieldGV.tbGV.contentTextBox.Text = giangVien.TenGV;
        fieldLop.tbLop.contentTextBox.Text = lop.TenLop;

        SetupListLich();
        fieldSiSoToiDa._numberField.contentTextBox.Text = nhomHP.SiSoToiDa + "";
    }

    private void SetupDetail()
    {
        _hocPhanField.tb.Enable = false;
        
        NhomHocPhanDto nhomHP = _nhomHocPhanController.GetById(_selectedId);
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(nhomHP.MaHP);
        GiangVienDto giangVien = _giangVienController.GetById(nhomHP.MaGV);
        LopDto lop = _lopController.GetLopById(nhomHP.MaLop);
        
        _hocPhanField.tb.contentTextBox.Text = hocPhan.TenHP;
        fieldGV.tbGV.contentTextBox.Text = giangVien.TenGV;
        fieldLop.tbLop.contentTextBox.Text = lop.TenLop;

        fieldKhoa._combobox.Enable = false;
        _hocPhanField.tb.Enable = false;
        fieldGV.tbGV.Enable = false;
        fieldLop.tbLop.Enable = false;
        fieldSiSoToiDa._numberField.Enable = false;
        
        _insertButton.Enabled = false;
        
        SetupListLich();
        fieldSiSoToiDa._numberField.contentTextBox.Text = nhomHP.SiSoToiDa + "";
    }

    private void SetupListLich()
    {
        List<LichHocDto> listLich = _lichHocController.GetByMaNhp(_selectedId);
        foreach (var item in listLich) listLichHocTam.Add(item);
        UpdateDisplayDataLich();
    }


    /// //////////////////////NHOMHOCPHAN/////////////////////////
    private LichHocDto GetLichById(int ID)
    {
        foreach (LichHocDto lichHoc in listLichHocTam)
        {
            if (lichHoc.MaLH == ID)
            {
                return lichHoc;
            }
        }

        return new LichHocDto();
    }

    private void OnClickBtnLuu()
    {
        if (_dialogType == DialogType.Sua)
            Update();
        else if (_dialogType == DialogType.Them) Insert();
    }

    private void Insert()
    {
        var tenHp = _hocPhanField.tb.contentTextBox.Text;
        var tenGv = fieldGV.tbGV.contentTextBox.Text;
        var tenLop = fieldLop.tbLop.contentTextBox.Text;
        var siSoToiDa = fieldSiSoToiDa._numberField.contentTextBox.Text;
        if (!Validate(tenHp, tenGv, tenLop, siSoToiDa, listLichHocTam)) return;

        HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHp);
        GiangVienDto giangVien = _giangVienController.GetByTen(tenGv);
        LopDto lop = _lopController.GetByTen(tenLop);
        int sisotd = int.Parse(siSoToiDa);

        var hocKy = int.Parse(_hocKy);
        var nam = _nam;
        
        var nhomHP = new NhomHocPhanDto
        {
            MaDotDK = _maDotDK,
            MaGV = giangVien.MaGV,
            MaHP = hocPhan.MaHP,
            MaLop = lop.MaLop,
            HocKy = hocKy,
            Nam = nam,
            SiSoToiDa = sisotd
        };
        //
        if (!_nhomHocPhanController.Insert(nhomHP))
        {
            MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        List<NhomHocPhanDto> listnhp = _nhomHocPhanController.GetAll();
        var manhp = listnhp[listnhp.Count - 1].MaNHP;

        foreach (var item in listLichHocTam)
        {
            var lich = new LichHocDto
            {
                MaPH = item.MaPH,
                MaNHP = manhp,
                Thu = item.Thu,
                TietBatDau = item.TietBatDau,
                TietKetThuc = item.TietKetThuc,
                TuNgay = item.TuNgay,
                DenNgay = item.DenNgay,
                SoTiet = item.SoTiet,
                Type = item.Type
            };
            if (!_lichHocController.Insert(lich))
            {
                MessageBox.Show("Thêm lịch thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        //insert lichsd
        foreach (LichSuDungDto item in listLichSuDungTam)
        {
            LichSuDungDto lich = new LichSuDungDto
            {
                MaPH = item.MaPH,
                ThoiGianBatDau = item.ThoiGianBatDau,
                ThoiGianKetThuc = item.ThoiGianKetThuc,
                GhiChu = $"Nhóm học phần {manhp} sử dụng"
            };
            if (!_lichSuDungController.Insert(lich))
            {
                MessageBox.Show("Thêm lịch sử dụng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maLSD = _lichSuDungController.GetLastId();
            LichSD_NhomHPDto lisd_nhomhp = new()
            {
                MaLSD = maLSD,
                MaNHP = manhp
            };
            if (!_lichSD_NhomHPController.Add(lisd_nhomhp))
            {
                MessageBox.Show("Thêm lịch sử dụng nhóm hp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        MessageBox.Show("Thêm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Finish?.Invoke();
        Close();
    }

    //
    private void Update()
    {
        var tenHp = _hocPhanField.tb.contentTextBox.Text;
        var tenGv = fieldGV.tbGV.contentTextBox.Text;
        var tenLop = fieldLop.tbLop.contentTextBox.Text;
        var siSoToiDa = fieldSiSoToiDa._numberField.contentTextBox.Text;
        if (!Validate(tenHp, tenGv, tenLop, siSoToiDa, listLichHocTam)) return;

        HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHp);
        GiangVienDto giangVien = _giangVienController.GetByTen(tenGv);
        LopDto lop = _lopController.GetByTen(tenLop);
        int sisotd = int.Parse(siSoToiDa);

        var hocKy = int.Parse(_hocKy);
        var nam = _nam;

        List<DotDangKyDto> listDotDk = _dotDangKyController.GetAll();
        DotDangKyDto dotDk = listDotDk[listDotDk.Count - 1];
        
        var nhomHP = new NhomHocPhanDto
        {
            MaNHP = _selectedId,
            MaDotDK = _maDotDK,
            MaGV = giangVien.MaGV,
            MaHP = hocPhan.MaHP,
            MaLop = lop.MaLop,
            HocKy = hocKy,
            Nam = nam,
            SiSoToiDa = sisotd
        };
        
        if (!_nhomHocPhanController.Update(nhomHP))
        {
            MessageBox.Show("Sửa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        //Xóa hết lịch cũ để cập nhật lại
        List<LichHocDto> listLich = _lichHocController.GetByMaNhp(_selectedId);
        if (listLich.Count != 0)
        {
            foreach (var item in listLich) _lichHocController.HardDelete(item.MaLH);
        
            foreach (var item in listLichHocTam)
            {
                var lich = new LichHocDto
                {
                    MaPH = item.MaPH,
                    MaNHP = _selectedId,
                    Thu = item.Thu,
                    TietBatDau = item.TietBatDau,
                    TietKetThuc = item.TietKetThuc,
                    TuNgay = item.TuNgay,
                    DenNgay = item.DenNgay,
                    SoTiet = item.SoTiet,
                    Type = item.Type
                };
                if (!_lichHocController.Insert(lich))
                {
                    MessageBox.Show("Sửa lịch thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
        
        List<LichSD_NhomHPDto> listLichSd_NhomHP = _lichSD_NhomHPController.GetByMaNHP(_selectedId);
        if (listLichSd_NhomHP.Count != 0)
        {
            foreach (LichSD_NhomHPDto lsdnhp in listLichSd_NhomHP)
            {
                if (!_lichSD_NhomHPController.Delete(lsdnhp.MaLSD, lsdnhp.MaNHP))
                {
                    MessageBox.Show("Xóa lichsdnhp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            
            foreach (LichSD_NhomHPDto lsdnhp in listLichSd_NhomHP)
            {
                if (!_lichSuDungController.Delete(lsdnhp.MaLSD))
                {
                    MessageBox.Show("Xóa lichsd thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
        
        foreach (LichSuDungDto item in listLichSuDungTam)
        {
            LichSuDungDto lich = new LichSuDungDto
            {
                MaPH = item.MaPH,
                ThoiGianBatDau = item.ThoiGianBatDau,
                ThoiGianKetThuc = item.ThoiGianKetThuc,
                GhiChu = $"Nhóm học phần {_selectedId} sử dụng"
            };
            if (!_lichSuDungController.Insert(lich))
            {
                MessageBox.Show("Thêm lịch sử dụng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maLSD = _lichSuDungController.GetLastId();
            LichSD_NhomHPDto lisd_nhomhp = new()
            {
                MaLSD = maLSD,
                MaNHP = _selectedId
            };
            if (!_lichSD_NhomHPController.Add(lisd_nhomhp))
            {
                MessageBox.Show("Thêm lịch sử dụng nhóm hp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        
        
        //Xóa hết lịch cũ để cập nhật lại
        
        
        MessageBox.Show("Sửa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Finish?.Invoke();
        Close();
    }

    private bool Validate(string tenHP, string tenGV, string tenLop, string siSoToiDa, List<LichHocDto> listLich)
    {
        if (tenHP.Equals(""))
        {
            MessageBox.Show("Học phần không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!_hocPhanController.ExistByTen(tenHP))
        {
            MessageBox.Show("Học phần không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (tenGV.Equals(""))
        {
            MessageBox.Show("Giảng viên không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!_giangVienController.ExistByTen(tenGV))
        {
            MessageBox.Show("Giảng viên không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (tenLop.Equals(""))
        {
            MessageBox.Show("Lớp không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!_lopController.ExistByTen(tenLop))
        {
            MessageBox.Show("Lớp không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (siSoToiDa.Equals(""))
        {
            MessageBox.Show("Sĩ số tối đa không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (listLichHocTam.Count == 0)
        {
            MessageBox.Show("Vui lòng thêm lịch học!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        string tenHp = _hocPhanField.tb.contentTextBox.Text;
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHp);

        if (!listLich.Any(x => x.Type.Equals("Lý thuyết + Thực hành")))
        {
            if (hocPhan.SoTietThucHanh != 0)
            {
                if (!listLich.Any(x => x.Type.Equals("Thực hành")))
                {
                    MessageBox.Show(
                        $"Chưa đủ tiết học, vui lòng thêm lịch học \nSố tiết thực hành học phần {tenHp} là {hocPhan.SoTietThucHanh} tiết",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (hocPhan.SoTietLyThuyet != 0)
            {
                if (!listLich.Any(x => x.Type.Equals("Lý thuyết")))
                {
                    MessageBox.Show(
                        $"Chưa đủ tiết học, vui lòng thêm lịch học \nSố tiết lý thuyết học phần{tenHp} là {hocPhan.SoTietLyThuyet} tiết",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
        }

        return true;
    }
}