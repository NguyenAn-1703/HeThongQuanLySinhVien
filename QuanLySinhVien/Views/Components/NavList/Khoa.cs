using System.Data;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class Khoa : NavBase
{
    private string[] _listSelectionForComboBox = new []{"Mã khoa", "Tên khoa"};

    private DataTable table = new DataTable();
    private CUse cuse;
    private KhoaController _kcontroller;
    private DataGridView dgv;
    public Khoa()
    {
        _kcontroller = new KhoaController();
        cuse = new CUse();
        Init();
        
    }
    private void Init()
    {
        Dock = DockStyle.Fill; // để nó full vùng center
        Size = new Size(1200, 900);

        var borderTop = new Panel
        {
            Dock = DockStyle.Fill
        };

        borderTop.Controls.Add(Bottom()); // Add Bottom trước
        borderTop.Controls.Add(Top());    // Add Top sau để nằm trên cùng

        Controls.Add(borderTop);
    }

    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Top,   // đổi sang Top
            BackColor = MyColor.GrayBackGround,
            Height = 90
        };
        return mainTop;
    }

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Fill,  // chiếm toàn bộ phần còn lại
            BackColor = MyColor.GrayBackGround,
            Padding = new Padding(20, 0, 20, 0)
        };

        table.Columns.Add("Tên khoa", typeof(string));
        table.Columns.Add("Email", typeof(string));
        table.Columns.Add("Địa chỉ", typeof(string));

        // for (int i = 0; i < 50; i++)
        // {
        //     table.Rows.Add("abc", "deff","ghi");
        // }

        dgv = new DataGridView
        {
            Dock = DockStyle.Fill,
            DataSource = table,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BorderStyle = BorderStyle.None,
            BackgroundColor = MyColor.GrayBackGround,
            AllowUserToAddRows = false,
            ReadOnly = true
        };

        dgv.EnableHeadersVisualStyles = false; // quan trọng, để tự set màu
        dgv.ColumnHeadersDefaultCellStyle.BackColor = MyColor.MediumBlue; // xanh dương
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // chữ trắng
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        dgv.ColumnHeadersHeight = 40; // chiều cao header
        dgv.AutoSize = false;
        dgv.BorderStyle = BorderStyle.None;
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgv.RowHeadersVisible = false;
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
        dgv.AllowUserToResizeRows = false;   // khóa resize row
        dgv.AllowUserToResizeColumns = false; // khóa resize column
        
        // 3. Add vào panel
        mainBot.Controls.Add(dgv);
        loadData();        
        return mainBot;
    }

    public void loadData()
    {
        dgv.DataSource = _kcontroller.GetDanhSachKhoa();
    }
    
    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
}