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
        private readonly ICellHandlersController _cellHandlersController;

        public GameFieldView()
        {
            InitializeComponent();

            _cellHandlersController = VMLocator.CellHandlersController;
            DataContext = _cellHandlersController;

            Loaded += (s, e) => AttachHandlers();
            Unloaded += (s, e) => DetachHandlers();

            InitGameField();
        }

        private void AttachHandlers()
        {
            _cellHandlersController.RedrawField += OnRedrawFieldChanged;
        }

        private void OnRedrawFieldChanged(object sender, EventArgs e)
        {
            GameField.ResetField();
            InitGameField();
        }

        private void DetachHandlers()
        {
            _cellHandlersController.RedrawField -= OnRedrawFieldChanged;
        }
        
        private void InitGameField()
        {
            GameField.GenerateNewField(_cellHandlersController);
        }
    }
}
