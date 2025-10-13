using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView.Painting;
using OpenTK.Graphics.OpenGL4;
using QuanLySinhVien.Views.Components.ViewComponents;
using SkiaSharp;

namespace QuanLySinhVien.Views.Components.CommonUse.Chart;

public class OverviewChart : RoundTLP
{
    public OverviewChart()
    {
        Size = new System.Drawing.Size(50, 50);
        Margin = new Padding(10);
        
        var values2 = new double[] { 3, 11, 5, 3, 7, 3, 8 };

        var series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = values2,
                IgnoresBarPosition = true
            }
        };

        var yAxis = new Axis
        {
            MinLimit = 0,
            MaxLimit = 10
        };

        var xAxis = new Axis
        {
            Labels = new string[] { "2018", "2019", "2020", "2021", "2022", "2023", "2024" },
            LabelsRotation = 0, 
            Name = "Năm" 
        };
        
        var cartesianChart = new CartesianChart
        {
            Series = series,
            YAxes = [yAxis],
            XAxes = [xAxis],
            Location = new System.Drawing.Point(0, 0),
            Size = new System.Drawing.Size(50, 50),
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
        };

        Controls.Add(cartesianChart);
    }
}