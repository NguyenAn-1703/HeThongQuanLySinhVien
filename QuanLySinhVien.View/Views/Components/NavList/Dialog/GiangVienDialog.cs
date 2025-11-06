using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.AddImg;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class GiangVienDialog : Form
{
    private readonly DialogType _dialogType;
    private readonly int _idGiangVien;
    private readonly List<LabelTextField> _listTextBox;
    private readonly string _title;
    private TitleButton _btnLuu;

    private BtnThemAnh _btnUpimg;

    private MyTLP _contentLayout;
    private CustomButton _exitButton;
    private GiangVienController _GiangVienController;
    private KhoaController _KhoaController;
    private List<InputFormItem> _listIFI;
    private LopController _lopController;
    private MyTLP _mainLayout;
    private PictureBox _pb;
    private TaiKhoanController _taiKhoanController;
    private string imgPath = "";

    public GiangVienDialog(string title, DialogType dialogType, int idGiangVien = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _GiangVienController = GiangVienController.GetInstance();
        _KhoaController = KhoaController.GetInstance();
        _lopController = LopController.GetInstance();
        _taiKhoanController = TaiKhoanController.getInstance();
        _idGiangVien = idGiangVien;
        _title = title;
        _dialogType = dialogType;
        Init();
    }

    public event Action Finish;

    private void Init()
    {
        Width = 900;
        Height = 600;
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
            ColumnCount = 3,
            RowCount = 3,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            Padding = new Padding(10),
            Margin = new Padding(3, 50, 3, 3)
        };

        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
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
            new InputFormItem("Tên giảng viên", TextFieldType.NormalText), //0
            new InputFormItem("Ngày sinh", TextFieldType.Date), //1
            new InputFormItem("Giới tính", TextFieldType.Combobox), //2
            new InputFormItem("Số điện thoại", TextFieldType.NormalText), //3
            new InputFormItem("Email", TextFieldType.NormalText), //4
            new InputFormItem("Tài khoản", TextFieldType.ListBoxTK), //5
            new InputFormItem("Khoa", TextFieldType.Combobox) //6
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
        _listTextBox[2].SetComboboxList(gioiTinh.ToList());
        _listTextBox[2].SetComboboxSelection(gioiTinh[0]);

        List<string> listKh = _KhoaController.GetDanhSachKhoa().Select(x => x.TenKhoa).ToList();
        _listTextBox[6].SetComboboxList(listKh.ToList());
        _listTextBox[6].SetComboboxSelection(listKh[0]);
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
            Text = "Ảnh giảng viên",
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


    private void SetupInsert()
    {
    }

    private void SetupUpdate()
    {
        GiangVienDto giangVien = _GiangVienController.GetById(_idGiangVien);
        _listTextBox[0].SetText(giangVien.TenGV);
        _listTextBox[1]._dField.dateField.Value = ConvertDate.ConvertStringToDate(giangVien.NgaySinhGV);
        _listTextBox[2].SetComboboxSelection(giangVien.GioiTinhGV);
        _listTextBox[3].SetText(giangVien.SoDienThoai);
        _listTextBox[4].SetText(giangVien.Email);
        _listTextBox[5].tbTK.contentTextBox.Text = _taiKhoanController.GetTaiKhoanById(giangVien.MaTK).TenDangNhap;
        _listTextBox[6].SetComboboxSelection(_KhoaController.GetKhoaById(giangVien.MaKhoa).TenKhoa);
        LoadImage(giangVien.AnhDaiDien);
    }

    private void SetupDetail()
    {
        GiangVienDto giangVien = _GiangVienController.GetById(_idGiangVien);
        _listTextBox[0].SetText(giangVien.TenGV);
        _listTextBox[1]._dField.dateField.Value = ConvertDate.ConvertStringToDate(giangVien.NgaySinhGV);
        _listTextBox[2].SetComboboxSelection(giangVien.GioiTinhGV);
        _listTextBox[3].SetText(giangVien.SoDienThoai);
        _listTextBox[4].SetText(giangVien.Email);
        _listTextBox[5].tbTK.contentTextBox.Text = _taiKhoanController.GetTaiKhoanById(giangVien.MaTK).TenDangNhap;
        _listTextBox[6].SetComboboxSelection(_KhoaController.GetKhoaById(giangVien.MaKhoa).TenKhoa);
        LoadImage(giangVien.AnhDaiDien);

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._dField.Enabled = false;
        _listTextBox[2]._combobox.Enabled = false;
        _listTextBox[3]._field.Enable = false;
        _listTextBox[4]._field.Enable = false;
        _listTextBox[5].tbTK.Enable = false;
        _listTextBox[6]._combobox.Enable = false;

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
        var tbTenGV = _listTextBox[0]._field.contentTextBox;
        var tbNgaySinh = _listTextBox[1]._dField.dateField;
        var tbSoDienThoai = _listTextBox[3]._field.contentTextBox;
        var tbEmail = _listTextBox[4]._field.contentTextBox;
        var tbTK = _listTextBox[5].tbTK.contentTextBox;

        var tenGV = tbTenGV.Text;
        var ngaySinh = tbNgaySinh.Text;
        var soDienThoai = tbSoDienThoai.Text;
        var email = tbEmail.Text;
        var taiKhoan = tbTK.Text;
        
        tbTenGV.TabIndex = 1;
        tbNgaySinh.TabIndex = 2;
        tbSoDienThoai.TabIndex = 3;
        tbEmail.TabIndex = 4;
        tbTK.TabIndex = 5;

        var gioiTinh = _listTextBox[2].GetSelectionCombobox();
        int maKhoa = _KhoaController.GetByTen(_listTextBox[6].GetSelectionCombobox()).MaKhoa;

        if (Validate(imgPath,
                tenGV, ngaySinh, soDienThoai,
                email, taiKhoan,
                tbTenGV, tbNgaySinh,
                tbSoDienThoai, tbEmail, tbTK
            ))
        {
            int maTK = _taiKhoanController.GetTaiKhoanByUsrName(taiKhoan).MaTK;

            var giangVien = new GiangVienDto
            {
                TenGV = tenGV,
                NgaySinhGV = ngaySinh,
                GioiTinhGV = gioiTinh,
                SoDienThoai = soDienThoai,
                Email = email,
                MaTK = maTK,
                MaKhoa = maKhoa,
                AnhDaiDien = imgPath
            };
            _GiangVienController.Insert(giangVien);
            MessageBox.Show("Thêm giảng viên thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            Close();
            Finish.Invoke();
        }
    }

    private void Update()
    {
        var tbTenGV = _listTextBox[0]._field.contentTextBox;
        var tbNgaySinh = _listTextBox[1]._dField.dateField;
        var tbSoDienThoai = _listTextBox[3]._field.contentTextBox;
        var tbEmail = _listTextBox[4]._field.contentTextBox;
        var tbTK = _listTextBox[5].tbTK.contentTextBox;

        tbTenGV.TabIndex = 1;
        tbNgaySinh.TabIndex = 2;
        tbSoDienThoai.TabIndex = 3;
        tbEmail.TabIndex = 4;
        tbTK.TabIndex = 5;
        
        var tenGV = tbTenGV.Text;
        var ngaySinh = tbNgaySinh.Text;
        var soDienThoai = tbSoDienThoai.Text;
        var email = tbEmail.Text;
        var taiKhoan = tbTK.Text;

        var gioiTinh = _listTextBox[2].GetSelectionCombobox();
        int maKhoa = _KhoaController.GetByTen(_listTextBox[6].GetSelectionCombobox()).MaKhoa;


        if (Validate(imgPath,
                tenGV, ngaySinh, soDienThoai,
                email, taiKhoan,
                tbTenGV, tbNgaySinh,
                tbSoDienThoai, tbEmail, tbTK
            ))
        {
            int maTK = _taiKhoanController.GetTaiKhoanByUsrName(taiKhoan)!.MaTK;

            var giangVien = new GiangVienDto
            {
                MaGV = _idGiangVien,
                TenGV = tenGV,
                NgaySinhGV = ngaySinh,
                GioiTinhGV = gioiTinh,
                SoDienThoai = soDienThoai,
                Email = email,
                MaTK = maTK,
                MaKhoa = maKhoa,
                AnhDaiDien = imgPath
            };
            _GiangVienController.Update(giangVien);
            MessageBox.Show("Sửa giảng viên thành công!", "Thành công", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            Close();
            Finish.Invoke();
        }
    }


    public bool Validate(
        string imgPath,
        string tenGV, string ngaySinh, string soDienThoai, string email,
        string taiKhoan,
        TextBox tbTenGV, DateTimePicker tbNgaySinh, TextBox tbSoDienThoai,
        TextBox tbEmail, TextBox tbTK)
    {
        
        Dictionary<int, Control> dic = new Dictionary<int, Control>();
        dic.Add(0 , tbTenGV);
        dic.Add(1 , tbNgaySinh);
        dic.Add(2 , tbSoDienThoai);
        dic.Add(3 , tbEmail);
        dic.Add(4 , tbTK);

        ValidateResult rs = _GiangVienController.Validate(imgPath,
            tenGV, ngaySinh, soDienThoai,
            email, taiKhoan);

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