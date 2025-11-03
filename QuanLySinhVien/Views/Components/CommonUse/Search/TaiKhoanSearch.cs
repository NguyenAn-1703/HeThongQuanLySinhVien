using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class TaiKhoanSearch
{
    private readonly List<TaiKhoanDto> _rawData;

    public TaiKhoanSearch(List<TaiKhoanDto> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<TaiKhoanDto>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<TaiKhoanDto>();
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

    private List<TaiKhoanDto> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaTK.ToString().ToLower().Contains(keyword) ||
                        x.MaNQ.ToString().ToLower().Contains(keyword) ||
                        x.TenDangNhap.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<TaiKhoanDto> SearchFilter(string keyword, string selection)
    {
        List<TaiKhoanDto> result;
        if (selection.Equals("Mã tài khoản"))
            result = _rawData
                .Where(x => x.MaTK.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Mã nhóm quyền"))
            result = _rawData
                .Where(x => x.MaNQ.ToString().ToLower().Contains(keyword))
                .ToList();
        else
            result = _rawData
                .Where(x => x.TenDangNhap.ToLower().Contains(keyword))
                .ToList();
        return result;
    }
}