namespace QuanLySinhVien.Views.Components.CommonUse;

// Đổi kiểu mảng string sang list
public class ConvertArray_ListString
{
    public static List<String> ConvertArrayToListString(String[] array)
    {
        List<String> list;
        try
        {
            list = new List<String>(array);
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

    public static List<List<object>> ConvertArrayToListObject(object[,] array)
    {
        List<List<object>> list = new List<List<object>>();
        for (int row = 0; row < array.GetLength(0); row++)
        {
            List<object> rowItems = new List<object>();
            for (int col = 0; col < array.GetLength(1); col++)
            {
                rowItems.Add(array[row, col]);
            }
            list.Add(rowItems);
        }
        return list;
    }
}