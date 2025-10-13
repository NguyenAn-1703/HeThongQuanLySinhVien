using QuanLySinhVien.Views.Enums;
using Svg;

namespace QuanLySinhVien.Views.Forms;

public class FormAddGV : Form
{
    private Form myForm;
    public FormAddGV()
    {
        Text = "Thêm giảng viên";
        Size = new Size(600, 1000);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.None;
        Controls.Add(fullDialog());
        myForm = this;
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
            Text = "Thêm giảng viên",
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
        List<string> list = new List<string>{"Mã giảng viên: " ,"Tên giảng viên: " , "Ngày sinh: " , "Khoa: " , "Giới tính: ", "Số điện thoại", "Email", "Trạng thái"};
        float tile = 60f / list.Count;
        string[] cbb = new []{"Công nghệ thông tin" };
        string[] cbbStatus = new[] {"Đang dạy", "Đang nghỉ phép"};
        List<Control> rightComponents = new List<Control>();
        var radioNam = new RadioButton
        {
            Text = "Nam",
            Anchor = AnchorStyles.None,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true
        };
        var radioNu = new RadioButton
        {
            Text = "Nữ",
            Anchor = AnchorStyles.None,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true
        };
        var tableRadioButton = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
        };
        tableRadioButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableRadioButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableRadioButton.Controls.Add(radioNam, 0, 0);
        tableRadioButton.Controls.Add(radioNu, 1, 0);
        
        var panelRadioButton = new Panel()
        {
            Dock = DockStyle.Fill,
            Width = 300,
            Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        panelRadioButton.Controls.Add(tableRadioButton);
        var combo = new ComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 300,
            Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        combo.Items.AddRange(cbb);
        rightComponents.Add(new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        });
        rightComponents.Add(new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        });
        rightComponents.Add(new DateTimePicker()
        {
            Value = DateTime.Now,
            Width = 300,
            Height = 100,
            Font = Components.GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
        });
        rightComponents.Add(combo);
        rightComponents.Add(panelRadioButton);
        rightComponents.Add(new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        });
        var comboStatus = new ComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 300,
            Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        };
        comboStatus.Items.AddRange(cbbStatus);
        rightComponents.Add(comboStatus);
        rightComponents.Add(new TextBox()
        {
            BorderStyle = BorderStyle.FixedSingle,
            Width = 300,
            Height = 100,
            Font = Components.GetFont.GetFont.GetMainFont(11, FontType.Regular),
            Anchor = AnchorStyles.None,
        });

        
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
            borderMiddleLeft.RowStyles.Add(new RowStyle(SizeType.Percent,  tile));
            borderMiddleRight.RowStyles.Add(new RowStyle(SizeType.Percent, tile));
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
            Text = "Thêm",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Green,
            Cursor = Cursors.Hand,
            // Dock = DockStyle.Right,
            Font = Components.GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            Width = 140,
            Height = 50,
            Margin = new Padding(20, 0, 20, 0),
        };
        var resetButton = new Button
        {
            Text = "Làm mới",
            TextAlign = ContentAlignment.MiddleCenter,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Cyan,
            Cursor = Cursors.Hand,
            // Dock = DockStyle.Fill,
            Font = Components.GetFont.GetFont.GetMainFont(10, FontType.Regular),
            Anchor = AnchorStyles.None,
            Width = 140,
            Height = 50,
            Margin = new Padding(20, 0, 20, 0),
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