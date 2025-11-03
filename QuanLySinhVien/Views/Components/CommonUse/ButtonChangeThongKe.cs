namespace QuanLySinhVien.Views.Components.CommonUse;

public class ButtonChangeThongKe : TableLayoutPanel
{
    private readonly string txt;

    public ButtonChangeThongKe(string txt)
    {
        this.txt = txt;
        Init();
    }

    public event Action<string> mouseClick;

    private void Init()
    {
        AutoSize = true;
        Margin = new Padding(0);
        BackColor = MyColor.GrayBackGround;
        var lbl = new Label
        {
            Text = txt,
            AutoSize = true
        };
        Controls.Add(lbl);
        SetAction();
    }

    private void SetAction()
    {
        MouseEnter += (sender, args) => OnMouseEnter();
        MouseLeave += (sender, args) => OnMouseLeave();
        MouseDown += (sender, args) => OnMouseDown();
        MouseUp += (sender, args) => OnMouseUp();

        foreach (Control c in Controls)
        {
            c.MouseEnter += (sender, args) => OnMouseEnter();
            c.MouseLeave += (sender, args) => OnMouseLeave();
            c.MouseDown += (sender, args) => OnMouseDown();
            c.MouseUp += (sender, args) => OnMouseUp();
        }
    }

    private void OnMouseEnter()
    {
        BackColor = MyColor.GrayHoverColor;
    }

    private void OnMouseLeave()
    {
        BackColor = MyColor.GrayBackGround;
    }

    private void OnMouseDown()
    {
        BackColor = MyColor.GraySelectColor;
        mouseClick?.Invoke(txt);
    }

    private void OnMouseUp()
    {
        BackColor = MyColor.GrayHoverColor;
    }
}