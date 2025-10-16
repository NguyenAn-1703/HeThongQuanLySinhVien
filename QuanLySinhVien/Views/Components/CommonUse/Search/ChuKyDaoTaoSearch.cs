using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class ChuKyDaoTaoSearch
{
    private List<ChuKyDaoTaoDto> _rawData;
    public event Action<List<ChuKyDaoTaoDto>> FinishSearch;

    public ChuKyDaoTaoSearch(List<ChuKyDaoTaoDto> rawData)
    {
        this._rawData = rawData;
    }
    public void Search(string text, string selection)
    {
        List<ChuKyDaoTaoDto> result = new List<ChuKyDaoTaoDto>();
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

    List<ChuKyDaoTaoDto> SearchAll(string keyword, string selection)
    {
        List<ChuKyDaoTaoDto> result = _rawData
            .Where(x => x.MaCKDT.ToString().ToLower().Contains(keyword) || 
                        x.NamBatDau.ToString().ToLower().Contains(keyword) ||
                        x.NamKetThuc.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    List<ChuKyDaoTaoDto> SearchFilter(string keyword, string selection)
    {
        List<ChuKyDaoTaoDto> result;
        if (selection.Equals("Mã chu kỳ"))
        {
            result = _rawData
                .Where(x => x.MaCKDT.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Năm bắt đầu"))
        {
            result = _rawData
                .Where(x => x.NamBatDau.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else
        {
            result = _rawData
                .Where(x => x.NamKetThuc.ToLower().Contains(keyword))
                .ToList();
        }
        return result;
    }
    
}