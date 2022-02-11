// ***********************************************************************
// Assembly         : QRCodeGen.Modules.UI
// Author           : Jai Brown
// Created          : 24-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 30-01-2022
// ***********************************************************************
// <copyright file="Values.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Classes
{
    using System;
    using System.IO;

    public class Values
    {
        public static readonly string ConfigFilePath =
            Path.Combine(
                $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}",
                "QRCodeGen",
                "Settings.json");
    }
}
