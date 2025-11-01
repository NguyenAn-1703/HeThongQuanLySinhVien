using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NhapDiemThiDialog : RoundTLP
{
    private string _title;
    private MyTLP _mainLayout;
    private CustomButton _exitButton;
    private List<LabelTextField> _listTextBox;
    private int _idCaThi;
    private HocPhanDto _hocPhan;
    private TitleButton _btnLuu;
    public event Action Finish;
    private string imgPath = "";
    private TableNhapDiemThi _tableSV;

    private List<SinhVienDTO> _rawData;
    private List<object> _displayData;

    private SinhVienController _sinhVienController;
    private CaThi_SinhVienController _caThiSinhVienController;
    private CaThiController _caThiController;
    private HocPhanController _hocPhanController;
    private NhomHocPhanController _nhomHocPhanController;
    private DiemQuaTrinhController _diemQuaTrinhController;
    private KetQuaController _ketQuaController;
    private CotDiemController _cotDiemController;
    private CustomButton _backButton;
    public event Action Back;

    public NhapDiemThiDialog(string title, int idCaThi = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _idCaThi = idCaThi;
        _title = title;
        _sinhVienController = SinhVienController.GetInstance();
        _caThiSinhVienController = CaThi_SinhVienController.GetInstance();
        _hocPhanController =  HocPhanController.GetInstance();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _diemQuaTrinhController =  DiemQuaTrinhController.GetInstance();
        _ketQuaController =  KetQuaController.GetInstance();
        _cotDiemController =  CotDiemController.GetInstance();
        _caThiController = CaThiController.GetInstance();
        Init();
    }

    void Init()
    {
        Dock = DockStyle.Fill;
        BackColor = MyColor.White;
        Margin = new Padding(0);
        Border = true;


        _mainLayout = new MyTLP()
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
        };

        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        _hocPhan = _hocPhanController.GetHocPhanById(_caThiController.GetById(_idCaThi).MaHP);
        
        SetContent();
        SetBottom();
        this.Controls.Add(_mainLayout);
        SetAction();
        SetupDetail();
    }

    private MyTLP _contentLayout;

    void SetContent()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1,
        };
        
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTitleBar();
        
        string[] headers = new string[] {"Mã sinh viên", "Tên sinh viên", "Giới tính","Ngày sinh" }; 
        string[] columnNames = new string [] {"MaSV","TenSV","GioiTinh","NgaySinh"};
        SetDisplayData();
        SetupCotDiem();
        
        _tableSV = new TableNhapDiemThi(headers.ToList(),columnNames.ToList(), _displayData, listDiemThiSV, _hocPhan.MaHP);
        _contentLayout.Controls.Add(_tableSV);

        _btnLuu = new TitleButton("Lưu thay đổi"){Dock = DockStyle.Right};
        _contentLayout.Controls.Add(_btnLuu);
        
        _mainLayout.Controls.Add(_contentLayout);
    }

    void SetTitleBar()
    {
        MyTLP panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3,
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _backButton = new CustomButton(20, 20, "back.svg", MyColor.GrayBackGround){Pad = 5, Anchor = AnchorStyles.None};
        _backButton.HoverColor = MyColor.GrayHoverColor;
        _backButton.SelectColor = MyColor.GraySelectColor;
        
        Label lblTitle = new Label
        {
            Text = "DS sinh viên thi ca thi mã : " + _idCaThi + " , Học phần: " + _hocPhanController.GetHocPhanById(_hocPhan.MaHP).TenHP,
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold),
            Margin = new Padding(7, 3, 3, 3),
        };
        panel.Controls.Add(_backButton);
        panel.Controls.Add(lblTitle);
        
        _contentLayout.Controls.Add(panel);
    }

    void SetDisplayData()
    {
        _rawData = new List<SinhVienDTO>();
        
        // Sinh Vien trong ca thi
        List<CaThi_SinhVienDto> listCTSV = _caThiSinhVienController.GetByMaCT(_idCaThi);
        foreach (CaThi_SinhVienDto item in listCTSV)
        {
            _rawData.Add(_sinhVienController.GetById(item.MaSV));
        }
        
        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                MaSV = x.MaSV,
                TenSV = x.TenSV,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh
            }
        );
        
    }

    void UpdateDataDisplay(List<SVNhapDiemDisplay> input)
    {
        this._displayData = ConvertObject.ConvertToDisplay(input, x => new
        {
            MaSV = x.MaSV,
            TenSV = x.TenSV,
            GioiTinh = x.GioiTinh,
            NgaySinh = x.NgaySinh
        });
    }

    void SetBottom()
    {
        //Thêm có Đặt lại, Lưu, Hủy
        MyTLP panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3,
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));


        panel.Controls.Add(new Panel { Height = 0 });
        
        this._mainLayout.Controls.Add(panel, 0, 3);
    }
    

    void SetupDetail()
    {
        
    }

    void SetAction()
    {
        _backButton._mouseDown += () => Back?.Invoke();
        _btnLuu._mouseDown += () => UpdateDiem();
    }

    void UpdateDiem()
    {
        DialogResult rs = MessageBox.Show("Xác nhận lưu thay đổi ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (rs == DialogResult.No)
        {
            return;
        }
        _tableSV.UpdateDiem();
    }
    
    List<SVNhapDiemDisplay> ConvertDtoToDisplay(List<SinhVienDTO> input)
    {
        List<SVNhapDiemDisplay> rs = ConvertObject.ConvertDtoToDto(input, x => new SVNhapDiemDisplay
        {
            MaSV = x.MaSinhVien,
            TenSV = x.TenSinhVien,
            GioiTinh = x.GioiTinh,
            NgaySinh = x.NgaySinh
        });
        return rs;
    }

    private List<DiemThiSV> listDiemThiSV = new List<DiemThiSV>();
    void SetupCotDiem()
    {
        int maHP = _hocPhan.MaHP;
        foreach (SinhVienDTO sv in _rawData)
        {
            KetQuaDto kq = _ketQuaController.GetByMaSVMaHP(sv.MaSinhVien, maHP);
            DiemThiSV diemThi = new DiemThiSV
            {
                MaSV = sv.MaSinhVien,
                diemThi = kq.DiemThi,
            };
            listDiemThiSV.Add(diemThi);
        }
    }
}