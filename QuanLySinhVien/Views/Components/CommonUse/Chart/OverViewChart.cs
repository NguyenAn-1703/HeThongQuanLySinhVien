using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;

namespace QuanLySinhVien.Views.Components.CommonUse.Chart;

public class OverViewChart : UserControl
{
    public OverViewChart(int[] listValue)
    {
        Size = new System.Drawing.Size(400, 400);

        var cartesianChart = new CartesianChart
        {
            LegendPosition = LegendPosition.Bottom,
            Series = [
                new LineSeries<int>
                {
                    Values = listValue,
                    Name = "Tổng số sinh viên"
                },
                // new ColumnSeries<int>
                // {
                //     Values = [4, 7, 3, 8],
                //     Name = "Ana"
                // }
            ],

            Location = new System.Drawing.Point(0, 0),
            Size = new System.Drawing.Size(400, 400),
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
        };

        Controls.Add(cartesianChart);
    }
}