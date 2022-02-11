// ***********************************************************************
// Assembly         : QRCodeGen.Modules.Payloads
// Author           : Jai Brown
// Created          : 23-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 26-01-2022
// ***********************************************************************
// <copyright file="PhonePayloadViewModel.cs" company="Jai Brown">
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

    public class PhonePayloadViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IEventAggregator eventAggregator;
        private string phoneNumber;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PhonePayloadViewModel" /> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public PhonePayloadViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectedTabChangedEvent>()
                                .Subscribe(this.HandleSelectionChanged);
            this.PropertyChanged += this.OnPropertyChanged;

            Logger.Debug("PhonePayloadViewModel created.");
        }

        /// <summary>
        ///     Gets or sets a string representing the phone number.
        /// </summary>
        /// <value>
        ///     The string representing the phone number.
        /// </value>
        public string PhoneNumber
        {
            get => this.phoneNumber;
            set => this.SetProperty(ref this.phoneNumber, value);
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

            if (name != "Url")
            {
                this.PhoneNumber = string.Empty;
            }
        }

        /// <summary>Handles the event called when the value of a property changes.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Debug("In OnPropertyChanged, generating PhoneNumber payload.");

            this.eventAggregator.GetEvent<PayloadEvent>()
                                .Publish(new PayloadGenerator.PhoneNumber(this.PhoneNumber));
        }
    }
}
