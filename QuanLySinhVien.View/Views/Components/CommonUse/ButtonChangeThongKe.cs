using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class ButtonChangeThongKe : TableLayoutPanel
{
    private readonly string txt;
    private int index;
    private bool isSelected = false;

    public ButtonChangeThongKe(int index, string txt)
    {
        this.txt = txt;
        this.index = index;
        Init();
    }

    public event Action<int, string> mouseClick;
    private MyTLP selectBar;
    private Label lbl;

    private void Init()
    {
        Height = 40;
        Width = 110;
        Margin = new Padding(0);
        BackColor = MyColor.GrayBackGround;
        Cursor = Cursors.Hand;
        RowCount = 2;
        
        lbl = new Label
        {
            Text = txt,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.Bold),
            Anchor = AnchorStyles.None,
            ForeColor = MyColor.Black
        };

        selectBar = new MyTLP
        {
            BackColor = MyColor.GrayBackGround,
            Width = 50,
            Height = 3,
            Anchor = AnchorStyles.None
        };
        
        Controls.Add(lbl);
        Controls.Add(selectBar);
        SetAction();
    }

    private void SetAction()
    {
        MouseEnter += (sender, args) => OnMouseEnter();
        MouseLeave += (sender, args) => OnMouseLeave();
        MouseDown += (sender, args) => OnMouseDown();

        foreach (Control c in Controls)
        {
            c.MouseEnter += (sender, args) => OnMouseEnter();
            c.MouseLeave += (sender, args) => OnMouseLeave();
            c.MouseDown += (sender, args) => OnMouseDown();
        }
    }

    private void OnMouseEnter()
    {
        BackColor = MyColor.GrayHoverColor;
        if(!isSelected) selectBar.BackColor = MyColor.GrayHoverColor;
    }

    private void OnMouseLeave()
    {
        BackColor = MyColor.GrayBackGround;
        if(!isSelected) selectBar.BackColor = MyColor.GrayBackGround;
    }

    private void OnMouseDown()
    {
        mouseClick?.Invoke(index, txt);
    }

    public void SetSelected()
    {
        lbl.ForeColor = MyColor.MainColor;
        selectBar.BackColor = MyColor.MainColor;
        isSelected = true;
    }

    public void UnSelected()
    {
        lbl.ForeColor = MyColor.Black;
        selectBar.BackColor = MyColor.GrayBackGround;
        isSelected = false;
    }
    
}