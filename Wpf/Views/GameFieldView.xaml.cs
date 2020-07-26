using System;
using System.Windows.Controls;

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

            Loaded += (s, e) => AttachHandlers();
            Unloaded += (s, e) => DetacheHandlers();

            InitGameField();
        }

        private void AttachHandlers()
        {
            (DataContext as GameFieldVM).CreateField += CreateField;
            (DataContext as GameFieldVM).ResetField += ResetField;
        }

        private void CreateField(object sender, EventArgs e)
        {
            InitGameField();
        }

        private void DetacheHandlers()
        {
            (DataContext as GameFieldVM).CreateField -= CreateField;
            (DataContext as GameFieldVM).ResetField -= ResetField;
        }
        
        private void InitGameField()
        {
            GameField.GenerateNewField(DataContext as IGameFieldController);
        }

        private void ResetField(object sender, EventArgs e)
        {
            GameField.ResetField();
        }
    }
}
