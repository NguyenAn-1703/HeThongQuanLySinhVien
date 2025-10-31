using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.AddImg;
using QuanLySinhVien.Views.Components.CommonUse.Search;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NhapDiemDialog : RoundTLP
{
    private string _title;
    private MyTLP _mainLayout;
    private CustomButton _exitButton;
    private List<LabelTextField> _listTextBox;
    private int _idNHP;
    private NhomHocPhanDto _nhomHP;
    private TitleButton _btnLuu;
    public event Action Finish;
    private string imgPath = "";
    private TableNhapDiem _tableSV;

    private List<SinhVienDTO> _rawData;
    private List<object> _displayData;

    private SinhVienController _sinhVienController;
    private DangKyController _dangKyController;
    private HocPhanController _hocPhanController;
    private NhomHocPhanController _nhomHocPhanController;
    private DiemQuaTrinhController _diemQuaTrinhController;
    private KetQuaController _ketQuaController;
    private CotDiemController _cotDiemController;
    private CustomButton _backButton;
    public event Action Back;

    public NhapDiemDialog(string title, int idNHP = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _idNHP = idNHP;
        _title = title;
        _sinhVienController = SinhVienController.GetInstance();
        _dangKyController = DangKyController.GetInstance();
        _hocPhanController =  HocPhanController.GetInstance();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _nhomHP = _nhomHocPhanController.GetById(_idNHP);
        _diemQuaTrinhController =  DiemQuaTrinhController.GetInstance();
        _ketQuaController =  KetQuaController.GetInstance();
        _cotDiemController =  CotDiemController.GetInstance();
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
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
        };
        
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTitleBar();
        
        string[] headers = new string[] {"Mã sinh viên", "Tên sinh viên", "Giới tính","Ngày sinh" }; 
        string[] columnNames = new string [] {"MaSV","TenSV","GioiTinh","NgaySinh"};
        SetDisplayData();
        SetupCotDiem();
        
        _tableSV = new TableNhapDiem(headers.ToList(),columnNames.ToList(), _displayData, listDiemSV, _nhomHP.MaHP);
        _contentLayout.Controls.Add(_tableSV);

        _btnLuu = new TitleButton("Lưu thay đổi"){Dock = DockStyle.Right};
        _contentLayout.Controls.Add(_btnLuu);
        
        _mainLayout.Controls.Add(_contentLayout);
    }

    private CtdtSearchBar _searchBar;
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
            Text = "DS sinh viên học nhóm: " + _idNHP + " , Học phần: " + _hocPhanController.GetHocPhanById(_nhomHP.MaHP).TenHP,
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold),
            Margin = new Padding(7, 3, 3, 3),
        };
        _searchBar = new CtdtSearchBar();
        panel.Controls.Add(_backButton);
        panel.Controls.Add(lblTitle);
        panel.Controls.Add(_searchBar);
        
        _contentLayout.Controls.Add(panel);
    }

    void SetDisplayData()
    {
        _rawData = new List<SinhVienDTO>();
        List<DangKyDto> listDK =  _dangKyController.GetByMaNHP(_idNHP);
        foreach (DangKyDto item in listDK)
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
        _searchBar.KeyDown += (s) => OnSearch(s);
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
    
    void OnSearch(string input)
    {
        string keyword = input.ToLower().Trim();
        List<SVNhapDiemDisplay> result = ConvertDtoToDisplay(_rawData)
            .Where(x => x.MaSV.ToString().ToLower().Contains(keyword) || 
                        x.TenSV.ToString().ToLower().Contains(keyword) || 
                        x.GioiTinh.ToString().ToLower().Contains(keyword) || 
                        x.NgaySinh.ToString().ToLower().Contains(keyword)
            )
            .ToList();
        UpdateDataDisplay(result);
        _tableSV.UpdateData(_displayData);
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

    private List<DiemSV> listDiemSV = new List<DiemSV>();
    void SetupCotDiem()
    {
        int maHP = _nhomHP.MaHP;
        List<KetQuaDto> listKq = new List<KetQuaDto>();

        foreach (SinhVienDTO sv in _rawData)
        {
            if (_ketQuaController.ExistByMaSVMaHP(sv.MaSinhVien,  maHP))
            {
                KetQuaDto kq = _ketQuaController.GetByMaSVMaHP(sv.MaSinhVien, maHP);
                listKq.Add(kq);
            }
        }
        
        foreach (KetQuaDto item in listKq)
        {
            if (_diemQuaTrinhController.ExistsByMaKQ(item.MaKQ))
            {
                DiemQuaTrinhDto diemQt =  _diemQuaTrinhController.GetByMaKQ(item.MaKQ);
                List<CotDiemDto> listCotDiemSV = _cotDiemController.GetByMaDQT(diemQt.MaDQT);
                DiemSV diemSv = new DiemSV
                {
                    MaSV = item.MaSV,
                    listCotDiem = listCotDiemSV,
                };
                listDiemSV.Add(diemSv);
            }
        }

    }
}