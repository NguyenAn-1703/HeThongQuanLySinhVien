using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

public class NganhDao
{
    // thông tin database local

    // lấy danh sách khoa ( hàm dùng mỗi ln loadData )

    private static NganhDao _instance;

    private NganhDao()
    {
    }

    public static NganhDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new NganhDao();
        }

        return _instance;
    }


    public List<NganhDto> GetAll()
    {
        List<NganhDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand("SELECT MaNganh, MaKhoa, TenNganh FROM nganh WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new NganhDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh")
            });
        }

        return result;
    }

    public bool Insert(NganhDto nganhDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            // status = 1, id auto +1
            string query = @"INSERT INTO nganh (MaKhoa, TenNganh)
                                 VALUES (@MaKhoa, @TenNganh)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhoa", nganhDto.MaKhoa);
                cmd.Parameters.AddWithValue("@TenNganh", nganhDto.TenNganh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // edit khoa -> get id = getById call form controller
    public bool Update(NganhDto nganhDto)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            string query = @"UPDATE nganh 
                                 SET MaKhoa = @MaKhoa,
                                     TenNganh = @TenNganh
                                 WHERE MaNganh = @MaNganh";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhoa", nganhDto.MaKhoa);
                cmd.Parameters.AddWithValue("@TenNganh", nganhDto.TenNganh);
                cmd.Parameters.AddWithValue("@MaNganh", nganhDto.MaNganh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // delete khoa
    public bool Delete(int maNganh)
    {
        int rowAffected = 0;
        using (MySqlConnection conn = MyConnection.GetConnection())
        {
            // update status = 0 
            string query = @"UPDATE nganh
                               SET Status = 0
                               WHERE MaNganh = @MaNganh;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNganh", maNganh);
                rowAffected = cmd.ExecuteNonQuery();
            }
        }

        return rowAffected > 0;
    }

    // id -> data (1row)
    public NganhDto GetNganhById(int maNganh)
    {
        NganhDto result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd =
            new MySqlCommand("SELECT MaNganh, MaKhoa, TenNganh FROM nganh WHERE Status = 1 AND MaNganh = @MaNganh",
                conn);
        cmd.Parameters.AddWithValue("@MaNganh", maNganh);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new NganhDto
            {
                MaKhoa = reader.GetInt32("MaKhoa"),
                MaNganh = reader.GetInt32("MaNganh"),
                TenNganh = reader.GetString("TenNganh")
            };
        }

        return result;
    }
    
    public int CountNganhByStatus(int status)
    {
        using var conn = MyConnection.GetConnection();
        const string sql = "SELECT COUNT(*) AS Total FROM Nganh WHERE Status = @Status";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Status", status);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}