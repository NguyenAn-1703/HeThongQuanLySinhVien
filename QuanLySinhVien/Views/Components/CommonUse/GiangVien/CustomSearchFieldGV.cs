namespace QuanLySinhVien.Views.Components.CommonUse.GiangVien;

using QuanLySinhVien.Controllers;
using QuanLySinhVien.Models;


public class CustomSearchFieldGV : CustomTextBox
{
    CustomPopup _popup;

    private Form _parent;

    private List<GiangVienDto> listHP;
    private GiangVienController _GiangVienController;

    private List<string> _columnNames;
    private List<object> _cellDatas;
    private List<object> displayDatas;

    private GiangVienDto selectedHP;
    
    public CustomSearchFieldGV()
    {
        _parent = new  Form();
        _GiangVienController = GiangVienController.GetInstance();
        _popup = new CustomPopup();
        listHP = _GiangVienController.GetAll();
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
        
        selectedHP = _GiangVienController.GetById(index);
        contentTextBox.Text = selectedHP.TenGV;
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
        
        _popup.Visible = true;

        List<GiangVienDto> searchList = listHP.Where(x => 
            x.MaGV.ToString().ToLower().Contains(keyword) ||
            x.TenGV.ToLower().Contains(keyword)
            ).ToList();
        displayDatas = ConvertObject.ConvertToDisplay(searchList, x => new
        {
            MaGV = x.MaGV,
            TenGV = x.TenGV
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
        string [] arr = new[] { "MaGV", "TenGV"};
        _columnNames = arr.ToList();
        displayDatas = ConvertObject.ConvertToDisplay(listHP, x => new
        {
            MaGV = x.MaGV,
            TenGV = x.TenGV
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