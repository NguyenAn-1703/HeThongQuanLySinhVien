using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class TitleButton : RoundTLP
{
    private readonly string _path;
    private readonly string _title;

    public TitleButton(string title, string path = "")
    {
        Border = true;
        _path = path;
        _title = title;
        Init();
    }

    public Color BackgroundColor { get; set; } = MyColor.White;
    public Color HoverColor { get; set; } = MyColor.GrayHoverColor;
    public Color SelectColor { get; set; } = MyColor.GraySelectColor;
    public Label _label { get; set; }

    public event Action _mouseDown;

    private void Init()
    {
        Cursor = Cursors.Hand;
        ColumnCount = 2;
        BackColor = MyColor.White;
        AutoSize = true;
        Padding = new Padding(7, 5, 7, 5);


        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var pb = new PictureBox();
        if (!_path.Equals(""))
        {
            pb.Anchor = AnchorStyles.None;
            pb.Size = new Size(20, 20);
            pb.Image = GetSvgBitmap.GetBitmap(_path);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
        }
        else
        {
            pb.Size = new Size(0, 0);
        }

        Controls.Add(pb);


        _label = new Label
        {
            Text = _title,
            AutoSize = true,
            Font = GetFont.GetFont.GetMainFont(10, FontType.Regular)
        };

        Controls.Add(_label);

        SetAction();
    }

    private void SetAction()
    {
        MouseEnter += (sender, args) => { OnMouseEnter(); };
        MouseLeave += (sender, args) => { OnMouseLeave(); };
        MouseDown += (sender, args) => { OnMouseDown(); };
        MouseUp += (sender, args) => { OnMouseUp(); };

        foreach (Control i in Controls)
        {
            i.MouseEnter += (sender, args) => { OnMouseEnter(); };
            i.MouseLeave += (sender, args) => { OnMouseLeave(); };
            i.MouseDown += (sender, args) => { OnMouseDown(); };
            i.MouseUp += (sender, args) => { OnMouseUp(); };
        }
    }

    private void OnMouseEnter()
    {
        BackColor = MyColor.GrayHoverColor;
    }

    private void OnMouseLeave()
    {
        BackColor = MyColor.White;
    }

    private void OnMouseDown()
    {
        BackColor = MyColor.GraySelectColor;
        _mouseDown?.Invoke();
    }

    private void OnMouseUp()
    {
        BackColor = MyColor.GrayHoverColor;
    }
}