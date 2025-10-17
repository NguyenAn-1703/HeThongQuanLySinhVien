using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse.Search;

public class CtdtSearchBar : RoundTLP
{
    TextBox _txtSearch;
    public event Action<string> KeyDown;
    public int TxtWidth { get; set; } = 200;
    
    public CtdtSearchBar()
    {
        _txtSearch = new TextBox();
        Init();
    }

    void Init()
    {
        this.Padding = new Padding(3);
        this.AutoSize = true;
        this.Border = true;
        this.ColumnCount = 2;
        
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        
        _txtSearch.Width = TxtWidth;
        _txtSearch.AutoSize = true;
        _txtSearch.Font = GetFont.GetFont.GetMainFont(11, FontType.Regular);
        _txtSearch.PlaceholderText = "Tìm kiếm ...";
        _txtSearch.BorderStyle = BorderStyle.None;

        PictureBox glassIcon = GetPb();
        this.Controls.Add(glassIcon);
        this.Controls.Add(_txtSearch);
        
        _txtSearch.Enter += (sender, args) => onEnter();
        _txtSearch.Leave += (sender, args) => onLeave();
        _txtSearch.KeyDown += (sender, args) => OnKeyDown(args);
        _txtSearch.TextChanged += (sender, args) => OnTextChanged();
        
        
    }

    PictureBox GetPb()
    {
        PictureBox pb = new PictureBox
        {
            Image = GetSvgBitmap.GetBitmap("search.svg"),
            Size = new Size(20, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Anchor = AnchorStyles.None
        };
        return pb;
    }
    
    void onEnter()
    {
        this.BorderColor = MyColor.MainColor;
        this.Invalidate();
    }

    void onLeave()
    {
        this.BorderColor = MyColor.GraySelectColor;
        this.Invalidate();
    }

    void OnKeyDown(KeyEventArgs k)
    {
        if (k.KeyCode == Keys.Enter)
        {
            KeyDown?.Invoke(_txtSearch.Text);
        }
    }

    void OnTextChanged()
    {
        KeyDown?.Invoke(_txtSearch.Text);
    }
}