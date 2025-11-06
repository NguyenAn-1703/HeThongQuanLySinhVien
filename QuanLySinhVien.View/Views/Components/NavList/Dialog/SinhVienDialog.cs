using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.AddImg;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class SinhVienDialog : Form
{
    private readonly DialogType _dialogType;
    private readonly int _idSinhVien;
    private readonly List<LabelTextField> _listTextBox;
    private readonly string _title;
    private TitleButton _btnLuu;

    private BtnThemAnh _btnUpimg;

    private MyTLP _contentLayout;
    private CustomButton _exitButton;
    private KhoaHocController _khoaHocController;
    private List<InputFormItem> _listIFI;
    private LopController _lopController;
    private MyTLP _mainLayout;
    private PictureBox _pb;
    private SinhVienController _SinhVienController;
    private TaiKhoanController _taiKhoanController;
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

    public event Action Finish;

    private void Init()
    {
        Width = 1300;
        Height = 650;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = MyColor.LightGray
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
            ColumnCount = 4,
            RowCount = 4,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            Margin = new Padding(3, 50, 3, 3)
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

    private void SetListField()
    {
        InputFormItem[] arr = new[]
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
            new InputFormItem("Tài khoản", TextFieldType.ListBoxTK) //10
        };

        _listIFI = arr.ToList();

        foreach (InputFormItem item in _listIFI)
        {
            var field = new LabelTextField(item.content, item.type);
            _listTextBox.Add(field);
            _contentLayout.Controls.Add(field);
        }

        SetSelectionCbx();
    }

    private void SetSelectionCbx()
    {
        var gioiTinh = new[] { "Nữ", "Nam" };
        _listTextBox[1].SetComboboxList(gioiTinh.ToList());
        _listTextBox[1].SetComboboxSelection(gioiTinh[0]);

        var trangThai = new[] { "Đang học", "Tốt nghiệp", "Thôi học" };
        _listTextBox[7].SetComboboxList(trangThai.ToList());
        _listTextBox[7].SetComboboxSelection(trangThai[0]);

        List<string> listKh = _khoaHocController.GetAll().Select(x => x.TenKhoaHoc).ToList();
        _listTextBox[8].SetComboboxList(listKh.ToList());
        _listTextBox[8].SetComboboxSelection(listKh[0]);
    }

    private void SetContainerPictureBox()
    {
        _btnUpimg = new BtnThemAnh("Tải lên") { Anchor = AnchorStyles.None };

        var panel = new MyTLP
        {
            Anchor = AnchorStyles.Top,
            AutoSize = true,
            Margin = new Padding(3, 20, 3, 3),
            RowCount = 3
        };

        var title = new Label
        {
            Text = "Ảnh sinh viên",
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.Bold),
            Anchor = AnchorStyles.None
        };

        var pbContainer = new RoundTLP { Border = true, AutoSize = true };
        _pb = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(150, 200),
            Image = GetSvgBitmap.GetBitmap("upload.svg"),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = MyColor.White,
            Margin = new Padding(2)
        };


        pbContainer.Controls.Add(_pb);
        panel.Controls.Add(title);
        panel.Controls.Add(pbContainer);
        panel.Controls.Add(_btnUpimg);

        _contentLayout.Controls.Add(panel);

        _contentLayout.SetRowSpan(panel, 5);
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


    //Set dữ liệu có sẵn hoặc những dòng không được chọn
    private void SetupInsert()
    {
    }

    private void SetupUpdate()
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

    private void SetupDetail()
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

    private void SetAction()
    {
        if (_dialogType == DialogType.Them) _btnLuu._mouseDown += () => Insert();

        if (_dialogType == DialogType.Sua) _btnLuu._mouseDown += () => Update();

        _btnUpimg.OnClickAddImg += s => LoadImage(s);
    }

    private void LoadImage(string s)
    {
        imgPath = s;
        _pb.Image = Image.FromFile(changeToAbsolutePath(imgPath));
    }

    private string changeToAbsolutePath(string path)
    {
        return Path.Combine(Application.StartupPath, path.Replace("/", "\\"));
    }

    private void Insert()
    {
        var tbTenSV = _listTextBox[0]._field.contentTextBox;
        var tbSoDT = _listTextBox[2]._field.contentTextBox;
        var tbNgaySinh = _listTextBox[3].GetDField();
        var tbEmail = _listTextBox[4]._field.contentTextBox;
        var tbCCCD = _listTextBox[5]._numberField.contentTextBox;
        var tbQueQuan = _listTextBox[6]._field.contentTextBox;
        var tbTenLop = _listTextBox[9].tbLop.contentTextBox;
        var tbTenTK = _listTextBox[10].tbTK.contentTextBox;

        tbTenSV.TabIndex = 1;
        tbSoDT.TabIndex = 2;
        tbNgaySinh.TabIndex = 3;
        tbEmail.TabIndex = 4;
        tbCCCD.TabIndex = 5;
        tbQueQuan.TabIndex = 6;
        tbTenLop.TabIndex = 7;
        tbTenTK.TabIndex = 8;
        

        var tenSV = _listTextBox[0].GetTextTextField(); //
        var gioiTinh = _listTextBox[1].GetSelectionCombobox();
        var soDT = _listTextBox[2]._field.contentTextBox.Text; //
        var ngaySinh = _listTextBox[3].GetDField().Text; //
        var email = _listTextBox[4].GetTextTextField(); //
        var cccd = _listTextBox[5]._numberField.contentTextBox.Text; //
        var queQuan = _listTextBox[6].GetTextTextField(); //
        var trangThai = _listTextBox[7].GetSelectionCombobox();
        var tenKH = _listTextBox[8].GetSelectionCombobox();
        var tenLop = _listTextBox[9].tbLop.contentTextBox.Text; //
        var tenTK = _listTextBox[10].tbTK.contentTextBox.Text; //

        if (Validate(imgPath,
                tenSV, soDT, ngaySinh,
                email, cccd, queQuan,
                tenLop, tenTK,
                tbTenSV, tbSoDT, tbNgaySinh,
                tbEmail, tbCCCD, tbQueQuan,
                tbTenLop, tbTenTK
            ))
        {
            var sinhVien = new SinhVienDTO
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
                AnhDaiDienSinhVien = imgPath
            };
            if (!_SinhVienController.AddSinhVien(sinhVien))
            {
                MessageBox.Show("Thêm sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Thêm sinh viên thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            Close();
            Finish.Invoke();
        }
    }

    private void Update()
    {
        var tbTenSV = _listTextBox[0]._field.contentTextBox;
        var tbSoDT = _listTextBox[2]._field.contentTextBox;
        var tbNgaySinh = _listTextBox[3].GetDField();
        var tbEmail = _listTextBox[4]._field.contentTextBox;
        var tbCCCD = _listTextBox[5]._numberField.contentTextBox;
        var tbQueQuan = _listTextBox[6]._field.contentTextBox;
        var tbTenLop = _listTextBox[9].tbLop.contentTextBox;
        var tbTenTK = _listTextBox[10].tbTK.contentTextBox;

        var tenSV = _listTextBox[0].GetTextTextField(); //
        var gioiTinh = _listTextBox[1].GetSelectionCombobox();
        var soDT = _listTextBox[2]._field.contentTextBox.Text; //
        var ngaySinh = _listTextBox[3].GetDField().Text; //
        var email = _listTextBox[4].GetTextTextField(); //
        var cccd = _listTextBox[5]._numberField.contentTextBox.Text; //
        var queQuan = _listTextBox[6].GetTextTextField(); //
        var trangThai = _listTextBox[7].GetSelectionCombobox();
        var tenKH = _listTextBox[8].GetSelectionCombobox();
        var tenLop = _listTextBox[9].tbLop.contentTextBox.Text; //
        var tenTK = _listTextBox[10].tbTK.contentTextBox.Text; //
        
        tbTenSV.TabIndex = 1;
        tbSoDT.TabIndex = 2;
        tbNgaySinh.TabIndex = 3;
        tbEmail.TabIndex = 4;
        tbCCCD.TabIndex = 5;
        tbQueQuan.TabIndex = 6;
        tbTenLop.TabIndex = 7;
        tbTenTK.TabIndex = 8;

        if (Validate(imgPath,
                tenSV, soDT, ngaySinh,
                email, cccd, queQuan,
                tenLop, tenTK,
                tbTenSV, tbSoDT, tbNgaySinh,
                tbEmail, tbCCCD, tbQueQuan,
                tbTenLop, tbTenTK
            ))
        {
            var sinhVien = new SinhVienDTO
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
                AnhDaiDienSinhVien = imgPath
            };
            if (!_SinhVienController.EditSinhVien(sinhVien))
            {
                MessageBox.Show("Sửa sinh viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Sửa sinh viên thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            Close();
            Finish.Invoke();
        }
    }


    private bool Validate(
        string imgPath, string tenSV, string soDT, string ngaySinh,
        string email, string cccd, string queQuan,
        string tenLop, string tenTK,
        TextBox tbTenSV, TextBox tbSoDT, DateTimePicker tbNgaySinh,
        TextBox tbEmail, TextBox tbCCCD, TextBox tbQueQuan,
        TextBox tbTenLop, TextBox tbTenTK)
    {
        Dictionary<int, Control> dic = new Dictionary<int, Control>();
        dic.Add(0 , tbTenSV);
        dic.Add(1 , tbSoDT);
        dic.Add(2 , tbNgaySinh);
        dic.Add(3 , tbEmail);
        dic.Add(4 , tbCCCD);
        dic.Add(5 , tbQueQuan);
        dic.Add(6 , tbTenLop);
        dic.Add(7 , tbTenTK);

        ValidateResult rs = _SinhVienController.Validate(imgPath, tenSV, soDT, ngaySinh,
            email, cccd, queQuan,
            tenLop, tenTK);

        if (rs.index == -1)
        {
            return true;
        }
        
        MessageBox.Show(rs.message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        Control c = dic.TryGetValue(rs.index, out Control c2) ? c2 : new Control();
        c.Focus();
        if (c is TextBoxBase tb)
        {
            tb.SelectAll();
        }

        return false;

    }
}