using System.ComponentModel;
using System.Data;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components;

public class CustomPopup : Panel
{
    public CustomDataGridView _dt;
    private List<object> _cellDatas;
    private List<string> _columnNames;
    private BindingList<object> _displayData;

    public event Action<int> KeyEnter;

    public CustomPopup()
    {
        // Size = new Size(300, 300);

        Height = 105;
        _dt = new CustomDataGridView();
        Init();
    }

    void Init()
    {
        this.BorderStyle = BorderStyle.FixedSingle;

        _dt.AlternatingRowsDefaultCellStyle = _dt.RowsDefaultCellStyle;
        // _dt.ScrollBars = ScrollBars.None;
        _dt.ShowCellToolTips = false;

        _dt.Dock = DockStyle.None;

        this.Controls.Add(_dt);
        SetAction();
    }

    public void SetData(List<string> columnNames, List<object> cellDatas)
    {
        _columnNames = columnNames;
        _cellDatas = cellDatas;

        foreach (string i in _columnNames)
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

    void SetAction()
    {
        this.Resize += (sender, args) => { _dt.Width = this.Width + 15;};
        
        _dt.KeyDown += (sender, args) =>
        {
            if (args.KeyCode == Keys.Enter)
            {
                int id = (int)_dt.SelectedRows[0].Cells[0].Value;
                KeyEnter?.Invoke(id);
            }
        };

        _dt.CellClick += (sender, args) =>
        {
            int id = (int)_dt.SelectedRows[0].Cells[0].Value;
            KeyEnter?.Invoke(id);
        };
    }
}