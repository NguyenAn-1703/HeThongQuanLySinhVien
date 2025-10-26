using QuanLySinhVien.Views.Components.CommonUse.GiangVien;
using QuanLySinhVien.Views.Components.CommonUse.Lop;
using QuanLySinhVien.Views.Components.CommonUse.Nganh;
using QuanLySinhVien.Views.Components.CommonUse.PhongHoc;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

//Gồm title + textbox cho các form
public class LabelTextField : MyTLP
{
    private string _title;
    private TextFieldType _fieldType;
    public CustomTextBox _field; // normal Field
    public CustomTextBox _password;
    public CustomTextBox _numberField;
    public CustomCombobox _combobox;
    
    private PictureBox _eyePb;
    private bool statusEp = false;
    
    public CustomDateField _dTNgayField;
    public CustomDateField _dTGioField;

    public DateTimePicker _namField;

    public CustomCombobox _listBox;

    public CustomDateField _dField;
//
    
    
    public LabelTextField(string title, TextFieldType fieldType)
    {
        _title = title;
        _fieldType = fieldType;
        Init();
    }

    void Init()
    {
        
        this.RowCount = 2;
        this.AutoSize = true;
        this.Dock = DockStyle.Fill;
        
        
        Label label = new Label();
        label.Text = this._title;
        label.AutoSize = true;
        label.Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold);

        this.Controls.Add(label);

        SetField();
    }

    void SetField()
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
            case  TextFieldType.ListBoxPH:
                SetListBoxPH();
                break;
            case  TextFieldType.ListBoxLop:
                SetListBoxLop();
                break;
            case  TextFieldType.ListBoxNganh:
                SetListBoxNganh();
                break;
            case  TextFieldType.Number:
                SetListBoxNumber();
                break;
            
            
        }
    }

    void SetNormalTextField()
    {
        _field = new CustomTextBox();
        _field.Dock = DockStyle.Top;
        _field.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
        this.Controls.Add(_field);
    }

    void SetPasswordTextField()
    {
        _password = new CustomTextBox();
        _password.Dock = DockStyle.Top;
        _password.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
        _password.contentTextBox.PasswordChar = '*';
        setEyeButton();
        this.Controls.Add(_password);
    }
    
    void SetComboboxField()
    {
        _combobox = new CustomCombobox(new string[0]);
        _combobox.Dock = DockStyle.Top;
        _combobox.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
        this.Controls.Add(_combobox);
    }

    void SetDateTimeField()
    {
        MyTLP panel = new  MyTLP
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            AutoSize = true,
        };
        _dTNgayField = new CustomDateField
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };

        this.Controls.Add(_dTNgayField);
        
        _dTGioField = new CustomDateField()
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };
        _dTGioField.dateField.Format = DateTimePickerFormat.Custom; 
        _dTGioField.dateField.CustomFormat = " HH:mm:ss";
        
        _dTGioField.dateField.ShowUpDown = true;
        panel.Controls.Add(_dTNgayField);
        panel.Controls.Add(_dTGioField);
        this.Controls.Add(panel);
        
    }

    void SetDateField()
    {
        _dField = new CustomDateField
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Regular),
            AutoSize = true,
            Dock = DockStyle.Top
        };


        this.Controls.Add(_dField);
    }
    

    void SetNamField()
    {
        _namField = new DateTimePicker();
        _namField = new DateTimePicker();
        _namField.Format = DateTimePickerFormat.Custom;
        _namField.CustomFormat = "yyyy";
        _namField.ShowUpDown = true; 
        this.Controls.Add(_namField);
    }

    public CustomSearchFieldHP tb;
    void SetListBox()
    {
        tb =  new CustomSearchFieldHP();
        tb.Dock  = DockStyle.Top;
        this.Controls.Add(tb);
        
    }
    
    public CustomSearchFieldGV tbGV;
    void SetListBoxGV()
    {
        tbGV =  new CustomSearchFieldGV();
        tbGV.Dock  = DockStyle.Top;
        this.Controls.Add(tbGV);
    }
    
    public CustomSearchFieldPH tbPH;
    void SetListBoxPH()
    {
        tbPH =  new CustomSearchFieldPH();
        tbPH.Dock  = DockStyle.Top;
        this.Controls.Add(tbPH);
    }
    
    public CustomSearchFieldLop tbLop;
    void SetListBoxLop()
    {
        tbLop =  new CustomSearchFieldLop();
        tbLop.Dock  = DockStyle.Top;
        this.Controls.Add(tbLop);
    }
    
    public CustomSearchFieldNG tbNganh;
    void SetListBoxNganh()
    {
        tbNganh =  new CustomSearchFieldNG();
        tbNganh.Dock  = DockStyle.Top;
        this.Controls.Add(tbNganh);
    }
    
    


    void SetListBoxNumber()
    {
        _numberField = new CustomTextBox();
        _numberField.Dock = DockStyle.Top;
        _numberField.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
        this.Controls.Add(_numberField);

        _numberField.contentTextBox.KeyPress += (sender, args) =>
        {
            if (!char.IsControl(args.KeyChar) && !char.IsDigit(args.KeyChar))
            {
                args.Handled = true; // Chặn ký tự
            }
        };
    }
    

    public string GetTextNam()
    {
        return _namField.Text;
    }
    
    

    void setEyeButton()
    {

        _eyePb = new PictureBox
        {
            Size = new Size(25, 25), 
            SizeMode = PictureBoxSizeMode.Zoom, 
            Image = GetSvgBitmap.GetBitmap("eye-close.svg"),
        };
        _eyePb.Anchor = AnchorStyles.None;
        _eyePb.Cursor = Cursors.Hand;
        
        this._password.Controls.Add(_eyePb);    
        
        _eyePb.MouseEnter += (sender, args) => {_eyePb.BackColor = MyColor.GrayHoverColor;};
        _eyePb.MouseLeave += (sender, args) => {_eyePb.BackColor = MyColor.White;};
        _eyePb.MouseClick += (sender, args) => { onClickEyeBtn();};

        foreach (Control c in _eyePb.Controls)
        {
            c.MouseEnter += (sender, args) => {_eyePb.BackColor = MyColor.GrayHoverColor;};
            c.MouseLeave += (sender, args) => {_eyePb.BackColor = MyColor.White;};
            c.MouseClick += (sender, args) => { onClickEyeBtn();};
            c.Cursor = Cursors.Hand;
        }

    }

    void onClickEyeBtn()
    {
        //mở -> đóng
        if (statusEp)
        {
            _eyePb.Image = GetSvgBitmap.GetBitmap("eye-close.svg");
            this._password.contentTextBox.PasswordChar = '*';
            statusEp = false;
        }
        // đóng -> mở
        else
        {
            _eyePb.Image = GetSvgBitmap.GetBitmap("eye-open.svg");
            this._password.contentTextBox.PasswordChar = '\0';
            statusEp = true;
        }
    }

    public void SetComboboxList(List<string> list)
    {
        this._combobox.combobox.Items.Clear();
        foreach (string s in list)
        {
            this._combobox.combobox.Items.Add(s);
        }
    }
    public void SetComboboxSelection(string input)
    {
        this._combobox.SetSelectionCombobox(input);
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