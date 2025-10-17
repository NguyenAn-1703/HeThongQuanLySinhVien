namespace QuanLySinhVien.Views.Components.CommonUse;

public class ButtonChangeThongKe : TableLayoutPanel
{
    public event Action<string> mouseClick;
    private string txt;
    public ButtonChangeThongKe(string txt)
    {
        this.txt = txt;
        Init();
    }

    void Init()
    {
        AutoSize = true;
        Margin = new Padding(0);
        BackColor = MyColor.GrayBackGround;
        Label lbl = new Label
        {
            Text = txt,
            AutoSize = true,
        };
        this.Controls.Add(lbl);
        SetAction();
    }

    void SetAction()
    {
        this.MouseEnter += (sender, args) => OnMouseEnter();
        this.MouseLeave += (sender, args) =>  OnMouseLeave();
        this.MouseDown += (sender, args) =>   OnMouseDown();
        this.MouseUp += (sender, args) =>    OnMouseUp();

        foreach (Control c in this.Controls)
        {
            c.MouseEnter += (sender, args) =>  OnMouseEnter();
            c.MouseLeave += (sender, args) =>  OnMouseLeave();
            c.MouseDown += (sender, args) =>  OnMouseDown();
            c.MouseUp += (sender, args) =>    OnMouseUp();
        }
    }

    void OnMouseEnter()
    {
        this.BackColor = MyColor.GrayHoverColor;

    }
    void OnMouseLeave()
    {
        this.BackColor = MyColor.GrayBackGround;

    }
    void OnMouseDown()
    {
        this.BackColor = MyColor.GraySelectColor;
        mouseClick?.Invoke(txt);
    }
    void OnMouseUp()
    {
        this.BackColor = MyColor.GrayHoverColor;
    }
}