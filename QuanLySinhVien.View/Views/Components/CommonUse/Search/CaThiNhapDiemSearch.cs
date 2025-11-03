using QuanLySinhVien.Shared.DTO.SearchObject;

namespace QuanLySinhVien.View.Views.Components.CommonUse.Search;

public class CaThiNhapDiemSearch
{
    private List<CaThiNhapDiemDisplay> _filters;

    public CaThiNhapDiemSearch(List<CaThiNhapDiemDisplay> filters)
    {
        _filters = filters;
    }

    public event Action<List<CaThiNhapDiemDisplay>> FinishSearch;

    public void Search(string text, string selection, List<CaThiNhapDiemDisplay> filters)
    {
        _filters = filters;
        var result = new List<CaThiNhapDiemDisplay>();
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();

        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch?.Invoke(result);
    }

    private List<CaThiNhapDiemDisplay> SearchAll(string keyword)
    {
        return _filters
            .Where(x =>
                x.MaCT.ToString().ToLower().Contains(keyword) ||
                x.TenHP.ToLower().Contains(keyword) ||
                x.NgayThi.ToString().ToLower().Contains(keyword) ||
                x.TenPhong.ToLower().Contains(keyword)
            )
            .ToList();
    }

    private List<CaThiNhapDiemDisplay> SearchFilter(string keyword, string selection)
    {
        List<CaThiNhapDiemDisplay> result;

        if (selection.Equals("Mã ca thi"))
            result = _filters.Where(x => x.MaCT.ToString().ToLower().Contains(keyword)).ToList();
        else if (selection.Equals("Tên HP"))
            result = _filters.Where(x => x.TenHP.ToLower().Contains(keyword)).ToList();
        else if (selection.Equals("Ngày thi"))
            result = _filters.Where(x => x.NgayThi.ToString().ToLower().Contains(keyword)).ToList();
        else
            result = _filters.Where(x => x.TenPhong.ToLower().Contains(keyword)).ToList();

        return result;
    }
}