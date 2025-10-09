using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

//Gồm title + textbox cho các form
public class LabelTextField : TableLayoutPanel
{
    private string _title;
    private TextFieldType _fieldType;
    private CustomTextBox _field; // normal Field
    private CustomTextBox _password;
    private CustomCombobox _combobox;
    
    private PictureBox _eyePb;
    private bool statusEp = false;
    
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
            
        }
    }

    void SetNormalTextField()
    {
        _field = new CustomTextBox();
        _field.Dock = DockStyle.Top;
        _field.Font = GetFont.GetFont.GetMainFont(13, FontType.Regular);
        this.Controls.Add(_field);
    }

    void SetPasswordTextField()
    {
        _password = new CustomTextBox();
        _password.Dock = DockStyle.Top;
        _password.Font = GetFont.GetFont.GetMainFont(13, FontType.Regular);
        _password.contentTextBox.PasswordChar = '*';
        setEyeButton();
        this.Controls.Add(_password);
    }
    
    void SetComboboxField()
    {
        _combobox = new CustomCombobox(new string[0]);
        _combobox.Dock = DockStyle.Top;
        _combobox.Font = GetFont.GetFont.GetMainFont(13, FontType.Regular);
        this.Controls.Add(_combobox);
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
            this._field.contentTextBox.PasswordChar = '*';
            statusEp = false;
        }
        // đóng -> mở
        else
        {
            _eyePb.Image = GetSvgBitmap.GetBitmap("eye-open.svg");
            this._field.contentTextBox.PasswordChar = '\0';
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
    
}