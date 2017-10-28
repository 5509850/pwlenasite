using Android.Content.Res;
using pw.lena.CrossCuttingConcerns.Helpers;
using Android.Util;

namespace Pw.Lena.Slave.Droid.UI.Utils
{
    internal class CustomFontUtils
    {
        public const string Oswald = "oswald";
        public const string SourceSansPro = "sourcesanspro";

        public static void ApplyFont(Android.Widget.TextView target, IAttributeSet attrs)
        {
            Guard.ThrowIfNull(target, "target");

            TypedArray a = target.Context.ObtainStyledAttributes(attrs, Resource.Styleable.Fonts);
            string fontName = a.GetString(Resource.Styleable.Fonts_font);

            ApplyFont(target, fontName);

            a.Recycle();
        }

        public static void ApplyFont(Android.Widget.TextView target, string fontName)
        {
            Guard.ThrowIfNull(target, "target");

            string fontFileName = GetFontFileName(fontName);

            if (!string.IsNullOrEmpty(fontFileName))
            {
                target.Typeface = FontCache.Get("fonts/" + fontFileName, target.Context);
            }
        }

        private static string GetFontFileName(string fontName)
        {
            switch (fontName)
            {
                case "oswald":
                    return "Oswald-Regular.ttf";
                case "sourcesanspro":
                    return "SourceSansPro-Regular.otf";
                default:
                    return string.Empty;
            }
        }
    }
}