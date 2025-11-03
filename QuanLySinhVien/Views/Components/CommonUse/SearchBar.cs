using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class SearchBar : MyTLP
{
    private TextBox _field;

    private RoundTLP _searchFieldPanel;

    // private TextBox _searchBox;
    private CustomCombobox Filter;

    private List<string> listSelection;

    public SearchBar()
    {
        listSelection = new List<string>();
        Init();
    }

    public event Action<string, string> KeyDown;

    private void Init()
    {
        ColumnCount = 2;
        Dock = DockStyle.Fill;
        // Giới hạn chiều rộng tối đa 700px, chiều cao không giới hạn
        MaximumSize = new Size(700, 0);

        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        // this.CellBorderStyle = MyTLPCellBorderStyle.Single;
        SetSearchField();

        Filter = getFilter();

        Controls.Add(_searchFieldPanel);

        Controls.Add(Filter);
    }

    private void SetSearchField()
    {
        _searchFieldPanel = new RoundTLP
        {
            BackColor = MyColor.White,
            ColumnCount = 2,
            Dock = DockStyle.Top,
            AutoSize = true,
            Margin = new Padding(10, 25, 10, 0),
            BorderRadius = 30,
            Border = true
        };
        _searchFieldPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _searchFieldPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // _searchFieldPanel.CellBorderStyle = MyTLPCellBorderStyle.Single;


        var iconGlass = new PictureBox
        {
            Image = GetSvgBitmap.GetBitmap("search.svg"),
            Size = new Size(30, 30),
            SizeMode = PictureBoxSizeMode.Zoom,
            Margin = new Padding(20, 5, 20, 5)
        };

        _field = new TextBox();
        _field.PlaceholderText = "Tìm kiếm ...";
        _field.Dock = DockStyle.Fill;
        _field.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
        _field.BorderStyle = BorderStyle.None;
        _field.Margin = new Padding(3, 5, 7, 3);


        _searchFieldPanel.Controls.Add(iconGlass);
        _searchFieldPanel.Controls.Add(_field);

        _field.Enter += (sender, args) => onEnter();
        _field.Leave += (sender, args) => onLeave();
        _field.KeyDown += (sender, args) => OnKeyDown(args);
        _field.TextChanged += (sender, args) => OnTextChanged();
    }

    private CustomCombobox getFilter()
    {
        var filter = new CustomCombobox(new string[] { });
        filter.combobox.Font = GetFont.GetFont.GetMainFont(9, FontType.Regular);
        filter.Anchor = AnchorStyles.None;
        return filter;
    }

    public void UpdateListCombobox(List<string> list)
    {
        listSelection = list;
        listSelection.Insert(0, "Tất cả");
        Filter.combobox.Items.Clear();

        foreach (var i in listSelection) Filter.combobox.Items.Add(i);
        Filter.combobox.SelectedIndex = 0;
        Filter.combobox.SelectedIndexChanged += (sender, args) => OnChangeItem();
    }

    private void onEnter()
    {
        _searchFieldPanel.BorderColor = MyColor.MainColor;
        _searchFieldPanel.Invalidate();
    }

    private void onLeave()
    {
        _searchFieldPanel.BorderColor = MyColor.GraySelectColor;
        _searchFieldPanel.Invalidate();
    }

    private void OnKeyDown(KeyEventArgs k)
    {
        if (k.KeyCode == Keys.Enter) KeyDown.Invoke(_field.Text, Filter.combobox.SelectedItem + " ");
    }

    private void OnChangeItem()
    {
        KeyDown.Invoke(_field.Text, Filter.combobox.SelectedItem + " ");
    }

    private void OnTextChanged()
    {
        KeyDown.Invoke(_field.Text, Filter.combobox.SelectedItem + " ");
    }
}