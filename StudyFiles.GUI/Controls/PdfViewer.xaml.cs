using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Windows.Data.Pdf;
using Windows.Storage.Streams;

namespace StudyFiles.GUI.Controls
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer
    {

        #region Bindable Properties

        public IList<System.Drawing.Image> ImageSource
        {
            get => (IList<System.Drawing.Image>) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(IList<System.Drawing.Image>), typeof(PdfViewer),
                new PropertyMetadata(null, propertyChangedCallback: OnImageSourceChanged));

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pdfDrawer = (PdfViewer) d;

            var items = pdfDrawer.PagesContainer.Items;
            items.Clear();

            pdfDrawer.ImageSource
                .Select(ConvertImage)
                .ToList()
                .ForEach(img => items.Add(img));

            //if (string.IsNullOrEmpty(pdfDrawer.ImageSource))
            //    return;

            //var path = Path.GetFullPath(pdfDrawer.ImageSource);

            //StorageFile.GetFileFromPathAsync(path).AsTask()
            //    //load pdf document on background thread
            //    .ContinueWith(task => PdfDocument.LoadFromFileAsync(task.Result).AsTask()).Unwrap()
            //    //display on UI Thread
            //    .ContinueWith(task => PdfToImages(pdfDrawer, task.Result),
            //        TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion


        public PdfViewer()
        {
            InitializeComponent();
        }

        private static Image ConvertImage(System.Drawing.Image image)
        {
            var img = new Image();

            var bmp = new System.Drawing.Bitmap(image);
            IntPtr hBitmap = bmp.GetHbitmap();
            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = wpfBitmap;
            img.Width = 810;
            img.Height = 1110;
            img.Stretch = System.Windows.Media.Stretch.Fill;

            return img;
        }

        private static async Task PdfToImages(PdfViewer pdfViewer, PdfDocument pdfDoc)
        {
            var items = pdfViewer.PagesContainer.Items;
            items.Clear();

            if (pdfDoc == null)
                return;

            for (uint i = 0; i < pdfDoc.PageCount; i++)
            {
                using var page = pdfDoc.GetPage(i);

                var bitmap = await PageToBitmapAsync(page);
                var image = new Image
                {
                    Source = bitmap,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 4, 0, 4),
                    MaxWidth = 800
                };
                items.Add(image);
            }
        }

        private static async Task<BitmapImage> PageToBitmapAsync(PdfPage page)
        {
            var image = new BitmapImage();

            using (var stream = new InMemoryRandomAccessStream())
            {
                await page.RenderToStreamAsync(stream);

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream.AsStream();
                image.EndInit();
            }

            return image;
        }

    }
}
