using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien.Models;

public class Nganh
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaNganh { get; set; }
    public int MaKhoa { get; set; }
    public string TenNganh { get; set; }
}
