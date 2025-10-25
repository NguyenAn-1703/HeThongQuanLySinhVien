using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.utils;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class LopDialog : CustomDialog
{
    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private LopController _LopController;
    private int _idLop;
    public event Action Finish;
    
    private List<NganhDto> _listNganh;
    private List<GiangVienDto> _listGiangVien;

    private NganhController _nganhController;
    private GiangVienController _giangVienController;

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
        List<string> listTenGV = _listGiangVien.Select(x => x.TenGV).ToList();
        List<string> listTenNganh = _listNganh.Select(x => x.TenNganh).ToList();
        
        _listTextBox[1]._combobox.UpdateSelection(listTenGV.ToArray());
        _listTextBox[1]._combobox.SetSelectionCombobox(listTenGV[0]);
        _listTextBox[2]._combobox.UpdateSelection(listTenNganh.ToArray());
        _listTextBox[2]._combobox.SetSelectionCombobox(listTenNganh[0]);
        
        _btnLuu._mouseDown += () => { Insert(); };
    }

    void SetupUpdate()
    {
        if (_idLop == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        
        List<string> listTenGV = _listGiangVien.Select(x => x.TenGV).ToList();
        List<string> listTenNganh = _listNganh.Select(x => x.TenNganh).ToList();
        
        LopDto Lop = _LopController.GetLopById(_idLop);
        
        _listTextBox[0].GetTextField().Text = Lop.TenLop;
        
        _listTextBox[1]._combobox.UpdateSelection(listTenGV.ToArray());
        _listTextBox[1]._combobox.SetSelectionCombobox(listTenGV[0]);
        
        _listTextBox[2]._combobox.UpdateSelection(listTenNganh.ToArray());
        _listTextBox[2]._combobox.SetSelectionCombobox(listTenNganh[0]);
        
        
        _btnLuu._mouseDown += () => { Update(Lop); };
    }

    void SetupDetail()
    {
        if (_idLop == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        
        List<string> listTenGV = _listGiangVien.Select(x => x.TenGV).ToList();
        List<string> listTenNganh = _listNganh.Select(x => x.TenNganh).ToList();
        
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
    

    void Insert()
    {
        TextBox TxtTenLop = _listTextBox[0].GetTextField();
        string tenLop = TxtTenLop.Text;

        int maGV = _giangVienController.GetByTen(_listTextBox[1].GetSelectionCombobox()).MaGV;
        int maNganh = _nganhController.GetByTen(_listTextBox[2].GetSelectionCombobox()).MaNganh;
        
        if (Validate(TxtTenLop, tenLop))
        {
            LopDto Lop = new LopDto
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
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
    }

    void Update(LopDto Lop)
    {
        TextBox TxtTenLop = _listTextBox[0].GetTextField();
        string tenLop = TxtTenLop.Text;

        int maGV = _giangVienController.GetByTen(_listTextBox[1].GetSelectionCombobox()).MaGV;
        int maNganh = _nganhController.GetByTen(_listTextBox[2].GetSelectionCombobox()).MaNganh;
        
        if (Validate(TxtTenLop, tenLop))
        {
            LopDto LopNew = new LopDto
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
                this.Close();
            }
            else
            {
                MessageBox.Show("Sửa thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
    }

    bool Validate(TextBox TxtLop, string tenLop)
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