using Ninject;
using PlatformAbstractions.Interfaces;
using pw.lena.CrossCuttingConcerns.Helpers.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pw.lena.CrossCuttingConcerns.Helpers
{
    public class NumberValueConverter : IViewModelValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return ((int)value).ToString("N0", new StandardKernel().Get<ILocalizer>().CurrentCulture());
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueString = value as string;

            if (valueString != null)
            {
                var unformattedValueString = valueString.Replace(new StandardKernel().Get<ILocalizer>().CurrentCulture().NumberFormat.CurrencyGroupSeparator, string.Empty);
                int result;

                if (typeof(int) == targetType)
                {
                    int.TryParse(unformattedValueString, NumberStyles.Integer, new StandardKernel().Get<ILocalizer>().CurrentCulture(), out result);

                    return result;
                }
                else if (typeof(int?) == targetType)
                {
                    if (int.TryParse(unformattedValueString, NumberStyles.Currency, new StandardKernel().Get<ILocalizer>().CurrentCulture(), out result))
                    {
                        return (int?)result;
                    }

                    return null;
                }
            }

            throw new InvalidCastException($"Unable to convert \"{value}\" value to \"{targetType}\" type.");
        }
    }
}
