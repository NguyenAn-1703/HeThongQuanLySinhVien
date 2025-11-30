using Mysqlx.Crud;
using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class NamHocKyDialog : CustomDialog
{
    private LabelTextField _fieldHocKy;
    private LabelTextField _fieldNam;
    public event Action Finish;

    private DotDangKyController _dotDangKyController;
    public NamHocKyDialog(string title, DialogType dialogType, int width = 375, int height = 300) : base(title, dialogType, width, height)
    {
        _dotDangKyController = DotDangKyController.GetInstance();
        Init();
    }

    void Init()
    {
        string[] hockys = new[] { "Học kỳ 1", "Học kỳ 2" };
        
        _fieldHocKy = new LabelTextField("Học kỳ", TextFieldType.Combobox);
        _fieldNam = new LabelTextField("Năm", TextFieldType.Year);

        _fieldNam._namField.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
        _fieldNam._namField.Dock = DockStyle.Fill;
        
        _fieldHocKy.SetComboboxList(hockys.ToList());
        _fieldHocKy.SetComboboxSelection(hockys[0]);
        
        _textBoxsContainer.Controls.Add(_fieldHocKy);
        _textBoxsContainer.Controls.Add(_fieldNam);

        SetAction();
    }

    void SetAction()
    {
        _btnLuu._mouseDown += () => Insert();
    }

    void Insert()
    {
        string nam = _fieldNam._namField.Text;
        int hky = int.Parse(_fieldHocKy.GetSelectionCombobox().Split(' ')[2]);
        DotDangKyDto dotDK = new DotDangKyDto
        {
            HocKy = hky,
            Nam = nam,
            ThoiGianBatDau = DateTime.Now,
            ThoiGianKetThuc = DateTime.Now
        };
        if (_dotDangKyController.Insert(dotDK))
        {
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