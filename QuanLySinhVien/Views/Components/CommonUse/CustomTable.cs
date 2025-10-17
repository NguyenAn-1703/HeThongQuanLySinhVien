using System.ComponentModel;
using System.Data;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

#region Cách dùng

//Set bool action trước để set nút xóa, sửa
//Nếu set action và không set nút, tự động sẽ có nút xóa

#endregion

public class CustomTable : TableLayoutPanel
{
    public CustomDataGridView _dataGridView;
    List<string> _headerContent;
    private FlowLayoutPanel _header;
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
        _header = new FlowLayoutPanel();
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
        _editBtn = new CustomButton(20, 20, "fix.svg", MyColor.MainColor);

        Point myPoint = _topForm.PointToClient(rec);

        _editBtn.Location = myPoint;

        _topForm.Controls.Add(_editBtn);

        _editBtn.BringToFront();

        _editBtn.MouseDown += (sender, args) => edit(index);
    }

    void OnHoverDeleteBtn(Point rec, int index)
    {
        int rowIndex = index;
        //Vẽ vào form, không phụ thuộc layout
        _topForm = this.FindForm();
        _deleteBtn = new CustomButton(20, 20, "trashbin.svg", MyColor.RedHover);

        Point myPoint = _topForm.PointToClient(rec);

        _deleteBtn.Location = myPoint;

        _topForm.Controls.Add(_deleteBtn);

        _deleteBtn.BringToFront();

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
        // object o = _cellDatas[index];
        // Console.WriteLine("Xóa" + o.ToString());
        int Id = (int)_dataGridView.Rows[index].Cells[0].Value;
        
        OnDelete?.Invoke(Id);
        _deleteBtn.Dispose();
    }

    void edit(int index)
    {
        // object o = _cellDatas[index];
        // Console.WriteLine("Sửa" + o.ToString());
        
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
    //
    // List<string> GetStringDataRowByIndex(int index)
    // {
    //     object row = _cellDatas[index];
    //     List<string> arrayString = new List<string>();
    //     
    //     foreach (var item in row)
    //     {
    //         string value = item + "";
    //         arrayString.Add(value);
    //     }
    //     return arrayString;
    // }

    // List<List<string>> getDisplayCellDatas()
    // {
    //     List<List<string>> listString = _cellDatas
    //         .Select(row => row.Select(item => item?.ToString() ?? "").ToList())
    //         .ToList();
    //     return listString;
    // }

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
}