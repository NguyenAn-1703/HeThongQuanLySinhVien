namespace QuanLySinhVien.Views.Components.CommonUse;

// Đổi kiểu mảng string sang list
public class ConvertArray_ListString
{
    public static List<string> ConvertArrayToListString(string[] array)
    {
        List<string> list;
        try
        {
            list = new List<string>(array);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return list;
    }
    
    public static String[] ConvertListToArrayString(List<string> list)
    {
        String[] array;
        try
        {
            array = list.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return array;
    }
}