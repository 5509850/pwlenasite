﻿using System;
using System.Globalization;

namespace pw.lena.CrossCuttingConcerns.Helpers.Contracts
{
    public interface IViewModelValueConverter
    {
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
