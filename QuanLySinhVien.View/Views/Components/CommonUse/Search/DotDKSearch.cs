using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.View.Views.Components.CommonUse.Search;

public class DotDKSearch
{
    private readonly List<DotDangKyDto> _rawData;

    public DotDKSearch(List<DotDangKyDto> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<DotDangKyDto>> FinishSearch;

    public void Search(string text, string selection)
    {
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();

        List<DotDangKyDto> result;

        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch?.Invoke(result);
    }

    private List<DotDangKyDto> SearchAll(string keyword)
    {
        var result = _rawData
            .Where(x =>
                x.MaDotDK.ToString().ToLower().Contains(keyword) ||
                x.HocKy.ToString().ToLower().Contains(keyword) ||
                (x.Nam?.ToLower().Contains(keyword) ?? false) ||
                x.ThoiGianBatDau.ToString("dd/MM/yyyy HH:mm").ToLower().Contains(keyword) ||
                x.ThoiGianKetThuc.ToString("dd/MM/yyyy HH:mm").ToLower().Contains(keyword)
            )
            .ToList();

        return result;
    }

    private List<DotDangKyDto> SearchFilter(string keyword, string selection)
    {
        List<DotDangKyDto> result;

        if (selection.Equals("Mã đợt đăng ký"))
            result = _rawData
                .Where(x => x.MaDotDK.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Học kỳ"))
            result = _rawData
                .Where(x => x.HocKy.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Năm"))
            result = _rawData
                .Where(x => (x.Nam?.ToLower().Contains(keyword) ?? false))
                .ToList();
        else if (selection.Equals("Thời gian bắt đầu"))
            result = _rawData
                .Where(x => x.ThoiGianBatDau
                    .ToString("dd/MM/yyyy HH:mm")
                    .ToLower().Contains(keyword))
                .ToList();
        else // Thời gian kết thúc
            result = _rawData
                .Where(x => x.ThoiGianKetThuc
                    .ToString("dd/MM/yyyy HH:mm")
                    .ToLower().Contains(keyword))
                .ToList();

        return result;
    }
}
