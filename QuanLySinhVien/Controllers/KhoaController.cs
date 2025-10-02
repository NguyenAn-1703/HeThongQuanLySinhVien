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

        public DataTable GetDanhSachKhoa()
        {
            
            return _khoaDao.GetAllKhoa();
        }
    }
}