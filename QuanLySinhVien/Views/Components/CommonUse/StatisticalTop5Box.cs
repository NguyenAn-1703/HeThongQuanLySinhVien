using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Views.Components.ViewComponents;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class StatisticalTop5Box : RoundTLP
{
    private readonly float[] _listPhanTram;
    private readonly string[] _listTen;
    private readonly Label _title;

    public StatisticalTop5Box(string title, string[] dsTen, float[] dsPhanTram)
    {
        _title = new Label();
        _title.Text = title;
        _listTen = dsTen;
        _listPhanTram = dsPhanTram;
        Init();
    }

    private void Init()
    {
        BackColor = MyColor.White;
        Dock = DockStyle.Fill;
        AutoSize = true;
        Margin = new Padding(5);
        ColumnCount = 2;
        RowCount = 6;

        _title.AutoSize = true;
        _title.Font = GetFont.GetFont.GetMainFont(11, FontType.Bold);

        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Controls.Add(_title);
        SetColumnSpan(_title, 2);

        for (var i = 0; i < _listPhanTram.Length; i++)
        {
            var lbl1 = new Label();
            var lbl2 = new Label();
            lbl1.AutoSize = true;
            lbl2.AutoSize = true;

            lbl1.Text = _listTen[i];
            lbl2.Text = _listPhanTram[i] + "%";

            lbl1.Font = GetFont.GetFont.GetMainFont(10, FontType.Regular);
            lbl2.Font = GetFont.GetFont.GetMainFont(10, FontType.Regular);


            Controls.Add(lbl1);
            Controls.Add(lbl2);
        }

        // this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
    }
}