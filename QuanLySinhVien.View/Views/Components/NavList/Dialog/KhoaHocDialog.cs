using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class KhoaHocDialog : CustomDialog
{
    private readonly int _idKhoaHoc;
    private readonly List<InputFormItem> _listIFI;
    private readonly List<LabelTextField> _listTextBox;
    private ChuKyDaoTaoController _chuKyDaoTaoController;

    private CustomButton _exitButton;
    private KhoaHocController _KhoaHocController;

    public KhoaHocDialog(string title, DialogType dialogType, List<InputFormItem> listIFI,
        KhoaHocController KhoaHocController, ChuKyDaoTaoController chuKyDaoTaoController,
        int idKhoaHoc = -1) : base(title, dialogType)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _KhoaHocController = KhoaHocController;
        _chuKyDaoTaoController = chuKyDaoTaoController;
        _idKhoaHoc = idKhoaHoc;
        Init();
    }

    public event Action Finish;

    private void Init()
    {
        for (var i = 0; i < _listIFI.Count; i++)
        {
            _listTextBox.Add(new LabelTextField(_listIFI[i].content, _listIFI[i].type));
            _textBoxsContainer.Controls.Add(_listTextBox[i]);
        }

        if (_dialogType == DialogType.Them)
            SetupInsert();
        else if (_dialogType == DialogType.Sua)
            SetupUpdate();
        else
            SetupDetail();
    }

    //Set dữ liệu có sẵn hoặc những dòng không được chọn
    private void SetupInsert()
    {
        _btnLuu._mouseDown += () => { Insert(); };
    }

    private void SetupUpdate()
    {
        if (_idKhoaHoc == -1) throw new Exception("Lỗi chưa cài đặt index");
        KhoaHocDto KhoaHoc = _KhoaHocController.GetKhoaHocById(_idKhoaHoc);

        _listTextBox[0].GetTextField().Text = KhoaHoc.TenKhoaHoc + "";
        _listTextBox[1].GetTextField().Text = KhoaHoc.NienKhoaHoc + "";

        _btnLuu._mouseDown += () => { UpdateTK(KhoaHoc); };
    }

    private void SetupDetail()
    {
        if (_idKhoaHoc == -1) throw new Exception("Lỗi chưa cài đặt index");
        KhoaHocDto KhoaHoc = _KhoaHocController.GetKhoaHocById(_idKhoaHoc);

        _listTextBox[0].GetTextField().Text = KhoaHoc.TenKhoaHoc + "";
        _listTextBox[1].GetTextField().Text = KhoaHoc.NienKhoaHoc + "";

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._field.Enable = false;
    }

    private void Insert()
    {
        var TxtTenKhoaHoc = _listTextBox[0].GetTextField();
        var TxtNienKhoaHoc = _listTextBox[1].GetTextField();

        var tenKhoaHoc = _listTextBox[0].GetTextTextField();
        var nienKhoaHoc = _listTextBox[1].GetTextTextField();

        int maCKDT;

        if (Validate(TxtTenKhoaHoc, TxtNienKhoaHoc, tenKhoaHoc, nienKhoaHoc))
        {
            maCKDT = _chuKyDaoTaoController.GetByNienKhoa(nienKhoaHoc).MaCKDT;
            var KhoaHoc = new KhoaHocDto
            {
                MaCKDT = maCKDT,
                TenKhoaHoc = tenKhoaHoc,
                NienKhoaHoc = nienKhoaHoc
            };

            if (_KhoaHocController.Insert(KhoaHoc))
            {
                MessageBox.Show("Thêm khóa học thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Thêm khóa học thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }
    }

    private void UpdateTK(KhoaHocDto KhoaHoc)
    {
        var TxtTenKhoaHoc = _listTextBox[0].GetTextField();
        var TxtNienKhoaHoc = _listTextBox[1].GetTextField();

        var tenKhoaHoc = _listTextBox[0].GetTextTextField();
        var nienKhoaHoc = _listTextBox[1].GetTextTextField();


        int maCKDT;
        if (Validate(TxtTenKhoaHoc, TxtNienKhoaHoc, tenKhoaHoc, nienKhoaHoc))
        {
            maCKDT = _chuKyDaoTaoController.GetByNienKhoa(nienKhoaHoc).MaCKDT;
            var KhoaHocNew = new KhoaHocDto
            {
                MaKhoaHoc = KhoaHoc.MaKhoaHoc,
                MaCKDT = maCKDT,
                TenKhoaHoc = tenKhoaHoc,
                NienKhoaHoc = nienKhoaHoc
            };

            if (_KhoaHocController.Update(KhoaHocNew))
            {
                MessageBox.Show("Sửa khóa học thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Sửa khóa học thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }
    }

    private bool Validate(TextBox TxtTenKhoaHoc, TextBox NienKhoaHoc, string tenKhoaHoc, string nienKhoaHoc)
    {
        if (Shared.Validate.IsEmpty(tenKhoaHoc))
        {
            MessageBox.Show("Tên khóa học không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtTenKhoaHoc.Focus();
            return false;
        }

        if (Shared.Validate.IsEmpty(nienKhoaHoc))
        {
            MessageBox.Show("Niên khóa học không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            NienKhoaHoc.Focus();
            return false;
        }

        if (!Shared.Validate.IsAcademicYear(nienKhoaHoc))
        {
            MessageBox.Show("Niên khóa học không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            NienKhoaHoc.Focus();
            return false;
        }

        //set mã chu kỳ tự động
        if (!_chuKyDaoTaoController.Validate(nienKhoaHoc))
        {
            MessageBox.Show("Niên khóa học không hợp lệ!, không nằm trong một chu kỳ nào!", "Lỗi", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            NienKhoaHoc.Focus();
            return false;
        }

        return true;
    }
}