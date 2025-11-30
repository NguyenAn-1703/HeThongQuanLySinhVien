namespace QuanLySinhVien.Shared;

public class ConvertTietToGio
{
    public static (DateTime Start, DateTime End) GetThoiGianTiet(int tiet, DateTime ngay)
    {
        // Tiết sáng (1–5)
        if (tiet >= 1 && tiet <= 5)
        {
            DateTime start = ngay.Date.AddHours(7); // 07:00 là tiết 1

            // Thêm phút cho các tiết sau
            // Mỗi tiết 50 phút → nhân (tiet - 1) * 50
            start = start.AddMinutes((tiet - 1) * 50);

            // Nếu là tiết 3, 4, 5 → phải tính thêm thời gian nghỉ sau tiết 2
            if (tiet >= 3)
                start = start.AddMinutes(20); // nghỉ 20 phút sau tiết 2

            DateTime end = start.AddMinutes(50);
            return (start, end);
        }

        // Tiết chiều (6–10)
        if (tiet >= 6 && tiet <= 10)
        {
            DateTime start = ngay.Date.AddHours(13); // 13:00 là tiết 6

            // Vị trí trong buổi chiều (tiết 6 = 1, tiết 7 = 2, ...)
            int pos = tiet - 6;

            start = start.AddMinutes(pos * 50);

            // Nghỉ sau tiết 7 → ảnh hưởng từ tiết 8 trở đi
            if (tiet >= 8)
                start = start.AddMinutes(20);

            DateTime end = start.AddMinutes(50);
            return (start, end);
        }

        throw new Exception("Tiết không hợp lệ (chỉ từ 1 đến 10).");
    }

    public static void TestGetThoiGianTiet()
    {
        DateTime ngay = DateTime.Parse("2025-11-28"); // em muốn đổi ngày thì thay ở đây

        for (int tiet = 1; tiet <= 10; tiet++)
        {
            var tg = GetThoiGianTiet(tiet, ngay);
            Console.WriteLine($"Tiết {tiet}: {tg.Start:HH:mm} -> {tg.End:HH:mm}");
        }
    }

    
}