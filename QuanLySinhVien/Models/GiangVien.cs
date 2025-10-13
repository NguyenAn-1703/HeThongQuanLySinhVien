using System.Data;
using System.Runtime.InteropServices.JavaScript;

namespace QuanLySinhVien.Models;

public class GiangVien
{
    public int MaGV { get; set; }
    public int MaTK { get; set; }
    public int MaKhoa { get; set; }
    public string TenGV { get; set; }
    public DateOnly NgaySinhGV { get; set; }
    public string GioiTinhGV { get; set; }
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
    
    // 
    public string TenKhoa { get; set; }
    
    //
    public object[] ToDataRow()
    {
        return new object[] {MaGV, TenGV, TenKhoa, NgaySinhGV, GioiTinhGV, SoDienThoai, Email, Status};
    }
}