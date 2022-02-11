// ***********************************************************************
// Assembly         : QRCodeGen
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="UIModule.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Modules.UI
{
    using JaINTP.QRCodeGen.Core;
    using JaINTP.QRCodeGen.Modules.UI.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    public class UIModule
        : IModule
    {
        /// <summary>
        ///     Notifies the module that it has been initialized.
        /// </summary>
        /// <param name="containerProvider">
        ///     The container provider.
        /// </param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.MainRegion, nameof(MainView));
        }

        /// <summary>
        ///     Used to register types with the container that will be used by your application.
        /// </summary>
        /// <param name="containerRegistry">
        ///     The container registry.
        /// </param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView>();
            containerRegistry.RegisterForNavigation<SettingsView>();
        }
    }
}
