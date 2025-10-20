using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;
using System.Windows.Forms;

namespace QuanLySinhVien.Views.Components.NavList.Dialog
{
    public class PhongHocDialog : CustomDialog
    {
        public TextBox TxtTenPH { get; private set; }
        public TextBox TxtLoaiPH { get; private set; }
        public TextBox TxtCoSo { get; private set; }
        public NumericUpDown NumSucChua { get; private set; }
        public TextBox TxtTinhTrang { get; private set; }
 
        private PhongHocDto _currentData;
        private DialogType _dialogType;

        // Constructor
        public PhongHocDialog(string title, DialogType dialogType) : base(title, dialogType, 400, 450)
        {
            _dialogType = dialogType;
            _currentData = new PhongHocDto();
            InitializePhongHocControls();

            if (dialogType != DialogType.ChiTiet)
            {
                _btnLuu._mouseDown += () =>
                {
                    if (ValidateInput())
                    {
                        _currentData.TenPH = TxtTenPH.Text.Trim();
                        _currentData.LoaiPH = TxtLoaiPH.Text.Trim();
                        _currentData.CoSo = TxtCoSo.Text.Trim();
                        _currentData.SucChua = (int)NumSucChua.Value;
                        _currentData.TinhTrang = TxtTinhTrang.Text.Trim();
                        
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                };
            }
            
            if (_dialogType == DialogType.ChiTiet)
            {
                SetReadOnly();
            }
        }
        
        public void LoadData(PhongHocDto phongHoc)
        {
            if (phongHoc != null)
            {
                _currentData = phongHoc;
                TxtTenPH.Text = phongHoc.TenPH;
                TxtLoaiPH.Text = phongHoc.LoaiPH;
                TxtCoSo.Text = phongHoc.CoSo;
                NumSucChua.Value = phongHoc.SucChua;
                TxtTinhTrang.Text = phongHoc.TinhTrang;
            }
        }

        // ngăn sửa khi xem chi tiết
        private void SetReadOnly()
        {
            TxtTenPH.Enabled = false;
            TxtLoaiPH.Enabled = false;
            TxtCoSo.Enabled = false;
            NumSucChua.Enabled = false;
            TxtTinhTrang.Enabled = false;
        }
        
        // lấy kết quả
        public PhongHocDto GetResult()
        {
            return _currentData;
        }

        // init
        private void InitializePhongHocControls()
        {
            _textBoxsContainer.RowCount = 5;
            _textBoxsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            _textBoxsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _textBoxsContainer.Padding = new Padding(20);

            TxtTenPH = new TextBox { Dock = DockStyle.Fill };
            TxtLoaiPH = new TextBox { Dock = DockStyle.Fill };
            TxtCoSo = new TextBox { Dock = DockStyle.Fill };
            NumSucChua = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 500 };
            TxtTinhTrang = new TextBox { Dock = DockStyle.Fill };

            // layout
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

        // check value trống
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