using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Entity;

namespace QuanLySinhVien.Models.DAO;

public class LopDAO
{
    private static LopDAO _instance;

    private LopDAO()
    {
    }

    public static LopDAO GetInstance()
    {
        if (_instance == null) _instance = new LopDAO();
        return _instance;
    }

    public List<LopDto> GetAll()
    {
        List<LopDto> result;
        
        //ADO.NET
        // using var conn = MyConnection.GetConnection();
        // using var cmd = new MySqlCommand("SELECT MaLop, MaGV, MaNganh, TenLop, SoLuongSV FROM lop WHERE Status = 1",
        //     conn);
        // using var reader = cmd.ExecuteReader();
        //
        // while (reader.Read())
        //     result.Add(new LopDto
        //     {
        //         MaLop = reader.GetInt32("MaLop"),
        //         MaGV = reader.GetInt32("MaGV"),
        //         MaNganh = reader.GetInt32("MaNganh"),
        //         TenLop = reader.GetString("TenLop"),
        //         SoLuongSV = reader.GetInt32("SoLuongSV")
        //     });
        
        //Linq to entity
        using var context = new AppDbContext();
        result = context.Lops
            .Where(l => l.Status)
            .Select(l => new LopDto
            {
                MaLop = l.MaLop,
                MaGV = l.MaGV,
                MaNganh = l.MaNganh,
                TenLop = l.TenLop,
                SoLuongSV = l.SoLuongSV
            }).ToList();
        
        return result;
    }

    public bool Insert(LopDto lopDto)
    {
        //ADO.NET
        // var rowAffected = 0;
        // using (var conn = MyConnection.GetConnection())
        // {
        //     var query = @"INSERT INTO lop (MaGV, MaNganh, TenLop, SoLuongSV)
        //                      VALUES (@MaGV, @MaNganh, @TenLop, @SoLuongSV)";
        //     using (var cmd = new MySqlCommand(query, conn))
        //     {
        //         cmd.Parameters.AddWithValue("@MaGV", lopDto.MaGV);
        //         cmd.Parameters.AddWithValue("@MaNganh", lopDto.MaNganh);
        //         cmd.Parameters.AddWithValue("@TenLop", lopDto.TenLop);
        //         cmd.Parameters.AddWithValue("@SoLuongSV", lopDto.SoLuongSV);
        //
        //         rowAffected = cmd.ExecuteNonQuery();
        //     }
        // }
        //
        // return rowAffected > 0;
        
        //Linq to Entity
        using var context = new AppDbContext();
        
        var lopEntity = new LopEntity()
        {
            MaGV = lopDto.MaGV,
            MaNganh = lopDto.MaNganh,
            TenLop = lopDto.TenLop,
            SoLuongSV = lopDto.SoLuongSV,
            Status = true,
        };
        context.Lops.Add(lopEntity);
        return context.SaveChanges() > 0;
    }

    public bool Update(LopDto lopDto)
    {
        //ADO.NET
        // var rowAffected = 0;
        // using (var conn = MyConnection.GetConnection())
        // {
        //     var query = @"UPDATE lop 
        //                      SET MaGV = @MaGV,
        //                          MaNganh = @MaNganh,
        //                          TenLop = @TenLop,
        //                          SoLuongSV = @SoLuongSV
        //                      WHERE MaLop = @MaLop";
        //     using (var cmd = new MySqlCommand(query, conn))
        //     {
        //         cmd.Parameters.AddWithValue("@MaLop", lopDto.MaLop);
        //         cmd.Parameters.AddWithValue("@MaGV", lopDto.MaGV);
        //         cmd.Parameters.AddWithValue("@MaNganh", lopDto.MaNganh);
        //         cmd.Parameters.AddWithValue("@TenLop", lopDto.TenLop);
        //         cmd.Parameters.AddWithValue("@SoLuongSV", lopDto.SoLuongSV);
        //
        //         rowAffected = cmd.ExecuteNonQuery();
        //     }
        // }
        //
        // return rowAffected > 0;
        
        //Linq to entity
        using var context = new AppDbContext();

        var lopEntity = context.Lops.FirstOrDefault(l => l.MaLop == lopDto.MaLop && l.Status);
        if (lopEntity == null)
            return false;

        lopEntity.MaGV = lopDto.MaGV;
        lopEntity.MaNganh = lopDto.MaNganh;
        lopEntity.TenLop = lopDto.TenLop;
        lopEntity.SoLuongSV = lopDto.SoLuongSV;

        return context.SaveChanges() > 0;
    }

