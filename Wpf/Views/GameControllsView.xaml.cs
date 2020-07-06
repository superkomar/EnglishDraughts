using System.Windows.Controls;

using Wpf.ViewModels;

namespace Wpf.Views
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
