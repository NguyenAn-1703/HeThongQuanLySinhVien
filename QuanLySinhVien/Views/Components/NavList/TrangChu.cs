using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class TrangChu : NavBase
{
    public TrangChu()
    {
        Init();
    }

    private void Init()
    {
        //BackColor = Color.Blue;
        Dock = DockStyle.Fill;
        Size = new Size(1200, 900);
        var borderTop = new Panel
        {
            Dock = DockStyle.Fill,
            //Padding = new  Padding(0 , 110 , 0 , 0),
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
        throw new NotImplementedException();
    }
}