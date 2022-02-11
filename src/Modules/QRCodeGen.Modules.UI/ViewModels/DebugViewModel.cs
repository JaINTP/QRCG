//// *********************************************************************
// Assembly         : $projectname$
// Author           : jaibr
// Created          : 18-01-2022
//
// Last Modified By : jaibr
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="DebugViewModel.cs" company="jaibr">
//     Copyright (c) jaibr. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Modules.UI.ViewModels
{
    using System;
    using System.Windows.Input;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Services.Dialogs;

    [RegionMemberLifetime(KeepAlive = true)]
    public class DebugViewModel
        : BindableBase, IDialogAware
    {
        private string title = "Debug Console";
        private int height = 400;
        private int width = 600;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DebugViewModel" /> class.
        /// </summary>
        public DebugViewModel()
        {
            this.CloseDialogCommand = new DelegateCommand<string>(this.CloseDialog);
        }

        /// <summary>
        ///     Instructs the <see cref="T:Prism.Services.Dialogs.IDialogWindow" /> to close the dialog.
        /// </summary>
        public event Action<IDialogResult> RequestClose;

        /// <summary>
        ///     Gets or sets the title of the dialog that will show in the window title bar.
        /// </summary>
        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        /// <summary>
        ///     Gets or sets the dialog height.
        /// </summary>
        /// <value>
        ///     The dialog height.
        /// </value>
        public int Height
        {
            get => this.height;
            set => this.SetProperty(ref this.height, value);
        }

        /// <summary>
        ///     Gets or sets the dialog width.
        /// </summary>
        /// <value>
        ///     The dialog width.
        /// </value>
        public int Width
        {
            get => this.width;
            set => this.SetProperty(ref this.width, value);
        }

        /// <summary>
        ///     Gets the CloseDialogCommand.
        /// </summary>
        /// <value>
        ///     The close dialog command.
        /// </value>
        public ICommand CloseDialogCommand { get; private set; }

        /// <summary>
        ///     Closes the dialog.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        public void CloseDialog(string parameter)
        {
            var result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.OK;
            }
            else if (parameter?.ToLower() == "false")
            {
                result = ButtonResult.Cancel;
            }

            this.RaiseRequestClose(new DialogResult(result));
        }

        /// <summary>
        ///     Raises the request close.
        /// </summary>
        /// <param name="dialogResult">
        ///     The dialog result.
        /// </param>
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            this.RequestClose?.Invoke(dialogResult);
        }

        /// <summary>
        ///     Determines if the dialog can be closed.
        /// </summary>
        /// <returns>
        ///     If <c>true</c> the dialog can be closed. If <c>false</c> the dialog will not close.
        /// </returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        ///     Called when the dialog is closed.
        /// </summary>
        public void OnDialogClosed()
        {
        }

        /// <summary>
        ///     Called when the dialog is opened.
        /// </summary>
        /// <param name="parameters">
        ///     The parameters passed to the dialog.
        /// </param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
