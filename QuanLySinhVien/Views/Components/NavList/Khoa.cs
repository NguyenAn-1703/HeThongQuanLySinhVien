using System.Data;
using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.NavList;

namespace QuanLySinhVien.Views.Components;

public class Khoa : NavBase
{
    // variable
    private string[] _listSelectionForComboBox = new[] { "Mã khoa", "Tên khoa" };

    private KhoaController _kcontroller;
    
    private CustomTable _table;
    private List<KhoaDto> listKhoa;
    private Panel _mainBot;

    //button
    private Button btnThem;
    private Button btnSua;
    private Button btnXoa;



    public Khoa()
    {
        _kcontroller = new KhoaController();
        Init();
        loadData();
        setActionListener();
    }

    private void Init()
    {
        Dock = DockStyle.Fill;
        Size = new Size(1200, 900);
        _mainBot = Bottom();
        Controls.Add(_mainBot);
        Controls.Add(Top());

    }

    private new Panel Top()
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

        
        mainTop.Controls.Add(btnXoa);

        return mainTop;
    }

    void setActionListener()
    {
        this._table.OnEdit += i => BtnSua_Click(i);
        this._table.OnDelete += i => BtnXoa_Click(i);
    }

    private new Panel Bottom()
    {
        Panel mainBot = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = MyColor.GrayBackGround,
            Padding = new Padding(20, 0, 20, 0)
            
        };

        
        
        
        return mainBot;
    }


    // load data function
    public void loadData()
    {
        listKhoa = _kcontroller.GetDanhSachKhoa();
        
        string[] headerArray = new string[] { "Mã khoa", "Tên khoa", "email", "Địa chỉ" };
        List<string> headerList = ConvertArray_ListString.ConvertArrayToListString(headerArray);
        var columnNames = new List<string> { "MaKhoa", "TenKhoa", "Email","DiaChi"};
            
        var cells = listKhoa.Cast<object>().ToList();
        _table = new CustomTable(headerList, columnNames,cells, true, true, true);
        _mainBot.Controls.Add(_table);
    }

    // envent
    private void BtnThem_Click(object? sender, EventArgs e)
    {
        try
        {
            // call function (controller)
            // form
            Form form = new Form()
            {
                Text = "Thêm Khoa mới",
                Size = new System.Drawing.Size(400, 300), // form lớn hơn
                StartPosition = FormStartPosition.CenterScreen // hiện giữa màn hình
            };

            // Label, TextBox
            Label lblTen = new Label() { Text = "Tên Khoa:", Top = 30, Left = 30, Width = 100 };
            TextBox txtTen = new TextBox() { Top = 30, Left = 140, Width = 200 };

            Label lblEmail = new Label() { Text = "Email:", Top = 80, Left = 30, Width = 100 };
            TextBox txtEmail = new TextBox() { Top = 80, Left = 140, Width = 200 };

            Label lblDiaChi = new Label() { Text = "Địa chỉ:", Top = 130, Left = 30, Width = 100 };
            TextBox txtDiaChi = new TextBox() { Top = 130, Left = 140, Width = 200 };

            // button
            Button btnOk = new Button()
                { Text = "OK", Top = 200, Left = 80, Width = 80, DialogResult = DialogResult.OK };
            Button btnCancel = new Button()
                { Text = "Cancel", Top = 200, Left = 200, Width = 80, DialogResult = DialogResult.Cancel };

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
            DialogResult rs = form.ShowDialog();
            if (rs == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Tên Khoa không được để trống!", "Lỗi");
                    return;
                }

                _kcontroller.ThemKhoa(txtTen.Text.Trim(), txtEmail.Text.Trim(), txtDiaChi.Text.Trim());
            }
            else if (rs == DialogResult.Cancel)
            {
                MessageBox.Show("Hủy thêm khoa mới", "Thông báo");
            }

            // reload table
            loadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi thêm khoa: " + ex.Message);
        }
    }

    // edit button
    private void BtnSua_Click(int index)
    {
        index++;
        try
        {
            // check null
            if (index <= 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa!", "Thông báo");
                return;
            }
    
            // check null
            if (index <= 0)
                throw new ArgumentException("ID khoa không hợp lệ!");
    
            // get data by id -> show in dialog
            KhoaDto khoa = _kcontroller.GetKhoaById(index);
            if (khoa == null)
                throw new Exception("Khoa không tồn tại!");
    
            // variable -> function
            string tenKhoaMoi = khoa.TenKhoa;
            string emailMoi = khoa.Email;
            string diaChiMoi = khoa.DiaChi;
    
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
                Button btnOK = new Button()
                    { Text = "OK", Location = new Point(100, 200), DialogResult = DialogResult.OK };
                Button btnCancel = new Button()
                    { Text = "Hủy", Location = new Point(200, 200), DialogResult = DialogResult.Cancel };
    
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
                    // call controller function
                    _kcontroller.SuaKhoa(index, txtTen.Text, txtEmail.Text, txtDiaChi.Text);
                }
            }
    
            // reload table
            loadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi sửa khoa: " + ex.Message);
        }
    }

    // del button
    private void BtnXoa_Click(int index)
    {
        index++;
        try
        {
            // call functon(controller) 
            // check null
            if (index <= 0)
                throw new ArgumentException("ID khoa không hợp lệ!");
    
            // Button y/n
            DialogResult rs =
                MessageBox.Show(
                    "Bạn chắc chắn muốn xóa khoa " + _kcontroller.GetKhoaById(index).TenKhoa,
                    "Cảnh báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
            if (rs == DialogResult.Yes)
            {
                // call controller function
                _kcontroller.XoaKhoa(index);
            }
            else if (rs == DialogResult.No)
            {
                MessageBox.Show("Đã hủy xóa khoa", "Thông báo");
            }
    
            // reload table
            loadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi xóa khoa: " + ex.Message);
        }
    }

    public override List<string> getComboboxList()
    {
        return ConvertArray_ListString.ConvertArrayToListString(this._listSelectionForComboBox);
    }

    public override void onSearch(string txtSearch, string filter)
    {
    }
}