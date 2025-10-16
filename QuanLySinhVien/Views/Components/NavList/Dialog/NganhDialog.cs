using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NganhDialog : CustomDialog
{
    private List<LabelTextField> _listLabelTextField;
    private NganhDao _nganhDao;
    private NganhDto _nganhDto;
    private int _nganhId;
    public event Action Finish;

    public NganhDialog(DialogType dialogType, NganhDto nganhDto, NganhDao nganhDao)
        : base(GetTitle(dialogType), dialogType, 450, 400)
    {
        _listLabelTextField = new List<LabelTextField>();
        _nganhDao = nganhDao;
        _nganhDto = nganhDto;
        _nganhId = nganhDto?.MaNganh ?? -1;
        Init();
    }

    private static string GetTitle(DialogType type) => type switch
    {
        DialogType.Them => "Thêm ngành",
        DialogType.Sua => "Cập nhật ngành",
        DialogType.ChiTiet => "Chi tiết ngành",
        _ => "Ngành"
    };

    void Init()
    {
        if (_dialogType == DialogType.ChiTiet)
        {
            _listLabelTextField.Add(new LabelTextField("Mã Ngành", TextFieldType.NormalText));
            _textBoxsContainer.Controls.Add(_listLabelTextField[0]);
        }


        _listLabelTextField.Add(new LabelTextField("Tên Ngành", TextFieldType.NormalText));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);


        _listLabelTextField.Add(new LabelTextField("Khoa", TextFieldType.Combobox));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);


        var khoaList = new KhoaDao().GetAll();
        var khoaComboList = khoaList.Select(k => $"{k.MaKhoa} - {k.TenKhoa}").ToList();
        _listLabelTextField[_listLabelTextField.Count - 1]._combobox.UpdateSelection(khoaComboList.ToArray());

        if (_dialogType == DialogType.Them)
        {
            SetupInsert();
        }
        else if (_dialogType == DialogType.Sua)
        {
            SetupUpdate();
        }
        else
        {
            SetupDetail();
        }
    }

    void SetupInsert()
    {
        _btnLuu._mouseDown += () => { Insert(); };
    }

    void SetupUpdate()
    {
        if (_nganhId == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }


        if (_nganhDto != null)
        {
            int tenNganhIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
            _listLabelTextField[tenNganhIndex].GetTextField().Text = _nganhDto.TenNganh;


            int khoaIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
            string targetValue = $"{_nganhDto.MaKhoa} - ";
            foreach (string item in _listLabelTextField[khoaIndex]._combobox.combobox.Items)
            {
                if (item.StartsWith(targetValue))
                {
                    _listLabelTextField[khoaIndex]._combobox.combobox.SelectedItem = item;
                    break;
                }
            }
        }

        _btnLuu._mouseDown += () => { UpdateNganh(); };
    }

    void SetupDetail()
    {
        if (_nganhId == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }


        if (_nganhDto != null)
        {
            _listLabelTextField[0].GetTextField().Text = _nganhDto.MaNganh.ToString();
            _listLabelTextField[0]._field.Enable = false;


            _listLabelTextField[1].GetTextField().Text = _nganhDto.TenNganh;
            _listLabelTextField[1]._field.Enable = false;


            string targetValue = $"{_nganhDto.MaKhoa} - ";
            foreach (string item in _listLabelTextField[2]._combobox.combobox.Items)
            {
                if (item.StartsWith(targetValue))
                {
                    _listLabelTextField[2]._combobox.combobox.SelectedItem = item;
                    break;
                }
            }

            _listLabelTextField[2]._combobox.Enable = false;
        }
    }

    void Insert()
    {
        int tenNganhIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
        int khoaIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;

        string tenNganh = _listLabelTextField[tenNganhIndex].GetTextTextField();
        string selectedKhoa = _listLabelTextField[khoaIndex]._combobox.combobox.SelectedItem?.ToString();

        if (ValidateInsert(tenNganh, selectedKhoa))
        {
            int maKhoa = int.Parse(selectedKhoa.Split('-')[0].Trim());

            NganhDto nganh = new NganhDto
            {
                MaKhoa = maKhoa,
                TenNganh = tenNganh
            };

            if (_nganhDao.Insert(nganh))
            {
                MessageBox.Show("Thêm ngành thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm ngành thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    void UpdateNganh()
    {
        int tenNganhIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
        int khoaIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;

        string tenNganh = _listLabelTextField[tenNganhIndex].GetTextTextField();
        string selectedKhoa = _listLabelTextField[khoaIndex]._combobox.combobox.SelectedItem?.ToString();

        if (ValidateUpdate(tenNganh, selectedKhoa))
        {
            int maKhoa = int.Parse(selectedKhoa.Split('-')[0].Trim());

            NganhDto nganh = new NganhDto
            {
                MaNganh = _nganhDto.MaNganh,
                MaKhoa = maKhoa,
                TenNganh = tenNganh
            };

            if (_nganhDao.Update(nganh))
            {
                MessageBox.Show("Cập nhật ngành thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật ngành thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    bool ValidateInsert(string tenNganh, string selectedKhoa)
    {
        if (CommonUse.Validate.IsEmpty(tenNganh))
        {
            MessageBox.Show("Tên ngành không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (string.IsNullOrEmpty(selectedKhoa))
        {
            MessageBox.Show("Vui lòng chọn khoa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    bool ValidateUpdate(string tenNganh, string selectedKhoa)
    {
        if (CommonUse.Validate.IsEmpty(tenNganh))
        {
            MessageBox.Show("Tên ngành không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (string.IsNullOrEmpty(selectedKhoa))
        {
            MessageBox.Show("Vui lòng chọn khoa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }
}