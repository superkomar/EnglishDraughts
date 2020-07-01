using System.Windows.Controls;

using EnglishDraughts.ViewModel;

namespace EnglishDraughts.View
{
    /// <summary>
    /// Interaction logic for GameHistoryView.xaml
    /// </summary>
    public partial class GameHistoryView : UserControl
    {
        public GameHistoryView()
        {
            InitializeComponent();

            DataContext = VMLocator.GameHistoryVM;
        }
    }
}
