namespace QuanLySinhVien.Views.Components.NavList;

public class ThongKe : Panel
{

    // private void Init()
    // {
    //     //BackColor = Color.Blue;
    //     Dock = DockStyle.Bottom;
    //     Size = new Size(1200, 900);
    //     var borderTop = new Panel
    //     {
    //         Dock = DockStyle.Fill,
    //         // Padding = new  Padding(0 , 30 , 0 , 0),
    //     };
    //     borderTop.Controls.Add(Top());
    //     Controls.Add(borderTop);
    //     Controls.Add(Bottom());
    // }
    //
    // private Panel Top()
    // {
    //     Panel mainTop = new Panel
    //     {
    //         Dock = DockStyle.Bottom,
    //         // BackColor = ColorTranslator.FromHtml("#E5E7EB"),
    //         BackColor = Color.Red,
    //         Height = 90,
    //     };
    //     return mainTop;
    // }
    //
    // private Panel Bottom()
    // {
    //     Panel mainBot = new Panel
    //     {
    //         Dock = DockStyle.Bottom,
    //         BackColor = Color.Green,
    //         Height = 780,
    //     };
    //     return mainBot;
    // }
    public ThongKe()
    {
        Init();
    }

    void Init()
    {
        this.Dock = DockStyle.Fill;
        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            RowCount = 2,
            ColumnCount = 4,
            Dock = DockStyle.Fill,
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        mainLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        
        mainLayout.Controls.Add(GetTopBar());
        mainLayout.Controls.Add(GetMainPanel());

        this.Controls.Add(mainLayout);
    }

    TableLayoutPanel GetTopBar()
    {
        TableLayoutPanel panel = new TableLayoutPanel();
        panel.BackColor = MyColor.Red;
        return panel;
    }

    TableLayoutPanel GetMainPanel()
    {
        TableLayoutPanel panel = new TableLayoutPanel();
        panel.BackColor = MyColor.GrayBackGround;
        return panel;
    }
}