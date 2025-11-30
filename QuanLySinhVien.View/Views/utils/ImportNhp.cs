using Mysqlx.Crud;
using OfficeOpenXml;
using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.View.utils;

public class ImportNhp
{
    private string _filePath;

    private HocPhanController _hocPhanController;
    private GiangVienController _giangVienController;
    private LopController _lopController;
    private PhongHocController _phongHocController;
    
    private NhomHocPhanController _nhomHocPhanController;
    private LichHocController _lichHocController;
    private LichSuDungController _lichSuDungController;
    private LichSD_NhomHPController _lichSD_NhomHPController;

    private List<NhomHocPhanDto> listNhpTam;
    private List<LichHocDto> listLichTam;
    private List<LichSuDungDto> _listLichSuDungForChecking;
    private int tempIdNhp = 0;

    private int STTColIndex = 1;
    private int MaHPColIndex = 2;
    private int MaGVColIndex = 3;
    private int MaLopColIndex = 4;
    private int SiSoToiDaIndex = 5;
    private int TenPHIndex = 6;
    private int ThuIndex = 7;
    private int TietBDIndex = 8;
    private int TietKTIndex = 9;
    private int TuNgayIndex = 10;
    private int CaIndex = 11;

    private int _idDotDK;
    private int _hocKy;
    private string _nam;

    public event Action Finish;
    public ImportNhp(int idDotDk, int hocKy, string nam)
    {
        _idDotDK = idDotDk;
        _hocKy = hocKy;
        _nam = nam;
        Init();
    }

    void Init()
    {
        _hocPhanController = HocPhanController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        _lopController = LopController.GetInstance();
        _phongHocController = PhongHocController.getInstance();
        _lichSuDungController = LichSuDungController.GetInstance();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _lichHocController = LichHocController.GetInstance();
        _lichSD_NhomHPController = LichSD_NhomHPController.GetInstance();
        listNhpTam = new List<NhomHocPhanDto>();
        listLichTam = new List<LichHocDto>();
    }

