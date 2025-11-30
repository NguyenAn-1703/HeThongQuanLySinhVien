using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class HocPhanDao
{
    private static HocPhanDao _instance;

    private HocPhanDao() { }

    public static HocPhanDao GetInstance()
    {
        if (_instance == null) _instance = new HocPhanDao();
        return _instance;
    }

    // =============================
    //           GET ALL
    // =============================
    public List<HocPhanDto> GetAll()
    {
        List<HocPhanDto> result = new();

        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaHP, MaHPTruoc, TenHP, SoTinChi, HeSoDiem, HeSoHocPhan, 
                     SoTietLyThuyet, SoTietThucHanh, MaKhoa
              FROM hocphan 
              WHERE Status = 1", conn);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new HocPhanDto
            {
                MaHP = reader.GetInt32("MaHP"),
                MaHPTruoc = reader.IsDBNull(reader.GetOrdinal("MaHPTruoc")) ? null : reader.GetInt32(reader.GetOrdinal("MaHPTruoc")),
                TenHP = reader.GetString("TenHP"),
                SoTinChi = reader.GetInt32("SoTinChi"),
                HeSoDiem = reader.GetString("HeSoDiem"),
                HeSoHocPhan = reader.GetFloat("HeSoHocPhan"),
                SoTietLyThuyet = reader.GetInt32("SoTietLyThuyet"),
                SoTietThucHanh = reader.GetInt32("SoTietThucHanh"),
                MaKhoa = reader.GetInt32("MaKhoa"),
                Status = 1
            });
        }

        return result;
    }

    // =============================
    //            INSERT
    // =============================
    public bool Insert(HocPhanDto dto)
    {
        using var conn = MyConnection.GetConnection();

        string query = @"
            INSERT INTO hocphan 
                (MaHPTruoc, TenHP, SoTinChi, HeSoDiem, HeSoHocPhan, 
                 SoTietLyThuyet, SoTietThucHanh, MaKhoa, Status)
            VALUES 
                (@MaHPTruoc, @TenHP, @SoTinChi, @HeSoDiem, @HeSoHocPhan, 
                 @SoTietLyThuyet, @SoTietThucHanh, @MaKhoa, 1)
        ";

        using var cmd = new MySqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@MaHPTruoc", dto.MaHPTruoc ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@TenHP", dto.TenHP);
        cmd.Parameters.AddWithValue("@SoTinChi", dto.SoTinChi);
        cmd.Parameters.AddWithValue("@HeSoDiem", dto.HeSoDiem);
        cmd.Parameters.AddWithValue("@HeSoHocPhan", dto.HeSoHocPhan);
        cmd.Parameters.AddWithValue("@SoTietLyThuyet", dto.SoTietLyThuyet);
        cmd.Parameters.AddWithValue("@SoTietThucHanh", dto.SoTietThucHanh);
        cmd.Parameters.AddWithValue("@MaKhoa", dto.MaKhoa);

        return cmd.ExecuteNonQuery() > 0;
    }

    // =============================
    //            UPDATE
    // =============================
    public bool Update(HocPhanDto dto)
    {
        using var conn = MyConnection.GetConnection();

        string query = @"
            UPDATE hocphan SET 
                MaHPTruoc = @MaHPTruoc,
                TenHP = @TenHP,
                SoTinChi = @SoTinChi,
                HeSoDiem = @HeSoDiem,
                HeSoHocPhan = @HeSoHocPhan,
                SoTietLyThuyet = @SoTietLyThuyet,
                SoTietThucHanh = @SoTietThucHanh,
                MaKhoa = @MaKhoa
            WHERE MaHP = @MaHP
        ";

        using var cmd = new MySqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@MaHPTruoc", dto.MaHPTruoc ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@TenHP", dto.TenHP);
        cmd.Parameters.AddWithValue("@SoTinChi", dto.SoTinChi);
        cmd.Parameters.AddWithValue("@HeSoDiem", dto.HeSoDiem);
        cmd.Parameters.AddWithValue("@HeSoHocPhan", dto.HeSoHocPhan);
        cmd.Parameters.AddWithValue("@SoTietLyThuyet", dto.SoTietLyThuyet);
        cmd.Parameters.AddWithValue("@SoTietThucHanh", dto.SoTietThucHanh);
        cmd.Parameters.AddWithValue("@MaKhoa", dto.MaKhoa);
        cmd.Parameters.AddWithValue("@MaHP", dto.MaHP);

        return cmd.ExecuteNonQuery() > 0;
    }

    // =============================
    //        SOFT DELETE
    // =============================
    public bool Delete(int maHP)
    {
        using var conn = MyConnection.GetConnection();

        string query = @"UPDATE hocphan SET Status = 0 WHERE MaHP = @MaHP";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaHP", maHP);

        return cmd.ExecuteNonQuery() > 0;
    }

    // =============================
    //         GET BY ID
    // =============================
    public HocPhanDto? GetHocPhanById(int maHP)
    {
        using var conn = MyConnection.GetConnection();

        using var cmd = new MySqlCommand(
            @"SELECT MaHP, MaHPTruoc, TenHP, SoTinChi, HeSoDiem, HeSoHocPhan,
                     SoTietLyThuyet, SoTietThucHanh, MaKhoa
              FROM hocphan 
              WHERE MaHP = @MaHP AND Status = 1", conn);

        cmd.Parameters.AddWithValue("@MaHP", maHP);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read()) return null;

        return new HocPhanDto
        {
            MaHP = reader.GetInt32("MaHP"),
            MaHPTruoc = reader.IsDBNull(reader.GetOrdinal("MaHPTruoc")) ? null : reader.GetInt32(reader.GetOrdinal("MaHPTruoc")),
            TenHP = reader.GetString("TenHP"),
            SoTinChi = reader.GetInt32("SoTinChi"),
            HeSoDiem = reader.GetString("HeSoDiem"),
            HeSoHocPhan = reader.GetFloat("HeSoHocPhan"),
            SoTietLyThuyet = reader.GetInt32("SoTietLyThuyet"),
            SoTietThucHanh = reader.GetInt32("SoTietThucHanh"),
            MaKhoa = reader.GetInt32("MaKhoa"),
            Status = 1
        };
    }

    // =============================
    //         GET BY NAME
    // =============================
    public HocPhanDto? GetHocPhanByTen(string tenHP)
    {
        using var conn = MyConnection.GetConnection();

        using var cmd = new MySqlCommand(
            @"SELECT MaHP, MaHPTruoc, TenHP, SoTinChi, HeSoDiem, HeSoHocPhan,
                     SoTietLyThuyet, SoTietThucHanh, MaKhoa
              FROM hocphan 
              WHERE TenHP = @TenHP AND Status = 1", conn);

        cmd.Parameters.AddWithValue("@TenHP", tenHP);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read()) return null;

        return new HocPhanDto
        {
            MaHP = reader.GetInt32("MaHP"),
            MaHPTruoc = reader.IsDBNull(reader.GetOrdinal("MaHPTruoc")) ? null : reader.GetInt32(reader.GetOrdinal("MaHPTruoc")),
            TenHP = reader.GetString("TenHP"),
            SoTinChi = reader.GetInt32("SoTinChi"),
            HeSoDiem = reader.GetString("HeSoDiem"),
            HeSoHocPhan = reader.GetFloat("HeSoHocPhan"),
            SoTietLyThuyet = reader.GetInt32("SoTietLyThuyet"),
            SoTietThucHanh = reader.GetInt32("SoTietThucHanh"),
            MaKhoa = reader.GetInt32("MaKhoa"),
            Status = 1
        };
    }

    // =============================
    //   GET LIST FOR COMBOBOX
    // =============================
    public List<string> GetAllForCombobox()
    {
        List<string> result = new();

        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaHP, TenHP FROM hocphan WHERE Status = 1 ORDER BY MaHP", conn);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add($"{reader.GetInt32("MaHP")} - {reader.GetString("TenHP")}");
        }

        return result;
    }
}
