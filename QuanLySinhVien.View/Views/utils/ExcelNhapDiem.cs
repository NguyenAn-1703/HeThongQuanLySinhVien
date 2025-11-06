using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

public static class ExcelDiemHelper
{
    public static Dictionary<string, List<double>> ImportDiem(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var result = new Dictionary<string, List<double>>();

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var ws = package.Workbook.Worksheets[0];
            int rowCount = ws.Dimension.End.Row;
            int colCount = ws.Dimension.End.Column;

            // === 1. Tìm cột có tiêu đề "Mã sinh viên" ===
            int maSvCol = -1;
            for (int col = 1; col <= colCount; col++)
            {
                string header = ws.Cells[1, col].Text.Trim();
                if (header.Equals("Mã sinh viên", StringComparison.OrdinalIgnoreCase))
                {
                    maSvCol = col;
                    break;
                }
            }

            if (maSvCol == -1)
                throw new Exception("Không tìm thấy cột 'Mã sinh viên' trong file Excel!");

            // === 2. Xác định các cột điểm (sau cột mã SV, tối đa 5 cột) ===
            int startDiemCol = maSvCol + 1;
            int endDiemCol = Math.Min(colCount, maSvCol + 5);

            // === 3. Đọc từng dòng dữ liệu ===
            for (int row = 2; row <= rowCount; row++)
            {
                string maSv = ws.Cells[row, maSvCol].Text.Trim();
                if (string.IsNullOrEmpty(maSv)) continue; // bỏ qua dòng trống

                var listDiem = new List<double>();
                for (int col = startDiemCol; col <= endDiemCol; col++)
                {
                    string cellValue = ws.Cells[row, col].Text.Trim();
                    if (double.TryParse(cellValue, out double diem))
                        listDiem.Add(diem);
                }

                result[maSv] = listDiem;
            }
        }

        return result;
    }
}