    public void Import()
    {
        if (!ValidateFile())
        {
            return;
        }

        if (!ValidateSheet())
        {
            return;
        }

        if (!ValidateColumn())
        {
            return;
        }

        if (!ValidateMerge())
        {
            return;
        }

        if (!ValidateLichHocDotDK())
        {
            return;
        }

        Console.Write("import oke");
        Insert();
    }
    
    
    private void Insert()
    {
        foreach (var nhomHp in listNhpTam)
        {
            if (!_nhomHocPhanController.Insert(nhomHp))
            {
                MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            List<NhomHocPhanDto> listnhp = _nhomHocPhanController.GetAll();
            var manhp = listnhp[listnhp.Count - 1].MaNHP;

            List<LichHocDto> listLichHocTam = GetListLichHocByMaNhp(nhomHp.MaNHP);
            
            foreach (var item in listLichHocTam)
            {
                var lich = new LichHocDto
                {
                    MaPH = item.MaPH,
                    MaNHP = manhp,
                    Thu = item.Thu,
                    TietBatDau = item.TietBatDau,
                    TietKetThuc = item.TietKetThuc,
                    TuNgay = item.TuNgay,
                    DenNgay = item.DenNgay,
                    SoTiet = item.SoTiet,
                    Type = item.Type
                };
                if (!_lichHocController.Insert(lich))
                {
                    MessageBox.Show("Thêm lịch thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            List<LichSuDungDto> listLichSuDungTam = GetListLichSuDungByListLich(listLichHocTam);
            
            //insert lichsd
            foreach (LichSuDungDto item in listLichSuDungTam)
            {
                LichSuDungDto lich = new LichSuDungDto
                {
                    MaPH = item.MaPH,
                    ThoiGianBatDau = item.ThoiGianBatDau,
                    ThoiGianKetThuc = item.ThoiGianKetThuc,
                    GhiChu = $"Nhóm học phần {manhp} sử dụng"
                };
                if (!_lichSuDungController.Insert(lich))
                {
                    MessageBox.Show("Thêm lịch sử dụng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maLSD = _lichSuDungController.GetLastId();
                LichSD_NhomHPDto lisd_nhomhp = new()
                {
                    MaLSD = maLSD,
                    MaNHP = manhp
                };
                if (!_lichSD_NhomHPController.Add(lisd_nhomhp))
                {
                    MessageBox.Show("Thêm lịch sử dụng nhóm hp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
        MessageBox.Show("Thêm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Finish?.Invoke();
    }

    public bool ValidateFile()
    {
        using (OpenFileDialog ofd = new OpenFileDialog())
        {
            ofd.Title = "Chọn file Excel để import nhóm học phần";
            ofd.Filter = "Excel Files|*.xlsx;*.xls";
            ofd.Multiselect = false;
            
            // // Nếu user bấm Cancel → bỏ qua, return false để không import
            // if (ofd.ShowDialog() == DialogResult.Cancel)
            // {
            //     return false;
            // }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;

                // --- Validate đường dẫn và file ---
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("File không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                FileInfo info = new FileInfo(filePath);
                if (info.Length == 0)
                {
                    MessageBox.Show("File rỗng, không thể import!", "Lỗi", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }

                _filePath = filePath;
            }
        }

        return true;
    }

    public bool ValidateSheet()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(new FileInfo(_filePath)))
        {
            // Kiểm tra có sheet nào không
            if (package.Workbook.Worksheets.Count == 0)
            {
                MessageBox.Show(
                    "File Excel không có sheet nào!",
                    "Lỗi Import",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            // Kiểm tra sheet1 tồn tại
            var sheet = package.Workbook.Worksheets["Sheet1"];
            if (sheet == null)
            {
                MessageBox.Show(
                    "Không tìm thấy sheet 'Sheet1' trong file Excel!",
                    "Lỗi Import",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            return true; // OK
        }
    }

    public bool ValidateColumn()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(_filePath)))
        {
            var sheet = package.Workbook.Worksheets["Sheet1"];

            // Danh sách cột bắt buộc theo thứ tự
            List<string> requiredHeaders = new List<string>()
            {
                "STT",
                "Mã học phần",
                "Mã giảng viên",
                "Mã lớp",
                "Sĩ số tối đa",
                "Phòng",
                "Thứ",
                "Tiết bắt đầu",
                "Tiết kết thúc",
                "Từ ngày"
            };

            // Kiểm tra từng header
            for (int i = 0; i < requiredHeaders.Count; i++)
            {
                string expected = requiredHeaders[i];
                string actual = sheet.Cells[1, i + 1].Text?.Trim();

                if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        $"Cột số {i + 1} phải là '{expected}' nhưng lại là '{actual}'.\n" +
                        $"Vui lòng kiểm tra lại file Excel.",
                        "Sai định dạng cột",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return false;
                }
            }

            return true; // OK
        }
    }

    public bool ValidateMerge()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(_filePath)))
        {
            var ws = package.Workbook.Worksheets["Sheet1"];

            int startRow = 2;
            int row = startRow;
            while (row <= GetLastRow(ws))
            {
                var cell = ws.Cells[row, 1];
                int mergeRowCount = MergeRowNumber(cell, ws);
                if (!ValidateNhp(ws, row, mergeRowCount)) return false;

                row += mergeRowCount;
            }
        }

        return true;
    }

    public int MergeRowNumber(ExcelRange cell, ExcelWorksheet ws)
    {
        if (cell.Merge)
        {
            // Lấy merge range, nếu không có hàm MergedCells bị trả về null nên cần check
            var mergedAddress = ws.MergedCells[cell.Start.Row, cell.Start.Column];
            if (!string.IsNullOrEmpty(mergedAddress))
            {
                var mergedRange = ws.Cells[mergedAddress];
                // Số dòng = End.Row - Start.Row + 1
                return mergedRange.End.Row - mergedRange.Start.Row + 1;
            }
        }

        // Nếu không merge, trả về 1 dòng
        return 1;
    }

    public bool ValidateNhp(ExcelWorksheet ws, int rowIndex, int mergeCount)
    {
        //check nhp
        string maHps = ws.Cells[rowIndex, MaHPColIndex].Value.ToString() ?? "";
        string maGVs = ws.Cells[rowIndex, MaGVColIndex].Value.ToString() ?? "";
        string maLops = ws.Cells[rowIndex, MaLopColIndex].Value.ToString() ?? "";
        string siSoToiDas = ws.Cells[rowIndex, SiSoToiDaIndex].Value.ToString() ?? "";

        if (!ValidateDataNhomHocPhan(maHps, maGVs, maLops, siSoToiDas, rowIndex))
        {
            return false;
        }

        int maNhp = tempIdNhp++;
        
        NhomHocPhanDto nhp = new NhomHocPhanDto
        {
            MaNHP = maNhp,
            MaDotDK = _idDotDK,
            MaGV = int.Parse(maGVs),
            MaHP = int.Parse(maHps),
            MaLop = int.Parse(maLops),
            HocKy = _hocKy,
            Nam = _nam,
            SiSoToiDa = int.Parse(siSoToiDas),
        };
        listNhpTam.Add(nhp);
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(nhp.MaHP);
        
        //check lich hoc

        int startRow = rowIndex;
        int endRow = rowIndex + mergeCount - 1;
        List<LichHocDto> listLichForValidate = new List<LichHocDto>();
        for (int j = startRow; j <= endRow; j++)
        {
            string tenPH = ws.Cells[j, TenPHIndex].Value.ToString() ?? "";
            string thu = ws.Cells[j, ThuIndex].Value.ToString() ?? "";
            string tietBD = ws.Cells[j, TietBDIndex].Value.ToString() ?? "";
            string tietKT = ws.Cells[j, TietKTIndex].Value.ToString() ?? "";
            string tuNgay = ws.Cells[j, TuNgayIndex].Value.ToString() ?? "";
            string ca = ws.Cells[j, CaIndex].Value.ToString() ?? "";
            if (!ValidateDataLichHoc(tenPH, thu, tietBD, tietKT, tuNgay,ca, rowIndex))
            {
                return false;
            }
            
            LichHocDto lich = new LichHocDto
            {
                MaNHP = maNhp,
                MaPH = _phongHocController.GetByTen(tenPH).MaPH,
                Thu = thu + "",
                TietBatDau = int.Parse(tietBD),
                TietKetThuc = int.Parse(tietKT),
                SoTiet = int.Parse(tietKT) - int.Parse(tietBD),
                TuNgay = ConvertDate.ConvertStringToDate(tuNgay),
                Type = ca,
            };
            
            int soTietHocLich = 0;
            if (ca.Equals("Lý thuyết"))
            {
                soTietHocLich = hocPhan.SoTietLyThuyet;
            }
            else if (ca.Equals("Thực hành"))
            {
                soTietHocLich = hocPhan.SoTietLyThuyet;
            }
            DateTime ngayKetThuc = GetNgayKetThuc(lich.TietBatDau, lich.TietKetThuc, soTietHocLich, lich.TuNgay);
            lich.DenNgay = ngayKetThuc;
            
            if (!ValidateLichHocNhp(listLichForValidate, lich, rowIndex)) return false;
            
            List<LichSuDungDto> listLichSdForValidate = GetListLichSd($"Thứ {lich.Thu}", lich.TietBatDau, lich.TietKetThuc, lich.TuNgay, lich.DenNgay, lich.MaPH);
            PhongHocDto phong = _phongHocController.GetPhongHocById(lich.MaPH);
            if (!ValidateLichTrungDb(phong, listLichSdForValidate, rowIndex)) return false;
                
            listLichForValidate.Add(lich);
            listLichTam.Add(lich);
        }

        return true;
    }

    DateTime GetNgayKetThuc(int tbd, int tkt, int soTiet, DateTime startDate)
    {
        int soTietTrenTuan = tkt - tbd + 1;

        int soTuan = (int)Math.Ceiling((double)soTiet / soTietTrenTuan); //lam tron len
        int soNgay = soTuan * 7;

        var endDate = startDate.AddDays(soNgay);
        return endDate;
    }

    bool ValidateLichHocNhp(List<LichHocDto> listLich, LichHocDto lich, int row)
    {
        foreach (var item in listLich)
        {
            if (lich.TietBatDau <= item.TietKetThuc && item.TietBatDau <= lich.TietKetThuc && item.Thu == lich.Thu)
            {
                MessageBox.Show($"Dòng {row}: Lỗi trùng lịch!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        return true;
    }
    
    bool ValidateLichHocDotDK()
    {
        int n = listLichTam.Count;

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                var a = listLichTam[i];
                var b = listLichTam[j];

                // 1. Khác phòng → không thể trùng
                if (a.MaPH != b.MaPH)
                    continue;

                // 2. Khác thứ → không thể trùng
                if (a.Thu != b.Thu)
                    continue;

                // 3. Kiểm tra trùng tiết
                bool overlap =
                    a.TietBatDau <= b.TietKetThuc &&
                    b.TietBatDau <= a.TietKetThuc;

                if (overlap)
                {
                    MessageBox.Show(
                        $"Trùng lịch học!\n" +
                        $"Phòng: {a.MaPH}\n" +
                        $"Thứ: {a.Thu}\n" +
                        $"Tiết: [{a.TietBatDau} - {a.TietKetThuc}] trùng với [{b.TietBatDau} - {b.TietKetThuc}]",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return false;
                }
            }
        }

        return true;
    }


    bool ValidateLichTrungDb(PhongHocDto phong, List<LichSuDungDto> listOfNhp, int row)
    {
        _listLichSuDungForChecking = _lichSuDungController.GetDsLichSdByMaPh(phong.MaPH);

        foreach (var lichPhong in _listLichSuDungForChecking)
        {
            foreach (var lichNhp in listOfNhp)
            {
                // Kiểm tra trùng thời gian
                bool isOverlap =
                    lichPhong.ThoiGianBatDau < lichNhp.ThoiGianKetThuc &&
                    lichNhp.ThoiGianBatDau < lichPhong.ThoiGianKetThuc;

                if (isOverlap)
                {
                    // Log chi tiết để debug
                    Console.WriteLine(
                        $"TRÙNG LỊCH: " +
                        $"Phòng {phong.MaPH} | " +
                        $"{lichPhong.ThoiGianBatDau:dd/MM HH:mm}-{lichPhong.ThoiGianKetThuc:HH:mm} " +
                        $"VS Nhóm HP {lichNhp.ThoiGianBatDau:dd/MM HH:mm}-{lichNhp.ThoiGianKetThuc:HH:mm}"
                    );
                    
                    MessageBox.Show($"Dòng {row}: Lỗi phòng học bận!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;

                }
            }
        }

        return true; // không trùng
    }
    
    public List<LichSuDungDto> GetListLichSd(
        string thu,
        int tietBatDau,
        int tietKetThuc,
        DateTime ngayBatDau,
        DateTime ngayKetThuc,
        int maPhong)
    {
        List<LichSuDungDto> list = new List<LichSuDungDto>();

        DayOfWeek targetDay = ConvertThu.ConvertToDayOfWeek(thu);

        // Tìm ngày đầu tiên trùng với thứ
        DateTime current = ngayBatDau;
        while (current.DayOfWeek != targetDay)
            current = current.AddDays(1);

        // Mỗi tiết sẽ add riêng 1 lịch luôn
        while (current <= ngayKetThuc)
        {
            for (int tiet = tietBatDau; tiet <= tietKetThuc; tiet++)
            {
                var tg = ConvertTietToGio.GetThoiGianTiet(tiet, current);

                list.Add(new LichSuDungDto
                {
                    MaPH = maPhong,
                    ThoiGianBatDau = tg.Start,
                    ThoiGianKetThuc = tg.End,
                });
            }

            current = current.AddDays(7);
        }

        return list;
    }


    bool ValidateDataLichHoc(
        string tenPH,
        string thu,
        string tietBatDau,
        string tietKetThuc,
        string tuNgay,
        string ca,
        int row // để hiện lỗi dòng bao nhiêu
    )
    {
        // ============= TÊN PHÒNG =============
        if (string.IsNullOrWhiteSpace(tenPH))
        {
            MessageBox.Show($"Dòng {row}: Tên phòng học không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!_phongHocController.ExistByTen(tenPH))
        {
            MessageBox.Show($"Dòng {row}: Phòng học không tồn tại!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }


        // ============= THỨ =============
        if (string.IsNullOrWhiteSpace(thu))
        {
            MessageBox.Show($"Dòng {row}: Thứ không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(thu, out int thuVal) || thuVal < 2 || thuVal > 7)
        {
            MessageBox.Show($"Dòng {row}: Thứ phải nằm trong khoảng từ 2 đến 7!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }


        // ============= TIẾT BẮT ĐẦU =============
        if (string.IsNullOrWhiteSpace(tietBatDau))
        {
            MessageBox.Show($"Dòng {row}: Tiết bắt đầu không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(tietBatDau, out int tbd) || tbd < 1 || tbd > 10)
        {
            MessageBox.Show($"Dòng {row}: Tiết bắt đầu phải là số từ 1 đến 10!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }


        // ============= TIẾT KẾT THÚC =============
        if (string.IsNullOrWhiteSpace(tietKetThuc))
        {
            MessageBox.Show($"Dòng {row}: Tiết kết thúc không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(tietKetThuc, out int tkt) || tkt < 1 || tkt > 10)
        {
            MessageBox.Show($"Dòng {row}: Tiết kết thúc phải là số từ 1 đến 10!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }


        // ============= KIỂM TRA LIÊN QUAN TỚI TIẾT =============
        if (tbd > tkt)
        {
            MessageBox.Show($"Dòng {row}: Tiết bắt đầu không được lớn hơn tiết kết thúc!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // Buổi sáng: 1–5 ; Buổi chiều: 6–10
        bool isMorningTbd = tbd >= 1 && tbd <= 5;
        bool isMorningTkt = tkt >= 1 && tkt <= 5;

        bool isAfternoonTbd = tbd >= 6 && tbd <= 10;
        bool isAfternoonTkt = tkt >= 6 && tkt <= 10;

        if (!((isMorningTbd && isMorningTkt) || (isAfternoonTbd && isAfternoonTkt)))
        {
            MessageBox.Show($"Dòng {row}: Tiết bắt đầu và tiết kết thúc phải thuộc cùng 1 buổi (1–5 hoặc 6–10)!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }


        // ============= TỪ NGÀY =============
        if (string.IsNullOrWhiteSpace(tuNgay))
        {
            MessageBox.Show($"Dòng {row}: Từ ngày không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!DateTime.TryParseExact(tuNgay, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime _))
        {
            MessageBox.Show($"Dòng {row}: Từ ngày phải có định dạng dd/MM/yyyy!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        //Ca
        if (string.IsNullOrWhiteSpace(ca))
        {
            MessageBox.Show($"Dòng {row}: Ca không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        if (!ca.Equals("Lý thuyết") && !ca.Equals("Thực hành"))
        {
            MessageBox.Show($"Dòng {row}: Ca không hợp lệ!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        return true;
    }


    bool ValidateDataNhomHocPhan(string maHP, string maGV, string maLop, string siSoToiDa, int row)
    {
        // Mã học phần
        if (string.IsNullOrWhiteSpace(maHP))
        {
            MessageBox.Show($"Dòng {row}: Mã học phần không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!Validate.IsPositiveInt(maHP))
        {
            MessageBox.Show($"Dòng {row}: Mã học phần không hợp lệ!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!_hocPhanController.ExistById(int.Parse(maHP)))
        {
            MessageBox.Show($"Dòng {row}: Mã học phần không tồn tại trong hệ thống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // Mã giảng viên
        if (string.IsNullOrWhiteSpace(maGV))
        {
            MessageBox.Show($"Dòng {row}: Mã giảng viên không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!Validate.IsPositiveInt(maGV))
        {
            MessageBox.Show($"Dòng {row}: Mã giảng viên không hợp lệ!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!_giangVienController.ExistById(int.Parse(maGV)))
        {
            MessageBox.Show($"Dòng {row}: Giảng viên không tồn tại!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // Mã lớp
        if (string.IsNullOrWhiteSpace(maLop))
        {
            MessageBox.Show($"Dòng {row}: Mã lớp không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!Validate.IsPositiveInt(maLop))
        {
            MessageBox.Show($"Dòng {row}: Mã lớp không hợp lệ!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!_lopController.ExistById(int.Parse(maLop)))
        {
            MessageBox.Show($"Dòng {row}: Lớp không tồn tại!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // Sĩ số tối đa
        if (string.IsNullOrWhiteSpace(siSoToiDa))
        {
            MessageBox.Show($"Dòng {row}: Sĩ số tối đa không được để trống!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!Validate.IsPositiveInt(siSoToiDa))
        {
            MessageBox.Show($"Dòng {row}: Sĩ số tối đa không hợp lệ!",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    public List<LichHocDto> GetListLichHocByMaNhp(int maNhp)
    {
        List<LichHocDto> rs = new List<LichHocDto>();
        foreach (var item in listLichTam)
        {
            if (item.MaNHP == maNhp)
            {
                rs.Add(item);
            }
        }
        return rs;
    }
    
    public int GetLastRow(ExcelWorksheet ws)
    {
        int lastRow = 0;
        for (int row = 1; row <= ws.Dimension.End.Row; row++)
        {
            for (int col = 1; col <= ws.Dimension.End.Column; col++)
            {
                var val = ws.Cells[row, col].Value;
                if (val != null && !string.IsNullOrEmpty(val.ToString().Trim()))
                {
                    if (row > lastRow)
                        lastRow = row;
                }
            }
        }

        return lastRow;
    }

    List<LichSuDungDto> GetListLichSuDungByListLich(List<LichHocDto> input)
    {
        List<LichSuDungDto> rs = new List<LichSuDungDto>();
        foreach (var item in input)
        {
            rs.AddRange(GetListLichSd($"Thứ {item.Thu}", item.TietBatDau, item.TietKetThuc, item.TuNgay, item.DenNgay, item.MaPH));
        }

        return rs;
    }
}