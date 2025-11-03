using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class ChuKyDaoTaoSearch
{
    private readonly List<ChuKyDaoTaoDto> _rawData;

    public ChuKyDaoTaoSearch(List<ChuKyDaoTaoDto> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<ChuKyDaoTaoDto>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<ChuKyDaoTaoDto>();
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();


        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword, selection);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch.Invoke(result);
    }

    private List<ChuKyDaoTaoDto> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaCKDT.ToString().ToLower().Contains(keyword) ||
                        x.NamBatDau.ToString().ToLower().Contains(keyword) ||
                        x.NamKetThuc.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<ChuKyDaoTaoDto> SearchFilter(string keyword, string selection)
    {
        List<ChuKyDaoTaoDto> result;
        if (selection.Equals("Mã chu kỳ"))
            result = _rawData
                .Where(x => x.MaCKDT.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Năm bắt đầu"))
            result = _rawData
                .Where(x => x.NamBatDau.ToString().ToLower().Contains(keyword))
                .ToList();
        else
            result = _rawData
                .Where(x => x.NamKetThuc.ToLower().Contains(keyword))
                .ToList();
        return result;
    }
}