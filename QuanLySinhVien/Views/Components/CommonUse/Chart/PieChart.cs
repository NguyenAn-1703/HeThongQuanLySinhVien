using ExCSS;
using LiveCharts.Wpf;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using PieChart = LiveChartsCore.SkiaSharpView.WinForms.PieChart;

namespace QuanLySinhVien.Views.Components.CommonUse.Chart;

public class CustomPieChart : TableLayoutPanel
{
    private PieChart pieChart;
    private string[] _content;
    private float[] _percent;

    public CustomPieChart(string[] content, float[] percent)
    {
        _content = content;
        _percent = percent;
        Init();
    }

    void Init()
    {
        this.Anchor = AnchorStyles.None;
        this.AutoSize = true;
        pieChart = new PieChart
        {
            Size = new Size(300, 300),
        };

        pieChart.Series = GetListISeries();
        pieChart.LegendPosition = LegendPosition.Bottom;
        
        pieChart.Anchor = AnchorStyles.None;
        Controls.Add(pieChart);
    }

    List<ISeries> GetListISeries()
    {
        List<ISeries> list = new List<ISeries>();
        for (int i = 0; i < _content.Length; i++)
        {
            list.Add(new PieSeries<float> {Values = new float[]{_percent[i]}, Name = _content[i]});
        }
        return list;
    }
}