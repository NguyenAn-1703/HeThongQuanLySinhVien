// File: Views/Components/NavList/Dialog/PhongHocDialog.cs
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;
using System.Windows.Forms;

namespace QuanLySinhVien.Views.Components.NavList.Dialog
{
    public class PhongHocDialog : CustomDialog
    {
        // Các control được public để lớp View có thể lấy dữ liệu
        public TextBox TxtTenPH { get; private set; }
        public TextBox TxtLoaiPH { get; private set; }
        public TextBox TxtCoSo { get; private set; }
        public NumericUpDown NumSucChua { get; private set; }
        public TextBox TxtTinhTrang { get; private set; }
        
        // DTO để lưu trữ dữ liệu (cho cả thêm mới và cập nhật)
        private PhongHocDto _currentData;

        public PhongHocDialog(string title, DialogType dialogType) : base(title, dialogType, 400, 450)
        {
            _currentData = new PhongHocDto();
            InitializePhongHocControls();

            _btnLuu.Click += (sender, e) =>
            {
                if (ValidateInput())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };
        }
        
        public void LoadData(PhongHocDto phongHoc)
        {
            if (phongHoc != null)
            {
                _currentData = phongHoc; // Lưu lại dữ liệu gốc để lấy ID khi cập nhật
                TxtTenPH.Text = phongHoc.TenPH;
                TxtLoaiPH.Text = phongHoc.LoaiPH;
                TxtCoSo.Text = phongHoc.CoSo;
                NumSucChua.Value = phongHoc.SucChua;
                TxtTinhTrang.Text = phongHoc.TinhTrang;
            }
        }
        
        // Lấy kết quả từ dialog
        public PhongHocDto GetResult()
        {
            _currentData.TenPH = TxtTenPH.Text.Trim();
            _currentData.LoaiPH = TxtLoaiPH.Text.Trim();
            _currentData.CoSo = TxtCoSo.Text.Trim();
            _currentData.SucChua = (int)NumSucChua.Value;
            _currentData.TinhTrang = TxtTinhTrang.Text.Trim();
            return _currentData;
        }

        private void InitializePhongHocControls()
        {
            _textBoxsContainer.RowCount = 5;
            _textBoxsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            _textBoxsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _textBoxsContainer.Padding = new Padding(20);

            // Khởi tạo các control
            TxtTenPH = new TextBox { Dock = DockStyle.Fill };
            TxtLoaiPH = new TextBox { Dock = DockStyle.Fill };
            TxtCoSo = new TextBox { Dock = DockStyle.Fill };
            NumSucChua = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 500 };
            TxtTinhTrang = new TextBox { Dock = DockStyle.Fill };

            // Thêm vào layout
            _textBoxsContainer.Controls.Add(new Label { Text = "Tên phòng:", Anchor = AnchorStyles.Left, Dock = DockStyle.Fill }, 0, 0);
            _textBoxsContainer.Controls.Add(TxtTenPH, 1, 0);
            _textBoxsContainer.Controls.Add(new Label { Text = "Loại phòng:", Anchor = AnchorStyles.Left, Dock = DockStyle.Fill }, 0, 1);
            _textBoxsContainer.Controls.Add(TxtLoaiPH, 1, 1);
            _textBoxsContainer.Controls.Add(new Label { Text = "Cơ sở:", Anchor = AnchorStyles.Left, Dock = DockStyle.Fill }, 0, 2);
            _textBoxsContainer.Controls.Add(TxtCoSo, 1, 2);
            _textBoxsContainer.Controls.Add(new Label { Text = "Sức chứa:", Anchor = AnchorStyles.Left, Dock = DockStyle.Fill }, 0, 3);
            _textBoxsContainer.Controls.Add(NumSucChua, 1, 3);
            _textBoxsContainer.Controls.Add(new Label { Text = "Tình trạng:", Anchor = AnchorStyles.Left, Dock = DockStyle.Fill }, 0, 4);
            _textBoxsContainer.Controls.Add(TxtTinhTrang, 1, 4);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TxtTenPH.Text))
            {
                MessageBox.Show("Tên phòng học không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TxtTenPH.Focus();
                return false;
            }
            return true;
        }
    }
}