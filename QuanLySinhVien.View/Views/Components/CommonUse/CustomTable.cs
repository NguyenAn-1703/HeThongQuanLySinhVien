using System.ComponentModel;
using QuanLySinhVien.Shared.Enums;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

#region Cách dùng

//Set bool action trước để set nút xóa, sửa
//Nếu set action và không set nút, tự động sẽ có nút xóa

#endregion

public class CustomTable : MyTLP
{
    private readonly bool _action;
    private readonly List<string> _columnNames; //để truy suất
    private readonly bool _delete;
    private readonly bool _edit;
    private readonly List<string> _headerContent;
    private List<object> _cellDatas;
    public CustomDataGridView _dataGridView;
    private CustomButton _deleteBtn;
    private BindingList<object> _displayCellData;

    private CustomButton _editBtn;
    protected MyFLP _header;
    private Form _topForm;


    public CustomTable(List<string> headerContent, List<string> columnNames, List<object> cells, bool action = false,
        bool edit = false, bool delete = false)
    {
        _headerContent = headerContent;
        _header = new MyFLP();
        _cellDatas = cells;
        _action = action;
        _edit = edit;
        _delete = delete;
        _columnNames = columnNames;
        Init();
    }

    public event Action<int> OnEdit;
    public event Action<int> OnDelete;
    public event Action<int> OnDetail;

    private void Init()
    {
        Configuration();
        SetHeader();
        SetContent();
        SetEventListen();
    }

    private void Configuration()
    {
        Dock = DockStyle.Fill;
        RowCount = 2;
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _dataGridView = new CustomDataGridView(_action, _edit, _delete);
    }

    private void SetHeader()
    {
        _header.Dock = DockStyle.Top;
        _header.AutoSize = true;
        _header.FlowDirection = FlowDirection.LeftToRight;
        _header.WrapContents = false;
        _header.BackColor = MyColor.MainColor;
        _header.Margin = new Padding(0, 0, 0, 0);
        _header.Padding = new Padding(0, 0, 0, 0);

        foreach (var i in _headerContent) _header.Controls.Add(GetLabel(i));

        if (_action) _header.Controls.Add(GetLabel("Hành động"));

        Controls.Add(_header);
    }

    private void SetContent()
    {
        for (var i = 0; i < _headerContent.Count; i++)
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
        Console.WriteLine(_cellDatas.Count);

        Controls.Add(_dataGridView);
    }

    private Label GetLabel(string text)
    {
        var lbl = new Label
        {
            Dock = DockStyle.Top,
            Height = 30,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = text,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            ForeColor = MyColor.White,
            Margin = new Padding(0, 3, 0, 3)
        };
        return lbl;
    }

    private void SetEventListen()
    {
        Resize += (sender, args) => OnResize();
        _dataGridView.CellDoubleClick += (sender, args) => OnDoubleClickRow(args);

        _dataGridView.BtnHoverEdit += (rec, index) => OnHoverEditBtn(rec, index);
        _dataGridView.BtnHoverDelete += (rec, index) => OnHoverDeleteBtn(rec, index);

        _dataGridView.BtnEditLeave += () => OnLeaveEditBtn();
        _dataGridView.BtnDeleteLeave += () => OnLeaveDeleteBtn();


        // fix lỗi hiện scrollbar sớm
        _dataGridView.ScrollBars = ScrollBars.None;
        _dataGridView.DataBindingComplete += (s, e) =>
        {
            if (_dataGridView.IsHandleCreated)
                // đợi vòng UI tiếp theo để layout xong hoàn toàn
                _dataGridView.BeginInvoke(() => { _dataGridView.ScrollBars = ScrollBars.Both; });
            else
                _dataGridView.ScrollBars = ScrollBars.Both;
        };
    }

    private void OnDoubleClickRow(DataGridViewCellEventArgs e)
    {
        var index = e.RowIndex;
        detail(index);
    }

