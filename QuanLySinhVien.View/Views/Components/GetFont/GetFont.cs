using System.Drawing.Text;
using QuanLySinhVien.Shared.Enums;

namespace QuanLySinhVien.View.Views.Components.GetFont;

public class GetFont
{
    private static readonly PrivateFontCollection pfc = new();

    static GetFont()
    {
        try
        {
            pfc.AddFontFile("Views/font/Montserrat-Regular.ttf");
            pfc.AddFontFile("Views/font/Montserrat-SemiBold.ttf");
            pfc.AddFontFile("Views/font/Montserrat-ExtraBold.ttf");
            pfc.AddFontFile("Views/font/Montserrat-Black.ttf");
            pfc.AddFontFile("Views/font/PlayfairDisplay-ExtraBold.ttf");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        for (var i = 0; i < 5; i++) Console.WriteLine(pfc.Families[i].Name);
    }

    public static Font GetMainFont(float size, FontType type)
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

    public static Font GetPlayFairFont(float size, FontType type)
    {
        Font font;
        switch (type)
        {
            case FontType.Regular:
                font = new Font(pfc.Families[4], size, FontStyle.Regular);
                break;
            default:
                font = new Font(pfc.Families[4], size, FontStyle.Regular);
                break;
        }

        return font;
    }
}