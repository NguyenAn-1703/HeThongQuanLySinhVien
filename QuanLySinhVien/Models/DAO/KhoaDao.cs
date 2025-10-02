using System.Data;
using MySql.Data.MySqlClient;

namespace QuanLySinhVien.Models.DAO
{
    public class KhoaDao
    {
        private string _connectionString = "Server=localhost;" +
                                           "Database=quanlysinhvien;" +
                                           "Uid=root;" +
                                           "Pwd=bi2552453;";

        public DataTable GetAllKhoa()
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT 
                MaKhoa AS 'Mã khoa',
                TenKhoa AS 'Tên khoa',
                Email,
                DiaChi AS 'Địa chỉ'
                FROM Khoa";
                
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}