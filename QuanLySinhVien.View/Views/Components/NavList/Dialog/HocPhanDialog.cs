using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Shared.Structs;
using QuanLySinhVien.View.Views.Components.CommonUse;

namespace QuanLySinhVien.View.Views.Components.NavList.Dialog;

public class HocPhanDialog : CustomDialog
{
    private readonly HocPhanDao _hocPhanDao;
    private readonly HocPhanDto _hocPhanDto;
    private readonly int _hocPhanId;
    private readonly List<LabelTextField> _listLabelTextField;
    private HocPhanController _hocPhanController;

    public HocPhanDialog(DialogType dialogType, HocPhanDto hocPhanDto, HocPhanDao hocPhanDao)
        : base(GetTitle(dialogType), dialogType, 500, 700)
    {
        _listLabelTextField = new List<LabelTextField>();
        _hocPhanDao = hocPhanDao;
        _hocPhanDto = hocPhanDto;
        _hocPhanController = HocPhanController.GetInstance();
        _hocPhanId = hocPhanDto?.MaHP ?? -1;
        Init();
    }

    public event Action Finish;

    private static string GetTitle(DialogType type)
    {
        return type switch
        {
            DialogType.Them => "Thêm học phần",
            DialogType.Sua => "Cập nhật học phần",
            DialogType.ChiTiet => "Chi tiết học phần",
            _ => "Học phần"
        };
    }

    private void Init()
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
        var maHPTruocIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
        _listLabelTextField[maHPTruocIndex]._combobox.UpdateSelection(hocPhanComboList.ToArray());

