using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class SearchBar: TableLayoutPanel
{
    // private TextBox _searchBox;
    private ComboBox Filter;

    private List<String> listSelection;
    
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
        
        this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

        RoundTLP searchField = getSearchField();

        this.Filter = getFilter();
        
        this.Controls.Add(searchField);
        
        this.Controls.Add(Filter);
        
    }

    RoundTLP getSearchField()
    {
        RoundTLP searchFieldPanel = new RoundTLP
        {
            BackColor = MyColor.White,
            ColumnCount = 2,
            Dock = DockStyle.Top,
            AutoSize = true,
            Margin = new Padding(10, 25, 10, 0),
            BorderRadius = 30
        };
        searchFieldPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        searchFieldPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        
        // searchFieldPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        

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
        field.Margin = new Padding(3, 5, 3, 3);
        
        
        searchFieldPanel.Controls.Add(iconGlass);
        searchFieldPanel.Controls.Add(field);
        return searchFieldPanel;

    }

    ComboBox getFilter()
    {
        ComboBox filter = new ComboBox();
        filter.Margin = new Padding(10, 35, 10, 0);
        return filter;
    }

    public void UpdateListCombobox(List<String> list)
    {
        this.listSelection = list;
        this.Filter.Items.Clear();
        
        foreach (string i in  listSelection)
        {
            this.Filter.Items.Add(i);
        }
    }
}