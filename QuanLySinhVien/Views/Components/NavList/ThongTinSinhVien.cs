using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList;

public class ThongTinSinhVien:NavBase
{
    // phần 1 thông tin sinh viên
    private Label txtMaSv;
    private Label txtTenSv;
    private Label txtNgaySinhSv;
    private Label txtGioiTinhSv;
    private Label txtSdtSv;
    private Label txtCccdSv;
    private Label txtEmailSv;
    private Label txtNoiSinhSv;
    private Label txtTinhTrangSv;
    
    // // phần 2 : thông tin khóa học
    private Label txtNganh;
    private Label txtLop;
    private Label txtKhoa;
    private Label txtBacHeDaoTao;
    private Label txtNienKhoa;
    
    // // Phần 3: Cố vấn học tập
    private Label txtTaiKhoanCv;
    private Label txtHoVaTenCv;
    private Label txtEmailCv;
    private Label txtSdtCv;
    
    private string[] _listSelectionForComboBox = new []{""};


    public ThongTinSinhVien()
    {
        InitComponent();
    }

    private void InitComponent()
    {
        Console.WriteLine("InitComponent");
         var fullTable = new TableLayoutPanel()
         {
             Dock = DockStyle.Fill,
             ColumnCount = 2,
         };
         fullTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
         fullTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        
         var leftTable = new TableLayoutPanel
         {
             Dock = DockStyle.Fill,
             RowCount = 3,
         };

         var parrentInformation1 = new TableLayoutPanel
         {
             Dock = DockStyle.Fill,
             RowCount = 2,
         };
         parrentInformation1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
         parrentInformation1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));

         var parrentInformation2 = new TableLayoutPanel
         {
             Dock = DockStyle.Fill,
             RowCount = 2,
         };
         parrentInformation2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
         parrentInformation2.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));

         var leftInformation1 = new TableLayoutPanel
         {
             ColumnCount = 3,
             Dock = DockStyle.Fill,
             BackColor = Color.Red,
         };
        
         var leftInformation2 = new TableLayoutPanel
         {
             ColumnCount = 2,
             BackColor = Color.Blue,
             Dock = DockStyle.Fill
         };
         
        
         var leftInformation3 = new TableLayoutPanel
         {   
             ColumnCount = 2,
             BackColor = Color.Green,
             Dock = DockStyle.Fill,
         };
         leftInformation1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
         leftInformation1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
         leftInformation1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
         leftInformation2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
         leftInformation2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
         leftInformation3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
         leftInformation3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
         var headerTtsv = new Label
         {
             Text = "Thông tin sinh viên",
             Font = GetFont.GetFont.GetMainFont(13 , FontType.Bold),
             Height = 70,
             Dock = DockStyle.Fill,
             ForeColor = Color.RoyalBlue,
             BackColor = Color.Aquamarine,
             TextAlign = ContentAlignment.MiddleCenter,
         };
         var Ttsv = new FlowLayoutPanel()
         {
             Dock = DockStyle.Fill,
             BackColor = Color.White,
             FlowDirection = FlowDirection.TopDown,
         };
         
         var headerTtkh = new Label
         {
             Text = "Thông tin khóa học",
             Font = GetFont.GetFont.GetMainFont(13 , FontType.Bold),
             Height = 70,
             Dock = DockStyle.Fill,
             ForeColor = Color.RoyalBlue,
             BackColor = Color.Aquamarine,
             TextAlign = ContentAlignment.MiddleCenter,
         };
         
         var Ttkh = new FlowLayoutPanel()
         {
             Dock = DockStyle.Fill,
             BackColor = Color.White,
             FlowDirection = FlowDirection.TopDown,
         };
         
         var rightTable = new TableLayoutPanel
         {
             RowCount = 3,
             Dock = DockStyle.Fill,
         };

         var rightInformation1 = new Panel
         {
             BackColor  = Color.CadetBlue,
             Dock = DockStyle.Fill,  
         };

         var rightInformation2 = new TableLayoutPanel
         {
             BackColor  = Color.AliceBlue,
             Dock = DockStyle.Fill,
         };

         var rightInformation3 = new Panel
         {
            BackColor  = Color.Cyan,
            Dock = DockStyle.Fill,
         };

         txtMaSv = JLable("Mã sinh viên");
         
         txtTenSv = JLable("Tên sinh Viên");

         txtNgaySinhSv = JLable("Ngày sinh");

         txtGioiTinhSv = JLable("Giới tính");

         txtSdtSv = JLable("Số điện thoại");

         txtCccdSv = JLable("Số CCCD");
        
         txtNoiSinhSv = JLable("Nơi sinh");

         txtTinhTrangSv = JLable("Tình trạng");

         txtNganh = JLable("Ngành");
         txtLop = JLable("Lớp");
         txtKhoa = JLable("Khoa");
         txtBacHeDaoTao = JLable("Bậc hệ đào tạo");
         txtNienKhoa = JLable("Niên khóa");
         Ttsv.Controls.Add(txtMaSv);
         Ttsv.Controls.Add(txtTenSv);
         Ttsv.Controls.Add(txtNgaySinhSv);
         Ttsv.Controls.Add(txtGioiTinhSv);
         Ttsv.Controls.Add(txtSdtSv);
         Ttsv.Controls.Add(txtCccdSv);
         Ttsv.Controls.Add(txtNoiSinhSv);
         Ttsv.Controls.Add(txtTinhTrangSv);
         
         
         Ttkh.Controls.Add(txtNganh);
         Ttkh.Controls.Add(txtLop);
         Ttkh.Controls.Add(txtKhoa);
         Ttkh.Controls.Add(txtBacHeDaoTao);
         Ttkh.Controls.Add(txtNienKhoa);
         Ttkh.Controls.Add(headerTtsv);
        
         parrentInformation1.Controls.Add(headerTtsv , 0 , 0);
         leftInformation1.Controls.Add(Ttsv , 0 , 0);
         parrentInformation1.Controls.Add(leftInformation1 , 0 , 1);
         leftInformation2.Controls.Add(Ttkh , 0 , 0);
         parrentInformation2.Controls.Add(headerTtkh , 0 , 0);
         parrentInformation2.Controls.Add(leftInformation2 , 0 , 1);
         
        
         leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
         leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
         leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
         leftTable.Controls.Add(parrentInformation1, 0, 0);
         leftTable.Controls.Add(parrentInformation2, 0, 1);
         leftTable.Controls.Add(leftInformation3, 0, 2);
         rightTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
         rightTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
         rightTable.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
         rightTable.Controls.Add(rightInformation1, 0, 0);
         rightTable.Controls.Add(rightInformation2, 0, 1);
         rightTable.Controls.Add(rightInformation3, 0, 2);
         
         
         fullTable.Controls.Add(leftTable , 0 , 0);
         fullTable.Controls.Add(rightTable , 1 , 0);
        
         Controls.Add(fullTable);
    }

    public Label JLable(string text)
    {
        Label x = new Label
        {
            Text = text,
            Width = 200,
            Font = GetFont.GetFont.GetMainFont(12f,FontType.Regular),
            Height = 30,
        };
        return x;
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }
    
    public override void onSearch(string txtSearch, string filter)
    { }
}

