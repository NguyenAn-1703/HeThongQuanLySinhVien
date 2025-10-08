using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using QuanLySinhVien.Views.Components.ViewComponents;
using LiveChartsCore.SkiaSharpView.WinForms;

namespace QuanLySinhVien.Views.Components.CommonUse.Chart;

public class OverViewChart : RoundTLP
{
    public OverViewChart(int[] listValue)
    {
        Size = new System.Drawing.Size(400, 400);
        Margin = new Padding(10, 10, 10, 10);
        var cartesianChart = new CartesianChart()
        {
            LegendPosition = LegendPosition.Bottom,
            
            Series = new ISeries[]
            {
                new LineSeries<int> { Values = listValue, Name = "Tổng số sinh viên" }
            },
        
            Location = new System.Drawing.Point(0, 0),
            Size = new System.Drawing.Size(400, 400),
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
        };
        
        Controls.Add(cartesianChart);
    }
}