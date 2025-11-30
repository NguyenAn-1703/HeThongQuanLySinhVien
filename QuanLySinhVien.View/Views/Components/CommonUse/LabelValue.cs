using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components;

public class LabelValue : MyTLP
{
    public string title { get; set; }
    public string value { get; set; }
    public Label lblTitle { get; set; }
    public Label lblValue { get; set; }

    public LabelValue(string title, string value)
    {
        this.title = title;
        this.value = value;
        Init();
    }

    void Init()
    {
        AutoSize = true;
        ColumnCount = 2;
        lblTitle = new Label{Text = title, AutoSize = true, Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold)};
        lblValue = new Label{Text = value, AutoSize = true, Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold)};
        this.Controls.Add(lblTitle);
        this.Controls.Add(lblValue);
    }
}