using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList.Dialog
{
    public class KhoaDialog : CustomDialog
    {
        public TextBox TxtTenKhoa { get; private set; }
        public TextBox TxtEmail { get; private set; }
        public TextBox TxtDiaChi { get; private set; }

        public KhoaDialog(string title, DialogType dialogType) : base(title, dialogType, 400, 350)
        {
            // init
            InitializeKhoaControls();
            
            // QUAN TRỌNG: Dùng _mouseDown delegate thay vì Click event
            if (dialogType != DialogType.ChiTiet)
            {
                _btnLuu._mouseDown += () =>
                {
                    if (ValidateInput())
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                };
            }
            
            if (dialogType == DialogType.ChiTiet)
            {
                // Vô hiệu hóa các ô nhập liệu
                TxtTenKhoa.Enabled = false;
                TxtEmail.Enabled = false;
                TxtDiaChi.Enabled = false;
            }
        }
        
        public void LoadData(KhoaDto khoa)
        {
            if (khoa != null)
            {
                TxtTenKhoa.Text = khoa.TenKhoa;
                TxtEmail.Text = khoa.Email;
                TxtDiaChi.Text = khoa.DiaChi;
            }
        }
        
        private void InitializeKhoaControls()
        {
            _textBoxsContainer.RowCount = 3;
            _textBoxsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            _textBoxsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _textBoxsContainer.Padding = new Padding(20);

            // Tên khoa
            Label lblTen = new Label
            {
                Text = "Tên Khoa:",
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill
            };
            TxtTenKhoa = new TextBox { Dock = DockStyle.Fill };
            _textBoxsContainer.Controls.Add(lblTen, 0, 0);
            _textBoxsContainer.Controls.Add(TxtTenKhoa, 1, 0);

            // Email
            Label lblEmail = new Label
            {
                Text = "Email:",
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill
            };
            TxtEmail = new TextBox
            {
                Dock = DockStyle.Fill
            };
            _textBoxsContainer.Controls.Add(lblEmail, 0, 1);
            _textBoxsContainer.Controls.Add(TxtEmail, 1, 1);

            // Địa chỉ
            Label lblDiaChi = new Label
            {
                Text = "Địa chỉ:",
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill
            };
            TxtDiaChi = new TextBox { Dock = DockStyle.Fill };
            _textBoxsContainer.Controls.Add(lblDiaChi, 0, 2);
            _textBoxsContainer.Controls.Add(TxtDiaChi, 1, 2);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TxtTenKhoa.Text))
            {
                MessageBox.Show("Tên khoa không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TxtTenKhoa.Focus();
                return false;
            }

            return true;
        }
    }
}