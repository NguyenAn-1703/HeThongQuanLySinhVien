using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Views.Components.CommonUse;

public class CustomSearchFieldHP : CustomTextBox
{
    CustomPopup _popup;

    private Form _parent;

    private List<HocPhanDto> listHP;
    private HocPhanController _hocPhanController;

    private List<string> _columnNames;
    private List<object> _cellDatas;
    private List<object> displayDatas;

    private HocPhanDto selectedHP;
    
    public CustomSearchFieldHP()
    {
        _parent = new  Form();
        _hocPhanController = HocPhanController.GetInstance();
        _popup = new CustomPopup();
        listHP = _hocPhanController.GetAll();
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
        this.contentTextBox.KeyDown +=  (sender, args) => OnKeyDown(args);
        this._popup.KeyEnter += (index) => OnKeyEnter(index);
    }

    void OnKeyEnter(int index)
    {
        
        selectedHP = _hocPhanController.GetHocPhanById(index);
        contentTextBox.Text = selectedHP.TenHP;
        contentTextBox.Focus();
        contentTextBox.SelectAll();
        _popup.Visible = false;
    }

    void OnKeyDown(KeyEventArgs e)
    {
        //chọn trong list
        if (e.KeyCode == Keys.Down && !contentTextBox.Text.Equals(""))
        {
            if (_popup.Visible == true)
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
        

            List<HocPhanDto> searchList = listHP.Where(x => 
                x.MaHP.ToString().ToLower().Contains(keyword) ||
                x.TenHP.ToLower().Contains(keyword)
            ).ToList();
            displayDatas = ConvertObject.ConvertToDisplay(searchList, x => new
            {
                MaHP = x.MaHP,
                TenHP = x.TenHP
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
        string [] arr = new[] { "MaHP", "TenHP"};
        _columnNames = arr.ToList();
        displayDatas = ConvertObject.ConvertToDisplay(listHP, x => new
        {
            MaHP = x.MaHP,
            TenHP = x.TenHP
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