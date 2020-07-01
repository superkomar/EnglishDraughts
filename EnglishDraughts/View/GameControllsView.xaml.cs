using System.Windows.Controls;

using EnglishDraughts.ViewModel;

namespace EnglishDraughts.View
{
    /// <summary>
    /// Interaction logic for GameControllsView.xaml
    /// </summary>
    public partial class GameControllsView : UserControl
    {
        public GameControllsView()
        {
            InitializeComponent();

            DataContext = VMLocator.GameControllsVM;
        }
    }
}
