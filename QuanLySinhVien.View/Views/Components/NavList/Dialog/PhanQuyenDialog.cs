using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.CommonUse;
using QuanLySinhVien.View.Views.Structs;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class PhanQuyenDialog : Form
{
    private readonly DialogType _dialogType;
    private readonly int _idNhomQuyen;
    private readonly List<List<CustomCheckBoxNQ>> _listCheckBox;
    private readonly List<string> _listIDChucNang;

    private readonly string _title;
    private TitleButton _btnLuu;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;

    private TableLayoutPanel _contentPanel;
    private CustomButton _exitButton;
    private TableLayoutPanel _mainLayout;
    private List<List<StatusCB>> _newListUpdate;

    private NhomQuyenController _nhomQuyenController;

    private List<List<StatusCB>> _oldListUpdate;
    private LabelTextField _txtTenNQ;

    private List<QuyenChucNangJS> ListDefaultQuyen_ChucNang; //jsom

    public PhanQuyenDialog(string title, DialogType dialogType, int idNhomQuyen = -1)
    {
        _dialogType = dialogType;
        _title = title;
        _idNhomQuyen = idNhomQuyen;
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController = ChucNangController.getInstance();
        _listCheckBox = new List<List<CustomCheckBoxNQ>>();
        _listIDChucNang = new List<string>();
        Init();
    }

    public event Action Finish;

    private void Init()
    {
        Width = 1200;
        Height = 900;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;

        SuspendLayout();
        //title, content, button
        _mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = MyColor.LightGray
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTopBar();
        SetContent();
        SetBottomButton();

        Controls.Add(_mainLayout);
        ResumeLayout();
    }

    private void SetTopBar()
    {
        var panel = new TableLayoutPanel
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

    private void SetContent()
    {
        //tenNQ, header, containercheckbox
        _contentPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(20)
        };

        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        SetTextBoxTenNQ();

        SetHeader();

        SetContainerCheckBox();

        _mainLayout.Controls.Add(_contentPanel);
    }

    private void SetTextBoxTenNQ()
    {
        _txtTenNQ = new LabelTextField("Tên nhóm quyền", TextFieldType.NormalText);
        _txtTenNQ.Dock = DockStyle.None;
        _txtTenNQ._field.BackColor = MyColor.White;
        _txtTenNQ._field.contentTextBox.Width = 500;

        _contentPanel.Controls.Add(_txtTenNQ);
    }

    private void SetHeader()
    {
        var header = new TableLayoutPanel
        {
            ColumnCount = 5,
            Dock = DockStyle.Fill,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            AutoSize = true,
            Padding = new Padding(0, 5, 0, 5),
            Margin = new Padding(0, 20, 0, 0),
            BackColor = MyColor.MainColor
        };

        for (var i = 0; i < header.ColumnCount; i++) header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        header.Controls.Add(GetLblHeader("Danh mục chức năng"));
        header.Controls.Add(GetLblHeader("Xem"));
        header.Controls.Add(GetLblHeader("Thêm"));
        header.Controls.Add(GetLblHeader("Sửa"));
        header.Controls.Add(GetLblHeader("Xóa"));

        _contentPanel.Controls.Add(header);
    }

    private void SetContainerCheckBox()
    {
        //index trong ô tablelayout
        var c = 1; // bỏ qua cột tiêu ddeef
        var r = 0;

        ListDefaultQuyen_ChucNang = _nhomQuyenController.GetListAllChucNang_HanhDong();
        var rowCount = ListDefaultQuyen_ChucNang.Count;
        var panel = new TableLayoutPanel
        {
            ColumnCount = 5,
            RowCount = rowCount,
            Dock = DockStyle.Fill,
            BackColor = MyColor.White,
            Margin = new Padding(0)
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        for (var i = 0; i < rowCount; i++) panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        for (var i = 0; i < rowCount; i++)
        {
            r = i;
            c = 1;

            QuyenChucNangJS quyenCN = ListDefaultQuyen_ChucNang[i];
            _listIDChucNang.Add(quyenCN.Name);
            panel.Controls.Add(GetLbl(quyenCN.Name), 0, i);

            var temp = new List<CustomCheckBoxNQ>();

            if (quyenCN.Actions.Contains("Xem"))
            {
                var checkBoxNq = new CustomCheckBoxNQ(quyenCN.ID, "Xem");
                temp.Add(checkBoxNq);
                panel.Controls.Add(checkBoxNq, c, r);
                c++;
            }
            else
            {
                c++;
            }

            if (quyenCN.Actions.Contains("Them"))
            {
                var checkBoxNq = new CustomCheckBoxNQ(quyenCN.ID, "Them");
                temp.Add(checkBoxNq);
                panel.Controls.Add(checkBoxNq, c, r);
                c++;
            }
            else
            {
                c++;
            }

            if (quyenCN.Actions.Contains("Sua"))
            {
                var checkBoxNq = new CustomCheckBoxNQ(quyenCN.ID, "Sua");
                temp.Add(checkBoxNq);
                panel.Controls.Add(checkBoxNq, c, r);
                c++;
            }
            else
            {
                c++;
            }

            if (quyenCN.Actions.Contains("Xoa"))
            {
                var checkBoxNq = new CustomCheckBoxNQ(quyenCN.ID, "Xoa");
                temp.Add(checkBoxNq);
                panel.Controls.Add(checkBoxNq, c, r);
                c++;
            }
            else
            {
                c++;
            }

            _listCheckBox.Add(temp);
        }

        _contentPanel.Controls.Add(panel);

        if (_dialogType == DialogType.Sua)
            SetupUpdate();
        else if (_dialogType == DialogType.ChiTiet)
            SetupDetail();
        else
            SetUpInsert();
    }

    private void SetUpInsert()
    {
        SetActionForStatusCB();
    }

    private void SetupUpdate()
    {
        if (_idNhomQuyen == -1) throw new Exception("Lỗi chưa cài đặt index");

        _txtTenNQ.GetTextField().Text = _nhomQuyenController.GetTenQuyenByID(_idNhomQuyen);

        LoadCheckBoxBySelectedIndex();

        //Copy dữ liệu cũ

        _oldListUpdate = _listCheckBox
            .Select(row => row
                .Select(chk => new StatusCB
                {
                    CustomCheckBox = chk,
                    Check = chk.Checked
                })
                .ToList())
            .ToList();

        SetActionForStatusCB();
    }

    private void SetupDetail()
    {
        if (_idNhomQuyen == -1) throw new Exception("Lỗi chưa cài đặt index");

        _txtTenNQ.GetTextField().Text = _nhomQuyenController.GetTenQuyenByID(_idNhomQuyen);

        LoadCheckBoxBySelectedIndex();

        foreach (var list in _listCheckBox)
        foreach (var item in list)
            item.Enabled = false;
    }

    private void LoadCheckBoxBySelectedIndex()
    {
        //Duyệt từng chức năng để giảm vòng lặp
        for (var i = 0; i < _listIDChucNang.Count; i++)
        {
            var maCN = i + 1;
            List<ChiTietQuyenDto> listCTQSelected = _chiTietQuyenController.GetByMaNQMaCN(_idNhomQuyen, maCN);

            //Xét trong mảng checkbox 1 chức năng có nằm trong mảng chi tiết quyền của quyền được chọn không
            for (var j = 0; j < _listCheckBox[i].Count; j++)
            for (var k = 0; k < listCTQSelected.Count; k++)
                if (_listCheckBox[i][j].HD.Equals(listCTQSelected[k].HanhDong))
                    _listCheckBox[i][j].Checked = true;
        }
    }

    //xem được tick thì mới tick dc thêm sửa xóa
    private void SetActionForStatusCB()
    {
        foreach (var list in _listCheckBox)
        {
            if (!list[0].Checked)
                for (var i = 1; i < list.Count; i++)
                    list[i].Enabled = false;

            list[0].CheckedChanged += (sender, args) =>
            {
                if (list[0].Checked)
                    for (var i = 1; i < list.Count; i++)
                        list[i].Enabled = true;
                else
                    for (var i = 1; i < list.Count; i++)
                    {
                        list[i].Checked = false;
                        list[i].Enabled = false;
                    }
            };
        }
    }

    private Label GetLbl(string i)
    {
        return new Label
        {
            Text = i,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            Anchor = AnchorStyles.None
        };
    }

    private Label GetLblHeader(string i)
    {
        return new Label
        {
            Text = i,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            ForeColor = MyColor.White,
            Anchor = AnchorStyles.None
        };
    }


    private void SetBottomButton()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Right,
            ColumnCount = 2,
            AutoSize = true
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };

        if (_dialogType == DialogType.Them || _dialogType == DialogType.Sua)
        {
            _btnLuu = new TitleButton("Lưu");
            panel.Controls.Add(_btnLuu);
            if (_dialogType == DialogType.Them)
                _btnLuu._mouseDown += () => Insert();
            else
                _btnLuu._mouseDown += () => Update();


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

        _mainLayout.Controls.Add(panel);
    }

    private void Insert()
    {
        if (Shared.Validate.IsEmpty(_txtTenNQ.GetTextField().Text))
        {
            MessageBox.Show("Tên nhóm quyền không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var nhomquyen = new NhomQuyenDto
        {
            TenNhomQuyen = _txtTenNQ.GetTextField().Text
        };

        if (!_nhomQuyenController.Insert(nhomquyen))
        {
            MessageBox.Show("Lỗi thêm quyền", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return;
        }

        int MaNQ = _nhomQuyenController.GetLastAutoIncrement();

        //Lấy ds MaNQ, MaCN, HanhDong
        foreach (var list in _listCheckBox)
        foreach (var item in list)
        {
            int MaCN = _chucNangController.GetByTen(item.ID).MaCN;
            if (item.Checked)
            {
                var HanhDong = item.HD;
                var dto = new ChiTietQuyenDto
                {
                    MaNQ = MaNQ,
                    MaCN = MaCN,
                    HanhDong = HanhDong
                };
                if (!_chiTietQuyenController.Insert(dto))
                {
                    MessageBox.Show("Lỗi thêm chi tiết quyền", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
            }
        }

        MessageBox.Show("Thêm nhóm quyền thành công", "Thêm thành công", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        Close();
        Finish?.Invoke();
    }

    private void Update()
    {
        if (Shared.Validate.IsEmpty(_txtTenNQ.GetTextField().Text))
        {
            MessageBox.Show("Tên nhóm quyền không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        _newListUpdate = _listCheckBox
            .Select(row => row
                .Select(chk => new StatusCB
                {
                    CustomCheckBox = chk,
                    Check = chk.Checked
                })
                .ToList())
            .ToList();

        var nhomquyen = new NhomQuyenDto
        {
            MaNQ = _idNhomQuyen,
            TenNhomQuyen = _txtTenNQ.GetTextField().Text
        };

        if (!_nhomQuyenController.Update(nhomquyen))
        {
            MessageBox.Show("Lỗi Sửa quyền", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return;
        }

        for (var i = 0; i < _listCheckBox.Count; i++)
        for (var j = 0; j < _listCheckBox[i].Count; j++)
        {
            //List cũ có, mới không có -> xóa
            if (_oldListUpdate[i][j].Check && !_newListUpdate[i][j].Check)
            {
                int maCN = _chucNangController.GetByTen(_oldListUpdate[i][j].CustomCheckBox.ID).MaCN;
                var hanhDong = _oldListUpdate[i][j].CustomCheckBox.HD;
                if (!_chiTietQuyenController.HardDelete(_idNhomQuyen, maCN, hanhDong))
                {
                    MessageBox.Show("Lỗi sửa chi tiết quyền", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
            }

            //List cũ không có, mới có -> thêm
            if (!_oldListUpdate[i][j].Check && _newListUpdate[i][j].Check)
            {
                Console.WriteLine(!_oldListUpdate[i][j].Check + " " + _oldListUpdate[i][j].Check);

                int maCN = _chucNangController.GetByTen(_newListUpdate[i][j].CustomCheckBox.ID).MaCN;
                var hanhDong = _newListUpdate[i][j].CustomCheckBox.HD;

                var chiTietQuyenDto = new ChiTietQuyenDto
                {
                    MaNQ = _idNhomQuyen,
                    MaCN = maCN,
                    HanhDong = hanhDong
                };

                if (!_chiTietQuyenController.Insert(chiTietQuyenDto))
                {
                    MessageBox.Show("Lỗi sửa chi tiết quyền", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
            }
        }

        MessageBox.Show("Sửa nhóm quyền thành công", "Sửa thành công", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        Close();
        Finish?.Invoke();
    }
}