// ***********************************************************************
// Assembly         : QRCodeGen.Modules.Payloads
// Author           : Jai Brown
// Created          : 23-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 26-01-2022
// ***********************************************************************
// <copyright file="PayloadsModule.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Modules.Payloads
{
    using JaINTP.QRCodeGen.Core;
    using JaINTP.QRCodeGen.Modules.Payloads.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    public class PayloadsModule
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
            regionManager.RegisterViewWithRegion(RegionNames.PayloadRegion, typeof(UrlPayloadView));
            regionManager.RegisterViewWithRegion(RegionNames.PayloadRegion, typeof(MeCardPayloadView));
            regionManager.RegisterViewWithRegion(RegionNames.PayloadRegion, typeof(PhonePayloadView));
            regionManager.RegisterViewWithRegion(RegionNames.PayloadRegion, typeof(BookmarkPayloadView));
            regionManager.RegisterViewWithRegion(RegionNames.PayloadRegion, typeof(WifiPayloadView));
            regionManager.RegisterViewWithRegion(RegionNames.PayloadRegion, typeof(MailPayloadView));
            regionManager.RegisterViewWithRegion(RegionNames.PayloadRegion, typeof(GeoPayloadView));
        }

        /// <summary>
        ///     Used to register types with the container that will be used by your application.
        /// </summary>
        /// <param name="containerRegistry">
        ///     The container registry.
        /// </param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}