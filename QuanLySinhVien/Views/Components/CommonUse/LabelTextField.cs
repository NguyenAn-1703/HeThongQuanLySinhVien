using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

//Gồm title + textbox cho các form
public class LabelTextField : TableLayoutPanel
{
    private string _title;
    private TextFieldType _fieldType;
    
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

    TextBox GetField()
    {
        TextBox field = new TextBox();
        field.Dock = DockStyle.Top;
        field.Font = GetFont.GetFont.GetMainFont(13, FontType.Regular);
        switch (_fieldType)
        {
            case TextFieldType.NormalText:
                break;
            case TextFieldType.Password:
                field.PasswordChar = '*';
                break;
            
        }
        return field;
    }
    
}