using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class ChuongTrinhDaoTaoSearch
{
    private readonly List<ChuongTrinhDaoTaoDisplayObject> _rawData;

    public ChuongTrinhDaoTaoSearch(List<ChuongTrinhDaoTaoDisplayObject> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<ChuongTrinhDaoTaoDisplayObject>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<ChuongTrinhDaoTaoDisplayObject>();
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();


        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword, selection);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch.Invoke(result);
    }

    private List<ChuongTrinhDaoTaoDisplayObject> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaCTDT.ToString().ToLower().Contains(keyword) ||
                        x.TenNganh.ToString().ToLower().Contains(keyword) ||
                        x.ChuKy.ToString().ToLower().Contains(keyword) ||
                        x.LoaiHinhDT.ToString().ToLower().Contains(keyword) ||
                        x.TrinhDo.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<ChuongTrinhDaoTaoDisplayObject> SearchFilter(string keyword, string selection)
    {
        // string[] _headerArray = new string[] { "Mã chương trình đào tạo", "Tên ngành", "Chu kỳ","Loại hình đào tạo", "Trình độ" };
        List<ChuongTrinhDaoTaoDisplayObject> result;
        if (selection.Equals("Mã chương trình đào tạo"))
            result = _rawData
                .Where(x => x.MaCTDT.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Tên ngành"))
            result = _rawData
                .Where(x => x.TenNganh.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Chu kỳ"))
            result = _rawData
                .Where(x => x.ChuKy.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Loại hình đào tạo"))
            result = _rawData
                .Where(x => x.LoaiHinhDT.ToString().ToLower().Contains(keyword))
                .ToList();
        else
            result = _rawData
                .Where(x => x.TrinhDo.ToLower().Contains(keyword))
                .ToList();
        return result;
    }
}