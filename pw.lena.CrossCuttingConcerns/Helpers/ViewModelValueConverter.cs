using pw.lena.CrossCuttingConcerns.Helpers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pw.lena.CrossCuttingConcerns.Helpers
{
    public class ViewModelValueConverter
    {
        private static readonly List<IViewModelValueConverter> Converters = new List<IViewModelValueConverter>();

        public static string Convert<TConverter>(object value) where TConverter : IViewModelValueConverter, new()
        {
            return Convert(value, typeof(TConverter));
        }

        public static string Convert(object value, Type converterType)
        {
            var converter = GetConverter(converterType);

            return (string)converter.Convert(value, typeof(string), null, null);
        }

        public static TResult ConvertBack<TConverter, TResult>(string value) where TConverter : IViewModelValueConverter, new()
        {
            return (TResult)ConvertBack(value, typeof(TConverter), typeof(TResult));
        }

        public static object ConvertBack(string value, Type converterType, Type resultType)
        {
            var converter = GetConverter(converterType);

            return converter.ConvertBack(value, resultType, null, null);
        }

        private static IViewModelValueConverter GetConverter(Type converterType)
        {
            var converter = Converters.FirstOrDefault(x => x.GetType() == converterType);

            if (converter == null)
            {
                converter = (IViewModelValueConverter)Activator.CreateInstance(converterType);

                Converters.Add(converter);
            }

            return converter;
        }
    }
}
