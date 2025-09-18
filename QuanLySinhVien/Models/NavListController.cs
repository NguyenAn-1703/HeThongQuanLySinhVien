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
        buttonClickTrans["Học phí"] = "HocPhi";
        buttonClickTrans["Học phần"] = "HocPhan";
        buttonClickTrans["Ngành"] = "Nganh";
        buttonClickTrans["Phòng học"] = "PhongHoc";
        buttonClickTrans["Mở đăng kí học phần"] = "MoDangKyHocPhan";
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
}