using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using System.Collections.Generic;

namespace QuanLySinhVien.Controllers
{
    public class PhongHocController
    {
        private readonly PhongHocDao _dao;

        public PhongHocController()
        {
            _dao = new PhongHocDao();
        }
        
        public List<PhongHocDto> GetDanhSachPhongHoc()
        {
            return _dao.GetAll();
        }
        
        public PhongHocDto GetPhongHocById(int maPH)
        {
            return _dao.GetById(maPH);
        }
        
        public bool ThemPhongHoc(PhongHocDto phongHoc)
        {
            return _dao.Insert(phongHoc);
        }
        
        public bool SuaPhongHoc(PhongHocDto phongHoc)
        {
            return _dao.Update(phongHoc);
        }
        
        public bool XoaPhongHoc(int maPH)
        {
            return _dao.Delete(maPH);
        }
    }
}