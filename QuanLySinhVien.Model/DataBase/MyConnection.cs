using MySqlConnector;

namespace QuanLySinhVien.Database;

public class MyConnection
{
    private static readonly string _connectionString = "Server=localhost;" +
                                                       "Database=quanlysinhvien;" +
                                                       "Uid=root;" +
                                                       "Pwd=bi2552453;";

    public static MySqlConnection GetConnection()
    {
        try
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        catch (Exception e)
        {
            Console.WriteLine("Loi ket noi databee x" + e.Message);
            throw;
        }
    }
}