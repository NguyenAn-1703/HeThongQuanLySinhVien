using Svg;

namespace QuanLySinhVien.View.Views.Components.ViewComponents;

public class GetSvgBitmap
{
    public static Bitmap GetBitmap(string file)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Views", "img", file);
        try
        {
            if (Path.GetExtension(path).ToLower() != ".svg")
                throw new Exception("File không phải SVG!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Lỗi không có file");
        }

        var svgDocument = SvgDocument.Open(path);
        var btm = svgDocument.Draw();
        return btm;
    }
}