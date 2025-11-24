using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class CustomTextBox : RoundTLP
{
    public TextBox contentTextBox;

    public CustomTextBox()
    {
        Init();
    }

    public bool Enable
    {
        get => contentTextBox.Enabled;
        set
        {
            contentTextBox.Enabled = value;
            contentTextBox.BackColor = MyColor.White;
        }
    }

    private void Init()
    {
        Border = true;
        AutoSize = true;

        RowCount = 1;
        ColumnCount = 2;

        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        contentTextBox = new TextBox();
        contentTextBox.Dock = DockStyle.Fill;
        contentTextBox.Margin = new Padding(5);

        contentTextBox.Font = new Font("Segoe UI", 11);

        // contentTextBox.BorderStyle = BorderStyle.FixedSingle;

        // this.BorderStyle = BorderStyle.FixedSingle;
        contentTextBox.BorderStyle = BorderStyle.None;

        Controls.Add(contentTextBox);

        contentTextBox.Enter += (sender, args) => OnClick();
        contentTextBox.Leave += (sender, args) => OnLeave();
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

    public void SetText(string input)
    {
        contentTextBox.Text = input;
    }
}