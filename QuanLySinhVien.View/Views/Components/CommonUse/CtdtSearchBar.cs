using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.View.Views.Components.ViewComponents;

namespace QuanLySinhVien.View.Views.Components.CommonUse.Search;

public class CtdtSearchBar : RoundTLP
{
    private readonly TextBox _txtSearch;

    public CtdtSearchBar()
    {
        _txtSearch = new TextBox();
        Init();
    }

    public int TxtWidth { get; set; } = 200;
    public new event Action<string> KeyDown;

    private void Init()
    {
        Padding = new Padding(3);
        AutoSize = true;
        Border = true;
        ColumnCount = 2;

        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));


        _txtSearch.Width = TxtWidth;
        _txtSearch.AutoSize = true;
        _txtSearch.Font = GetFont.GetFont.GetMainFont(11, FontType.Regular);
        _txtSearch.PlaceholderText = "Tìm kiếm ...";
        _txtSearch.BorderStyle = BorderStyle.None;

        var glassIcon = GetPb();
        Controls.Add(glassIcon);
        Controls.Add(_txtSearch);

        _txtSearch.Enter += (sender, args) => onEnter();
        _txtSearch.Leave += (sender, args) => onLeave();
        _txtSearch.KeyDown += (sender, args) => OnKeyDown(args);
        _txtSearch.TextChanged += (sender, args) => OnTextChanged();
    }

    private PictureBox GetPb()
    {
        var pb = new PictureBox
        {
            Image = GetSvgBitmap.GetBitmap("search.svg"),
            Size = new Size(20, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Anchor = AnchorStyles.None
        };
        return pb;
    }

    private void onEnter()
    {
        BorderColor = MyColor.MainColor;
        Invalidate();
    }

    private void onLeave()
    {
        BorderColor = MyColor.GraySelectColor;
        Invalidate();
    }

    private void OnKeyDown(KeyEventArgs k)
    {
        if (k.KeyCode == Keys.Enter) KeyDown?.Invoke(_txtSearch.Text);
    }

    private void OnTextChanged()
    {
        KeyDown?.Invoke(_txtSearch.Text);
    }
}