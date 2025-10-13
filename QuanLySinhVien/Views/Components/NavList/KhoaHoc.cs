using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components.NavList;

public class KhoaHoc : NavBase
{
    private string[] _listSelectionForComboBox = new[] { "" };
    
    
    
    
    
    
    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}