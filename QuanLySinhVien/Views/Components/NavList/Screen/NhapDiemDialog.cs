using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.CommonUse.Search.SearchObject;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NhapDiemDialog : RoundTLP
{
    private readonly int _idNHP;
    private readonly NhomHocPhanDto _nhomHP;

    private readonly List<DiemSV> listDiemSV = new();
    private CustomButton _backButton;
    private TitleButton _btnLuu;

    private MyTLP _contentLayout;
    private CotDiemController _cotDiemController;
    private DangKyController _dangKyController;
    private DiemQuaTrinhController _diemQuaTrinhController;
    private List<object> _displayData;
    private CustomButton _exitButton;
    private HocPhanController _hocPhanController;
    private KetQuaController _ketQuaController;
    private List<LabelTextField> _listTextBox;
    private MyTLP _mainLayout;
    private NhomHocPhanController _nhomHocPhanController;

    private List<SinhVienDTO> _rawData;

    private SinhVienController _sinhVienController;
    private TableNhapDiem _tableSV;
    private string _title;
    private string imgPath = "";

    public NhapDiemDialog(string title, int idNHP = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _idNHP = idNHP;
        _title = title;
        _sinhVienController = SinhVienController.GetInstance();
        _dangKyController = DangKyController.GetInstance();
        _hocPhanController = HocPhanController.GetInstance();
        _nhomHocPhanController = NhomHocPhanController.GetInstance();
        _nhomHP = _nhomHocPhanController.GetById(_idNHP);
        _diemQuaTrinhController = DiemQuaTrinhController.GetInstance();
        _ketQuaController = KetQuaController.GetInstance();
        _cotDiemController = CotDiemController.GetInstance();
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

        _tableSV = new TableNhapDiem(headers.ToList(), columnNames.ToList(), _displayData, listDiemSV, _nhomHP.MaHP);
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
            Text = "DS sinh viên học nhóm: " + _idNHP + " , Học phần: " +
                   _hocPhanController.GetHocPhanById(_nhomHP.MaHP).TenHP,
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
        List<DangKyDto> listDK = _dangKyController.GetByMaNHP(_idNHP);
        foreach (var item in listDK) _rawData.Add(_sinhVienController.GetById(item.MaSV));

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
    }

    private void UpdateDiem()
    {
        var rs = MessageBox.Show("Xác nhận lưu thay đổi ?", "Xác nhận", MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        if (rs == DialogResult.No) return;
        _tableSV.UpdateDiem();
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
        var maHP = _nhomHP.MaHP;
        var listKq = new List<KetQuaDto>();

        foreach (var sv in _rawData)
            if (_ketQuaController.ExistByMaSVMaHP(sv.MaSinhVien, maHP))
            {
                KetQuaDto kq = _ketQuaController.GetByMaSVMaHP(sv.MaSinhVien, maHP);
                listKq.Add(kq);
            }

        foreach (var item in listKq)
            if (_diemQuaTrinhController.ExistsByMaKQ(item.MaKQ))
            {
                DiemQuaTrinhDto diemQt = _diemQuaTrinhController.GetByMaKQ(item.MaKQ);
                List<CotDiemDto> listCotDiemSV = _cotDiemController.GetByMaDQT(diemQt.MaDQT);
                DiemSV diemSv = new DiemSV
                {
                    MaSV = item.MaSV,
                    listCotDiem = listCotDiemSV
                };
                listDiemSV.Add(diemSv);
            }
    }
}