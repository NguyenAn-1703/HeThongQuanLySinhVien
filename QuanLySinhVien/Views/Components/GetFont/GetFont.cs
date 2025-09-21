using System.Drawing.Text;
using QuanLySinhVien.Views.Enums;

namespace QuanLySinhVien.Views.Components.GetFont;

public class GetFont
{
    public Font GetMainFont(int size, FontType type)
    {
        PrivateFontCollection pfc = new PrivateFontCollection();
        switch (type)
        {
            case FontType.Regular:
                pfc.AddFontFile("font/Montserrat-Regular.ttf");
                break;
            case FontType.SemiBold:
                pfc.AddFontFile("font/Montserrat-SemiBold.ttf");
                break;
            case FontType.Bold:
                pfc.AddFontFile("font/Montserrat-Bold.ttf");
                break;
            case FontType.ExtraBold:
                pfc.AddFontFile("font/Montserrat-ExtraBold.ttf");
                break;
            case FontType.Black:
                pfc.AddFontFile("font/Montserrat-Black.ttf");
                break;
            default: return null;
        }
        Font font = new Font(pfc.Families[0], size, FontStyle.Regular, GraphicsUnit.Pixel);
        return font;
    }
}