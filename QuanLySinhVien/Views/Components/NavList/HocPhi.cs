using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class HocPhi : NavBase
{
    private string ID = "HOCPHI";
    private string[] _listSelectionForComboBox = new []{""};
    
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;
    
    
    private List<ChiTietQuyenDto> _listAccess;
    public HocPhi(NhomQuyenDto quyen, TaiKhoanDto taiKhoan) : base(quyen, taiKhoan)
    {
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        Init();
    }
    

    
    
    // -------------- Graphics --------------- //
    private float GetFontWidth(Label label)
    {
        Graphics g = label.CreateGraphics();
        SizeF size = g.MeasureString(label.Text, label.Font);

        return size.Width;
    }
    
    
    
    
    
    
    // -------------- Label --------------- //
    private Label LbHeding()
    {
        Label lb = new Label
        {
            Dock = DockStyle.Left,
            Text = "Học phí",
            Font = new Font("JetBrains Mono", 17f, FontStyle.Bold),
            Height = 90,
            TextAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(30, 0, 0, 0),
        };
        
        lb.Width = Convert.ToInt32(GetFontWidth(lb)) + 50;
        return lb;
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
            //Padding = new  Padding(0 , 30 , 0 , 0),
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
            Controls = { LbHeding() }
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