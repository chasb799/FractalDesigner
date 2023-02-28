using FractalDesigner.Model;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FractalDesigner.ViewModel
{
    public class FractalVM : INotifyPropertyChanged
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private WriteableBitmap _imageBitmap;

        public WriteableBitmap ImageBitmap
        {
            get => _imageBitmap;
            set
            {
                _imageBitmap = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The image size.
        /// </summary>

        private int _imageWidth;

        public int ImageWidth
        {
            set
            {
                _imageWidth = value;
                OnPropertyChanged();
            }
        }

        private int _imageHeight;

        public int ImageHeight
        {
            set
            {
                _imageHeight = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The user parameter input.
        /// </summary>
        private string _parameterInput;

        public string ParameterInput
        {
            get => _parameterInput;
            set
            {
                _parameterInput = value;
                OnPropertyChanged();
            }
        }

        private ICommand _imageLoadedCommand;

        public ICommand ImageLoadedCommand
        {
            get
            {
                if (_imageLoadedCommand == null)
                {
                    _imageLoadedCommand = new RelayCommand<RoutedEventArgs>(GetSize);
                }

                return _imageLoadedCommand;
            }
        }

        private ICommand _sizeChangedCommand;

        public ICommand SizeChangedCommand
        {
            get
            {
                if (_sizeChangedCommand == null)
                {
                    _sizeChangedCommand = new RelayCommand<RoutedEventArgs>(GetSize);
                }

                return _sizeChangedCommand;
            }
        }

        private ICommand _drawFractalsCommand;

        public ICommand DrawFractalsCommand
        {
            get
            {
                if (_drawFractalsCommand == null)
                {
                    _drawFractalsCommand =
                        new RelayCommand(GenerateBitmap);
                }

                return _drawFractalsCommand;
            }
        }

        private ICommand _clearImageCommand;

        public ICommand ClearImageCommand
        {
            get
            {
                if (_clearImageCommand == null)
                {
                    _clearImageCommand = new RelayCommand(ClearBitmap);
                }

                return _clearImageCommand;
            }
        }

        private ICommand _closeWindowCommand;

        public ICommand CloseWindowCommand
        {
            get
            {
                if (_closeWindowCommand == null)
                {
                    _closeWindowCommand = new RelayCommand(CloseWindow);
                }
                return _closeWindowCommand;
            }
        }

        private void GetSize(RoutedEventArgs args)
        {
            if (args.Source is Canvas canvas)
            {
                _imageWidth = (int)Math.Floor(canvas.ActualWidth);
                _imageHeight = (int)Math.Floor(canvas.ActualHeight);
            }
        }

        /// <summary>
        /// Calculate the points in an ink canvas using escape time algorithm for mandelbrot set.
        /// </summary>
        private void GenerateBitmap()
        {
            ImageBitmap = new WriteableBitmap(_imageWidth, _imageHeight, 96, 96, PixelFormats.Bgra32, null);
            var pixels = new byte[_imageBitmap.PixelWidth * _imageBitmap.PixelHeight * _imageBitmap.Format.BitsPerPixel / 8];

            const int maxIterations = 100;
            var mandelbrot = new Mandelbrot();

            double reStart = -1;
            double reEnd = 1;
            double imStart = -1;
            double imEnd = 1;

            var i = 0;
            for (var x = 0; x < _imageBitmap.PixelWidth; x++)
            {
                for (var y = 0; y < _imageBitmap.PixelHeight; y++)
                {
                    var c = new Complex(reStart + ((double)x/(double)_imageBitmap.PixelWidth) * ((double)reEnd-(double)reStart), 
                            imStart + ((double)-y/(double)_imageBitmap.PixelHeight) * ((double)imEnd-(double)imStart));
                    var iterations = (double)mandelbrot.CalculateIterations(c, maxIterations);
                    var color = (double)255 - 2.55*iterations;

                    pixels[i] = (byte)color;
                    pixels[i + 1] = (byte)color;
                    pixels[i + 2] = (byte)color;
                    pixels[i + 3] = 255;
                    i += 4;
                }
            }

            ImageBitmap.WritePixels(new Int32Rect(0, 0, _imageBitmap.PixelWidth, _imageBitmap.PixelHeight), pixels, _imageBitmap.PixelWidth * _imageBitmap.Format.BitsPerPixel / 8, 0);
        }

        private void ClearBitmap()
        {
            var pixels = new byte[_imageBitmap.PixelWidth * _imageBitmap.PixelHeight * _imageBitmap.Format.BitsPerPixel / 8];

            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = 220;
            }
            _imageBitmap.WritePixels(new Int32Rect(0,0, _imageBitmap.PixelWidth, _imageBitmap.PixelHeight), pixels, _imageBitmap.PixelWidth *_imageBitmap.Format.BitsPerPixel / 8, 0);
        }

        private void CloseWindow()
        {
            var windows = Application.Current.Windows.OfType<Window>();
            var activeWindow = windows.FirstOrDefault(x => x.IsActive);
            if (activeWindow != null)
            {
                activeWindow.Close();
            }
        }

        public FractalVM()
        {
        }

        /// <summary>
        /// Implementation of INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}