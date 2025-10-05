using System.Data;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;


public class CustomTable : TableLayoutPanel
{
    CustomDataGridView _dataGridView;
    DataTable _dataTable;
    List<String> _headerContent;
    private FlowLayoutPanel _header;
    private List<List<object>> _cellDatas;
    private bool _action;
    private Form _topForm;

    private CustomButton _editBtn;
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
        SetEventListen();
    }

    void Configuration()
    {
        this.Dock = DockStyle.Fill;
        this.RowCount = 2;
        this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        _dataGridView = new CustomDataGridView(true);
    }

    void SetHeader()
    {
        _header.Dock = DockStyle.Top;
        _header.AutoSize = true;
        _header.FlowDirection = FlowDirection.LeftToRight;
        _header.WrapContents = false;
        _header.BackColor = MyColor.MainColor;
        _header.Margin =  new Padding(0, 0, 0, 0);
        _header.Padding = new Padding(5, 0, 5,0);
        
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

        if (_action)
        {
            _dataTable.Columns.Add("Hành động");
        }
        
        _dataGridView.DataSource = _dataTable;
        _dataGridView.Dock = DockStyle.Fill;
        _dataGridView.Font = GetFont.GetFont.GetMainFont(9, FontType.Regular);
        

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
        this._dataGridView.BtnHover += (rec) => OnHoverEditBtn(rec);
        this._dataGridView.BtnLeave += () => OnLeaveEditBtn();
    }

    void OnHoverEditBtn(Point rec)
    {
        //Vẽ vào form, không phụ thuộc layout
        _topForm = this.FindForm();
        _editBtn = new CustomButton(20, 20, "fix.svg", MyColor.MediumBlue);
         
        Point myPoint = _topForm.PointToClient(rec);
        
        _editBtn.Location = myPoint;
        
        _topForm.Controls.Add(_editBtn);
        
        _editBtn.BringToFront();
        Console.WriteLine("Dang goij hover");
    }

    void OnLeaveEditBtn()
    {
        _editBtn.Dispose();
        Console.WriteLine("Dang goij dípose");
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