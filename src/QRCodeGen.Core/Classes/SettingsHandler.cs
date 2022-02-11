// ***********************************************************************
// Assembly         : QRCodeGen.Core
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="SettingsHandler.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Core.Classes
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using Prism.Mvvm;

    /// <summary>
    /// Class designed to handle the serializing and deserializing of json based configuration
    /// files.
    ///     Got the idea from https://stackoverflow.com/a/6541739/2695708
    ///
    /// Inherits from BindableBase 'cause I'm Lazy and want a PropertyChanged event to save
    ///     Settings to file.
    /// </summary>
    /// <typeparam name="T">The Settings Class provided by the user.</typeparam>
    public class SettingsHandler<T>
        : BindableBase
        where T : class, new()
    {
        private string? filePath;

        /// <summary>
        ///     Gets or sets the settings file path.
        /// </summary>
        /// <value>
        ///     The file path.
        /// </value>
        [JsonIgnore]
        public virtual string? FilePath
        {
            get => this.filePath;
            set => this.SetProperty(ref this.filePath, value);
        }

        /// <summary>
        ///     Loads the Json based settings file and creates a settings class from it.
        ///     Uses a shitty hack to generate an object of type T via its JsonProperty attributes.
        /// </summary>
        /// <param name="filePath">
        ///     The fully qualified path to the settings file.
        /// </param>
        /// <returns>
        ///     Deserialised object of type T.
        /// </returns>
        public static T Load(string filePath)
        {
            T t;

            if (File.Exists(filePath))
            {
                using var streamReader = new StreamReader(filePath);
                using var jsonReader = new JsonTextReader(streamReader);
                var jsonSerializer = new JsonSerializer();
                t = jsonSerializer.Deserialize<T>(jsonReader);
            }
            else
            {
                // Cheap hack to have Newtonsoft build out settings file from the JsonProperty.
                // attributes in the Settings class. Will probably throw exceptions if you don't
                // have them...
                t = JsonConvert.DeserializeObject<T>("{}");
            }

            var type = typeof(T);
            var propInfo = type.GetProperty("FilePath");
            propInfo.SetValue(t, Convert.ChangeType(filePath, propInfo.PropertyType), null);

            return t;
        }

        /// <summary>
        ///     Saves the settings file as Json to the specified location.
        ///     Creates the parent directory in the path if it doesn't exist.
        /// </summary>
        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(this.FilePath));

            var jsonSerializer = new JsonSerializer();
            using var streamWriter = new StreamWriter(this.FilePath);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            jsonSerializer.Serialize(jsonWriter, this);
        }
    }
}