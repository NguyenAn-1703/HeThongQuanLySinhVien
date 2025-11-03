using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class LichHocDao
{
    private static LichHocDao _instance;

    private LichHocDao()
    {
    }

    public static LichHocDao GetInstance()
    {
        if (_instance == null) _instance = new LichHocDao();

        return _instance;
    }

    public List<LichHocDto> GetAll()
    {
        List<LichHocDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(@"SELECT MaLH, MaPH, MaNHP, Thu, 
                                                  TietBatDau, TuNgay, DenNgay, 
                                                  TietKetThuc, SoTiet, Type
                                           FROM LichHoc
                                           WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new LichHocDto
            {
                MaLH = reader.GetInt32("MaLH"),
                MaPH = reader.GetInt32("MaPH"),
                MaNHP = reader.GetInt32("MaNHP"),
                Thu = reader.GetString("Thu"),
                TietBatDau = reader.GetInt32("TietBatDau"),
                TuNgay = reader.GetDateTime("TuNgay"),
                DenNgay = reader.GetDateTime("DenNgay"),
                TietKetThuc = reader.GetInt32("TietKetThuc"),
                SoTiet = reader.GetInt32("SoTiet"),
                Type = reader.GetString("Type")
            });

        return result;
    }

    public bool Insert(LichHocDto lichHocDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO LichHoc (MaPH, MaNHP, Thu, 
                                                  TietBatDau, TuNgay, DenNgay, 
                                                  TietKetThuc, SoTiet, Type)
                             VALUES (@MaPH, @MaNHP, @Thu, 
                                     @TietBatDau, @TuNgay, @DenNgay, 
                                     @TietKetThuc, @SoTiet, @Type)";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaPH", lichHocDto.MaPH);
                cmd.Parameters.AddWithValue("@MaNHP", lichHocDto.MaNHP);
                cmd.Parameters.AddWithValue("@Thu", lichHocDto.Thu);
                cmd.Parameters.AddWithValue("@TietBatDau", lichHocDto.TietBatDau);
                cmd.Parameters.AddWithValue("@TuNgay", lichHocDto.TuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", lichHocDto.DenNgay);
                cmd.Parameters.AddWithValue("@TietKetThuc", lichHocDto.TietKetThuc);
                cmd.Parameters.AddWithValue("@SoTiet", lichHocDto.SoTiet);
                cmd.Parameters.AddWithValue("@Type", lichHocDto.Type);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(LichHocDto lichHocDto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE LichHoc
                             SET MaPH = @MaPH,
                                 MaNHP = @MaNHP,
                                 Thu = @Thu,
                                 TietBatDau = @TietBatDau,
                                 TuNgay = @TuNgay,
                                 DenNgay = @DenNgay,
                                 TietKetThuc = @TietKetThuc,
                                 SoTiet = @SoTiet,
                                 Type = @Type
                             WHERE MaLH = @MaLH";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLH", lichHocDto.MaLH);
                cmd.Parameters.AddWithValue("@MaPH", lichHocDto.MaPH);
                cmd.Parameters.AddWithValue("@MaNHP", lichHocDto.MaNHP);
                cmd.Parameters.AddWithValue("@Thu", lichHocDto.Thu);
                cmd.Parameters.AddWithValue("@TietBatDau", lichHocDto.TietBatDau);
                cmd.Parameters.AddWithValue("@TuNgay", lichHocDto.TuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", lichHocDto.DenNgay);
                cmd.Parameters.AddWithValue("@TietKetThuc", lichHocDto.TietKetThuc);
                cmd.Parameters.AddWithValue("@SoTiet", lichHocDto.SoTiet);
                cmd.Parameters.AddWithValue("@Type", lichHocDto.Type);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maLH)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE LichHoc
                             SET Status = 0
                             WHERE MaLH = @MaLH";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLH", maLH);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public LichHocDto GetById(int maLH)
    {
        LichHocDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(@"SELECT MaLH, MaPH, MaNHP, Thu, 
                                                  TietBatDau, TuNgay, DenNgay, 
                                                  TietKetThuc, SoTiet, Type
                                           FROM LichHoc
                                           WHERE Status = 1 AND MaLH = @MaLH", conn);
        cmd.Parameters.AddWithValue("@MaLH", maLH);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result = new LichHocDto
            {
                MaLH = reader.GetInt32("MaLH"),
                MaPH = reader.GetInt32("MaPH"),
                MaNHP = reader.GetInt32("MaNHP"),
                Thu = reader.GetString("Thu"),
                TietBatDau = reader.GetInt32("TietBatDau"),
                TuNgay = reader.GetDateTime("TuNgay"),
                DenNgay = reader.GetDateTime("DenNgay"),
                TietKetThuc = reader.GetInt32("TietKetThuc"),
                SoTiet = reader.GetInt32("SoTiet"),
                Type = reader.GetString("Type")
            };

        return result;
    }

    public List<LichHocDto> GetLichHocByPhongAndDate(int maPH, DateTime date)
    {
        var result = new List<LichHocDto>();
        var thu = (int)date.DayOfWeek == 0 ? 8 : (int)date.DayOfWeek + 1;

        var query = @"
                SELECT 
                    lh.MaLH, lh.MaPH, lh.MaNHP, lh.Thu, 
                    lh.TietBatDau, lh.TuNgay, lh.DenNgay, lh.TietKetThuc, lh.SoTiet,
                    ph.TenPH,
                    hp.TenHP,
                    gv.TenGV,
                    nhp.SiSo
                FROM LichHoc lh
                INNER JOIN PhongHoc ph ON lh.MaPH = ph.MaPH
                INNER JOIN NhomHocPhan nhp ON lh.MaNHP = nhp.MaNHP
                INNER JOIN HocPhan hp ON nhp.MaHP = hp.MaHP
                INNER JOIN GiangVien gv ON nhp.MaGV = gv.MaGV
                WHERE lh.MaPH = @MaPH 
                    AND lh.Thu = @Thu
                    AND @CurrentDate BETWEEN lh.TuNgay AND lh.DenNgay
                    AND lh.Status = 1
                ORDER BY lh.TietBatDau";

        try
        {
            using var connection = MyConnection.GetConnection();
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@MaPH", maPH);
            command.Parameters.AddWithValue("@Thu", thu);
            command.Parameters.AddWithValue("@CurrentDate", date.Date);

            using var reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(new LichHocDto
                {
                    MaLH = reader.GetInt32("MaLH"),
                    MaPH = reader.GetInt32("MaPH"),
                    MaNHP = reader.GetInt32("MaNHP"),
                    Thu = reader.GetString("Thu"),
                    TietBatDau = reader.GetInt32("TietBatDau"),
                    TuNgay = reader.GetDateTime("TuNgay"),
                    DenNgay = reader.GetDateTime("DenNgay"),
                    TietKetThuc = reader.GetInt32("TietKetThuc"),
                    SoTiet = reader.GetInt32("SoTiet"),
                    TenPH = reader.GetString("TenPH"),
                    TenHP = reader.GetString("TenHP"),
                    TenGV = reader.GetString("TenGV"),
                    SiSo = reader.GetInt32("SiSo")
                });
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy lịch học: {ex.Message}");
        }

        return result;
    }

    // GetByMaNhp
    public List<LichHocDto> GetByMaNhp(int maNhp)
    {
        List<LichHocDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(@"SELECT MaLH, MaPH, MaNHP, Thu, 
                                                  TietBatDau, TuNgay, DenNgay, 
                                                  TietKetThuc, SoTiet, Type
                                           FROM LichHoc
                                           WHERE Status = 1 AND MaNHP = @MaNHP", conn);
        cmd.Parameters.AddWithValue("@MaNHP", maNhp);
        using var reader = cmd.ExecuteReader();


        while (reader.Read())
            result.Add(new LichHocDto
            {
                MaLH = reader.GetInt32("MaLH"),
                MaPH = reader.GetInt32("MaPH"),
                MaNHP = reader.GetInt32("MaNHP"),
                Thu = reader.GetString("Thu"),
                TietBatDau = reader.GetInt32("TietBatDau"),
                TuNgay = reader.GetDateTime("TuNgay"),
                DenNgay = reader.GetDateTime("DenNgay"),
                TietKetThuc = reader.GetInt32("TietKetThuc"),
                SoTiet = reader.GetInt32("SoTiet"),
                Type = reader.GetString("Type")
            });

        return result;
    }

    public bool HardDelete(int maLH)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"DELETE FROM LichHoc WHERE MaLH = @MaLH";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLH", maLH);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }
}