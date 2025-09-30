using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomCombobox : RoundTLP
{
    public ComboBox combobox;
    private string[] _items;
    public CustomCombobox(string[] items)
    {
        combobox  = new ComboBox();
        _items = items;
        Init();
    }

    void Init()
    {
        this.Border = true;

        this.AutoSize = true;
        combobox.AutoSize = true;
        combobox.Items.AddRange(_items);
        combobox.Margin = new Padding(2);
        
        this.Controls.Add(combobox);
        this.combobox.Enter += (sender, args) => OnClick();
        this.combobox.Leave += (sender, args) => OnLeave();
    }
    void OnClick()
    {
        this.BorderColor = MyColor.MainColor;
        this.Invalidate();
    }
    
    void OnLeave()
    {
        this.BorderColor = MyColor.GraySelectColor;
        this.Invalidate();
    }
}