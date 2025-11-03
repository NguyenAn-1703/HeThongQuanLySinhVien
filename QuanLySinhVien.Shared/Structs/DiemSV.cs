using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Shared.Structs;

public struct DiemSV
{
    public int MaSV { get; set; }
    public List<CotDiemDto> listCotDiem { get; set; }
}