using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Views.Components.CommonUse.SearchField.TaiKhoan;

public class CustomSearchFieldTK : CustomTextBox
{
    private readonly CustomPopup _popup;
    private List<object> _cellDatas;

    private List<string> _columnNames;

    private Form _parent;
    private TaiKhoanController _TaiKhoanController;
    private List<object> displayDatas;

    private List<TaiKhoanDto> listHP;

    private TaiKhoanDto selectedHP;

    public CustomSearchFieldTK()
    {
        _parent = new Form();
        _TaiKhoanController = TaiKhoanController.getInstance();
        _popup = new CustomPopup();
        Init();
    }

    private void Init()
    {
        listHP = _TaiKhoanController.GetTaiKhoanNotUsed();
        SetData();

        SetAction();
    }

    private void SetAction()
    {
        contentTextBox.TextChanged += (sender, args) => OnTextChanged();
        contentTextBox.KeyDown += (sender, args) => OnKeyDown(args);
        _popup.KeyEnter += index => OnKeyEnter(index);
    }

    private void OnKeyEnter(int index)
    {
        selectedHP = _TaiKhoanController.GetTaiKhoanById(index);
        contentTextBox.Text = selectedHP.TenDangNhap;
        contentTextBox.Focus();
        contentTextBox.SelectAll();
        _popup.Visible = false;
    }

    private void OnKeyDown(KeyEventArgs e)
    {
        //chọn trong list
        if (e.KeyCode == Keys.Down)
            if (_popup.Visible && !contentTextBox.Text.Equals(""))
                _popup._dt.Focus();

        //chọn trực tiếp
        if (e.KeyCode == Keys.Enter && _popup.Visible && !contentTextBox.Text.Equals(""))
            if (_popup._dt.SelectedRows[0].Index == 0)
            {
                var index = (int)_popup._dt.SelectedRows[0].Cells[0].Value;
                OnKeyEnter(index);
                _popup.Visible = false;
            }
    }

    private void OnTextChanged()
    {
        if (contentTextBox.Focused)
        {
            if (_parent != FindForm()) SetupLocation();

            var keyword = contentTextBox.Text.Trim().ToLower();

            if (keyword.Equals(""))
            {
                _popup.Visible = false;
                return;
            }


            var searchList = listHP.Where(x =>
                x.MaTK.ToString().ToLower().Contains(keyword) ||
                x.TenDangNhap.ToLower().Contains(keyword)
            ).ToList();
            displayDatas = ConvertObject.ConvertToDisplay(searchList, x => new
            {
                x.MaTK, x.TenDangNhap
            });
            _popup.UpdateData(displayDatas);

            if (searchList.Count == 0)
                _popup.Visible = false;
            else
                _popup.Visible = true;
        }
    }

    public void SetData()
    {
        var arr = new[] { "MaTK", "TenDangNhap" };
        _columnNames = arr.ToList();
        displayDatas = ConvertObject.ConvertToDisplay(listHP, x => new
        {
            x.MaTK, x.TenDangNhap
        });
        _popup.SetData(_columnNames, displayDatas);
    }

    public void SetupLocation()
    {
        _parent = FindForm();

        var screenPoint = PointToScreen(new Point(0, Height));
        var location = _parent.PointToClient(screenPoint);

        _popup.Location = location;

        _popup.Width = Width;

        _parent.Controls.Add(_popup);
        _popup.BringToFront();

        Console.WriteLine(location);
    }
}