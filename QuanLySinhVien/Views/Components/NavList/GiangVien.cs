using System.Data;
using System.Runtime.InteropServices.JavaScript;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Enums;
using QuanLySinhVien.Views.Forms;
using Svg;

namespace QuanLySinhVien.Views.Components;

public class GiangVien : NavBase
{
    private string[] _listSelectionForComboBox = new []{"Mã giảng viên", "Tên giảng viên"};
    private int PanelTopHeight = 90;
    private CUse _cUse;
    private Form formAddPopUp;
    private DataTable dt;

    public GiangVien()
    {
        _cUse = new CUse();
        Init();
        LoadDatabase();
    }
    
    // Co so du lieu ---------------------------------------------------------------------
    private void LoadDatabase()
    {
        dt.Rows.Clear();
        foreach (var gv in GiangVienController.GetAll())
        {
            dt.Rows.Add(gv.ToDataRow());
        }
        GridGV().DataSource = dt;
    }




    // Giao dien --------------------------------------------------------------------------
    private void Init()
    {
        dt = new DataTable()
        {
            Columns =
            {
                new DataColumn("Mã giảng viên", typeof(int)),
                new DataColumn("Họ tên",  typeof(string)),
                new DataColumn("Khoa", typeof(string)),
                new DataColumn("Ngày sinh", typeof(DateOnly)),
                new DataColumn("Giới tính", typeof(string)),
                new DataColumn("Số điện thoại", typeof(string)),
                new DataColumn("Email", typeof(string)),
                new DataColumn("Trạng thái", typeof(string)),
            }
        };
        
        //BackColor = Color.Blue;
        Dock = DockStyle.Fill;
        Size = new Size(1200, 900);
        var borderTop = new Panel
        {
            Dock = DockStyle.Fill,
            //Padding = new  Padding(0 , 110 , 0 , 0),
        };
        borderTop.Controls.Add(Top());
        Controls.Add(borderTop);
        Controls.Add(Bottom());
    }
    private Label HeaderLabel()
    {
        Label lb = new Label()
        {
            Text = "Giảng viên",
            Font = GetFont.GetFont.GetMainFont(13, FontType.SemiBold),
            Dock = DockStyle.Left,
            BackColor = Color.Transparent,
            Height = PanelTopHeight,
            TextAlign = ContentAlignment.MiddleRight,
        };
        lb.Width += 50;
        return lb;
    }


    // Form Add ----------------------------------------------
    private Panel PanelTop(string txt)
    {
        Label lb = new Label()
        {
            Text = txt,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Bold),
            Dock = DockStyle.Left,
            BackColor = Color.Transparent,
            ForeColor = Color.Black,
            Height = 30,
            Width = 300,
            // AutoSize = true,
            TextAlign = ContentAlignment.MiddleRight,
        };
        
