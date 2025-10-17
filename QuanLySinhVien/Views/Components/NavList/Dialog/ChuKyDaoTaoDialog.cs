using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class ChuKyDaoTaoDialog : CustomDialog
{

    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private ChuKyDaoTaoController _ChuKyDaoTaoController;
    private int _idChuKyDaoTao;
    public event Action Finish;

    public ChuKyDaoTaoDialog(string title, DialogType dialogType, List<InputFormItem> listIFI, ChuKyDaoTaoController ChuKyDaoTaoController, int idChuKyDaoTao = -1) : base(title, dialogType)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _ChuKyDaoTaoController = ChuKyDaoTaoController;
        _idChuKyDaoTao = idChuKyDaoTao;
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
        if (_idChuKyDaoTao == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        ChuKyDaoTaoDto ChuKyDaoTao = _ChuKyDaoTaoController.GetById(_idChuKyDaoTao);
        
        _listTextBox[0].GetTextField().Text = ChuKyDaoTao.NamBatDau + "";
        _listTextBox[1].GetTextField().Text = ChuKyDaoTao.NamKetThuc + "";
        
        _btnLuu._mouseDown += () => { UpdateTK(ChuKyDaoTao); };
    }

    void SetupDetail()
    {
        if (_idChuKyDaoTao == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        ChuKyDaoTaoDto ChuKyDaoTao = _ChuKyDaoTaoController.GetById(_idChuKyDaoTao);
        
        _listTextBox[0].GetTextField().Text = ChuKyDaoTao.NamBatDau + "";
        _listTextBox[1].GetTextField().Text = ChuKyDaoTao.NamKetThuc + "";

        _listTextBox[0]._field.Enable = false;
        _listTextBox[1]._field.Enable = false;
        
    }

    void Insert()
    {
        TextBox TxtNamBatDau = _listTextBox[0].GetTextField();
        TextBox TxtNamKetThuc = _listTextBox[1].GetTextField();

        string namBatDau = _listTextBox[0].GetTextTextField();
        string namKetThuc = _listTextBox[1].GetTextTextField();
        
        if (Validate(TxtNamBatDau, TxtNamKetThuc, namBatDau, namKetThuc))
        {
            ChuKyDaoTaoDto ChuKyDaoTao = new ChuKyDaoTaoDto
            {
                NamBatDau = namBatDau,
                NamKetThuc = namKetThuc,
            };
            
            if (_ChuKyDaoTaoController.Insert(ChuKyDaoTao))
            {
                MessageBox.Show("Thêm chu kỳ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm chu kỳ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            
        }

    }

    void UpdateTK(ChuKyDaoTaoDto ChuKyDaoTao)
    {
        TextBox TxtNamBatDau = _listTextBox[0].GetTextField();
        TextBox TxtNamKetThuc = _listTextBox[1].GetTextField();

        string namBatDau = _listTextBox[0].GetTextTextField();
        string namKetThuc = _listTextBox[1].GetTextTextField();

        if (Validate(TxtNamBatDau,TxtNamKetThuc,namBatDau,namKetThuc))
        {
            ChuKyDaoTaoDto ChuKyDaoTaoNew = new ChuKyDaoTaoDto
            {
                MaCKDT = ChuKyDaoTao.MaCKDT,
                NamBatDau = namBatDau,
                NamKetThuc = namKetThuc,
            };
            
            if (_ChuKyDaoTaoController.Update(ChuKyDaoTaoNew))
            {
                MessageBox.Show("Sửa chu kỳ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sửa chu kỳ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
    }

    bool Validate(TextBox TxtNamBatDau, TextBox TxtNamKetThuc, string namBatDau, string namKetThuc)
    {
        if (CommonUse.Validate.IsEmpty(namBatDau))
        {
            MessageBox.Show("Năm bắt đầu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamBatDau.Focus();
            return false;
        }
        if (CommonUse.Validate.IsEmpty(namKetThuc))
        {
            MessageBox.Show("Năm kết thúc không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamKetThuc.Focus();
            return false;
        }
        if (!CommonUse.Validate.IsYear(namBatDau))
        {
            MessageBox.Show("Năm bắt đầu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamBatDau.Focus();
            return false;
        }
        if (!CommonUse.Validate.IsYear(namKetThuc))
        {
            MessageBox.Show("Năm kết thúc không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamKetThuc.Focus();
            return false;
        }
        if (!CommonUse.Validate.IsStartYearAndEndYear(namBatDau, namKetThuc))
        {
            MessageBox.Show("Năm bắt đầu phải nhỏ hơn năm kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtNamBatDau.Focus();
            return false;
        }
        
        return true;
    }
    
    
    
}