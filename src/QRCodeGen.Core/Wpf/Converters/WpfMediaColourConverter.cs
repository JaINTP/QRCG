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
    using DrawingColour = System.Drawing.Color;
    using MediaColour = System.Windows.Media.Color;

    /// <summary>
    ///     Handles the conversion from a boolean value to its opposite value provided by the converter parameter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public sealed class WpfMediaColourConverter
        : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            DrawingColour drawingColour = (DrawingColour)value;

            return MediaColour.FromArgb(
                drawingColour.A,
                drawingColour.R,
                drawingColour.G,
                drawingColour.B);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            MediaColour mediaColour = (MediaColour)value;

            return DrawingColour.FromArgb(
                mediaColour.A,
                mediaColour.R,
                mediaColour.G,
                mediaColour.B);
        }
    }
}
