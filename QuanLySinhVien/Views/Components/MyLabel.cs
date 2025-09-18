namespace QuanLySinhVien.Views.Components;

public class MyLabel:Label
{
    public MyLabel(string text, Color foreColor, int fontSize, FontStyle fontStyle)
    {
        base.ForeColor = foreColor;
        base.Text = text;
        base.Font = new Font("Arial", fontSize, fontStyle);
        base.TextAlign = ContentAlignment.MiddleCenter;
        
        // Default
        base.AutoSize = true;
    }
}