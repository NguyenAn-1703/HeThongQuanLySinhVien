using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class LopSearch
{
    private readonly List<LopDisplay> _rawData;

    public LopSearch(List<LopDisplay> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<LopDisplay>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<LopDisplay>();
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();


        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword, selection);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch.Invoke(result);
    }

    private List<LopDisplay> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaLop.ToString().ToLower().Contains(keyword) ||
                        x.TenLop.ToString().ToLower().Contains(keyword) ||
                        x.TenGV.ToString().ToLower().Contains(keyword) ||
                        x.TenNganh.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<LopDisplay> SearchFilter(string keyword, string selection)
    {
        List<LopDisplay> result;
        if (selection.Equals("Mã lớp"))
            result = _rawData
                .Where(x => x.MaLop.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Tên lớp"))
            result = _rawData
                .Where(x => x.TenLop.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Tên ngành"))
            result = _rawData
                .Where(x => x.TenNganh.ToString().ToLower().Contains(keyword))
                .ToList();
        else
            result = _rawData
                .Where(x => x.TenGV.ToLower().Contains(keyword))
                .ToList();

        return result;
    }
}