using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.NavList.Dialog;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components;

public class NhomHocPhanScreen : RoundTLP
{

    private MyTLP _contentLayout;
    private CustomButton _backButton;
    private MyTLP _mainLayout;

    // private List<SinhVienDTO> _rawData;

    private CustomTable _table;
    private string _title;
    private string imgPath = "";
    private TitleButton _insertButton;

    private int _hky;
    private string _nam;
    private int _idDotDK;
    private DialogType _dialogType;

    private List<NhomHocPhanDto> _rawData;
    private List<object> _displayData;

    private NhomHocPhanController _nhomHocPhanController;
    private KhoaController _khoaController;
    private HocPhanController _hocPhanController;
    private DotDangKyController _dotDangKyController;
    private LichHocController _lichHocController;
    private LichSuDungController _lichSuDungController;
    private LichSD_NhomHPController _lichSD_NhomHPController;

    public NhomHocPhanScreen(string title, int hky, string nam, int idDotDK, DialogType dialogType)
    {
        _title = title;
        _hky = hky;
        _nam = nam;
        _idDotDK = idDotDK;
        _dialogType = dialogType;
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _khoaController =  KhoaController.GetInstance();
        _hocPhanController =  HocPhanController.GetInstance();
        _dotDangKyController = DotDangKyController.GetInstance();
        _lichHocController = LichHocController.GetInstance();
        _lichSuDungController = LichSuDungController.GetInstance();
        _lichSD_NhomHPController = LichSD_NhomHPController.GetInstance();
        Init();
    }
    
    public event Action Finish;
    public event Action Back;

    private void Init()
    {
        Dock = DockStyle.Fill;
        BackColor = MyColor.White;
        Border = true;

        _mainLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            Padding = new Padding(7),
        };

        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        SetContent();
        SetBottom();
        Controls.Add(_mainLayout);
        SetAction();

