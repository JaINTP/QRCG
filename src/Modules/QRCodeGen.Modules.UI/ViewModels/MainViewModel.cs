// ***********************************************************************
// Assembly         : QRCodeGen.Modules.UI
// Author           : Jai Brown
// Created          : 18-01-2022
//
// Last Modified By : Jai Brown
// Last Modified On : 18-01-2022
// ***********************************************************************
// <copyright file="MainViewModel.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace JaINTP.QRCodeGen.Modules.UI.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using JaINTP.QRCodeGen.Core.Events;
    using JaINTP.QRCodeGen.Core.Mvvm;
    using MvvmDialogs;
    using MvvmDialogs.FrameworkDialogs.OpenFile;
    using MvvmDialogs.FrameworkDialogs.SaveFile;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;
    using QRCoder;
    using Color = System.Drawing.Color;

    /// <summary>
    ///     Enum to represent the different QR renderers supported.
    /// </summary>
    public enum QRCodeType
    {
        /// <summary>
        ///     Basic QR Code.
        /// </summary>
        QrCode,

        /// <summary>
        ///     "Artistic" QR Code.
        /// </summary>
        ArtQrCode,
    }

    /// <summary>
    ///     The Main Viewmodel.
    /// </summary>
    public class MainViewModel
        : RegionViewModelBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly SemaphoreSlim Semaphore = new (1, 1);
        private readonly CancellationTokenSource cancelTokenSource;
        private readonly DialogService dialogService;
        private readonly IEventAggregator eventAggregator;
        private readonly string noLogoPath = @"pack://application:,,,/QRCodeGen.Modules.UI;component/Resources/No_Logo.jpeg";

        private CancellationToken cancelToken;
        private bool imageSelected;
        private Bitmap? logoBackgroundBitmap;
        private ImageSource logoBackgroundImage;
        private PayloadGenerator.Payload payload;
        private QRCodeType qrType;
        private ImageSource? qrImageSource;
        private Color selectedBackground;
        private Color selectedDarkForeground;
        private Color selectedForeground;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="regionManager">
        ///     The region manager.
        /// </param>
        /// <param name="eventAggregator">
        ///     The event aggregator.
        /// </param>
        public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager)
        {
            this.dialogService = new DialogService();
            this.eventAggregator = eventAggregator;

            this.payload = new PayloadGenerator.Url("google.com");

            this.AddImageCommand = new DelegateCommand(this.AddImageCommandExecute);
            this.RemoveImageCommand = new DelegateCommand(this.RemoveImageCommandExecute);
            this.SaveImageCommand = new DelegateCommand(this.SaveImageCommandExecute);
            this.SelectionChangedCommand = new DelegateCommand<object>(this.SelectionChangedCommandExecute);

            this.cancelTokenSource = new CancellationTokenSource();
            this.cancelToken = CancellationToken.None;

            this.imageSelected = false;
            this.logoBackgroundImage = new BitmapImage(new Uri(this.noLogoPath));
            this.qrType = QRCodeType.QrCode;
            this.selectedBackground = Color.White;
            this.selectedDarkForeground = Color.Black;
            this.selectedForeground = Color.Transparent;

            this.PropertyChanged += this.MainViewModel_PropertyChanged;
            eventAggregator.GetEvent<PayloadEvent>()
                            .Subscribe(this.HandlePayloadEvent);

            this.GenerateQrCodes();
            Logger.Debug("MainViewModel created.");
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [image selected].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [image selected]; otherwise, <c>false</c>.
        /// </value>
        public bool ImageSelected
        {
            get => this.imageSelected;
            set
            {
                this.SetProperty(ref this.imageSelected, value);
                this.GenerateQrCodes();
            }
        }

        /// <summary>
        ///     Gets or sets the logo background image.
        /// </summary>
        /// <value>
        ///     The logo background image.
        /// </value>
        public ImageSource LogoBackgroundImage
        {
            get => this.logoBackgroundImage;
            set
            {
                this.SetProperty(ref this.logoBackgroundImage, value);
                this.GenerateQrCodes();
            }
        }

        /// <summary>
        ///     Gets or sets the QR payload.
        /// </summary>
        /// <value>
        ///     The payload.
        /// </value>
        public PayloadGenerator.Payload Payload
        {
            get => this.payload;
            set
            {
                this.SetProperty(ref this.payload, value);
                this.GenerateQrCodes();
            }
        }

        /// <summary>
        ///     Gets or sets the qr image source.
        /// </summary>
        /// <value>
        ///     The qr image source.
        /// </value>
        public ImageSource? QrImageSource
        {
            get => this.qrImageSource;
            set => this.SetProperty(ref this.qrImageSource, value);
        }

        /// <summary>
        ///     Gets or sets the qr code type.
        /// </summary>
        /// <value>
        ///     The qr code type.
        /// </value>
        public QRCodeType QRType
        {
            get => this.qrType;
            set
            {
                this.SetProperty(ref this.qrType, value);
                this.GenerateQrCodes();
            }
        }

        /// <summary>
        ///     Gets or sets the selected background.
        /// </summary>
        /// <value>
        ///     The selected background.
        /// </value>
        public Color SelectedBackground
        {
            get => this.selectedBackground;
            set
            {
                this.SetProperty(ref this.selectedBackground, value);
                this.GenerateQrCodes();
            }
        }

        /// <summary>
        ///     Gets or sets the dark selected foreground.
        /// </summary>
        /// <value>
        ///     The dark selected foreground.
        /// </value>
        public Color SelectedDarkForeground
        {
            get => this.selectedDarkForeground;
            set
            {
                this.SetProperty(ref this.selectedDarkForeground, value);
                this.GenerateQrCodes();
            }
        }

        /// <summary>
        ///     Gets or sets the selected foreground.
        /// </summary>
        /// <value>
        ///     The selected foreground.
        /// </value>
        public Color SelectedForeground
        {
            get => this.selectedForeground;
            set
            {
                this.SetProperty(ref this.selectedForeground, value);
                this.GenerateQrCodes();
            }
        }

        /// <summary>
        ///     Gets the open command.
        /// </summary>
        /// <value>
        ///     The open command.
        /// </value>
        public ICommand AddImageCommand { get; private set; }

        /// <summary>
        ///     Gets the remove command.
        /// </summary>
        /// <value>
        ///     The remove command.
        /// </value>
        public ICommand RemoveImageCommand { get; private set; }

        /// <summary>
        ///     Gets the save image command.
        /// </summary>
        /// <value>
        ///     The save image command.
        /// </value>
        public ICommand SaveImageCommand { get; private set; }

        /// <summary>
        ///     Gets the selection changed command.
        /// </summary>
        /// <value>
        ///     The selection changed command.
        /// </value>
        public ICommand SelectionChangedCommand { get; private set; }

        /// <summary>
        ///     Converts a Bitmap to a BitmapImage.
        /// </summary>
        /// <param name="bitmap">
        ///     The Bitmap to convert.
        /// </param>
        /// <returns>
        ///     The BitmapImage equivalent of the provided Bitmap.
        /// </returns>
        private static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using var memory = new MemoryStream();

            var bitmapImage = new BitmapImage();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            bitmap.Dispose();
            return bitmapImage;
        }

        /// <summary>
        ///     Converts a BitmapImage to a Bitmap.
        /// </summary>
        /// <param name="bitmapImage">
        ///     The BitmapImage to convert.
        /// </param>
        /// <returns>
        ///     The Bitmap equivalent of the provided BitmapImage.
        /// </returns>
        private static Bitmap ToBitmap(BitmapImage bitmapImage)
        {
            using var outStream = new MemoryStream();

            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(outStream);
            var bitmap = new System.Drawing.Bitmap(outStream);

            return new Bitmap(bitmap);
        }

        /// <summary>
        ///     The AddCommand's execute method.
        /// </summary>
        private void AddImageCommandExecute()
        {
            Logger.Debug("AddImageCommand executed.");

            var settings = new OpenFileDialogSettings
            {
                Title = "Select Logo Image",
                InitialDirectory = Environment.SpecialFolder.MyPictures.ToString(),
                Filter = "Image Files|*.jpg;*.jpeg;*.png;",
                Multiselect = false,
            };

            this.ImageSelected = (bool)this.dialogService.ShowOpenFileDialog(this, settings);

            if (this.ImageSelected)
            {
                this.LogoBackgroundImage = new BitmapImage(new Uri(settings.FileName));
                this.logoBackgroundBitmap = ToBitmap((BitmapImage)this.LogoBackgroundImage);
            }

            this.GenerateQrCodes();
        }

        /// <summary>
        ///     The RemoveCommand's execute method.
        /// </summary>
        private void RemoveImageCommandExecute()
        {
            Logger.Debug("RemoveImageCommand executed.");
            this.ImageSelected = false;
            this.LogoBackgroundImage = new BitmapImage(new Uri(this.noLogoPath));
            this.logoBackgroundBitmap = null;

            this.GenerateQrCodes();
        }

        /// <summary>
        ///     The SaveImageCommand's execute method.
        /// </summary>
        private void SaveImageCommandExecute()
        {
            Logger.Debug("SaveImageCommand executed.");

            var settings = new SaveFileDialogSettings
            {
                Title = "Save QR Code Image",
                InitialDirectory = Environment.SpecialFolder.MyPictures.ToString(),
                Filter = "Image Files|*.jpg;*.jpeg;",
            };

            var success = this.dialogService.ShowSaveFileDialog(this, settings);

            if (success == true)
            {
                try
                {
                    var tempBitmap = ToBitmap(this.QrImageSource as BitmapImage);
                    tempBitmap.Save(settings.FileName, ImageFormat.Bmp);
                }
                catch (Exception ex)
                {
                    Logger.Debug(ex, "Exception caught in SaveImageCommandExecute.");
                }
            }
        }

        /// <summary>
        ///     The SelectionCommand's execute method.
        /// </summary>
        /// <param name="obj">
        ///     The parameter object passed to the command.
        /// </param>
        private void SelectionChangedCommandExecute(object obj)
        {
            Logger.Debug("SelectionChangedCommand executed.");
            var header = ((System.Windows.Controls.TabItem)obj).Header;
            this.eventAggregator.GetEvent<SelectedTabChangedEvent>()
                                .Publish((string)header);
        }

        /// <summary>
        ///     The Payload event handler.
        /// </summary>
        /// <param name="payload">
        ///     The payload.
        /// </param>
        private void HandlePayloadEvent(PayloadGenerator.Payload payload)
        {
            Logger.Debug("HandlePayload event called.");
            this.Payload = payload;
        }

        /// <summary>
        ///     Handles the PropertyChanged event of the MainViewModel control.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event.
        /// </param>
        /// <param name="e">
        ///     The <see cref="PropertyChangedEventArgs" /> instance containing the event data.
        /// </param>
        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Debug("MainViewModel_PropertyChanged event called.");
            if (Semaphore != null && Semaphore.CurrentCount == 0)
            {
                this.cancelTokenSource.Cancel();
            }
        }

        /// <summary>
        ///     Generates the qr codes.
        /// </summary>
        private void GenerateQrCodes()
        {
            if (this.QRType == QRCodeType.ArtQrCode)
            {
                this.QrImageSource = this.GenerateArtQrCode(
                    this.payload,
                    20,
                    this.SelectedDarkForeground,
                    this.SelectedForeground,
                    this.SelectedBackground,
                    this.logoBackgroundBitmap,
                    0.6,
                    true);
            }
            else if (this.QRType == QRCodeType.QrCode)
            {
                this.QrImageSource = this.GenerateQrCode(
                    this.payload,
                    20,
                    this.SelectedDarkForeground,
                    this.SelectedBackground,
                    this.logoBackgroundBitmap,
                    15,
                    0,
                    true,
                    null);
            }
        }

        /// <summary>
        ///     Generates a qr code.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="pixelsPerModule">The pixels per module.</param>
        /// <param name="foregroundColour">The foreground colour.</param>
        /// <param name="backgroundColour">The background colour.</param>
        /// <param name="iconBitmap">The icon bitmap.</param>
        /// <param name="iconSizePercent">The icon size percentage.</param>
        /// <param name="iconBorderWidth">Width of the icon border.</param>
        /// <param name="drawQuietZones">if set to <c>true</c> [draw quiet zones].</param>
        /// <param name="iconBackgroundColor">Color of the icon background.</param>
        /// <returns>
        ///   A BitmapImage containing a newly generated QR code.
        /// </returns>
        private BitmapImage? GenerateQrCode(
            PayloadGenerator.Payload payload,
            int pixelsPerModule,
            Color foregroundColour,
            Color backgroundColour,
            Bitmap? iconBitmap = null,
            int iconSizePercent = 15,
            int iconBorderWidth = 0,
            bool drawQuietZones = true,
            Color? iconBackgroundColor = null)
        {
            Logger.Debug("In GenerateArtQrCode.");
            this.cancelToken = this.cancelTokenSource.Token;

            Task<BitmapImage?> task = Task.Run(
                async () =>
                {
                    await Semaphore.WaitAsync();
                    BitmapImage? bitmapImage = null;

                    try
                    {
                        using var qrGenerator = new QRCodeGenerator();
                        using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                        using var qrCode = new QRCode(qrCodeData);
                        var tempBitmap = qrCode.GetGraphic(
                                pixelsPerModule,        // pixelsPerModule
                                foregroundColour,       // darkColor
                                backgroundColour,       // lightColor
                                iconBitmap,             // icon
                                iconSizePercent,        // iconSizePercent
                                iconBorderWidth,        // iconBorderWidth
                                drawQuietZones,         // drawQuietZones
                                iconBackgroundColor);   // iconBackgroundColor
                        bitmapImage = ToBitmapImage(tempBitmap);
                        tempBitmap.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(ex, "Exception caught in GenerateArtQrCode.");
                    }

                    Semaphore.Release();

                    return bitmapImage;
                },
                this.cancelToken);

            return task.Result;
        }

        /// <summary>
        ///     Generates an art qr code.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="pixelsPerModule">The pixels per module.</param>
        /// <param name="darkForegroundColor">Color of the dark foreground.</param>
        /// <param name="lightForegroundColor">Color of the light foreground.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="backgroundImage">The background image.</param>
        /// <param name="pixelSizeFactor">The pixel size factor.</param>
        /// <param name="drawQuietZones">if set to <c>true</c> [draw quiet zones].</param>
        /// <param name="quietZoneStyle">The quiet zone style.</param>
        /// <param name="backgroundImageStyle">The background image style.</param>
        /// <param name="finderPatternImage">The finder pattern image.</param>
        /// <returns>
        ///   A BitmapImage containing a newly generated QR code.
        /// </returns>
        private BitmapImage? GenerateArtQrCode(
            PayloadGenerator.Payload payload,
            int pixelsPerModule,
            Color darkForegroundColor,
            Color lightForegroundColor,
            Color backgroundColor,
            Bitmap? backgroundImage = null,
            double pixelSizeFactor = 0.6,
            bool drawQuietZones = true,
            ArtQRCode.QuietZoneStyle quietZoneStyle = ArtQRCode.QuietZoneStyle.Flat,
            ArtQRCode.BackgroundImageStyle backgroundImageStyle = ArtQRCode.BackgroundImageStyle.Fill,
            Bitmap? finderPatternImage = null)
        {
            Logger.Debug("In GenerateArtQrCode.");
            this.cancelToken = this.cancelTokenSource.Token;

            Task<BitmapImage?> task = Task.Run(
                async () =>
                {
                    await Semaphore.WaitAsync();
                    BitmapImage? bitmapImage = null;

                    try
                    {
                        using var qrGenerator = new QRCodeGenerator();
                        using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                        using var qrCode = new ArtQRCode(qrCodeData);
                        var tempBitmap = qrCode.GetGraphic(
                                pixelsPerModule,        // pixelsPerModule
                                darkForegroundColor,    // darkColor
                                lightForegroundColor,   // lightColor
                                backgroundColor,        // backgroundColor
                                backgroundImage,        // backgroundImage
                                pixelSizeFactor,        // pixelSizeFactor
                                drawQuietZones,         // drawQuietZones
                                quietZoneStyle,         // quietZoneRenderingStyle
                                backgroundImageStyle,   // backgroundImageStyle
                                finderPatternImage);    // finderPatternImage
                        bitmapImage = ToBitmapImage(tempBitmap);
                        tempBitmap.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(ex, "Exception caught in GenerateArtQrCode.");
                    }

                    Semaphore.Release();
                    return bitmapImage;
                },
                this.cancelToken);

            return task.Result;
        }
    }
}
