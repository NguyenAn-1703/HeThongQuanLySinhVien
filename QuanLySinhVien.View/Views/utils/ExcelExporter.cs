using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace QuanLySinhVien.View.utils;

public class ExcelExporter
{
    public static void ExportToExcel<T>(
        List<T> data,
        string sheetName = "Sheet1",
        string? reportTitle = null,
        Dictionary<string, string>? columnHeaders = null)
    {
        // Chọn nơi lưu file
        using (SaveFileDialog sfd = new SaveFileDialog()
               {
                   Filter = "Excel files (*.xlsx)|*.xlsx",
                   Title = "Chọn nơi lưu file Excel",
                   FileName = $"{sheetName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
               })
        {
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            // ✅ Khai báo license (EPPlus v7)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add(sheetName);
                int startRow = 1;

                // --- Tiêu đề báo cáo ---
                if (!string.IsNullOrEmpty(reportTitle))
                {
                    ws.Cells[startRow, 1].Value = reportTitle;
                    ws.Cells[startRow, 1, startRow, typeof(T).GetProperties().Length].Merge = true;
                    ws.Cells[startRow, 1].Style.Font.Size = 16;
                    ws.Cells[startRow, 1].Style.Font.Bold = true;
                    ws.Cells[startRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    startRow += 1;
                }

                var props = typeof(T).GetProperties();
                int col = 1;

                // --- Header ---
                foreach (var prop in props)
                {
                    string headerName = prop.Name;
                    if (columnHeaders != null && columnHeaders.ContainsKey(prop.Name))
                        headerName = columnHeaders[prop.Name];

                    ws.Cells[startRow, col].Value = headerName;
                    ws.Cells[startRow, col].Style.Font.Bold = true;
                    ws.Cells[startRow, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[startRow, col].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    ws.Cells[startRow, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    col++;
                }

                // --- Dữ liệu ---
                int row = startRow + 1;
                foreach (var item in data)
                {
                    col = 1;
                    foreach (var prop in props)
                    {
                        ws.Cells[row, col].Value = prop.GetValue(item);
                        col++;
                    }

                    row++;
                }

                ws.Cells.AutoFitColumns();

                // Lưu file
                FileInfo fileInfo = new FileInfo(sfd.FileName);
                package.SaveAs(fileInfo);

                MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}