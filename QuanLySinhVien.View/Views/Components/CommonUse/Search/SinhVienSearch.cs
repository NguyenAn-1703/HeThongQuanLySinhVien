using QuanLySinhVien.Shared.DTO.SearchObject;

namespace QuanLySinhVien.View.Views.Components.CommonUse.Search;

public class SinhVienSearch
{
    private readonly List<SinhVienDisplay> _rawData;

    public SinhVienSearch(List<SinhVienDisplay> rawData)
    {
        _rawData = rawData;
    }

    public event Action<List<SinhVienDisplay>> FinishSearch;

    public void Search(string text, string selection)
    {
        var result = new List<SinhVienDisplay>();
        var keyword = text.ToLower().Trim();
        selection = selection.Trim();

        if (selection.Equals("Tất cả"))
            result = SearchAll(keyword, selection);
        else
            result = SearchFilter(keyword, selection);

        FinishSearch.Invoke(result);
    }

    private List<SinhVienDisplay> SearchAll(string keyword, string selection)
    {
        var result = _rawData
            .Where(x => x.MaSinhVien.ToString().ToLower().Contains(keyword) ||
                        x.TenSinhVien.ToString().ToLower().Contains(keyword) ||
                        x.NgaySinh.ToString().ToLower().Contains(keyword) ||
                        x.GioiTinh.ToString().ToLower().Contains(keyword) ||
                        x.TenLop.ToString().ToLower().Contains(keyword) ||
                        x.TenNganh.ToString().ToLower().Contains(keyword) ||
                        x.TrangThai.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    private List<SinhVienDisplay> SearchFilter(string keyword, string selection)
    {
        List<SinhVienDisplay> result;
        if (selection.Equals("Mã sinh viên"))
            result = _rawData
                .Where(x => x.MaSinhVien.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Tên sinh viên"))
            result = _rawData
                .Where(x => x.TenSinhVien.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Ngày sinh"))
            result = _rawData
                .Where(x => x.NgaySinh.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Giới tính"))
            result = _rawData
                .Where(x => x.GioiTinh.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Lớp"))
            result = _rawData
                .Where(x => x.TenLop.ToString().ToLower().Contains(keyword))
                .ToList();
        else if (selection.Equals("Ngành"))
            result = _rawData
                .Where(x => x.TenNganh.ToString().ToLower().Contains(keyword))
                .ToList();
        else //trạng thái
            result = _rawData
                .Where(x => x.TrangThai.ToLower().Contains(keyword))
                .ToList();
        return result;
    }
}