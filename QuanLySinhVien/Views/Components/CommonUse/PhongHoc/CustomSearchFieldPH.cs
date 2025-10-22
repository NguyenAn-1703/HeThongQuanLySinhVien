using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.CommonUse.PhongHoc;

public class CustomSearchFieldPH : CustomTextBox
{
    CustomPopup _popup;

    private Form _parent;

    private List<PhongHocDto> listHP;
    private PhongHocController _PhongHocController;

    private List<string> _columnNames;
    private List<object> _cellDatas;
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
        
        selectedHP = _PhongHocController.GetPhongHocById(index);
        contentTextBox.Text = selectedHP.TenPH;
        contentTextBox.Focus();
        contentTextBox.SelectAll();
        _popup.Visible = false;
    }

    void OnKeyDown(KeyEventArgs e)
    {
        //chọn trong list
        if (e.KeyCode == Keys.Down)
        {
            if (_popup.Visible == true)
            {
                _popup._dt.Focus();
            }
        }

        //chọn trực tiếp
        if (e.KeyCode == Keys.Enter && _popup.Visible == true)
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
        

        List<PhongHocDto> searchList = listHP.Where(x =>
            x.MaPH.ToString().ToLower().Contains(keyword) ||
            x.TenPH.ToLower().Contains(keyword)
        ).ToList();
        displayDatas = ConvertObject.ConvertToDisplay(searchList, x => new
        {
            MaPH = x.MaPH,
            TenPH = x.TenPH
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

    public void SetData()
    {
        string[] arr = new[] { "MaPH", "TenPH" };
        _columnNames = arr.ToList();
        displayDatas = ConvertObject.ConvertToDisplay(listHP, x => new
        {
            MaPH = x.MaPH,
            TenPH = x.TenPH
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