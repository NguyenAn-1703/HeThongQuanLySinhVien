using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;

namespace QuanLySinhVien.View.Views.Components.CommonUse.Search;

public class TaiKhoanSearch
{
    private readonly List<TaiKhoanDisplay> _rawData;

    public TaiKhoanSearch(List<TaiKhoanDisplay> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<TaiKhoanDisplay>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<TaiKhoanDisplay>();
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

    private List<TaiKhoanDisplay> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaTK.ToString().ToLower().Contains(keyword) ||
                        x.TenNhomQuyen.ToString().ToLower().Contains(keyword) ||
                        x.TenDangNhap.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<TaiKhoanDisplay> SearchFilter(string keyword, string selection)
    {
        List<TaiKhoanDisplay> result;
        if (selection.Equals("Mã tài khoản"))
            result = _rawData
                .Where(x => x.MaTK.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Tên nhóm quyền"))
            result = _rawData
                .Where(x => x.TenNhomQuyen.ToString().ToLower().Contains(keyword))
                .ToList();
        else
            result = _rawData
                .Where(x => x.TenDangNhap.ToLower().Contains(keyword))
                .ToList();
        return result;
    }
}