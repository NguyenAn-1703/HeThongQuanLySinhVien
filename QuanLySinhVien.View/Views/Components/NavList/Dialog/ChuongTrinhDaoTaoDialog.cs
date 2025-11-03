using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Components.CommonUse.Search;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class ChuongTrinhDaoTaoDialog : Form
{
    private readonly string[] _arrLHDT = new[] { "Chính quy", "Văn bằng 2", "Vừa học vừa làm" };
    private readonly string[] _arrTDDT = new[] { "Cử nhân", "Thạc sĩ", "Tiến sĩ" };
    private readonly DialogType _dialogType;
    private readonly List<string> _header;

    private readonly string[] _headerArray = new[] { "Mã học phần", "Tên học phần", "Hành động" };

    private readonly string[] _headerDetailArray = new[] { "Mã học phần", "Tên học phần" };
    private readonly int _idChuongTrinhDaoTao;
    private readonly List<InputFormItem> _listIFI;
    private readonly List<string> _listLHDT;
    private readonly List<string> _listTDDT;
    private readonly List<LabelTextField> _listTextBox;

    private readonly string _title;
    private TitleButton _btnLuu;
    private ChuKyDaoTaoController _chuKyDaoTaoController;
    private ChuongTrinhDaoTao_HocPhanController _chuongTrinhDaoTao_HocPhanController;
    private ChuongTrinhDaoTaoController _chuongTrinhDaoTaoController;

    private MyTLP _contentPanel;

    private CustomButton _exitButton;
    private List<string> _headerDetail;

    private HocPhanController _hocPhanController;
    private List<object> _leftDisplayData;
    private List<HocPhanDto> _leftRawData;

    private CtdtSearchBar _leftSearchBar;


    private CTDTTable _leftTable;

    private MyTLP _mainLayout;
    private List<HocPhanDto> _newListUpdate;

    private NganhController _nganhController;

    private List<HocPhanDto> _oldListUpdate;
    private List<object> _rightDisplayData;
    private List<HocPhanDto> _rightRawData;
    private CtdtSearchBar _rightSearchBar;

    private CTDTTable _rightTable;


    public ChuongTrinhDaoTaoDialog(string title, DialogType dialogType, List<InputFormItem> listIFI,
        ChuongTrinhDaoTaoController chuongTrinhDaoTaoController, int idChuongTrinhDaoTao = -1)
    {
        _listTextBox = new List<LabelTextField>();
        _listIFI = listIFI;
        _chuongTrinhDaoTaoController = chuongTrinhDaoTaoController;
        _idChuongTrinhDaoTao = idChuongTrinhDaoTao;
        _title = title;
        _hocPhanController = HocPhanController.GetInstance();

        _leftRawData = new List<HocPhanDto>();
        _leftDisplayData = new List<object>();
        _rightRawData = new List<HocPhanDto>();
        _rightDisplayData = new List<object>();
        _dialogType = dialogType;

        _header = _headerArray.ToList();
        _headerDetail = _headerDetailArray.ToList();
        _listLHDT = _arrLHDT.ToList();
        _listTDDT = _arrTDDT.ToList();

        _nganhController = NganhController.GetInstance();
        _chuKyDaoTaoController = ChuKyDaoTaoController.GetInstance();

        _chuongTrinhDaoTao_HocPhanController = ChuongTrinhDaoTao_HocPhanController.GetInstance();
        Init();
    }

    public event Action Finish;

    private void Init()
    {
        Width = 1200;
        Height = 750;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;

        _mainLayout = new MyTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            BorderStyle = BorderStyle.FixedSingle
            // CellBorderStyle = MyTLPCellBorderStyle.Single
        };

        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTopBar();
        SetTitleBar();
        SetContent();
        SetBottom();

        Controls.Add(_mainLayout);

        SetAction();


        if (_dialogType == DialogType.Them)
            SetupInsert();
        else if (_dialogType == DialogType.Sua)
            SetupUpdate();
        else
            SetupDetail();
    }

    private void SetTopBar()
    {
        var panel = new MyTLP
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0)
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


        var topTitle = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.Left,
            BackColor = MyColor.GrayBackGround,
            Dock = DockStyle.Fill,
            Margin = new Padding(0)
        };
        panel.Controls.Add(topTitle);

        _exitButton = new CustomButton(25, 25, "exitbutton.svg", MyColor.GrayBackGround, false, false, false, false);
        _exitButton.HoverColor = MyColor.GrayHoverColor;
        _exitButton.SelectColor = MyColor.GraySelectColor;
        _exitButton.Margin = new Padding(0);
        _exitButton.Anchor = AnchorStyles.Right;

        _exitButton.MouseDown += (sender, args) => Close();
        panel.Controls.Add(_exitButton);

        _mainLayout.Controls.Add(panel);
    }

    private void SetTitleBar()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            BackColor = MyColor.MainColor,
            Margin = new Padding(0),
            Padding = new Padding(0, 10, 0, 10)
        };

        var title = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.None,
            AutoSize = true,
            ForeColor = MyColor.White,
            Font = GetFont.GetFont.GetMainFont(16, FontType.Black)
        };

        panel.Controls.Add(title);
        _mainLayout.Controls.Add(panel);
    }

    private void SetContent()
    {
        _contentPanel = new MyTLP
        {
            ColumnCount = 2,
            RowCount = 2,
            Dock = DockStyle.Fill,
            Margin = new Padding(0)
        };
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        SetTextBoxContainer();

        SetTable();

        _mainLayout.Controls.Add(_contentPanel);
    }

    private void SetTextBoxContainer()
    {
        var panel = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0),
            BackColor = MyColor.LightGray,
            ColumnCount = 2,
            RowCount = 2
            // Padding = new Padding(100, 10, 100, 10),
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));


        for (var i = 0; i < _listIFI.Count; i++)
        {
            var textField = new LabelTextField(_listIFI[i].content, _listIFI[i].type)
            {
                Margin = new Padding(100, 5, 100, 5)
            };
            _listTextBox.Add(textField);
            panel.Controls.Add(_listTextBox[i]);
        }

        _contentPanel.Controls.Add(panel);
        _contentPanel.SetColumnSpan(panel, 2);
    }

    private Label GetLabel(string text)
    {
        var lbl = new Label
        {
            Text = text,
            Font = GetFont.GetFont.GetMainFont(11, FontType.SemiBold),
            AutoSize = true
        };
        return lbl;
    }

    private void SetTable()
    {
        //mahp, tenhp, hành động
        //mahp, tenhp, hành động

        var leftPnl = new RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(15, 0, 15, 15),
            Border = true,
            BorderColor = MyColor.GraySelectColor,
            Margin = new Padding(15)
        };
        var rightPnl = new RoundTLP
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(15, 0, 15, 15),
            Border = true,
            BorderColor = MyColor.GraySelectColor,
            Margin = new Padding(15)
        };
        leftPnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        leftPnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        leftPnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        rightPnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        rightPnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        rightPnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var lblhp = GetLabel("Học phần: ");
        var lblhpct = GetLabel("Học phần thuộc chương trình đào tạo: ");
        lblhp.Margin = new Padding(0, 20, 0, 0);
        lblhpct.Margin = new Padding(0, 20, 0, 0);
        lblhp.Anchor = AnchorStyles.None;
        lblhpct.Anchor = AnchorStyles.None;

        _leftSearchBar = new CtdtSearchBar { Dock = DockStyle.Right };
        _rightSearchBar = new CtdtSearchBar { Dock = DockStyle.Right };


        _leftRawData = _hocPhanController.GetAll();

        SetLeftDisplayData();
        var leftColumnNameArr = new[] { "MaHP", "TenHP", "ActionPlus" };
        _leftTable = new CTDTTable(_header, leftColumnNameArr.ToList(), _leftDisplayData, TableCTDTType.Plus);
        // _contentPanel.Controls.Add(_leftTable);

        SetRightDisplayData();
        var rightColumnNameArr = new[] { "MaHP", "TenHP", "ActionMinus" };
        _rightTable = new CTDTTable(_header, rightColumnNameArr.ToList(), _rightDisplayData, TableCTDTType.Minus);


        leftPnl.Controls.Add(lblhp);
        leftPnl.Controls.Add(_leftSearchBar);
        leftPnl.Controls.Add(_leftTable);

        rightPnl.Controls.Add(lblhpct);
        rightPnl.Controls.Add(_rightSearchBar);
        rightPnl.Controls.Add(_rightTable);

        _contentPanel.Controls.Add(leftPnl);
        _contentPanel.Controls.Add(rightPnl);
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

        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            panel.Controls.Add(new Panel { Height = 0 });

            _btnLuu = new TitleButton("Lưu");
            panel.Controls.Add(_btnLuu);

            var btnHuy = new TitleButton("Hủy");
            btnHuy._mouseDown += () => Close();

            panel.Controls.Add(btnHuy);
        }
        else
        {
            panel.Controls.Add(new Panel { Height = 0 });

            var btnThoat = new TitleButton("Thoát");
            btnThoat._mouseDown += () => Close();
            panel.Controls.Add(btnThoat, 2, 0);
        }


        _mainLayout.Controls.Add(panel, 0, 3);
    }

    private void SetAction()
    {
        _leftSearchBar.KeyDown += s => OnLeftSearch(s);
        _rightSearchBar.KeyDown += s => OnRightSearch(s);

        _leftTable.BtnClick += id => InsertHP(id);
        _rightTable.BtnClick += id => DeleteHP(id);

        _leftTable.OnDetail += i =>
        {
            new HocPhanDialog(DialogType.ChiTiet, _hocPhanController.GetHocPhanById(i), HocPhanDao.GetInstance())
                .ShowDialog();
        };

        _rightTable.OnDetail += i =>
        {
            new HocPhanDialog(DialogType.ChiTiet, _hocPhanController.GetHocPhanById(i), HocPhanDao.GetInstance())
                .ShowDialog();
        };

        if (_dialogType == DialogType.Them) _btnLuu._mouseDown += () => Insert();

        if (_dialogType == DialogType.Sua) _btnLuu._mouseDown += () => Update();
    }

    private void SetLeftDisplayData()
    {
        _leftDisplayData = ConvertObject.ConvertToDisplay(_leftRawData, x => new
        {
            x.MaHP, x.TenHP
        });
    }

    private void SetupInsert()
    {
        SetupCombobox();
    }

    private void SetupUpdate()
    {
        SetupCombobox();

        ChuongTrinhDaoTaoDto chuongTrinh = _chuongTrinhDaoTaoController.GetById(_idChuongTrinhDaoTao);

        NganhDto nganh = _nganhController.GetNganhById(chuongTrinh.MaNganh);
        ChuKyDaoTaoDto chuKy = _chuKyDaoTaoController.GetById(chuongTrinh.MaCKDT);

        _listTextBox[0].SetComboboxSelection(nganh.TenNganh);
        _listTextBox[1].SetComboboxSelection(chuKy.NamBatDau + " - " + chuKy.NamKetThuc);
        _listTextBox[2].SetComboboxSelection(chuongTrinh.LoaiHinhDT);
        _listTextBox[3].SetComboboxSelection(chuongTrinh.TrinhDo);


        List<ChuongTrinhDaoTao_HocPhanDto> listCTDT_HP =
            _chuongTrinhDaoTao_HocPhanController.GetByMaCTDT(_idChuongTrinhDaoTao);

        var listHocPhan = new List<HocPhanDto>();
        foreach (var item in listCTDT_HP) listHocPhan.Add(_hocPhanController.GetHocPhanById(item.MaHP));
        _rightRawData = listHocPhan;
        _leftRawData.RemoveAll(a => listHocPhan.Any(b => a.MaHP == b.MaHP));
        UpdateTableWNewRawData();

        //chỉ cần lấy mã để so sánh
        _oldListUpdate = _rightRawData
            .Select(x => new HocPhanDto
            {
                MaHP = x.MaHP
            })
            .ToList();
        ;
    }

    private void SetupDetail()
    {
        SetupCombobox();
        ChuongTrinhDaoTaoDto chuongTrinh = _chuongTrinhDaoTaoController.GetById(_idChuongTrinhDaoTao);
        NganhDto nganh = _nganhController.GetNganhById(chuongTrinh.MaNganh);
        ChuKyDaoTaoDto chuKy = _chuKyDaoTaoController.GetById(chuongTrinh.MaCKDT);

        _listTextBox[0].SetComboboxSelection(nganh.TenNganh);
        _listTextBox[1].SetComboboxSelection(chuKy.NamBatDau + " - " + chuKy.NamKetThuc);
        _listTextBox[2].SetComboboxSelection(chuongTrinh.LoaiHinhDT);
        _listTextBox[3].SetComboboxSelection(chuongTrinh.TrinhDo);

        List<ChuongTrinhDaoTao_HocPhanDto> listCTDT_HP =
            _chuongTrinhDaoTao_HocPhanController.GetByMaCTDT(_idChuongTrinhDaoTao);

        var listHocPhan = new List<HocPhanDto>();
        foreach (var item in listCTDT_HP) listHocPhan.Add(_hocPhanController.GetHocPhanById(item.MaHP));
        _rightRawData = listHocPhan;
        _leftRawData.RemoveAll(a => listHocPhan.Any(b => a.MaHP == b.MaHP));


        UpdateTableWNewRawData();

        _leftTable.EnableActionColumn();
        _rightTable.EnableActionColumn();
        _listTextBox[0].GetComboboxField().Enabled = false;
        _listTextBox[1].GetComboboxField().Enabled = false;
        _listTextBox[2].GetComboboxField().Enabled = false;
        _listTextBox[3].GetComboboxField().Enabled = false;
    }

    private void SetupCombobox()
    {
        List<NganhDto> listNganh = _nganhController.GetAll();
        var listTenNganh = listNganh.Select(x => x.TenNganh).ToList();
        _listTextBox[0].SetComboboxList(listTenNganh);
        _listTextBox[0].SetComboboxSelection(listTenNganh[0]);

        List<ChuKyDaoTaoDto> listChuKy = _chuKyDaoTaoController.GetAll();
        var listTenChuKy = listChuKy.Select(x => x.NamBatDau + " - " + x.NamKetThuc).ToList();
        _listTextBox[1].SetComboboxList(listTenChuKy);
        _listTextBox[1].SetComboboxSelection(listTenChuKy[0]);

        _listTextBox[2].SetComboboxList(_listLHDT);
        _listTextBox[2].SetComboboxSelection(_listLHDT[0]);

        _listTextBox[3].SetComboboxList(_listTDDT);
        _listTextBox[3].SetComboboxSelection(_listTDDT[0]);
    }

    private void SetRightDisplayData()
    {
        _rightDisplayData = ConvertObject.ConvertToDisplay(_rightRawData, x => new
        {
            x.MaHP, x.TenHP
        });
    }

    private void InsertHP(int maHP)
    {
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(maHP);
        _rightRawData.Add(hocPhan);
        _leftRawData.RemoveAll(a => a.MaHP == maHP);
        UpdateTableWNewRawData();
    }

    private void DeleteHP(int maHP)
    {
        HocPhanDto hocPhan = _hocPhanController.GetHocPhanById(maHP);
        _leftRawData.Add(hocPhan);
        _rightRawData.RemoveAll(a => a.MaHP == maHP);
        UpdateTableWNewRawData();
    }

    private void UpdateTableWNewRawData()
    {
        _leftRawData = _leftRawData.OrderBy(x => x.MaHP).ToList();
        _rightRawData = _rightRawData.OrderBy(x => x.MaHP).ToList();
        SetRightDisplayData();
        SetLeftDisplayData();

        _leftTable._dataGridView.DataSource = _leftDisplayData;
        _rightTable._dataGridView.DataSource = _rightDisplayData;
    }

    private void UpdateLeftDisplayData(List<HocPhanDto> dtos)
    {
        _leftDisplayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaHP, x.TenHP
        });
    }

    private void UpdateRightDisplayData(List<HocPhanDto> dtos)
    {
        _rightDisplayData = ConvertObject.ConvertToDisplay(dtos, x => new
        {
            x.MaHP, x.TenHP
        });
    }


    private void OnLeftSearch(string input)
    {
        var keyword = input.ToLower().Trim();
        var result = _leftRawData
            .Where(x => x.MaHP.ToString().ToLower().Contains(keyword) ||
                        x.TenHP.ToString().ToLower().Contains(keyword)
            )
            .ToList();

        UpdateLeftDisplayData(result);
        _leftTable._dataGridView.DataSource = _leftDisplayData;
    }

    private void OnRightSearch(string input)
    {
        var keyword = input.ToLower().Trim();
        var result = _rightRawData
            .Where(x => x.MaHP.ToString().ToLower().Contains(keyword) ||
                        x.TenHP.ToString().ToLower().Contains(keyword)
            )
            .ToList();

        UpdateRightDisplayData(result);
        _rightTable._dataGridView.DataSource = _rightDisplayData;
    }


    private void Insert()
    {
        var startendYear = _listTextBox[1].GetSelectionCombobox();
        ChuKyDaoTaoDto chuky = _chuKyDaoTaoController.GetByStartYearEndYear(startendYear);

        var tenNganh = _listTextBox[0].GetSelectionCombobox();
        NganhDto nganh = _nganhController.GetByTen(tenNganh);

        var loaiHinhDaoTao = _listTextBox[2].GetSelectionCombobox();
        var trinhDoDaoTao = _listTextBox[2].GetSelectionCombobox();

        if (!_chuongTrinhDaoTaoController.ValidateNganhChuKy(nganh.MaNganh, chuky.MaCKDT))
        {
            MessageBox.Show("Đã tồn tại chương trình đào tạo của ngành này thuộc chu kỳ này!", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var listSelected = _rightRawData;
        if (listSelected.Count == 0)
        {
            MessageBox.Show("Chưa có học phần nào được chọn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_chuongTrinhDaoTaoController.Insert(new ChuongTrinhDaoTaoDto
            {
                MaCKDT = chuky.MaCKDT,
                MaNganh = nganh.MaNganh,
                LoaiHinhDT = loaiHinhDaoTao,
                TrinhDo = trinhDoDaoTao
            }))
        {
            MessageBox.Show("Lỗi thêm chương trình đào tạo!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int MaCTDT = _chuongTrinhDaoTaoController.GetLastAutoIncrement();

        foreach (var dto in listSelected)
            if (
                !_chuongTrinhDaoTao_HocPhanController.Insert(new ChuongTrinhDaoTao_HocPhanDto
                {
                    MaHP = dto.MaHP,
                    MaCTDT = MaCTDT
                }))
            {
                MessageBox.Show("Lỗi thêm chi tiết chương trình đào tạo!", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

        MessageBox.Show("Thêm chương trình đào tạo thành công!", "Thành công", MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        Close();
        Finish?.Invoke();
    }

    private void Update()
    {
        _newListUpdate = _rightRawData;

        var startendYear = _listTextBox[1].GetSelectionCombobox();
        ChuKyDaoTaoDto chuky = _chuKyDaoTaoController.GetByStartYearEndYear(startendYear);

        var tenNganh = _listTextBox[0].GetSelectionCombobox();
        NganhDto nganh = _nganhController.GetByTen(tenNganh);

        var loaiHinhDaoTao = _listTextBox[2].GetSelectionCombobox();
        var trinhDoDaoTao = _listTextBox[2].GetSelectionCombobox();

        if (!_chuongTrinhDaoTaoController.ValidateNganhChuKy(_idChuongTrinhDaoTao, nganh.MaNganh, chuky.MaCKDT))
        {
            MessageBox.Show("Đã tồn tại chương trình đào tạo của ngành này thuộc chu kỳ này!", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var listSelected = _rightRawData;
        if (listSelected.Count == 0)
        {
            MessageBox.Show("Chưa có học phần nào được chọn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_chuongTrinhDaoTaoController.Update(new ChuongTrinhDaoTaoDto
            {
                MaCTDT = _idChuongTrinhDaoTao,
                MaCKDT = chuky.MaCKDT,
                MaNganh = nganh.MaNganh,
                LoaiHinhDT = loaiHinhDaoTao,
                TrinhDo = trinhDoDaoTao
            }))
        {
            MessageBox.Show("Lỗi thêm chương trình đào tạo!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }


        //List cũ có, mới không có -> xóa
        foreach (var dto in _oldListUpdate)
            if (!_newListUpdate.Any(x => x.MaHP == dto.MaHP))
                _chuongTrinhDaoTao_HocPhanController.HardDelete(_idChuongTrinhDaoTao, dto.MaHP);

        //List cũ không có, mới có -> thêm
        foreach (var dto in _newListUpdate)
            if (!_oldListUpdate.Any(x => x.MaHP == dto.MaHP))
                _chuongTrinhDaoTao_HocPhanController.Insert(new ChuongTrinhDaoTao_HocPhanDto
                {
                    MaCTDT = _idChuongTrinhDaoTao,
                    MaHP = dto.MaHP
                });

        MessageBox.Show("Sửa chương trình đào tạo thành công!", "Thành công", MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        Close();
        Finish?.Invoke();
    }
}