using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class CaThiDialog : Form
{
    private readonly CaThiController _CaThiController;
    private readonly CaThi_SinhVienController _caThiSinhVienController;
    private readonly DangKyController _dangKyController;
    private readonly DialogType _dialogType;

    private readonly int _hky;
    private readonly HocPhanController _hocPhanController;
    private readonly int _idCaThi;
    private readonly List<LabelTextField> _listTextBox;
    private readonly string _nam;
    private readonly PhongHocController _phongHocController;

    private readonly SinhVienController _sinhVienController;
    private readonly string _title;
    private TitleButton _btnLuu;


    private MyTLP _contentLayout;
    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private MyTLP _mainLayout;
    private List<SinhVienDTO> _rawDataSV;
    private List<SinhVienDTO> _rawDataSVEx;


    private CTDTTable _tableSinhVien;

    private CTDTTable _tableSinhVienEx;
    private List<object> displayDataSV;
    private List<object> displayDataSVEx;

    public CaThiDialog(string title, DialogType dialogType, int hky, string nam, int idCaThi = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _CaThiController = CaThiController.GetInstance();
        _sinhVienController = SinhVienController.GetInstance();
        _caThiSinhVienController = CaThi_SinhVienController.GetInstance();
        _dangKyController = DangKyController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _idCaThi = idCaThi;
        _title = title;
        _dialogType = dialogType;
        _hky = hky;
        _nam = nam;
        Init();
    }

    public event Action Finish;

    private void Init()
    {
        Width = 1400;
        Height = 750;
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

        Controls.Add(_mainLayout);

        SetAction();


        if (_dialogType == DialogType.Them)
            SetupInsert();
        else if (_dialogType == DialogType.Sua)
            SetupUpdate();
        else
            SetupDetail();
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
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3
        };

        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        SetFieldContainer();
        SetTableSV();
        SetTableExam();
        _mainLayout.Controls.Add(_contentLayout);
    }

    private void SetFieldContainer()
    {
        var panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(10),
            Padding = new Padding(10),
            Border = true
        };

        var list = new List<LabelTextField>();

        list.Add(new LabelTextField("Học kỳ", TextFieldType.NormalText));
        list.Add(new LabelTextField("Năm", TextFieldType.NormalText));
        list.Add(new LabelTextField("Học phần", TextFieldType.ListBoxHP));
        list.Add(new LabelTextField("Phòng", TextFieldType.ListBoxPH));
        list.Add(new LabelTextField("Thời gian bắt đầu", TextFieldType.DateTime));
        list.Add(new LabelTextField("Thời lượng", TextFieldType.Timehhmm));

        foreach (var item in list)
        {
            _listTextBox.Add(item);
            panel.Controls.Add(item);
        }

        _listTextBox[0].SetText("Học kỳ " + _hky);
        _listTextBox[1].SetText("Năm " + _nam);

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._field.Enable = false;

        _listTextBox[4]._dTGioField.dateField.Width = 10;
        _listTextBox[5]._fTimeHH.dateField.Value = DateTime.Today;
        _listTextBox[5]._fTimeMM.dateField.Value = DateTime.Today;
        _contentLayout.Controls.Add(panel);
    }

    private void SetTableSV()
    {
        var panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(10),
            Padding = new Padding(10),
            Border = true,
            RowCount = 2
        };

        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var title = new Label
        {
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold),
            Text = "DS sinh viên học học phần"
        };

        _rawDataSV = new List<SinhVienDTO>();
        displayDataSV = new List<object>();
        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            var columnNameArr = new[] { "MaSV", "TenSV", "ActionPlus" };
            var headerArr = new[] { "Mã sinh viên", "Tên sinh viên", "Hành động" };
            _tableSinhVien = new CTDTTable(headerArr.ToList(), columnNameArr.ToList(), displayDataSV,
                TableCTDTType.Plus);
        }
        else
        {
            var columnNameArr = new[] { "MaSV", "TenSV" };
            var headerArr = new[] { "Mã sinh viên", "Tên sinh viên" };
            _tableSinhVien = new CTDTTable(headerArr.ToList(), columnNameArr.ToList(), displayDataSV,
                TableCTDTType.Detail);
        }


        panel.Controls.Add(title);
        panel.Controls.Add(_tableSinhVien);

        _contentLayout.Controls.Add(panel);
    }

    private void SetTableExam()
    {
        var panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(10),
            Padding = new Padding(10),
            Border = true,
            RowCount = 2
        };

        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var title = new Label
        {
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold),
            Text = "DS thi"
        };

        _rawDataSVEx = new List<SinhVienDTO>();
        displayDataSVEx = new List<object>();

        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            var columnNameArr = new[] { "MaSV", "TenSV", "ActionMinus" };
            var headerArr = new[] { "Mã sinh viên", "Tên sinh viên", "Hành động" };
            _tableSinhVienEx = new CTDTTable(headerArr.ToList(), columnNameArr.ToList(), displayDataSV,
                TableCTDTType.Minus);
        }
        else
        {
            var columnNameArr = new[] { "MaSV", "TenSV" };
            var headerArr = new[] { "Mã sinh viên", "Tên sinh viên" };
            _tableSinhVienEx = new CTDTTable(headerArr.ToList(), columnNameArr.ToList(), displayDataSV,
                TableCTDTType.Detail);
        }


        panel.Controls.Add(title);
        panel.Controls.Add(_tableSinhVienEx);
        _contentLayout.Controls.Add(panel);
    }

    private void UpdateDataDisplayTblSv(List<SinhVienDTO> dtos)
    {
        displayDataSV = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaSV = x.MaSinhVien,
            TenSV = x.TenSinhVien
        });
    }

    private void UpdateDataDisplayTblSvEx(List<SinhVienDTO> dtos)
    {
        displayDataSVEx = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            MaSV = x.MaSinhVien,
            TenSV = x.TenSinhVien
        });
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

        _mainLayout.Controls.Add(panel);
    }

    private void SetupInsert()
    {
    }

    private void SetupUpdate()
    {
        var caThi = _CaThiController.GetById(_idCaThi);
        var hocPhan = _hocPhanController.GetHocPhanById(caThi.MaHP);
        var phong = _phongHocController.GetPhongHocById(caThi.MaPH);

        _listTextBox[2].tb.contentTextBox.Text = hocPhan.TenHP;
        _listTextBox[3].tbPH.contentTextBox.Text = phong.TenPH;
        _listTextBox[4]._dTNgayField.dateField.Value = ConvertDate.ConvertStringToDateTime(caThi.ThoiGianBatDau);
        _listTextBox[4]._dTGioField.dateField.Value = ConvertDate.ConvertStringToDateTime(caThi.ThoiGianBatDau);
        _listTextBox[5]._fTimeHH.dateField.Value = ConvertDate.ConvertStringToTime(caThi.ThoiLuong);
        _listTextBox[5]._fTimeMM.dateField.Value = ConvertDate.ConvertStringToTime(caThi.ThoiLuong);


        var listCTSV = _caThiSinhVienController.GetByMaCT(_idCaThi);
        foreach (var item in listCTSV)
        {
            var sinhVien = _sinhVienController.GetById(item.MaSV);
            _rawDataSV.RemoveAll(x => x.MaSinhVien == item.MaSV);
            _rawDataSVEx.Add(sinhVien);
        }

        UpdateDataDisplayTblSv(_rawDataSV);
        _tableSinhVien.UpdateData(displayDataSV);

        UpdateDataDisplayTblSvEx(_rawDataSVEx);
        _tableSinhVienEx.UpdateData(displayDataSVEx);
    }

    private void SetupDetail()
    {
        var caThi = _CaThiController.GetById(_idCaThi);
        var hocPhan = _hocPhanController.GetHocPhanById(caThi.MaHP);
        var phong = _phongHocController.GetPhongHocById(caThi.MaPH);

        _listTextBox[2].tb.contentTextBox.Text = hocPhan.TenHP;
        _listTextBox[3].tbPH.contentTextBox.Text = phong.TenPH;
        _listTextBox[4]._dTNgayField.dateField.Value = ConvertDate.ConvertStringToDateTime(caThi.ThoiGianBatDau);
        _listTextBox[4]._dTGioField.dateField.Value = ConvertDate.ConvertStringToDateTime(caThi.ThoiGianBatDau);
        _listTextBox[5]._fTimeHH.dateField.Value = ConvertDate.ConvertStringToTime(caThi.ThoiLuong);
        _listTextBox[5]._fTimeMM.dateField.Value = ConvertDate.ConvertStringToTime(caThi.ThoiLuong);


        var listCTSV = _caThiSinhVienController.GetByMaCT(_idCaThi);
        foreach (var item in listCTSV)
        {
            var sinhVien = _sinhVienController.GetById(item.MaSV);
            _rawDataSV.RemoveAll(x => x.MaSinhVien == item.MaSV);
            _rawDataSVEx.Add(sinhVien);
        }

        UpdateDataDisplayTblSv(_rawDataSV);
        _tableSinhVien.UpdateData(displayDataSV);

        UpdateDataDisplayTblSvEx(_rawDataSVEx);
        _tableSinhVienEx.UpdateData(displayDataSVEx);

        _listTextBox[2].tb.Enable = false;
        _listTextBox[3].tbPH.Enable = false;
        _listTextBox[4]._dTNgayField.Enabled = false;
        _listTextBox[4]._dTGioField.Enabled = false;
        _listTextBox[5]._fTimeHH.Enabled = false;
        _listTextBox[5]._fTimeMM.Enabled = false;
    }

    private void SetAction()
    {
        if (_dialogType == DialogType.Them) _btnLuu._mouseDown += () => Insert();

        if (_dialogType == DialogType.Sua) _btnLuu._mouseDown += () => Update();

        _listTextBox[2].tb.contentTextBox.TextChanged += (sender, args) => UpdateTableSVByHP();
        _tableSinhVien.OnDetail += i => DetailSV(i);
        _tableSinhVienEx.OnDetail += i => DetailSV(i);
        _tableSinhVien.BtnClick += i => InsertSV(i);
        _tableSinhVienEx.BtnClick += i => RemoveSV(i);
    }

    private void DetailSV(int i)
    {
        var svd = new SinhVienDialog("Chi tiết sinh viên", DialogType.ChiTiet, i);
        svd.ShowDialog();
    }

    private void InsertSV(int maSV)
    {
        var sv = _sinhVienController.GetById(maSV);
        _rawDataSV.RemoveAll(x => x.MaSinhVien == maSV);
        _rawDataSVEx.Add(sv);

        UpdateDataDisplayTblSv(_rawDataSV);
        UpdateDataDisplayTblSvEx(_rawDataSVEx);

        _tableSinhVien.UpdateData(displayDataSV);
        _tableSinhVienEx.UpdateData(displayDataSVEx);
    }

    private void RemoveSV(int maSV)
    {
        var sv = _sinhVienController.GetById(maSV);
        _rawDataSVEx.RemoveAll(x => x.MaSinhVien == maSV);
        _rawDataSV.Add(sv);

        UpdateDataDisplayTblSv(_rawDataSV);
        UpdateDataDisplayTblSvEx(_rawDataSVEx);

        _tableSinhVien.UpdateData(displayDataSV);
        _tableSinhVienEx.UpdateData(displayDataSVEx);
    }

    private void UpdateTableSVByHP()
    {
        //lọc sv có đăng ký học phần này ở học kỳ này
        var tenHP = _listTextBox[2].tb.contentTextBox.Text;
        var maHP = _hocPhanController.GetHocPhanByTen(tenHP).MaHP;
        if (maHP == 0) return;
        _rawDataSV.Clear();
        var listDK = _dangKyController.GetListByHkyNamMaHP(_hky, _nam, maHP);

        foreach (var item in listDK)
        {
            var sv = _sinhVienController.GetById(item.MaSV);
            //loại những sv đã cho vào thi học phần này
            if (!_caThiSinhVienController.ExistSVThiHp(maHP, sv.MaSinhVien)) _rawDataSV.Add(sv);
        }

        UpdateDataDisplayTblSv(_rawDataSV);
        _tableSinhVien.UpdateData(displayDataSV);
    }

    private void Insert()
    {
        var tbHocPhan = _listTextBox[2].tb.contentTextBox;
        var tbPhongHoc = _listTextBox[3].tbPH.contentTextBox;

        if (!Validate(tbHocPhan, tbPhongHoc)) return;

        var MaHP = _hocPhanController.GetHocPhanByTen(tbHocPhan.Text).MaHP;
        var MaPH = _phongHocController.GetByTen(tbPhongHoc.Text).MaPH;
        var startTime = _listTextBox[4]._dTNgayField.dateField.Value;
        var thu = ConvertDate.GetThuByDateTime(startTime);
        var thoiGianBd = _listTextBox[4]._dTNgayField.dateField.Text + " " + _listTextBox[4]._dTGioField.dateField.Text;
        var thoiLuong = _listTextBox[5]._fTimeHH.dateField.Text + ":" + _listTextBox[5]._fTimeMM.dateField.Text;

        var caThi = new CaThiDto
        {
            MaHP = MaHP,
            MaPH = MaPH,
            Thu = "Thứ " + thu,
            ThoiGianBatDau = thoiGianBd,
            ThoiLuong = thoiLuong,
            Nam = _nam,
            HocKy = _hky
        };

        if (!_CaThiController.Insert(caThi))
        {
            MessageBox.Show("Thêm ca thi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        foreach (var sv in _rawDataSVEx)
        {
            var cathisinhvien = new CaThi_SinhVienDto
            {
                MaCT = _CaThiController.GetLastId(),
                MaSV = sv.MaSinhVien
            };
            if (!_caThiSinhVienController.Insert(cathisinhvien))
            {
                MessageBox.Show("Thêm ca thi sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        MessageBox.Show("Thêm ca thi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
        Finish.Invoke();
    }

    private void Update()
    {
        var tbHocPhan = _listTextBox[2].tb.contentTextBox;
        var tbPhongHoc = _listTextBox[3].tbPH.contentTextBox;

        if (!Validate(tbHocPhan, tbPhongHoc)) return;

        var MaHP = _hocPhanController.GetHocPhanByTen(tbHocPhan.Text).MaHP;
        var MaPH = _phongHocController.GetByTen(tbPhongHoc.Text).MaPH;
        var startTime = _listTextBox[4]._dTNgayField.dateField.Value;
        var thu = ConvertDate.GetThuByDateTime(startTime);
        var thoiGianBd = _listTextBox[4]._dTNgayField.dateField.Text + " " + _listTextBox[4]._dTGioField.dateField.Text;
        var thoiLuong = _listTextBox[5]._fTimeHH.dateField.Text + ":" + _listTextBox[5]._fTimeMM.dateField.Text;

        var caThi = new CaThiDto
        {
            MaCT = _idCaThi,
            MaHP = MaHP,
            MaPH = MaPH,
            Thu = "Thứ" + thu,
            ThoiGianBatDau = thoiGianBd,
            ThoiLuong = thoiLuong,
            Nam = _nam,
            HocKy = _hky
        };

        if (!_CaThiController.Update(caThi))
        {
            MessageBox.Show("Sua ca thi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        //xóa list cũ
        _caThiSinhVienController.HardDeleteByMaCT(_idCaThi);
        foreach (var sv in _rawDataSVEx)
        {
            var cathisinhvien = new CaThi_SinhVienDto
            {
                MaCT = _idCaThi,
                MaSV = sv.MaSinhVien
            };
            if (!_caThiSinhVienController.Insert(cathisinhvien))
            {
                MessageBox.Show("Sua ca thi sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        MessageBox.Show("Sửa ca thi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
        Finish.Invoke();
    }

    public bool Validate(TextBox tbHocPhan, TextBox tbPhongHoc)
    {
        if (CommonUse.Validate.IsEmpty(tbHocPhan.Text))
        {
            MessageBox.Show("Học phần không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbHocPhan.Focus();
            tbHocPhan.SelectAll();
            return false;
        }

        if (!_hocPhanController.ExistByTen(tbHocPhan.Text))
        {
            MessageBox.Show("Học phần không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbHocPhan.Focus();
            tbHocPhan.SelectAll();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(tbPhongHoc.Text))
        {
            MessageBox.Show("Phòng học không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbPhongHoc.Focus();
            tbPhongHoc.SelectAll();
            return false;
        }

        if (!_phongHocController.ExistByTen(tbPhongHoc.Text))
        {
            MessageBox.Show("Phòng học không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbPhongHoc.Focus();
            tbPhongHoc.SelectAll();
            return false;
        }

        if (_rawDataSVEx.Count == 0)
        {
            MessageBox.Show("Vui lòng thêm sinh viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }
}