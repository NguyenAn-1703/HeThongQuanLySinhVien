using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Views.Components.CommonUse.GiangVien;
using QuanLySinhVien.Views.Components.CommonUse.Lop;
using QuanLySinhVien.Views.Components.CommonUse.Nganh;
using QuanLySinhVien.Views.Components.CommonUse.PhongHoc;
using QuanLySinhVien.Views.Components.CommonUse.SearchField.TaiKhoan;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

//Gồm title + textbox cho các form
public class LabelTextField : MyTLP
{
    private readonly TextFieldType _fieldType;
    private readonly string _title;
    public CustomCombobox _combobox;

    public CustomDateField _dField;
    public CustomDateField _dTGioField;

    public CustomDateField _dTNgayField;

    private PictureBox _eyePb;
    public CustomTextBox _field; // normal Field


    public CustomDateField _fTime;

    public CustomDateField _fTimeHH;
    public CustomDateField _fTimeMM;

    public CustomCombobox _listBox;

    public DateTimePicker _namField;
    public CustomTextBox _numberField;
    public CustomTextBox _password;
    private bool statusEp;

    public CustomSearchFieldHP tb;

    public CustomSearchFieldGV tbGV;

    public CustomSearchFieldLop tbLop;

    public CustomSearchFieldNG tbNganh;

    public CustomSearchFieldPH tbPH;

    public CustomSearchFieldTK tbTK;
//


    public LabelTextField(string title, TextFieldType fieldType)
    {
        _title = title;
        _fieldType = fieldType;
        Init();
    }

    private void Init()
    {
        RowCount = 2;
        AutoSize = true;
        Dock = DockStyle.Fill;


        var label = new Label();
        label.Text = _title;
        label.AutoSize = true;
        label.Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold);

        Controls.Add(label);

