// ***********************************************************************
// Assembly         : QRCodeGen.Modules.Payloads
// Author           : Jai Brown
// Created          : 25-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 25-01-2022
// ***********************************************************************
// <copyright file="BookmarkPayloadViewModel.cs" company="Jai Brown">
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

    public class BookmarkPayloadViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IEventAggregator eventAggregator;
        private string title = string.Empty;
        private string url = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BookmarkPayloadViewModel" /> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public BookmarkPayloadViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectedTabChangedEvent>()
                                .Subscribe(this.HandleSelectionChanged);
            this.PropertyChanged += this.OnPropertyChanged;

            Logger.Debug("BookmarkPayloadViewModel created.");
        }

        /// <summary>
        ///     Gets or sets the bookmark's title.
        /// </summary>
        /// <value>
        ///     The bookmark's title.
        /// </value>
        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        /// <summary>
        ///     Gets or sets the bookmark's URL.
        /// </summary>
        /// <value>
        ///     The bookmark's URL.
        /// </value>
        public string Url
        {
            get => this.url;
            set => this.SetProperty(ref this.url, value);
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

            if (name != "Bookmark")
            {
                this.Url = string.Empty;
                this.Title = string.Empty;
            }
        }

        /// <summary>
        ///     Handles the event called when the value of a property changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Debug("In OnPropertyChanged, generating Bookmark payload.");

            this.eventAggregator.GetEvent<PayloadEvent>()
                                .Publish(new PayloadGenerator.Bookmark(this.Url, this.Title));
        }
    }
}
