namespace QuanLySinhVien.Views.Components.CommonUse;

public class GetPng
{
    public static Image GetImage(string filepath)
    {
        Image img;
        try
        {
            img =  Image.FromFile(filepath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
        return img;
    }
}