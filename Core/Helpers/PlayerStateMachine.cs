using Core.Enums;
using Core.Models;

namespace Core.Helpers
{
    internal class PlayerStateMachine
    {
        private const PlayerSide FirstPlayer = PlayerSide.Black;

        private readonly (PlayerBase Player, PlayerSide Side) _blackPlayer;
        private readonly (PlayerBase Player, PlayerSide Side) _whitePlayer;

        private MachineState _machineState;

        public enum MachineState
        {
            Normal = 0,
            Repeat,
            Start
        }

        public PlayerStateMachine(PlayerBase blackPlayer, PlayerBase whitePlayer)
        {
            _blackPlayer = (blackPlayer, PlayerSide.Black);
            _whitePlayer = (whitePlayer, PlayerSide.White);

            _machineState = MachineState.Start;
        }

        public PlayerBase BlackPlayer => GetBySide(PlayerSide.Black).Player;

        public PlayerBase WhitePlayer => GetBySide(PlayerSide.White).Player;

        public (PlayerBase Player, PlayerSide Side) CurPlayer { get; private set; }

        /// <summary>
        /// Change state to a new one for a single call GetNextPlayer() method.
        /// After that reset to the default value.
        /// </summary>
        /// <param name="state">New state</param>
        public void ChangeStateForOneGet(MachineState state)
        {
            _machineState = state;
        }

        public (PlayerBase Player, PlayerSide Side) GetNextPlayer()
        {
            switch (_machineState)
            {
                case MachineState.Normal:
                {
                    CurPlayer = CurPlayer == _blackPlayer ? _whitePlayer : _blackPlayer;
                    break;
                }
                case MachineState.Start:
                {
                    CurPlayer = GetBySide(FirstPlayer);
                    break;
                }
            }

            _machineState = default;

            return CurPlayer;
        }

        private (PlayerBase Player, PlayerSide Side) GetBySide(PlayerSide side) =>
            side switch
            {
                PlayerSide.Black => _blackPlayer,
                PlayerSide.White => _whitePlayer,
                _ => throw new System.NotImplementedException()
            };
    }
}
