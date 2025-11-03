using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class NganhSearch
{
    private readonly List<NganhDto> _rawData;

    public NganhSearch(List<NganhDto> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<NganhDto>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<NganhDto>();
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();

        // Console.WriteLine("'" + keyword + "'");
        // Console.WriteLine("'" + selection + "'");

        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword, selection);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch.Invoke(result);
    }

    private List<NganhDto> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaNganh.ToString().ToLower().Contains(keyword) ||
                        x.MaKhoa.ToString().ToLower().Contains(keyword) ||
                        x.TenNganh.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<NganhDto> SearchFilter(string keyword, string selection)
    {
        List<NganhDto> result;
        if (selection.Equals("Mã ngành"))
            result = _rawData
                .Where(x => x.MaNganh.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Mã khoa"))
            result = _rawData
                .Where(x => x.MaKhoa.ToString().ToLower().Contains(keyword))
                .ToList();
        else
            result = _rawData
                .Where(x => x.TenNganh.ToLower().Contains(keyword))
                .ToList();
        return result;
    }
}