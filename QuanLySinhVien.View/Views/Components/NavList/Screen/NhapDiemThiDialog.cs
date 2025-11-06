using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.DTO.SearchObject;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class NhapDiemThiDialog : RoundTLP
{
    private readonly int _idCaThi;
    private CaThiDto _caThi;

    private readonly List<DiemThiSV> listDiemThiSV = new();
    private CustomButton _backButton;
    private TitleButton _btnLuu;
    private CaThiController _caThiController;
    private CaThi_SinhVienController _caThiSinhVienController;

    private MyTLP _contentLayout;
    private CotDiemController _cotDiemController;
    private DiemQuaTrinhController _diemQuaTrinhController;
    private List<object> _displayData;
    private CustomButton _exitButton;
    private HocPhanDto _hocPhan;
    private HocPhanController _hocPhanController;
    private KetQuaController _ketQuaController;
    private List<LabelTextField> _listTextBox;
    private MyTLP _mainLayout;
    private NhomHocPhanController _nhomHocPhanController;

    private List<SinhVienDTO> _rawData;

    private SinhVienController _sinhVienController;
    private TableNhapDiemThi _tableSV;
    private string _title;
    private string imgPath = "";

    public NhapDiemThiDialog(string title, int idCaThi = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _idCaThi = idCaThi;
        _title = title;
        _sinhVienController = SinhVienController.GetInstance();
        _caThiSinhVienController = CaThi_SinhVienController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _diemQuaTrinhController = DiemQuaTrinhController.GetInstance();
        _ketQuaController = KetQuaController.GetInstance();
        _cotDiemController = CotDiemController.GetInstance();
        _caThiController = CaThiController.GetInstance();
        _caThi = _caThiController.GetById(_idCaThi);
        _hocPhan = _hocPhanController.GetHocPhanById(_caThi.MaHP);
        Init();
    }

    public event Action Finish;
    public event Action Back;

    private void Init()
    {
        Dock = DockStyle.Fill;
        BackColor = MyColor.White;
        Margin = new Padding(0);
        Border = true;


        _mainLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 2
        };

        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        _hocPhan = _hocPhanController.GetHocPhanById(_caThiController.GetById(_idCaThi).MaHP);

        SetContent();
        SetBottom();
        Controls.Add(_mainLayout);
        SetAction();
        SetupDetail();
    }

    private void SetContent()
    {
        _contentLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1
        };

        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTitleBar();

        var headers = new[] { "Mã sinh viên", "Tên sinh viên", "Giới tính", "Ngày sinh" };
        var columnNames = new[] { "MaSV", "TenSV", "GioiTinh", "NgaySinh" };
        SetDisplayData();
        SetupCotDiem();

        _tableSV = new TableNhapDiemThi(headers.ToList(), columnNames.ToList(), _displayData, listDiemThiSV,
            _hocPhan.MaHP);
        _contentLayout.Controls.Add(_tableSV);

        _btnLuu = new TitleButton("Lưu thay đổi") { Dock = DockStyle.Right };
        _contentLayout.Controls.Add(_btnLuu);

        _mainLayout.Controls.Add(_contentLayout);
    }

    private void SetTitleBar()
    {
        var panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _backButton = new CustomButton(20, 20, "back.svg", MyColor.GrayBackGround)
            { Pad = 5, Anchor = AnchorStyles.None };
        _backButton.HoverColor = MyColor.GrayHoverColor;
        _backButton.SelectColor = MyColor.GraySelectColor;

        var lblTitle = new Label
        {
            Text = "DS sinh viên thi ca thi mã : " + _idCaThi + " , Học phần: " +
                   _hocPhanController.GetHocPhanById(_hocPhan.MaHP).TenHP,
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(12, FontType.SemiBold),
            Margin = new Padding(7, 3, 3, 3)
        };
        panel.Controls.Add(_backButton);
        panel.Controls.Add(lblTitle);

        _contentLayout.Controls.Add(panel);
    }

    private void SetDisplayData()
    {
        _rawData = new List<SinhVienDTO>();

        // Sinh Vien trong ca thi
        List<CaThi_SinhVienDto> listCTSV = _caThiSinhVienController.GetByMaCT(_idCaThi);
        foreach (var item in listCTSV) _rawData.Add(_sinhVienController.GetById(item.MaSV));

        _displayData = ConvertObject.ConvertToDisplay(ConvertDtoToDisplay(_rawData), x => new
            {
                x.MaSV,
                x.TenSV,
                x.GioiTinh,
                x.NgaySinh
            }
        );
    }

    private void UpdateDataDisplay(List<SVNhapDiemDisplay> input)
    {
        _displayData = ConvertObject.ConvertToDisplay(input, x => new
        {
            x.MaSV,
            x.TenSV,
            x.GioiTinh,
            x.NgaySinh
        });
    }

    private void SetBottom()
    {
        //Thêm có Đặt lại, Lưu, Hủy
        var panel = new MyTLP
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            ColumnCount = 3
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));


        panel.Controls.Add(new Panel { Height = 0 });

        _mainLayout.Controls.Add(panel, 0, 3);
    }


    private void SetupDetail()
    {
    }

    private void SetAction()
    {
        _backButton._mouseDown += () => Back?.Invoke();
        _btnLuu._mouseDown += () => UpdateDiem();
        _tableSV.OnDetail += (i) => DetailSV(i);
    }

    void DetailSV(int maSV)
    {
        DiemSVDialog dialog = new DiemSVDialog(maSV, _hocPhan.MaHP);
        dialog.ShowDialog();
    }

    private void UpdateDiem()
    {
        var rs = MessageBox.Show("Xác nhận lưu thay đổi ?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        if (rs == DialogResult.No) return;
        _tableSV.UpdateDiem();
        
        _ketQuaController.UpdateDiemHeSoSV(_rawData, _hocPhan);

    }

    private List<SVNhapDiemDisplay> ConvertDtoToDisplay(List<SinhVienDTO> input)
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

    private void SetupCotDiem()
    {
        var maHP = _hocPhan.MaHP;
        foreach (var sv in _rawData)
        {
            KetQuaDto kq = _ketQuaController.GetByMaSVMaHP(sv.MaSinhVien, maHP);
            DiemThiSV diemThi = new DiemThiSV
            {
                MaSV = sv.MaSinhVien,
                diemThi = kq.DiemThi
            };
            listDiemThiSV.Add(diemThi);
        }
    }
}