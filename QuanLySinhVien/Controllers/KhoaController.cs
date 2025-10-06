using System.Data;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers
{
    public class KhoaController
    {
        private readonly KhoaDao _khoaDao;

        public KhoaController()
        {
            _khoaDao = new KhoaDao();
        }

        // get databse (all)
        public DataTable GetDanhSachKhoa()
        {
            return _khoaDao.GetAllKhoa();
        }

        // pop up dialog
        public void ThemKhoa(string tenKhoa, string email, string diaChi)
        {
            // check data
            if (string.IsNullOrWhiteSpace(tenKhoa))
                throw new ArgumentException("Tên khoa không được để trống!");

            _khoaDao.InsertKhoa(tenKhoa, email, diaChi);
        }

        // edit khoa
        public void SuaKhoa(int idKhoa, string txtTen, string txtEmail, string txtDiaChi)
        {
            // call DAO function
            _khoaDao.UpdateKhoa(idKhoa, txtTen, txtEmail, txtDiaChi);
        }


        // delete khoa
        public void XoaKhoa(int idKhoa)
        {
            // call DAO function
            _khoaDao.DeleteKhoa(idKhoa);
        }

        // get name by ID ( khoa.cs <-> dao )
        public DataRow GetKhoaById(int id)
        {
            DataRow tmp = _khoaDao.GetKhoaById(id);
            return tmp;
        }
    }
}