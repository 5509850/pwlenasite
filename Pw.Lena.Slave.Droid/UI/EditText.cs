using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Pw.Lena.Slave.Droid.UI.Utils;
using pw.lena.CrossCuttingConcerns.Helpers;

namespace Pw.Lena.Slave.Droid.UI
{
    public class EditText : AppCompatEditText
    {
        private static readonly IInputFilter[] EmptyInputFilters = { };

        private IInputFilter[] inputFilters = { };

        public EditText(Context context) : this(context, null)
        {
        }

        public EditText(Context context, IAttributeSet attrs) : this(context, attrs, Android.Resource.Attribute.EditTextStyle)
        {
        }

        public EditText(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            CustomFontUtils.ApplyFont(this, attrs);
        }

        public EditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public event EventHandler FocusOut;

        public override void SetFilters(IInputFilter[] filters)
        {
            if (filters.Equals(inputFilters) || filters.Equals(EmptyInputFilters))
            {
                base.SetFilters(filters);
            }
            else if (!filters.Equals(inputFilters))
            {
                inputFilters = filters;
            }
        }

        protected override void OnFocusChanged(bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Rect previouslyFocusedRect)
        {
            base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);

            if (gainFocus)
            {
                SetFilters(inputFilters);

                if ((InputType & InputTypes.NumberFlagDecimal) != 0 && !string.IsNullOrEmpty(Text))
                {
                    var convertedValue = ViewModelValueConverter.ConvertBack<CurrencyValueConverter, double?>(Text);

                    if (convertedValue != null)
                    {
                        Text = convertedValue.Value.ToString(CurCult.CurrentCulture);
                    }
                }
            }
            else
            {
                SetFilters(EmptyInputFilters);

                if ((InputType & InputTypes.NumberFlagDecimal) != 0 && !string.IsNullOrEmpty(Text))
                {
                    var convertedValue = ViewModelValueConverter.ConvertBack<CurrencyValueConverter, double?>(Text);

                    if (convertedValue != null)
                    {
                        Text = ViewModelValueConverter.Convert<CurrencyValueConverter>(convertedValue);
                    }
                }

                FocusOut?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}