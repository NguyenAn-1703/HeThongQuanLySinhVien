using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NhomHocPhanSearch
{
    private List<NhomHocPhanDisplay> _rawData;
    public event Action<List<NhomHocPhanDisplay>> FinishSearch;

    public NhomHocPhanSearch(List<NhomHocPhanDisplay> rawData)
    {
        this._rawData = rawData;
    }

    public void Search(string text, string selection)
    {
        List<NhomHocPhanDisplay> result = new List<NhomHocPhanDisplay>();
        string keyword = text.ToLower().Trim();
        selection = selection.Trim();

        if (selection.Equals("Tất cả"))
        {
            result = SearchAll(keyword);
        }
        else
        {
            result = SearchFilter(keyword, selection);
        }

        FinishSearch?.Invoke(result);
    }

    private List<NhomHocPhanDisplay> SearchAll(string keyword)
    {
        return _rawData
            .Where(x =>
                x.MaNHP.ToString().ToLower().Contains(keyword) ||
                x.TenHP.ToLower().Contains(keyword) ||
                x.Siso.ToString().ToLower().Contains(keyword) ||
                x.TenGiangVien.ToLower().Contains(keyword) ||
                x.Type.ToLower().Contains(keyword) ||
                x.TenPhong.ToLower().Contains(keyword) ||
                x.Thu.ToLower().Contains(keyword) ||
                x.TietBatDau.ToString().ToLower().Contains(keyword) ||
                x.SoTiet.ToString().ToLower().Contains(keyword)
            )
            .ToList();
    }

    private List<NhomHocPhanDisplay> SearchFilter(string keyword, string selection)
    {
        List<NhomHocPhanDisplay> result;

        if (selection.Equals("Mã nhóm học phần"))
        {
            result = _rawData.Where(x => x.MaNHP.ToString().ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Tên học phần"))
        {
            result = _rawData.Where(x => x.TenHP.ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Sĩ số"))
        {
            result = _rawData.Where(x => x.Siso.ToString().ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Tên giảng viên"))
        {
            result = _rawData.Where(x => x.TenGiangVien.ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Loại nhóm"))
        {
            result = _rawData.Where(x => x.Type.ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Phòng học"))
        {
            result = _rawData.Where(x => x.TenPhong.ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Thứ"))
        {
            result = _rawData.Where(x => x.Thu.ToLower().Contains(keyword)).ToList();
        }
        else if (selection.Equals("Tiết bắt đầu"))
        {
            result = _rawData.Where(x => x.TietBatDau.ToString().ToLower().Contains(keyword)).ToList();
        }
        else // Số tiết
        {
            result = _rawData.Where(x => x.SoTiet.ToString().ToLower().Contains(keyword)).ToList();
        }

        return result;
    }
}
