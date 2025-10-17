using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Forms;

public class FormUpdateGV : Form
{
    private Form myForm;
    private GiangVien formParent;
    private GiangVienDto giangVien;
    private TextBox txbMaGV = new TextBox()
    {
        BorderStyle = BorderStyle.FixedSingle,
        Width = 300,
        Height = 100,
        Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
        Anchor = AnchorStyles.None,
        ReadOnly = true,
    };

    private TextBox txbTenGV = new TextBox()
    {
        BorderStyle = BorderStyle.FixedSingle,
        Width = 300,
        Height = 100,
        Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
        Anchor = AnchorStyles.None,
    };

    private ComboBox cbbKhoa = new ComboBox()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 300,
        Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
        Anchor = AnchorStyles.None,
    };

    private DateTimePicker dtpkNgaySinh = new DateTimePicker()
    {
        Width = 300,
        Height = 100,
        Font = Components.GetFont.GetFont.GetMainFont(10, FontType.Regular),
        Anchor = AnchorStyles.None,
        Format = DateTimePickerFormat.Custom,
        CustomFormat = "dd/MM/yyyy",
    };

    private TableLayoutPanel tbGioiTinh = new TableLayoutPanel()
    {
        Dock = DockStyle.Fill,
        ColumnCount = 2,
        RowCount = 1,
        
        ColumnStyles =
        {
            new ColumnStyle(SizeType.Percent, 50),
            new ColumnStyle(SizeType.Percent, 50),
        },
        Controls =
        {
            { new RadioButton
                {
                    Name = "rdbNam",
                    Text = "Nam",
                    Anchor = AnchorStyles.None,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = true
                }, 
                0, 0},
            { new RadioButton{
                Name = "rdbNu",
                Text = "Nữ",
                Anchor = AnchorStyles.None,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true
            }, 1, 0},
        },
    };

    private TextBox txbSoDienThoai = new TextBox()
    {
        BorderStyle = BorderStyle.FixedSingle,
        Width = 300,
        Height = 100,
        Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
        Anchor = AnchorStyles.None,
    };

    private TextBox txbEmail = new TextBox()
    {
        BorderStyle = BorderStyle.FixedSingle,
        Width = 300,
        Height = 100,
        Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
        Anchor = AnchorStyles.None,
    };

    private ComboBox cbbTrangThai = new ComboBox()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 300,
        Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
        Anchor = AnchorStyles.None,
    };
    
    public FormUpdateGV(GiangVienDto giangVien, GiangVien formParent)
    {
        this.giangVien = giangVien;
        this.formParent = formParent;
        Size = new Size(600, 1000);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.None;
        Controls.Add(fullDialog());
        myForm = this;
        
        Load();
    }

    private void Load()
    {
        txbMaGV.Text = giangVien.MaGV + "";
        txbTenGV.Text = giangVien.TenGV;
        KhoaController khoaController = new KhoaController();
        List<KhoaDto> listKhoa = khoaController.GetDanhSachKhoa();
        cbbKhoa.DataSource = listKhoa;
        cbbKhoa.DisplayMember = "TenKhoa";
        cbbKhoa.ValueMember = "MaKhoa";
        cbbKhoa.SelectedValue = giangVien.MaKhoa;
        dtpkNgaySinh.Value = giangVien.NgaySinhGV.ToDateTime(TimeOnly.MinValue);
        if(giangVien.GioiTinhGV == "Nam") tbGioiTinh.Controls["rdbNam"].Select();
        else tbGioiTinh.Controls["rdbNu"].Select();
        txbSoDienThoai.Text = giangVien.SoDienThoai;
        txbEmail.Text = giangVien.Email;
        List<string> cbbStatus = new List<string> { "Đang công tác", "Đang nghỉ phép" };
        cbbTrangThai.DataSource =  cbbStatus;
        cbbTrangThai.SelectedItem = giangVien.TrangThai;
    }

    private Label JLable(string txt)
    {
        Label lbl = new Label()
        {
            Text = txt,
            Font = Components.GetFont.GetFont.GetMainFont(10, FontType.Bold),
            Dock = DockStyle.Right,
            TextAlign = ContentAlignment.TopRight,
            Anchor = AnchorStyles.None
        };
        lbl.Width += 100;
        return lbl;
    }
    
    private TableLayoutPanel fullDialog()
    {
        // TopDialog
        var textLabel = new Label
        {
            Text = "Sửa thông tin giảng viên",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = Components.GetFont.GetFont.GetMainFont(13, FontType.Bold),
            Width = 300,
            Height = 60,
            Dock = DockStyle.Fill,
        };
        var topDialog = new Panel()
        {
            BackColor = Color.Aqua,
            Dock = DockStyle.Fill,
            Controls = { textLabel }
        };
        
        // MiddleDialog
        List<string> list = new List<string>{"Mã giảng viên: " ,"Tên giảng viên: " ,"Khoa: " , "Ngày sinh: ", "Giới tính: ", "Số điện thoại", "Email", "Trạng thái"};
        List<Control> rightComponents = new List<Control> {txbMaGV, txbTenGV, cbbKhoa, dtpkNgaySinh, tbGioiTinh, txbSoDienThoai, txbEmail, cbbTrangThai};
        
        var borderMiddleLeft = new TableLayoutPanel()
        {
            ColumnCount = 1,
            RowCount = list.Count,
            Dock = DockStyle.Fill,
        };

        var borderMiddleRight = new TableLayoutPanel()
        {
            ColumnCount = 1,
            RowCount = list.Count,
            Dock = DockStyle.Fill,
        };
        
        
        for (int i = 0; i < list.Count; i++)
        {
            Label lb = JLable(list[i]);
            Control rb = rightComponents[i];
            borderMiddleLeft.RowStyles.Add(new RowStyle(SizeType.Percent,  60f / list.Count));
            borderMiddleRight.RowStyles.Add(new RowStyle(SizeType.Percent, 60f / list.Count));
            borderMiddleLeft.Controls.Add(lb , 0 , i);
            borderMiddleRight.Controls.Add(rb , 1 , i);
        }

        
        var borderMiddle = new TableLayoutPanel()
        {
            ColumnCount = 2,
            RowCount = 1,
            Dock = DockStyle.Fill,
            ColumnStyles =
            {
                new ColumnStyle(SizeType.Percent, 35F),
                new ColumnStyle(SizeType.Percent, 65F),
            },
            Controls =
            {
                {borderMiddleLeft , 0 , 0},
                {borderMiddleRight , 1 , 0},
            },
        };
        var middleDialog = new Panel()
        {
            Dock = DockStyle.Fill,
            Controls = { borderMiddle }
        };
        var imgUpload = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "img" , "upload.svg")).Draw(250, 250);
        var boxImg = new PictureBox
        {   
            Image = imgUpload,
            //BackColor = Color.Blue,
            Anchor = AnchorStyles.None,
            Width = 250,
            Height = 250,
        };
        var imgAddButton = new Button
        {
            Text = "Thêm ảnh",
            Anchor = AnchorStyles.None,
            Font = Components.GetFont.GetFont.GetMainFont(12, FontType.Bold),
            ImageAlign = ContentAlignment.MiddleCenter,
            Width = 200,
            Height = 70,
        };
        var imgMiddleDialog = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            RowStyles =
            {
                new RowStyle(SizeType.Percent, 20F),
                new RowStyle(SizeType.Percent, 80F),
            },
            Controls =
            {
                {imgAddButton , 0 , 0},
                {boxImg , 0 , 1}
            }
        };
        var cancelButton = new Button
        {
            Text = "Hủy",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Red,
            Cursor = Cursors.Hand,
            Font = Components.GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            Width = 140,
            Height = 50,
            Margin = new Padding(20, 0, 20, 0),
        };
        cancelButton.Click += (s, e) =>
        {
            myForm.Close();
        };
        var addButton = new Button
        {
            Text = "Sửa",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Green,
            Cursor = Cursors.Hand,
            Font = Components.GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            Width = 140,
            Height = 50,
            Margin = new Padding(20, 0, 20, 0),
        };
        addButton.Click += (s, e) =>
        {
            try
            {
                GiangVienDto gv = giangVien;
                gv.TenGV = txbTenGV.Text;
                KhoaDto khoaDto = new KhoaDto();
                khoaDto = (KhoaDto) cbbKhoa.SelectedItem;
                gv.MaKhoa = khoaDto.MaKhoa;
                gv.TenKhoa = khoaDto.TenKhoa;
                gv.NgaySinhGV = DateOnly.FromDateTime(dtpkNgaySinh.Value);
                if (((RadioButton)tbGioiTinh.Controls["rdbNam"]).Checked) gv.GioiTinhGV = "Nam";
                else gv.GioiTinhGV = "Nữ";
                gv.SoDienThoai = txbSoDienThoai.Text;
                gv.Email = txbEmail.Text;
                gv.TrangThai = cbbTrangThai.SelectedItem.ToString();
                GiangVienController.UpdateGV(gv);
                MessageBox.Show("Cập nhật thông tin giảng viên thành công", "Thông báo",  MessageBoxButtons.OK, MessageBoxIcon.Information);
                formParent.LoadDatabase();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };
        var resetButton = new Button
        {
            Text = "Làm mới",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Cyan,
            Cursor = Cursors.Hand,
            Font = Components.GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            Width = 140,
            Height = 50,
            Margin = new Padding(20, 0, 20, 0),
        };
        resetButton.Click += (s, e) =>
        {
            try
            {
                int MaGV = giangVien.MaGV;
                giangVien = GiangVienController.GetGVById(MaGV);
                Load();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        };
        
        var middleDialogFull = new TableLayoutPanel
        {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill,
            RowStyles =
            {
                new RowStyle(SizeType.Percent, 40F),
                new RowStyle(SizeType.Percent, 60F)
            },
            Controls =
            {
                {imgMiddleDialog , 0 , 0},
                {middleDialog , 0 , 1},
            }
        };
        var bottomDialog = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
            ColumnStyles =
            {
                new  ColumnStyle(SizeType.Percent, 33.33F),
                new  ColumnStyle(SizeType.Percent, 33.33F),
                new  ColumnStyle(SizeType.Percent, 33.33F),
            },
            Controls = { cancelButton, resetButton, addButton },
        };


        
        TableLayoutPanel tb = new TableLayoutPanel{
            ColumnCount = 1,
            RowCount = 3,
            Dock = DockStyle.Fill,
            
            Controls = {{topDialog , 0 , 0}, 
                        {middleDialogFull , 0 , 1}, 
                        {bottomDialog , 0 , 2}},
            RowStyles = {new  RowStyle(SizeType.Percent, 5F), 
                         new RowStyle(SizeType.Percent, 80F),
                         new RowStyle(SizeType.Percent, 15F)}
        };
        return tb;
    }
}