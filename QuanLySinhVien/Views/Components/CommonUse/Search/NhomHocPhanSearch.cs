using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NhomHocPhanSearch
{
    private List<NhomHocPhanDisplay> _filters;
    public event Action<List<NhomHocPhanDisplay>> FinishSearch;

    public NhomHocPhanSearch(List<NhomHocPhanDisplay> filters)
    {
        this._filters = filters;
    }

    public void Search(string text, string selection, List<NhomHocPhanDisplay> filters)
    {
        _filters = filters;
        List<NhomHocPhanDisplay> result = new List<NhomHocPhanDisplay>();
        string keyword = text.ToLower().Trim();
        selection = selection.Trim();

        if (selection.Equals("Tất cả"))
        {
            result = SearchAll(keyword);
        }
        else
        {
            result = SearchFilter(keyword, selection);
        }

        FinishSearch?.Invoke(result);
    }

    private List<NhomHocPhanDisplay> SearchAll(string keyword)
    {
        return _filters
            .Where(x =>
                x.MaNHP.ToString().ToLower().Contains(keyword) ||
                x.TenHP.ToLower().Contains(keyword) ||
                x.Siso.ToString().ToLower().Contains(keyword) ||
                x.TenGiangVien.ToLower().Contains(keyword) 
            )
            .ToList();
    }

    private List<NhomHocPhanDisplay> SearchFilter(string keyword, string selection)
    {
        List<NhomHocPhanDisplay> result;

        if (selection.Equals("Mã nhóm học phần"))
        {
            result = _filters.Where(x => x.MaNHP.ToString().ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Tên học phần"))
        {
            result = _filters.Where(x => x.TenHP.ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Sĩ số"))
        {
            result = _filters.Where(x => x.Siso.ToString().ToLower().Contains(keyword)).ToList();
        }
        else
        {
            result = _filters.Where(x => x.TenGiangVien.ToLower().Contains(keyword)).ToList();
        }

        return result;
    }
}
