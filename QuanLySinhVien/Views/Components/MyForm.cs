namespace QuanLySinhVien.Views.Components;

public class MyForm : Form
{
    public MyForm(string text, Size size)
    {
        base.Text = text;
        Size = size;
        
        // Default
        StartPosition = FormStartPosition.CenterScreen;
    }
}