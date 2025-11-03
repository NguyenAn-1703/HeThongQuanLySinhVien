using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class LopDialog : CustomDialog
{
    private readonly int _idLop;
    private readonly List<GiangVienDto> _listGiangVien;
    private readonly List<InputFormItem> _listIFI;

    private readonly List<NganhDto> _listNganh;
    private readonly List<LabelTextField> _listTextBox;
    private CustomButton _exitButton;
    private GiangVienController _giangVienController;
    private LopController _LopController;

    private NganhController _nganhController;

    public LopDialog(string title, DialogType dialogType, List<InputFormItem> listIFI,
        LopController LopController,
        int idLop = -1) : base(title, dialogType)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _LopController = LopController;
        _idLop = idLop;
        _nganhController = NganhController.GetInstance();
        _giangVienController = GiangVienController.GetInstance();
        _listNganh = _nganhController.GetAll();
        _listGiangVien = _giangVienController.GetAll();
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
        var listTenGV = _listGiangVien.Select(x => x.TenGV).ToList();
        var listTenNganh = _listNganh.Select(x => x.TenNganh).ToList();

        _listTextBox[1]._combobox.UpdateSelection(listTenGV.ToArray());
        _listTextBox[1]._combobox.SetSelectionCombobox(listTenGV[0]);
        _listTextBox[2]._combobox.UpdateSelection(listTenNganh.ToArray());
        _listTextBox[2]._combobox.SetSelectionCombobox(listTenNganh[0]);

        _btnLuu._mouseDown += () => { Insert(); };
    }

    private void SetupUpdate()
    {
        if (_idLop == -1) throw new Exception("Lỗi chưa cài đặt index");

        var listTenGV = _listGiangVien.Select(x => x.TenGV).ToList();
        var listTenNganh = _listNganh.Select(x => x.TenNganh).ToList();

        LopDto Lop = _LopController.GetLopById(_idLop);

        _listTextBox[0].GetTextField().Text = Lop.TenLop;

        _listTextBox[1]._combobox.UpdateSelection(listTenGV.ToArray());
        _listTextBox[1]._combobox.SetSelectionCombobox(listTenGV[0]);

        _listTextBox[2]._combobox.UpdateSelection(listTenNganh.ToArray());
        _listTextBox[2]._combobox.SetSelectionCombobox(listTenNganh[0]);


        _btnLuu._mouseDown += () => { Update(Lop); };
    }

    private void SetupDetail()
    {
        if (_idLop == -1) throw new Exception("Lỗi chưa cài đặt index");

        var listTenGV = _listGiangVien.Select(x => x.TenGV).ToList();
        var listTenNganh = _listNganh.Select(x => x.TenNganh).ToList();

        LopDto Lop = _LopController.GetLopById(_idLop);

        _listTextBox[0].GetTextField().Text = Lop.TenLop;

        _listTextBox[1]._combobox.UpdateSelection(listTenGV.ToArray());
        _listTextBox[1]._combobox.SetSelectionCombobox(listTenGV[0]);

        _listTextBox[2]._combobox.UpdateSelection(listTenNganh.ToArray());
        _listTextBox[2]._combobox.SetSelectionCombobox(listTenNganh[0]);

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._combobox.Enable = false;
        _listTextBox[2]._combobox.Enable = false;
    }


    private void Insert()
    {
        var TxtTenLop = _listTextBox[0].GetTextField();
        var tenLop = TxtTenLop.Text;

        int maGV = _giangVienController.GetByTen(_listTextBox[1].GetSelectionCombobox()).MaGV;
        int maNganh = _nganhController.GetByTen(_listTextBox[2].GetSelectionCombobox()).MaNganh;

        if (Validate(TxtTenLop, tenLop))
        {
            var Lop = new LopDto
            {
                TenLop = tenLop,
                MaGV = maGV,
                MaNganh = maNganh
            };

            if (_LopController.Insert(Lop))
            {
                MessageBox.Show("Thêm thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Thêm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }
    }

    private void Update(LopDto Lop)
    {
        var TxtTenLop = _listTextBox[0].GetTextField();
        var tenLop = TxtTenLop.Text;

        int maGV = _giangVienController.GetByTen(_listTextBox[1].GetSelectionCombobox()).MaGV;
        int maNganh = _nganhController.GetByTen(_listTextBox[2].GetSelectionCombobox()).MaNganh;

        if (Validate(TxtTenLop, tenLop))
        {
            var LopNew = new LopDto
            {
                MaLop = Lop.MaLop,
                TenLop = tenLop,
                MaGV = maGV,
                MaNganh = maNganh
            };

            if (_LopController.Update(LopNew))
            {
                MessageBox.Show("Sửa thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Sửa thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }
    }

    private bool Validate(TextBox TxtLop, string tenLop)
    {
        if (CommonUse.Validate.IsEmpty(tenLop))
        {
            MessageBox.Show("Tên lớp không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtLop.Focus();
            return false;
        }

        return true;
    }
}