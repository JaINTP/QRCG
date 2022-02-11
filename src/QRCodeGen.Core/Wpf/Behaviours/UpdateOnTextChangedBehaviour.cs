// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 30-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 30-01-2022
// ***********************************************************************
// <copyright file="UpdateOnTextChangedBehaviour.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Wpf.Behaviours
{
    using System.Windows.Controls;

    /// <summary>
    ///     TextBox behaviour.
    /// </summary>
    public class UpdateOnTextChangedBehaviour
        : AttachableForStyleBehaviour<TextBox, UpdateOnTextChangedBehaviour>
    {
        /// <summary>
        ///     Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///     Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.TextChanged += this.AssociatedObject_TextChanged;
        }

        /// <summary>
        ///     Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///     Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.TextChanged -= this.AssociatedObject_TextChanged;
        }

        /// <summary>
        ///     Handles the TextChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="TextChangedEventArgs" /> instance containing the event data.
        /// </param>
        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.AssociatedObject.GetBindingExpression(TextBox.TextProperty)
                                 .UpdateSource();
        }
    }
}
