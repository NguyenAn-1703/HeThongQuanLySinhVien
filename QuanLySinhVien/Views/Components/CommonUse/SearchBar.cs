using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class SearchBar: TableLayoutPanel
{
    private TextBox _searchBox;
    private ComboBox Filter;
    
    public SearchBar()
    {
        Init();
    }

    void Init()
    {
        this.ColumnCount = 2;
        this.AutoSize = true;
        
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


        this._searchBox = new TextBox();
        this._searchBox.Width = 100;
        this._searchBox.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);

        this.Filter = new ComboBox();
        
        this.Controls.Add(_searchBox);
        this.Controls.Add(Filter);


    }
}