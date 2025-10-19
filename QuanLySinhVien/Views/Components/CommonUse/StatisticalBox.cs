using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class StatisticalBox : RoundTLP
{
    private StatisticalIndex _index;
    private PictureBox _pb;
    public StatisticalBox(String content, Double number, StatisticalIndex index)
    {
        _index =  index;
        this.Height = 140;
        this.Anchor = AnchorStyles.None;
        this.Dock = DockStyle.Fill;
        this.Padding = new Padding(10); 
        this.Margin = new Padding(10, 0, 10, 0);
        this.BorderRadius = 20;

        this.RowCount = 2;
        this.ColumnCount = 2;
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        SetPictureBox();
        SetUpStatisticalBox();
        
        Label titleLbl = new Label();
        titleLbl.Text = content;
        titleLbl.ForeColor = MyColor.White;
        titleLbl.Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold);
        titleLbl.AutoSize = true;
        titleLbl.Margin = new Padding(3, 10, 3, 3);
        
        Label numberLbl = new Label();
        numberLbl.Text = number.ToString("N0");
        numberLbl.ForeColor = MyColor.White;
        numberLbl.Font = GetFont.GetFont.GetMainFont(14, FontType.Black);
        numberLbl.AutoSize = true;
        numberLbl.Margin = new Padding(3, 10, 3, 3);
        
        this.Controls.Add(_pb);
        this.SetRowSpan(_pb, 2);
        
        this.Controls.Add(titleLbl);
        this.Controls.Add(numberLbl);
    }

    void SetPictureBox()
    {
        _pb = new PictureBox
        {
            Size = new Size(50, 50),
            SizeMode = PictureBoxSizeMode.Zoom,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 0, 0, 0)
        };
    }

    void SetUpStatisticalBox()
    {
        switch (_index)
        {
            case StatisticalIndex.first:
                this.BackColor = MyColor.DarkBlue;
                _pb.Image = GetSvgBitmap.GetBitmap("whiteuser.svg");
                break;
            case StatisticalIndex.second:
                this.BackColor = MyColor.MediumBlue;
                _pb.Image = GetSvgBitmap.GetBitmap("whiteteacher.svg");
                break;
            case StatisticalIndex.third:
                this.BackColor = MyColor.StrongBlue;
                _pb.Image = GetSvgBitmap.GetBitmap("whitebook.svg");
                break;
            case StatisticalIndex.fourth:
                this.BackColor = MyColor.SkyBlue;
                _pb.Image = GetSvgBitmap.GetBitmap("whitemoney.svg");
                break;
        }
    }
    
}