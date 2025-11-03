// using QuanLySinhVien.View.Views.Components.CommonUse;
// using QuanLySinhVien.View.Views.Enums;
//
// namespace QuanLySinhVien.View.Views.Components;
//
// public class CustomListBox : TableLayoutPanel
// {
//     CustomTextBox txt;
//     ListBox listBox;
//     private List<string> _listData;
//
//     private int _itemHeight;
//     
//     public CustomListBox()
//     {
//         txt = new CustomTextBox();
//         listBox = new ListBox();
//         _listData = new List<string>();
//         Init();
//     }
//
//     void Init()
//     {
//         RowCount = 2;
//         AutoSize = true;
//         
//         RowStyles.Add(new RowStyle(SizeType.AutoSize));
//         RowStyles.Add(new RowStyle(SizeType.AutoSize));
//         
//         CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
//         
//         txt.Dock = DockStyle.Fill;
//
//         
//         listBox.Font = GetFont.GetFont.GetMainFont(12, FontType.Regular);
//         listBox.AutoSize = true;
//         _itemHeight = listBox.Height;
//
//         
//         this.Controls.Add(txt);
//         this.Controls.Add(listBox);
//         
//
//         SetAction();
//     }
//
//     void SetAction()
//     {
//         txt.contentTextBox.TextChanged  += (sender, args) => UpdateList();
//     }
//
//     void UpdateList()
//     {
//         
//         string text = txt.contentTextBox.Text;
//         Console.WriteLine(text);
//         
//         List<string> newList = _listData.Where(x => x.Contains(text)).ToList();
//         
//         listBox.Items.Clear();
//         listBox.Items.AddRange(newList.ToArray());
//         
//         listBox.Height = _itemHeight *  listBox.Items.Count;
//     }
//
//     public void SetList(List<string> listItem)
//     {
//         _listData = listItem;
//     }
//     
//     
// }
//
//
//

