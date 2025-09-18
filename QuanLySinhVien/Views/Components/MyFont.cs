namespace QuanLySinhVien.Views.Components;

public class MyFont
{
    public Font Font { get; set; }
    
    public MyFont(float size, FontStyle style)
    {
        Font = new Font("Arial", size, style);
    }
}