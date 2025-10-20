using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class PhongHoc : NavBase
{
    private string ID = "PHONGHOC";
    private string[] _listSelectionForComboBox = new []{"Mã phòng học", "Tên phòng học"};
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    private List<ChiTietQuyenDto> _listAccess;
    
    public PhongHoc(NhomQuyenDto quyen) : base(quyen)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        Init();
    }
        
    private void Init()
    {
        CheckQuyen();
        //BackColor = Color.Blue;
        Dock = DockStyle.Bottom;
        Size = new Size(1200, 900);
        var borderTop = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new  Padding(0 , 30 , 0 , 0),
        };
        borderTop.Controls.Add(Top());
        Controls.Add(borderTop);
        Controls.Add(Bottom());
    }
    
    void CheckQuyen()
    {
        int maCN = _chucNangController.GetByTen(ID).MaCN;
        _listAccess = _chiTietQuyenController.GetByMaNQMaCN(_quyen.MaNQ, maCN);
        foreach (ChiTietQuyenDto x in _listAccess)
        {
            Console.WriteLine(x.HanhDong);
        }
    }

    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Bottom,
            // BackColor = ColorTranslator.FromHtml("#E5E7EB"),
            BackColor = Color.Red,
            Height = 90,
        };
        return mainTop;
    }

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = Color.Green,
            Height = 780,
        };
        return mainBot;
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    public override void onSearch(string txtSearch, string filter)
    { }
}