using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien.Shared.Entity;

[Table("lop")]
public class LopEntity
{
    [Key] [Column("MaLop")] public int MaLop { get; set; }

    [Column("MaGV")] public int MaGV { get; set; }

    [Column("MaNganh")] public int MaNganh { get; set; }

    [Column("TenLop")] public string TenLop { get; set; }

    [Column("SoLuongSV")] public int SoLuongSV { get; set; }

    [Column("Status")] public bool Status { get; set; } = true;
}