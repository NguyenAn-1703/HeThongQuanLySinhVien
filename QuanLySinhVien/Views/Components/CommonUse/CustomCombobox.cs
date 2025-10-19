using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

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
        
        
        combobox.DropDownStyle = ComboBoxStyle.DropDownList;
        combobox.AutoSize = true;
        combobox.Dock = DockStyle.Fill;
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

    public void UpdateSelection(string[] newlist)
    {
        _items = newlist;
        combobox.Items.Clear();
        combobox.Items.AddRange(_items);
        combobox.SelectedIndex = 0;
    }

    public bool Enable
    {
        get =>  combobox.Enabled;
        set
        {
            combobox.Enabled = value;
            combobox.BackColor = MyColor.White;
        }
    }

    public void SetSelectionCombobox(string text)
    {
        foreach (var i in combobox.Items)
        {
            if (i.ToString().Trim().Equals(text))
            {
                combobox.SelectedItem = i;
                return;
            }
        }
    }
}