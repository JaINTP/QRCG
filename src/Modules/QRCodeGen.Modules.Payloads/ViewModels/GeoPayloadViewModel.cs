// ***********************************************************************
// Assembly         : QRCodeGen.Modules.Payloads
// Author           : Jai Brown
// Created          : 26-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 30-01-2022
// ***********************************************************************
// <copyright file="GeoPayloadViewModel.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Modules.Payloads.ViewModels
{
    using System.ComponentModel;
    using GMap.NET;
    using GMap.NET.MapProviders;
    using JaINTP.QRCodeGen.Core.Classes;
    using JaINTP.QRCodeGen.Core.Events;
    using JaINTP.QRCodeGen.Core.Mvvm;
    using Prism.Events;
    using Prism.Regions;
    using QRCoder;

    public class GeoPayloadViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ApplicationSettings appSettings;
        private readonly string configFile = Values.ConfigFilePath;
        private readonly IEventAggregator eventAggregator;

        private PointLatLng defaultLatLng = new(-27.52775820686189, 153.072509765625);
        private PointLatLng position;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GeoPayloadViewModel"/> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public GeoPayloadViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager)
        {
            this.appSettings = ApplicationSettings.Load(this.configFile);
            GoogleMapProvider.Instance.ApiKey = this.appSettings.MapsApiKey;

            this.MapProvider = GMapProviders.OpenStreetMap;
            this.Position = this.defaultLatLng;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectedTabChangedEvent>()
                                .Subscribe(this.HandleSelectionChanged);
            this.PropertyChanged += this.OnPropertyChanged;

            Logger.Debug("GeoPayloadViewModel created.");
        }

        public GMapProvider MapProvider { get; set; }

        public PointLatLng Position
        {
            get => this.position;
            set => this.SetProperty(ref this.position, value);
        }

        /// <summary>
        ///     <para>Handles the selection changed event of the parent tab control.</para>
        ///     <para>Used to clear bound properties when tab is changed.</para>
        /// </summary>
        /// <param name="name">
        ///     The name of the currently selected tab.
        /// </param>
        private void HandleSelectionChanged(string name)
        {
            Logger.Debug("In HandleSelectionChanged, clearing properties.");

            if (name != "Geo")
            {
                this.Position = this.defaultLatLng;
            }
        }

        /// <summary>
        ///     Handles the event called when the value of a property changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Debug("In OnPropertyChanged, generating Geolocation payload.");

            this.eventAggregator.GetEvent<PayloadEvent>()
                                .Publish(new PayloadGenerator.Geolocation(
                                    this.Position.Lat.ToString(),
                                    this.Position.Lng.ToString(),
                                    PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps));
        }
    }
}
