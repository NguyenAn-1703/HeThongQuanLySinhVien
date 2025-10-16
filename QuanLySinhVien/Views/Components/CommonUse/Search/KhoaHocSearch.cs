using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class KhoaHocSearch
{
    private List<KhoaHocDto> _rawData;
    public event Action<List<KhoaHocDto>> FinishSearch;

    public KhoaHocSearch(List<KhoaHocDto> rawData)
    {
        this._rawData = rawData;
    }
    public void Search(string text, string selection)
    {
        List<KhoaHocDto> result = new List<KhoaHocDto>();
        string keyword = text.ToLower().Trim();
        selection =  selection.Trim();

        if (selection.Equals("Tất cả"))
        {
            result = SearchAll(keyword, selection);
        }
        else
        {
            result = SearchFilter(keyword, selection);
        }
        
        FinishSearch?.Invoke(result);
    }

    List<KhoaHocDto> SearchAll(string keyword, string selection)
    {
        List<KhoaHocDto> result = _rawData
            .Where(x => x.MaKhoaHoc.ToString().ToLower().Contains(keyword) || 
                        x.TenKhoaHoc.ToString().ToLower().Contains(keyword) ||
                        x.NienKhoaHoc.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    List<KhoaHocDto> SearchFilter(string keyword, string selection)
    {
        List<KhoaHocDto> result;
        if (selection.Equals("Mã khóa học"))
        {
            result = _rawData
                .Where(x => x.MaKhoaHoc.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Tên khóa học"))
        {
            result = _rawData
                .Where(x => x.TenKhoaHoc.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else
        {
            result = _rawData
                .Where(x => x.NienKhoaHoc.ToLower().Contains(keyword))
                .ToList();
        }
        return result;
    }
}