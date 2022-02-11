// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 30-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 30-01-2022
// ***********************************************************************
// <copyright file="AttachableForStyleBehaviour.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Wpf.Behaviours
{
    using System.Linq;
    using System.Windows;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    ///     Amazing piece of behaviour code found at https://stackoverflow.com/a/31292989.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    /// <typeparam name="TBehavior">The type of the behavior.</typeparam>
    /// <seealso cref="Microsoft.Xaml.Behaviors.Behavior&lt;TComponent&gt;" />
    public class AttachableForStyleBehaviour<TComponent, TBehavior>
        : Behavior<TComponent>
        where TComponent : DependencyObject
        where TBehavior : AttachableForStyleBehaviour<TComponent, TBehavior>, new()
    {
        /// <summary>
        ///     The is enabled for style property.
        /// </summary>
        private static readonly DependencyProperty IsEnabledForStyleProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabledForStyle",
                typeof(bool),
                typeof(AttachableForStyleBehaviour<TComponent, TBehavior>),
                new FrameworkPropertyMetadata(false, OnIsEnabledForStyleChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is enabled for style.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is enabled for style; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledForStyle
        {
            get => (bool)this.GetValue(IsEnabledForStyleProperty);
            set => this.SetValue(IsEnabledForStyleProperty, value);
        }

        /// <summary>
        ///     Called when [is enabled for style changed].
        /// </summary>
        /// <param name="d">
        ///     The dependency object.
        /// </param>
        /// <param name="e">
        ///     The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.
        /// </param>
        private static void OnIsEnabledForStyleChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uie)
            {
                var behColl = Interaction.GetBehaviors(uie);
                var existingBehavior = behColl.FirstOrDefault(b => b.GetType() == typeof(TBehavior)) as TBehavior;

                if ((bool)e.NewValue == false && existingBehavior != null)
                {
                    behColl.Remove(existingBehavior);
                }
                else if ((bool)e.NewValue == true && existingBehavior == null)
                {
                    behColl.Add(new TBehavior());
                }
            }
        }
    }
}