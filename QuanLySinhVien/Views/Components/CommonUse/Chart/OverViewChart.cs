using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;

namespace QuanLySinhVien.Views.Components.CommonUse.Chart;

public class View : UserControl
{
    public View()
    {
        Size = new System.Drawing.Size(400, 400);

        var cartesianChart = new CartesianChart
        {
            LegendPosition = LegendPosition.Right,
            Series = [
                new LineSeries<int>
                {
                    Values = [5, 10, 8, 4],
                    Name = "Mary"
                },
                new ColumnSeries<int>
                {
                    Values = [4, 7, 3, 8],
                    Name = "Ana"
                }
            ],

            Location = new System.Drawing.Point(0, 0),
            Size = new System.Drawing.Size(400, 400),
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
        };

        Controls.Add(cartesianChart);
    }
}