using System.ComponentModel;
using System.Threading.Tasks;

using Core;
using Core.Enums;
using Core.Interfaces;
using Core.Models;

using Robot;

using Wpf.Interfaces;
using Wpf.Properties;
using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    internal class MainWindowVM : NotifyPropertyChanged
    {
        private readonly RobotLauncher _robotLauncher;
        private readonly WpfPlayer _wpfPlayer;
        private GameController _gameController;
        public MainWindowVM()
        {
            Reporter = new StatusReporter();
            Reporter.ReportInfo(Resources.WpfPlalyer_StartStatus);

            _wpfPlayer = new WpfPlayer(VMLocator.GameFieldVM, VMLocator.GameControllsVM, Reporter);
            _robotLauncher = new RobotLauncher(VMLocator.GameControllsVM.RobotTime.Value);

            AttachHandlers();
        }

        public IStatusReporter Reporter { get; }

        private void AttachHandlers()
        {
            VMLocator.GameControllsVM.PropertyChanged += OnControllsPropertyChanged;
            VMLocator.GameControllsVM.RobotTime.PropertyChanged += OnControllsPropertyChanged;
        }

        private (IGamePlayer Black, IGamePlayer White) GetPlayers() =>
            VMLocator.GameControllsVM.Side.Value switch
            {
                PlayerSide.White => (_robotLauncher, _wpfPlayer),
                PlayerSide.Black => (_wpfPlayer, _robotLauncher),
                _ => throw new System.NotImplementedException(),
            };

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

        private async Task StartGameAsync()
        {
            var (Black, White) = GetPlayers();

            _gameController = new GameController(
                Core.Constants.FieldDimension,
                blackPlayer: Black,
                whitePlayer: White);

            await foreach (var state in _gameController.StartGameAsync())
            {
                switch (state.State)
                {
                    case GameState.StateType.Start:
                    {
                        VMLocator.GameFieldVM.InitGameField(state.Field);

                        Black.InitGame(PlayerSide.Black);
                        White.InitGame(PlayerSide.White);
                        break;
                    }
                    case GameState.StateType.Finish:
                    {
                        VMLocator.GameFieldVM.UpdateGameField(state.Field);

                        Black.FinishGame(state.Side);
                        White.FinishGame(state.Side);
                        break;
                    }
                    case GameState.StateType.Turn:
                    default:
                    {
                        VMLocator.GameFieldVM.UpdateGameField(state.Field);
                        break;
                    }
                }
            }

            VMLocator.GameControllsVM.UpdateState(GameControllsVM.StateType.GameFinish);
        }
    }
}
