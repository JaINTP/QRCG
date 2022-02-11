// ***********************************************************************
// Assembly         : QRCodeGen.Modules.Payloads
// Author           : Jai Brown
// Created          : 30-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 30-01-2022
// ***********************************************************************
// <copyright file="MeCardPayloadViewModel.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Modules.Payloads.ViewModels
{
    using System;
    using System.ComponentModel;
    using JaINTP.QRCodeGen.Core.Events;
    using JaINTP.QRCodeGen.Core.Mvvm;
    using Prism.Events;
    using Prism.Regions;
    using QRCoder;

    public class MeCardPayloadViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IEventAggregator eventAggregator;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string? nickname = null;
        private string? homePhone = null;
        private string? mobilePhone = null;
        private string? workPhone = null;
        private string? email = null;
        private DateTime? selectedDate = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeCardPayloadViewModel" /> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public MeCardPayloadViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectedTabChangedEvent>()
                                .Subscribe(this.HandleSelectionChanged);
            this.PropertyChanged += this.OnPropertyChanged;

            Logger.Debug("MeCardPayloadViewModel created.");
        }

        /// <summary>
        ///     Gets or sets the first name.
        /// </summary>
        /// <value>
        ///     The first name.
        /// </value>
        public string FirstName
        {
            get => this.firstName;
            set => this.SetProperty(ref this.firstName, value);
        }

        /// <summary>
        ///     Gets or sets the last name.
        /// </summary>
        /// <value>
        ///     The last name.
        /// </value>
        public string LastName
        {
            get => this.lastName;
            set => this.SetProperty(ref this.lastName, value);
        }

        /// <summary>
        ///     Gets or sets the nickname.
        /// </summary>
        /// <value>
        ///     The nickname.
        /// </value>
        public string? Nickname
        {
            get => this.nickname;
            set => this.SetProperty(ref this.nickname, value);
        }

        /// <summary>
        ///     Gets or sets the home phone.
        /// </summary>
        /// <value>
        ///     The home phone.
        /// </value>
        public string? HomePhone
        {
            get => this.homePhone;
            set => this.SetProperty(ref this.homePhone, value);
        }

        /// <summary>
        ///     Gets or sets the mobile phone.
        /// </summary>
        /// <value>
        ///     The mobile phone.
        /// </value>
        public string? MobilePhone
        {
            get => this.mobilePhone;
            set => this.SetProperty(ref this.mobilePhone, value);
        }

        /// <summary>
        ///     Gets or sets the work phone.
        /// </summary>
        /// <value>
        ///     The work phone.
        /// </value>
        public string? WorkPhone
        {
            get => this.workPhone;
            set => this.SetProperty(ref this.workPhone, value);
        }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public string? Email
        {
            get => this.email;
            set => this.SetProperty(ref this.email, value);
        }

        /// <summary>
        ///     Gets or sets the selected date.
        /// </summary>
        /// <value>
        ///     The selected date.
        /// </value>
        public DateTime? SelectedDate
        {
            get => this.selectedDate;
            set => this.SetProperty(ref this.selectedDate, value);
        }

        /// <summary>
        ///   <para>Handles the selection changed event of the parent tab control.</para>
        ///   <para>Used to clear bound properties when tab is changed.</para>
        /// </summary>
        /// <param name="name">
        ///   The name of the currently selected tab.
        /// </param>
        private void HandleSelectionChanged(string name)
        {
            Logger.Debug("In HandleSelectionChanged, clearing properties.");

            if (name != "MeCard")
            {
                this.FirstName = string.Empty;
                this.LastName = string.Empty;
                this.Nickname = null;
                this.HomePhone = null;
                this.MobilePhone = null;
                this.WorkPhone = null;
                this.Email = null;
                this.SelectedDate = null;
            }
        }

        /// <summary>
        ///     Handles the event called when the value of a property changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Debug("In OnPropertyChanged, generating ContactData(MeCard) payload.");

            this.eventAggregator.GetEvent<PayloadEvent>()
                                .Publish(new PayloadGenerator.ContactData(
                                    PayloadGenerator.ContactData.ContactOutputType.VCard3,
                                    this.FirstName,
                                    this.LastName,
                                    this.Nickname,
                                    this.HomePhone,
                                    this.MobilePhone,
                                    this.WorkPhone,
                                    this.Email,
                                    this.SelectedDate));
        }
    }
}
