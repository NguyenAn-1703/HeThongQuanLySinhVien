using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using PieChart = LiveChartsCore.SkiaSharpView.WinForms.PieChart;

namespace QuanLySinhVien.View.Views.Components.CommonUse.Chart;

public class CustomPieChart : TableLayoutPanel
{
    private readonly string[] _content;
    private readonly float[] _percent;
    private PieChart pieChart;

    public CustomPieChart(string[] content, float[] percent)
    {
        _content = content;
        _percent = percent;
        Init();
    }

    private void Init()
    {
        Anchor = AnchorStyles.None;
        AutoSize = true;
        pieChart = new PieChart
        {
            Size = new Size(300, 300)
        };

        pieChart.Series = GetListISeries();
        pieChart.LegendPosition = LegendPosition.Bottom;

        pieChart.Anchor = AnchorStyles.None;
        Controls.Add(pieChart);
    }

    private List<ISeries> GetListISeries()
    {
        var list = new List<ISeries>();
        for (var i = 0; i < _content.Length; i++)
            list.Add(new PieSeries<float>
            {
                Values = new[] { _percent[i] }, Name = _content[i],
                ToolTipLabelFormatter = point => $"{point.Label}: {Math.Round(point.Model, 2)}%"
            });
        return list;
    }
}