using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class SearchBar: TableLayoutPanel
{
    // private TextBox _searchBox;
    private CustomCombobox Filter;

    private List<String> listSelection;

    private RoundTLP _searchFieldPanel;
    
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

        TextBox field = new TextBox();
        field.PlaceholderText = "Tìm kiếm ...";
        field.Dock = DockStyle.Fill;
        field.Font = GetFont.GetFont.GetMainFont(12,  FontType.Regular);
        field.BorderStyle = BorderStyle.None;
        field.Margin = new Padding(3, 5, 7, 3);
        
        
        _searchFieldPanel.Controls.Add(iconGlass);
        _searchFieldPanel.Controls.Add(field);

        field.Enter += (sender, args) => onEnter();
        field.Leave += (sender, args) => onLeave();

    }

    CustomCombobox getFilter()
    {
        CustomCombobox filter = new CustomCombobox(new String[]{});
        filter.Margin = new Padding(10, 35, 10, 0);
        return filter;
    }

    public void UpdateListCombobox(List<String> list)
    {
        this.listSelection = list;
        this.listSelection.Insert(0, "Tất cả");
        this.Filter.combobox.Items.Clear();
        
        foreach (string i in  listSelection)
        {
            this.Filter.combobox.Items.Add(i);
        }
        this.Filter.combobox.SelectedIndex = 0;
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
}