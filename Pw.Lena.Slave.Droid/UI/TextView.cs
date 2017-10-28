using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Pw.Lena.Slave.Droid.UI.Utils;

namespace Pw.Lena.Slave.Droid.UI
{
    public class TextView : AppCompatTextView
    {
        public TextView(Context context) : this(context, null)
        {
        }

        public TextView(Context context, IAttributeSet attrs) : this(context, attrs, Android.Resource.Attribute.TextViewStyle)
        {
        }

        public TextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            CustomFontUtils.ApplyFont(this, attrs);
        }

        public TextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}