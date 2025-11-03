using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class HocPhanSearch
{
    private readonly List<HocPhanDto> _rawData;

    public HocPhanSearch(List<HocPhanDto> rawData)
    {
        _rawData = rawData ?? new List<HocPhanDto>();
    }

    public event Action<List<HocPhanDto>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<HocPhanDto>();
        var keyword = (text ?? string.Empty).ToLower().Trim();
        selection = (selection ?? string.Empty).Trim();

        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch?.Invoke(result);
    }

    private List<HocPhanDto> SearchAll(string keyword)
    {
        return _rawData
            .Where(x => (x.MaHP + "").ToLower().Contains(keyword)
                        || (x.MaHPTruoc?.ToString() ?? string.Empty).ToLower().Contains(keyword)
                        || (x.TenHP ?? string.Empty).ToLower().Contains(keyword)
                        || (x.SoTinChi + "").ToLower().Contains(keyword))
            .ToList();
    }

    private List<HocPhanDto> SearchFilter(string keyword, string selection)
    {
        if (selection.Equals("Mã HP")) return _rawData.Where(x => (x.MaHP + "").ToLower().Contains(keyword)).ToList();
        if (selection.Equals("Mã HP Trước"))
            return _rawData.Where(x => (x.MaHPTruoc?.ToString() ?? string.Empty).ToLower().Contains(keyword)).ToList();
        if (selection.Equals("Tên HP"))
            return _rawData.Where(x => (x.TenHP ?? string.Empty).ToLower().Contains(keyword)).ToList();
        // Số Tín Chỉ
        return _rawData.Where(x => (x.SoTinChi + "").ToLower().Contains(keyword)).ToList();
    }
}