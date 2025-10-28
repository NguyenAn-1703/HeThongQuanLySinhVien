using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;

namespace QuanLyGiangVien.Views.Components.CommonUse.Search;

public class GiangVienSearch
{
    private List<GiangVienDisplay> _rawData;
    public event Action<List<GiangVienDisplay>> FinishSearch;

    public GiangVienSearch(List<GiangVienDisplay> rawData)
    {
        this._rawData = rawData;
    }
    public void Search(string text, string selection)
    {
        List<GiangVienDisplay> result = new List<GiangVienDisplay>();
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

    List<GiangVienDisplay> SearchAll(string keyword, string selection)
    {
        List<GiangVienDisplay> result = _rawData
            .Where(x => x.MaGV.ToString().ToLower().Contains(keyword) || 
                        x.TenGV.ToString().ToLower().Contains(keyword) ||
                        x.TenKhoa.ToString().ToLower().Contains(keyword) || 
                        x.NgaySinhGV.ToString().ToLower().Contains(keyword) || 
                        x.GioiTinhGV.ToString().ToLower().Contains(keyword) || 
                        x.SoDienThoai.ToString().ToLower().Contains(keyword) || 
                        x.Email.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        return result;
    }

    List<GiangVienDisplay> SearchFilter(string keyword, string selection)
    {
        List<GiangVienDisplay> result;
        if (selection.Equals("Mã giảng viên"))
        {
            result = _rawData
                .Where(x => x.MaGV.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Tên giảng viên"))
        {
            result = _rawData
                .Where(x => x.TenGV.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Khoa"))
        {
            result = _rawData
                .Where(x => x.TenKhoa.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Ngày sinh"))
        {
            result = _rawData
                .Where(x => x.NgaySinhGV.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Giới tính"))
        {
            result = _rawData
                .Where(x => x.GioiTinhGV.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Số điện thoại"))
        {
            result = _rawData
                .Where(x => x.SoDienThoai.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else //email
        {
            result = _rawData
                .Where(x => x.Email.ToLower().Contains(keyword))
                .ToList();
        }
        return result;
    }
    
}