// ***********************************************************************
// Assembly         : QRCodeGen.Modules.UI
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="SettingsViewModel.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Modules.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media;
    using ControlzEx.Theming;
    using JaINTP.QRCodeGen.Core.Classes;
    using JaINTP.QRCodeGen.Core.Mvvm;
    using JaINTP.QRCodeGen.Core.Wpf.Themeing;
    using NLog;
    using Prism.Commands;
    using Prism.Regions;

    /// <summary>
    ///     The Settings ViewModel.
    /// </summary>
    [RegionMemberLifetime(KeepAlive = true)]
    public class SettingsViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ApplicationSettings appSettings;
        private readonly string configFile = Values.ConfigFilePath;

        private AccentColourData? selectedAccentColour;
        private AppThemeData? selectedTheme;
        private string mapsApiKey;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SettingsViewModel" /> class.
        /// </summary>
        /// <param name="regionManager">
        ///     The region manager.
        /// </param>
        public SettingsViewModel(IRegionManager regionManager)
            : base(regionManager)
        {
            this.appSettings = ApplicationSettings.Load(this.configFile);
            this.GoBackCommand = new DelegateCommand(this.GoBackCommandExecute);
            this.PropertyChanged += this.SettingsViewModel_PropertyChanged;

            this.AppThemes =
                ThemeManager.Current
                            .Themes
                            .GroupBy(x => x.BaseColorScheme)
                            .Select(x => x.First())
                            .Select(a => new AppThemeData
                            {
                                Name = a.BaseColorScheme,
                                BorderColourBrush = a.Resources["MahApps.Brushes.ThemeForeground"] as Brush,
                                ColourBrush = a.Resources["MahApps.Brushes.ThemeBackground"] as Brush,
                            })
                            .ToList() !;

            this.AccentColours =
                ThemeManager.Current
                            .Themes
                            .GroupBy(x => x.ColorScheme)
                            .OrderBy(a => a.Key)
                            .Select(a => new AccentColourData
                            {
                                Name = a.Key,
                                ColourBrush = a.First().ShowcaseBrush,
                            })
                            .ToList();

            // Excuse the ugliness.
            this.SelectedAccentColour =
                this.AccentColours.Single(x => x.Name == (this.appSettings.SelectedAccentColour?.Name ?? "Green"));
            this.SelectedTheme =
                this.AppThemes.Single(x => x.Name == (this.appSettings.SelectedTheme?.Name ?? "Dark"));
            this.SelectedAccentColour.ChangeAccentCommand?.Execute(null);
            this.SelectedTheme.ChangeAccentCommand?.Execute(null);
            this.MapsApiKey = this.appSettings.MapsApiKey;

            Logger.Debug("SettingsViewModel created.");
        }

        /// <summary>
        ///     Gets or sets the accent colours.
        /// </summary>
        /// <value>
        ///     The accent colours.
        /// </value>
        public List<AccentColourData> AccentColours { get; set; }

        /// <summary>
        ///     Gets or sets the application themes.
        /// </summary>
        /// <value>
        ///     The application themes.
        /// </value>
        public List<AppThemeData> AppThemes { get; set; }

        /// <summary>
        ///     Gets or sets the selected accent colour.
        /// </summary>
        /// <value>
        ///     The selected accent colour.
        /// </value>
        public AccentColourData SelectedAccentColour
        {
            get => this.selectedAccentColour;
            set => this.SetProperty(ref this.selectedAccentColour, value);
        }

        /// <summary>
        ///     Gets or sets the selected theme.
        /// </summary>
        /// <value>
        ///     The selected theme.
        /// </value>
        public AppThemeData SelectedTheme
        {
            get => this.selectedTheme;
            set => this.SetProperty(ref this.selectedTheme, value);
        }

        /// <summary>
        ///     Gets or sets the maps API key.
        /// </summary>
        /// <value>
        ///     The maps API key.
        /// </value>
        public string MapsApiKey
        {
            get => this.mapsApiKey;
            set => this.SetProperty(ref this.mapsApiKey, value);
        }

        /// <summary>
        ///     Gets the go back command.
        /// </summary>
        /// <value>
        ///     The go back command.
        /// </value>
        public ICommand GoBackCommand { get; private set; }

        /// <summary>
        ///     The GoBackCommand's execute method.
        /// </summary>
        private void GoBackCommandExecute()
        {
            Logger.Debug("GoBackCommand executed.");

            this.RegionManager
                .Regions["MainRegion"]
                .NavigationService
                .Journal
                .GoBack();
        }

        /// <summary>
        ///     Handles the PropertyChanged event of the SettingsViewModel control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="PropertyChangedEventArgs" /> instance containing the event data.
        /// </param>
        private void SettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Debug("SettingsViewModel_PropertyChanged event called.");

            if (e.PropertyName == "SelectedTheme")
            {
                this.appSettings.SelectedTheme = this.SelectedTheme;
            }
            else if (e.PropertyName == "SelectedAccentColour")
            {
                this.appSettings.SelectedAccentColour = this.SelectedAccentColour;
            }
            else
            {
                var localType = this.GetType();
                var localPropInfo = localType.GetProperty(e.PropertyName);

                var settingsType = typeof(ApplicationSettings);
                var settingsPropInfo = settingsType.GetProperty(e.PropertyName);

                settingsPropInfo.SetValue(
                    this.appSettings,
                    Convert.ChangeType(localPropInfo.GetValue(this), settingsPropInfo.PropertyType),
                    null);
            }

            this.appSettings.Save();
        }
    }
}