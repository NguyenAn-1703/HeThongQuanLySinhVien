using Mysqlx.Crud;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class PhanQuyenDialog : Form
{
    private TableLayoutPanel _mainLayout;
    private CustomButton _exitButton;
    private TitleButton _btnLuu;
    public event Action Finish;
    
    private string _title;
    private DialogType _dialogType;
    private int _idNhomQuyen;

    private TableLayoutPanel _contentPanel;
    private LabelTextField _txtTenNQ;

    private NhomQuyenController _nhomQuyenController;
    private ChiTietQuyenController _chiTietQuyenController;
    private ChucNangController _chucNangController;

    private List<QuyenChucNangJS> ListDefaultQuyen_ChucNang; //jsom
    private List<string> _listIDChucNang;
    private List<List<CustomCheckBoxNQ>> _listCheckBox;

    private List<List<StatusCB>> _oldListUpdate;
    private List<List<StatusCB>> _newListUpdate;
    public PhanQuyenDialog(string title, DialogType dialogType, int idNhomQuyen = -1)
    {
        _dialogType = dialogType;
        _title = title;
        _idNhomQuyen = idNhomQuyen;
        _nhomQuyenController = NhomQuyenController.GetInstance();
        _chiTietQuyenController = ChiTietQuyenController.getInstance();
        _chucNangController =  ChucNangController.getInstance();
        _listCheckBox = new List<List<CustomCheckBoxNQ>>();
        _listIDChucNang =  new List<string>();
        Init();
    }

    void Init()
    {
        Width = 1200;
        Height = 900;
        BackColor = MyColor.White;
        StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        
        this.SuspendLayout();
        //title, content, button
        _mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = MyColor.LightGray,
        };
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        SetTopBar();
        SetContent();
        SetBottomButton();
        
        this.Controls.Add(_mainLayout);
        this.ResumeLayout();
    }
    
    void SetTopBar()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Margin = new Padding(0),
        };
        
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


        Label topTitle = new Label
        {
            Text = _title,
            Anchor = AnchorStyles.Left,
            BackColor = MyColor.GrayBackGround,
            Dock = DockStyle.Fill,
            Margin = new Padding(0),
        };
        panel.Controls.Add(topTitle);
        
        _exitButton = new CustomButton(25, 25, "exitbutton.svg", MyColor.GrayBackGround, false, false, false, false);
        _exitButton.HoverColor = MyColor.GrayHoverColor;
        _exitButton.SelectColor = MyColor.GraySelectColor;
        _exitButton.Margin = new Padding(0);
        _exitButton.Anchor = AnchorStyles.Right;
        
        _exitButton.MouseDown +=  (sender, args) => this.Close(); 
        panel.Controls.Add(_exitButton);
        
        this._mainLayout.Controls.Add(panel);
    }

    void SetContent()
    {
        
        //tenNQ, header, containercheckbox
        _contentPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(20),
        };
        
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        SetTextBoxTenNQ();
        
        SetHeader();

        SetContainerCheckBox();
        
        _mainLayout.Controls.Add(_contentPanel);
        

    }

    void SetTextBoxTenNQ()
    {
        _txtTenNQ = new LabelTextField("Tên nhóm quyền", TextFieldType.NormalText);
        _txtTenNQ.Dock = DockStyle.None;
        _txtTenNQ._field.BackColor = MyColor.White;
        _txtTenNQ._field.contentTextBox.Width = 500;
        
        _contentPanel.Controls.Add(_txtTenNQ);
        
    }

    void SetHeader()
    {
        TableLayoutPanel header = new TableLayoutPanel
        {
            ColumnCount = 5,
            Dock = DockStyle.Fill,
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            AutoSize = true,
            Padding = new Padding(0, 5, 0, 5),
            Margin = new Padding(0, 20, 0, 0),
            BackColor = MyColor.MainColor
        };

        for (int i = 0; i < header.ColumnCount; i++)
        {
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        }

        header.Controls.Add( GetLblHeader("Danh mục chức năng"));
        header.Controls.Add( GetLblHeader("Xem"));
        header.Controls.Add( GetLblHeader("Thêm"));
        header.Controls.Add( GetLblHeader("Sửa"));
        header.Controls.Add( GetLblHeader("Xóa"));
        
        _contentPanel.Controls.Add(header);
    }

    void SetContainerCheckBox()
    {
        //index trong ô tablelayout
        int c = 1; // bỏ qua cột tiêu ddeef
        int r = 0; 
        
        ListDefaultQuyen_ChucNang = _nhomQuyenController.GetListAllChucNang_HanhDong();
        int rowCount =  ListDefaultQuyen_ChucNang.Count;
        TableLayoutPanel panel = new TableLayoutPanel
        {
            ColumnCount = 5,
            RowCount = rowCount,
            Dock = DockStyle.Fill,
            BackColor = MyColor.White,
            Margin = new Padding(0),
            // CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };
        
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        for (int i = 0; i < rowCount; i++)
        {
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        }

        for (int i = 0; i < rowCount; i++)
        {
            r = i;
            c = 1;
            
            QuyenChucNangJS quyenCN = ListDefaultQuyen_ChucNang[i];
            _listIDChucNang.Add(quyenCN.Name);
            panel.Controls.Add(GetLbl(quyenCN.Name), 0, i);
            
            List<CustomCheckBoxNQ> temp = new List<CustomCheckBoxNQ>();
            
            if (quyenCN.Actions.Contains("Xem"))
            {
                CustomCheckBoxNQ checkBoxNq = new CustomCheckBoxNQ(quyenCN.ID, "Xem");
                temp.Add(checkBoxNq);
                panel.Controls.Add(checkBoxNq, c, r);
                c++;
            }
            else c++;
            
            if (quyenCN.Actions.Contains("Them"))
            {
                CustomCheckBoxNQ checkBoxNq = new CustomCheckBoxNQ(quyenCN.ID, "Them");
                temp.Add(checkBoxNq);
                panel.Controls.Add(checkBoxNq, c, r);
                c++;
            }
            else c++;
            
            if (quyenCN.Actions.Contains("Sua"))
            {
                CustomCheckBoxNQ checkBoxNq = new CustomCheckBoxNQ(quyenCN.ID, "Sua");
                temp.Add(checkBoxNq);
                panel.Controls.Add(checkBoxNq, c, r);
                c++;
            }
            else c++;
            
            if (quyenCN.Actions.Contains("Xoa"))
            {
                CustomCheckBoxNQ checkBoxNq = new CustomCheckBoxNQ(quyenCN.ID, "Xoa");
                temp.Add(checkBoxNq);
                panel.Controls.Add(checkBoxNq, c, r);
                c++;
            }
            else c++;
            
            _listCheckBox.Add(temp);
            
        }
        
        _contentPanel.Controls.Add(panel);

        if (_dialogType == DialogType.Sua)
        {
            SetupUpdate();
        }
        else if (_dialogType == DialogType.ChiTiet)
        {
            SetupDetail();
        }
        else
        {
            SetUpInsert();
        }
        
    }

    void SetUpInsert()
    {
        SetActionForStatusCB();
    }

    void SetupUpdate()
    {
        if (_idNhomQuyen == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }
        
        _txtTenNQ.GetTextField().Text = _nhomQuyenController.GetTenQuyenByID(_idNhomQuyen);

        LoadCheckBoxBySelectedIndex();
        
        //Copy dữ liệu cũ
        
        _oldListUpdate = _listCheckBox
            .Select(row => row
                .Select(chk => new StatusCB
                {
                    CustomCheckBox = chk,
                    Check = chk.Checked,
                })
                .ToList())
            .ToList();
        
        SetActionForStatusCB();
    }
    
    void SetupDetail()
    {
        if (_idNhomQuyen == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }

        _txtTenNQ.GetTextField().Text = _nhomQuyenController.GetTenQuyenByID(_idNhomQuyen);

        LoadCheckBoxBySelectedIndex();

        foreach (List<CustomCheckBoxNQ> list in _listCheckBox)
        {
            foreach (CustomCheckBoxNQ item in list)
            {
                item.Enabled = false;
            }
        }
    }

    void LoadCheckBoxBySelectedIndex()
    {
        //Duyệt từng chức năng để giảm vòng lặp
        for (int i = 0; i < _listIDChucNang.Count; i++)
        {
            int maCN = i + 1;
            List<ChiTietQuyenDto> listCTQSelected = _chiTietQuyenController.GetByMaNQMaCN(_idNhomQuyen,maCN);

            //Xét trong mảng checkbox 1 chức năng có nằm trong mảng chi tiết quyền của quyền được chọn không
            for (int j = 0; j < _listCheckBox[i].Count; j++)
            {
                for (int k = 0; k < listCTQSelected.Count; k++)
                {
                    if (_listCheckBox[i][j].HD.Equals(listCTQSelected[k].HanhDong))
                    {
                        _listCheckBox[i][j].Checked = true;
                    }
                }
            }
        }
    }

    //xem được tick thì mới tick dc thêm sửa xóa
    void SetActionForStatusCB()
    {
        foreach (List<CustomCheckBoxNQ> list in _listCheckBox)
        {
            if (!list[0].Checked)
            {
                for (int i = 1; i < list.Count; i++)
                {
                    list[i].Enabled = false;
                }
            }
            
            list[0].CheckedChanged += (sender, args) =>
            {
                if (list[0].Checked)
                {
                    for (int i = 1; i < list.Count; i++)
                    {
                        list[i].Enabled = true;
                    }
                }
                else
                {
                    for (int i = 1; i < list.Count; i++)
                    {
                        list[i].Checked = false;
                        list[i].Enabled = false;
                    }
                }
            };
        }

    }
    
    Label GetLbl(string i)
    {
        return new Label
        {
            Text = i,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            Anchor = AnchorStyles.None,
        };
    }
    
    Label GetLblHeader(string i)
    {
        return new Label
        {
            Text = i,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            ForeColor = MyColor.White,
            Anchor = AnchorStyles.None,
        };
    }
    
    
    void SetBottomButton()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            Dock = DockStyle.Right,
            ColumnCount = 2,
            AutoSize = true,
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

        
            TitleButton btnHuy = new TitleButton("Hủy");
            btnHuy._mouseDown += () => this.Close();
            
            panel.Controls.Add(btnHuy);
        }
        else
        {
            panel.Controls.Add(new Panel{Height = 0});
        
            TitleButton btnThoat = new TitleButton("Thoát");
            btnThoat._mouseDown += () => this.Close();
            panel.Controls.Add(btnThoat, 2, 0);
        }

        _mainLayout.Controls.Add(panel);
    }

    void Insert()
    {
        if (CommonUse.Validate.IsEmpty(_txtTenNQ.GetTextField().Text))
        {
            MessageBox.Show("Tên nhóm quyền không được để trống", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        NhomQuyenDto nhomquyen = new NhomQuyenDto
        {
            TenNhomQuyen = _txtTenNQ.GetTextField().Text,
        };
        
        if (!_nhomQuyenController.Insert(nhomquyen))
        {
            MessageBox.Show("Lỗi thêm quyền", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
            return;
        }

        int MaNQ = _nhomQuyenController.GetLastAutoIncrement();
        
        //Lấy ds MaNQ, MaCN, HanhDong
        foreach (List<CustomCheckBoxNQ> list in _listCheckBox)
        {
            foreach (CustomCheckBoxNQ item in list)
            {
                int MaCN = _chucNangController.GetByTen(item.ID).MaCN;
                if (item.Checked)
                {
                    string HanhDong = item.HD;
                    ChiTietQuyenDto dto = new ChiTietQuyenDto
                    {
                        MaNQ = MaNQ,
                        MaCN = MaCN,
                        HanhDong = HanhDong,
                    };
                    if (!_chiTietQuyenController.Insert(dto))
                    {
                        MessageBox.Show("Lỗi thêm chi tiết quyền", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
            }
        }
        MessageBox.Show("Thêm nhóm quyền thành công", "Thêm thành công",  MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.Close();
        Finish?.Invoke();
    }
    
    void Update()
    {
        if (CommonUse.Validate.IsEmpty(_txtTenNQ.GetTextField().Text))
        {
            MessageBox.Show("Tên nhóm quyền không được để trống", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        _newListUpdate = _listCheckBox
            .Select(row => row
                .Select(chk => new StatusCB
                {
                    CustomCheckBox = chk,
                    Check = chk.Checked,
                })
                .ToList())
            .ToList();
        
        NhomQuyenDto nhomquyen = new NhomQuyenDto
        {
            MaNQ = _idNhomQuyen,
            TenNhomQuyen = _txtTenNQ.GetTextField().Text,
        };
        
        if (!_nhomQuyenController.Update(nhomquyen))
        {
            MessageBox.Show("Lỗi Sửa quyền", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
            return;
        }
        
        for (int i = 0; i < _listCheckBox.Count; i++)
        {
            for (int j = 0; j < _listCheckBox[i].Count; j++)
            {
                //List cũ có, mới không có -> xóa
                if (_oldListUpdate[i][j].Check && !_newListUpdate[i][j].Check)
                {
                    int maCN = _chucNangController.GetByTen(_oldListUpdate[i][j].CustomCheckBox.ID).MaCN;
                    string hanhDong = _oldListUpdate[i][j].CustomCheckBox.HD;
                    if (!_chiTietQuyenController.HardDelete(_idNhomQuyen, maCN, hanhDong))
                    {
                        MessageBox.Show("Lỗi sửa chi tiết quyền", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
                //List cũ không có, mới có -> thêm
                if (!_oldListUpdate[i][j].Check && _newListUpdate[i][j].Check)
                {
                    Console.WriteLine(!_oldListUpdate[i][j].Check + " " + _oldListUpdate[i][j].Check);
                    
                    int maCN = _chucNangController.GetByTen(_newListUpdate[i][j].CustomCheckBox.ID).MaCN;
                    string hanhDong = _newListUpdate[i][j].CustomCheckBox.HD;

                    ChiTietQuyenDto chiTietQuyenDto = new ChiTietQuyenDto
                    {
                        MaNQ = _idNhomQuyen,
                        MaCN = maCN,
                        HanhDong = hanhDong,
                    };
                    
                    if (!_chiTietQuyenController.Insert(chiTietQuyenDto))
                    {
                        MessageBox.Show("Lỗi sửa chi tiết quyền", "Lỗi",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
            }
        }
        
        MessageBox.Show("Sửa nhóm quyền thành công", "Sửa thành công",  MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.Close();
        Finish?.Invoke();
    }
    
}