using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class NhomQuyenSearch
{
    private List<NhomQuyenDto> _rawData;
    public event Action<List<NhomQuyenDto>> FinishSearch;

    public NhomQuyenSearch(List<NhomQuyenDto> rawData)
    {
        this._rawData = rawData;
    }
    public void Search(string text, string selection)
    {
        List<NhomQuyenDto> result = new List<NhomQuyenDto>();
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
        
        FinishSearch.Invoke(result);
    }

    List<NhomQuyenDto> SearchAll(string keyword, string selection)
    {
        List<NhomQuyenDto> result = _rawData
            .Where(x => x.MaNQ.ToString().ToLower().Contains(keyword) || 
                        x.TenNhomQuyen.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    List<NhomQuyenDto> SearchFilter(string keyword, string selection)
    {
        List<NhomQuyenDto> result;
        if (selection.Equals("Mã nhóm quyền"))
        {
            result = _rawData
                .Where(x => x.MaNQ.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else
        {
            result = _rawData
                .Where(x => x.TenNhomQuyen.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        return result;
    }
}