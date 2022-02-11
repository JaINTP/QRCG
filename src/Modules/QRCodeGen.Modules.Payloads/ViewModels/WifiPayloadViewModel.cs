// ***********************************************************************
// Assembly         : QRCodeGen.Modules.Payloads
// Author           : Jai Brown
// Created          : 25-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 25-01-2022
// ***********************************************************************
// <copyright file="WifiPayloadViewModel.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Modules.Payloads.ViewModels
{
    using System.ComponentModel;
    using JaINTP.QRCodeGen.Core.Events;
    using JaINTP.QRCodeGen.Core.Mvvm;
    using Prism.Events;
    using Prism.Regions;
    using QRCoder;

    /// <summary>
    ///     Enum containing possible wifi encryption types.
    /// </summary>
    public enum Auth
    {
        /// <summary>
        ///     WPA encryption type.
        /// </summary>
        WPA = 0,

        /// <summary>
        ///     WEP encryption type.</para>
        /// </summary>
        WEP = 1,

        /// <summary>
        ///     No encryption type.
        /// </summary>
        None = 2,
    }

    public class WifiPayloadViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IEventAggregator eventAggregator;

        private Auth encType = Auth.None;
        private string password = string.Empty;
        private string ssid = string.Empty;

        /// <summary>Initializes a new instance of the <see cref="WifiPayloadViewModel" /> class.</summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public WifiPayloadViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectedTabChangedEvent>()
                                .Subscribe(this.HandleSelectionChanged);
            this.PropertyChanged += this.OnPropertyChanged;

            Logger.Debug("WifiPayloadViewModel created.");
        }

        /// <summary>
        ///     Gets or sets the wifi encryption type.
        /// </summary>
        /// <value>
        ///     The wifi encryption type. WEP, WPA or None.
        /// </value>
        public Auth EncType
        {
            get => this.encType;
            set => this.SetProperty(ref this.encType, value);
        }

        /// <summary>
        ///     Gets or sets the wifi password.
        /// </summary>
        /// <value>
        ///     The wifi password.
        /// </value>
        public string Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        /// <summary>
        ///     Gets or sets the ssid/wifi name.
        /// </summary>
        /// <value>
        ///     The ssid/wifi name.
        /// </value>
        public string Ssid
        {
            get => this.ssid;
            set => this.SetProperty(ref this.ssid, value);
        }

        /// <summary>
        ///   <para>Handles the selection changed event of the parent tab control.</para>
        ///   <para>Used to clear bound properties when tab is changed.</para>
        /// </summary>
        /// <param name="name">The name of the currently selected tab.</param>
        private void HandleSelectionChanged(string name)
        {
            Logger.Debug("In HandleSelectionChanged, clearing properties.");

            if (name != "Wifi")
            {
                this.EncType = Auth.None;
                this.Password = string.Empty;
                this.Ssid = string.Empty;
            }
        }

        /// <summary>Handles the event called when the value of a property changes.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Debug("In OnPropertyChanged, generating Wifi payload.");

            this.eventAggregator.GetEvent<PayloadEvent>()
                                .Publish(new PayloadGenerator.WiFi(
                                    this.Ssid,
                                    this.Password,
                                    (PayloadGenerator.WiFi.Authentication)this.EncType));
        }
    }
}
