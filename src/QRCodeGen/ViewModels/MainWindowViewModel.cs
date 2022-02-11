// ***********************************************************************
// Assembly         : QRCodeGen
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="MainWindowViewModel.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.ViewModels
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Input;
    using JaINTP.QRCodeGen.Core.Mvvm;
    using Prism.Commands;
    using Prism.Regions;
    using Prism.Services.Dialogs;

    /// <summary>
    ///     MainWindow's viewmodel.
    /// </summary>
    public class MainWindowViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IDialogService dialogService;
        private string title = $"QRCodeGen v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        /// <param name="regionManager">
        ///     The region manager.
        /// </param>
        /// <param name="dialogService">
        ///     The dialog service.
        /// </param>
        public MainWindowViewModel(IRegionManager regionManager, IDialogService dialogService)
            : base(regionManager)
        {
            this.dialogService = dialogService;
            this.DebugCommand = new DelegateCommand(this.DebugCommandExecute);
            this.GithubCommand = new DelegateCommand(this.GithubCommandExecute);
            this.SettingsCommand = new DelegateCommand(this.SettingsCommandExecute);

            Logger.Debug("MainWindowViewModel created.");
        }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        /// <summary>
        ///     Gets the debug command.
        /// </summary>
        /// <value>
        ///     The debug command.
        /// </value>
        public ICommand DebugCommand { get; private set; }

        /// <summary>
        ///     Gets the github command.
        /// </summary>
        /// <value>
        ///     The github command.
        /// </value>
        public ICommand GithubCommand { get; private set; }

        /// <summary>
        ///     Gets the settings command.
        /// </summary>
        /// <value>
        ///     The settings command.
        /// </value>
        public ICommand SettingsCommand { get; private set; }

        private void DebugCommandExecute()
        {
            this.dialogService.Show(
                nameof(Modules.UI.Views.DebugView),
                new DialogParameters(),
                r => { },
                nameof(DialogWindow));
        }

        /// <summary>
        ///     The GithubCommand's execute method.
        /// </summary>
        private void GithubCommandExecute()
        {
            Logger.Debug("GithubCommand executed.");

            var psInfo = new ProcessStartInfo(@"https://github.com/JaINTP")
            {
                UseShellExecute = true,
                Verb = "open",
            };

            Process.Start(psInfo);
        }

        /// <summary>
        ///     The SettingsCommand's execute method.
        /// </summary>
        private void SettingsCommandExecute()
        {
            Logger.Debug("SettingsCommand executed.");
            this.RegionManager.Regions["MainRegion"].RequestNavigate("SettingsView");
        }
    }
}
