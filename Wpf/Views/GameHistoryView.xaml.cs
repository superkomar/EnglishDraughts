using System.Windows.Controls;

using Wpf.ViewModels;

namespace Wpf.Views
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
