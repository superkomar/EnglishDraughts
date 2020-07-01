using System.Collections.ObjectModel;
using System.Windows.Controls;

using EnglishDraughts.ViewModel;
using EnglishDraughts.ViewModel.Interfaces;

namespace EnglishDraughts.View
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
        }

        private void GenerateField()
        {  
            if (!(DataContext is IGameFieldVM viewModel)) return;

            var field = new ObservableCollection<FieldRow>();

            for (var i = 0; i < viewModel.Dimension; i++)
            {
            }
            
            
            //myDataGrid.ItemsSource = field;
        }

        private class FieldRow
        {
            public FieldCell First { get; set; }
            public FieldCell Second { get; set; }
        }

        //private IList<FieldCell> GenerateRow(int dimention)
        //{

        //}
    }
}
