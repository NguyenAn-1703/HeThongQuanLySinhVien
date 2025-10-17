using QuanLySinhVien.Models;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class HocPhanDialog : CustomDialog
{
    private List<LabelTextField> _listLabelTextField;
    private HocPhanDao _hocPhanDao;
    private HocPhanDto _hocPhanDto;
    private int _hocPhanId;
    public event Action Finish;

    public HocPhanDialog(DialogType dialogType, HocPhanDto hocPhanDto, HocPhanDao hocPhanDao)
        : base(GetTitle(dialogType), dialogType, 500, 700)
    {
        _listLabelTextField = new List<LabelTextField>();
        _hocPhanDao = hocPhanDao;
        _hocPhanDto = hocPhanDto;
        _hocPhanId = hocPhanDto?.MaHP ?? -1;
        Init();
    }

    private static string GetTitle(DialogType type) => type switch
    {
        DialogType.Them => "Thêm học phần",
        DialogType.Sua => "Cập nhật học phần",
        DialogType.ChiTiet => "Chi tiết học phần",
        _ => "Học phần"
    };

    void Init()
    {
        if (_dialogType == DialogType.ChiTiet)
        {
            _listLabelTextField.Add(new LabelTextField("Mã HP", TextFieldType.NormalText));
            _textBoxsContainer.Controls.Add(_listLabelTextField[0]);
        }


        _listLabelTextField.Add(new LabelTextField("Tên HP", TextFieldType.NormalText));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);


        _listLabelTextField.Add(new LabelTextField("Mã HP Trước", TextFieldType.Combobox));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);


        _listLabelTextField.Add(new LabelTextField("Số Tín Chỉ", TextFieldType.NormalText));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);


        _listLabelTextField.Add(new LabelTextField("Hệ Số HP", TextFieldType.NormalText));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);


        _listLabelTextField.Add(new LabelTextField("Số Tiết LT", TextFieldType.NormalText));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);


        _listLabelTextField.Add(new LabelTextField("Số Tiết TH", TextFieldType.NormalText));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);


        var hocPhanComboList = _hocPhanDao.GetAllForCombobox();
        hocPhanComboList.Insert(0, "(Không có)");
        int maHPTruocIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
        _listLabelTextField[maHPTruocIndex]._combobox.UpdateSelection(hocPhanComboList.ToArray());

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
        if (_hocPhanId == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }


        if (_hocPhanDto != null)
        {
            int tenHPIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
            _listLabelTextField[tenHPIndex].GetTextField().Text = _hocPhanDto.TenHP;


            int soTinChiIndex = _dialogType == DialogType.ChiTiet ? 3 : 2;
            _listLabelTextField[soTinChiIndex].GetTextField().Text = _hocPhanDto.SoTinChi.ToString();


            int heSoHPIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
            _listLabelTextField[heSoHPIndex].GetTextField().Text = _hocPhanDto.HeSoHocPhan.ToString();


            int soTietLTIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
            _listLabelTextField[soTietLTIndex].GetTextField().Text = _hocPhanDto.SoTietLyThuyet.ToString();


            int soTietTHIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;
            _listLabelTextField[soTietTHIndex].GetTextField().Text = _hocPhanDto.SoTietThucHanh.ToString();


            int maHPTruocIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
            if (_hocPhanDto.MaHPTruoc.HasValue)
            {
                string targetValue = $"{_hocPhanDto.MaHPTruoc.Value} - ";
                foreach (string item in _listLabelTextField[maHPTruocIndex]._combobox.combobox.Items)
                {
                    if (item.StartsWith(targetValue))
                    {
                        _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem = "(Không có)";
            }
        }

        _btnLuu._mouseDown += () => { UpdateHocPhan(); };
    }

    void SetupDetail()
    {
        if (_hocPhanId == -1)
        {
            throw new Exception("Lỗi chưa cài đặt index");
        }


        if (_hocPhanDto != null)
        {
            _listLabelTextField[0].GetTextField().Text = _hocPhanDto.MaHP.ToString();
            _listLabelTextField[0]._field.Enable = false;


            _listLabelTextField[1].GetTextField().Text = _hocPhanDto.TenHP;
            _listLabelTextField[1]._field.Enable = false;


            _listLabelTextField[3].GetTextField().Text = _hocPhanDto.SoTinChi.ToString();
            _listLabelTextField[3]._field.Enable = false;


            _listLabelTextField[4].GetTextField().Text = _hocPhanDto.HeSoHocPhan.ToString();
            _listLabelTextField[4]._field.Enable = false;


            _listLabelTextField[5].GetTextField().Text = _hocPhanDto.SoTietLyThuyet.ToString();
            _listLabelTextField[5]._field.Enable = false;


            _listLabelTextField[6].GetTextField().Text = _hocPhanDto.SoTietThucHanh.ToString();
            _listLabelTextField[6]._field.Enable = false;


            if (_hocPhanDto.MaHPTruoc.HasValue)
            {
                string targetValue = $"{_hocPhanDto.MaHPTruoc.Value} - ";
                foreach (string item in _listLabelTextField[2]._combobox.combobox.Items)
                {
                    if (item.StartsWith(targetValue))
                    {
                        _listLabelTextField[2]._combobox.combobox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                _listLabelTextField[2]._combobox.combobox.SelectedItem = "(Không có)";
            }

            _listLabelTextField[2]._combobox.Enable = false;
        }
    }

    void Insert()
    {
        int tenHPIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
        int maHPTruocIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
        int soTinChiIndex = _dialogType == DialogType.ChiTiet ? 3 : 2;
        int heSoHPIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
        int soTietLTIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
        int soTietTHIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;

        string tenHP = _listLabelTextField[tenHPIndex].GetTextTextField();
        string selectedHPTruoc = _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem?.ToString();
        string soTinChiText = _listLabelTextField[soTinChiIndex].GetTextTextField();
        string heSoHPText = _listLabelTextField[heSoHPIndex].GetTextTextField();
        string soTietLTText = _listLabelTextField[soTietLTIndex].GetTextTextField();
        string soTietTHText = _listLabelTextField[soTietTHIndex].GetTextTextField();

        if (ValidateInsert(tenHP, selectedHPTruoc, soTinChiText, heSoHPText, soTietLTText, soTietTHText))
        {
            int? maHPTruoc = null;
            if (selectedHPTruoc != "(Không có)" && !string.IsNullOrEmpty(selectedHPTruoc))
            {
                maHPTruoc = int.Parse(selectedHPTruoc.Split('-')[0].Trim());
            }

            HocPhanDto hocPhan = new HocPhanDto
            {
                MaHPTruoc = maHPTruoc,
                TenHP = tenHP,
                SoTinChi = int.Parse(soTinChiText),
                HeSoHocPhan = float.Parse(heSoHPText),
                SoTietLyThuyet = int.Parse(soTietLTText),
                SoTietThucHanh = int.Parse(soTietTHText)
            };

            if (_hocPhanDao.Insert(hocPhan))
            {
                MessageBox.Show("Thêm học phần thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm học phần thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    void UpdateHocPhan()
    {
        int tenHPIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
        int maHPTruocIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
        int soTinChiIndex = _dialogType == DialogType.ChiTiet ? 3 : 2;
        int heSoHPIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
        int soTietLTIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
        int soTietTHIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;

        string tenHP = _listLabelTextField[tenHPIndex].GetTextTextField();
        string selectedHPTruoc = _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem?.ToString();
        string soTinChiText = _listLabelTextField[soTinChiIndex].GetTextTextField();
        string heSoHPText = _listLabelTextField[heSoHPIndex].GetTextTextField();
        string soTietLTText = _listLabelTextField[soTietLTIndex].GetTextTextField();
        string soTietTHText = _listLabelTextField[soTietTHIndex].GetTextTextField();

        if (ValidateUpdate(tenHP, selectedHPTruoc, soTinChiText, heSoHPText, soTietLTText, soTietTHText))
        {
            int? maHPTruoc = null;
            if (selectedHPTruoc != "(Không có)" && !string.IsNullOrEmpty(selectedHPTruoc))
            {
                maHPTruoc = int.Parse(selectedHPTruoc.Split('-')[0].Trim());
            }

            HocPhanDto hocPhan = new HocPhanDto
            {
                MaHP = _hocPhanDto.MaHP,
                MaHPTruoc = maHPTruoc,
                TenHP = tenHP,
                SoTinChi = int.Parse(soTinChiText),
                HeSoHocPhan = float.Parse(heSoHPText),
                SoTietLyThuyet = int.Parse(soTietLTText),
                SoTietThucHanh = int.Parse(soTietTHText)
            };

            if (_hocPhanDao.Update(hocPhan))
            {
                MessageBox.Show("Cập nhật học phần thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật học phần thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    bool ValidateInsert(string tenHP, string selectedHPTruoc, string soTinChiText, string heSoHPText,
        string soTietLTText, string soTietTHText)
    {
        if (CommonUse.Validate.IsEmpty(tenHP))
        {
            MessageBox.Show("Tên HP không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (string.IsNullOrEmpty(selectedHPTruoc))
        {
            MessageBox.Show("Vui lòng chọn Mã HP Trước!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(soTinChiText, out _))
        {
            MessageBox.Show("Số tín chỉ phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!float.TryParse(heSoHPText, out _))
        {
            MessageBox.Show("Hệ số học phần phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(soTietLTText, out _))
        {
            MessageBox.Show("Số tiết lý thuyết phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(soTietTHText, out _))
        {
            MessageBox.Show("Số tiết thực hành phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    bool ValidateUpdate(string tenHP, string selectedHPTruoc, string soTinChiText, string heSoHPText,
        string soTietLTText, string soTietTHText)
    {
        if (CommonUse.Validate.IsEmpty(tenHP))
        {
            MessageBox.Show("Tên HP không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (string.IsNullOrEmpty(selectedHPTruoc))
        {
            MessageBox.Show("Vui lòng chọn Mã HP Trước!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(soTinChiText, out _))
        {
            MessageBox.Show("Số tín chỉ phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!float.TryParse(heSoHPText, out _))
        {
            MessageBox.Show("Hệ số học phần phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(soTietLTText, out _))
        {
            MessageBox.Show("Số tiết lý thuyết phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(soTietTHText, out _))
        {
            MessageBox.Show("Số tiết thực hành phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }
}