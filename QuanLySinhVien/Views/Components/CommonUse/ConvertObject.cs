namespace QuanLySinhVien.Views.Components.CommonUse;

public class ConvertObject
{
    public static List<object> ConvertListDtoToListObj<T>(List<T> source)
    {
        return source?.Cast<object>().ToList() ?? new List<object>();
    }

    //có thể chỉ giữ lại thuộc tính cần hiển thị thôi
    public static List<object> ConvertToDisplay<T, TResult>(
        IEnumerable<T> source,
        Func<T, TResult> selector)
    {
        if (source == null)
            return new List<object>();

        return source.Select(selector)
            .Cast<object>()
            .ToList();
    }

    public static List<TResult> ConvertDtoToDto<T, TResult>(
        IEnumerable<T> source,
        Func<T, TResult> selector
        )
    {
        return source.Select(selector)
            .ToList();
    }
}