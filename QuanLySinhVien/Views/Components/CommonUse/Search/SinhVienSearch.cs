using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class SinhVienSearch
{
    private List<SinhVienDisplay> _rawData;
    public event Action<List<SinhVienDisplay>> FinishSearch;

    public SinhVienSearch(List<SinhVienDisplay> rawData)
    {
        this._rawData = rawData;
    }
    public void Search(string text, string selection)
    {
        List<SinhVienDisplay> result = new List<SinhVienDisplay>();
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

    List<SinhVienDisplay> SearchAll(string keyword, string selection)
    {
        List<SinhVienDisplay> result = _rawData
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

    List<SinhVienDisplay> SearchFilter(string keyword, string selection)
    {
        List<SinhVienDisplay> result;
        if (selection.Equals("Mã sinh viên"))
        {
            result = _rawData
                .Where(x => x.MaSinhVien.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Tên sinh viên"))
        {
            result = _rawData
                .Where(x => x.TenSinhVien.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Ngày sinh"))
        {
            result = _rawData
                .Where(x => x.NgaySinh.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Giới tính"))
        {
            result = _rawData
                .Where(x => x.GioiTinh.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Lớp"))
        {
            result = _rawData
                .Where(x => x.TenLop.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else if(selection.Equals("Ngành"))
        {
            result = _rawData
                .Where(x => x.TenNganh.ToString().ToLower().Contains(keyword))
                .ToList();
        }
        else //trạng thái
        {
            result = _rawData
                .Where(x => x.TrangThai.ToLower().Contains(keyword))
                .ToList();
        }
        return result;
    }
    
}