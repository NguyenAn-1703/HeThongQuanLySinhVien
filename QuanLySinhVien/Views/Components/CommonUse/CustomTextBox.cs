using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomTextBox : RoundTLP
{
    public TextBox contentTextBox;
    
    public CustomTextBox()
    {
        Init();
    }

    void Init()
    {
        this.Border = true;
        this.AutoSize = true;
        
        this.RowCount = 1;
        this.ColumnCount = 2;
        
        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        contentTextBox = new TextBox();
        contentTextBox.Dock = DockStyle.Fill;
        contentTextBox.Margin = new Padding(7,5,3,3);
        
        contentTextBox.Font = GetFont.GetFont.GetMainFont(11, FontType.Regular);
        
        // contentTextBox.BorderStyle = BorderStyle.FixedSingle;
        
        // this.BorderStyle = BorderStyle.FixedSingle;
        this.contentTextBox.BorderStyle = BorderStyle.None;
        
        this.Controls.Add(contentTextBox);

        this.contentTextBox.Enter += (sender, args) => OnClick();
        this.contentTextBox.Leave += (sender, args) => OnLeave();
    }

    void OnClick()
    {
        this.BorderColor = MyColor.MainColor;
        this.Invalidate();
    }
    
    void OnLeave()
    {
        this.BorderColor = MyColor.GraySelectColor;
        this.Invalidate();
    }
    
    public bool Enable
    {
        get =>  contentTextBox.Enabled;
        set
        {
            contentTextBox.Enabled = value;
            contentTextBox.BackColor = MyColor.White;
        }
    }
}