using System.Data;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers
{
    public class KhoaController
    {
        private static KhoaController _instance;
        private readonly KhoaDao _khoaDao;

        public KhoaController()
        {
            _khoaDao = new KhoaDao();
        }
        
        // Lấy instance duy nhất
        public static KhoaController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new KhoaController();
            }

            return _instance;
        }

        // get databse (all)
        public List<KhoaDto> GetDanhSachKhoa()
        {
            return _khoaDao.GetAll();
        }

        // pop up dialog
        public void ThemKhoa(string tenKhoa, string email, string diaChi)
        {
            // check data
            if (string.IsNullOrWhiteSpace(tenKhoa))
                throw new ArgumentException("Tên khoa không được để trống!");

            _khoaDao.Insert(new  KhoaDto{TenKhoa = tenKhoa, Email = email, DiaChi = diaChi});
        }

        // edit khoa
        public void SuaKhoa(int idKhoa, string txtTen, string txtEmail, string txtDiaChi)
        {
            // call DAO function
            _khoaDao.Update(new  KhoaDto{MaKhoa = idKhoa,TenKhoa = txtTen, Email = txtEmail, DiaChi = txtDiaChi});
        }


        // delete khoa
        public void XoaKhoa(int idKhoa)
        {
            // call DAO function
            _khoaDao.Delete(idKhoa);
        }

        // get name by ID ( khoa.cs <-> dao )
        public KhoaDto GetKhoaById(int id)
        {
            KhoaDto tmp = _khoaDao.GetKhoaById(id);
            return tmp;
        }
    }
}