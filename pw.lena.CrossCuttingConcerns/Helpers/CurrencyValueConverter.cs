using Ninject;
using PlatformAbstractions.Interfaces;
using pw.lena.CrossCuttingConcerns.Helpers.Contracts;
using System;
using System.Globalization;

namespace pw.lena.CrossCuttingConcerns.Helpers
{
    public class CurrencyValueConverter : IViewModelValueConverter
    {
        private const int RoundDecimals = 2;    

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return ((double)value).ToString($"N{RoundDecimals}", new StandardKernel().Get<ILocalizer>().CurrentCulture());
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueString = value as string;

            if (valueString != null)
            {
                var unformattedValueString = valueString.Replace(new StandardKernel().Get<ILocalizer>().CurrentCulture().NumberFormat.CurrencyGroupSeparator, string.Empty);
                double result;

                if (typeof(double) == targetType)
                {
                    double.TryParse(unformattedValueString, NumberStyles.Currency, new StandardKernel().Get<ILocalizer>().CurrentCulture(), out result);

                    return Math.Round((decimal)result, RoundDecimals);
                }
                else if (typeof(double?) == targetType)
                {
                    if (double.TryParse(unformattedValueString, NumberStyles.Currency, new StandardKernel().Get<ILocalizer>().CurrentCulture(), out result))
                    {
                        return (double?)Math.Round((decimal)result, RoundDecimals);
                    }

                    return null;
                }
            }

            throw new InvalidCastException($"Unable to convert \"{value}\" value to \"{targetType}\" type.");
        }
    }
}