    public bool Delete(int maLop)
    {
        //ADO.NET
        // var rowAffected = 0;
        // using (var conn = MyConnection.GetConnection())
        // {
        //     var query = @"UPDATE lop
        //                      SET Status = 0
        //                      WHERE MaLop = @MaLop;";
        //     using (var cmd = new MySqlCommand(query, conn))
        //     {
        //         cmd.Parameters.AddWithValue("@MaLop", maLop);
        //         rowAffected = cmd.ExecuteNonQuery();
        //     }
        // }
        //
        // return rowAffected > 0;
        
        //Linq to entity
        using var context = new AppDbContext();

        var lopEntity = context.Lops.FirstOrDefault(l => l.MaLop == maLop && l.Status);
        if (lopEntity == null)
            return false;

        lopEntity.Status = false;

        return context.SaveChanges() > 0;
    }

    public LopDto GetLopById(int maLop)
    {
        var result = new LopDto();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaLop, MaGV, MaNganh, TenLop, SoLuongSV FROM lop WHERE Status = 1 AND MaLop = @MaLop", conn);
        cmd.Parameters.AddWithValue("@MaLop", maLop);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new LopDto
            {
                MaLop = reader.GetInt32("MaLop"),
                MaGV = reader.GetInt32("MaGV"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenLop = reader.GetString("TenLop"),
                SoLuongSV = reader.GetInt32("SoLuongSV")
            };

        return result;
    }

    public LopDto GetByTen(string tenLop)
    {
        var result = new LopDto();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaLop, MaGV, MaNganh, TenLop, SoLuongSV FROM lop WHERE Status = 1 AND TenLop = @TenLop", conn);
        cmd.Parameters.AddWithValue("@TenLop", tenLop);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new LopDto
            {
                MaLop = reader.GetInt32("MaLop"),
                MaGV = reader.GetInt32("MaGV"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenLop = reader.GetString("TenLop"),
                SoLuongSV = reader.GetInt32("SoLuongSV")
            };

        return result;
    }


    public List<LopDto> GetByNganh(int maNganh)
    {
        List<LopDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(@"
            SELECT l.MaLop, l.MaGV, l.MaNganh, l.TenLop, l.SoLuongSV,
                   COALESCE(COUNT(s.MaSV), 0) as SoSinhVienHienTai
            FROM lop l
            LEFT JOIN sinhvien s ON l.MaLop = s.MaLop AND s.Status = 1
            WHERE l.Status = 1 AND l.MaNganh = @MaNganh
            GROUP BY l.MaLop, l.MaGV, l.MaNganh, l.TenLop, l.SoLuongSV
            ORDER BY l.TenLop", conn);
        cmd.Parameters.AddWithValue("@MaNganh", maNganh);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new LopDto
            {
                MaLop = reader.GetInt32("MaLop"),
                MaGV = reader.GetInt32("MaGV"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenLop = reader.GetString("TenLop"),
                SoLuongSV = reader.GetInt32("SoLuongSV"),
                SoSinhVienHienTai = reader.GetInt32("SoSinhVienHienTai")
            });

        return result;
    }

    // public LopDto GetById(int maLop)
    // {
    //     LopDto result = new();
    //     using var conn = MyConnection.GetConnection();
    //     using var cmd = new MySqlCommand(@"
    //         SELECT l.MaLop, l.MaGV, l.MaNganh, l.TenLop, l.SoLuongSV,
    //                COALESCE(COUNT(s.MaSV), 0) as SoSinhVienHienTai
    //         FROM lop l
    //         LEFT JOIN sinhvien s ON l.MaLop = s.MaLop AND s.Status = 1
    //         WHERE l.Status = 1 AND l.MaLop = @MaLop
    //         GROUP BY l.MaLop, l.MaGV, l.MaNganh, l.TenLop, l.SoLuongSV", conn);
    //     cmd.Parameters.AddWithValue("@MaLop", maLop);
    //     using var reader = cmd.ExecuteReader();
    //
    //     if (reader.Read())
    //         result = new LopDto
    //         {
    //             MaLop = reader.GetInt32("MaLop"),
    //             MaGV = reader.GetInt32("MaGV"),
    //             MaNganh = reader.GetInt32("MaNganh"),
    //             TenLop = reader.GetString("TenLop"),
    //             SoLuongSV = reader.GetInt32("SoLuongSV"),
    //             SoSinhVienHienTai = reader.GetInt32("SoSinhVienHienTai")
    //         };
    //
    //     return result;
    // }
}