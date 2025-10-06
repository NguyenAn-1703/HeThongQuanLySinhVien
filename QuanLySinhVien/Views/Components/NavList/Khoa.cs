using System.Data;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components
{
    public class Khoa : NavBase
    {
        // variable
        private string[] _listSelectionForComboBox = new[] { "Mã khoa", "Tên khoa" };

        private DataTable table = new DataTable();
        private CUse cuse;
        private KhoaController _kcontroller;
        private DataGridView dgv;

        //button
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        
        // default
        private int selectedMaKhoa = -1; // mặc định chưa chọn


        public Khoa()
        {
            _kcontroller = new KhoaController();
            cuse = new CUse();
            Init();
        }

        private void Init()
        {
            Dock = DockStyle.Fill;
            Size = new Size(1200, 900);

            var borderTop = new Panel
            {
                Dock = DockStyle.Fill
            };

            borderTop.Controls.Add(Bottom());
            borderTop.Controls.Add(Top());

            Controls.Add(borderTop);
        }

        private Panel Top()
        {
            Panel mainTop = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = MyColor.GrayBackGround,
                Height = 90
            };

            // edit button
            btnThem = new Button
            {
                Text = "Thêm khoa",
                Size = new Size(120, 40),
                Location = new Point(30, 25)
            };
            btnThem.Click += BtnThem_Click;
            mainTop.Controls.Add(btnThem);

            btnSua = new Button
            {
                Text = "Sửa khoa",
                Size = new Size(120, 40),
                Location = new Point(170, 25)
            };
            btnSua.Click += BtnSua_Click;
            mainTop.Controls.Add(btnSua);

            btnXoa = new Button
            {
                Text = "Xóa khoa",
                Size = new Size(120, 40),
                Location = new Point(310, 25)
            };
            btnXoa.Click += BtnXoa_Click;
            mainTop.Controls.Add(btnXoa);

            return mainTop;
        }

        private Panel Bottom()
        {
            Panel mainBot = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = MyColor.GrayBackGround,
                Padding = new Padding(20, 0, 20, 0)
            };

            // edit dgv
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = MyColor.GrayBackGround,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                CellBorderStyle = DataGridViewCellBorderStyle.None,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            
            // Header
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = MyColor.MediumBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // sọc
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // add columns -> dataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã khoa", typeof(int));
            dt.Columns.Add("Tên khoa", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Địa chỉ", typeof(string));

            // get all data
            dt = _kcontroller.GetDanhSachKhoa();
            
            // debug
            foreach (DataColumn col in dt.Columns)
            {
                Console.WriteLine(col.ColumnName);
            }

            // test table
            // for (int i = 0; i < 50; i++)
            // {
            //     dt.Rows.Add(i, "Khoa" + i, "email" + i + "@gmail.com", "Địa chỉ " + i);
            // }

            dgv.DataSource = dt;

            // event click in row
            dgv.CellClick += Dgv_CellClick!;

            mainBot.Controls.Add(dgv);
            return mainBot;
        }
        

        // load data function
        public void loadData()
        {
            dgv.DataSource = _kcontroller.GetDanhSachKhoa();
        }

        public override List<string> getComboboxList()
        {
            return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
        }

        // envent
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                // call function (controller)
                _kcontroller.NhapKhoaMoi(); 
                
                // reload table
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khoa: " + ex.Message);
            }
        }

        // edit button
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            try
            {
                // check null
                if (selectedMaKhoa <= 0)
                {
                    MessageBox.Show("Vui lòng chọn dòng cần sửa!", "Thông báo");
                    return;
                }

                // call function(controller)
                _kcontroller.SuaKhoa(selectedMaKhoa);
                
                // reload table
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa khoa: " + ex.Message);
            }
        }
        
        // del button
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            try
            {
                // call functon(controller) 
                _kcontroller.XoaKhoa(selectedMaKhoa);
                
                // reload table
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa khoa: " + ex.Message);
            }
        }
        
        // get id after click
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // check header or null
            if (e.RowIndex >= 0)
            {
                // get id by function
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                selectedMaKhoa = Convert.ToInt32(row.Cells["Mã khoa"].Value);
            }
        }
    }
}
