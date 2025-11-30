using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class NhomHocPhanDao
{
    private static NhomHocPhanDao _instance;

    private NhomHocPhanDao() { }

    public static NhomHocPhanDao GetInstance()
    {
        if (_instance == null) _instance = new NhomHocPhanDao();
        return _instance;
    }

    public List<NhomHocPhanDto> GetAll()
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaDotDK, MaLop, HocKy, Nam, SiSo, SiSoToiDa FROM NhomHocPhan WHERE Status = 1",
            conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new NhomHocPhanDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaGV = reader.GetInt32("MaGV"),
                MaHP = reader.GetInt32("MaHP"),
                MaDotDK = reader.GetInt32("MaDotDK"),
                MaLop = reader.GetInt32("MaLop"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                SiSo = reader.GetInt32("SiSo"),
                SiSoToiDa = reader.GetInt32("SiSoToiDa")
            });

        return result;
    }

    public bool Insert(NhomHocPhanDto dto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO NhomHocPhan 
                            (MaGV, MaHP, MaDotDK, MaLop, HocKy, Nam, SiSo, SiSoToiDa)
                          VALUES 
                            (@MaGV, @MaHP, @MaDotDK, @MaLop, @HocKy, @Nam, @SiSo, @SiSoToiDa)";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaGV", dto.MaGV);
                cmd.Parameters.AddWithValue("@MaHP", dto.MaHP);
                cmd.Parameters.AddWithValue("@MaDotDK", dto.MaDotDK);
                cmd.Parameters.AddWithValue("@MaLop", dto.MaLop);
                cmd.Parameters.AddWithValue("@HocKy", dto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", dto.Nam);
                cmd.Parameters.AddWithValue("@SiSo", dto.SiSo);
                cmd.Parameters.AddWithValue("@SiSoToiDa", dto.SiSoToiDa);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Update(NhomHocPhanDto dto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE NhomHocPhan
                             SET MaGV = @MaGV,
                                 MaHP = @MaHP,
                                 MaDotDK = @MaDotDK,
                                 MaLop = @MaLop,
                                 HocKy = @HocKy,
                                 Nam = @Nam,
                                 SiSo = @SiSo,
                                 SiSoToiDa = @SiSoToiDa
                             WHERE MaNHP = @MaNHP";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNHP", dto.MaNHP);
                cmd.Parameters.AddWithValue("@MaGV", dto.MaGV);
                cmd.Parameters.AddWithValue("@MaHP", dto.MaHP);
                cmd.Parameters.AddWithValue("@MaDotDK", dto.MaDotDK);
                cmd.Parameters.AddWithValue("@MaLop", dto.MaLop);
                cmd.Parameters.AddWithValue("@HocKy", dto.HocKy);
                cmd.Parameters.AddWithValue("@Nam", dto.Nam);
                cmd.Parameters.AddWithValue("@SiSo", dto.SiSo);
                cmd.Parameters.AddWithValue("@SiSoToiDa", dto.SiSoToiDa);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public bool Delete(int maNHP)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"UPDATE NhomHocPhan
                             SET Status = 0
                             WHERE MaNHP = @MaNHP";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNHP", maNHP);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    public NhomHocPhanDto GetById(int maNHP)
    {
        NhomHocPhanDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaDotDK, MaLop, HocKy, Nam, SiSo, SiSoToiDa " +
            "FROM NhomHocPhan WHERE Status = 1 AND MaNHP = @MaNHP",
            conn);

        cmd.Parameters.AddWithValue("@MaNHP", maNHP);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            result = new NhomHocPhanDto
            {
                MaNHP = reader.GetInt32("MaNHP"),
                MaGV = reader.GetInt32("MaGV"),
                MaHP = reader.GetInt32("MaHP"),
                MaDotDK = reader.GetInt32("MaDotDK"),
                MaLop = reader.GetInt32("MaLop"),
                HocKy = reader.GetInt32("HocKy"),
                Nam = reader.GetString("Nam"),
                SiSo = reader.GetInt32("SiSo"),
                SiSoToiDa = reader.GetInt32("SiSoToiDa")
            };
        }

        return result;
    }

    private NhomHocPhanDto ReadNHP(MySqlDataReader reader)
    {
        return new NhomHocPhanDto
        {
            MaNHP = reader.GetInt32("MaNHP"),
            MaGV = reader.GetInt32("MaGV"),
            MaHP = reader.GetInt32("MaHP"),
            MaDotDK = reader.GetInt32("MaDotDK"),
            MaLop = reader.GetInt32("MaLop"),
            HocKy = reader.GetInt32("HocKy"),
            Nam = reader.GetString("Nam"),
            SiSo = reader.GetInt32("SiSo"),
            SiSoToiDa = reader.GetInt32("SiSoToiDa")
        };
    }

    public List<NhomHocPhanDto> GetByLichMaDangKy(int MaDotDK)
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaDotDK, MaLop, HocKy, Nam, SiSo, SiSoToiDa " +
            "FROM NhomHocPhan WHERE Status = 1 AND MaDotDK = @MaDotDK",
            conn);
        cmd.Parameters.AddWithValue("@MaDotDK", MaDotDK);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(ReadNHP(reader));

        return result;
    }

    public List<NhomHocPhanDto> GetByHkyNamMaHP(int hky, string nam, int maHp)
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaDotDK, MaLop, HocKy, Nam, SiSo, SiSoToiDa " +
            "FROM NhomHocPhan WHERE Status = 1 AND HocKy = @HocKy AND Nam = @Nam AND MaHP = @MaHP",
            conn);

        cmd.Parameters.AddWithValue("@HocKy", hky);
        cmd.Parameters.AddWithValue("@Nam", nam);
        cmd.Parameters.AddWithValue("@MaHP", maHp);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(ReadNHP(reader));

        return result;
    }

    public List<NhomHocPhanDto> GetByHkyNam(int hky, string nam)
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaDotDK, MaLop, HocKy, Nam, SiSo, SiSoToiDa " +
            "FROM NhomHocPhan WHERE Status = 1 AND HocKy = @HocKy AND Nam = @Nam",
            conn);

        cmd.Parameters.AddWithValue("@HocKy", hky);
        cmd.Parameters.AddWithValue("@Nam", nam);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(ReadNHP(reader));

        return result;
    }

    public List<NhomHocPhanDto> GetByHkyNamMaGV(int hky, string nam, int maGV)
    {
        List<NhomHocPhanDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNHP, MaGV, MaHP, MaDotDK, MaLop, HocKy, Nam, SiSo, SiSoToiDa " +
            "FROM NhomHocPhan WHERE Status = 1 AND HocKy = @HocKy AND Nam = @Nam AND MaGV = @MaGV",
            conn);

        cmd.Parameters.AddWithValue("@HocKy", hky);
        cmd.Parameters.AddWithValue("@Nam", nam);
        cmd.Parameters.AddWithValue("@MaGV", maGV);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(ReadNHP(reader));

        return result;
    }
}
