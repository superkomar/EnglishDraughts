using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Wpf.Interfaces;
using Wpf.ViewModels;
using Wpf.Views.Controls;

namespace Wpf.Views
{
    /// <summary>
    /// Interaction logic for GameFieldView.xaml
    /// </summary>
    public partial class GameFieldView : UserControl
    {
        public GameFieldView()
        {
            InitializeComponent();

            DataContext = VMLocator.GameFieldVM;

            GenerateField();
        }

        private void GenerateField()
        {
            if (!(DataContext is IGameFieldVM gameFieldVM)) return;

            for (int i = 0; i < gameFieldVM.Dimension; i++)
            {
                GameField.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            for (int j = 0; j < gameFieldVM.Dimension; j++)
            {
                GameField.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            GameField.Margin = new Thickness(0);
            GameField.Background = Brushes.Black;

            for (int i = 0; i < gameFieldVM.Dimension; i++)
            {
                for (int j = 0; j < gameFieldVM.Dimension; j++)
                {
                    var cell = new GameCell(gameFieldVM.GetCellHandler(i, j), 60, 60)
                    {
                        Margin = new Thickness(0),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    GameField.Children.Add(cell);

                    Grid.SetColumn(cell, j);
                    Grid.SetRow(cell, i);
                }
            }
        }
    }
}
