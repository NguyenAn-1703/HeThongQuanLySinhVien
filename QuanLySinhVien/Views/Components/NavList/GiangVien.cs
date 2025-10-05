using System.Data;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Components;

public class GiangVien : NavBase
{
    private string[] _listSelectionForComboBox = new []{"Mã giảng viên", "Tên giảng viên"};
    private int PanelTopHeight = 90;
    public GiangVien()
    {
        Init();
    }
    private void Init()
    {
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
            Width = 250,
            TextAlign = ContentAlignment.MiddleRight,
        };
        
        Panel pn = new Panel()
        {
            Dock = DockStyle.Top,
            Height = 50,
            Width = 250,
            
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
                Controls = { pnKhoa, pnGioiTinh, pnNgaySinh, pnTen, pnMa, HeaderAddForm }
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
            dtgv.Columns.Add(
                new DataGridViewImageColumn()
                {
                    Name = "Sửa",
                    Image = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "img", "fix.svg")).Draw(20, 20),
                    DataPropertyName = null,
                    Width = 75,
                    DisplayIndex = 5
                }
            );
            dtgv.Columns.Add(
                new DataGridViewImageColumn()
                {
                    Name = "Xóa",
                    Image =
                        SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "img", "trashbin.svg")).Draw(20, 20),
                    DataPropertyName = null,
                    Width = 75,
                    DisplayIndex = 6
                }
            );
            dtgv.ReadOnly = true;
            dtgv.Columns["Sửa"].ReadOnly = false;
            dtgv.Columns["Xóa"].ReadOnly = false;
            dtgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex == dtgv.Columns["Sửa"]?.Index) { Console.WriteLine("sua"); }
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
    public override void onSearch(string txtSearch){}

}