    private void OnHoverEditBtn(Point rec, int index)
    {
        var rowIndex = index;
        //Vẽ vào form, không phụ thuộc layout
        _topForm = FindForm();
        _editBtn = new CustomButton(20, 20, "fix.svg", MyColor.MainColor)
        {
            Cursor = Cursors.Hand
        };

        _topForm.Controls.Add(_editBtn);
        _editBtn.BringToFront();

        var myPoint = _topForm.PointToClient(rec);
        _editBtn.Location = myPoint;

        var cursorPos = Cursor.Position;
        var cursorOnForm = _topForm.PointToClient(cursorPos);

        // Nếu chuột đang nằm trong vùng nút thì hiển thị, nếu không thì ẩn
        if (_editBtn.Bounds.Contains(cursorOnForm))
            _editBtn.Visible = true;
        else
            _editBtn.Visible = false;
        _editBtn.MouseLeave += (sender, args) => _editBtn.Visible = false;


        _editBtn.MouseDown += (sender, args) => edit(index);
    }

    private void OnHoverDeleteBtn(Point rec, int index)
    {
        var rowIndex = index;
        //Vẽ vào form, không phụ thuộc layout
        _topForm = FindForm();
        _deleteBtn = new CustomButton(20, 20, "trashbin.svg", MyColor.RedHover)
        {
            Cursor = Cursors.Hand
        };

        _topForm.Controls.Add(_deleteBtn);
        _deleteBtn.BringToFront();

        var myPoint = _topForm.PointToClient(rec);
        _deleteBtn.Location = myPoint;

        var cursorPos = Cursor.Position;
        var cursorOnForm = _topForm.PointToClient(cursorPos);

        // Nếu chuột đang nằm trong vùng nút thì hiển thị, nếu không thì ẩn
        if (_deleteBtn.Bounds.Contains(cursorOnForm))
            _deleteBtn.Visible = true;
        else
            _deleteBtn.Visible = false;
        _deleteBtn.MouseLeave += (sender, args) => _deleteBtn.Visible = false;


        _deleteBtn.MouseDown += (sender, args) => delete(index);
    }

    private void OnLeaveEditBtn()
    {
        _editBtn.Dispose();
    }

    private void OnLeaveDeleteBtn()
    {
        _deleteBtn.Dispose();
    }

    private void delete(int index)
    {
        var Id = (int)_dataGridView.Rows[index].Cells[0].Value;

        OnDelete?.Invoke(Id);
        _deleteBtn.Dispose();
    }

    private void edit(int index)
    {
        var Id = (int)_dataGridView.Rows[index].Cells[0].Value;

        OnEdit?.Invoke(Id);

        _editBtn.Dispose();
    }

    private void detail(int index)
    {
        // object o = _cellDatas[index];
        // Console.WriteLine("Chi tiết" + o.ToString());

        var Id = (int)_dataGridView.Rows[index].Cells[0].Value;

        OnDetail?.Invoke(Id);
    }

