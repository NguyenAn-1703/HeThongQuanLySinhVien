using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class DiemSVDialog : CustomDialog
{
    private KetQuaController _ketQuaController;
    private DiemQuaTrinhController _diemQuaTrinhController;
    private HocPhanController _hocPhanController;
    private int _maSv;
    private int _maHp;
    
    public DiemSVDialog(int maSV, int maHP) : base ("Điểm sinh viên", DialogType.ChiTiet, 400, 350)
    {
        _ketQuaController = KetQuaController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _diemQuaTrinhController = DiemQuaTrinhController.GetInstance();
        _maSv = maSV;
        _maHp = maHP;
        Init();
    }

    private MyTLP mainLayout;
    void Init()
    {
        mainLayout = new MyTLP
        {
            RowCount = 2,
            Dock = DockStyle.Fill
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        SetHocPhanHeSoContainer();
        SetDiemContainer();
        SetDiemContainer();
        
        _textBoxsContainer.Controls.Add(mainLayout);
    }

    void SetHocPhanHeSoContainer()
    {
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(_maHp);
        string tenHp = hocPhan.TenHP;
        string heSo = hocPhan.HeSoHocPhan;
        MyTLP panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
        };

        Label lblHp = new Label
        {
            Text = "Học phần: " + tenHp,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold)
        };
        
        Label lblHs = new Label
        {
            Text = "Hệ số: " + heSo,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold)
        };
        
        panel.Controls.Add(lblHp);
        panel.Controls.Add(lblHs);
        mainLayout.Controls.Add(panel);

    }

    void SetDiemContainer()
    {
        RoundTLP panel = new RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            ColumnCount = 2,
            Padding = new Padding(5),
            Border = true,
        };
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        panel.ColumnStyles.Add(new  ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new  ColumnStyle(SizeType.Percent, 100));

        KetQuaDto ketQua = _ketQuaController.GetByMaSVMaHP(_maSv, _maHp);
        DiemQuaTrinhDto diemQuaTrinh = _diemQuaTrinhController.GetByMaKQ(ketQua.MaKQ);
        
        Label lblDqt = getLabel("Điểm quá trình");
        Label lblDqtD = getLabel(diemQuaTrinh.DiemSo + "");
        lblDqtD.Anchor = AnchorStyles.None;
        lblDqtD.ForeColor = MyColor.MainColor;

        Label lblDT = getLabel("Điểm thi");
        Label lblDTD = getLabel(ketQua.DiemThi + "");
        lblDTD.Anchor = AnchorStyles.None;
        lblDTD.ForeColor = MyColor.MainColor;

        
        Label lblDH4 = getLabel("Điểm hệ 4");
        Label lblDH4D = getLabel(ketQua.DiemHe4 + "");
        lblDH4D.Anchor = AnchorStyles.None;
        lblDH4D.ForeColor = MyColor.MainColor;

        
        Label lblDH10 = getLabel("Điểm hệ 10");
        Label lblDH10D = getLabel(ketQua.DiemHe10 + "");
        lblDH10D.Anchor = AnchorStyles.None;
        lblDH10D.ForeColor = MyColor.MainColor;


        
        panel.Controls.Add(lblDqt);
        panel.Controls.Add(lblDqtD);
        panel.Controls.Add(lblDT);
        panel.Controls.Add(lblDTD);
        panel.Controls.Add(lblDH4);
        panel.Controls.Add(lblDH4D);
        panel.Controls.Add(lblDH10);
        panel.Controls.Add(lblDH10D);
        
        mainLayout.Controls.Add(panel);
        
    }


    Label getLabel(string content)
    {
        Label lblHs = new Label
        {
            Text = content,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(10, FontType.SemiBold)
        };
        return lblHs;
    }
}