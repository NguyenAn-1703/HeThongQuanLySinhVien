using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class CustomDateField : RoundTLP
{
    public DateTimePicker dateField;

    public CustomDateField()
    {
        dateField = new DateTimePicker();
        Init();
    }

    private void Init()
    {
        Border = true;

        AutoSize = true;


        dateField.Format = DateTimePickerFormat.Custom;
        dateField.CustomFormat = "dd/MM/yyyy";


        dateField.AutoSize = true;
        dateField.Dock = DockStyle.Fill;
        dateField.Margin = new Padding(2);

        Controls.Add(dateField);
        dateField.Enter += (sender, args) => OnClick();
        dateField.Leave += (sender, args) => OnLeave();
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
}