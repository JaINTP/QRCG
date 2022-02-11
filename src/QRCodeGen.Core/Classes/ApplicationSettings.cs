// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 19-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 30-01-2022
// ***********************************************************************
// <copyright file="ApplicationSettings.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Classes
{
    using System.ComponentModel;
    using JaINTP.QRCodeGen.Core.Wpf.Themeing;
    using Newtonsoft.Json;

    /// <summary>
    /// Basic bitch class representing specific application specific settings.
    /// </summary>
    public class ApplicationSettings
        : SettingsHandler<ApplicationSettings>
    {
        private string mapsApiKey;
        private AppThemeData selectedTheme;
        private AccentColourData selectedAccentColour;

        [DefaultValue("")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string MapsApiKey
        {
            get => this.mapsApiKey;
            set => this.SetProperty(ref this.mapsApiKey, value);
        }

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public AppThemeData? SelectedTheme
        {
            get => this.selectedTheme;
            set => this.SetProperty(ref this.selectedTheme, value);
        }

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public AccentColourData? SelectedAccentColour
        {
            get => this.selectedAccentColour;
            set => this.SetProperty(ref this.selectedAccentColour, value);
        }
    }
}