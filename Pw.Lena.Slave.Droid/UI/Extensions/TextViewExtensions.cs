using JetBrains.Annotations;
using pw.lena.CrossCuttingConcerns.Helpers;
using Pw.Lena.Slave.Droid.UI.Utils;
using Android.Text;
using Android.Text.Style;

namespace Pw.Lena.Slave.Droid.UI.Extensions
{
    public static class TextViewExtensions
    {
        public static void ApplyOswaldFont([NotNull] this Android.Widget.TextView target)
        {
            Guard.ThrowIfNull(target, nameof(target));

            CustomFontUtils.ApplyFont(target, CustomFontUtils.Oswald);
        }

        public static void ApplySourceSansProFont([NotNull] this Android.Widget.TextView target)
        {
            Guard.ThrowIfNull(target, nameof(target));

            CustomFontUtils.ApplyFont(target, CustomFontUtils.SourceSansPro);
        }

        public static void SetTextUnderline(this Android.Widget.TextView textView, int resId)
        {
            Guard.ThrowIfNull(textView, nameof(textView));

            textView.SetText(resId);

            var span = new SpannableString(textView.Text);
            span.SetSpan(new UnderlineSpan(), 0, textView.Text.Length, 0);

            textView.SetText(span, TextView.BufferType.Spannable);
        }
    }
}