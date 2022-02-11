// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 24-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 24-01-2022
// ***********************************************************************
// <copyright file="PayloadEvent.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Events
{
    using Prism.Events;
    using QRCoder;

    /// <summary>
    /// Filthy fucking hack until I can figure out how to do this right..
    /// </summary>
    /// <seealso cref="Prism.Events.PubSubEvent&lt;TPayload&gt;" />
    public class PayloadEvent
        : PubSubEvent<PayloadGenerator.Payload>
    {
    }
}
