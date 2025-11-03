namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomCheckBoxNQ : CheckBox
{
    public CustomCheckBoxNQ(string ID = "", string HD = "")
    {
        this.ID = ID;
        this.HD = HD;
        Init();
    }

    public string ID { get; set; }
    public string HD { get; set; }

    private void Init()
    {
        AutoSize = true;
        Anchor = AnchorStyles.None;
    }
}