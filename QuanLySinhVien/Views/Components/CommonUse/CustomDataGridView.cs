namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomDataGridView : DataGridView
{
    private bool _action;
    public CustomDataGridView(bool action = false)
    {
        _action = action;
        Init();
    }

    void Init()
    {
        
    }
}