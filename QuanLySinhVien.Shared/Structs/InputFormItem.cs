using QuanLySinhVien.Shared.Enums;

namespace QuanLySinhVien.Shared.Structs;

public struct InputFormItem
{
    public string content;
    public TextFieldType type;

    public InputFormItem(string content, TextFieldType type)
    {
        this.content = content;
        this.type = type;
    }
}