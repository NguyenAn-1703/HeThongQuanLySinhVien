using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Controllers;

using System.Collections.Generic;
public class NavListController
{
    private NhomQuyenDto _quyen;
    public NavListController(NhomQuyenDto quyen)
    {
        _quyen = quyen;
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
        buttonClickTrans["Lớp"] = "Lop";
        buttonClickTrans["Phòng học"] = "PhongHoc";
        buttonClickTrans["Chu kỳ đào tạo"] = "ChuKyDaoTao";
        buttonClickTrans["Khóa học"] = "KhoaHoc";
        buttonClickTrans["Mở đăng ký học phần"] = "MoDangKyHocPhan";
        buttonClickTrans["Tổ chức thi"] = "ToChucThi";
        buttonClickTrans["Nhập điểm"] = "NhapDiem";
        buttonClickTrans["Quản lí tài khoản"] = "QuanLiTaiKhoan";
        buttonClickTrans["Phân quyền"] = "PhanQuyen";
        buttonClickTrans["Thống kê"] = "ThongKe";
        buttonClickTrans["Chương trình đào tạo"] = "ChuongTrinhDaoTao";
        //SV
        buttonClickTrans["Thông tin cá nhân"] = "ThongTinSinhVien";
        buttonClickTrans["Đăng ký học phần"] = "DangKyHocPhan";
    }
    
    public String getDataButton(string key)
    {
        if (!buttonClickTrans.ContainsKey(key))
        {
            return "";
        }
        return buttonClickTrans[key];
    }

    public NavBase update(string s)
    {
        NavBase ans;
        if (s.Equals("TrangChu"))
        {
            ans = new TrangChu(_quyen);
        }
        else if (s.Equals("SinhVien"))
        {
            ans = new SinhVien(_quyen);
            // ans = new ThongTinSinhVien(_quyen);
        }
        else if (s.Equals("GiangVien"))
        {
            ans = new GiangVien(_quyen);
        }
        else if (s.Equals("HocPhan"))
        {
            ans = new HocPhan(_quyen);
        }
        else if (s.Equals("MoDangKyHocPhan"))
        {
            ans = new MoDangKyHocPhan(_quyen);
        }
        else if (s.Equals("ToChucThi"))
        {
            ans = new ToChucThi(_quyen);
        }
        else if (s.Equals("Nganh"))
        {
            ans = new NganhPanel(_quyen);
        }
        else if (s.Equals("Lop"))
        {
            ans = new QuanLiLop(_quyen);
        }
        else if (s.Equals("PhongHoc"))
        {
            ans = new PhongHoc(_quyen);
        }
        else if (s.Equals("ChuKyDaoTao"))
        {
            ans = new ChuKyDaoTao(_quyen);
        }
        else if (s.Equals("KhoaHoc"))
        {
            ans = new KhoaHoc(_quyen);
        }
        else if (s.Equals("PhanQuyen"))
        {
            ans = new PhanQuyen(_quyen);
        }
        else if (s.Equals("ThongKe"))
        {
            ans = new ThongKe(_quyen);
        }
        else if (s.Equals("ChuongTrinhDaoTao"))
        {
            ans = new ChuongTrinhDaoTao(_quyen);
        }
        else if (s.Equals("NhapDiem"))
        {
            ans = new NhapDiem(_quyen);
        }
        else if (s.Equals("HocPhi"))
        {
            ans = new HocPhi(_quyen);
        }else if (s.Equals("QuanLiTaiKhoan"))
        {
            ans = new QuanLiTaiKhoan(_quyen);
        }
        //SV
        else if(s.Equals("ThongTinSinhVien"))
        {
            ans =  new ThongTinSinhVien(_quyen);
        }
        else if (s.Equals("DangKyHocPhan"))
        {
            ans = new DangKyHocPhan(_quyen);
        }
        
        else
        {
            ans =  new Khoa(_quyen);
        }
        return ans;
    }
}