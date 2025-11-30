using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Model.DAO;

public class LichSD_NhomHPDao
{
    private static LichSD_NhomHPDao _instance;

    private LichSD_NhomHPDao() { }

    public static LichSD_NhomHPDao GetInstance()
    {
        if (_instance == null) _instance = new LichSD_NhomHPDao();
        return _instance;
    }

    // ==============================
    // GET ALL
    // ==============================
    public List<LichSD_NhomHPDto> GetAll()
    {
        List<LichSD_NhomHPDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaLSD, MaNHP FROM LichSD_NhomHP", conn);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new LichSD_NhomHPDto
            {
                MaLSD = reader.GetInt32("MaLSD"),
                MaNHP = reader.GetInt32("MaNHP")
            });
        }

        return result;
    }

    // ==============================
    // INSERT
    // ==============================
    public bool Insert(LichSD_NhomHPDto dto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO LichSD_NhomHP (MaLSD, MaNHP)
                          VALUES (@MaLSD, @MaNHP)";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLSD", dto.MaLSD);
                cmd.Parameters.AddWithValue("@MaNHP", dto.MaNHP);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // ==============================
    // DELETE thật
    // ==============================
    public bool Delete(int maLSD, int maNHP)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"DELETE FROM LichSD_NhomHP
                          WHERE MaLSD = @MaLSD AND MaNHP = @MaNHP";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLSD", maLSD);
                cmd.Parameters.AddWithValue("@MaNHP", maNHP);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // ==============================
    // GET BY ID
    // ==============================
    public LichSD_NhomHPDto GetById(int maLSD, int maNHP)
    {
        LichSD_NhomHPDto result = null;

        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaLSD, MaNHP
              FROM LichSD_NhomHP
              WHERE MaLSD = @MaLSD AND MaNHP = @MaNHP", conn);

        cmd.Parameters.AddWithValue("@MaLSD", maLSD);
        cmd.Parameters.AddWithValue("@MaNHP", maNHP);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new LichSD_NhomHPDto
            {
                MaLSD = reader.GetInt32("MaLSD"),
                MaNHP = reader.GetInt32("MaNHP")
            };
        }

        return result;
    }
}
