using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

using Wpf.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    internal class TurnsConstructor : ITurnsConstructor
    {
        private GameField _gameField;
        private IStatusReporter _reporter;
        private IEnumerable<GameTurn> _requiredJumps;
        private IResultSender<IGameTurn> _sender;
        private List<IGameTurn> _turns;

        public enum Result
        {
            Ok,
            Fail,
            Continue
        }

        #region ITurnsConstructor

        public bool IsJumpsContinue { get; private set; }
        
        public PlayerSide Side { get; private set; }
        
        public bool CheckTurnStartCell(int cellIdx) =>
            !_requiredJumps.Any() || _requiredJumps.FirstOrDefault(x => x.Start == cellIdx) != null;

        public void Clear()
        {
            IsJumpsContinue = false;
            _turns.Clear();
        }

        public Result TryMakeTurn(int start, int end)
        {
            var gameTurn = ModelsCreator.CreateGameTurn(_gameField, Side, start, end);
            
            if (gameTurn == null || (gameTurn.IsSimple && _requiredJumps.Any()))
            {
                return Result.Fail;
            }

            _turns.Add(gameTurn);

            GameFieldUtils.TryMakeTurn(_gameField, gameTurn, out GameField newField);

            // Not the last jump
            if (!gameTurn.IsSimple && GameFieldUtils.FindTurnsForCell(newField, gameTurn.Turns.Last(), TurnType.Jump).Any())
            {
                _reporter?.ReportInfo($"{Side}: Continue jumps");

                IsJumpsContinue = true;

                _gameField = newField;

                return Result.Continue;
            }

            _sender?.Send(ModelsCreator.CreateGameTurn(_turns));

            return Result.Ok;
        }

        #endregion

        public void UpdateState(GameField newField, PlayerSide side, IStatusReporter reporter, IResultSender<IGameTurn> sender)
        {
            Side = side;
            _gameField = newField;
            _reporter = reporter;
            _sender = sender;

            _turns = new List<IGameTurn>();
            _requiredJumps = GameFieldUtils.FindRequiredJumps(_gameField, Side);

            IsJumpsContinue = false;

            _reporter?.ReportInfo($"{Side}: " + (_requiredJumps.Any()
                ? "Choose cell for required jump"
                : "Choose cell for turn"));
        }
    }
}
