using System.Data;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;


public class CustomTable : TableLayoutPanel
{
    DataGridView _dataGridView;
    DataTable _dataTable;
    List<String> _headerContent;
    private FlowLayoutPanel _header;
    private List<List<object>> _cellDatas;
    private bool _action;
    public CustomTable(List<String> headerContent, List<List<object>> cells, bool action = false)
    {
        _headerContent = headerContent;
        _header = new FlowLayoutPanel();
        _dataTable = new DataTable();
        _cellDatas = cells;
        _action = action;
        Init();
    }

    void Init()
    {
        Configuration();
        SetHeader();
        SetContent();
        // this.CellBorderStyle =TableLayoutPanelCellBorderStyle.Single;
        this.Resize += (sender, args) => OnResize();
    }

    void Configuration()
    {
        this.Dock = DockStyle.Fill;
        this.RowCount = 2;
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        
        _dataGridView = new DataGridView
        {
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AllowUserToResizeColumns = false,
            AllowUserToResizeRows = false,
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            RowHeadersVisible = false,
            ColumnHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#f5f5f5")
            }, 
        };
        _dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    void SetHeader()
    {
        _header.Dock = DockStyle.Top;
        _header.AutoSize = true;
        _header.FlowDirection = FlowDirection.LeftToRight;
        _header.WrapContents = false;
        
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
            _dataTable.Columns.Add(_headerContent[i], typeof(string));
        }
        
        for (int i = 0; i < _cellDatas.Count; i++)
        {
            _dataTable.Rows.Add(_cellDatas[i].ToArray());
        }
        
        _dataGridView.DataSource = _dataTable;
        _dataGridView.BorderStyle = BorderStyle.FixedSingle;
        _dataGridView.Dock = DockStyle.Fill;
        _dataGridView.Font = GetFont.GetFont.GetMainFont(8, FontType.Regular);
        this.Controls.Add(_dataGridView);
    }

    Label GetLabel(String text)
    {
        Label lbl = new Label
        {
            Dock = DockStyle.Top,
            Height = 30,
            Text = text,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = GetFont.GetFont.GetMainFont(9, FontType.SemiBold),
        };
        lbl.BorderStyle = BorderStyle.FixedSingle;
        return lbl;
    }

    void OnResize()
    {
        int tableWidth = this.Width - 24;
        int columnSize = _action ? tableWidth / (_headerContent.Count + 1) : tableWidth / _headerContent.Count;
        foreach (Control c in _header.Controls)
        {
            c.Size = new Size(columnSize, c.Height);
        }
    }
    
}