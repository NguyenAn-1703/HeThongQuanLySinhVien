using System.ComponentModel;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components;

public class CustomPopup : Panel
{
    private List<object> _cellDatas;
    private List<string> _columnNames;
    private BindingList<object> _displayData;
    public CustomDataGridView _dt;

    public CustomPopup()
    {
        // Size = new Size(300, 300);

        Height = 105;
        _dt = new CustomDataGridView();
        Init();
    }

    public event Action<int> KeyEnter;

    private void Init()
    {
        BorderStyle = BorderStyle.FixedSingle;

        _dt.AlternatingRowsDefaultCellStyle = _dt.RowsDefaultCellStyle;
        // _dt.ScrollBars = ScrollBars.None;


        _dt.Dock = DockStyle.None;
        _dt.Height = 105;
        Controls.Add(_dt);
        SetAction();
    }

    public void SetData(List<string> columnNames, List<object> cellDatas)
    {
        _columnNames = columnNames;
        _cellDatas = cellDatas;

        foreach (var i in _columnNames)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = i,
                DataPropertyName = i,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            _dt.Columns.Add(column);
        }

        _displayData = new BindingList<object>(_cellDatas);
        _dt.DataSource = _displayData;
    }

    public void UpdateData(List<object> newData)
    {
        _cellDatas = newData ?? new List<object>();
        _displayData = new BindingList<object>(_cellDatas);
        _dt.DataSource = _displayData;
    }

    private void SetAction()
    {
        Resize += (sender, args) => { _dt.Width = Width + 15; };

        _dt.KeyDown += (sender, args) =>
        {
            if (args.KeyCode == Keys.Enter)
            {
                var id = (int)_dt.SelectedRows[0].Cells[0].Value;
                KeyEnter?.Invoke(id);
            }
        };

        _dt.CellClick += (sender, args) =>
        {
            var id = (int)_dt.SelectedRows[0].Cells[0].Value;
            KeyEnter?.Invoke(id);
        };
    }
}