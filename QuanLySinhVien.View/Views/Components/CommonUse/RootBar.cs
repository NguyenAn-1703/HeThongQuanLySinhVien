using QuanLySinhVien.Shared.Enums;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class RootBar : MyTLP
{
    private List<string> rootList;
    private Label lblRootBar;
    public RootBar()
    {
        rootList = new List<string>();
        Init();
    }

    void Init()
    {
        Dock = DockStyle.Fill;
        AutoSize = true;

        SetLblRootBar();
        this.Controls.Add(lblRootBar);
    }

    void SetLblRootBar()
    {
        lblRootBar = new Label
        {
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(12, FontType.Bold),
            ForeColor = Color.Gray
        };
    }

    public void AddItem(string item)
    {
        if (rootList.Count > 0)
        {
            item = " > " + item;
        }
        rootList.Add(item);
        Update();
    }

    public void RemoveItem()
    {
        int lastIndex = rootList.Count - 1;
        rootList.RemoveAt(lastIndex);
        Update();
    }

    void Update()
    {
        string lblText = "";
        foreach (string item in rootList) lblText += item + " ";
        lblRootBar.Text = lblText;
    }
}