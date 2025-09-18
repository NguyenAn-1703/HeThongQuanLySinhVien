namespace QuanLySinhVien.Views.Components;

public class QuanLiTaiKhoan : Panel
{
    public QuanLiTaiKhoan()
    {
        Init();
    }
        
    private void Init()
    {
        //BackColor = Color.Blue;
        Dock = DockStyle.Bottom;
        Size = new Size(1200, 900);
        var borderTop = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new  Padding(0 , 30 , 0 , 0),
        };
        borderTop.Controls.Add(Top());
        Controls.Add(Bottom());
        Controls.Add(borderTop);
    }

    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Top,
            // BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            BackColor = Color.Red,
            Height = 80,
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
}