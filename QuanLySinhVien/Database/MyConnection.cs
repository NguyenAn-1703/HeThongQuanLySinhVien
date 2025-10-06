using MySqlConnector;

namespace QuanLySinhVien.Database;

public class MyConnection
{
    private static string _connectionString = "Server=localhost;" +
                                       "Database=quanlysinhvien;" +
                                       "Uid=root;" +
                                       "Pwd=;";

    public static MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}