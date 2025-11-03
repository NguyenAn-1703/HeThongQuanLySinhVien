using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class ChuongTrinhDaoTaoDao
{
    private static ChuongTrinhDaoTaoDao _instance;

    private ChuongTrinhDaoTaoDao()
    {
    }

    public static ChuongTrinhDaoTaoDao GetInstance()
    {
        if (_instance == null)
            _instance = new ChuongTrinhDaoTaoDao();
        return _instance;
    }

    public List<ChuongTrinhDaoTaoDto> GetAll()
    {
        List<ChuongTrinhDaoTaoDto> result = new();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaCTDT, MaCKDT, MaNganh, LoaiHinhDT, TrinhDo 
              FROM chuongtrinhdaotao WHERE Status = 1", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new ChuongTrinhDaoTaoDto
            {
                MaCTDT = reader.GetInt32("MaCTDT"),
                MaCKDT = reader.GetInt32("MaCKDT"),
                MaNganh = reader.GetInt32("MaNganh"),
                LoaiHinhDT = reader.GetString("LoaiHinhDT"),
                TrinhDo = reader.GetString("TrinhDo")
            });

        return result;
    }

    public ChuongTrinhDaoTaoDto? GetById(int maCTDT)
    {
        ChuongTrinhDaoTaoDto? result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            @"SELECT MaCTDT, MaCKDT, MaNganh, LoaiHinhDT, TrinhDo 
              FROM chuongtrinhdaotao WHERE MaCTDT = @MaCTDT AND Status = 1", conn);
        cmd.Parameters.AddWithValue("@MaCTDT", maCTDT);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            result = new ChuongTrinhDaoTaoDto
            {
                MaCTDT = reader.GetInt32("MaCTDT"),
                MaCKDT = reader.GetInt32("MaCKDT"),
                MaNganh = reader.GetInt32("MaNganh"),
                LoaiHinhDT = reader.GetString("LoaiHinhDT"),
                TrinhDo = reader.GetString("TrinhDo")
            };

        return result;
    }

    public bool Insert(ChuongTrinhDaoTaoDto dto)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        var query = @"INSERT INTO chuongtrinhdaotao (MaCKDT, MaNganh, LoaiHinhDT, TrinhDo, Status)
                         VALUES (@MaCKDT, @MaNganh, @LoaiHinhDT, @TrinhDo, 1)";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCKDT", dto.MaCKDT);
        cmd.Parameters.AddWithValue("@MaNganh", dto.MaNganh);
        cmd.Parameters.AddWithValue("@LoaiHinhDT", dto.LoaiHinhDT);
        cmd.Parameters.AddWithValue("@TrinhDo", dto.TrinhDo);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public bool Update(ChuongTrinhDaoTaoDto dto)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        var query = @"UPDATE chuongtrinhdaotao
                         SET MaCKDT = @MaCKDT,
                             MaNganh = @MaNganh,
                             LoaiHinhDT = @LoaiHinhDT,
                             TrinhDo = @TrinhDo
                         WHERE MaCTDT = @MaCTDT";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCTDT", dto.MaCTDT);
        cmd.Parameters.AddWithValue("@MaCKDT", dto.MaCKDT);
        cmd.Parameters.AddWithValue("@MaNganh", dto.MaNganh);
        cmd.Parameters.AddWithValue("@LoaiHinhDT", dto.LoaiHinhDT);
        cmd.Parameters.AddWithValue("@TrinhDo", dto.TrinhDo);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public bool Delete(int maCTDT)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        var query = @"UPDATE chuongtrinhdaotao
                         SET Status = 0
                         WHERE MaCTDT = @MaCTDT";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaCTDT", maCTDT);
        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    public int GetLastAutoIncrement()
    {
        var Id = 0;

        var query = "SELECT MAX(MaCTDT) FROM chuongtrinhdaotao";

        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(query, conn);
        var result = cmd.ExecuteScalar();
        if (result != DBNull.Value)
            Id = Convert.ToInt32(result);

        return Id;
    }
}