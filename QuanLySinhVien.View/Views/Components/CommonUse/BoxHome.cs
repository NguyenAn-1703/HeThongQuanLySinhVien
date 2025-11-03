using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class BoxHome : RoundTLP
{
    private readonly string _content;
    private readonly string _title;

    public BoxHome(string title, string content)
    {
        _title = title;
        _content = content;
        Init();
    }

    private void Init()
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

        var tag = new RoundTLP(true, false, false);
        tag.BackColor = MyColor.MainColor;
        tag.Size = new Size(40, 130);
        Controls.Add(tag);
        SetRowSpan(tag, 2);

        var circle = new PictureBox
        {
            Anchor = AnchorStyles.None,
            Size = new Size(20, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetSvgBitmap.GetBitmap("circle.svg")
        };
        Controls.Add(circle);

        var lblTitle = new Label();
        lblTitle.Text = _title;
        lblTitle.ForeColor = MyColor.MainColor;
        lblTitle.AutoSize = true;
        lblTitle.Font = GetFont.GetFont.GetMainFont(14, FontType.SemiBold);
        Controls.Add(lblTitle);

        var lblContent = new Label();
        lblContent.Text = _content;
        lblContent.ForeColor = MyColor.MainColor;
        lblContent.AutoSize = true;
        lblContent.Font = GetFont.GetFont.GetMainFont(11, FontType.Regular);
        Controls.Add(lblContent);

        SetColumnSpan(lblContent, 2);
    }
}