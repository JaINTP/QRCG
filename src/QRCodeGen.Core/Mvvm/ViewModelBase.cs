// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="ViewModelBase.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Mvvm
{
    using Prism.Mvvm;
    using Prism.Navigation;
    using Prism.Regions;

    /// <summary>
    ///     Basic class for bindable viewmodels.
    ///     Autogenerated by Prism.
    /// </summary>
    /// <seealso cref="Prism.Mvvm.BindableBase" />
    /// <seealso cref="Prism.Regions.INavigationAware" />
    /// <seealso cref="Prism.Navigation.IDestructible" />
    public abstract class ViewModelBase
        : BindableBase, INavigationAware, IDestructible
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModelBase" /> class.
        /// </summary>
        /// <param name="regionManager">
        ///     The region manager.
        /// </param>
        protected ViewModelBase(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;
        }

        /// <summary>
        ///     Gets the region manager.
        /// </summary>
        /// <value>
        ///     The region manager.
        /// </value>
        protected IRegionManager RegionManager { get; }

        /// <inheritdoc/>
        public virtual void Destroy()
        {
        }

        /// <inheritdoc/>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <inheritdoc/>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <inheritdoc/>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}