using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Structs;

public struct DiemSV
{
    public int MaSV { get; set; }
    public List<CotDiemDto> listCotDiem { get; set; }
}