namespace QuanLySinhVien.Views.Components;

public class SinhVien : Panel
{
    public SinhVien()
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
            Dock = DockStyle.Bottom,
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
            BackColor = Color.Blue,
            Height = 90,
        };
        return mainTop;
    }

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = Color.Pink,
            Height = 780,
        };
        return mainBot;
    }
    
}