using Svg;

namespace QuanLySinhVien.Views.Components.ViewComponents;

public class GetSvgBitmap
{
    public static Bitmap GetBitmap(string file)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "img", file);
        try
        {
            if (Path.GetExtension(path).ToLower() != ".svg")
                throw new Exception("File không phải SVG!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Lỗi không có file");
        }
        
        SvgDocument svgDocument = SvgDocument.Open(path);
        Bitmap btm = svgDocument.Draw();
        return btm;
    }
}