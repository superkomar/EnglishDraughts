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
            Unloaded += (s, e) => DetachHandlers();

            InitGameField();
        }

        private void AttachHandlers()
        {
            (DataContext as IGameFieldVM).RedrawField += OnRedrawFieldChanged;
        }

        private void OnRedrawFieldChanged(object sender, EventArgs e)
        {
            GameField.ResetField();
            InitGameField();
        }

        private void DetachHandlers()
        {
            (DataContext as IGameFieldVM).RedrawField -= OnRedrawFieldChanged;
        }
        
        private void InitGameField()
        {
            GameField.GenerateNewField(DataContext as IGameFieldVM);
        }
    }
}
