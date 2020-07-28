using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Core.Enums;

using Wpf.ViewModels.Enums;

namespace Wpf.Views.Controls
{
    public static class ControlUtils
    {
        public static readonly BitmapImage Empty = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/Empty.png"));

        public static readonly BitmapImage BlackCell = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/BlackCell.png"));
        public static readonly BitmapImage WhiteCell = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/WhiteCell.png"));

        public static readonly BitmapImage BlackMen = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/BlackMen.png"));
        public static readonly BitmapImage BlackKing = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/BlackKing.png"));

        public static readonly BitmapImage WhiteMen = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/WhiteMen.png"));
        public static readonly BitmapImage WhiteKing = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/WhiteKing.png"));

        public static Image ConstructImage(BitmapImage bitmap) =>
            bitmap != null ? new Image { Source = bitmap } : new Image();

        public static Image ConstructImage(Core.Enums.CellState cellType) =>
            ConstructImage(GetBitmapByType(cellType));

        public static BitmapImage GetBitmapByType(CellType cellType) =>
            cellType switch
            {
                CellType.Black => BlackCell,
                CellType.White => WhiteCell,
                _ => null,
            };

        public static BitmapImage GetBitmapByType(Core.Enums.CellState cellState) =>
            cellState switch
            {
                Core.Enums.CellState.BlackMen => BlackMen,
                Core.Enums.CellState.BlackKing => BlackKing,
                Core.Enums.CellState.WhiteMen => WhiteMen,
                Core.Enums.CellState.WhiteKing => WhiteKing,
                _ => null,
            };

        private static BitmapImage ConstructBitmap(Uri imageUri)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = imageUri;
            bitmap.EndInit();

            return bitmap;
        }
    }
}