        Panel pn = new Panel()
        {
            Dock = DockStyle.Top,
            Height = 50,
            Width = 300,
            // AutoSize = true,
            Controls = { lb }
        };
        return pn;
    }
    private Button AddButton()
    {
        Button btn = new Button()
        {
            Dock = DockStyle.Fill,
            Text = "Thêm",
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Cursor = Cursors.Hand,
            Image = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "img" , "plus.svg")).Draw(25, 25),
            ImageAlign = ContentAlignment.MiddleLeft,
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 1 },
            TextAlign = ContentAlignment.MiddleCenter,
        };

        btn.Click += (s, e) =>
        {
            int WForm = 600;
            int HForm = 600;

            Label HeaderAddForm = new Label()
            {
                Text = "Thêm giảng viên",
                Font = GetFont.GetFont.GetMainFont(13, FontType.SemiBold),
                ForeColor = MyColor.White,
                Dock = DockStyle.Top,
                BackColor = MyColor.MainColor,
                Height = 70,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            
            Panel pnMa = PanelTop("Mã giảng viên: ");
            Panel pnTen = PanelTop("Họ tên: ");
            Panel pnNgaySinh = PanelTop("Ngày sinh: ");
            Panel pnGioiTinh = PanelTop("Giới tính: ");
            Panel pnKhoa = PanelTop("Khoa: ");

            
            Form form = new Form()
            {
                Text = "Thêm giảng viên",
                Size = new Size(WForm, HForm),
                StartPosition = FormStartPosition.CenterParent,
                Controls = { HeaderAddForm}
            };
            form.ShowDialog();
        };
        return btn;
    }
    private Panel PanelButtonAdd()
    {
        Panel pn = new Panel()
        {
            Height = PanelTopHeight,
            Dock = DockStyle.Right,
            Padding = new Padding(25, 30, 25 , 25),
            Controls = { AddButton() }
        };
        return pn;
    }

    private DataGridView GridGV()
    {
        DataTable dt = new DataTable()
        {
            Columns =
            {
                new DataColumn("Mã giảng viên", typeof(string)),
                new DataColumn("Họ tên",  typeof(string)),
                new DataColumn("Ngày sinh", typeof(DateTime)),
                new DataColumn("Giới tính", typeof(string)),
                new DataColumn("Khoa", typeof(string)),
            }
        };
        for(int i=1; i<=30; i++) dt.Rows.Add("123", "Nguyễn Thanh Sang", new DateTime(2025, 01, 01), "Nam", "Công nghệ thông tin");
        
        DataGridView dtgv = new DataGridView()
        {
            DataSource = dt,
            Dock = DockStyle.Fill,
            AutoGenerateColumns = true,
            RowTemplate = { Height = 37 },
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.None,
            RowHeadersVisible = false,
            EnableHeadersVisualStyles = false,
            ColumnHeadersHeight = 45,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = ColorTranslator.FromHtml("#07689F"),
                ForeColor = ColorTranslator.FromHtml("#f5f5f5"),
                Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
            },
            AllowUserToAddRows = false,
            RowsDefaultCellStyle = { BackColor = MyColor.White },
            AlternatingRowsDefaultCellStyle = { BackColor = Color.LightGray },
        };
        bool _actionsAdded = false;
        dtgv.DataBindingComplete += (s, e) =>
        {
            if (_actionsAdded) return;
            _actionsAdded = true;
            
            
            var editIcon = _cUse.CreateIconWithBackground(
                Path.Combine(AppContext.BaseDirectory,"img","fix.svg"),
                Color.Black,
                ColorTranslator.FromHtml("#6DB7E3"),
                28,
                6,
                4
            );
            var delIcon  = _cUse.CreateIconWithBackground(
                Path.Combine(AppContext.BaseDirectory,"img","trashbin.svg"),
                Color.Black,
                ColorTranslator.FromHtml("#FF6B6B"),
                28,
                6,
                4
            );
            var colEdit = new DataGridViewImageColumn {
                Name = "Sửa",
                Image = editIcon,
                DataPropertyName = null,
                Width = 60,
                ImageLayout = DataGridViewImageCellLayout.Normal
            };
            
            var colDel = new DataGridViewImageColumn {
                Name = "Xóa",
                Image = delIcon,
                DataPropertyName = null,
                Width = 60,
                ImageLayout = DataGridViewImageCellLayout.Normal
            };
            dtgv.Columns.Add(colEdit);
            dtgv.Columns.Add(colDel);
            dtgv.ReadOnly = true;
            dtgv.Columns["Sửa"].ReadOnly = false;
            dtgv.Columns["Xóa"].ReadOnly = false;
            dtgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex == dtgv.Columns["Sửa"]?.Index)
                {
                    Console.WriteLine("Sửa");
                }
                else if (e.ColumnIndex == dtgv.Columns["Xóa"]?.Index) { Console.WriteLine("xoa"); }
            };
            
        };
        
        
        
        dtgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        return dtgv;
    }
    
    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = MyColor.GrayBackGround,
            Height = PanelTopHeight,
            Controls = { HeaderLabel(), PanelButtonAdd() }
        };
        return mainTop;
    }

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = MyColor.GrayBackGround,
            Height = 780,
            Padding = new  Padding(25, 0, 25, 25),
            Controls = { GridGV() }
        };
        return mainBot;
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    public override void onSearch(string txtSearch, string filter)
    { }

}