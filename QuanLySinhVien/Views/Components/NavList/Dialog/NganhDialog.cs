using QuanLySinhVien.Controller.Controllers;
using QuanLySinhVien.Models.DAO;
using QuanLySinhVien.Shared.DTO;
using QuanLySinhVien.Shared.Enums;
using QuanLySinhVien.Views.Components.CommonUse;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class NganhDialog : CustomDialog
{
    private readonly List<LabelTextField> _listLabelTextField;
    private readonly NganhDto _nganhDto;
    private readonly int _nganhId;
    private HocPhiTinChiController _hocPhiTinChiController;
    private KhoaController _khoaController;
    private NganhController _nganhController;

    public NganhDialog(DialogType dialogType, NganhDto nganhDto, NganhDao nganhDao)
        : base(GetTitle(dialogType), dialogType, 450, 500)
    {
        _khoaController = KhoaController.GetInstance();
        _listLabelTextField = new List<LabelTextField>();
        _nganhController = NganhController.GetInstance();
        _hocPhiTinChiController = HocPhiTinChiController.GetInstance();
        _nganhDto = nganhDto;
        _nganhId = nganhDto?.MaNganh ?? -1;
        Init();
    }

    public event Action Finish;

    private static string GetTitle(DialogType type)
    {
        return type switch
        {
            DialogType.Them => "Thêm ngành",
            DialogType.Sua => "Cập nhật ngành",
            DialogType.ChiTiet => "Chi tiết ngành",
            _ => "Ngành"
        };
    }

    private void Init()
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

        var khoaList = _khoaController.GetDanhSachKhoa();
        var khoaComboList = khoaList.Select(k => $"{k.MaKhoa} - {k.TenKhoa}").ToList();
        _listLabelTextField[_listLabelTextField.Count - 1]._combobox.UpdateSelection(khoaComboList.ToArray());

        _listLabelTextField.Add(new LabelTextField("Học phí/tín chỉ", TextFieldType.Number));
        _textBoxsContainer.Controls.Add(_listLabelTextField[_listLabelTextField.Count - 1]);

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
        if (_nganhId == -1) throw new Exception("Lỗi chưa cài đặt index");


        if (_nganhDto != null)
        {
            // int tenNganhIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
            _listLabelTextField[0].GetTextField().Text = _nganhDto.TenNganh;


            // int khoaIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
            var targetValue = $"{_nganhDto.MaKhoa} - ";
            foreach (string item in _listLabelTextField[1]._combobox.combobox.Items)
                if (item.StartsWith(targetValue))
                {
                    _listLabelTextField[1]._combobox.combobox.SelectedItem = item;
                    break;
                }

            double hocPhi = _hocPhiTinChiController.GetNewestHocPhiTinChiByMaNganh(_nganhId).SoTienMotTinChi;
            _listLabelTextField[2]._numberField.contentTextBox.Text = hocPhi.ToString();
        }

        _btnLuu._mouseDown += () => { UpdateNganh(); };
    }

    private void SetupDetail()
    {
        if (_nganhId == -1) throw new Exception("Lỗi chưa cài đặt index");


        if (_nganhDto != null)
        {
            _listLabelTextField[0].GetTextField().Text = _nganhDto.MaNganh.ToString();
            _listLabelTextField[0]._field.Enable = false;


            _listLabelTextField[1].GetTextField().Text = _nganhDto.TenNganh;
            _listLabelTextField[1]._field.Enable = false;


            var targetValue = $"{_nganhDto.MaKhoa} - ";
            foreach (string item in _listLabelTextField[2]._combobox.combobox.Items)
                if (item.StartsWith(targetValue))
                {
                    _listLabelTextField[2]._combobox.combobox.SelectedItem = item;
                    break;
                }

            _listLabelTextField[2]._combobox.Enable = false;

            double hocPhi = _hocPhiTinChiController.GetNewestHocPhiTinChiByMaNganh(_nganhId).SoTienMotTinChi;
            _listLabelTextField[3]._numberField.contentTextBox.Text = hocPhi.ToString();
            _listLabelTextField[3]._numberField.Enable = false;
        }
    }

    private void Insert()
    {
        // int tenNganhIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
        // int khoaIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;
        //
        var tenNganh = _listLabelTextField[0].GetTextTextField();
        var selectedKhoa = _listLabelTextField[1]._combobox.combobox.SelectedItem?.ToString();
        var hocPhi = _listLabelTextField[2]._numberField.contentTextBox.Text;

        if (Validate(tenNganh, selectedKhoa, hocPhi))
        {
            var maKhoa = int.Parse(selectedKhoa.Split('-')[0].Trim());

            var nganh = new NganhDto
            {
                MaKhoa = maKhoa,
                TenNganh = tenNganh
            };

            if (_nganhController.Insert(nganh))
            {
                int maNganh = _nganhController.GetLastAutoIncrement();
                InsertHocPhiTinChi(hocPhi, maNganh);

                MessageBox.Show("Thêm ngành thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Thêm ngành thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private void UpdateNganh()
    {
        var tenNganhIndex = _dialogType == DialogType.ChiTiet ? 1 : 0;
        var khoaIndex = _dialogType == DialogType.ChiTiet ? 2 : 1;

        var tenNganh = _listLabelTextField[tenNganhIndex].GetTextTextField();
        var selectedKhoa = _listLabelTextField[khoaIndex]._combobox.combobox.SelectedItem?.ToString();
        var hocPhi = _listLabelTextField[2]._numberField.contentTextBox.Text;


        if (Validate(tenNganh, selectedKhoa, hocPhi))
        {
            var maKhoa = int.Parse(selectedKhoa.Split('-')[0].Trim());

            var nganh = new NganhDto
            {
                MaNganh = _nganhDto.MaNganh,
                MaKhoa = maKhoa,
                TenNganh = tenNganh
            };

            if (_nganhController.Update(nganh))
            {
                InsertHocPhiTinChi(hocPhi, _nganhDto.MaNganh);

                MessageBox.Show("Cập nhật ngành thành công!", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Finish?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Cập nhật ngành thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private void InsertHocPhiTinChi(string hocPhiS, int maNganh)
    {
        var hocPhi = double.Parse(hocPhiS);

        var hocPhiTinChi = new HocPhiTinChiDto
        {
            MaNganh = maNganh,
            SoTienMotTinChi = hocPhi,
            ThoiGianApDung = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
        };

        if (!_hocPhiTinChiController.Insert(hocPhiTinChi))
            MessageBox.Show("Lỗi thêm học phí tín chỉ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private bool Validate(string tenNganh, string selectedKhoa, string hocPhi)
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

        if (CommonUse.Validate.IsEmpty(hocPhi))
        {
            MessageBox.Show("Học phí không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }
}