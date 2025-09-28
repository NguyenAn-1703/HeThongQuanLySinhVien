using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;

namespace WinFormsSample.Pies.Doughnut;

public partial class MyChart : UserControl
{
    private readonly PieChart pieChart;

    public MyChart()
    {
        pieChart = new PieChart
        {
            Size = new Size(150, 150),
        };
        pieChart.Series = new ISeries[]
        {
            new PieSeries<double> { Values = new double[] { 3 }, Name = "A" },
            new PieSeries<double> { Values = new double[] { 7 }, Name = "B" }
        };
        pieChart.Anchor = AnchorStyles.None;
        Controls.Add(pieChart);
    }
}