using System.ComponentModel;
using System.Threading.Tasks;

using Core;
using Core.Enums;
using Core.Interfaces;

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
            Reporter.ReportStatus(Resources.WpfPlalyer_StartStatus);

            _wpfPlayer = new WpfPlayer(VMLocator.GameFieldVM, VMLocator.GameControlsVM, Reporter);
            _robotLauncher = new RobotLauncher(VMLocator.GameControlsVM.RobotTime.Value, Reporter);

            AttachHandlers();
        }

        public IReporter Reporter { get; }

        private void AttachHandlers()
        {
            VMLocator.GameControlsVM.PropertyChanged += OnControllsPropertyChanged;
            VMLocator.GameControlsVM.RobotTime.PropertyChanged += OnControllsPropertyChanged;
        }

        private (IGamePlayer Black, IGamePlayer White) GetPlayers() =>
            VMLocator.GameControlsVM.Side.Value switch
            {
                PlayerSide.White => (_robotLauncher, _wpfPlayer),
                PlayerSide.Black => (_wpfPlayer, _robotLauncher),
                _ => throw new System.NotImplementedException(),
            };

        private async void OnControllsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IGameControlsVM.StartGameCmd):
                {
                    await StartGameAsync();
                    break;
                }
                case nameof(IGameControlsVM.EndGameCmd):
                {
                    _gameController?.StopGame();
                    break;
                }
                case nameof(IGameControlsVM.RobotTime):
                {
                    _robotLauncher.TurnTime = VMLocator.GameControlsVM.RobotTime.Value;
                    break;
                }
                case nameof(IGameControlsVM.UndoCmd):
                {
                    _gameController?.Undo(deep: 2);
                    break;
                }
                case nameof(IGameControlsVM.RedoCmd):
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
                Settings.Default.DefaultFieldDimension,
                blackPlayer: Black,
                whitePlayer: White);

            await foreach (var state in _gameController.StartGameAsync())
            {
                switch (state.State)
                {
                    case StateType.Start:
                    {
                        VMLocator.GameFieldVM.InitGameField(state.Field);

                        Black.InitGame(PlayerSide.Black);
                        White.InitGame(PlayerSide.White);
                        break;
                    }
                    case StateType.Finish:
                    {
                        VMLocator.GameFieldVM.UpdateGameField(state.Field);

                        Black.FinishGame(state.Side);
                        White.FinishGame(state.Side);
                        break;
                    }
                    case StateType.Turn:
                    default:
                    {
                        VMLocator.GameFieldVM.UpdateGameField(state.Field);
                        break;
                    }
                }
            }

            VMLocator.GameControlsVM.UpdateState(GameControlsVM.StateType.GameEnd);
        }
    }
}