        SetField();
    }

    private void SetField()
    {
        switch (_fieldType)
        {
            case TextFieldType.NormalText:
                SetNormalTextField();
                break;
            case TextFieldType.Password:
                SetPasswordTextField();
                break;
            case TextFieldType.Combobox:
                SetComboboxField();
                break;
            case TextFieldType.DateTime:
                SetDateTimeField();
                break;
            case TextFieldType.Date:
                SetDateField();
                break;
            case TextFieldType.Year:
                SetNamField();
                break;
            case TextFieldType.ListBoxHP:
                SetListBox();
                break;
            case TextFieldType.ListBoxGV:
                SetListBoxGV();
                break;
            case TextFieldType.ListBoxPH:
                SetListBoxPH();
                break;
            case TextFieldType.ListBoxLop:
                SetListBoxLop();
                break;
            case TextFieldType.ListBoxNganh:
                SetListBoxNganh();
                break;
            case TextFieldType.Number:
                SetListBoxNumber();
                break;
            case TextFieldType.ListBoxTK:
                SetListBoxTK();
                break;
            case TextFieldType.Time:
                SetTimeField();
                break;
            case TextFieldType.Timehhmm:
                SetTimeFieldHHMM();
                break;
        }
    }

    private void SetNormalTextField()
    {
        _field = new CustomTextBox { BackColor = MyColor.White };
        _field.Dock = DockStyle.Top;
        _field.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
        Controls.Add(_field);
    }

    private void SetPasswordTextField()
    {
        _password = new CustomTextBox { BackColor = MyColor.White };
        _password.Dock = DockStyle.Top;
        _password.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
        _password.contentTextBox.PasswordChar = '*';
        setEyeButton();
        Controls.Add(_password);
    }

    private void SetComboboxField()
    {
        _combobox = new CustomCombobox(new string[0]);
        _combobox.Dock = DockStyle.Top;
        _combobox.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
        Controls.Add(_combobox);
    }

    private void SetDateTimeField()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            AutoSize = true
        };
        _dTNgayField = new CustomDateField
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };

        Controls.Add(_dTNgayField);

        _dTGioField = new CustomDateField
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };
        _dTGioField.dateField.Format = DateTimePickerFormat.Custom;
        _dTGioField.dateField.CustomFormat = "HH:mm:ss";

        _dTGioField.dateField.ShowUpDown = true;
        panel.Controls.Add(_dTNgayField);
        panel.Controls.Add(_dTGioField);
        Controls.Add(panel);
    }

    private void SetDateField()
    {
        _dField = new CustomDateField
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };


        Controls.Add(_dField);
    }


    private void SetNamField()
    {
        _namField = new DateTimePicker();
        _namField = new DateTimePicker();
        _namField.Format = DateTimePickerFormat.Custom;
        _namField.CustomFormat = "yyyy";
        _namField.ShowUpDown = true;
        Controls.Add(_namField);
    }

    private void SetListBox()
    {
        tb = new CustomSearchFieldHP();
        tb.BackColor = MyColor.White;
        tb.Dock = DockStyle.Top;
        Controls.Add(tb);
    }

    private void SetListBoxGV()
    {
        tbGV = new CustomSearchFieldGV();
        tbGV.BackColor = MyColor.White;
        tbGV.Dock = DockStyle.Top;
        Controls.Add(tbGV);
    }

    private void SetListBoxPH()
    {
        tbPH = new CustomSearchFieldPH();
        tbPH.BackColor = MyColor.White;

        tbPH.Dock = DockStyle.Top;
        Controls.Add(tbPH);
    }

    private void SetListBoxLop()
    {
        tbLop = new CustomSearchFieldLop();
        tbLop.BackColor = MyColor.White;

        tbLop.Dock = DockStyle.Top;
        Controls.Add(tbLop);
    }

    private void SetListBoxNganh()
    {
        tbNganh = new CustomSearchFieldNG();
        tbNganh.BackColor = MyColor.White;

        tbNganh.Dock = DockStyle.Top;
        Controls.Add(tbNganh);
    }

    private void SetListBoxTK()
    {
        tbTK = new CustomSearchFieldTK();
        tbTK.BackColor = MyColor.White;

        tbTK.Dock = DockStyle.Top;
        Controls.Add(tbTK);
    }


    private void SetListBoxNumber()
    {
        _numberField = new CustomTextBox { BackColor = MyColor.White };
        _numberField.Dock = DockStyle.Top;
        _numberField.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
        Controls.Add(_numberField);

        _numberField.contentTextBox.KeyPress += (sender, args) =>
        {
            if (!char.IsControl(args.KeyChar) && !char.IsDigit(args.KeyChar)) args.Handled = true; // Chặn ký tự
        };
    }


    public string GetTextNam()
    {
        return _namField.Text;
    }

    private void SetTimeField()
    {
        _fTime = new CustomDateField
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };
        _fTime.dateField.Format = DateTimePickerFormat.Custom;
        _fTime.dateField.CustomFormat = "HH:mm:ss";

        _fTime.dateField.ShowUpDown = true;
        Controls.Add(_fTime);
    }

    private void SetTimeFieldHHMM()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            AutoSize = true
        };


        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


        var lblGio = new Label
        {
            Text = "giờ",
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            AutoSize = true,
            Margin = new Padding(3, 15, 3, 3)
        };
        var lblPh = new Label
        {
            Text = "phút",
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            AutoSize = true,
            Margin = new Padding(3, 15, 3, 3)
        };

        _fTimeHH = new CustomDateField
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };
        _fTimeHH.dateField.Format = DateTimePickerFormat.Custom;
        _fTimeHH.dateField.CustomFormat = "HH";
        _fTimeHH.dateField.ShowUpDown = true;
        _fTimeHH.dateField.Width = 10;

        _fTimeMM = new CustomDateField
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };
        _fTimeMM.dateField.Format = DateTimePickerFormat.Custom;
        _fTimeMM.dateField.CustomFormat = "mm";
        _fTimeMM.dateField.ShowUpDown = true;
        _fTimeMM.dateField.Width = 10;


        panel.Controls.Add(_fTimeHH);
        panel.Controls.Add(lblGio);
        panel.Controls.Add(_fTimeMM);
        panel.Controls.Add(lblPh);
        Controls.Add(panel);
    }


    private void setEyeButton()
    {
        _eyePb = new PictureBox
        {
            Size = new Size(25, 25),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap("eye-close.svg")
        };
        _eyePb.Anchor = AnchorStyles.None;
        _eyePb.Cursor = Cursors.Hand;

        _password.Controls.Add(_eyePb);

        _eyePb.MouseEnter += (sender, args) => { _eyePb.BackColor = MyColor.GrayHoverColor; };
        _eyePb.MouseLeave += (sender, args) => { _eyePb.BackColor = MyColor.White; };
        _eyePb.MouseClick += (sender, args) => { onClickEyeBtn(); };

        foreach (Control c in _eyePb.Controls)
        {
            c.MouseEnter += (sender, args) => { _eyePb.BackColor = MyColor.GrayHoverColor; };
            c.MouseLeave += (sender, args) => { _eyePb.BackColor = MyColor.White; };
            c.MouseClick += (sender, args) => { onClickEyeBtn(); };
            c.Cursor = Cursors.Hand;
        }
    }

    private void onClickEyeBtn()
    {
        //mở -> đóng
        if (statusEp)
        {
            _eyePb.Image = GetSvgBitmap.GetBitmap("eye-close.svg");
            _password.contentTextBox.PasswordChar = '*';
            statusEp = false;
        }
        // đóng -> mở
        else
        {
            _eyePb.Image = GetSvgBitmap.GetBitmap("eye-open.svg");
            _password.contentTextBox.PasswordChar = '\0';
            statusEp = true;
        }
    }

    public void SetComboboxList(List<string> list)
    {
        _combobox.combobox.Items.Clear();
        foreach (var s in list) _combobox.combobox.Items.Add(s);
    }

    public void SetComboboxSelection(string input)
    {
        _combobox.SetSelectionCombobox(input);
    }

    public void SetText(string input)
    {
        _field.SetText(input);
    }

    public void SetPassword(string input)
    {
        _password.SetText(input);
    }

    public TextBox GetTextField()
    {
        return _field.contentTextBox;
    }

    public TextBox GetPasswordField()
    {
        return _password.contentTextBox;
    }

    public ComboBox GetComboboxField()
    {
        return _combobox.combobox;
    }

    public string GetTextTextField()
    {
        return _field.contentTextBox.Text;
    }

    public string GetTextPasswordField()
    {
        return _password.contentTextBox.Text;
    }

    public string GetSelectionCombobox()
    {
        return _combobox.combobox.SelectedItem.ToString();
    }

    public DateTimePicker GetDField()
    {
        return _dField.dateField;
    }

    public DateTimePicker GetDTNgayField()
    {
        return _dTNgayField.dateField;
    }

    public DateTimePicker GetDTGioField()
    {
        return _dTGioField.dateField;
    }
}