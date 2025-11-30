using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Model.DAO;

public class LichSD_CaThiDao
{
    private static LichSD_CaThiDao _instance;

    private LichSD_CaThiDao() { }

    public static LichSD_CaThiDao GetInstance()
    {
        if (_instance == null) _instance = new LichSD_CaThiDao();
        return _instance;
    }

    // ==============================
    // GET ALL
    // ==============================
    public List<LichSD_CaThiDto> GetAll()
    {
        List<LichSD_CaThiDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaLSD, MaCT FROM LichSD_CaThi", conn);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new LichSD_CaThiDto
            {
                MaLSD = reader.GetInt32("MaLSD"),
                MaCT = reader.GetInt32("MaCT")
            });
        }

        return result;
    }

    // ==============================
    // INSERT
    // ==============================
    public bool Insert(LichSD_CaThiDto dto)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"INSERT INTO LichSD_CaThi (MaLSD, MaCT)
                          VALUES (@MaLSD, @MaCT)";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLSD", dto.MaLSD);
                cmd.Parameters.AddWithValue("@MaCT", dto.MaCT);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // ==============================
    // DELETE thật
    // ==============================
    public bool Delete(int maLSD, int maCT)
    {
        var rowAffected = 0;
        using (var conn = MyConnection.GetConnection())
        {
            var query = @"DELETE FROM LichSD_CaThi
                          WHERE MaLSD = @MaLSD AND MaCT = @MaCT";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaLSD", maLSD);
                cmd.Parameters.AddWithValue("@MaCT", maCT);

                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // ==============================
    // GET BY ID
    // ==============================
    public LichSD_CaThiDto GetById(int maLSD, int maCT)
    {
        LichSD_CaThiDto result = null;

        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaLSD, MaCT 
              FROM LichSD_CaThi 
              WHERE MaLSD = @MaLSD AND MaCT = @MaCT", conn);

        cmd.Parameters.AddWithValue("@MaLSD", maLSD);
        cmd.Parameters.AddWithValue("@MaCT", maCT);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new LichSD_CaThiDto
            {
                MaLSD = reader.GetInt32("MaLSD"),
                MaCT = reader.GetInt32("MaCT")
            };
        }

        return result;
    }
}
