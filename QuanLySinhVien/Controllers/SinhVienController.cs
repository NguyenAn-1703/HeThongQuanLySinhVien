namespace QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
public class SinhVienController
{
    public SinhVienDAO sinhVienDAO;
    public NganhDao nganhDao;
    public KhoaHocDAO khoaHocDao;
    public LopDAO lopDao;
    
    public SinhVienController()
    {   
        sinhVienDAO = new SinhVienDAO();
        nganhDao = new NganhDao();
        khoaHocDao = new KhoaHocDAO();
        lopDao = new LopDAO();
    }

    public List<SinhVienDTO> LayDanhSachSinhVienTable()
    {
        return sinhVienDAO.getTableSinhVien();
    }

    public void EditSinhVien(SinhVienDTO sinhVien)
    {
        
        sinhVienDAO.Update(sinhVien);
    }

    public void DeleteSinhVien(int id)
    {
        sinhVienDAO.Delete(id);
    }

    public SinhVienDTO GetSinhVienById(int id)
    {
        return sinhVienDAO.GetSinhVienById(id);
    }

    public void AddSinhVien(SinhVienDTO sinhVien)
    {
        sinhVienDAO.Add(sinhVien);
    }

    public List<SinhVienDTO> Search(string text, string filter)
    {
        return sinhVienDAO.Search(text, filter);
    }
    
    public List<NganhDto> GetAllNganh()
    {
        return nganhDao.GetAll();
    }

    public List<dynamic> GetAllKhoaHoc()
    {
        return khoaHocDao.GetAllWithDisplayText();
    }

    public List<LopDto> GetLopByNganh(int maNganh)
    {
        return lopDao.GetByNganh(maNganh);
    }

    public List<LopDto> GetLopByNganhAndKhoaHoc(int maNganh, string tenKhoaHoc)
    {
        return lopDao.GetByNganhAndKhoaHoc(maNganh, tenKhoaHoc);
    }

    public NganhDto GetNganhById(int maNganh)
    {
        return nganhDao.GetNganhById(maNganh);
    }

    public KhoaHocDto GetKhoaHocById(int maKhoaHoc)
    {
        return khoaHocDao.GetById(maKhoaHoc);
    }

    public LopDto GetLopById(int maLop)
    {
        return lopDao.GetById(maLop);
    }
}