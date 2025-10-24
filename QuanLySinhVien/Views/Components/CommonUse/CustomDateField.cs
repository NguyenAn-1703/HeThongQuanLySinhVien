using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomDateField : RoundTLP
{
    public DateTimePicker dateField;
    public CustomDateField()
    {
        dateField  = new DateTimePicker();
        Init();
    }

    void Init()
    {
        this.Border = true;

        this.AutoSize = true;

        
        dateField.Format = DateTimePickerFormat.Custom;
        dateField.CustomFormat = " dd/MM/yyyy";
        
        
        dateField.AutoSize = true;
        dateField.Dock = DockStyle.Fill;
        dateField.Margin = new Padding(2);
        
        this.Controls.Add(dateField);
        this.dateField.Enter += (sender, args) => OnClick();
        this.dateField.Leave += (sender, args) => OnLeave();
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