    private void OnResize()
    {
        int tableWidth;
        int columnSize;
        int scrollbarWidth = SystemInformation.HorizontalScrollBarHeight;

        if (_dataGridView.DisplayedRowCount(false) < _dataGridView.RowCount)
        {
            tableWidth = Width - scrollbarWidth;
            columnSize = tableWidth / _header.Controls.Count;
            foreach (Control c in _header.Controls) c.Size = new Size(columnSize, c.Height);
            _header.Controls[_header.Controls.Count - 1].Width = columnSize + 25;
        }
        else
        {
            tableWidth = Width;
            columnSize = tableWidth / _header.Controls.Count;
            foreach (Control c in _header.Controls) c.Size = new Size(columnSize, c.Height);
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
                for (var i = 0; i < _headerContent.Count && i < row.Count; i++) values.Add(row[i]);
                // pad if row has fewer cells than headers
                while (values.Count < _headerContent.Count) values.Add(string.Empty);
            }
            else
            {
                for (var i = 0; i < _headerContent.Count; i++) values.Add(string.Empty);
            }

            // add placeholder for action column if enabled
            if (_action) values.Add(string.Empty);

            _dataGridView.Rows.Add(values.ToArray());
        }
    }

    public void UpdateData(List<object> newItems)
    {
        _cellDatas = newItems ?? new List<object>();
        _displayCellData = new BindingList<object>(_cellDatas);
        _dataGridView.DataSource = _displayCellData;
    }

    public void AddColumn(ColumnType columnType, string title)
    {
        if (columnType == ColumnType.CheckBox)
            AddCbColumn(title);
        else if (columnType == ColumnType.Button) AddBtnColumn(title);
    }

    public event Action<int, bool> ClickCB;

    private void AddCbColumn(string title)
    {
        var chkCol = new DataGridViewCheckBoxColumn();
        // chkCol.HeaderText = "Chọn";
        chkCol.Name = "colChon";
        // chkCol.Width = 50; // Tùy chỉnh độ rộng
        chkCol.ReadOnly = false;
        _dataGridView.Columns.Add(chkCol);
        _dataGridView.ReadOnly = false;
        // _dataGridView.AutoGenerateColumns = false;
        _header.Controls.Add(GetLabel(title));


        _dataGridView.CurrentCellDirtyStateChanged += (sender, e) =>
        {
            if (_dataGridView.IsCurrentCellDirty) _dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };

        _dataGridView.CellContentClick += (sender, e) => { OnClickCB(e); };
    }

    // Cài đặt sk click cb
    private void OnClickCB(DataGridViewCellEventArgs e)
    {
        if (_dataGridView.Columns[e.ColumnIndex].Name == "colChon" && e.RowIndex >= 0)
        {
            var isChecked = Convert.ToBoolean(_dataGridView.Rows[e.RowIndex].Cells["colChon"].EditedFormattedValue);
            var i = Convert.ToInt32(_dataGridView.Rows[e.RowIndex].Cells[0].Value);
            ClickCB?.Invoke(i, isChecked);
        }
    }

    public void FailCb(int i)
    {
        var rowIndex = -1;
        foreach (DataGridViewRow row in _dataGridView.Rows)
            if (row.Cells[0].Value != null && Convert.ToInt32(row.Cells[0].Value) == i)
            {
                rowIndex = row.Index;
                break;
            }

        _dataGridView.Rows[rowIndex].Cells["colChon"].Value = false;
        _dataGridView.RefreshEdit();
        _dataGridView.Refresh();
    }

    public void tickCB(int i)
    {
        var rowIndex = -1;
        foreach (DataGridViewRow row in _dataGridView.Rows)
            if (row.Cells[0].Value != null && Convert.ToInt32(row.Cells[0].Value) == i)
            {
                rowIndex = row.Index;
                break;
            }

        _dataGridView.Rows[rowIndex].Cells["colChon"].Value = true;

        _dataGridView.RefreshEdit();
        _dataGridView.Refresh();
    }

    public void ConfigDKHP()
    {
        _dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        _dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
    }

    public void DisapleActionColumn()
    {
        if (!_action) return;
        if (!_dataGridView.Columns["Action"].Visible) return;
        _header.Controls.RemoveAt(_header.Controls.Count - 1);
        _dataGridView.Columns["Action"].Visible = false;
        OnResize();
    }

    public void EnableActionColumn()
    {
        if (!_action) return;
        if (_dataGridView.Columns["Action"].Visible) return;
        _dataGridView.Columns["Action"].Visible = true;

        _header.Controls.Add(GetLabel("Hành động"));
        OnResize();
    }

    private void AddBtnColumn(string title)
    {
        _header.Controls.Add(GetLabel(title));

        var btnCol = new DataGridViewButtonColumn();
        btnCol.HeaderText = title;
        btnCol.Name = "colButton";
        btnCol.Text = "Cập nhật";
        btnCol.UseColumnTextForButtonValue = true;

        _dataGridView.Columns.Add(btnCol);
        _dataGridView.CellContentClick += dgvHocPhi_CellContentClick;
    }

    private void dgvHocPhi_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (_dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
        {
            var maSV = int.Parse(_dataGridView.Rows[e.RowIndex].Cells["MaSV"].Value.ToString());
            OnClickBtn(maSV);
        }
    }

    public event Action<int> MouseClickBtnCapNhat;

    private void OnClickBtn(int maSV)
    {
        MouseClickBtnCapNhat?.Invoke(maSV);
    }
    
    public void FocusLastRow()
    {
        int lastRow = _dataGridView.Rows.Count - 1;
        if (lastRow >= 0)
        {
            _dataGridView.ClearSelection();
            _dataGridView.Rows[lastRow].Selected = true;
            _dataGridView.FirstDisplayedScrollingRowIndex = lastRow;
        }
    }
}