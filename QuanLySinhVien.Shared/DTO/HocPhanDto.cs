using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien.Shared.DTO;

public class HocPhanDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaHP { get; set; }

    public int? MaHPTruoc { get; set; }
    public string TenHP { get; set; }
    public int SoTinChi { get; set; }
    public string HeSoHocPhan { get; set; }
    public int SoTietLyThuyet { get; set; }
    public int SoTietThucHanh { get; set; }
    public int Status { get; set; } = 1;
}