using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class NhomQuyenSearch
{
    private readonly List<NhomQuyenDto> _rawData;

    public NhomQuyenSearch(List<NhomQuyenDto> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<NhomQuyenDto>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<NhomQuyenDto>();
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();

        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword, selection);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch.Invoke(result);
    }

    private List<NhomQuyenDto> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaNQ.ToString().ToLower().Contains(keyword) ||
                        x.TenNhomQuyen.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<NhomQuyenDto> SearchFilter(string keyword, string selection)
    {
        List<NhomQuyenDto> result;
        if (selection.Equals("Mã nhóm quyền"))
            result = _rawData
                .Where(x => x.MaNQ.ToString().ToLower().Contains(keyword))
                .ToList();
        else
            result = _rawData
                .Where(x => x.TenNhomQuyen.ToString().ToLower().Contains(keyword))
                .ToList();
        return result;
    }
}