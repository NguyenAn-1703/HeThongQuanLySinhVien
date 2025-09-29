using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

//Gồm title + textbox cho các form
public class LabelTextField : TableLayoutPanel
{
    private string _title;
    private TextFieldType _fieldType;
    private CustomTextBox _field;
    
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

        this.Controls.Add(GetField());
    }

    CustomTextBox GetField()
    {
        _field = new CustomTextBox();
        _field.Dock = DockStyle.Top;
        _field.Font = GetFont.GetFont.GetMainFont(13, FontType.Regular);
        switch (_fieldType)
        {
            case TextFieldType.NormalText:
                break;
            case TextFieldType.Password:
                _field.contentTextBox.PasswordChar = '*';
                setEyeButton();
                break;
            
        }
        return _field;
    }

    void setEyeButton()
    {

        _eyePb = new PictureBox
        {
            Size = new Size(25, 25), 
            SizeMode = PictureBoxSizeMode.Zoom, 
            Image = GetSvgBitmap.GetBitmap("eye-close.svg"),
        };
        _eyePb.Location = new Point(this._field.contentTextBox.Right + 207, 4);
        _eyePb.Cursor = Cursors.Hand;
        
        this._field.Controls.Add(_eyePb);    
        
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
    
}