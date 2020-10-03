using System.ComponentModel;
using System.Threading.Tasks;

using Core;
using Core.Enums;
using Core.Interfaces;
using Core.Models;

using Robot;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    internal class MainWindowVM : NotifyPropertyChanged
    {
        private const string StartStatusText = "Wait start the game";

        private GameController _gameController;

        private readonly WpfPlayer _wpfPlayer;
        private readonly RobotLauncher _robotLauncher;

        public MainWindowVM()
        {
            Reporter = new StatusReporter();
            Reporter.ReportInfo(StartStatusText);

            _wpfPlayer = new WpfPlayer(VMLocator.GameFieldVM, VMLocator.GameControllsVM, Reporter);
            _robotLauncher = new RobotLauncher(VMLocator.GameControllsVM.RobotTime.Value);

            AttachHandlers();
        }

        public IStatusReporter Reporter { get; }

        private void AttachHandlers()
        {
            //VMLocator.GameFieldVM.PropertyChanged += OnFieldPropertyChanged;
            VMLocator.GameControllsVM.PropertyChanged += OnControllsPropertyChanged;
        }

        private void DetachHandlers()
        {
            //VMLocator.GameFieldVM.PropertyChanged -= OnFieldPropertyChanged;
            VMLocator.GameControllsVM.PropertyChanged -= OnControllsPropertyChanged;
        }

        private async void OnControllsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IGameControllsVM.StartCmd):
                {
                    await StartGameAsync();
                    break;
                }
                case nameof(IGameControllsVM.FinishCmd):
                {
                    _gameController?.StopGame();
                    break;
                }
                case nameof(IGameControllsVM.RobotTime):
                {
                    _robotLauncher.TurnTime = VMLocator.GameControllsVM.RobotTime.Value;
                    break;
                }
                case nameof(IGameControllsVM.UndoCmd):
                {
                    _gameController?.Undo(deep: 2);
                    break;
                }
                case nameof(IGameControllsVM.RedoCmd):
                {
                    _gameController?.Redo(deep: 2);
                    break;
                }
            }
        }

        private (IGamePlayer Black, IGamePlayer White) GetPlayers() =>
            VMLocator.GameControllsVM.Side.Value switch
            {
                PlayerSide.White => (_robotLauncher, _wpfPlayer),
                PlayerSide.Black => (_wpfPlayer, _robotLauncher),
                _ => throw new System.NotImplementedException(),
            };

        private async Task StartGameAsync()
        {
            var (Black, White) = GetPlayers();

            var black = new WpfPlayer(VMLocator.GameFieldVM, VMLocator.GameControllsVM, Reporter);
            var white = new WpfPlayer(VMLocator.GameFieldVM, VMLocator.GameControllsVM, Reporter);

            _gameController = new GameController(Core.Constants.FieldDimension, black, white);

            await foreach (var state in _gameController.StartGameAsync())
            {
                switch (state.State)
                {
                    case GameState.StateType.Start:
                    {
                        VMLocator.GameFieldVM.InitGameField(state.Field);

                        black.InitGame(PlayerSide.Black);
                        white.InitGame(PlayerSide.White);
                        break;
                    }
                    case GameState.StateType.Finish:
                    {
                        VMLocator.GameFieldVM.UpdateGameField(state.Field);

                        black.FinishGame(state.Side);
                        white.FinishGame(state.Side);
                        return;
                    }
                    case GameState.StateType.Turn:
                    default:
                    {
                        VMLocator.GameFieldVM.UpdateGameField(state.Field);
                        break;
                    }
                }
            }
        }
    }
}
