using QuanLySinhVien.DB;

namespace QuanLySinhVien.DAO;

public class NganhDAO
{
    private AppDbContext db = new AppDbContext();

    public List<Models.Nganh> getAllNganh()
    {
        return db.nganh.ToList();
    }

    public void addNganh(Models.Nganh nganh)
    {
        db.nganh.Add(nganh);
        db.SaveChanges();
    }

    public void deleteNganh(int maNganh)
    {
        var nganh = db.nganh.Find(maNganh);
        if (nganh != null)
        {
            db.nganh.Remove(nganh);
            db.SaveChanges();
        }
    }

    public void updateNganh(Models.Nganh nganh)
    {
        Console.WriteLine(nganh.MaNganh);
        var existingNganh = db.nganh.Find(nganh.MaNganh);
        if (existingNganh != null)
        {
            existingNganh.TenNganh = nganh.TenNganh;
            existingNganh.MaKhoa = nganh.MaKhoa;
            db.SaveChanges();
        }
        else
        {
            Console.Write("Nganh not found");
        }
    }
}