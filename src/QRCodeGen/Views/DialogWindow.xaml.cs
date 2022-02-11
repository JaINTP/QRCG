//// *********************************************************************
// Assembly         : QRCodeGen
// Author           : Jai Brown
// Created          : 11-02-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 11-02-2022
// ***********************************************************************
// <copyright file="DialogWindow.xaml.cs" company="jaibr">
//     Copyright (c) jaibr. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Views
{
    using Prism.Services.Dialogs;

    /// <summary>
    ///     Interaction logic for DialogWindow.xaml.
    /// </summary>
    public partial class DialogWindow
        : MahApps.Metro.Controls.MetroWindow, IDialogWindow
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogWindow" /> class.
        /// </summary>
        public DialogWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the result of the dialog.
        /// </summary>
        public IDialogResult Result { get; set; }
    }
}
