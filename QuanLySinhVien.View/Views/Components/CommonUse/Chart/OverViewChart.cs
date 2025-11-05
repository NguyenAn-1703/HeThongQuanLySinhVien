using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.ViewComponents;
using Padding = System.Windows.Forms.Padding;

namespace QuanLySinhVien.View.Views.Components.CommonUse.Chart;

public class OverviewChart : RoundTLP
{
    public OverviewChart(string[] labels, double[] values)
    {
        Size = new Size(50, 50);
        Margin = new Padding(10);

        var values2 = new double[] { 3, 11, 5, 3, 7, 3, 8 };

        var series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                // Values = values2,
                Values = values,
                IgnoresBarPosition = true
            }
        };

        var yAxis = new Axis
        {
            MinLimit = 0,
            MaxLimit = values.Max()
        };

        var xAxis = new Axis
        {
            // Labels = new string[] { "2018", "2019", "2020", "2021", "2022", "2023", "2024" },
            Labels = labels,
            LabelsRotation = 0
        };

        var cartesianChart = new CartesianChart
        {
            Series = series,
            YAxes = [yAxis],
            XAxes = [xAxis],
            Location = new Point(0, 0),
            Size = new Size(50, 50),
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
        };

        var titleLabel = new Label
        {
            Text = "Tổng số sinh viên nhập học 5 năm gần nhất",
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 30
        };

        Controls.Add(titleLabel);

        Controls.Add(cartesianChart);
    }
}