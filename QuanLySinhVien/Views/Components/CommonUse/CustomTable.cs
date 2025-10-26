using System.ComponentModel;
using System.Data;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

#region Cách dùng

//Set bool action trước để set nút xóa, sửa
//Nếu set action và không set nút, tự động sẽ có nút xóa

#endregion

public class CustomTable : MyTLP
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

    void Init()
    {
        Configuration();
        SetHeader();
        SetContent();
        SetEventListen();
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
        _header.WrapContents = false;
        _header.BackColor = MyColor.MainColor;
        _header.Margin = new Padding(0, 0, 0, 0);
        _header.Padding = new Padding(5, 0, 5, 0);

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
        Console.WriteLine(_cellDatas.Count);
            
        this.Controls.Add(_dataGridView);
        
    }

    Label GetLabel(String text)
    {
        Label lbl = new Label
        {
            Dock = DockStyle.Top,
            Height = 30,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = text,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
            ForeColor = MyColor.White,
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
        _editBtn.MouseLeave +=  (sender, args) =>_editBtn.Visible = false;
        
        
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
        _deleteBtn.MouseLeave +=  (sender, args) =>_deleteBtn.Visible = false;


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
        // object o = _cellDatas[index];
        // Console.WriteLine("Chi tiết" + o.ToString());
        
        int Id = (int)_dataGridView.Rows[index].Cells[0].Value;
        
        OnDetail?.Invoke(Id);
    }

    void OnResize()
    {
        int tableWidth = this.Width - 24;
        int columnSize = tableWidth / _header.Controls.Count;
       
        foreach (Control c in _header.Controls)
        {
            c.Size = new Size(columnSize, c.Height);
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

    public void AddColumn(ColumnType columnType, string title)
    {
        if (columnType == ColumnType.CheckBox)
        {
            AddCbColumn(title);
        }
    }

    public event Action<int, bool> ClickCB;

    void AddCbColumn(string title)
    {
        DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
        // chkCol.HeaderText = "Chọn";
        chkCol.Name = "colChon";
        // chkCol.Width = 50; // Tùy chỉnh độ rộng
        chkCol.ReadOnly = false;
        _dataGridView.Columns.Add(chkCol);
        _dataGridView.ReadOnly = false;
        // _dataGridView.AutoGenerateColumns = false;
        this._header.Controls.Add(GetLabel(title));
        
        
        _dataGridView.CurrentCellDirtyStateChanged += (sender, e) =>
        {
            if (_dataGridView.IsCurrentCellDirty)
            {
                _dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        };
        
        _dataGridView.CellContentClick += (sender, e) =>
        {
            OnClickCB(e);
        };
    }
    // Cài đặt sk click cb
    void OnClickCB(DataGridViewCellEventArgs e)
    {
        if (_dataGridView.Columns[e.ColumnIndex].Name == "colChon" && e.RowIndex >= 0)
        {
            bool isChecked = Convert.ToBoolean(_dataGridView.Rows[e.RowIndex].Cells["colChon"].EditedFormattedValue);
            int i = Convert.ToInt32(_dataGridView.Rows[e.RowIndex].Cells[0].Value);
            ClickCB?.Invoke(i, isChecked);
        }
    }

    public void FailCb(int i)
    {
        int rowIndex = -1;
        foreach (DataGridViewRow row in _dataGridView.Rows)
        {
            if (row.Cells[0].Value != null && Convert.ToInt32(row.Cells[0].Value) == i)
            {
                rowIndex = row.Index;
                break;
            }
        }
        _dataGridView.Rows[rowIndex].Cells["colChon"].Value = false;
        _dataGridView.RefreshEdit();
        _dataGridView.Refresh();
    }

    public void tickCB(int i)
    {
        int rowIndex = -1;
        foreach (DataGridViewRow row in _dataGridView.Rows)
        {
            if (row.Cells[0].Value != null && Convert.ToInt32(row.Cells[0].Value) == i)
            {
                rowIndex = row.Index;
                break;
            }
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
        this.OnResize();
    }
    public void EnableActionColumn()
    {
        if (!_action) return;
        if (_dataGridView.Columns["Action"].Visible) return;
        _dataGridView.Columns["Action"].Visible = true;
        
        _header.Controls.Add(GetLabel("Hành động"));
        this.OnResize();
    }
}