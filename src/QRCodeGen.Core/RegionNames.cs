// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="RegionNames.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core
{
    /// <summary>
    ///     A basic class to encapsulate the available region names.
    /// </summary>
    public static class RegionNames
    {
        /// <summary>
        ///     The name of the main region.
        /// </summary>
        public const string MainRegion = "MainRegion";

        /// <summary>
        ///     The name of the payload region.
        /// </summary>
        public const string PayloadRegion = "PayloadRegion";

        /// <summary>
        ///     The name of the dialog region.
        /// </summary>
        public const string DialogRegion = "DialogRegion";
    }
}
