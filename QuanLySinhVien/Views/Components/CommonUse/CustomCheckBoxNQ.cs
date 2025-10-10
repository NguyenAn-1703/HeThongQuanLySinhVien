namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomCheckBoxNQ : CheckBox
{
    public string ID { get; set; }
    public string HD { get; set; }
    public CustomCheckBoxNQ(string ID, string HD)
    {
        this.ID = ID;
        this.HD = HD;
        Init();
    }

    void Init()
    {
        AutoSize = true;
        Anchor = AnchorStyles.None;
    }

}