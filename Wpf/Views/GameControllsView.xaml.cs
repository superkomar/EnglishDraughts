using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

using Wpf.ViewModels;

namespace Wpf.Views
{
    /// <summary>
    /// Interaction logic for GameControllsView.xaml
    /// </summary>
    public partial class GameControllsView : UserControl
    {
        private readonly Regex StringToIntRegex = new Regex("[^0-9]+");

        public GameControllsView()
        {
            InitializeComponent();

            DataContext = VMLocator.GameControllsVM;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringToIntRegex.IsMatch(e.Text);
        }
    }
}
