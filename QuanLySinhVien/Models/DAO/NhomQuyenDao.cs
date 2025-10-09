using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO;

public class NhomQuyenDao
{
    private static NhomQuyenDao _instance;
    private NhomQuyenDao() { }

    public static NhomQuyenDao GetInstance()
    {
        if (_instance == null)
        {
            _instance = new NhomQuyenDao();
        }
        return _instance;
    }

    // Lấy tất cả nhóm quyền còn hoạt động (Status = 1)
    public List<NhomQuyenDto> GetAll()
    {
        List<NhomQuyenDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNQ, TenNhomQuyen FROM nhomquyen WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new NhomQuyenDto
            {
                MaNQ = reader.GetInt32("MaNQ"),
                TenNhomQuyen = reader.GetString("TenNhomQuyen"),
            });
        }

        return result;
    }

    public bool Insert(NhomQuyenDto nhomQuyen)
    {
        int rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        string query = @"INSERT INTO nhomquyen (TenNhomQuyen)
                         VALUES (@TenNhomQuyen)";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@TenNhomQuyen", nhomQuyen.TenNhomQuyen);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public bool Update(NhomQuyenDto nhomQuyen)
    {
        int rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        string query = @"UPDATE nhomquyen 
                         SET TenNhomQuyen = @TenNhomQuyen
                         WHERE MaNQ = @MaNQ AND Status = 1";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaNQ", nhomQuyen.MaNQ);
        cmd.Parameters.AddWithValue("@TenNhomQuyen", nhomQuyen.TenNhomQuyen);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public bool Delete(int maNQ)
    {
        int rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        string query = @"UPDATE nhomquyen
                         SET Status = 0
                         WHERE MaNQ = @MaNQ";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaNQ", maNQ);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public NhomQuyenDto GetById(int maNQ)
    {
        NhomQuyenDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaNQ, TenNhomQuyen FROM nhomquyen WHERE MaNQ = @MaNQ AND Status = 1",
            conn);
        cmd.Parameters.AddWithValue("@MaNQ", maNQ);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result = new NhomQuyenDto
            {
                MaNQ = reader.GetInt32("MaNQ"),
                TenNhomQuyen = reader.GetString("TenNhomQuyen"),
            };
        }

        return result;
    }
}