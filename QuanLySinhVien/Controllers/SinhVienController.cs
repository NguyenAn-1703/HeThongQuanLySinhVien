using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers;

public class SinhVienController
{
    public SinhVienDAO SinhVienDao;
    public NganhDao NganhDao;
    public LopDAO LopDao;

    private static SinhVienController _instance;

    public static SinhVienController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SinhVienController();
        }
        return _instance;
    }
    private SinhVienController()
    {   
        SinhVienDao = new SinhVienDAO();
        NganhDao = NganhDao.GetInstance();
        LopDao = LopDAO.GetInstance();
    }

    public List<SinhVienDTO> GetAll()
    {
        return SinhVienDao.GetAll();
    }
    
    public List<SinhVienDTO> LayDanhSachSinhVienTable()
    {
        return SinhVienDao.getTableSinhVien();
    }

    public bool EditSinhVien(SinhVienDTO sinhVien)
    {
        return SinhVienDao.Update(sinhVien);
    }

    public bool DeleteSinhVien(int id)
    {
        return SinhVienDao.Delete(id);
    }

    public SinhVienDTO GetById(int id)
    {
        return SinhVienDao.GetById(id);
    }

    public bool AddSinhVien(SinhVienDTO sinhVien)
    {
        return SinhVienDao.Add(sinhVien);
    }
    
    public List<NganhDto> GetAllNganh()
    {
        return NganhDao.GetAll();
    }

    public List<LopDto> GetLopByNganh(int maNganh)
    {
        return LopDao.GetByNganh(maNganh);
    }


    public NganhDto GetNganhById(int maNganh)
    {
        return NganhDao.GetNganhById(maNganh);
    }

    public SinhVienDTO GetByMaTK(int maTK)
    {
        SinhVienDTO sv = new  SinhVienDTO();
        List<SinhVienDTO> listSv = SinhVienDao.GetAll();
        foreach (SinhVienDTO item in listSv)
        {
            if (item.MaTk == maTK)
            {
                sv =  item;
            }
        }
        return sv;
    }
}