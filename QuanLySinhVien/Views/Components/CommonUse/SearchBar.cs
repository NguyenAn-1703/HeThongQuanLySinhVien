using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class SearchBar: TableLayoutPanel
{
    // private TextBox _searchBox;
    private CustomCombobox Filter;

    private List<string> listSelection;

    private RoundTLP _searchFieldPanel;
    private TextBox _field;
    
    public event Action<string, string> KeyDown;
    
    public SearchBar()
    {
        listSelection = new List<string>();
        Init();
    }

    void Init()
    {
        this.ColumnCount = 2;
        this.Dock = DockStyle.Fill;
        // Giới hạn chiều rộng tối đa 700px, chiều cao không giới hạn
        this.MaximumSize = new Size(700, 0);
        
        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        
        // this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        SetSearchField();

        this.Filter = getFilter();

        this.Controls.Add(_searchFieldPanel);
        
        this.Controls.Add(Filter);
        
    }

    void SetSearchField()
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
        
        // _searchFieldPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        

        PictureBox iconGlass = new PictureBox
        {
            Image = GetSvgBitmap.GetBitmap("search.svg"),
            Size = new Size(30, 30),
            SizeMode = PictureBoxSizeMode.Zoom,
            Margin = new Padding(20, 5, 20, 5),
        };

        _field = new TextBox();
        _field.PlaceholderText = "Tìm kiếm ...";
        _field.Dock = DockStyle.Fill;
        _field.Font = GetFont.GetFont.GetMainFont(12,  FontType.Regular);
        _field.BorderStyle = BorderStyle.None;
        _field.Margin = new Padding(3, 5, 7, 3);
        
        
        _searchFieldPanel.Controls.Add(iconGlass);
        _searchFieldPanel.Controls.Add(_field);

        _field.Enter += (sender, args) => onEnter();
        _field.Leave += (sender, args) => onLeave();
        _field.KeyDown += (sender, args) => OnKeyDown(args);
        _field.TextChanged += (sender, args) => OnTextChanged();
    }

    CustomCombobox getFilter()
    {
        CustomCombobox filter = new CustomCombobox(new string[]{});
        filter.Margin = new Padding(10, 35, 10, 0);
        return filter;
    }

    public void UpdateListCombobox(List<string> list)
    {
        this.listSelection = list;
        this.listSelection.Insert(0, "Tất cả");
        this.Filter.combobox.Items.Clear();
        
        foreach (string i in  listSelection)
        {
            this.Filter.combobox.Items.Add(i);
        }
        this.Filter.combobox.SelectedIndex = 0;
        Filter.combobox.SelectedIndexChanged += (sender, args) => OnChangeItem();
    }

    void onEnter()
    {
        this._searchFieldPanel.BorderColor = MyColor.MainColor;
        this._searchFieldPanel.Invalidate();
    }

    void onLeave()
    {
        this._searchFieldPanel.BorderColor = MyColor.GraySelectColor;
        this._searchFieldPanel.Invalidate();
    }

    void OnKeyDown(KeyEventArgs k)
    {
        if (k.KeyCode == Keys.Enter)
        {
            KeyDown.Invoke(_field.Text, Filter.combobox.SelectedItem + " ");
        }
    }

    void OnChangeItem()
    {
        KeyDown.Invoke(_field.Text, Filter.combobox.SelectedItem + " ");
    }

    void OnTextChanged()
    {
        KeyDown.Invoke(_field.Text, Filter.combobox.SelectedItem + " ");
    }
}