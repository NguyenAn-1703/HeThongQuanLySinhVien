using QuanLySinhVien.Models;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Structs;

namespace QuanLySinhVien.Controller.Controllers;

public class GiangVienController
{
    private static GiangVienController _instance;
    private static List<GiangVienDto> giangVien;
    private TaiKhoanController _taiKhoanController;

    private GiangVienController()
    {
    }

    public static GiangVienController GetInstance()
    {
        if (_instance == null) _instance = new GiangVienController();
        return _instance;
    }

    public List<GiangVienDto> GetAll()
    {
        try
        {
            giangVien = GiangVienDao.GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return giangVien;
    }

    public void HardDeleteById(int id)
    {
        try
        {
            GiangVienDao.HardDeleteById(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void SoftDeleteById(int id)
    {
        try
        {
            GiangVienDao.SoftDeleteById(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public GiangVienDto GetById(int id)
    {
        var giangVien = new GiangVienDto();
        try
        {
            giangVien = GiangVienDao.GetGVById(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return giangVien;
    }

    public void Update(GiangVienDto giangVien)
    {
        try
        {
            if (giangVien.TenGV == "") throw new Exception("Tên giảng viên không được để trống");
            GiangVienDao.UpdateGV(giangVien);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void Insert(GiangVienDto giangVien)
    {
        try
        {
            if (giangVien.TenGV == "") throw new Exception("Tên giảng viên không được để trống");
            GiangVienDao.InsertGV(giangVien);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public GiangVienDto GetByTen(string ten)
    {
        return GiangVienDao.GetGVByTen(ten);
    }

    public bool ExistByTen(string ten)
    {
        var listPh = GetAll();
        foreach (var item in listPh)
            if (item.TenGV.Equals(ten))
                return true;

        return false;
    }

    public GiangVienDto GetByMaTK(int maTk)
    {
        var gv = new GiangVienDto();
        var listGV = GiangVienDao.GetAll();

        gv = listGV.First(x => x.MaTK == maTk);
        return gv;
    }

    public bool ExistByMaTk(int maTk)
    {
        var listGV = GiangVienDao.GetAll();
        foreach (var item in listGV)
        {
            if (item.MaTK == maTk)
            {
                return true;
            }
        }

        return false;
    }

    public ValidateResult Validate(
        string imgPath,
        string tenGV, string ngaySinh, string soDienThoai, string email,
        string taiKhoan)
    {
        ValidateResult rs = new ValidateResult
        {
            index = -1,
            message = "",
        };

        // 1. Kiểm tra ảnh
        if (Shared.Validate.IsEmpty(imgPath))
        {
            rs.index = 0;
            rs.message = "Vui lòng thêm ảnh giảng viên!";
            return rs;
        }

// 2. Kiểm tra tên giảng viên
        if (Shared.Validate.IsEmpty(tenGV))
        {
            rs.index = 0;
            rs.message = "Tên giảng viên không được để trống!";
            return rs;
        }

// 3. Kiểm tra ngày sinh
        if (Shared.Validate.IsEmpty(ngaySinh))
        {
            rs.index = 1;
            rs.message = "Ngày sinh không được để trống!";
            return rs;
        }

        DateTime ngaySinhDate = ConvertDate.ConvertStringToDate(ngaySinh);

// 4. Kiểm tra tuổi giảng viên (tối thiểu 22 tuổi)
        var homNay = DateTime.Now;
        var tuoi = homNay.Year - ngaySinhDate.Year;
        if (homNay.Month < ngaySinhDate.Month ||
            (homNay.Month == ngaySinhDate.Month && homNay.Day < ngaySinhDate.Day))
            tuoi--;

        if (tuoi < 22)
        {
            rs.index = 1;
            rs.message = "Giảng viên phải từ 22 tuổi trở lên!";
            return rs;
        }

// 5. Kiểm tra số điện thoại
        if (Shared.Validate.IsEmpty(soDienThoai))
        {
            rs.index = 2;
            rs.message = "Số điện thoại không được để trống!";
            return rs;
        }

        if (!Shared.Validate.IsValidPhoneNumber(soDienThoai))
        {
            rs.index = 2;
            rs.message = "Số điện thoại không hợp lệ!";
            return rs;
        }

// 6. Kiểm tra email
        if (Shared.Validate.IsEmpty(email))
        {
            rs.index = 3;
            rs.message = "Email không được để trống!";
            return rs;
        }

        if (!Shared.Validate.IsValidEmail(email))
        {
            rs.index = 3;
            rs.message = "Email không hợp lệ!";
            return rs;
        }

// 7. Kiểm tra tài khoản
        if (Shared.Validate.IsEmpty(taiKhoan))
        {
            rs.index = 4;
            rs.message = "Tên tài khoản không được để trống!";
            return rs;
        }

        _taiKhoanController = TaiKhoanController.getInstance();

        if (!_taiKhoanController.ExistByTen(taiKhoan))
        {
            rs.index = 4;
            rs.message = "Tài khoản không tồn tại!";
            return rs;
        }

        return rs;
    }
}