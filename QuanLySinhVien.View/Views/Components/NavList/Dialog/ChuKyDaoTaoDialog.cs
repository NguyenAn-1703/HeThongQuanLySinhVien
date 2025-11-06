using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class ChuKyDaoTaoDialog : CustomDialog
{
    private readonly int _idChuKyDaoTao;
    private readonly List<InputFormItem> _listIFI;
    private readonly List<LabelTextField> _listTextBox;
    private ChuKyDaoTaoController _ChuKyDaoTaoController;

    private CustomButton _exitButton;

    public ChuKyDaoTaoDialog(string title, DialogType dialogType, List<InputFormItem> listIFI,
        ChuKyDaoTaoController ChuKyDaoTaoController, int idChuKyDaoTao = -1) : base(title, dialogType)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _ChuKyDaoTaoController = ChuKyDaoTaoController;
        _idChuKyDaoTao = idChuKyDaoTao;
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
        if (_idChuKyDaoTao == -1) throw new Exception("Lỗi chưa cài đặt index");
        ChuKyDaoTaoDto ChuKyDaoTao = _ChuKyDaoTaoController.GetById(_idChuKyDaoTao);

        _listTextBox[0].GetTextField().Text = ChuKyDaoTao.NamBatDau + "";
        _listTextBox[1].GetTextField().Text = ChuKyDaoTao.NamKetThuc + "";

        _btnLuu._mouseDown += () => { UpdateTK(ChuKyDaoTao); };
    }

    private void SetupDetail()
    {
        if (_idChuKyDaoTao == -1) throw new Exception("Lỗi chưa cài đặt index");
        ChuKyDaoTaoDto ChuKyDaoTao = _ChuKyDaoTaoController.GetById(_idChuKyDaoTao);

        _listTextBox[0].GetTextField().Text = ChuKyDaoTao.NamBatDau + "";
        _listTextBox[1].GetTextField().Text = ChuKyDaoTao.NamKetThuc + "";

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._field.Enable = false;
    }

    private void Insert()
    {
        var TxtNamBatDau = _listTextBox[0].GetTextField();
        var TxtNamKetThuc = _listTextBox[1].GetTextField();

        var namBatDau = _listTextBox[0].GetTextTextField();
        var namKetThuc = _listTextBox[1].GetTextTextField();

        if (Validate(TxtNamBatDau, TxtNamKetThuc, namBatDau, namKetThuc))
        {
            var ChuKyDaoTao = new ChuKyDaoTaoDto
            {
                NamBatDau = namBatDau,
                NamKetThuc = namKetThuc
            };

            if (_ChuKyDaoTaoController.Insert(ChuKyDaoTao))
            {
                MessageBox.Show("Thêm chu kỳ thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Thêm chu kỳ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }
    }

    private void UpdateTK(ChuKyDaoTaoDto ChuKyDaoTao)
    {
        var TxtNamBatDau = _listTextBox[0].GetTextField();
        var TxtNamKetThuc = _listTextBox[1].GetTextField();

        var namBatDau = _listTextBox[0].GetTextTextField();
        var namKetThuc = _listTextBox[1].GetTextTextField();

        if (Validate(TxtNamBatDau, TxtNamKetThuc, namBatDau, namKetThuc))
        {
            var ChuKyDaoTaoNew = new ChuKyDaoTaoDto
            {
                MaCKDT = ChuKyDaoTao.MaCKDT,
                NamBatDau = namBatDau,
                NamKetThuc = namKetThuc
            };

            if (_ChuKyDaoTaoController.Update(ChuKyDaoTaoNew))
            {
                MessageBox.Show("Sửa chu kỳ thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Sửa chu kỳ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }
    }

    private bool Validate(TextBox TxtNamBatDau, TextBox TxtNamKetThuc, string namBatDau, string namKetThuc)
    {
        if (Shared.Validate.IsEmpty(namBatDau))
        {
            MessageBox.Show("Năm bắt đầu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamBatDau.Focus();
            return false;
        }

        if (Shared.Validate.IsEmpty(namKetThuc))
        {
            MessageBox.Show("Năm kết thúc không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamKetThuc.Focus();
            return false;
        }

        if (!Shared.Validate.IsYear(namBatDau))
        {
            MessageBox.Show("Năm bắt đầu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamBatDau.Focus();
            return false;
        }

        if (!Shared.Validate.IsYear(namKetThuc))
        {
            MessageBox.Show("Năm kết thúc không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamKetThuc.Focus();
            return false;
        }

        if (!Shared.Validate.IsStartYearAndEndYear(namBatDau, namKetThuc))
        {
            MessageBox.Show("Năm bắt đầu phải nhỏ hơn năm kết thúc!", "Lỗi", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            TxtNamBatDau.Focus();
            return false;
        }

        return true;
    }
}