// File: Models/DAO/PhongHocDao.cs

using MySqlConnector;
using QuanLySinhVien.Database;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Models.DAO;

public class PhongHocDao
{
    // lấy tất cả ph
    public List<PhongHocDto> GetAll()
    {
        var result = new List<PhongHocDto>();
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaPH, TenPH, LoaiPH, CoSo, SucChua, TinhTrang FROM PhongHoc WHERE Status = 1",
            conn
        );
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            result.Add(new PhongHocDto
            {
                MaPH = reader.GetInt32("MaPH"),
                TenPH = reader.GetString("TenPH"),
                LoaiPH = reader.GetString("LoaiPH"),
                CoSo = reader.GetString("CoSo"),
                SucChua = reader.GetInt32("SucChua"),
                TinhTrang = reader.GetString("TinhTrang")
            });
        return result;
    }

    // Thêm ph
    public bool Insert(PhongHocDto phongHocDto)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        const string query = @"
                INSERT INTO PhongHoc (TenPH, LoaiPH, CoSo, SucChua, TinhTrang)
                VALUES (@TenPH, @LoaiPH, @CoSo, @SucChua, @TinhTrang);";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@TenPH", phongHocDto.TenPH);
        cmd.Parameters.AddWithValue("@LoaiPH", phongHocDto.LoaiPH);
        cmd.Parameters.AddWithValue("@CoSo", phongHocDto.CoSo);
        cmd.Parameters.AddWithValue("@SucChua", phongHocDto.SucChua);
        cmd.Parameters.AddWithValue("@TinhTrang", phongHocDto.TinhTrang);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    // sửa ph
    public bool Update(PhongHocDto phongHocDto)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        const string query = @"
                UPDATE PhongHoc 
                SET TenPH = @TenPH,
                    LoaiPH = @LoaiPH,
                    CoSo = @CoSo,
                    SucChua = @SucChua,
                    TinhTrang = @TinhTrang
                WHERE MaPH = @MaPH;";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@TenPH", phongHocDto.TenPH);
        cmd.Parameters.AddWithValue("@LoaiPH", phongHocDto.LoaiPH);
        cmd.Parameters.AddWithValue("@CoSo", phongHocDto.CoSo);
        cmd.Parameters.AddWithValue("@SucChua", phongHocDto.SucChua);
        cmd.Parameters.AddWithValue("@TinhTrang", phongHocDto.TinhTrang);
        cmd.Parameters.AddWithValue("@MaPH", phongHocDto.MaPH);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    // xóa phòng học
    public bool Delete(int maPH)
    {
        var rowAffected = 0;
        using var conn = MyConnection.GetConnection();
        const string query = @"UPDATE PhongHoc SET Status = 0 WHERE MaPH = @MaPH;";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaPH", maPH);

        rowAffected = cmd.ExecuteNonQuery();
        return rowAffected > 0;
    }

    // id -> tt ph
    public PhongHocDto GetById(int maPH)
    {
        PhongHocDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaPH, TenPH, LoaiPH, CoSo, SucChua, TinhTrang FROM PhongHoc WHERE Status = 1 AND MaPH = @MaPH",
            conn);
        cmd.Parameters.AddWithValue("@MaPH", maPH);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            result = new PhongHocDto
            {
                MaPH = reader.GetInt32("MaPH"),
                TenPH = reader.GetString("TenPH"),
                LoaiPH = reader.GetString("LoaiPH"),
                CoSo = reader.GetString("CoSo"),
                SucChua = reader.GetInt32("SucChua"),
                TinhTrang = reader.GetString("TinhTrang")
            };
        return result;
    }

    public PhongHocDto GetByTen(string TenPH)
    {
        PhongHocDto result = null;
        using var conn = MyConnection.GetConnection();
        using var cmd = new MySqlCommand(
            "SELECT MaPH, TenPH, LoaiPH, CoSo, SucChua, TinhTrang FROM PhongHoc WHERE Status = 1 AND TenPH = @TenPH",
            conn);
        cmd.Parameters.AddWithValue("@TenPH", TenPH);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            result = new PhongHocDto
            {
                MaPH = reader.GetInt32("MaPH"),
                TenPH = reader.GetString("TenPH"),
                LoaiPH = reader.GetString("LoaiPH"),
                CoSo = reader.GetString("CoSo"),
                SucChua = reader.GetInt32("SucChua"),
                TinhTrang = reader.GetString("TinhTrang")
            };
        return result;
    }

    // lấy chi tiết lịch học ( theo ngày hiện tại )
    // maPH, tgian -> list TietHoc
    public List<LichHocChiTietDto> GetLichHocTrongNgay(int maPH, DateTime ngay)
    {
        var tmp = new List<LichHocChiTietDto>();
        var thu = (int)ngay.DayOfWeek + 1;
        using var conn = MyConnection.GetConnection();
        const string query = @"
                        SELECT 
                            hp.TenHocPhan,
                            gv.TenGV,
                            ph.TenPH,
                            lh.TietBatDau,
                            lh.TietKetThuc
                        FROM 
                            LichHoc lh
                        JOIN 
                            NhomHocPhan nhp ON lh.MaNHP = nhp.MaNHP
                        JOIN 
                            GiangVien gv ON nhp.MaGV = gv.MaGV
                        JOIN 
                            HocPhan hp ON nhp.MaHocPhan = hp.MaHocPhan
                        JOIN
                            PhongHoc ph ON lh.MaPH = ph.MaPH
                        WHERE 
                            lh.MaPH = @MaPH
                            AND @Ngay BETWEEN lh.TuNgay AND lh.DenNgay
                            AND DAYOFWEEK(@Ngay) = lh.Thu;
                    ";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@MaPH", maPH);
        cmd.Parameters.AddWithValue("@thu", thu);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            tmp.Add(new LichHocChiTietDto
            {
                TenLopHocPhan = reader.GetString("TenLopHocPhan"),
                TenGiangVien = reader.GetString("TenGiangVien"),
                TenPH = reader.GetString("TenPH"),
                TietBatDau = reader.GetInt32("TietBatDau"),
                TietKetThuc = reader.GetInt32("TietKetThuc")
            });

        return tmp;
    }

    // test
    public static void TestPhongHocDAO()
    {
        var dao = new PhongHocDao();

        Console.WriteLine("=== TEST PHONG HOC DAO ===");

        // 🧩 1. Insert mới
        Console.WriteLine("\n--- INSERT ---");
        var newPhongHoc = new PhongHocDto
        {
            TenPH = "Phòng A1.101",
            LoaiPH = "Lý thuyết",
            CoSo = "Cơ sở 1",
            SucChua = 100,
            TinhTrang = "Tốt"
        };
        var insertResult = dao.Insert(newPhongHoc);
        Console.WriteLine(insertResult ? "✅ Thêm mới thành công!" : "❌ Thêm mới thất bại!");

        // 🧩 2. Lấy danh sách tất cả
        Console.WriteLine("\n--- GET ALL ---");
        var allPhongHoc = dao.GetAll();
        var lastId = 0;
        foreach (var p in allPhongHoc)
        {
            Console.WriteLine($"ID: {p.MaPH}, Tên: {p.TenPH}, Sức chứa: {p.SucChua}, Tình trạng: {p.TinhTrang}");
            lastId = p.MaPH; // Lấy ID cuối cùng để test
        }

        // 🧩 3. Lấy 1 phòng theo ID
        Console.WriteLine($"\n--- GET BY ID (MaPH = {lastId}) ---");
        var phongHoc = dao.GetById(lastId);
        if (phongHoc != null)
            Console.WriteLine(
                $"Tên phòng: {phongHoc.TenPH}, Loại: {phongHoc.LoaiPH}, Tình trạng: {phongHoc.TinhTrang}");
        else
            Console.WriteLine($"❌ Không tìm thấy phòng có ID = {lastId}");

        // 🧩 4. Update
        Console.WriteLine($"\n--- UPDATE (MaPH = {lastId}) ---");
        if (phongHoc != null)
        {
            phongHoc.TenPH = "Phòng A1.101 (Đã sửa)";
            phongHoc.TinhTrang = "Đang bảo trì";
            var updateResult = dao.Update(phongHoc);
            Console.WriteLine(updateResult ? "✅ Cập nhật thành công!" : "❌ Cập nhật thất bại!");
        }

        // 🧩 5. Xóa
        Console.WriteLine($"\n--- DELETE (MaPH = {lastId}) ---");
        var deleteResult = dao.Delete(lastId);
        Console.WriteLine(deleteResult ? "✅ Xóa thành công!" : "❌ Xóa thất bại!");

        Console.WriteLine("\n=== KẾT THÚC TEST ===");
    }
}