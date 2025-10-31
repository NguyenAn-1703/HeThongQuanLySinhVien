namespace QuanLySinhVien.Views.Components.CommonUse;

public class IndexButton : CustomButton
{
    public int index { get; set; } = -1;
    public event Action<int> MouseDownIndex;
    public IndexButton(int width, int height, string svg, Color backColor, bool radius1 = true, bool radius2 = true, bool radius3 = true, bool radius4 = true, bool border = false) : base(width, height, svg, backColor, radius1, radius2, radius3, radius4, border)
    {
        
    }
    
    public override void OnMouseDown()
    {
        this.BackColor = this.SelectColor;
        MouseDownIndex?.Invoke(index);
    }
}