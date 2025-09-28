using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components;

public class TrangChu : NavBase
{
    private string[] _listSelectionForComboBox = new[] { "" };
    private PictureBox _image;
    private int _imgWidth = 364;
    private int _imgHeight = 626;
    private TableLayoutPanel _contentIfoImg;
    public TrangChu()
    {
        Init();
    }

    private void Init()
    {
        //BackColor = Color.Blue;
        Dock = DockStyle.Fill;
        Margin = new Padding(0);

        TableLayoutPanel mainLayout = new TableLayoutPanel();
        mainLayout.Margin = new Padding(0);
        mainLayout.Dock = DockStyle.Fill;

        mainLayout.ColumnCount = 2;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        mainLayout.Controls.Add(new Panel());

        
        SetImage();
        mainLayout.Controls.Add(_image);
        SetContentInFrontOfImg();
        
        this.Controls.Add(mainLayout);
        this.Controls.Add(_contentIfoImg);

        
        this.Resize += (sender, args) => { updateSize(); };

    }

    void updateSize()
    {
        _image.Width = this.Height * _imgWidth / _imgHeight;
        _contentIfoImg.Location = new Point(_image.Location.X, _image.Location.Y);
        _contentIfoImg.Size = new Size(_image.Width, _image.Height);
    }

    void SetImage()
    {
        _image = new PictureBox
        {
            Margin = new Padding(0),
            Image = GetPng.GetImage("img/jpg/homeimg.jpg"),
            Dock = DockStyle.Right,
            SizeMode = PictureBoxSizeMode.Zoom
        };
        _image.Paint += (sender, args) => Overlay(args);
    }
    
    private void Overlay(PaintEventArgs e)
    {
        Color overlayColor = Color.FromArgb(100, 0, 0, 0); 
        using (SolidBrush brush = new SolidBrush(overlayColor))
        {
            e.Graphics.FillRectangle(brush, _image.ClientRectangle);
        }
    }

    void SetContentInFrontOfImg()
    {
        _contentIfoImg = new TableLayoutPanel();
        _contentIfoImg.Location = new Point(_image.Location.X, _image.Location.Y);
        _contentIfoImg.Size = new Size(_image.Width, _image.Height);

        _contentIfoImg.RowCount = 3;
        
        _contentIfoImg.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        // _contentIfoImg.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentIfoImg.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Label lbl1 = new Label();
        lbl1.Text = "SGU";
        lbl1.AutoSize = true;
        lbl1.Font = GetFont.GetFont.GetMainFont(13, FontType.ExtraBold);
        lbl1.ForeColor = MyColor.White;
        
        _contentIfoImg.Controls.Add(lbl1);
    }


    public override List<string> getComboboxList()
        {
            return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
        }
    
    }