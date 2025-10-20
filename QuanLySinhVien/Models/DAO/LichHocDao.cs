using System;
using System.Collections.Generic;
using MySqlConnector;
using QuanLySinhVien.Database;

namespace QuanLySinhVien.Models.DAO
{
    public class LichHocDao
    {
        public List<LichHocDto> GetLichHocByPhongAndDate(int maPH, DateTime date)
        {
            var result = new List<LichHocDto>();
            int thu = ((int)date.DayOfWeek == 0) ? 8 : ((int)date.DayOfWeek + 1);

            string query = @"
                SELECT 
                    lh.MaLH, lh.MaPH, lh.MaNHP, lh.Thu, 
                    lh.TietBatDau, lh.TuNgay, lh.DenNgay, lh.TietKetThuc, lh.SoTiet,
                    ph.TenPH,
                    hp.TenHP,
                    gv.TenGV,
                    nhp.SiSo
                FROM LichHoc lh
                INNER JOIN PhongHoc ph ON lh.MaPH = ph.MaPH
                INNER JOIN NhomHocPhan nhp ON lh.MaNHP = nhp.MaNHP
                INNER JOIN HocPhan hp ON nhp.MaHP = hp.MaHP
                INNER JOIN GiangVien gv ON nhp.MaGV = gv.MaGV
                WHERE lh.MaPH = @MaPH 
                    AND lh.Thu = @Thu
                    AND @CurrentDate BETWEEN lh.TuNgay AND lh.DenNgay
                    AND lh.Status = 1
                ORDER BY lh.TietBatDau";

            try
            {
                using var connection = MyConnection.GetConnection();
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaPH", maPH);
                command.Parameters.AddWithValue("@Thu", thu);
                command.Parameters.AddWithValue("@CurrentDate", date.Date);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new LichHocDto
                    {
                        MaLH = reader.GetInt32("MaLH"),
                        MaPH = reader.GetInt32("MaPH"),
                        MaNHP = reader.GetInt32("MaNHP"),
                        Thu = reader.GetInt32("Thu"),
                        TietBatDau = reader.GetInt32("TietBatDau"),
                        TuNgay = reader.GetDateTime("TuNgay"),
                        DenNgay = reader.GetDateTime("DenNgay"),
                        TietKetThuc = reader.GetInt32("TietKetThuc"),
                        SoTiet = reader.GetInt32("SoTiet"),
                        TenPH = reader.GetString("TenPH"),
                        TenHP = reader.GetString("TenHP"),
                        TenGV = reader.GetString("TenGV"),
                        SiSo = reader.GetInt32("SiSo")
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy lịch học: {ex.Message}");
            }

            return result;
        }
    }
}
