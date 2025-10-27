using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Views.Components.CommonUse.AddImg;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class SinhVienDialog : Form
{
    private string _title;
    private DialogType _dialogType;
    private MyTLP _mainLayout;
    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private SinhVienController _SinhVienController;
    private KhoaHocController _khoaHocController;
    private LopController _lopController;
    private TaiKhoanController _taiKhoanController;
    private int _idSinhVien;
    private TitleButton _btnLuu;
    public event Action Finish;
    private string imgPath = "";

    public SinhVienDialog(string title, DialogType dialogType, int idSinhVien = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _SinhVienController = SinhVienController.GetInstance();
        _khoaHocController = KhoaHocController.GetInstance();
        _lopController = LopController.GetInstance();
        _taiKhoanController = TaiKhoanController.getInstance();
        _idSinhVien = idSinhVien;
        _title = title;
        _dialogType = dialogType;
        Init();
    }

    void Init()
    {
        Width = 1300;
        Height = 650;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new MyTLP()
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = MyColor.LightGray,
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

        _exitButton.MouseDown += (sender, args) => this.Close();
        panel.Controls.Add(_exitButton);

        this._mainLayout.Controls.Add(panel);
    }

    void SetTitleBar()
    {
        MyTLP panel = new MyTLP
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

    private MyTLP _contentLayout;

    void SetContent()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 4,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            Margin = new Padding(3, 50, 3, 3),
        };

        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));


        SetContainerPictureBox();
        SetListField();

        _mainLayout.Controls.Add(_contentLayout);
    }

    void SetListField()
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên sinh viên", TextFieldType.NormalText), //0
            new InputFormItem("Giới tính", TextFieldType.Combobox), //1
            new InputFormItem("Số điện thoại", TextFieldType.NormalText), //2
            new InputFormItem("Ngày sinh", TextFieldType.Date), //3
            new InputFormItem("Email", TextFieldType.NormalText), //4
            new InputFormItem("CCCD", TextFieldType.Number), //5
            new InputFormItem("Quê quán", TextFieldType.NormalText), //6
            new InputFormItem("Trạng thái", TextFieldType.Combobox), //7
            new InputFormItem("Khóa", TextFieldType.Combobox), //8
            new InputFormItem("Lớp", TextFieldType.ListBoxLop), //9
            new InputFormItem("Tài khoản", TextFieldType.ListBoxTK), //10
        };

        _listIFI = arr.ToList();

        foreach (InputFormItem item in _listIFI)
        {
            LabelTextField field = new LabelTextField(item.content, item.type);
            _listTextBox.Add(field);
            _contentLayout.Controls.Add(field);
        }

        SetSelectionCbx();
    }

    void SetSelectionCbx()
    {
        string[] gioiTinh = new[] { "Nữ", "Nam" };
        _listTextBox[1].SetComboboxList(gioiTinh.ToList());
        _listTextBox[1].SetComboboxSelection(gioiTinh[0]);

        string[] trangThai = new[] { "Đang học", "Tốt nghiệp", "Thôi học" };
        _listTextBox[7].SetComboboxList(trangThai.ToList());
        _listTextBox[7].SetComboboxSelection(trangThai[0]);

        List<string> listKh = _khoaHocController.GetAll().Select(x => x.TenKhoaHoc).ToList();
        _listTextBox[8].SetComboboxList(listKh.ToList());
        _listTextBox[8].SetComboboxSelection(listKh[0]);
    }

    private BtnThemAnh _btnUpimg;
    private PictureBox _pb;

    void SetContainerPictureBox()
    {
        _btnUpimg = new BtnThemAnh("Tải lên") { Anchor = AnchorStyles.None };

        MyTLP panel = new MyTLP
        {
            Anchor = AnchorStyles.Top,
            AutoSize = true,
            Margin = new Padding(3, 20, 3, 3),
            RowCount = 3,
        };

        Label title = new Label
        {
            Text = "Ảnh sinh viên",
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.Bold),
            Anchor = AnchorStyles.None
        };

        RoundTLP pbContainer = new RoundTLP { Border = true, AutoSize = true };
        _pb = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(150, 200),
            Image = GetSvgBitmap.GetBitmap("upload.svg"),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = MyColor.White,
            Margin = new Padding(2),
        };


        pbContainer.Controls.Add(_pb);
        panel.Controls.Add(title);
        panel.Controls.Add(pbContainer);
        panel.Controls.Add(_btnUpimg);

        _contentLayout.Controls.Add(panel);

        _contentLayout.SetRowSpan(panel, 5);
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
            panel.Controls.Add(new Panel { Height = 0 });

            _btnLuu = new TitleButton("Lưu");
            panel.Controls.Add(_btnLuu);

            TitleButton btnHuy = new TitleButton("Hủy");
            btnHuy._mouseDown += () => this.Close();

            panel.Controls.Add(btnHuy);
        }
        else
        {
            panel.Controls.Add(new Panel { Height = 0 });

            TitleButton btnThoat = new TitleButton("Thoát");
            btnThoat._mouseDown += () => this.Close();
            panel.Controls.Add(btnThoat, 2, 0);
        }

        this._mainLayout.Controls.Add(panel, 0, 3);
    }


    //Set dữ liệu có sẵn hoặc những dòng không được chọn
    void SetupInsert()
    {
    }

    void SetupUpdate()
    {
        SinhVienDTO sinhVien = _SinhVienController.GetById(_idSinhVien);
        _listTextBox[0].SetText(sinhVien.TenSinhVien);
        _listTextBox[1].SetComboboxSelection(sinhVien.GioiTinh);
        _listTextBox[2].SetText(sinhVien.SdtSinhVien);
        _listTextBox[3]._dField.dateField.Value = ConvertDate.ConvertStringToDate(sinhVien.NgaySinh);
        _listTextBox[4].SetText(sinhVien.Email);
        _listTextBox[5]._numberField.contentTextBox.Text = sinhVien.CCCD;
        _listTextBox[6].SetText(sinhVien.QueQuanSinhVien);
        _listTextBox[7].SetComboboxSelection(sinhVien.TrangThai);
        _listTextBox[8].SetComboboxSelection(_khoaHocController.GetKhoaHocById(sinhVien.MaKhoaHoc).TenKhoaHoc);
        _listTextBox[9].tbLop.contentTextBox.Text = _lopController.GetLopById(sinhVien.MaLop).TenLop;
        _listTextBox[10].tbTK.contentTextBox.Text = _taiKhoanController.GetTaiKhoanById(sinhVien.MaTk).TenDangNhap;
        LoadImage(sinhVien.AnhDaiDienSinhVien);
    }

    void SetupDetail()
    {
        SinhVienDTO sinhVien = _SinhVienController.GetById(_idSinhVien);
        _listTextBox[0].SetText(sinhVien.TenSinhVien);
        _listTextBox[1].SetComboboxSelection(sinhVien.GioiTinh);
        _listTextBox[2].SetText(sinhVien.SdtSinhVien);
        _listTextBox[3]._dField.dateField.Value = ConvertDate.ConvertStringToDate(sinhVien.NgaySinh);
        _listTextBox[4].SetText(sinhVien.Email);
        _listTextBox[5]._numberField.contentTextBox.Text = sinhVien.CCCD;
        _listTextBox[6].SetText(sinhVien.QueQuanSinhVien);
        _listTextBox[7].SetComboboxSelection(sinhVien.TrangThai);
        _listTextBox[8].SetComboboxSelection(_khoaHocController.GetKhoaHocById(sinhVien.MaKhoaHoc).TenKhoaHoc);
        _listTextBox[9].tbLop.contentTextBox.Text = _lopController.GetLopById(sinhVien.MaLop).TenLop;
        _listTextBox[10].tbTK.contentTextBox.Text = _taiKhoanController.GetTaiKhoanById(sinhVien.MaTk).TenDangNhap;
        LoadImage(sinhVien.AnhDaiDienSinhVien);

        
        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._combobox.Enabled = false;
        _listTextBox[2]._field.Enable = false;
        _listTextBox[3]._dField.Enabled = false;
        _listTextBox[4]._field.Enable = false;
        _listTextBox[5]._numberField.Enable = false;
        _listTextBox[6]._field.Enable = false;
        _listTextBox[7]._combobox.Enabled = false;
        _listTextBox[8]._combobox.Enabled = false;
        _listTextBox[9].tbLop.Enable = false;
        _listTextBox[10].tbTK.Enable = false;

        _btnUpimg.Visible = false;
    }

    void SetAction()
    {
        if (_dialogType == DialogType.Them)
        {
            _btnLuu._mouseDown += () => Insert();
        }

        if (_dialogType == DialogType.Sua)
        {
            _btnLuu._mouseDown += () => Update();
        }

        _btnUpimg.OnClickAddImg += s => LoadImage(s);
    }

    void LoadImage(string s)
    {
        imgPath = s;
        _pb.Image = Image.FromFile(changeToAbsolutePath(imgPath));
    }

    string changeToAbsolutePath(string path)
    {
        return Path.Combine(Application.StartupPath, path.Replace("/", "\\"));
    }

    void Insert()
    {
        TextBox tbTenSV = _listTextBox[0]._field.contentTextBox;
        TextBox tbSoDT = _listTextBox[2]._field.contentTextBox;
        DateTimePicker tbNgaySinh = _listTextBox[3].GetDField();
        TextBox tbEmail = _listTextBox[4]._field.contentTextBox;
        TextBox tbCCCD = _listTextBox[5]._numberField.contentTextBox;
        TextBox tbQueQuan = _listTextBox[6]._field.contentTextBox;
        TextBox tbTenLop = _listTextBox[9].tbLop.contentTextBox;
        TextBox tbTenTK = _listTextBox[10].tbTK.contentTextBox;


        // public int MaSinhVien { get; set; }
        // public string TenSinhVien { get; set; }
        // public string NgaySinh { get; set; }
        // public string GioiTinh { get; set; }
        // public string Nganh { get; set; }
        // public string TrangThai { get; set; }
        // public int MaKhoaHoc { get; set; }
        // public int MaLop { get; set; }
        // public int? MaTk { get; set; }
        // public string SdtSinhVien { get; set; }
        // public string QueQuanSinhVien { get; set; }
        // public string Email { get; set; }
        // public string CCCD { get; set; }
        // public string AnhDaiDienSinhVien { get; set; }

        string tenSV = _listTextBox[0].GetTextTextField(); //
        string gioiTinh = _listTextBox[1].GetSelectionCombobox();
        string soDT = _listTextBox[2]._field.contentTextBox.Text; //
        string ngaySinh = _listTextBox[3].GetDField().Text; //
        string email = _listTextBox[4].GetTextTextField(); //
        string cccd = _listTextBox[5]._numberField.contentTextBox.Text; //
        string queQuan = _listTextBox[6].GetTextTextField(); //
        string trangThai = _listTextBox[7].GetSelectionCombobox();
        string tenKH = _listTextBox[8].GetSelectionCombobox();
        string tenLop = _listTextBox[9].tbLop.contentTextBox.Text; //
        string tenTK = _listTextBox[10].tbTK.contentTextBox.Text; //

        if (Validate(imgPath,
                tenSV, soDT, ngaySinh,
                email, cccd, queQuan,
                tenLop, tenTK,
                tbTenSV, tbSoDT, tbNgaySinh,
                tbEmail, tbCCCD, tbQueQuan,
                tbTenLop, tbTenTK
            ))
        {
            SinhVienDTO sinhVien = new SinhVienDTO
            {
                TenSinhVien = tenSV,
                GioiTinh = gioiTinh,
                SdtSinhVien = soDT,
                NgaySinh = ngaySinh,
                Email = email,
                CCCD = cccd,
                QueQuanSinhVien = queQuan,
                TrangThai = trangThai,
                MaKhoaHoc = _khoaHocController.GetByTen(tenKH).MaKhoaHoc,
                MaLop = _lopController.GetByTen(tenLop).MaLop,
                MaTk = _taiKhoanController.GetTaiKhoanByUsrName(tenTK).MaTK,
                AnhDaiDienSinhVien = imgPath,
            };
            if (!_SinhVienController.AddSinhVien(sinhVien))
            {
                MessageBox.Show("Thêm sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Thêm sinh viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            Finish.Invoke();
        }
    }

    void Update()
    {
        TextBox tbTenSV = _listTextBox[0]._field.contentTextBox;
        TextBox tbSoDT = _listTextBox[2]._field.contentTextBox;
        DateTimePicker tbNgaySinh = _listTextBox[3].GetDField();
        TextBox tbEmail = _listTextBox[4]._field.contentTextBox;
        TextBox tbCCCD = _listTextBox[5]._numberField.contentTextBox;
        TextBox tbQueQuan = _listTextBox[6]._field.contentTextBox;
        TextBox tbTenLop = _listTextBox[9].tbLop.contentTextBox;
        TextBox tbTenTK = _listTextBox[10].tbTK.contentTextBox;

        string tenSV = _listTextBox[0].GetTextTextField(); //
        string gioiTinh = _listTextBox[1].GetSelectionCombobox();
        string soDT = _listTextBox[2]._field.contentTextBox.Text; //
        string ngaySinh = _listTextBox[3].GetDField().Text; //
        string email = _listTextBox[4].GetTextTextField(); //
        string cccd = _listTextBox[5]._numberField.contentTextBox.Text; //
        string queQuan = _listTextBox[6].GetTextTextField(); //
        string trangThai = _listTextBox[7].GetSelectionCombobox();
        string tenKH = _listTextBox[8].GetSelectionCombobox();
        string tenLop = _listTextBox[9].tbLop.contentTextBox.Text; //
        string tenTK = _listTextBox[10].tbTK.contentTextBox.Text; //

        if (Validate(imgPath,
                tenSV, soDT, ngaySinh,
                email, cccd, queQuan,
                tenLop, tenTK,
                tbTenSV, tbSoDT, tbNgaySinh,
                tbEmail, tbCCCD, tbQueQuan,
                tbTenLop, tbTenTK
            ))
        {
            SinhVienDTO sinhVien = new SinhVienDTO
            {
                MaSinhVien = _idSinhVien,
                TenSinhVien = tenSV,
                GioiTinh = gioiTinh,
                SdtSinhVien = soDT,
                NgaySinh = ngaySinh,
                Email = email,
                CCCD = cccd,
                QueQuanSinhVien = queQuan,
                TrangThai = trangThai,
                MaKhoaHoc = _khoaHocController.GetByTen(tenKH).MaKhoaHoc,
                MaLop = _lopController.GetByTen(tenLop).MaLop,
                MaTk = _taiKhoanController.GetTaiKhoanByUsrName(tenTK).MaTK,
                AnhDaiDienSinhVien = imgPath,
            };
            if (!_SinhVienController.EditSinhVien(sinhVien))
            {
                MessageBox.Show("Sửa sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Sửa sinh viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            Finish.Invoke();
        }
    }

    

    bool Validate(
        string imgPath, string tenSV, string soDT, string ngaySinh,
        string email, string cccd, string queQuan,
        string tenLop, string tenTK,
        TextBox tbTenSV, TextBox tbSoDT, DateTimePicker tbNgaySinh,
        TextBox tbEmail, TextBox tbCCCD, TextBox tbQueQuan,
        TextBox tbTenLop, TextBox tbTenTK)
    {
        DateTime homNay = DateTime.Now;
        DateTime ngaySinhDate = ConvertDate.ConvertStringToDate(ngaySinh);
        int tuoi = homNay.Year - ngaySinhDate.Year;

        if (homNay.Month < ngaySinhDate.Month ||
            (homNay.Month == ngaySinhDate.Month && homNay.Day < ngaySinhDate.Day))
        {
            tuoi--;
        }

        if (CommonUse.Validate.IsEmpty(imgPath))
        {
            MessageBox.Show("Vui lòng thêm ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTenSV.Focus();
            tbTenSV.SelectAll();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(tenSV))
        {
            MessageBox.Show("Tên sinh viên không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTenSV.Focus();
            tbTenSV.SelectAll();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(soDT))
        {
            MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbSoDT.Focus();
            tbSoDT.SelectAll();
            return false;
        }

        if (!CommonUse.Validate.IsValidPhoneNumber(soDT))
        {
            MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbSoDT.Focus();
            tbSoDT.SelectAll();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(ngaySinh))
        {
            MessageBox.Show("Ngày sinh không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbNgaySinh.Focus();
            return false;
        }

        if (tuoi < 17)
        {
            MessageBox.Show("Tuổi sinh viên phải lớn hơn 16!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbNgaySinh.Focus();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(email))
        {
            MessageBox.Show("Email không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbEmail.Focus();
            tbEmail.SelectAll();
            return false;
        }

        if (!CommonUse.Validate.IsValidEmail(email))
        {
            MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbEmail.Focus();
            tbEmail.SelectAll();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(cccd))
        {
            MessageBox.Show("Căn cước công dân không được để trống!", "Lỗi", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            tbCCCD.Focus();
            tbCCCD.SelectAll();
            return false;
        }

        if (!CommonUse.Validate.IsValidCCCD(cccd))
        {
            MessageBox.Show("Căn cước công dân không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbCCCD.Focus();
            tbCCCD.SelectAll();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(queQuan))
        {
            MessageBox.Show("Quê quán không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbQueQuan.Focus();
            tbQueQuan.SelectAll();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(tenLop))
        {
            MessageBox.Show("Tên lớp không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTenLop.Focus();
            tbTenLop.SelectAll();
            return false;
        }

        if (!_lopController.ExistByTen(tenLop))
        {
            MessageBox.Show("Lớp không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTenLop.Focus();
            tbTenLop.SelectAll();
            return false;
        }

        if (CommonUse.Validate.IsEmpty(tenTK))
        {
            MessageBox.Show("Tên tài khoản không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTenTK.Focus();
            tbTenTK.SelectAll();
            return false;
        }

        if (!_taiKhoanController.ExistByTen(tenTK))
        {
            MessageBox.Show("Tài khoản không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTenTK.Focus();
            tbTenTK.SelectAll();
            return false;
        }

        return true;
    }
}