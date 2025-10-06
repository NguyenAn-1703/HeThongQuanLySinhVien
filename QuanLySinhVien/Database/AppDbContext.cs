using QuanLySinhVien.Models;
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.DB;

public class AppDbContext : DbContext
{
    public DbSet<Nganh> nganh { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseMySql(
            "server=localhost;port=3306;database=quanlysinhvien;user=root;password;",
            new MySqlServerVersion(new Version(8, 0, 43))
        );
}
