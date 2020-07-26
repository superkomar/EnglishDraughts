using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Wpf.Interfaces;
using Wpf.ViewModels;
using Wpf.Views.Controls;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowVM();
        }
    }
}
