using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Core.Enums;

using Wpf.ViewModels.Enums;

namespace Wpf.Views.Controls
{
    internal static class ControlUtils
    {
        public static readonly BitmapImage Empty = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/Empty.png"));

        public static readonly BitmapImage BlackCell = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/BlackCell.png"));
        public static readonly BitmapImage WhiteCell = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/WhiteCell.png"));

        public static readonly BitmapImage BlackMan = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/BlackMan.png"));
        public static readonly BitmapImage BlackKing = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/BlackKing.png"));

        public static readonly BitmapImage WhiteMan = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/WhiteMan.png"));
        public static readonly BitmapImage WhiteKing = ConstructBitmap(new Uri("pack://application:,,,/Wpf;component/Images/WhiteKing.png"));

        public static Image ConstructImage(CellColor cell) =>
            ConstructImage(GetBitmapByType(cell));

        public static Image ConstructImage(CellState cellState) =>
            ConstructImage(GetBitmapByType(cellState));

        public static BitmapImage GetBitmapByType(CellColor cell) =>
            cell switch
            {
                CellColor.Black => BlackCell,
                CellColor.White => WhiteCell,
                _ => null,
            };

        public static BitmapImage GetBitmapByType(CellState cellState) =>
            cellState switch
            {
                CellState.BlackMan  => BlackMan,
                CellState.BlackKing => BlackKing,
                CellState.WhiteMan  => WhiteMan,
                CellState.WhiteKing => WhiteKing,
                _ => null,
            };

        private static Image ConstructImage(BitmapImage bitmap) =>
            bitmap != null ? new Image { Source = bitmap } : new Image();

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
