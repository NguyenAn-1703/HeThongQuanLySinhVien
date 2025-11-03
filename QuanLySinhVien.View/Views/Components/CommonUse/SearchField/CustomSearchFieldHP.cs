using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class CustomSearchFieldHP : CustomTextBox
{
    private readonly CustomPopup _popup;

    private readonly List<HocPhanDto> listHP;
    private List<object> _cellDatas;

    private List<string> _columnNames;
    private HocPhanController _hocPhanController;

    private Form _parent;
    private List<object> displayDatas;

    private HocPhanDto selectedHP;

    public CustomSearchFieldHP()
    {
        _parent = new Form();
        _hocPhanController = HocPhanController.GetInstance();
        _popup = new CustomPopup();
        listHP = _hocPhanController.GetAll();
        Init();
    }

    private void Init()
    {
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
        selectedHP = _hocPhanController.GetHocPhanById(index);
        contentTextBox.Text = selectedHP.TenHP;
        contentTextBox.Focus();
        contentTextBox.SelectAll();
        _popup.Visible = false;
    }

    private void OnKeyDown(KeyEventArgs e)
    {
        //chọn trong list
        if (e.KeyCode == Keys.Down && !contentTextBox.Text.Equals(""))
            if (_popup.Visible)
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
                x.MaHP.ToString().ToLower().Contains(keyword) ||
                x.TenHP.ToLower().Contains(keyword)
            ).ToList();
            displayDatas = ConvertObject.ConvertToDisplay(searchList, x => new
            {
                x.MaHP, x.TenHP
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
        var arr = new[] { "MaHP", "TenHP" };
        _columnNames = arr.ToList();
        displayDatas = ConvertObject.ConvertToDisplay(listHP, x => new
        {
            x.MaHP, x.TenHP
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
    }
}