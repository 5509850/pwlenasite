using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Pw.Lena.Slave.Droid.UI.Utils;

namespace Pw.Lena.Slave.Droid.UI
{
    public class Button : AppCompatButton
    {
        public Button(Context context) : this(context, null)
        {
        }

        public Button(Context context, IAttributeSet attrs) : this(context, attrs, Android.Resource.Attribute.ButtonStyle)
        {
        }

        public Button(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            CustomFontUtils.ApplyFont(this, attrs);
        }

        public Button(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}