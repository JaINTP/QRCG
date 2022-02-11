// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 23-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 23-01-2022
// ***********************************************************************
// <copyright file="TabControlRegionAdapter.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Mvvm
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Controls;
    using Prism.Regions;

    /// <summary>
    ///     Basic region adapter for TabControls.
    /// </summary>
    /// <seealso cref="Prism.Regions.RegionAdapterBase" />
    public class TabControlRegionAdapter
        : RegionAdapterBase<TabControl>
    {
        private TabControl regionTarget;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TabControlRegionAdapter" /> class.
        /// </summary>
        /// <param name="regionBehaviorFactory">
        ///     The factory used to create the region behaviors to attach to the created regions.
        /// </param>
        public TabControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        /// <inheritdoc/>
        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            region.Views.CollectionChanged += this.Views_CollectionChanged;
            this.regionTarget = regionTarget;
        }

        /// <inheritdoc/>
        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        /// <summary>
        ///     Handles the CollectionChanged event of the Views control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.
        /// </param>
        private void Views_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (UserControl item in e.NewItems)
                {
                    this.regionTarget.Items.Add(new TabItem
                    {
                        Header = item.Name,
                        Content = item,
                    });
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.NewItems)
                {
                    var itemToRemove =
                        this.regionTarget.Items
                                         .OfType<TabItem>()
                                         .FirstOrDefault(n => n.Content == item as UserControl);
                    this.regionTarget.Items.Remove(itemToRemove);
                }
            }
        }
    }
}
