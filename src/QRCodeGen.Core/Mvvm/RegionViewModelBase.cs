// ***********************************************************************
// Assembly         : RegionViewModelBase.Core
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="RegionViewModelBase.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Mvvm
{
    using System;
    using Prism.Regions;

    /// <summary>
    ///     Basic class for region navigatable viewmodels.
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    /// <seealso cref="Prism.Regions.INavigationAware" />
    /// <seealso cref="Prism.Regions.IConfirmNavigationRequest" />
    public class RegionViewModelBase
        : ViewModelBase, INavigationAware, IConfirmNavigationRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegionViewModelBase" /> class.
        /// </summary>
        /// <param name="regionManager">
        ///     The region manager.
        /// </param>
        public RegionViewModelBase(IRegionManager regionManager)
            : base(regionManager)
        {
            this.RegionManager = regionManager;
        }

        /// <summary>
        ///     Gets the region manager.
        /// </summary>
        /// <value>
        ///     The region manager.
        /// </value>
        protected new IRegionManager RegionManager { get; private set; }

        /// <inheritdoc/>
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        /// <inheritdoc/>
        public new virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <inheritdoc/>
        public new virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <inheritdoc/>
        public new virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
