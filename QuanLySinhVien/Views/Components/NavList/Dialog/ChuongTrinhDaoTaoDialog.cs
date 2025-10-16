using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class ChuongTrinhDaoTaoDialog : Form
{

    private CustomButton _exitButton;
    private List<InputFormItem> _listIFI;
    private List<LabelTextField> _listTextBox;
    private ChuongTrinhDaoTaoController _ChuongTrinhDaoTaoController;
    private int _idChuongTrinhDaoTao;
    public event Action Finish;
    
    private TableLayoutPanel _mainLayout;
    private TitleButton _btnLuu;
    
    private string _title;
    private DialogType _dialogType;
    
    

    public ChuongTrinhDaoTaoDialog(string title, DialogType dialogType, List<InputFormItem> listIFI, ChuongTrinhDaoTaoController chuongTrinhDaoTaoController, int idChuongTrinhDaoTao = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _ChuongTrinhDaoTaoController = chuongTrinhDaoTaoController;
        _idChuongTrinhDaoTao = idChuongTrinhDaoTao;
        Init();
    }

    void Init()
    {
        Width = 1200;
        Height = 900;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        
        _mainLayout = new  TableLayoutPanel
        {
            Dock =  DockStyle.Fill,
            RowCount = 4,
        };
        
        this.Controls.Add(_mainLayout);
        
        
        
        // if (_dialogType == DialogType.Them)
        // {
        //     SetupInsert();
        // }
        // else if (_dialogType == DialogType.Sua)
        // {
        //     SetupUpdate();
        // }
        // else
        // {
        //     SetupDetail();
        // }
        
    }
    

    void Insert()
    {
        

    }

    void UpdateTK(ChuongTrinhDaoTaoDto ChuongTrinhDaoTao)
    {
        
    }

    bool Validate(TextBox TxtTenDangNhap, TextBox TxtMatKhau, string tenDangNhap, string matKhau)
    {
        return true;
    }
    
    
}