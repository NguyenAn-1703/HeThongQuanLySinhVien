using System.Data;
using System.Runtime.InteropServices.JavaScript;

namespace QuanLySinhVien.Models;

public class GiangVienDto
{   
    public GiangVienDto(){}
    public int MaGV { get; set; }
    public int MaTK { get; set; }
    public int MaKhoa { get; set; }
    public string TenGV { get; set; }
    public DateOnly NgaySinhGV { get; set; }
    public string GioiTinhGV { get; set; }
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public string TrangThai { get; set; }
    public int Status { get; set; }
    
    public string AnhDaiDien { get; set; }
    
    // 
    public string TenKhoa { get; set; }
    
    //
    public object[] ToDataRow()
    {
        return new object[] {MaGV, TenGV, TenKhoa, NgaySinhGV, GioiTinhGV, SoDienThoai, Email, TrangThai};
    }
    
}