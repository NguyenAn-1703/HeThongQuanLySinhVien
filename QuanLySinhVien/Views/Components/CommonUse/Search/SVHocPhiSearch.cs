using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class SVHocPhiSearch
{
    private readonly List<SVHocPhiDisplay> _rawData;

    public SVHocPhiSearch(List<SVHocPhiDisplay> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<SVHocPhiDisplay>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<SVHocPhiDisplay>();
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();


        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword, selection);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch.Invoke(result);
    }

    private List<SVHocPhiDisplay> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaSV.ToString().ToLower().Contains(keyword) ||
                        x.TenSV.ToString().ToLower().Contains(keyword) ||
                        x.Khoa.ToString().ToLower().Contains(keyword) ||
                        x.Nganh.ToString().ToLower().Contains(keyword) ||
                        x.TrangThaiHP.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<SVHocPhiDisplay> SearchFilter(string keyword, string selection)
    {
        List<SVHocPhiDisplay> result;
        if (selection.Equals("Mã sinh viên"))
            result = _rawData
                .Where(x => x.MaSV.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Tên sinh viên"))
            result = _rawData
                .Where(x => x.TenSV.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Khoa"))
            result = _rawData
                .Where(x => x.Khoa.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Ngành"))
            result = _rawData
                .Where(x => x.Nganh.ToString().ToLower().Contains(keyword))
                .ToList();
        else //trạng thai
            result = _rawData
                .Where(x => x.TrangThaiHP.ToLower().Contains(keyword))
                .ToList();

        return result;
    }
}