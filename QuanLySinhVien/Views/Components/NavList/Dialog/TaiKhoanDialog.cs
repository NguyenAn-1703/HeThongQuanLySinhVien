using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.utils;
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
    private string[] arrType = new[] { "Quản trị viên", "Sinh viên" };
    public event Action Finish;

    public TaiKhoanDialog(string title, DialogType dialogType, List<InputFormItem> listIFI,
        TaiKhoanController taiKhoanController, NhomQuyenController nhomQuyenController,
        int idTaiKhoan = -1) : base(title, dialogType)
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
        _listTextBox[2]._combobox.UpdateSelection(arrType);
        _listTextBox[3]._combobox.UpdateSelection(_nhomQuyenController.GetAllTenNhomQuyen().ToArray());

        UpdateCBNQ(_listTextBox[2].GetSelectionCombobox());

        _btnLuu._mouseDown += () => { Insert(); };

        _listTextBox[2]._combobox.combobox.SelectedIndexChanged +=
            (sender, args) => OnChangeCBType(_listTextBox[2].GetSelectionCombobox());
    }

    void SetupUpdate()
    {
        if (_idTaiKhoan == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }

        TaiKhoanDto taiKhoan = _taiKhoanController.GetTaiKhoanById(_idTaiKhoan);

        _listTextBox[0].GetTextField().Text = taiKhoan.TenDangNhap + "";
        _listTextBox[1]._combobox.UpdateSelection(arrType);
        _listTextBox[1]._combobox.SetSelectionCombobox(taiKhoan.Type);

        _listTextBox[2]._combobox.UpdateSelection(_nhomQuyenController.GetAllTenNhomQuyen().ToArray());
        UpdateCBNQ(_listTextBox[1].GetSelectionCombobox());
        _listTextBox[2]._combobox.SetSelectionCombobox(_nhomQuyenController.GetTenQuyenByID(taiKhoan.MaNQ));

        
        
        _listTextBox[1]._combobox.combobox.SelectedIndexChanged +=
            (sender, args) => OnChangeCBType(_listTextBox[1].GetSelectionCombobox());
        
        _btnLuu._mouseDown += () => { UpdateTK(taiKhoan); };
    }

    void SetupDetail()
    {
        if (_idTaiKhoan == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }

        TaiKhoanDto taiKhoan = _taiKhoanController.GetTaiKhoanById(_idTaiKhoan);


        _listTextBox[0].GetTextField().Text = taiKhoan.TenDangNhap + "";
        _listTextBox[1]._combobox.UpdateSelection(arrType);
        _listTextBox[1]._combobox.SetSelectionCombobox(taiKhoan.Type);
        _listTextBox[2]._combobox.UpdateSelection(_nhomQuyenController.GetAllTenNhomQuyen().ToArray());
        _listTextBox[2]._combobox.SetSelectionCombobox(_nhomQuyenController.GetTenQuyenByID(taiKhoan.MaNQ));

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._combobox.Enable = false;
        _listTextBox[2]._combobox.Enable = false;
    }

    void OnChangeCBType(string selection)
    {
        UpdateCBNQ(selection);
    }

    // nếu type = quản trị viên -> hiển thị ds quyền tương ứng
    // nếu type = sinh viên -> hiển thị quyền sinh viên
    void UpdateCBNQ(string selection)
    {
        int indexCbNQ = _dialogType == DialogType.Them ? 3 : 2;
        List<string> listQuyen = _nhomQuyenController.GetAllTenNhomQuyen();
        Console.WriteLine("'" + selection + "'");
        if (selection.Equals("Quản trị viên"))
        {
            
            _listTextBox[indexCbNQ].Enabled = true;

            listQuyen.RemoveAll(x => x.Equals("SinhVien"));
            _listTextBox[indexCbNQ]._combobox.UpdateSelection(listQuyen.ToArray());
        }
        else
        {
            _listTextBox[indexCbNQ]._combobox.UpdateSelection(listQuyen.ToArray());
            _listTextBox[indexCbNQ].SetComboboxSelection("Sinh viên");
            _listTextBox[indexCbNQ].Enabled = false;
        }
    }


    void Insert()
    {
        TextBox TxtTenDangNhap = _listTextBox[0].GetTextField();
        TextBox TxtMatKhau = _listTextBox[1].GetTextField();

        string tenDangNhap = _listTextBox[0].GetTextTextField();
        string matKhau = _listTextBox[1].GetTextTextField();
        string type = _listTextBox[2].GetSelectionCombobox();
        string tenNhomQuyen = _listTextBox[3].GetSelectionCombobox();

        int maNQ = _nhomQuyenController.GetMaNhomQuyenByTen(tenNhomQuyen);

        string hashPassword = PasswordHasher.HashPassword(matKhau);

        if (ValidateInsert(TxtTenDangNhap, TxtMatKhau, tenDangNhap, matKhau))
        {
            TaiKhoanDto taiKhoan = new TaiKhoanDto
            {
                TenDangNhap = tenDangNhap,
                MatKhau = hashPassword,
                MaNQ = maNQ,
                Type = type,
            };

            if (_taiKhoanController.Insert(taiKhoan))
            {
                MessageBox.Show("Thêm tài khoản thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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
        string type  = _listTextBox[1].GetSelectionCombobox();
        string tenNhomQuyen = _listTextBox[2].GetSelectionCombobox();

        int maNQ = _nhomQuyenController.GetMaNhomQuyenByTen(tenNhomQuyen);

        if (ValidateUpdate(TxtTenDangNhap, tenDangNhap))
        {
            TaiKhoanDto taiKhoanNew = new TaiKhoanDto
            {
                MaTK = taiKhoan.MaTK,
                TenDangNhap = tenDangNhap,
                MatKhau = taiKhoan.MatKhau,
                MaNQ = maNQ,
                Type = type,
            };

            if (_taiKhoanController.Update(taiKhoanNew))
            {
                MessageBox.Show("Sửa tài khoản thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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