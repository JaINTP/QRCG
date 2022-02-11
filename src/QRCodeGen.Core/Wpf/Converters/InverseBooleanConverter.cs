// ***********************************************************************
// Assembly         : QRCodeGen.Modules.UI
// Author           : Jai Brown
// Created          : 21-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 21-01-2022
// ***********************************************************************
// <copyright file="InverseBooleanConverter.cs" company="Jai Brown">
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
    ///     Handles the conversion from a boolean value to its opposite value provided by the converter parameter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public sealed class InverseBooleanConverter
        : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (targetType != typeof(bool) && targetType != typeof(bool?) && targetType != typeof(bool))
            {
                throw new InvalidOperationException("The target has to be of type boolean.");
            }

            return !(bool?)value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return this.Convert(value, targetType, parameter, cultureInfo);
        }
    }
}