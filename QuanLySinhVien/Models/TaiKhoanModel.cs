namespace QuanLySinhVien.Models;

//tạm
public class TaiKhoanModel
{
    private string _maTk;
    private string _tenTK;
    private string _nguoiDung;

    public TaiKhoanModel(string maTk, string tenTk, string nguoiDung)
    {
        _maTk = maTk;
        _tenTK = tenTk;
        _nguoiDung = nguoiDung;
    }
}