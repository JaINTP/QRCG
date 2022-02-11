// ***********************************************************************
// Assembly         : QRCodeGen
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="App.xaml.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen
{
    using System.Windows;
    using System.Windows.Controls;
    using JaINTP.QRCodeGen.Core.Mvvm;
    using JaINTP.QRCodeGen.Modules.Payloads;
    using JaINTP.QRCodeGen.Modules.UI;
    using JaINTP.QRCodeGen.Modules.UI.ViewModels;
    using JaINTP.QRCodeGen.Modules.UI.Views;
    using JaINTP.QRCodeGen.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Prism.Unity;

    /// <summary>
    ///     Interaction logic for App.xaml.
    /// </summary>
    public partial class App
        : PrismApplication
    {
        protected override Window CreateShell()
        {
            this.Container.Resolve<DebugViewModel>();
            this.Container.Resolve<SettingsViewModel>();
            return this.Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<PayloadsModule>();
            moduleCatalog.AddModule<UIModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(
                typeof(TabControl),
                this.Container.Resolve<TabControlRegionAdapter>());
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<DialogWindow>("DialogWindow");
            containerRegistry.RegisterDialog<DebugView, DebugViewModel>();
            containerRegistry.RegisterForNavigation<MainView>();
            containerRegistry.RegisterForNavigation<SettingsView>();
            containerRegistry.RegisterForNavigation<DebugView>();
        }
    }
}
