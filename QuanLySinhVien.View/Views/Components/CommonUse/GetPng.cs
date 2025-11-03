namespace QuanLySinhVien.View.Views.Components.CommonUse;

public class GetPng
{
    public static Image GetImage(string filepath)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Views", filepath);
        Image img;
        try
        {
            img = Image.FromFile(path);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }

        return img;
    }
}