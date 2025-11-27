using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Entity;

namespace QuanLySinhVien.Database;


public class AppDbContext : DbContext 
{
    private static readonly string _connectionString = "Server=localhost;" +
                                                       "Database=quanlysinhvien;" +
                                                       "Uid=root;" +
                                                       "Pwd=bi2552453;";
    
    public DbSet<LopEntity> Lops { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                _connectionString,
                new MySqlServerVersion(new Version(8, 0, 21))
            );
        }
    }
}