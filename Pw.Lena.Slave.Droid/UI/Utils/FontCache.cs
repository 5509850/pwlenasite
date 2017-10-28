using System.Collections.Generic;
using Android.Content;
using Android.Graphics;

namespace Pw.Lena.Slave.Droid.UI.Utils
{
    internal class FontCache
    {
        private static Dictionary<string, Typeface> fontCache = new Dictionary<string, Typeface>();

        public static Typeface Get(string name, Context context)
        {
            Typeface typeface;

            if (!fontCache.TryGetValue(name, out typeface))
            {
                try
                {
                    typeface = Typeface.CreateFromAsset(context.Assets, name);
                }
                catch
                {
                    return null;
                }

                fontCache.Add(name, typeface);
            }

            return typeface;
        }
    }
}