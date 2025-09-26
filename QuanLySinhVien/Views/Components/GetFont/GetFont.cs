using System.Drawing.Text;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.GetFont;

public class GetFont
{
    static PrivateFontCollection pfc = new PrivateFontCollection();
    static GetFont()
    {
        try
        {
            pfc.AddFontFile("font/Montserrat-Regular.ttf");
            pfc.AddFontFile("font/Montserrat-SemiBold.ttf");
            pfc.AddFontFile("font/Montserrat-ExtraBold.ttf");
            pfc.AddFontFile("font/Montserrat-Black.ttf");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }


        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine(pfc.Families[i].Name);
        }
    }
    static public Font GetMainFont(float size, FontType type)
    {
        
        Font font;
        switch (type)
        {
            case FontType.Regular:
                font = new Font(pfc.Families[0], size, FontStyle.Regular);
                break;
            case FontType.SemiBold:
                font = new Font(pfc.Families[3], size, FontStyle.Regular);
                break;
            case FontType.Bold:
                font = new Font(pfc.Families[0], size, FontStyle.Bold);
                break;
            case FontType.ExtraBold:
                font = new Font(pfc.Families[2], size, FontStyle.Regular);
                break;
            case FontType.Black:
                font = new Font(pfc.Families[1], size, FontStyle.Regular);
                break;
            case FontType.SemiBoldItalic:
                font = new Font(pfc.Families[3], size, FontStyle.Italic);
                break;
            case FontType.RegularItalic:
                font = new Font(pfc.Families[0], size, FontStyle.Italic);
                break;
            default: throw new Exception("Font type not supported");
        }
        return font;
    }

 
}