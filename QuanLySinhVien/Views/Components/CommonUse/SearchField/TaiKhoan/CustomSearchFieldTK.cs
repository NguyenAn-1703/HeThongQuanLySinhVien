using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.CommonUse.SearchField.TaiKhoan;

public class CustomSearchFieldTK : CustomTextBox
{
    CustomPopup _popup;

    private Form _parent;

    private List<TaiKhoanDto> listHP;
    private TaiKhoanController _TaiKhoanController;

    private List<string> _columnNames;
    private List<object> _cellDatas;
    private List<object> displayDatas;

    private TaiKhoanDto selectedHP;

    public CustomSearchFieldTK()
    {
        _parent = new Form();
        _TaiKhoanController = TaiKhoanController.getInstance();
        _popup = new CustomPopup();
        Init();
    }

    void Init()
    {
        listHP = _TaiKhoanController.GetTaiKhoanNotUsed();
        SetData();

        SetAction();
    }

    void SetAction()
    {
        this.contentTextBox.TextChanged += (sender, args) => OnTextChanged();
        this.contentTextBox.KeyDown += (sender, args) => OnKeyDown(args);
        this._popup.KeyEnter += (index) => OnKeyEnter(index);
    }

    void OnKeyEnter(int index)
    {
        
        selectedHP = _TaiKhoanController.GetTaiKhoanById(index);
        contentTextBox.Text = selectedHP.TenDangNhap;
        contentTextBox.Focus();
        contentTextBox.SelectAll();
        _popup.Visible = false;
    }

    void OnKeyDown(KeyEventArgs e)
    {
        //chọn trong list
        if (e.KeyCode == Keys.Down)
        {
            if (_popup.Visible == true && !contentTextBox.Text.Equals(""))
            {
                _popup._dt.Focus();
            }
        }

        //chọn trực tiếp
        if (e.KeyCode == Keys.Enter && _popup.Visible == true && !contentTextBox.Text.Equals(""))
        {
            if (_popup._dt.SelectedRows[0].Index == 0)
            {
                int index = (int)_popup._dt.SelectedRows[0].Cells[0].Value;
                OnKeyEnter(index);
                _popup.Visible = false;
            }
        }
    }

    void OnTextChanged()
    {
        if (contentTextBox.Focused)
        {
            if (_parent != FindForm())
            {
                SetupLocation();
            }

            string keyword = contentTextBox.Text.Trim().ToLower();

            if (keyword.Equals(""))
            {
                _popup.Visible = false;
                return;
            }
        

            List<TaiKhoanDto> searchList = listHP.Where(x =>
                x.MaTK.ToString().ToLower().Contains(keyword) ||
                x.TenDangNhap.ToLower().Contains(keyword)
            ).ToList();
            displayDatas = ConvertObject.ConvertToDisplay(searchList, x => new
            {
                MaTK = x.MaTK,
                TenDangNhap = x.TenDangNhap
            });
            _popup.UpdateData(displayDatas);

            if (searchList.Count == 0)
            {
                _popup.Visible = false;
            }
            else
            {
                _popup.Visible = true;
            }
        }
        
    }

    public void SetData()
    {
        string[] arr = new[] { "MaTK", "TenDangNhap" };
        _columnNames = arr.ToList();
        displayDatas = ConvertObject.ConvertToDisplay(listHP, x => new
        {
            MaTK = x.MaTK,
            TenDangNhap = x.TenDangNhap
        });
        _popup.SetData(_columnNames, displayDatas);
    }

    public void SetupLocation()
    {
        _parent = FindForm();

        Point screenPoint = this.PointToScreen(new Point(0, this.Height));
        Point location = _parent.PointToClient(screenPoint);

        _popup.Location = location;

        _popup.Width = this.Width;

        _parent.Controls.Add(_popup);
        _popup.BringToFront();

        Console.WriteLine(location);
    }
}