        if (_dialogType == DialogType.Them)
            SetupInsert();
        else if (_dialogType == DialogType.Sua)
            SetupUpdate();
        else
            SetupDetail();
    }

    private void SetupInsert()
    {
        _btnLuu._mouseDown += () => { Insert(); };
    }

    private void SetupUpdate()
    {
        if (_hocPhanId == -1) throw new Exception("Lỗi chưa cài đặt index");


        if (_hocPhanDto != null)
        {
            var tenHPIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
            _listLabelTextField[tenHPIndex].GetTextField().Text = _hocPhanDto.TenHP;


            var soTinChiIndex = _dialogType == DialogType.ChiTiet ? 3 : 2;
            _listLabelTextField[soTinChiIndex].GetTextField().Text = _hocPhanDto.SoTinChi.ToString();


            var heSoHPIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
            _listLabelTextField[heSoHPIndex].GetTextField().Text = _hocPhanDto.HeSoHocPhan.ToString();


            var soTietLTIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
            _listLabelTextField[soTietLTIndex].GetTextField().Text = _hocPhanDto.SoTietLyThuyet.ToString();


            var soTietTHIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;
            _listLabelTextField[soTietTHIndex].GetTextField().Text = _hocPhanDto.SoTietThucHanh.ToString();


            var maHPTruocIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
            if (_hocPhanDto.MaHPTruoc.HasValue)
            {
                var targetValue = $"{_hocPhanDto.MaHPTruoc.Value} - ";
                foreach (string item in _listLabelTextField[maHPTruocIndex]._combobox.combobox.Items)
                    if (item.StartsWith(targetValue))
                    {
                        _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem = item;
                        break;
                    }
            }
            else
            {
                _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem = "(Không có)";
            }
        }

        _btnLuu._mouseDown += () => { UpdateHocPhan(); };
    }

    private void SetupDetail()
    {
        if (_hocPhanId == -1) throw new Exception("Lỗi chưa cài đặt index");


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
                var targetValue = $"{_hocPhanDto.MaHPTruoc.Value} - ";
                foreach (string item in _listLabelTextField[2]._combobox.combobox.Items)
                    if (item.StartsWith(targetValue))
                    {
                        _listLabelTextField[2]._combobox.combobox.SelectedItem = item;
                        break;
                    }
            }
            else
            {
                _listLabelTextField[2]._combobox.combobox.SelectedItem = "(Không có)";
            }

            _listLabelTextField[2]._combobox.Enable = false;
        }
    }

    private void Insert()
    {
        var tenHPIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
        var maHPTruocIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
        var soTinChiIndex = _dialogType == DialogType.ChiTiet ? 3 : 2;
        var heSoHPIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
        var soTietLTIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
        var soTietTHIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;

        var tenHP = _listLabelTextField[tenHPIndex].GetTextTextField();
        var selectedHPTruoc = _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem?.ToString();
        var soTinChiText = _listLabelTextField[soTinChiIndex].GetTextTextField();
        var heSoHPText = _listLabelTextField[heSoHPIndex].GetTextTextField();
        var soTietLTText = _listLabelTextField[soTietLTIndex].GetTextTextField();
        var soTietTHText = _listLabelTextField[soTietTHIndex].GetTextTextField();

        TextBox tbTenHp = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbTinChi = _listLabelTextField[soTinChiIndex].GetTextField();
        TextBox tbHeSo = _listLabelTextField[heSoHPIndex].GetTextField();
        TextBox tbSoTietLT = _listLabelTextField[soTietLTIndex].GetTextField();
        TextBox tbSoTietTH = _listLabelTextField[soTietTHIndex].GetTextField();
        
        tbTenHp.TabIndex = 1;
        tbTinChi.TabIndex = 2;
        tbHeSo.TabIndex = 3;
        tbSoTietLT.TabIndex = 4;
        tbSoTietTH.TabIndex = 5;

        if (Validate(tenHP, soTinChiText, heSoHPText, soTietLTText, soTietTHText, tbTenHp, tbTinChi, tbHeSo, tbSoTietLT, tbSoTietTH))
        {
            int? maHPTruoc = null;
            if (selectedHPTruoc != "(Không có)" && !string.IsNullOrEmpty(selectedHPTruoc))
                maHPTruoc = int.Parse(selectedHPTruoc.Split('-')[0].Trim());

            var hocPhan = new HocPhanDto
            {
                MaHPTruoc = maHPTruoc,
                TenHP = tenHP,
                SoTinChi = int.Parse(soTinChiText),
                HeSoHocPhan = heSoHPText,
                SoTietLyThuyet = int.Parse(soTietLTText),
                SoTietThucHanh = int.Parse(soTietTHText)
            };

            if (_hocPhanDao.Insert(hocPhan))
            {
                MessageBox.Show("Thêm học phần thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Thêm học phần thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private void UpdateHocPhan()
    {
        var tenHPIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
        var maHPTruocIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
        var soTinChiIndex = _dialogType == DialogType.ChiTiet ? 3 : 2;
        var heSoHPIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
        var soTietLTIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
        var soTietTHIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;

        var tenHP = _listLabelTextField[tenHPIndex].GetTextTextField();
        var selectedHPTruoc = _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem?.ToString();
        var soTinChiText = _listLabelTextField[soTinChiIndex].GetTextTextField();
        var heSoHPText = _listLabelTextField[heSoHPIndex].GetTextTextField();
        var soTietLTText = _listLabelTextField[soTietLTIndex].GetTextTextField();
        var soTietTHText = _listLabelTextField[soTietTHIndex].GetTextTextField();

        TextBox tbTenHp = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbTinChi = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbHeSo = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbSoTietLT = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbSoTietTH = _listLabelTextField[tenHPIndex].GetTextField();
        
        tbTenHp.TabIndex = 1;
        tbTinChi.TabIndex = 2;
        tbHeSo.TabIndex = 3;
        tbSoTietLT.TabIndex = 4;
        tbSoTietTH.TabIndex = 5;

        if (Validate(tenHP, soTinChiText, heSoHPText, soTietLTText, soTietTHText, tbTenHp, tbTinChi, tbHeSo, tbSoTietLT, tbSoTietTH))
        {
            int? maHPTruoc = null;
            if (selectedHPTruoc != "(Không có)" && !string.IsNullOrEmpty(selectedHPTruoc))
                maHPTruoc = int.Parse(selectedHPTruoc.Split('-')[0].Trim());

            var hocPhan = new HocPhanDto
            {
                MaHP = _hocPhanDto.MaHP,
                MaHPTruoc = maHPTruoc,
                TenHP = tenHP,
                SoTinChi = int.Parse(soTinChiText),
                HeSoHocPhan = heSoHPText,
                SoTietLyThuyet = int.Parse(soTietLTText),
                SoTietThucHanh = int.Parse(soTietTHText)
            };

            if (_hocPhanDao.Update(hocPhan))
            {
                MessageBox.Show("Cập nhật học phần thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Cập nhật học phần thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private bool Validate(string tenHP, string soTinChiText, string heSoHPText,
        string soTietLTText, string soTietTHText,
        TextBox tbTenHp, TextBox tbTinChi, TextBox tbHeSo, TextBox tbSoTietLT, TextBox tbSoTietTH)
    {
        Dictionary<int, Control> dic = new Dictionary<int, Control>();
        dic.Add(0 , tbTenHp);
        dic.Add(1 , tbTinChi);
        dic.Add(2 , tbHeSo);
        dic.Add(3 , tbSoTietLT);
        dic.Add(4 , tbSoTietTH);

        ValidateResult rs = _hocPhanController.Validate(tenHP, soTinChiText, heSoHPText, soTietLTText, soTietTHText);

        if (rs.index == -1)
        {
            return true;
        }
        
        MessageBox.Show(rs.message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        Control c = dic.TryGetValue(rs.index, out Control c2) ? c2 : new Control();
        c.Focus();
        if (c is TextBoxBase tb)
        {
            tb.SelectAll();
        }

        return false;
        
        
        
    }


}