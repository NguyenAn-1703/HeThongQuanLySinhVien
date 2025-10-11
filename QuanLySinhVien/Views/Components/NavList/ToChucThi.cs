using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class ToChucThi : NavBase
{
    
    private string[] _listSelectionForComboBox = new []{""};
    public ToChucThi()
    {
        Init();
    }
    
    
    // -------------- Graphics --------------- //
    private float GetFontWidth(Label label)
    {
        Graphics g = label.CreateGraphics();
        SizeF size = g.MeasureString(label.Text, label.Font);

        return size.Width;
    }
    
    
    
    
    
    
    // -------------- Label --------------- //
    private Label LbHeding()
    {
        Label lb = new Label
        {
            Dock = DockStyle.Left,
            Text = "Tổ chức thi",
            Font = new Font("JetBrains Mono", 17f, FontStyle.Bold),
            Height = 90,
            TextAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(30, 0, 0, 0),
        };
        
        lb.Width = Convert.ToInt32(GetFontWidth(lb)) + 50;
        return lb;
    }
    
    
    
    
    
        
    private void Init()
    {
        //BackColor = Color.Blue;
        Dock = DockStyle.Bottom;
        Size = new Size(1200, 900);
        var borderTop = new Panel
        {
            Dock = DockStyle.Fill,
            // Padding = new  Padding(0 , 30 , 0 , 0),
        };
        borderTop.Controls.Add(Top());
        Controls.Add(borderTop);
        Controls.Add(Bottom());
    }

    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Bottom,
            // BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            BackColor = Color.Red,
            Height = 90,
            Controls = { LbHeding() }
        };
        return mainTop;
    }

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = Color.Green,
            Height = 780,
        };
        return mainBot;
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    public override void onSearch(string txtSearch, string filter)
    { }
}