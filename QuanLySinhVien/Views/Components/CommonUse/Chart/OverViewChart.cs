using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.VisualElements;
using OpenTK.Graphics.OpenGL4;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using SkiaSharp;
using Padding = System.Windows.Forms.Padding;

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
        
        var titleLabel = new Label
        {
            Text = "Tổng số sinh viên nhập học 7 năm gần nhất",
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 30
        };

        Controls.Add(titleLabel);

        Controls.Add(cartesianChart);
    }
}