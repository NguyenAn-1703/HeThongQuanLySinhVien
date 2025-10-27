using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.CommonUse.Nganh;

public class CustomSearchFieldNG : CustomTextBox
{
    CustomPopup _popup;

    private Form _parent;

    private List<NganhDto> listHP;
    private NganhController _NganhController;

    private List<string> _columnNames;
    private List<object> _cellDatas;
    private List<object> displayDatas;

    private NganhDto selectedHP;

    public CustomSearchFieldNG()
    {
        _parent = new Form();
        _NganhController = NganhController.GetInstance();
        _popup = new CustomPopup();
        listHP = _NganhController.GetAll();
        Init();
    }

    void Init()
    {
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
        
        selectedHP = _NganhController.GetNganhById(index);
        contentTextBox.Text = selectedHP.TenNganh;
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
        

            List<NganhDto> searchList = listHP.Where(x =>
                x.MaNganh.ToString().ToLower().Contains(keyword) ||
                x.TenNganh.ToLower().Contains(keyword)
            ).ToList();
            displayDatas = ConvertObject.ConvertToDisplay(searchList, x => new
            {
                MaNganh = x.MaNganh,
                TenNganh = x.TenNganh
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
        string[] arr = new[] { "MaNganh", "TenNganh" };
        _columnNames = arr.ToList();
        displayDatas = ConvertObject.ConvertToDisplay(listHP, x => new
        {
            MaNganh = x.MaNganh,
            TenNganh = x.TenNganh
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

    }
}