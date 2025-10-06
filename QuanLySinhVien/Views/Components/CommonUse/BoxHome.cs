using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class BoxHome : RoundTLP
{
    private string _title;
    private string _content;
    public BoxHome(string title, string content)
    {
        _title = title;
        _content = content;
        Init();
    }

    void Init()
    {
        RowCount = 2;
        ColumnCount = 3;
        Size = new Size(370, 130);
        BackColor = MyColor.LightGray;
        
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        RoundTLP tag = new RoundTLP(true, false, false, true);
        tag.BackColor = MyColor.MainColor;
        tag.Size = new Size(40, 130);
        this.Controls.Add(tag);
        this.SetRowSpan(tag, 2);
        
        PictureBox circle = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(20, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap("circle.svg")
        };
        this.Controls.Add(circle);
        
        Label lblTitle = new  Label();
        lblTitle.Text = _title;
        lblTitle.ForeColor = MyColor.MainColor;
        lblTitle.AutoSize = true;
        lblTitle.Font = GetFont.GetFont.GetMainFont(14,  FontType.SemiBold);
        this.Controls.Add(lblTitle);
        
        Label lblContent = new  Label();
        lblContent.Text = _content;
        lblContent.ForeColor = MyColor.MainColor;
        lblContent.AutoSize = true;
        lblContent.Font = GetFont.GetFont.GetMainFont(11,  FontType.Regular);
        this.Controls.Add(lblContent);

        this.SetColumnSpan(lblContent, 2);

    }
    
    
}