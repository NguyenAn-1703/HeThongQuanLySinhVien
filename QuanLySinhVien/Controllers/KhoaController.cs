using System.Data;
using QuanLySinhVien.Models.DAO;

namespace QuanLySinhVien.Controllers
{
    public class KhoaController
    {
        private readonly KhoaDao _khoaDao;

        public KhoaController()
        {
            _khoaDao = new KhoaDao();
        }

        // get databse (all)
        public DataTable GetDanhSachKhoa()
        {
            return _khoaDao.GetAllKhoa();
        }
        
        // pop up dialog
        public void ThemKhoa(string tenKhoa, string email, string diaChi)
        {
            // check data
            if (string.IsNullOrWhiteSpace(tenKhoa))
                throw new ArgumentException("Tên khoa không được để trống!");

            _khoaDao.InsertKhoa(tenKhoa, email, diaChi);
        }

        // edit khoa
        public void SuaKhoa(int idKhoa)
        {
            // check null
            if (idKhoa <= 0)
                throw new ArgumentException("ID khoa không hợp lệ!");

            // get data by id -> show in dialog
            DataRow khoa = _khoaDao.GetKhoaById(idKhoa);
            if (khoa == null)
                throw new Exception("Khoa không tồn tại!");

            // variable -> function
            string tenKhoaMoi = khoa["TenKhoa"].ToString();
            string emailMoi = khoa["Email"].ToString();
            string diaChiMoi = khoa["DiaChi"].ToString();

            // create dialog
            using (Form inputForm = new Form())
            {
                // form
                inputForm.Text = "Sửa thông tin Khoa";
                inputForm.Size = new Size(400, 300);
                inputForm.StartPosition = FormStartPosition.CenterScreen;

                // item
                TextBox txtTen = new TextBox() { Text = tenKhoaMoi, Location = new Point(20, 20), Width = 340 };
                TextBox txtEmail = new TextBox() { Text = emailMoi, Location = new Point(20, 70), Width = 340 };
                TextBox txtDiaChi = new TextBox() { Text = diaChiMoi, Location = new Point(20, 120), Width = 340 };
                Button btnOK = new Button() { Text = "OK", Location = new Point(100, 200), DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Hủy", Location = new Point(200, 200), DialogResult = DialogResult.Cancel };

                // add item
                inputForm.Controls.Add(txtTen);
                inputForm.Controls.Add(txtEmail);
                inputForm.Controls.Add(txtDiaChi);
                inputForm.Controls.Add(btnOK);
                inputForm.Controls.Add(btnCancel);

                inputForm.AcceptButton = btnOK;
                inputForm.CancelButton = btnCancel;

                // button OK on click
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    // call DAO function
                    _khoaDao.UpdateKhoa(idKhoa, txtTen.Text, txtEmail.Text, txtDiaChi.Text);
                }
            }
        }
        
        // delete khoa
        public void XoaKhoa(int idKhoa)
        {
            // check null
            if (idKhoa <= 0)
                throw new ArgumentException("ID khoa không hợp lệ!");
            
            // call DAO function
            _khoaDao.DeleteKhoa(idKhoa);
        }
        
        // create dialog add khoa
        public void NhapKhoaMoi()
        {
            // form
            Form form = new Form()
            {
                Text = "Thêm Khoa mới",
                Size = new System.Drawing.Size(400, 300),  // form lớn hơn
                StartPosition = FormStartPosition.CenterScreen  // hiện giữa màn hình
            };

            // Label, TextBox
            Label lblTen = new Label() { Text = "Tên Khoa:", Top = 30, Left = 30, Width = 100 };
            TextBox txtTen = new TextBox() { Top = 30, Left = 140, Width = 200 };

            Label lblEmail = new Label() { Text = "Email:", Top = 80, Left = 30, Width = 100 };
            TextBox txtEmail = new TextBox() { Top = 80, Left = 140, Width = 200 };

            Label lblDiaChi = new Label() { Text = "Địa chỉ:", Top = 130, Left = 30, Width = 100 };
            TextBox txtDiaChi = new TextBox() { Top = 130, Left = 140, Width = 200 };

            // button
            Button btnOk = new Button() { Text = "OK", Top = 200, Left = 80, Width = 80, DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Text = "Cancel", Top = 200, Left = 200, Width = 80, DialogResult = DialogResult.Cancel };

            // add
            form.Controls.Add(lblTen);
            form.Controls.Add(txtTen);
            form.Controls.Add(lblEmail);
            form.Controls.Add(txtEmail);
            form.Controls.Add(lblDiaChi);
            form.Controls.Add(txtDiaChi);
            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);

            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            // check OK, -> call function
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Tên Khoa không được để trống!", "Lỗi");
                    return;
                }

                ThemKhoa(txtTen.Text.Trim(), txtEmail.Text.Trim(), txtDiaChi.Text.Trim());
            }
        }

    }
}