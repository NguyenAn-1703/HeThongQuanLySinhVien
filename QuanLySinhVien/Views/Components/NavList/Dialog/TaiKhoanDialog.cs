using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class TaiKhoanDialog : CustomDialog
{

    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private TaiKhoanController _taiKhoanController;
    private NhomQuyenController _nhomQuyenController;
    private int _idTaiKhoan;
    public event Action Finish;

    public TaiKhoanDialog(string title, DialogType dialogType, List<InputFormItem> listIFI, TaiKhoanController taiKhoanController, NhomQuyenController nhomQuyenController, int idTaiKhoan = -1) : base(title, dialogType)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _taiKhoanController = taiKhoanController;
        _nhomQuyenController = nhomQuyenController;
        _idTaiKhoan = idTaiKhoan;
        Init();
    }

    void Init()
    {
        for (int i = 0; i < _listIFI.Count; i++)
        {
            _listTextBox.Add(new LabelTextField(_listIFI[i].content, _listIFI[i].type));
            _textBoxsContainer.Controls.Add(_listTextBox[i]);
        }
        
        if (_dialogType == DialogType.Them)
        {
            SetupInsert();
        }
        else if (_dialogType == DialogType.Sua)
        {
            SetupUpdate();
        }
        else
        {
            SetupDetail();
        }
        
    }

    //Set dữ liệu có sẵn hoặc những dòng không được chọn
    void SetupInsert()
    {
        _listTextBox[2]._combobox.UpdateSelection(_nhomQuyenController.GetAllTenNhomQuyen().ToArray());

        _btnLuu._mouseDown += () => { Insert(); };
    }

    void SetupUpdate()
    {
        if (_idTaiKhoan == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        TaiKhoanDto taiKhoan = _taiKhoanController.GetTaiKhoanById(_idTaiKhoan);
        
        _listTextBox[0].GetTextField().Text = taiKhoan.TenDangNhap + "";
        _listTextBox[1]._combobox.UpdateSelection(_nhomQuyenController.GetAllTenNhomQuyen().ToArray());
        _listTextBox[1]._combobox.SetSelectionCombobox(_nhomQuyenController.GetTenQuyenByID(taiKhoan.MaNQ));
        
        _btnLuu._mouseDown += () => { UpdateTK(taiKhoan); };
    }

    void SetupDetail()
    {
        if (_idTaiKhoan == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        TaiKhoanDto taiKhoan = _taiKhoanController.GetTaiKhoanById(_idTaiKhoan);

        _listTextBox[1]._combobox.UpdateSelection(_nhomQuyenController.GetAllTenNhomQuyen().ToArray());
        
        _listTextBox[0].GetTextField().Text = taiKhoan.TenDangNhap + "";
        _listTextBox[1]._combobox.SetSelectionCombobox(taiKhoan.MaNQ + "");

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._combobox.Enable = false;
        
        Console.WriteLine("Chi tiết" + taiKhoan.MaTK + " " + taiKhoan.MaNQ + " " + taiKhoan.TenDangNhap + " " + taiKhoan.MatKhau);
    }

    void Insert()
    {
        TextBox TxtTenDangNhap = _listTextBox[0].GetTextField();
        TextBox TxtMatKhau = _listTextBox[1].GetTextField();

        string tenDangNhap = _listTextBox[0].GetTextTextField();
        string matKhau = _listTextBox[1].GetTextTextField();
        string tenNhomQuyen = _listTextBox[2].GetSelectionCombobox();

        int maNQ = _nhomQuyenController.GetMaNhomQuyenByTen(tenNhomQuyen);

        if (ValidateInsert(TxtTenDangNhap, TxtMatKhau, tenDangNhap, matKhau))
        {
            TaiKhoanDto taiKhoan = new TaiKhoanDto
            {
                TenDangNhap = tenDangNhap,
                MatKhau = matKhau,
                MaNQ = maNQ,
            };
            
            if (_taiKhoanController.Insert(taiKhoan))
            {
                MessageBox.Show("Thêm tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            
        }

    }

    void UpdateTK(TaiKhoanDto taiKhoan)
    {
        TextBox TxtTenDangNhap = _listTextBox[0].GetTextField();

        string tenDangNhap = _listTextBox[0].GetTextTextField();
        string tenNhomQuyen = _listTextBox[1].GetSelectionCombobox();

        int maNQ = _nhomQuyenController.GetMaNhomQuyenByTen(tenNhomQuyen);

        if (ValidateUpdate(TxtTenDangNhap,tenDangNhap))
        {
            TaiKhoanDto taiKhoanNew = new TaiKhoanDto
            {
                MaTK = taiKhoan.MaTK,
                TenDangNhap = tenDangNhap,
                MatKhau = taiKhoan.MatKhau,
                MaNQ = maNQ,
            };
            
            if (_taiKhoanController.Update(taiKhoanNew))
            {
                MessageBox.Show("Sửa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sửa tài khoản thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
    }

    bool ValidateInsert(TextBox TxtTenDangNhap, TextBox TxtMatKhau, string tenDangNhap, string matKhau)
    {
        if (CommonUse.Validate.IsEmpty(tenDangNhap))
        {
            MessageBox.Show("Tên đăng nhập không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtTenDangNhap.Focus();
            return false;
        }
        if (CommonUse.Validate.IsEmpty(matKhau))
        {
            MessageBox.Show("Mật khẩu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtMatKhau.Focus();
            return false;
        }
        if (!CommonUse.Validate.HasMinLength(matKhau, 6))
        {
            MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtMatKhau.Focus();
            TxtMatKhau.SelectAll();
            return false;
        }
        if (!CommonUse.Validate.HasMaxLength(matKhau, 20))
        {
            MessageBox.Show("Mật khẩu không quá 20 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtMatKhau.Focus();
            TxtMatKhau.SelectAll();
            return false;
        }
        return true;
    }
    
    bool ValidateUpdate(TextBox TxtTenDangNhap, string tenDangNhap)
    {
        if (CommonUse.Validate.IsEmpty(tenDangNhap))
        {
            MessageBox.Show("Tên đăng nhập không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtTenDangNhap.Focus();
            return false;
        }
        return true;
    }
    
    
}