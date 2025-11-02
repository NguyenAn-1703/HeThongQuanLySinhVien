using System.ComponentModel;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class TableNhapDiem : MyTLP
{
    public CustomDataGridView _dataGridView;
    List<string> _headerContent;
    protected MyFLP _header;
    private List<object> _cellDatas;
    private List<string> _columnNames; //để truy suất
    private BindingList<object> _displayCellData;
    private bool _action;
    private bool _edit;
    private bool _delete;
    private Form _topForm;

    private CustomButton _editBtn;
    private CustomButton _deleteBtn;

    public event Action<int> OnEdit;
    public event Action<int> OnDelete;
    public event Action<int> OnDetail;

    private List<DiemSV> _listDiemSV;
    private int _maHp;

    private KetQuaController _ketQuaController;
    private CotDiemController _cotDiemController;
    private DiemQuaTrinhController _diemQuaTrinhController;

    public TableNhapDiem(List<string> headerContent, List<string> columnNames, List<object> cells,
        List<DiemSV> listDiemSV, int mahp, bool action = false,
        bool edit = false, bool delete = false)
    {
        _headerContent = headerContent;
        _header = new MyFLP();
        _cellDatas = cells;
        _action = action;
        _edit = edit;
        _delete = delete;
        _columnNames = columnNames;
        _listDiemSV = listDiemSV;
        _maHp = mahp;
        _diemQuaTrinhController = DiemQuaTrinhController.GetInstance();
        _cotDiemController = CotDiemController.GetInstance();
        _ketQuaController = KetQuaController.GetInstance();
        Init();
    }

    void Init()
    {
        Configuration();
        SetHeader();
        SetContent();
        SetEventListen();
        SetCotDiem();
    }

    void Configuration()
    {
        this.Dock = DockStyle.Fill;
        this.RowCount = 2;
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _dataGridView = new CustomDataGridView(_action, _edit, _delete);
    }

    void SetHeader()
    {
        _header.Dock = DockStyle.Top;
        _header.AutoSize = true;
        _header.FlowDirection = FlowDirection.LeftToRight;
        _header.BackColor = MyColor.MainColor;
        _header.Margin = new Padding(0, 0, 0, 0);
        _header.Padding = new Padding(0, 0, 0, 0);

        foreach (String i in _headerContent)
        {
            this._header.Controls.Add(GetLabel(i));
        }

        if (_action)
        {
            _header.Controls.Add(GetLabel("Hành động"));
        }

        this.Controls.Add(_header);
    }

    void SetContent()
    {
        for (int i = 0; i < _headerContent.Count; i++)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = _columnNames[i],
                HeaderText = _headerContent[i],
                DataPropertyName = _columnNames[i],
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            _dataGridView.Columns.Add(column);
        }

        if (_action)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = "Action",
                HeaderText = "Hành động",
                DataPropertyName = "Action",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            _dataGridView.Columns.Add(column);
        }

        _displayCellData = new BindingList<object>(_cellDatas);
        _dataGridView.DataSource = _displayCellData;

        _dataGridView.Dock = DockStyle.Fill;
        _dataGridView.Font = GetFont.GetFont.GetMainFont(9, FontType.Regular);

        this.Controls.Add(_dataGridView);
    }

    Label GetLabel(String text)
    {
        Label lbl = new Label
        {
            // Dock = DockStyle.Top,
            Anchor = AnchorStyles.None,
            Height = 30,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = text,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            ForeColor = MyColor.White,
            Margin = new Padding(0, 3, 0, 3),
        };
        return lbl;
    }

    void SetEventListen()
    {
        this.Resize += (sender, args) => OnResize();
        this._dataGridView.CellDoubleClick += (sender, args) => OnDoubleClickRow(args);

        this._dataGridView.BtnHoverEdit += (rec, index) => OnHoverEditBtn(rec, index);
        this._dataGridView.BtnHoverDelete += (rec, index) => OnHoverDeleteBtn(rec, index);

        this._dataGridView.BtnEditLeave += () => OnLeaveEditBtn();
        this._dataGridView.BtnDeleteLeave += () => OnLeaveDeleteBtn();
    }

    void OnDoubleClickRow(DataGridViewCellEventArgs e)
    {
        int index = e.RowIndex;
        detail(index);
    }

    void OnHoverEditBtn(Point rec, int index)
    {
        int rowIndex = index;
        //Vẽ vào form, không phụ thuộc layout
        _topForm = this.FindForm();
        _editBtn = new CustomButton(20, 20, "fix.svg", MyColor.MainColor)
        {
            Cursor = Cursors.Hand,
        };

        _topForm.Controls.Add(_editBtn);
        _editBtn.BringToFront();

        Point myPoint = _topForm.PointToClient(rec);
        _editBtn.Location = myPoint;

        Point cursorPos = Cursor.Position;
        Point cursorOnForm = _topForm.PointToClient(cursorPos);

        // Nếu chuột đang nằm trong vùng nút thì hiển thị, nếu không thì ẩn
        if (_editBtn.Bounds.Contains(cursorOnForm))
        {
            _editBtn.Visible = true;
        }
        else
        {
            _editBtn.Visible = false;
        }

        _editBtn.MouseLeave += (sender, args) => _editBtn.Visible = false;


        _editBtn.MouseDown += (sender, args) => edit(index);
    }

    void OnHoverDeleteBtn(Point rec, int index)
    {
        int rowIndex = index;
        //Vẽ vào form, không phụ thuộc layout
        _topForm = this.FindForm();
        _deleteBtn = new CustomButton(20, 20, "trashbin.svg", MyColor.RedHover)
        {
            Cursor = Cursors.Hand,
        };

        _topForm.Controls.Add(_deleteBtn);
        _deleteBtn.BringToFront();

        Point myPoint = _topForm.PointToClient(rec);
        _deleteBtn.Location = myPoint;

        Point cursorPos = Cursor.Position;
        Point cursorOnForm = _topForm.PointToClient(cursorPos);

        // Nếu chuột đang nằm trong vùng nút thì hiển thị, nếu không thì ẩn
        if (_deleteBtn.Bounds.Contains(cursorOnForm))
        {
            _deleteBtn.Visible = true;
        }
        else
        {
            _deleteBtn.Visible = false;
        }

        _deleteBtn.MouseLeave += (sender, args) => _deleteBtn.Visible = false;


        _deleteBtn.MouseDown += (sender, args) => delete(index);
    }

    void OnLeaveEditBtn()
    {
        _editBtn.Dispose();
    }

    void OnLeaveDeleteBtn()
    {
        _deleteBtn.Dispose();
    }

    void delete(int index)
    {
        int Id = (int)_dataGridView.Rows[index].Cells[0].Value;

        OnDelete?.Invoke(Id);
        _deleteBtn.Dispose();
    }

    void edit(int index)
    {
        int Id = (int)_dataGridView.Rows[index].Cells[0].Value;

        OnEdit?.Invoke(Id);

        _editBtn.Dispose();
    }

    void detail(int index)
    {
        int Id = (int)_dataGridView.Rows[index].Cells[0].Value;

        OnDetail?.Invoke(Id);
    }

    void OnResize()
    {
        int tableWidth;
        int columnSize;

        if (_dataGridView.DisplayedRowCount(false) < _dataGridView.RowCount)
        {
            tableWidth = this.Width - 20;
            columnSize = tableWidth / _header.Controls.Count;
            foreach (Control c in _header.Controls)
            {
                c.Size = new Size(columnSize, c.Height);
            }

            _header.Controls[_header.Controls.Count - 1].Width = columnSize + 20;
        }
        else
        {
            tableWidth = this.Width;
            columnSize = tableWidth / _header.Controls.Count;

            foreach (Control c in _header.Controls)
            {
                c.Size = new Size(columnSize, c.Height);
            }
        }
    }

    public void UpdateData(List<List<object>> newRows)
    {
        // reset binding and repopulate rows manually to support list-based rows
        _dataGridView.DataSource = null;
        _dataGridView.Rows.Clear();

        if (newRows == null) return;

        foreach (var row in newRows)
        {
            var values = new List<object>();
            if (row != null)
            {
                // take only as many values as header columns define
                for (int i = 0; i < _headerContent.Count && i < row.Count; i++)
                {
                    values.Add(row[i]);
                }

                // pad if row has fewer cells than headers
                while (values.Count < _headerContent.Count)
                {
                    values.Add(string.Empty);
                }
            }
            else
            {
                for (int i = 0; i < _headerContent.Count; i++) values.Add(string.Empty);
            }

            // add placeholder for action column if enabled
            if (_action)
            {
                values.Add(string.Empty);
            }

            _dataGridView.Rows.Add(values.ToArray());
        }
    }

    public void UpdateData(List<object> newItems)
    {
        _cellDatas = newItems ?? new List<object>();
        _displayCellData = new BindingList<object>(_cellDatas);
        _dataGridView.DataSource = _displayCellData;
    }

    void SetCotDiem()
    {
        _dataGridView.ReadOnly = false;
        AddColumnTongDiem();

        AddColumn();

        SetActionDgv();
        SetAction();
    }

    private CustomButton _addColBtn;

    void AddColumnTongDiem()
    {
        MyTLP pnlCot = new MyTLP
        {
            Height = 70,
            ColumnCount = 2,
            Margin = new Padding(0, 3, 0, 3),
        };
        pnlCot.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlCot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _addColBtn = new CustomButton(15, 15, "plus.svg", MyColor.GrayBackGround, false, false, false, false)
            { Anchor = AnchorStyles.None, Margin = new Padding(7, 3, 3, 3) };
        _addColBtn.HoverColor = MyColor.GrayHoverColor;
        _addColBtn.SelectColor = MyColor.GraySelectColor;

        Label title = GetLabel("Tổng điểm");
        title.Margin = new Padding(3, 3, 20, 3);

        pnlCot.Controls.Add(_addColBtn);
        pnlCot.Controls.Add(title);
        _header.Controls.Add(pnlCot);


        DataGridViewTextBoxColumn textColTongDiem = new DataGridViewTextBoxColumn();
        textColTongDiem.HeaderText = "Tổng điểm";
        textColTongDiem.Name = "TongDiem";
        textColTongDiem.ReadOnly = true;
        // textColTongDiem.DefaultCellStyle.NullValue = 0 + "";

        _dataGridView.Columns.Add(textColTongDiem);
    }

    void SetAction()
    {
        _addColBtn._mouseDown += () => AddColumn();
        _dataGridView.DataBindingComplete += SetupData;
    }

    private List<TextBox> listTbHeSo = new List<TextBox>();
    private int index = 0;

    void AddColumn()
    {
        _header.SuspendLayout();

        // tối đa 5 cột điểm
        if (index >= 5)
        {
            MessageBox.Show("Chỉ được thêm tối đa 5 cột điểm", "Thông báo", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        index++;
        //thêm header
        MyTLP pnlCot = new MyTLP
        {
            Height = 70,
            ColumnCount = 2,
            RowCount = 2,
            Margin = new Padding(0, 3, 0, 3),
        };
        pnlCot.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlCot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));


        MyTLP panelTitle = new MyTLP
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 2
        };
        panelTitle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panelTitle.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        Label title = GetLabel("Cột điểm " + index);
        //nút xóa cột
        IndexButton btnRemove = new IndexButton(15, 15, "minus.svg", MyColor.GrayBackGround, false, false, false, false)
        {
            Anchor = AnchorStyles.None,
            index = index
        };
        btnRemove.HoverColor = MyColor.GrayHoverColor;
        btnRemove.SelectColor = MyColor.GraySelectColor;
        btnRemove.MouseDownIndex += (i) => RemoveColumn(i);

        panelTitle.Controls.Add(title);
        panelTitle.Controls.Add(btnRemove);


        Label lblhs = GetLabel("H.Số:");
        lblhs.AutoSize = true;
        TextBox hso = new TextBox
        {
            Dock = DockStyle.Fill,
            Font = GetFont.GetFont.GetMainFont(9, FontType.Regular),
            TextAlign = HorizontalAlignment.Center,
            PlaceholderText = "Hệ số",
            TabIndex = index,
            Text = "1",
        };
        hso.Leave += (sender, args) => OnChangeHeSo(hso);

        listTbHeSo.Add(hso);

        pnlCot.Controls.Add(panelTitle);
        pnlCot.SetColumnSpan(panelTitle, 2);
        pnlCot.Controls.Add(lblhs);
        pnlCot.Controls.Add(hso);

        _header.Controls.Add(pnlCot);

        int insertIndex = _header.Controls.Count - 2;
        _header.Controls.SetChildIndex(pnlCot, insertIndex);

        //thêm cột
        DataGridViewTextBoxColumn colNhapDiem = new DataGridViewTextBoxColumn();
        colNhapDiem.HeaderText = "Nhập điểm";
        colNhapDiem.Name = "colNhapDiem " + index;
        // colNhapDiem.DefaultCellStyle.NullValue = 0 + "";
        
        _dataGridView.Columns.Insert(insertIndex, colNhapDiem);
        OnResize();
        _header.ResumeLayout();
        SetupDefault();
    }

    void RemoveColumn(int i)
    {
        // tối thiểu 1 cột điểm
        if (index <= 1)
        {
            MessageBox.Show("Có ít nhất 1 cột điểm", "Thông báo", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }
        
        DialogResult rs = MessageBox.Show("Xóa cột sẽ xóa hết các dữ liệu trên cột \n Bạn có chắc muốn xóa ?",
            "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (rs == DialogResult.No)
        {
            return;
        }

        _header.SuspendLayout();
        // Xóa header, xóa cột, resetindex
        int numCol = _header.Controls.Count;
        int pos = numCol - (index + 1) + i - 1;
        _header.Controls.RemoveAt(pos);

        string colName = "colNhapDiem " + i;
        _dataGridView.Columns.Remove(colName);
        UpdateListTbHso(i);
        index--;

        OnResize();
        _header.ResumeLayout();
        OnResize();
    }

    void UpdateListTbHso(int i)
    {
        int pos = i - 1;
        listTbHeSo.RemoveAt(pos);
    }

    void SetActionDgv()
    {
        _dataGridView.EditingControlShowing += (sender, args) => dataGridView1_EditingControlShowing(sender, args);
        _dataGridView.CellMouseEnter += (sender, args) => dataGridView1_CellMouseEnter(sender, args);
        _dataGridView.CellPainting += (sender, args) => dataGridView1_CellPainting(sender, args);
        _dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
        _dataGridView.CellValueChanged += (sender, args) => dataGridView1_CellValueChanged(sender, args);
        _dataGridView.CellLeave += (sender, args) => dataGridView1_CellLeave(sender, args);
   

    }


    private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
        if (e.Control is TextBox tb)
        {
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.BackColor = Color.White;
            tb.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            tb.Margin = new Padding(0, 0, 19, 0);
            tb.AutoSize = false;
            tb.Dock = DockStyle.Fill;
        }
    }

    private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            var dgv = sender as DataGridView;
            var columnName = dgv.Columns[e.ColumnIndex].Name;

            if (columnName.StartsWith("colNhapDiem"))
            {
                dgv.Cursor = Cursors.Hand;
            }
            else
            {
                dgv.Cursor = Cursors.Default;
            }
        }
    }

    private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0)
            return;

        var dgv = sender as DataGridView;
        var columnName = dgv.Columns[e.ColumnIndex].Name;

        if (columnName.StartsWith("colNhapDiem"))
        {
            e.PaintBackground(e.ClipBounds, true);
            e.PaintContent(e.ClipBounds);

            using (Pen p = new Pen(Color.Gray, 1))
            {
                Rectangle rect = e.CellBounds;
                rect.X += 2;
                rect.Y += 2;
                rect.Width -= 4;
                rect.Height -= 4;
                e.Graphics.DrawRectangle(p, rect);
            }

            e.Handled = true;
        }
    }

    private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        var dgv = sender as DataGridView;
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // bỏ header

        var column = dgv.Columns[e.ColumnIndex];
        if (column.Name.StartsWith("colNhapDiem"))
        {
            OnChangeDiem(e.RowIndex, e.ColumnIndex);
        }
    }

    private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
    {
        if (_dataGridView.IsCurrentCellInEditMode)
            _dataGridView.EndEdit();
    }

    void OnChangeDiem(int rowIndex, int columnIndex)
    {
        DataGridViewCell cell = _dataGridView.Rows[rowIndex].Cells[columnIndex];
        if (cell == null)
        {
            return;
        }

        string diem = cell.Value.ToString();
        if (!ValidateDiem(diem))
        {
            cell.Value = 0;
            return;
        }

        UpdateTongDiem(rowIndex);
    }

    void OnChangeHeSo(TextBox heso)
    {
        if (!ValidateHeSo(heso.Text.Trim()))
        {
            heso.Text = "1";
        }
    }

    void UpdateTongDiem(int rowIndex)
    {
        float tongDiem = 0;
        float tongHeSo = 0;
        for (int i = 0; i < listTbHeSo.Count; i++)
        {
            int index = i + 1;
            DataGridViewCell cell = _dataGridView.Rows[rowIndex].Cells["colNhapDiem " + index];
            float diem;
            int heso;

            if (cell.Value == null)
            {
                diem = 0;
            }
            else
            {
                diem = float.Parse(cell.Value.ToString());
            }

            if (listTbHeSo[i].Text.Trim().Equals(""))
            {
                heso = 0;
            }
            else
            {
                heso = int.Parse(listTbHeSo[i].Text.Trim());
            }

            tongDiem += diem * heso;
            tongHeSo += heso;
        }

        if (tongHeSo != 0)
        {
            _dataGridView.Rows[rowIndex].Cells["TongDiem"].Value = tongDiem / tongHeSo;
        }
    }

    bool ValidateHeSo(string text)
    {
        return Validate.IsNumeric(text);
    }

    bool ValidateDiem(string diem)
    {
        return (Validate.IsValidDiem(diem));
    }

    public void SetupData(object sender, DataGridViewBindingCompleteEventArgs e)
    {
        SetupDefault();
        SetupCotDiem();
        _dataGridView.DataBindingComplete -= SetupData;
    }

    public void SetupDefault()
    {
        foreach (DataGridViewRow row in _dataGridView.Rows)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value == null)
                    cell.Value = 0;
            }
        }

    }

    public void SetupCotDiem()
    {
        if (_listDiemSV.Count == 0) return;
        DiemSV fstDiem = _listDiemSV[0];
        for (int i = 1; i < fstDiem.listCotDiem.Count; i++)
        {
            AddColumn();
        }

        for (int i = 0; i < listTbHeSo.Count; i++)
        {
            listTbHeSo[i].Text = fstDiem.listCotDiem[i].HeSo.ToString();
        }

        foreach (DiemSV diemSv in _listDiemSV)
        {
            int cell;
            int row = GetRowIndexByMaSv(diemSv.MaSV);

            if (diemSv.listCotDiem.Count == 0)
                return;

            for (int i = 0; i < diemSv.listCotDiem.Count; i++)
            {
                int index = i + 1;
                cell = GetColumnIndexByName("colNhapDiem " + index);
                _dataGridView.Rows[row].Cells[cell].Value = diemSv.listCotDiem[i].DiemSo;
            }
        }


    }

    int GetRowIndexByMaSv(int maSV)
    {
        foreach (DataGridViewRow x in _dataGridView.Rows)
        {
            if (int.Parse(x.Cells["MaSV"].Value.ToString()) == maSV)
            {
                return x.Index;
            }
        }

        return -1;
    }

    int GetColumnIndexByName(string name)
    {
        foreach (DataGridViewColumn x in _dataGridView.Columns)
        {
            if (x.Name.Equals(name))
            {
                return x.Index;
            }
        }

        return -1;
    }

    public void UpdateDiem()
    {
        List<DiemSV> listDiemSV = GetListDiemSV();
        //xóa cũ
        foreach (DiemSV diemSV in listDiemSV)
        {
            KetQuaDto kq = _ketQuaController.GetByMaSVMaHP(diemSV.MaSV, _maHp);
            
            //Xóa nếu có diemquatrinh cũ
            if (_diemQuaTrinhController.ExistsByMaKQ(kq.MaKQ))
            {
                DiemQuaTrinhDto dqt = _diemQuaTrinhController.GetByMaKQ(kq.MaKQ);
                List<CotDiemDto> listCotDiem = _cotDiemController.GetByMaDQT(dqt.MaDQT);

                foreach (CotDiemDto cd in listCotDiem)
                {
                    if (!_cotDiemController.HardDelete(cd.MaCD))
                    {
                        MessageBox.Show("Xóa cột điểm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                
                if (!_diemQuaTrinhController.HardDelete(dqt.MaDQT))
                {
                    MessageBox.Show("Xóa điểm quá trình thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        
        //thêm mới
        foreach (DiemSV diemSV in listDiemSV)
        {
            KetQuaDto kq = _ketQuaController.GetByMaSVMaHP(diemSV.MaSV, _maHp);

            float tongDiem = GetTongDiemDQT(diemSV);
            DiemQuaTrinhDto dqt = new DiemQuaTrinhDto
            {
                MaKQ = kq.MaKQ,
                DiemSo = tongDiem,
            };

            if (!_diemQuaTrinhController.Insert(dqt))
            {
                MessageBox.Show("Them điểm quá trình thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int maDQT = _diemQuaTrinhController.GetLastAutoIncrement();

            foreach (CotDiemDto cd in diemSV.listCotDiem)
            {
                CotDiemDto cotDiem = new CotDiemDto
                {
                    DiemSo = cd.DiemSo,
                    HeSo = cd.HeSo,
                    TenCotDiem = cd.TenCotDiem,
                    MaDQT = maDQT
                };
                if (!_cotDiemController.Insert(cotDiem))
                {
                    MessageBox.Show("Them cột điểm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        
        MessageBox.Show("Cập nhật thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public float GetTongDiemDQT(DiemSV diemSV)
    {
        float tongDiem = 0;
        float tongHeSo = 0;
            
        foreach (CotDiemDto cd in diemSV.listCotDiem)
        {
            tongDiem += cd.DiemSo * cd.HeSo;
            tongHeSo += cd.HeSo;
        }

        if (tongHeSo == 0)
        {
            return 0;
        }
        Console.WriteLine("he " + tongDiem / tongHeSo);
        return tongDiem / tongHeSo;
    }
    
    public List<DiemSV> GetListDiemSV()
    {
        List<DiemSV> rs = new List<DiemSV>();
        int numCol = _header.Controls.Count;
        int startPos = numCol - (index + 1);
        int endPos = _header.Controls.Count - 2; //index -1, cot tong diem -11
        
        foreach (DataGridViewRow row in _dataGridView.Rows)
        {
            List<CotDiemDto> listCotDiem = new List<CotDiemDto>();
            DiemSV diemSV = new DiemSV();
            diemSV.MaSV = int.Parse(row.Cells["MaSV"].Value.ToString());
            
            int colIndex = 1;
            for (int i = startPos; i <= endPos; i++)
            {
                CotDiemDto cotDiem = new CotDiemDto
                {
                    TenCotDiem = "Cột điểm " + colIndex,
                    DiemSo = float.Parse(row.Cells[i].Value.ToString()),
                    HeSo = int.Parse(listTbHeSo[colIndex - 1].Text)
                };
                listCotDiem.Add(cotDiem);
                colIndex++;
                // Console.WriteLine(cotDiem.TenCotDiem + " " + cotDiem.DiemSo + " " + cotDiem.HeSo);
            }
            diemSV.listCotDiem = listCotDiem;
            rs.Add(diemSV);
        }
        return rs;
    }
}