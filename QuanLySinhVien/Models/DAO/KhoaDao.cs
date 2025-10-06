using System.Data;
using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO
{
    public class KhoaDao
    {
        // thông tin database local
        
        // lấy danh sách khoa ( hàm dùng mỗi ln loadData )
        public DataTable GetAllKhoa()
        {
            using (MySqlConnection conn = MyConnection.GetConnection())
            {
                // check status = 1
                string query = @"SELECT 
                                MaKhoa AS 'Mã khoa',
                                TenKhoa AS 'Tên khoa',
                                Email,
                                DiaChi AS 'Địa chỉ'
                                FROM Khoa
                                WHERE Status = 1";

                MySqlDataAdapter da = new MySqlDataAdapter(query,conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // add Khoa
        public void InsertKhoa(string tenKhoa, string email, string diaChi)
        {
            using (MySqlConnection conn = MyConnection.GetConnection())
            {
                // status = 1, id auto +1
                string query = @"INSERT INTO Khoa (TenKhoa, Email, DiaChi)
                                 VALUES (@TenKhoa, @Email, @DiaChi)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenKhoa", tenKhoa);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // edit khoa -> get id = getById call form controller
        public void UpdateKhoa(int maKhoa, string tenKhoa, string email, string diaChi)
        {
            using (MySqlConnection conn = MyConnection.GetConnection())
            {
                string query = @"UPDATE Khoa 
                                 SET TenKhoa = @TenKhoa,
                                     Email = @Email,
                                     DiaChi = @DiaChi
                                 WHERE MaKhoa = @MaKhoa";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                    cmd.Parameters.AddWithValue("@TenKhoa", tenKhoa);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // delete khoa
        public void DeleteKhoa(int maKhoa)
        {
            using (MySqlConnection conn = MyConnection.GetConnection())
            {
                // update status = 0 
                string query = @"UPDATE Khoa
                               SET Status = 0
                               WHERE MaKhoa = @MaKhoa;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        // id -> data (1row)
        public DataRow GetKhoaById(int maKhoa)
        {
            using (MySqlConnection conn = MyConnection.GetConnection())
            {
                string query = @"SELECT MaKhoa, TenKhoa, Email, DiaChi, Status 
                         FROM Khoa 
                         WHERE MaKhoa = @MaKhoa AND Status = 1"; // chỉ lấy khoa đang active
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0]; // trả về dòng đầu tiên
                        else
                            return null;
                    }
                }
            }
        }

    }
}
