using QuanLySinhVien.Views.Components;

namespace QuanLySinhVien.Views.UserControls;

public class UcLogin : MyUc
{
    public MyPanel panelCenter = new MyPanel(new Size(1200, 900), MyColor.White, DockStyle.None);
    // public Label labelHeader = new Label()
    
    public UcLogin() : base(new Size(1440, 1024), MyColor.SkyBlue)
    {
        
        // Location Components
        panelCenter.Left = (this.Width - panelCenter.Width) / 2;
        panelCenter.Top = (this.Height - panelCenter.Height) / 2;
        
        // Add components
        Controls.Add(panelCenter);
    }
}