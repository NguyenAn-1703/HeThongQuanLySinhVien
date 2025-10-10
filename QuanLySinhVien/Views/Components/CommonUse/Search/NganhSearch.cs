using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class NganhSearch
{
    private List<NganhDto> _rawData;
    public event Action<List<NganhDto>> FinishSearch;

    public NganhSearch(List<NganhDto> rawData)
    {
        this._rawData = rawData;
    }
    
    public void Search(string text, string selection)
    {
        List<NganhDto> result = new List<NganhDto>();
        string keyword = text.ToLower().Trim();
        selection =  selection.Trim();
        
        // Console.WriteLine("'" + keyword + "'");
        // Console.WriteLine("'" + selection + "'");

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

    List<NganhDto> SearchAll(string keyword, string selection)
    {
        List<NganhDto> result = _rawData
            .Where(x => x.MaNganh.ToString().ToLower().Contains(keyword) || 
                        x.MaKhoa.ToString().ToLower().Contains(keyword) ||
                        x.TenNganh.ToString().ToLower().Contains(keyword)
                        )
            .ToList();
        return result;
    }

    List<NganhDto> SearchFilter(string keyword, string selection)
    {
        List<NganhDto> result;
        if (selection.Equals("Mã ngành"))
        {
            result = _rawData
                .Where(x => x.MaNganh.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Mã khoa"))
        {
            result = _rawData
                .Where(x => x.MaKhoa.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else
        {
            result = _rawData
                .Where(x => x.TenNganh.ToLower().Contains(keyword))
                .ToList();
        }
        return result;
    }
    
}