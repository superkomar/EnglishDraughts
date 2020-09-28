using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Wpf.ViewModels.CustomTypes
{
    internal interface ITurnsConstructor
    {
        bool IsJumpsContinue { get; }

        PlayerSide Side { get; }

        bool CheckTurnStartCell(int cellIdx);

        void Clear();

        TurnsConstructor.Result TryMakeTurn(int start, int end);
    }

    internal class TurnsConstructor : ITurnsConstructor
    {
        private GameField _gameField;
        private IStatusReporter _reporter;
        private IEnumerable<GameTurn> _requiredJumps;
        private IResultSetter<IGameTurn> _sender;
        private List<IGameTurn> _turns;

        public enum Result
        {
            Ok,
            Fail,
            Continue
        }

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

            _sender?.SetResult(ModelsCreator.CreateGameTurn(_turns));

            return Result.Ok;
        }

        public void UpdateState(GameField newField, PlayerSide side, IStatusReporter reporter, IResultSetter<IGameTurn> sender)
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
