// ***********************************************************************
// Assembly         : QRCodeGen.Modules.UI
// Author           : Jai Brown
// Created          : 21-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 21-01-2022
// ***********************************************************************
// <copyright file="WpfMediaColourConverter.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    ///     Handles the conversion from a boolean value to an enum value provided by the converter parameter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public sealed class EnumToBooleanConverter
        : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value == null || parameter == null)
            {
                return false;
            }

            return value.ToString().Equals(
                parameter.ToString(),
                StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc/>
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            return (bool)value ? Enum.Parse(targetType, parameter.ToString()) : null;
        }
    }
}
