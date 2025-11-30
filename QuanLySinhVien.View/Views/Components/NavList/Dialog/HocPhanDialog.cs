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
    private KhoaController _khoaController;
    private List<KhoaDto> listKhoa;
    private List<string> listTenKhoa;

    public HocPhanDialog(DialogType dialogType, HocPhanDto hocPhanDto, HocPhanDao hocPhanDao)
        : base(GetTitle(dialogType), dialogType, 500, 900)
    {
        _listLabelTextField = new List<LabelTextField>();
        _hocPhanDao = hocPhanDao;
        _hocPhanDto = hocPhanDto;
        _hocPhanController = HocPhanController.GetInstance();
        _khoaController = KhoaController.GetInstance();
        _hocPhanId = hocPhanDto?.MaHP ?? -1;
        listKhoa = _khoaController.GetDanhSachKhoa();
        listTenKhoa = listKhoa.Select(x => x.TenKhoa).ToList();
        
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

        _listLabelTextField.Add(new LabelTextField("Hệ Số Điểm", TextFieldType.NormalText));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);

        _listLabelTextField.Add(new LabelTextField("Hệ Số HP", TextFieldType.NormalText));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);

        LabelTextField txtSTLT = new LabelTextField("Số Tiết LT", TextFieldType.NormalText);
        txtSTLT._field.Enable = false;
        _listLabelTextField.Add(txtSTLT);
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);

        LabelTextField txtSTTH = new LabelTextField("Số Tiết TH", TextFieldType.NormalText);
        txtSTTH._field.Enable = false;
        _listLabelTextField.Add(txtSTTH);
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);

        LabelTextField cbxKhoa = new LabelTextField("Khoa", TextFieldType.Combobox);
        _listLabelTextField.Add(cbxKhoa);
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);
        
        cbxKhoa.SetComboboxList(listTenKhoa);
        cbxKhoa.SetComboboxSelection(listTenKhoa[0]);

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

    void SetActionInsertUpdate()
    {
        LabelTextField textSoTc = _listLabelTextField[2];
        LabelTextField textHeSo = _listLabelTextField[4];

        int soTC = 0;
        float heSo = 0;

        if (!Shared.Validate.IsPositiveInt(textSoTc._field.contentTextBox.Text))
        {
            soTC = 0;
        }
        else
        {
            soTC = Convert.ToInt32(textSoTc._field.contentTextBox.Text);
        }
        
        if (!Shared.Validate.IsNumeric(textHeSo._field.contentTextBox.Text))
        {
            heSo = 0;
        }
        else
        {
            heSo = float.Parse(textHeSo._field.contentTextBox.Text);
        }
        
        int soTietLT;
        int soTietTH;

        textSoTc._field.contentTextBox.TextChanged += (sender, args) =>
        {
            string text = textSoTc._field.contentTextBox.Text;
            if (!Shared.Validate.IsPositiveInt(text))
            {
                soTC = 0;
            }
            else
            {
                soTC = Convert.ToInt32(text);
            }

            CalculateSoTiet(soTC, heSo, out soTietLT, out soTietTH);
            _listLabelTextField[5]._field.contentTextBox.Text = soTietLT + "";
            _listLabelTextField[6]._field.contentTextBox.Text = soTietTH + "";
        };

        textHeSo._field.contentTextBox.TextChanged += (sender, args) =>
        {
            string text = textHeSo._field.contentTextBox.Text;
            if (!Shared.Validate.IsNumeric(text))
            {
                heSo = 0;
            }
            else
            {
                heSo = float.Parse(text);
            }
            
            CalculateSoTiet(soTC, heSo, out soTietLT, out soTietTH);
            _listLabelTextField[5]._field.contentTextBox.Text = soTietLT + "";
            _listLabelTextField[6]._field.contentTextBox.Text = soTietTH + "";

        };

    }

    void CalculateSoTiet(int soTC, float heSo, out int soTietLT, out int soTietTH)
    {
        if (heSo == 0)
        {
            soTietLT = 0;
            soTietTH = 0;
            return;
        }
        int tclt, tcth;
        tclt = 2 * soTC - (int)(soTC / heSo);
        tcth = soTC - tclt;
        soTietLT = tclt * 15;
        soTietTH = tcth * 30;
    }
    
    private void SetupInsert()
    {
        _btnLuu._mouseDown += () => { Insert(); };
        SetActionInsertUpdate();
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

            var heSoDiemIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
            _listLabelTextField[heSoDiemIndex].GetTextField().Text = _hocPhanDto.HeSoDiem;

            var heSoHPIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
            _listLabelTextField[heSoHPIndex].GetTextField().Text = _hocPhanDto.HeSoHocPhan.ToString();


            var soTietLTIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;
            _listLabelTextField[soTietLTIndex].GetTextField().Text = _hocPhanDto.SoTietLyThuyet.ToString();

            var soTietTHIndex = _dialogType == DialogType.ChiTiet ? 7 : 6;
            _listLabelTextField[soTietTHIndex].GetTextField().Text = _hocPhanDto.SoTietThucHanh.ToString();

            KhoaDto khoaDto = _khoaController.GetKhoaById(_hocPhanDto.MaKhoa);
            var khoaIndex = _dialogType == DialogType.ChiTiet ? 8 : 7;
            _listLabelTextField[khoaIndex].SetComboboxList(listTenKhoa);
            _listLabelTextField[khoaIndex].SetComboboxSelection(khoaDto.TenKhoa);

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
        SetActionInsertUpdate();
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

            _listLabelTextField[4].GetTextField().Text = _hocPhanDto.HeSoDiem;
            _listLabelTextField[4]._field.Enable = false;
            
            _listLabelTextField[5].GetTextField().Text = _hocPhanDto.HeSoHocPhan.ToString();
            _listLabelTextField[5]._field.Enable = false;


            _listLabelTextField[6].GetTextField().Text = _hocPhanDto.SoTietLyThuyet.ToString();
            _listLabelTextField[6]._field.Enable = false;


            _listLabelTextField[7].GetTextField().Text = _hocPhanDto.SoTietThucHanh.ToString();
            _listLabelTextField[7]._field.Enable = false;
            
            KhoaDto khoaDto = _khoaController.GetKhoaById(_hocPhanDto.MaKhoa);
            _listLabelTextField[8].SetComboboxList(listTenKhoa);
            _listLabelTextField[8].SetComboboxSelection(khoaDto.TenKhoa);


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
        var heSoDiemIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
        var heSoHPIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
        var soTietLTIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;
        var soTietTHIndex = _dialogType == DialogType.ChiTiet ? 7 : 6;
        var khoaIndex =  _dialogType == DialogType.ChiTiet ? 8 : 7;

        var tenHP = _listLabelTextField[tenHPIndex].GetTextTextField();
        var selectedHPTruoc = _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem?.ToString();
        var soTinChiText = _listLabelTextField[soTinChiIndex].GetTextTextField();
        var heSoDiem = _listLabelTextField[heSoDiemIndex].GetTextTextField();
        var heSoHPText = _listLabelTextField[heSoHPIndex].GetTextTextField();
        var soTietLTText = _listLabelTextField[soTietLTIndex].GetTextTextField();
        var soTietTHText = _listLabelTextField[soTietTHIndex].GetTextTextField();
        var tenKhoaText =  _listLabelTextField[khoaIndex].GetSelectionCombobox();

        TextBox tbTenHp = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbTinChi = _listLabelTextField[soTinChiIndex].GetTextField();
        TextBox tbHeSoDiem = _listLabelTextField[heSoDiemIndex].GetTextField();
        TextBox tbHeSo = _listLabelTextField[heSoHPIndex].GetTextField();
        TextBox tbSoTietLT = _listLabelTextField[soTietLTIndex].GetTextField();
        TextBox tbSoTietTH = _listLabelTextField[soTietTHIndex].GetTextField();
        
        tbTenHp.TabIndex = 1;
        tbTinChi.TabIndex = 2;
        tbHeSoDiem.TabIndex = 3;
        tbHeSo.TabIndex = 4;
        tbSoTietLT.TabIndex = 5;
        tbSoTietTH.TabIndex = 6;

        if (Validate(tenHP, soTinChiText, heSoDiem, soTietLTText, soTietTHText, tbTenHp, tbTinChi, tbHeSoDiem, tbSoTietLT, tbSoTietTH))
        {
            int? maHPTruoc = null;
            if (selectedHPTruoc != "(Không có)" && !string.IsNullOrEmpty(selectedHPTruoc))
                maHPTruoc = int.Parse(selectedHPTruoc.Split('-')[0].Trim());

            var hocPhan = new HocPhanDto
            {
                MaHPTruoc = maHPTruoc,
                TenHP = tenHP,
                SoTinChi = int.Parse(soTinChiText),
                HeSoDiem = heSoDiem,
                HeSoHocPhan = float.Parse(heSoHPText),
                SoTietLyThuyet = int.Parse(soTietLTText),
                SoTietThucHanh = int.Parse(soTietTHText),
                MaKhoa = _khoaController.GetByTen(tenKhoaText).MaKhoa,
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
        var heSoDiemIndex = _dialogType == DialogType.ChiTiet ? 4 : 3;
        var heSoHPIndex = _dialogType == DialogType.ChiTiet ? 5 : 4;
        var soTietLTIndex = _dialogType == DialogType.ChiTiet ? 6 : 5;
        var soTietTHIndex = _dialogType == DialogType.ChiTiet ? 7 : 6;
        var khoaIndex =  _dialogType == DialogType.ChiTiet ? 8 : 7;

        var tenHP = _listLabelTextField[tenHPIndex].GetTextTextField();
        var selectedHPTruoc = _listLabelTextField[maHPTruocIndex]._combobox.combobox.SelectedItem?.ToString();
        var soTinChiText = _listLabelTextField[soTinChiIndex].GetTextTextField();
        var heSoDiem = _listLabelTextField[heSoDiemIndex].GetTextTextField();
        var heSoHPText = _listLabelTextField[heSoHPIndex].GetTextTextField();
        var soTietLTText = _listLabelTextField[soTietLTIndex].GetTextTextField();
        var soTietTHText = _listLabelTextField[soTietTHIndex].GetTextTextField();
        var tenKhoaText =  _listLabelTextField[khoaIndex].GetSelectionCombobox();

        TextBox tbTenHp = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbTinChi = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbHeSoDiem = _listLabelTextField[heSoDiemIndex].GetTextField();
        TextBox tbHeSo = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbSoTietLT = _listLabelTextField[tenHPIndex].GetTextField();
        TextBox tbSoTietTH = _listLabelTextField[tenHPIndex].GetTextField();
        
        tbTenHp.TabIndex = 1;
        tbTinChi.TabIndex = 2;
        tbHeSoDiem.TabIndex = 3;
        tbHeSo.TabIndex = 4;
        tbSoTietLT.TabIndex = 5;
        tbSoTietTH.TabIndex = 6;

        if (Validate(tenHP, soTinChiText, heSoDiem, soTietLTText, soTietTHText, tbTenHp, tbTinChi, tbHeSoDiem, tbSoTietLT, tbSoTietTH))
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
                HeSoDiem = heSoDiem,
                HeSoHocPhan = float.Parse(heSoHPText),
                SoTietLyThuyet = int.Parse(soTietLTText),
                SoTietThucHanh = int.Parse(soTietTHText),
                MaKhoa = _khoaController.GetByTen(tenKhoaText).MaKhoa,
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

    private bool Validate(string tenHP, string soTinChiText, string heSoDiemText,
        string soTietLTText, string soTietTHText,
        TextBox tbTenHp, TextBox tbTinChi, TextBox tbHeSo, TextBox tbSoTietLT, TextBox tbSoTietTH)
    {
        Dictionary<int, Control> dic = new Dictionary<int, Control>();
        dic.Add(0 , tbTenHp);
        dic.Add(1 , tbTinChi);
        dic.Add(2 , tbHeSo);
        dic.Add(3 , tbSoTietLT);
        dic.Add(4 , tbSoTietTH);

        ValidateResult rs = _hocPhanController.Validate(tenHP, soTinChiText, heSoDiemText, soTietLTText, soTietTHText);

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