        if (_dialogType == DialogType.ChiTiet)
        {
            SetupDetail();
        }
        else if (_dialogType == DialogType.Sua)
        {
            SetupUpdate();
        }
    }

    private void SetContent()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1
        };

        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTitleBar();
        SetTable();

        _mainLayout.Controls.Add(_contentLayout);
    }

    private MyTLP panelTable;
    void SetTable()
    {
        panelTable = new MyTLP()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
        };
        panelTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panelTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        SetFilterContainer();
        
        SetRawData();
        var headers = new[] { "Mã nhóm", "Học phần", "Mã học phần", "Khoa", "Sĩ số", "Giảng viên" };
        var columnNames = new[] { "MaNHP", "TenHP", "MaHP", "TenKhoa", "Siso", "TenGiangVien" };
        SetDisplayData();

        if (_dialogType == DialogType.ChiTiet)
        {
            _table = new CustomTable(headers.ToList(), columnNames.ToList(), _displayData); 
        }
        else
        {
            _table = new CustomTable(headers.ToList(), columnNames.ToList(), _displayData, true, true, true); 
        }
        
        panelTable.Controls.Add(_table);
        _contentLayout.Controls.Add(panelTable);
    }

    private LabelTextField khoaField;
    private LabelTextField hocPhanField;
    private LabelTextField nhpField;
    void SetFilterContainer()
    {
        RoundTLP panel = new RoundTLP()
        {
            Border = true,
            Dock = DockStyle.Fill,
            AutoSize = true,
            RowCount = 4,
            Padding = new Padding(5, 5, 5, 5),
        };

        Label lbl = new Label
        {
            AutoSize = true,
            Text = "FILTER",
            Anchor = AnchorStyles.None,
            Font = GetFont.GetFont.GetMainFont(15, FontType.ExtraBold),
            Margin = new Padding(10),
        };
        
        panel.Controls.Add(lbl);

        List<KhoaDto> listKhoa = _khoaController.GetDanhSachKhoa();
        List<string> listTenKhoa = new List<string>();
        listTenKhoa.Add("Tất cả");
        listTenKhoa.AddRange(listKhoa.Select(x => x.TenKhoa).ToList());
        listTenKhoa.RemoveAt(1);
         
        khoaField = new LabelTextField("Khoa", TextFieldType.Combobox){Dock = DockStyle.None};
        khoaField._combobox.combobox.Width = 300;
        khoaField.SetComboboxList(listTenKhoa);
        khoaField.SetComboboxSelection("Tất cả");
        
        hocPhanField = new LabelTextField("Học phần", TextFieldType.ListBoxHP){Dock = DockStyle.None};
        hocPhanField.tb.contentTextBox.Width = 300;
        
        nhpField = new LabelTextField("Mã nhóm", TextFieldType.NormalText);
        
        panel.BackColor = MyColor.LightGray;
        panel.Controls.Add(khoaField);
        panel.Controls.Add(hocPhanField);
        panel.Controls.Add(nhpField);
        panelTable.Controls.Add(panel);
        SetActionFilter();
    }

    void SetActionFilter()
    {
        khoaField._combobox.combobox.SelectedIndexChanged += (sender, args) =>
        {
            hocPhanField.tb.contentTextBox.Text = "";
            string tenKhoa = khoaField.GetSelectionCombobox();
            if (tenKhoa.Equals("Tất cả"))
            {
                List<HocPhanDto> listHocPhan = _hocPhanController.GetAll();
                hocPhanField.tb.UpdateList(listHocPhan);
            }
            else
            {
                KhoaDto khoa = _khoaController.GetByTen(tenKhoa);
                List<HocPhanDto> listHocPhan = _hocPhanController.GetListHocPhanByMaKhoa(khoa.MaKhoa);
                hocPhanField.tb.UpdateList(listHocPhan);
            }
        };

        khoaField._combobox.combobox.SelectedIndexChanged += (sender, args) => Filter();
        hocPhanField.tb.contentTextBox.TextChanged += (sender, args) => Filter();
        nhpField._field.contentTextBox.TextChanged += (sender, args) => Filter();
    }

    void Filter()
    {
        string tenKhoa = khoaField.GetSelectionCombobox();
        string tenHocPhan = hocPhanField.tb.contentTextBox.Text;
        string maNhp = nhpField.GetTextTextField();

        List<NhomHocPhanDisplay> rs = _nhomHocPhanController.ConvertDtoToDisplay(_rawData);
        
        // 1. Lọc theo tên khoa
        if (!string.IsNullOrEmpty(tenKhoa) && tenKhoa != "Tất cả")
        {
            rs = rs.Where(x => x.TenKhoa.Equals(tenKhoa, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        // 2. Lọc theo tên học phần
        if (!string.IsNullOrEmpty(tenHocPhan))
        {
            rs = rs.Where(x => x.TenHP.Equals(tenHocPhan, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // 3. Lọc theo mã NHP (search kiểu contains để dễ tìm)
        if (!string.IsNullOrEmpty(maNhp))
        {
            rs = rs.Where(x => x.MaNHP.ToString().Contains(maNhp)).ToList();
        }
        
        _displayData = rs.Cast<object>().ToList();
        _table.UpdateData(_displayData);
    }
    
    void SetRawData()
    {
        _rawData = _nhomHocPhanController.GetByMaDotDky(_idDotDK);
    }

    private void SetTitleBar()
    {
        var panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            Margin = new Padding(0),
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        
        _insertButton = new TitleButton("Thêm", "plus.svg");
        _insertButton.Margin = new Padding(3, 3, 20, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;

        _backButton = new CustomButton(20, 20, "back.svg", MyColor.GrayBackGround)
            { Pad = 5, Anchor = AnchorStyles.None };
        _backButton.HoverColor = MyColor.GrayHoverColor;
        _backButton.SelectColor = MyColor.GraySelectColor;

        var lblTitle = new Label
        {
            Text = _title,
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(13, FontType.Black),
            Margin = new Padding(7, 3, 3, 3)
        };
        panel.Controls.Add(_backButton);
        panel.Controls.Add(lblTitle);
        panel.Controls.Add(_insertButton);

        _contentLayout.Controls.Add(panel);
    }

    private void SetDisplayData()
    {
        _displayData = _nhomHocPhanController.ConvertDtoToDisplay(_rawData).Cast<object>().ToList();
    }

    // private void UpdateDataDisplay(List<SVNhapDiemDisplay> input)
    // {
    //     _displayData = ConvertObject.ConvertToDisplay(input, x => new
    //     {
    //         x.MaSV,
    //         x.TenSV,
    //         x.GioiTinh,
    //         x.NgaySinh
    //     });
    // }

    private LabelTextField fieldTgbd;
    private LabelTextField fieldTgkt;
    private TitleButton updateBtn;
    private void SetBottom()
    {
        fieldTgbd = new LabelTextField("Thời gian bắt đầu", TextFieldType.DateTime){Dock = DockStyle.None};
        fieldTgkt = new LabelTextField("Thời gian kết thúc", TextFieldType.DateTime){Dock = DockStyle.None};
        updateBtn = new TitleButton("Cập nhật", "reload.svg"){Margin = new Padding(5, 20, 20, 5)};
        updateBtn._label.Font = GetFont.GetFont.GetMainFont(12, FontType.ExtraBold);
        //Thêm có Đặt lại, Lưu, Hủy
        var panel = new RoundTLP()
        {
            Border = true,
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3,
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        panel.Controls.Add(fieldTgbd);
        panel.Controls.Add(fieldTgkt);
        panel.Controls.Add(updateBtn);
        _mainLayout.Controls.Add(panel);
    }


    private void SetupDetail()
    {
        DotDangKyDto dotDK = _dotDangKyController.GetById(_idDotDK);
        fieldTgbd._dTGioField.dateField.Value = dotDK.ThoiGianBatDau;
        fieldTgbd._dTNgayField.dateField.Value = dotDK.ThoiGianBatDau;
        
        fieldTgkt._dTGioField.dateField.Value = dotDK.ThoiGianKetThuc;
        fieldTgkt._dTNgayField.dateField.Value = dotDK.ThoiGianKetThuc;
        
        _insertButton.Enabled = false;
        fieldTgbd.Enabled = false;
        fieldTgkt.Enabled = false;
        updateBtn.Enabled = false;
    }

    private void SetupUpdate()
    {
        DotDangKyDto dotDK = _dotDangKyController.GetById(_idDotDK);
        fieldTgbd._dTGioField.dateField.Value = dotDK.ThoiGianBatDau;
        fieldTgbd._dTNgayField.dateField.Value = dotDK.ThoiGianBatDau;
        
        fieldTgkt._dTGioField.dateField.Value = dotDK.ThoiGianKetThuc;
        fieldTgkt._dTNgayField.dateField.Value = dotDK.ThoiGianKetThuc;
        
        DateTime now  = DateTime.Now;
        if (now > dotDK.ThoiGianBatDau && now < dotDK.ThoiGianKetThuc)
        {
            UnEnableScreen();
        }
    }

    private void SetAction()
    {
        _backButton._mouseDown += () => Back?.Invoke();
        _table.OnDetail += (i) => DetailNhp(i);
        _table.OnEdit += i => EditNhp(i);
        _table.OnDelete += i => DeleteNhp(i);
        
        _insertButton._mouseDown += () => InsertNHP();
        updateBtn._mouseDown += () => UpdateTgdk();
    }

    void UpdateTgdk()
    {
        DateTime start = fieldTgbd._dTNgayField.dateField.Value;
        DateTime end = fieldTgkt._dTNgayField.dateField.Value;
        
        DateTime now = DateTime.Now;
        if (now > start && now < end)
        {
            DialogResult rs = MessageBox.Show($"Lich đăng ký năm {_nam} học kỳ {_hky} sẽ được mở vào: \n" +
                            $"{start.ToString("hh:mm:ss dd/MM/yyyy")} đến \n" +
                            $"{end.ToString("hh:mm:ss dd/MM/yyyy")} \n" +
                            $"Sau khi cập nhật sẽ không thể thay đổi!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (rs == DialogResult.No) return;
            
            DotDangKyDto dotDangKyDto = _dotDangKyController.GetById(_idDotDK);
            dotDangKyDto.ThoiGianBatDau = start;
            dotDangKyDto.ThoiGianKetThuc = end;
            if (!_dotDangKyController.Update(dotDangKyDto))
            {
                MessageBox.Show("Cap nhat lichdk thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UnEnableScreen();
        }
    }

    void UnEnableScreen()
    {
        _insertButton.Enabled = false;
        _table.DisapleActionColumn();
        fieldTgbd.Enabled = false;
        fieldTgkt.Enabled = false;
        updateBtn.Enabled = false;
    }

    void EditNhp(int i)
    {
        NhomHocPhanDialog dialog = new NhomHocPhanDialog(
            "Sửa nhóm học phần",
            DialogType.Sua,
            "1",
            "2025",
            _idDotDK,
            i
        );
        
        dialog.Finish += () =>
        {
            SetRawData();
            SetDisplayData();
            _table.UpdateData(_displayData);
        };
        
        dialog.ShowDialog();
    }
    void InsertNHP()
    {
        NhomHocPhanDialog dialog = new NhomHocPhanDialog(
                "Thêm nhóm học phần",
                DialogType.Them,
                "1",
                "2025",
                _idDotDK
            );

        dialog.Finish += () =>
        {
            SetRawData();
            SetDisplayData();
            _table.UpdateData(_displayData);
        };
        
        dialog.ShowDialog();
    }

    void DetailNhp(int i)
    {
        NhomHocPhanDialog dialog = new NhomHocPhanDialog(
            "Chi tiết học phần",
            DialogType.ChiTiet,
            "1",
            "2025",
            _idDotDK,
            i
        );
        
        dialog.ShowDialog();
    }

    void DeleteNhp(int i)
    {
        var select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);
        if (select == DialogResult.No) return;
        
        if (!_nhomHocPhanController.Delete(i))
        {
            MessageBox.Show("Xóa nhp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        List<LichHocDto> listLichHoc = _lichHocController.GetByMaNhp(i);
        if (listLichHoc.Count != 0)
        {
            foreach (LichHocDto lich in listLichHoc)
            {
                if (!_lichHocController.Delete(lich.MaLH))
                {
                    MessageBox.Show("Xóa lich hoc thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
        
        List<LichSD_NhomHPDto> listLichSd_NhomHP = _lichSD_NhomHPController.GetByMaNHP(i);
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
        
        MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        SetRawData();
        SetDisplayData();
        _table.UpdateData(_displayData);
    }


    //
    // private List<SVNhapDiemDisplay> ConvertDtoToDisplay(List<SinhVienDTO> input)
    // {
    //     List<SVNhapDiemDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new SVNhapDiemDisplay
    //     {
    //         MaSV = x.MaSinhVien,
    //         TenSV = x.TenSinhVien,
    //         GioiTinh = x.GioiTinh,
    //         NgaySinh = x.NgaySinh
    //     });
    //     return rs;
    // }

    // private void SetupCotDiem()
    // {
    //     var maHP = _hocPhan.MaHP;
    //     foreach (var sv in _rawData)
    //     {
    //         KetQuaDto kq = _ketQuaController.GetByMaSVMaHP(sv.MaSinhVien, maHP);
    //         DiemThiSV diemThi = new DiemThiSV
    //         {
    //             MaSV = sv.MaSinhVien,
    //             diemThi = kq.DiemThi
    //         };
    //         listDiemThiSV.Add(diemThi);
    //     }
    // }
}