using System.Data;

namespace QuanLySinhVien.Views.Components.CommonUse;


public class CustomTable : FlowLayoutPanel
{
    private List<string> _header;
    private Panel _parForDock = new Panel();
    private int[] _locationX;
    private FlowLayoutPanel _headerPanel;
    

    private List<Control> _lblHeaders;
    public CustomTable(List<string> header)
    {
        _header = header;
        _locationX = new int[_header.Count];
        _lblHeaders = new List<Control>();
        Init();
    }

    void Init()
    {
        this.Dock = DockStyle.Fill;
        this.FlowDirection = FlowDirection.TopDown;
        this.BorderStyle = BorderStyle.FixedSingle;
        this.Controls.Add(_parForDock);
        
        SetHeader();
        
        // FlowLayoutPanel panel = new FlowLayoutPanel{AutoSize = true};
        //
        // panel.FlowDirection = FlowDirection.TopDown;
        // panel.BorderStyle = BorderStyle.FixedSingle;
        // panel.SuspendLayout();
        // for (int i = 0; i < 20; i++)
        // {
        //     Panel box = new Panel
        //     {
        //         BackColor = MyColor.Red,
        //         Size = new Size(1000, 10),
        //     };
        //     panel.Controls.Add(box);
        // }
        //
        // this.Controls.Add(panel);
        //
        // panel.ResumeLayout();

        this.Resize += (sender, args) => OnResize();
    }

    void SetHeader()
    {
        _headerPanel  = new FlowLayoutPanel
        {
            AutoSize = true, 
            Dock = DockStyle.Top,
            Margin = new Padding(0),
            WrapContents = false,
        };
        
        _headerPanel.BorderStyle = BorderStyle.FixedSingle;
        
        for (int i = 0; i < _header.Count; i++)
        {
            _lblHeaders.Add(GetLabel(_header[i]));
            _headerPanel.Controls.Add(_lblHeaders[i]);
        }
        
        this.Controls.Add(_headerPanel);
        
    }

    Label GetLabel(string text)
    {
        Label lbl = new Label
        {
            Text = text,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0),
        };
        
        return lbl;
    }


    // Cho phần DockFill mỗi hàng
    void OnResize()
    {
        _parForDock.Size = new Size(this.Width - this.Padding.Left * 2, 0);
        
        int columnSize = this.Width/_header.Count;

        for (int i = 0; i < _header.Count; i++)
        {
            _lblHeaders[i].Size = new Size(columnSize, _lblHeaders[i].Height);
        }
    }
}