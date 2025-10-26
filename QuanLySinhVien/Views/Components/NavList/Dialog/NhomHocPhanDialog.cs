using System.Diagnostics.PerformanceData;
using Mysqlx.Crud;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NhomHocPhanDialog : Form
{
    
    private LichHocController _lichHocController;
    private HocPhanController _hocPhanController;
    private GiangVienController _giangVienController;
    private PhongHocController _phongHocController;
    private NhomHocPhanController _nhomHocPhanController;
    private LopController _lopController;
    
    private CustomButton _exitButton;
    private MyTLP _mainLayout;
    private string _title;
    private DialogType _dialogType;
    protected TitleButton _btnLuu;
    private TitleButton _insertButton;
    private CustomTable _tableLichHoc;
    private RoundTLP _lichContainer;

    private MyTLP _contentPanel;

    private List<LabelTextField> listField;
    
    private List<object> _lichDisplay;

    private string _hocKy;
    private string _nam;

    private LichHocDialog _lichDialog;

    private List<LichHocDto> listLichTam;

    private int _selectedId;

    public event Action Finish;

    private List<NhomHocPhanDto> _currentListNhp; // list nhoms học phần bên ngoài để check trùng lịch các nhóm học phần bên ngoài
    
    public NhomHocPhanDialog(string title, DialogType dialogType, string hocKy, string nam, List<NhomHocPhanDto> currentListNhp, int index = -1)
    {
        _lichDisplay = new List<object>();
        listField = new List<LabelTextField>();
        listLichTam = new List<LichHocDto>();
        _lichHocController = LichHocController.GetInstance();
        _hocPhanController =  HocPhanController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _lopController = LopController.GetInstance();
        _title = title;
        _dialogType = dialogType;
        _hocKy = hocKy;
        _nam = nam;
        _selectedId =  index;
        _currentListNhp = currentListNhp;
        Init();
    }

    void Init()
    {
        Size = new Size(1300, 700);

        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        
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
        
        this.Controls.Add(_mainLayout);

        if (_dialogType == DialogType.Sua)
        {
            SetupUpdate();
        }
        else if (_dialogType == DialogType.ChiTiet)
        {
            SetupDetail();
        }
        
    }

 

    void SetContent()
    {
        _contentPanel = new MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
        };
        
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

        SetFieldContainer();
        SetLichHocContainer();
        
        _mainLayout.Controls.Add(_contentPanel);
        
    }

    void SetFieldContainer()
    {
        listField.Add(new LabelTextField("Học kỳ", TextFieldType.NormalText));
        listField.Add(new LabelTextField("Năm", TextFieldType.NormalText));
        listField.Add(new LabelTextField("Học phần", TextFieldType.ListBoxHP));
        listField.Add(new LabelTextField("Giảng viên",  TextFieldType.ListBoxGV));
        listField.Add(new LabelTextField("Lớp",  TextFieldType.ListBoxLop));
        listField.Add(new LabelTextField("Sĩ số", TextFieldType.Number));
        
        
        RoundTLP panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            Border = true,
            Margin = new Padding(7),
            Padding = new Padding(7),
        };

        foreach (LabelTextField field in listField)
        {
            panel.Controls.Add(field);
        }
        
        listField[0].SetText(_hocKy + "");
        listField[1].SetText(_nam + "");
        
        listField[0]._field.Enable = false;
        listField[1]._field.Enable = false;
        _contentPanel.Controls.Add(panel);
    }

    void SetLichHocContainer()
    {
        _lichContainer = new RoundTLP
        {
            Border = true,
            Dock = DockStyle.Fill,
            Margin = new Padding(7),
            RowCount = 3,
            Padding = new Padding(7),
        };

        Label lblLich = new Label
        {
            Text = "Lịch học",
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(13, FontType.Black),
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 10, 0, 0),
        };
        
        _lichContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _lichContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _lichContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        _lichContainer.Controls.Add(lblLich);

        _insertButton = new TitleButton("Thêm", "plus.svg");
        // _insertButton.Margin = new Padding(3, 3, 10, 3);
        _insertButton._label.Font = GetFont.GetFont.GetMainFont(11, FontType.ExtraBold);
        _insertButton.Anchor = AnchorStyles.Right;
        _lichContainer.Controls.Add(_insertButton);

        if (_dialogType == DialogType.ChiTiet)
        {
            _insertButton.Visible = false;
        }

        SetTableLichHoc();
        
        
        
        
        _contentPanel.Controls.Add(_lichContainer);
        
    }

    private void SetTableLichHoc()
    {
        string[] header = new[] { "ID", "Thứ", "Tiết BD", "Tiết KT", "Phòng" };
        string[] columnNames = new[] { "ID" ,"Thu", "TietBatDau", "TietKetThuc","TenPH" };
        List<string> columnNamesList = columnNames.ToList();

        if (_dialogType == DialogType.ChiTiet)
        {
            _tableLichHoc = new CustomTable(header.ToList(), columnNamesList, _lichDisplay);
        }
        else
        {
            _tableLichHoc = new CustomTable(header.ToList(), columnNamesList, _lichDisplay, true, true, true);

        }

        _lichContainer.Controls.Add(_tableLichHoc);
    }

    void UpdateDisplayDataLich(List<LichHocDto> dtos)
    {
        int index = 1;
        _lichDisplay = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            ID = index++,
            Thu = x.Thu,
            TietBatDau = x.TietBatDau,
            TietKetThuc = x.TietKetThuc,
            TenPH = _phongHocController.GetPhongHocById(x.MaPH).TenPH
        });
        _tableLichHoc._dataGridView.DataSource = _lichDisplay;
    }
    
    void SetTopBar()
    {
        MyTLP panel = new MyTLP
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
        
        _mainLayout.Controls.Add(panel);
    }
    
    void SetTitleBar()
    {
        MyTLP panel = new MyTLP
        {
            BackColor = MyColor.MainColor,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0),
            Padding = new Padding(0, 10, 0 , 10),
        };
        
        
        Label lbl = GetTitle();
        panel.Controls.Add(lbl);
        _mainLayout.Controls.Add(panel);
    }
    
    Label GetTitle()
    {
        Label title = new Label
        {
            Text = _title,
            AutoSize = true,
            Dock = DockStyle.Fill,
            Anchor = AnchorStyles.None,
            Font = GetFont.GetFont.GetMainFont(16,  FontType.ExtraBold),
            ForeColor = Color.White
        };
        return title;
    }
    
    void SetBottom()
    {
        //Thêm có Đặt lại, Lưu, Hủy
        MyTLP panel = new MyTLP
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
        _insertButton._mouseDown += () => InsertLich();
        _tableLichHoc.OnDetail += i => DetailLich(i);
        _tableLichHoc.OnEdit += i => UpdateLich(i);
        _tableLichHoc.OnDelete += i => DeleteLich(i);

        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            _btnLuu._mouseDown += () => OnClickBtnLuu();

        }
    }

    
    /// //////////////////////LICH/////////////////////////
    void InsertLich()
    {
        _lichDialog = new LichHocDialog("Thêm lịch học", DialogType.Them, listLichTam, _currentListNhp);
        _lichDialog.Finish += dto =>
        {
            listLichTam.Add(dto);
            UpdateDisplayDataLich(listLichTam);
        };
        _lichDialog.ShowDialog();
    }

    void DetailLich(int index)
    {
        LichHocDto lich = GetLichById(index);
        _lichDialog = new LichHocDialog("Chi tiết lịch học", DialogType.ChiTiet,listLichTam,_currentListNhp, lich);
        _lichDialog.ShowDialog();
    }

    void UpdateLich(int index)
    {
        LichHocDto lich = GetLichById(index);
        _lichDialog = new LichHocDialog("Sửa lịch học", DialogType.Sua, listLichTam,_currentListNhp, lich);
        
        _lichDialog.Finish += dto =>
        {
            foreach (LichHocDto item in listLichTam)
            {
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
            }
            
            UpdateDisplayDataLich(listLichTam);
        };
        _lichDialog.ShowDialog();
    }

    public void DeleteLich(int index)
    {
        LichHocDto lich = GetLichById(index);
        Console.WriteLine("xo");
        
        DialogResult select = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (select == DialogResult.No)
        {
            return;
        }
        
        foreach (LichHocDto item in listLichTam)
        {
            if (item.MaLH == lich.MaLH)
            {
                Console.WriteLine("xooke");
                listLichTam.Remove(item);
                break;
            }
        }
            
        UpdateDisplayDataLich(listLichTam);
    }

    /// //////////////////////LICH/////////////////////////

    /// //////////////////////NHOMHOCPHAN/////////////////////////
    
    
    void SetupUpdate()
    {
        NhomHocPhanDto nhomHP = _nhomHocPhanController.GetById(_selectedId);
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(nhomHP.MaHP);
        GiangVienDto giangVien = _giangVienController.GetById(nhomHP.MaGV);
        LopDto lop = _lopController.GetLopById(nhomHP.MaLop);

        listField[2].tb.contentTextBox.Text = hocPhan.TenHP;
        listField[3].tbGV.contentTextBox.Text = giangVien.TenGV;
        listField[4].tbLop.contentTextBox.Text = lop.TenLop;
        listField[5]._numberField.contentTextBox.Text = nhomHP.SiSo + "";

        SetupListLich();
    }
    
    void SetupDetail()
    {
        NhomHocPhanDto nhomHP = _nhomHocPhanController.GetById(_selectedId);
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(nhomHP.MaHP);
        GiangVienDto giangVien = _giangVienController.GetById(nhomHP.MaGV);
        LopDto lop = _lopController.GetLopById(nhomHP.MaLop);

        listField[2].tb.contentTextBox.Text = hocPhan.TenHP;
        listField[3].tbGV.contentTextBox.Text = giangVien.TenGV;
        listField[4].tbLop.contentTextBox.Text = lop.TenLop;
        listField[5]._numberField.contentTextBox.Text = nhomHP.SiSo + "";
        
        listField[2].tb.Enable = false;
        listField[3].tbGV.Enable = false;
        listField[4].tbLop.Enable = false;
        listField[5]._numberField.Enable = false;

        SetupListLich();
    }

    void SetupListLich()
    {
        List<LichHocDto> listLich = _lichHocController.GetByMaNhp(_selectedId);
        foreach (LichHocDto item in listLich )
        {
            listLichTam.Add(item);
        }
        UpdateDisplayDataLich(listLichTam);
    }
    
    
    
    
    /// //////////////////////NHOMHOCPHAN/////////////////////////
    LichHocDto GetLichById(int ID)
    { 
        return listLichTam[ID - 1];
    }

    void OnClickBtnLuu()
    {
        if (_dialogType == DialogType.Sua)
        {
            Update();
        }
        else if (_dialogType == DialogType.Them)
        {
            Insert();
        }
    }

    void Insert()
    {
        string tenHp = listField[2].tb.contentTextBox.Text;
        string tenGv = listField[3].tbGV.contentTextBox.Text;
        string tenLop = listField[4].tbLop.contentTextBox.Text;
        string sisos = listField[5]._numberField.contentTextBox.Text;
        
        if (!Validate(tenHp,tenGv,tenLop, sisos))
        {
            return;
        }

        HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHp);
        GiangVienDto giangVien = _giangVienController.GetByTen(tenGv);
        LopDto lop = _lopController.GetByTen(tenLop);
        
        int siso = Int16.Parse(sisos);
        int hocKy = int.Parse(_hocKy.Split(" ")[2]);
        string nam = _nam;
        
        NhomHocPhanDto nhomHP = new NhomHocPhanDto
        {
            MaGV = giangVien.MaGV,
            MaHP = hocPhan.MaHP,
            MaLop = lop.MaLop,
            HocKy = hocKy,
            Nam = nam,
            SiSo = siso
        };

        if (!_nhomHocPhanController.Insert(nhomHP))
        {
            MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        List<NhomHocPhanDto> listnhp = _nhomHocPhanController.GetAll();
        int manhp = listnhp[listnhp.Count - 1].MaNHP;

        foreach (LichHocDto item in listLichTam)
        {
            LichHocDto lich = new LichHocDto
            {
                MaPH = item.MaPH,
                MaNHP = manhp,
                Thu = item.Thu,
                TietBatDau = item.TietBatDau,
                TietKetThuc = item.TietKetThuc,
                TuNgay = item.TuNgay,
                DenNgay = item.DenNgay,
                SoTiet = item.SoTiet,
                Type = item.Type,
            };
            if (!_lichHocController.Insert(lich))
            {
                MessageBox.Show("Thêm lịch thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        
        MessageBox.Show("Thêm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Finish?.Invoke();
        this.Close();

    }

    void Update()
    {
        string tenHp = listField[2].tb.contentTextBox.Text;
        string tenGv = listField[3].tbGV.contentTextBox.Text;
        string tenLop = listField[4].tbLop.contentTextBox.Text;
        string sisos = listField[5]._numberField.contentTextBox.Text;
        
        if (!Validate(tenHp,tenGv,tenLop, sisos))
        {
            return;
        }

        HocPhanDto hocPhan = _hocPhanController.GetHocPhanByTen(tenHp);
        GiangVienDto giangVien = _giangVienController.GetByTen(tenGv);
        LopDto lop = _lopController.GetByTen(tenLop);
        
        int siso = Int16.Parse(sisos);
        int hocKy = int.Parse(_hocKy.Split(" ")[2]);
        string nam = _nam;
        
        NhomHocPhanDto nhomHP = new NhomHocPhanDto
        {
            MaNHP = _selectedId,
            MaGV = giangVien.MaGV,
            MaHP = hocPhan.MaHP,
            MaLop = lop.MaLop,
            HocKy = hocKy,
            Nam = nam,
            SiSo = siso
        };

        if (!_nhomHocPhanController.Update(nhomHP))
        {
            MessageBox.Show("Sửa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        //Xóa hết lịch cũ để cập nhật lại
        List<LichHocDto> listLich = _lichHocController.GetByMaNhp(_selectedId);
        foreach (LichHocDto item in listLich)
        {
            _lichHocController.HardDelete(item.MaLH);
        }

        foreach (LichHocDto item in listLichTam)
        {
            LichHocDto lich = new LichHocDto
            {
                MaPH = item.MaPH,
                MaNHP = _selectedId,
                Thu = item.Thu,
                TietBatDau = item.TietBatDau,
                TietKetThuc = item.TietKetThuc,
                TuNgay = item.TuNgay,
                DenNgay = item.DenNgay,
                SoTiet = item.SoTiet,
                Type = item.Type,
            };
            if (!_lichHocController.Insert(lich))
            {
                MessageBox.Show("Sửa lịch thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        
        MessageBox.Show("Sửa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Finish?.Invoke();
        this.Close();
    }

    bool Validate(string tenHP, string tenGV, string tenLop, string siso)
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
        if (siso.Equals(""))
        {
            MessageBox.Show("Sĩ số không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (listLichTam.Count == 0)
        {
            MessageBox.Show("Vui lòng thêm lịch học!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }
    
}