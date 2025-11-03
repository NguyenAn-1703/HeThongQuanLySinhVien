using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Shared;
using QuanLySinhVien.Shared.DTO;

namespace QuanLySinhVien.Views.Components.CommonUse.PhongHoc;

public class CustomSearchFieldPH : CustomTextBox
{
    private readonly CustomPopup _popup;

    private readonly List<PhongHocDto> listHP;
    private List<object> _cellDatas;

    private List<string> _columnNames;

    private Form _parent;
    private PhongHocController _PhongHocController;
    private List<object> displayDatas;

    private PhongHocDto selectedHP;

    public CustomSearchFieldPH()
    {
        _parent = new Form();
        _PhongHocController = PhongHocController.getInstance();
        _popup = new CustomPopup();
        listHP = _PhongHocController.GetDanhSachPhongHoc();
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
        selectedHP = _PhongHocController.GetPhongHocById(index);
        contentTextBox.Text = selectedHP.TenPH;
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
                x.MaPH.ToString().ToLower().Contains(keyword) ||
                x.TenPH.ToLower().Contains(keyword)
            ).ToList();
            displayDatas = ConvertObject.ConvertToDisplay(searchList, x => new
            {
                x.MaPH, x.TenPH
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
        var arr = new[] { "MaPH", "TenPH" };
        _columnNames = arr.ToList();
        displayDatas = ConvertObject.ConvertToDisplay(listHP, x => new
        {
            x.MaPH, x.TenPH
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