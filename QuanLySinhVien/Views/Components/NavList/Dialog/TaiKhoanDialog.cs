using QuanLySinhVien.Views.Components.CommonUse;
using QuanLySinhVien.Views.Components.ViewComponents;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.NavList.Dialog;

public class TaiKhoanDialog : CustomDialog
{

    private CustomButton _exitButton;
    private TableLayoutPanel _mainLayout;

    public TaiKhoanDialog(DialogType dialogType, string title)
    {
        _dialogType = dialogType;
        _title = title;
        Init();
    }

    void Init()
    {
        // StartPosition = FormStartPosition.CenterScreen;
        // this.FormBorderStyle = FormBorderStyle.None;
        // this.SuspendLayout();
        //
        // _mainLayout = new TableLayoutPanel
        // {
        //     AutoSize = true,
        //     RowCount = 4,
        //     Dock = DockStyle.Fill,
        //     BorderStyle = BorderStyle.Fixed3D,
        // };
        //
        // _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        // _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        // _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        // _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        //
        // SetTopBar();
        
        if (_dialogType == DialogType.Them)
        {
            CreateFormInsert();
        }
        else if (_dialogType == DialogType.Sua)
        {
            CreateFormUpdate();
        }
        else
        {
            CreateFormDetail();
        }
        // this.Controls.Add(_mainLayout);
        // this.ResumeLayout(false);
    }

    // void SetTopBar()
    // {
    //     _exitButton = new CustomButton(25, 25, "exitbutton.svg", MyColor.White, false, false, false, false);
    //     _exitButton.HoverColor = MyColor.GrayHoverColor;
    //     _exitButton.SelectColor = MyColor.GraySelectColor;
    //     _exitButton.Anchor = AnchorStyles.Right;
    //     
    //     _exitButton.MouseDown +=  (sender, args) => this.Close(); 
    //     
    //     this._mainLayout.Controls.Add(_exitButton);
    // }

    void CreateFormInsert()
    {
        base._title = "Thêm tài khoản";
    }
    void CreateFormUpdate(){}
    void CreateFormDetail(){}
    
}