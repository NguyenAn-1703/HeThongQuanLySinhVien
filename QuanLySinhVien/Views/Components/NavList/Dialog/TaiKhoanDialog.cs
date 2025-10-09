using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class TaiKhoanDialog : CustomDialog
{

    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;

    public TaiKhoanDialog(string title, DialogType dialogType) : base(title, dialogType)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = new  List<InputFormItem>();
        Init();
    }

    void Init()
    {
        InputFormItem[] arr = new InputFormItem[]
        {
            new InputFormItem("Tên tài khoản", TextFieldType.NormalText),
            new InputFormItem("Mật khẩu", TextFieldType.NormalText),
            new InputFormItem("Nhóm quyền", TextFieldType.Combobox),
        };
        _listIFI.AddRange(arr);
        
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

    //Set những dòng không được chọn
    void SetupInsert()
    {

    }

    void SetupUpdate()
    {

    }

    void SetupDetail()
    {

    }
    
}