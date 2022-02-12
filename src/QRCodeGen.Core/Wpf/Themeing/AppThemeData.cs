// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 12-02-2022
// ***********************************************************************
// <copyright file="AppThemeData.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Wpf.Themeing
{
    using System.Windows;

    /// <summary>
    /// Basic class to emulate functionality from
    /// https://github.com/MahApps/MahApps.Metro/blob/main/src/MahApps.Metro.Samples/MahApps.Metro.Demo/MainWindowViewModel.cs.
    /// </summary>
    public class AppThemeData
        : AccentColourData
    {
        /// <inheritdoc/>
        protected override void ChangeAccentCommandExecute(object sender)
        {
            ControlzEx.Theming
                      .ThemeManager
                      .Current
                      .ChangeThemeBaseColor(Application.Current, this.Name);
        }
    }
}
