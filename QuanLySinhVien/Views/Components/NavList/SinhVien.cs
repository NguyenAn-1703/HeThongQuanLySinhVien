using Svg;

namespace QuanLySinhVien.Views.Components;

public class SinhVien : Panel
{
    public SinhVien()
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
            Dock = DockStyle.Bottom,
            //Padding = new  Padding(0 , 110 , 0 , 0),
        };
        borderTop.Controls.Add(Top());
        
        Controls.Add(borderTop);
        Controls.Add(Bottom());
    }
    
    
    
    
    // -------------- Graphics --------------- //
    private float GetFontWidth(Label label)
    {
        Graphics g = label.CreateGraphics();
        SizeF size = g.MeasureString(label.Text, label.Font);

        return size.Width;
    }
    
    
    
    
    
    
    // -------------- Label --------------- //
    private Label LbHeding()
    {
        Label lb = new Label
        {
            Dock = DockStyle.Left,
            Text = "Sinh viên",
            Font = new Font("JetBrains Mono", 17f, FontStyle.Bold),
            Height = 90,
            TextAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(30, 0, 0, 0),
        };
        
        lb.Width = Convert.ToInt32(GetFontWidth(lb)) + 40;
        return lb;
    }

    
    
    
    // -------------- SVG --------------- //
    private Image iconsPlus = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "img", "plus.svg")).Draw(25, 25);
    private Image iconsEdit = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "img", "edit.svg")).Draw(25, 25);
    
    
    // -------------- Button --------------- //
    private Button btnAdd()
    {
        Button btn = new Button()
        {
            Text = "Thêm",
            Font = new Font("JetBrains Mono", 10f, FontStyle.Bold),
            Width = 100,
            Height = 50,
            Cursor = Cursors.Hand,
            BackColor = MyColor.White,
            Dock = DockStyle.Fill,
            Image = iconsPlus,
            ImageAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0),
        };
        
        return btn;
    }

    private Button btnEdit()
    {
        Button btn = new Button()
        {
            Text = "Thêm",
            Font = new Font("JetBrains Mono", 10f, FontStyle.Bold),
            Width = 25,
            Height = 25,
            Cursor = Cursors.Hand,
            BackColor = MyColor.White,
            Dock = DockStyle.Fill,
            Image = iconsEdit,
            ImageAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0,0,0,0),
        };
        return btn;
    }
    
    
    // -------------- DataGridView --------------- //
    private DataGridView dgvSinhVien()
    {
        DataGridView dgv = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            ReadOnly = true,
            Font = new Font("JetBrains Mono", 10f, FontStyle.Regular),
            CellBorderStyle =  DataGridViewCellBorderStyle.Single,
            EnableHeadersVisualStyles = false,
            ColumnHeadersDefaultCellStyle =
            {
                Font = new Font("JetBrains Mono", 10f, FontStyle.Bold),
                ForeColor = MyColor.White,
                BackColor = MyColor.HeaderBlue,
                Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            DefaultCellStyle =
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
            },
            RowTemplate =
            {
                Height = 40
            },
            ColumnHeadersHeight = 50,
            Columns = { {"MaSV", "Mã sinh viên"},
                        {"HoTenSV", "Họ và tên"},
                        {"NgaySinhSV", "Ngày sinh"},
                        {"GioiTinhSV", "Giới tính"},
                        {"NganhSV", "Ngành"},
                        {"TrangThaiSV", "Trạng thái"},
                        {"HanhDongSV", "Hành động"},
            },
            
            Rows =
            {
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                {"3123410039", "Lê Mạnh Cường", "29/09/2005", "Nam", "Công nghệ thông tin", "Đang học"} ,
                
            }
        };
        dgv.CellPainting += (s, e) =>
        {
            if (e.ColumnIndex == dgv.Columns["HanhDongSV"].Index && e.RowIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, true);

                int btnSize = 25;
                int padding = 5;
                int w = e.CellBounds.Width;
                int h = e.CellBounds.Height;
                int top = e.CellBounds.Top + (h - btnSize) / 2;
                int left = e.CellBounds.Left + (w - btnSize * 2) / 2;
                
                Rectangle rectEdit = new Rectangle(left, top, btnSize, btnSize);
                Rectangle rectDelete = new Rectangle(rectEdit.Right + padding, top, btnSize,btnSize);
                
                e.Graphics.FillRectangle(new SolidBrush(MyColor.EditBlue), rectEdit);
                e.Graphics.FillRectangle(new SolidBrush(MyColor.DeleteRed), rectDelete);

                e.Graphics.DrawImage(iconsEdit, rectEdit);
                e.Graphics.DrawImage(iconsEdit, rectDelete);

                e.Handled = true;
            }
        };


        return dgv;
    }
    
    
    private Panel PanelButtonTop()
    {
        Panel panel = new Panel
        {
            BackColor = Color.Transparent,
            Width = 200,
            Height = 90,
            Dock = DockStyle.Right,
            Padding = new Padding(20, 20, 20, 20),
            Controls =
            {
                btnAdd()
            }
        };
        return panel;
    }

    private Panel Top()
    {
        Panel mainTop = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = MyColor.LightGray,
            Height = 90,
            Controls = {LbHeding(), PanelButtonTop()}
        };
        return mainTop;
    }

    private Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Bottom,
            BackColor = MyColor.LightGray,
            Height = 780,
            Padding = new Padding(20, 0, 20, 20),
            Controls = { dgvSinhVien() }
        };
        return mainBot;
    }

    
    
    // -------------- Event --------------- //
}