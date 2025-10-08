using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class TitleButton : RoundTLP
{
    private string _path;
    private string _title;
    
    public Color BackgroundColor {get; set;} = MyColor.White;
    public Color HoverColor { get; set; } = MyColor.GrayHoverColor;
    public Color SelectColor { get; set; } =  MyColor.GraySelectColor;
    public Label _label { get; set; }

    public event Action _mouseDown;

    public TitleButton(string title, string path = "")
    {
        Border = true;
        _path = path;
        _title = title;
        Init();
    }

    void Init()
    {
        ColumnCount = 2;
        BackColor = MyColor.White;
        AutoSize = true;
        Padding = new Padding(7, 5, 7, 5);
        Margin = new Padding(3, 3, 20, 3);

        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        PictureBox pb = new PictureBox();
        if (!_path.Equals(""))
        {
            pb.Anchor = AnchorStyles.None;
            pb.Size = new Size(20, 20);
            pb.Image = GetSvgBitmap.GetBitmap(_path);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
        }
        else
        {
            pb.Size = new Size(0,0);
        }

        Controls.Add(pb);


        _label = new Label
        {
            Text = _title,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular),
        };

        Controls.Add(_label);

        SetAction();
    }

    void SetAction()
    {
        this.MouseEnter += (sender, args) => { OnMouseEnter(); };
        this.MouseLeave += (sender, args) => { OnMouseLeave(); };
        this.MouseDown += (sender, args) => { OnMouseDown(); };
        this.MouseUp += (sender, args) => { OnMouseUp(); };
        foreach (Control i in this.Controls)
        {
            i.MouseEnter += (sender, args) => { OnMouseEnter(); };
            i.MouseLeave += (sender, args) => { OnMouseLeave(); };
            i.MouseDown += (sender, args) => { OnMouseDown(); };
            i.MouseUp += (sender, args) => { OnMouseUp(); };
        }
    }

    void OnMouseEnter()
    {
        this.BackColor = MyColor.GrayHoverColor;
    }

    void OnMouseLeave()
    {
        this.BackColor = MyColor.White;
    }

    void OnMouseDown()
    {
        this.BackColor = MyColor.GraySelectColor;
        _mouseDown?.Invoke();
    }

    void OnMouseUp()
    {
        this.BackColor = MyColor.GrayHoverColor;
    }
}