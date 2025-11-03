using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class CaThiSearch
{
    private readonly List<CaThiDisplay> _rawData;

    public CaThiSearch(List<CaThiDisplay> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<CaThiDisplay>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<CaThiDisplay>();
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

    private List<CaThiDisplay> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaCT.ToString().ToLower().Contains(keyword) ||
                        x.TenHP.ToString().ToLower().Contains(keyword) ||
                        x.TenPhong.ToString().ToLower().Contains(keyword) ||
                        x.Thu.ToString().ToLower().Contains(keyword) ||
                        x.ThoiGianBatDau.ToString().ToLower().Contains(keyword) ||
                        x.ThoiLuong.ToString().ToLower().Contains(keyword) ||
                        x.SiSo.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<CaThiDisplay> SearchFilter(string keyword, string selection)
    {
        List<CaThiDisplay> result;
        if (selection.Equals("Mã ca thi"))
            result = _rawData
                .Where(x => x.MaCT.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Học phần"))
            result = _rawData
                .Where(x => x.TenHP.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Phòng"))
            result = _rawData
                .Where(x => x.TenPhong.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Thứ"))
            result = _rawData
                .Where(x => x.Thu.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Thời gian bắt đầu"))
            result = _rawData
                .Where(x => x.ThoiGianBatDau.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Thời lượng"))
            result = _rawData
                .Where(x => x.ThoiLuong.ToString().ToLower().Contains(keyword))
                .ToList();
        else // sĩ số
            result = _rawData
                .Where(x => x.SiSo.ToString().ToLower().Contains(keyword))
                .ToList();
        return result;
    }
}