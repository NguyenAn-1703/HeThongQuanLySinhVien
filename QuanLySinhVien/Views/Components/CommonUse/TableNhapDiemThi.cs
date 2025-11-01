using System.ComponentModel;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Structs;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class TableNhapDiemThi : MyTLP
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

    private List<DiemThiSV> _listDiemThiSV;
    private int _maHp;

    private KetQuaController _ketQuaController;
    private CotDiemController _cotDiemController;
    private DiemQuaTrinhController _diemQuaTrinhController;

    public TableNhapDiemThi(List<string> headerContent, List<string> columnNames, List<object> cells,
        List<DiemThiSV> listDiemThiSV, int mahp, bool action = false,
        bool edit = false, bool delete = false)
    {
        _headerContent = headerContent;
        _header = new MyFLP();
        _cellDatas = cells;
        _action = action;
        _edit = edit;
        _delete = delete;
        _columnNames = columnNames;
        _listDiemThiSV = listDiemThiSV;
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

        AddColumn();

        SetActionDgv();
        SetAction();
    }
    
    void SetAction()
    {
        _dataGridView.DataBindingComplete += SetupData;
    }

    void AddColumn()
    {
        _header.SuspendLayout();

        //thêm header
        Label title = GetLabel("Điểm thi");

        _header.Controls.Add(title);

        int insertIndex = _header.Controls.Count - 1;
        _header.Controls.SetChildIndex(title, insertIndex);

        //thêm cột
        DataGridViewTextBoxColumn colNhapDiemThi = new DataGridViewTextBoxColumn();
        colNhapDiemThi.HeaderText = "Nhập điểm thi";
        colNhapDiemThi.Name = "colNhapDiemThi";
        
        _dataGridView.Columns.Insert(insertIndex, colNhapDiemThi);
        OnResize();
        _header.ResumeLayout();
        SetupDefault();
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

            if (columnName.StartsWith("colNhapDiemThi"))
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

        if (columnName.StartsWith("colNhapDiemThi"))
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
        if (column.Name.StartsWith("colNhapDiemThi"))
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
        foreach (DiemThiSV item in _listDiemThiSV)
        {
            int row = GetRowIndexByMaSv(item.MaSV);
            int cell = GetColumnIndexByName("colNhapDiemThi");
            _dataGridView.Rows[row].Cells[cell].Value = item.diemThi;
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
        foreach (DiemThiSV item in _listDiemThiSV)
        {
            int row = GetRowIndexByMaSv(item.MaSV);
            int cell = GetColumnIndexByName("colNhapDiemThi");
            float diem = float.Parse(_dataGridView.Rows[row].Cells[cell].Value.ToString());

            KetQuaDto ketQua = _ketQuaController.GetByMaSVMaHP(item.MaSV, _maHp);
            ketQua.DiemThi = diem;
            Console.WriteLine("masv " + ketQua.MaSV + "diem " + ketQua.DiemThi);
            if (!_ketQuaController.Update(ketQua))
            {
                MessageBox.Show("Sửa kết quả thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        MessageBox.Show("Cập nhật thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
    }

    
}