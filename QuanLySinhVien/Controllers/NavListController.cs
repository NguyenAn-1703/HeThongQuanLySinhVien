using QuanLySinhVien.Views.Components;

namespace QuanLySinhVien.Models;
using System.Collections.Generic;
public class NavListController
{
    public NavListController()
    {
        setDataButton();
    }
    public Dictionary<string , string> buttonClickTrans = new Dictionary<string, string>();

    public void setDataButton()
    {
        buttonClickTrans["Trang chủ"] = "TrangChu";
        buttonClickTrans["Sinh viên"] = "SinhVien";
        buttonClickTrans["Giảng viên"] = "GiangVien";
        buttonClickTrans["Khoa"] = "Khoa";
        buttonClickTrans["Học phí"] = "HocPhi";
        buttonClickTrans["Học phần"] = "HocPhan";
        buttonClickTrans["Ngành"] = "Nganh";
        buttonClickTrans["Phòng học"] = "PhongHoc";
        buttonClickTrans["Mở đăng ký học phần"] = "MoDangKyHocPhan";
        buttonClickTrans["Tổ chức thi"] = "ToChucThi";
        buttonClickTrans["Nhập điểm"] = "NhapDiem";
        buttonClickTrans["Quản lí tài khoản"] = "QuanLiTaiKhoan";
        buttonClickTrans["Phân quyền"] = "PhanQuyen";
        buttonClickTrans["Thống kê"] = "ThongKe";
        buttonClickTrans["Chương trình đào tạo"] = "ChuongTrinhDaoTao";
    }
    public String getDataButton(string key)
    {
        if (!buttonClickTrans.ContainsKey(key))
        {
            return "";
        }
        return buttonClickTrans[key];
    }

    public Panel update(string s)
    {
        Panel ans;
        if (s.Equals("TrangChu"))
        {
            ans = new TrangChu();
        }
        else if (s.Equals("SinhVien"))
        {
            ans = new SinhVien();
        }
        else if (s.Equals("Giangvien"))
        {
            ans = new GiangVien();
        }
        else if (s.Equals("HocPhan"))
        {
            ans = new HocPhan();
        }
        else if (s.Equals("MoDangKyHocPhan"))
        {
            ans = new MoDangKyHocPhan();
        }
        else if (s.Equals("ToChucThi"))
        {
            ans = new ToChucThi();
        }
        else if (s.Equals("Nganh"))
        {
            ans = new Nganh();
        }
        else if (s.Equals("PhongHoc"))
        {
            ans = new PhongHoc();
        }
        else if (s.Equals("PhanQuyen"))
        {
            ans = new PhanQuyen();
        }
        else if (s.Equals("ThongKe"))
        {
            ans = new ThongKe();
        }
        else if (s.Equals("ChuongTrinhDaoTao"))
        {
            ans = new ChuongTrinhDaoTao();
        }
        else if (s.Equals("NhapDiem"))
        {
            ans = new NhapDiem();
        }
        else if (s.Equals("HocPhi"))
        {
            ans = new HocPhi();
        }else if (s.Equals("QuanLiTaiKhoan"))
        {
            ans = new QuanLiTaiKhoan();
        }
        else
        {
            ans =  new Khoa();
        }
        return ans;
    }
}