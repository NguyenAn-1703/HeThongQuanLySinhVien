using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class KhoaHocDialog : CustomDialog
{

    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private KhoaHocController _KhoaHocController;
    private ChuKyDaoTaoController _chuKyDaoTaoController;
    private int _idKhoaHoc;
    public event Action Finish;

    public KhoaHocDialog(string title, DialogType dialogType, List<InputFormItem> listIFI, KhoaHocController KhoaHocController, ChuKyDaoTaoController chuKyDaoTaoController, int idKhoaHoc = -1) : base(title, dialogType)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _KhoaHocController = KhoaHocController;
        _chuKyDaoTaoController = chuKyDaoTaoController;
        _idKhoaHoc = idKhoaHoc;
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
        _btnLuu._mouseDown += () => { Insert(); };
    }

    void SetupUpdate()
    {
        if (_idKhoaHoc == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        KhoaHocDto KhoaHoc = _KhoaHocController.GetKhoaHocById(_idKhoaHoc);
        
        _listTextBox[0].GetTextField().Text = KhoaHoc.TenKhoaHoc + "";
        _listTextBox[1].GetTextField().Text = KhoaHoc.NienKhoaHoc + "";
        
        _btnLuu._mouseDown += () => { UpdateTK(KhoaHoc); };
    }

    void SetupDetail()
    {
        if (_idKhoaHoc == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        KhoaHocDto KhoaHoc = _KhoaHocController.GetKhoaHocById(_idKhoaHoc);
        
        _listTextBox[0].GetTextField().Text = KhoaHoc.TenKhoaHoc + "";
        _listTextBox[1].GetTextField().Text = KhoaHoc.NienKhoaHoc + "";

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._field.Enable = false;
        
    }

    void Insert()
    {
        TextBox TxtTenKhoaHoc = _listTextBox[0].GetTextField();
        TextBox TxtNienKhoaHoc = _listTextBox[1].GetTextField();

        string tenKhoaHoc = _listTextBox[0].GetTextTextField();
        string nienKhoaHoc = _listTextBox[1].GetTextTextField();

        int maCKDT;

        if (Validate(TxtTenKhoaHoc, TxtNienKhoaHoc, tenKhoaHoc, nienKhoaHoc))
        {
            maCKDT = _chuKyDaoTaoController.GetByNienKhoa(nienKhoaHoc).MaCKDT;
            KhoaHocDto KhoaHoc = new KhoaHocDto
            {
                MaCKDT = maCKDT,
                TenKhoaHoc = tenKhoaHoc,
                NienKhoaHoc = nienKhoaHoc,
            };
            
            if (_KhoaHocController.Insert(KhoaHoc))
            {
                MessageBox.Show("Thêm khóa học thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm khóa học thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            
        }

    }

    void UpdateTK(KhoaHocDto KhoaHoc)
    {
        TextBox TxtTenKhoaHoc = _listTextBox[0].GetTextField();
        TextBox TxtNienKhoaHoc = _listTextBox[1].GetTextField();

        string tenKhoaHoc = _listTextBox[0].GetTextTextField();
        string nienKhoaHoc = _listTextBox[1].GetTextTextField();

        
        int maCKDT;
        if (Validate(TxtTenKhoaHoc,TxtNienKhoaHoc,tenKhoaHoc,nienKhoaHoc))
        {
            maCKDT = _chuKyDaoTaoController.GetByNienKhoa(nienKhoaHoc).MaCKDT;
            KhoaHocDto KhoaHocNew = new KhoaHocDto
            {
                MaKhoaHoc = KhoaHoc.MaKhoaHoc,
                MaCKDT = maCKDT,
                TenKhoaHoc = tenKhoaHoc,
                NienKhoaHoc = nienKhoaHoc,
            };
            
            if (_KhoaHocController.Update(KhoaHocNew))
            {
                MessageBox.Show("Sửa khóa học thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sửa khóa học thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
    }

    bool Validate(TextBox TxtTenKhoaHoc, TextBox NienKhoaHoc, string tenKhoaHoc, string nienKhoaHoc)
    {
        if (CommonUse.Validate.IsEmpty(tenKhoaHoc))
        {
            MessageBox.Show("Tên khóa học không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtTenKhoaHoc.Focus();
            return false;
        }
        if (CommonUse.Validate.IsEmpty(nienKhoaHoc))
        {
            MessageBox.Show("Niên khóa học không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            NienKhoaHoc.Focus();
            return false;
        }
        if (!CommonUse.Validate.IsAcademicYear(nienKhoaHoc))
        {
            MessageBox.Show("Niên khóa học không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            NienKhoaHoc.Focus();
            return false;
        }
        //set mã chu kỳ tự động
        if (!_chuKyDaoTaoController.Validate(nienKhoaHoc))
        {
            MessageBox.Show("Niên khóa học không hợp lệ!, không nằm trong một chu kỳ nào!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            NienKhoaHoc.Focus();
            return false;
        }
        return true;
    }
    
}