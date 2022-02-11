// ***********************************************************************
// Assembly         : QRCodeGen.Modules.Payloads
// Author           : Jai Brown
// Created          : 25-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 26-01-2022
// ***********************************************************************
// <copyright file="MailPayloadViewModel.cs" company="Jai Brown">
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

    internal class MailPayloadViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IEventAggregator eventAggregator;
        private string message = string.Empty;
        private string recipient = string.Empty;
        private string subject = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MailPayloadViewModel" /> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public MailPayloadViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectedTabChangedEvent>()
                                .Subscribe(this.HandleSelectionChanged);
            this.PropertyChanged += this.OnPropertyChanged;

            Logger.Debug("MailPayloadViewModel created.");
        }

        /// <summary>
        ///     Gets or sets the email message.
        /// </summary>
        /// <value>
        ///     The email message.
        /// </value>
        public string Message
        {
            get => this.message;
            set => this.SetProperty(ref this.message, value);
        }

        /// <summary>
        ///     Gets or sets the email recipient.
        /// </summary>
        /// <value>
        ///     The email recipient.
        /// </value>
        public string Recipient
        {
            get => this.recipient;
            set => this.SetProperty(ref this.recipient, value);
        }

        /// <summary>
        ///     Gets or sets the email subject.
        /// </summary>
        /// <value>
        ///     The email subject.
        /// </value>
        public string Subject
        {
            get => this.subject;
            set => this.SetProperty(ref this.subject, value);
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

            if (name != "Mail")
            {
                this.Message = string.Empty;
                this.Recipient = string.Empty;
                this.Subject = string.Empty;
            }
        }

        /// <summary>
        ///     Handles the event called when the value of a property changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Debug("In OnPropertyChanged, generating Mail payload.");

            this.eventAggregator.GetEvent<PayloadEvent>()
                                .Publish(new PayloadGenerator.Mail(
                                    this.Subject,
                                    this.Recipient,
                                    this.Message));
        }
    }
}
