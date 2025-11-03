namespace QuanLySinhVien.Shared;

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

    public static string[] ConvertListToArrayString(List<string> list)
    {
        string[] array;
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
        var list = new List<List<object>>();
        for (var row = 0; row < array.GetLength(0); row++)
        {
            var rowItems = new List<object>();
            for (var col = 0; col < array.GetLength(1); col++) rowItems.Add(array[row, col]);
            list.Add(rowItems);
        }

        return list;
    }
}