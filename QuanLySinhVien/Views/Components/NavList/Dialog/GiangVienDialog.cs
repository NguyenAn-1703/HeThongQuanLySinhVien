using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.AddImg;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class GiangVienDialog : Form
{
    private string _title;
    private DialogType _dialogType;
    private MyTLP _mainLayout;
    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private GiangVienController _GiangVienController;
    private KhoaController _KhoaController;
    private LopController _lopController;
    private TaiKhoanController _taiKhoanController;
    private int _idGiangVien;
    private TitleButton _btnLuu;
    public event Action Finish;
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

    void Init()
    {
        Width = 900;
        Height = 600;
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
            ColumnCount = 3,
            RowCount = 3,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            Padding = new Padding(10),
            Margin = new Padding(3, 50, 3, 3),
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

    void SetListField()
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên giảng viên", TextFieldType.NormalText), //0
            new InputFormItem("Ngày sinh", TextFieldType.Date), //1
            new InputFormItem("Giới tính", TextFieldType.Combobox), //2
            new InputFormItem("Số điện thoại", TextFieldType.NormalText), //3
            new InputFormItem("Email", TextFieldType.NormalText), //4
            new InputFormItem("Tài khoản", TextFieldType.ListBoxTK), //5
            new InputFormItem("Khoa", TextFieldType.Combobox), //6
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
        _listTextBox[2].SetComboboxList(gioiTinh.ToList());
        _listTextBox[2].SetComboboxSelection(gioiTinh[0]);

        List<string> listKh = _KhoaController.GetDanhSachKhoa().Select(x => x.TenKhoa).ToList();
        _listTextBox[6].SetComboboxList(listKh.ToList());
        _listTextBox[6].SetComboboxSelection(listKh[0]);
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
            Text = "Ảnh giảng viên",
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


    void SetupInsert()
    {
    }

    void SetupUpdate()
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

    void SetupDetail()
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

        TextBox tbTenGV = _listTextBox[0]._field.contentTextBox;
        DateTimePicker tbNgaySinh = _listTextBox[1]._dField.dateField;
        TextBox tbSoDienThoai = _listTextBox[3]._field.contentTextBox;
        TextBox tbEmail = _listTextBox[4]._field.contentTextBox;
        TextBox tbTK = _listTextBox[5].tbTK.contentTextBox;

        string tenGV = tbTenGV.Text;
        string ngaySinh = tbNgaySinh.Text;
        string soDienThoai = tbSoDienThoai.Text;
        string email = tbEmail.Text;
        string taiKhoan = tbTK.Text;
        
        string gioiTinh = _listTextBox[2].GetSelectionCombobox();
        int maKhoa = _KhoaController.GetByTen(_listTextBox[6].GetSelectionCombobox()).MaKhoa;

        if (Validate(imgPath,
                tenGV, ngaySinh, soDienThoai,
                email, taiKhoan,
                tbTenGV, tbNgaySinh,
                tbSoDienThoai, tbEmail, tbTK
            ))
        {
            int maTK = _taiKhoanController.GetTaiKhoanByUsrName(taiKhoan).MaTK;
            
            GiangVienDto giangVien = new GiangVienDto
            {
                TenGV = tenGV,
                NgaySinhGV = ngaySinh,
                GioiTinhGV = gioiTinh,
                SoDienThoai = soDienThoai,
                Email = email,
                MaTK = maTK,
                MaKhoa = maKhoa,
                AnhDaiDien = imgPath,
            };
            _GiangVienController.Insert(giangVien);
            MessageBox.Show("Thêm giảng viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            Finish.Invoke();
        }
    }

    void Update()
    {

        TextBox tbTenGV = _listTextBox[0]._field.contentTextBox;
        DateTimePicker tbNgaySinh = _listTextBox[1]._dField.dateField;
        TextBox tbSoDienThoai = _listTextBox[3]._field.contentTextBox;
        TextBox tbEmail = _listTextBox[4]._field.contentTextBox;
        TextBox tbTK = _listTextBox[5].tbTK.contentTextBox;

        string tenGV = tbTenGV.Text;
        string ngaySinh = tbNgaySinh.Text;
        string soDienThoai = tbSoDienThoai.Text;
        string email = tbEmail.Text;
        string taiKhoan = tbTK.Text;

        string gioiTinh = _listTextBox[2].GetSelectionCombobox();
        int maKhoa = _KhoaController.GetByTen(_listTextBox[6].GetSelectionCombobox()).MaKhoa;
        

        if (Validate(imgPath,
                tenGV, ngaySinh, soDienThoai,
                email, taiKhoan,
                tbTenGV, tbNgaySinh,
                tbSoDienThoai, tbEmail, tbTK
            ))
        {
            
            int maTK = _taiKhoanController.GetTaiKhoanByUsrName(taiKhoan)!.MaTK;
            
            GiangVienDto giangVien = new GiangVienDto
            {
                MaGV = _idGiangVien,
                TenGV = tenGV,
                NgaySinhGV = ngaySinh,
                GioiTinhGV = gioiTinh,
                SoDienThoai = soDienThoai,
                Email = email,
                MaTK = maTK,
                MaKhoa = maKhoa,
                AnhDaiDien = imgPath,
            };
            _GiangVienController.Update(giangVien);
            MessageBox.Show("Sửa giảng viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
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
        // 1. Kiểm tra ảnh
        if (CommonUse.Validate.IsEmpty(imgPath))
        {
            MessageBox.Show("Vui lòng thêm ảnh giảng viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // 2. Kiểm tra tên giảng viên
        if (CommonUse.Validate.IsEmpty(tenGV))
        {
            MessageBox.Show("Tên giảng viên không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTenGV.Focus();
            tbTenGV.SelectAll();
            return false;
        }

        // 3. Kiểm tra ngày sinh
        if (CommonUse.Validate.IsEmpty(ngaySinh))
        {
            MessageBox.Show("Ngày sinh không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbNgaySinh.Focus();
            return false;
        }

        DateTime ngaySinhDate;
        ngaySinhDate = ConvertDate.ConvertStringToDate(ngaySinh);

        // 4. Kiểm tra tuổi giảng viên (tối thiểu 22 tuổi)
        DateTime homNay = DateTime.Now;
        int tuoi = homNay.Year - ngaySinhDate.Year;
        if (homNay.Month < ngaySinhDate.Month ||
            (homNay.Month == ngaySinhDate.Month && homNay.Day < ngaySinhDate.Day))
        {
            tuoi--;
        }

        if (tuoi < 22)
        {
            MessageBox.Show("Giảng viên phải từ 22 tuổi trở lên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbNgaySinh.Focus();
            return false;
        }

        // 5. Kiểm tra số điện thoại
        if (CommonUse.Validate.IsEmpty(soDienThoai))
        {
            MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbSoDienThoai.Focus();
            tbSoDienThoai.SelectAll();
            return false;
        }

        if (!CommonUse.Validate.IsValidPhoneNumber(soDienThoai))
        {
            MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbSoDienThoai.Focus();
            tbSoDienThoai.SelectAll();
            return false;
        }

        // 6. Kiểm tra email
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

        // 7. Kiểm tra tài khoản
        if (CommonUse.Validate.IsEmpty(taiKhoan))
        {
            MessageBox.Show("Tên tài khoản không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTK.Focus();
            tbTK.SelectAll();
            return false;
        }

        if (!_taiKhoanController.ExistByTen(taiKhoan))
        {
            MessageBox.Show("Tài khoản không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbTK.Focus();
            tbTK.SelectAll();
            return false;
        }

        return true;
    }
}