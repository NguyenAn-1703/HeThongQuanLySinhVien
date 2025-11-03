using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomCombobox : RoundTLP
{
    private string[] _items;
    public ComboBox combobox;

    public CustomCombobox(string[] items)
    {
        combobox = new ComboBox();
        _items = items;
        Init();
    }

    public int cbxWidth { get; set; } = 150;

    public bool Enable
    {
        get => combobox.Enabled;
        set
        {
            combobox.Enabled = value;
            combobox.BackColor = MyColor.White;
        }
    }

    private void Init()
    {
        Border = true;
        AutoSize = true;

        combobox.DropDownStyle = ComboBoxStyle.DropDownList;
        combobox.AutoSize = true;
        combobox.Width = cbxWidth;

        combobox.Dock = DockStyle.Fill;
        combobox.Items.AddRange(_items);
        combobox.Margin = new Padding(2);

        Controls.Add(combobox);
        combobox.Enter += (sender, args) => OnClick();
        combobox.Leave += (sender, args) => OnLeave();
    }

    private void OnClick()
    {
        BorderColor = MyColor.MainColor;
        Invalidate();
    }

    private void OnLeave()
    {
        BorderColor = MyColor.GraySelectColor;
        Invalidate();
    }

    public void UpdateSelection(string[] newlist)
    {
        _items = newlist;
        combobox.Items.Clear();
        combobox.Items.AddRange(_items);
        combobox.SelectedIndex = 0;
    }

    public void SetSelectionCombobox(string text)
    {
        foreach (var i in combobox.Items)
            if (i.ToString().Trim().Equals(text))
            {
                combobox.SelectedItem = i;
                return;
            }
    }
}