using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class StatisticalBox : RoundTLP
{
    private readonly StatisticalIndex _index;
    private PictureBox _pb;

    public StatisticalBox(string content, double number, StatisticalIndex index)
    {
        _index = index;
        Height = 140;
        Anchor = AnchorStyles.None;
        Dock = DockStyle.Fill;
        Padding = new Padding(10);
        Margin = new Padding(10, 0, 10, 0);
        BorderRadius = 20;

        RowCount = 2;
        ColumnCount = 2;
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        SetPictureBox();
        SetUpStatisticalBox();

        var titleLbl = new Label();
        titleLbl.Text = content;
        titleLbl.ForeColor = MyColor.White;
        titleLbl.Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold);
        titleLbl.AutoSize = true;
        titleLbl.Margin = new Padding(3, 10, 3, 3);

        var numberLbl = new Label();
        numberLbl.Text = number.ToString("N0");
        numberLbl.ForeColor = MyColor.White;
        numberLbl.Font = GetFont.GetFont.GetMainFont(14, FontType.Black);
        numberLbl.AutoSize = true;
        numberLbl.Margin = new Padding(3, 10, 3, 3);

        Controls.Add(_pb);
        SetRowSpan(_pb, 2);

        Controls.Add(titleLbl);
        Controls.Add(numberLbl);
    }

    private void SetPictureBox()
    {
        _pb = new PictureBox
        {
            Size = new Size(50, 50),
            SizeMode = PictureBoxSizeMode.Zoom,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 0, 0, 0)
        };
    }

    private void SetUpStatisticalBox()
    {
        switch (_index)
        {
            case StatisticalIndex.first:
                BackColor = MyColor.DarkBlue;
                _pb.Image = GetSvgBitmap.GetBitmap("whiteuser.svg");
                break;
            case StatisticalIndex.second:
                BackColor = MyColor.MediumBlue;
                _pb.Image = GetSvgBitmap.GetBitmap("whiteteacher.svg");
                break;
            case StatisticalIndex.third:
                BackColor = MyColor.StrongBlue;
                _pb.Image = GetSvgBitmap.GetBitmap("whitebook.svg");
                break;
            case StatisticalIndex.fourth:
                BackColor = MyColor.SkyBlue;
                _pb.Image = GetSvgBitmap.GetBitmap("whitemoney.svg");
                break;
        }
    }
}