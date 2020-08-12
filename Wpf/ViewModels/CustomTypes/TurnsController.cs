using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Interfaces;
using Core.Model;
using Core.Utils;

using Wpf.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    public interface ITurnsController
    {
        bool IsJumpsContinue { get; }

        PlayerSide Side { get; }

        void Clear();

        bool CheckTurnStartCell(int cellIdx);

        TurnsController.Result TryMakeTurn(int start, int end);
    }

    public class TurnsController : ITurnsController
    {
        private GameField _gameField;
        private IStatusReporter _reporter;
        private IEnumerable<GameTurn> _requiredJumps;

        private List<IGameTurn> _turns;

        public enum Result
        {
            Ok,
            Fail,
            Continue
        }

        public event EventHandler<IGameTurn> TakeTurn;

        public bool IsJumpsContinue { get; private set; }
        
        public PlayerSide Side { get; private set; }
        
        public bool CheckTurnStartCell(int cellIdx) =>
            !_requiredJumps.Any() || _requiredJumps.FirstOrDefault(x => x.Start == cellIdx) != null;

        public Result TryMakeTurn(int start, int end)
        {
            var gameTurn = ModelsCreator.CreateGameTurn(_gameField, Side, start, end);
            
            if (gameTurn == null || (gameTurn.IsSimple && _requiredJumps.Any()))
            {
                return Result.Fail;
            }

            _turns.Add(gameTurn);

            GameFieldUpdater.TryMakeTurn(_gameField, gameTurn, out GameField newField);

            // Not last jump
            if (!gameTurn.IsSimple && GameFieldUtils.FindTurnsForCell(newField, gameTurn.Turns.Last(), GameFieldUtils.TurnType.Jump).Any())
            {
                _reporter?.Report($"{Side}: Continue jumps");

                IsJumpsContinue = true;

                _gameField = newField;

                return Result.Continue;
            }

            OnTakeTurnChanged(ModelsCreator.CreateGameTurn(_turns));

            return Result.Ok;
        }

        public void UpdateField(GameField newField, PlayerSide side, IStatusReporter reporter)
        {
            Side = side;
            _gameField = newField;
            _reporter = reporter;

            _requiredJumps = GameFieldUtils.FindRequiredJumps(_gameField, Side);
            _turns = new List<IGameTurn>();

            IsJumpsContinue = false;

            _reporter?.Report($"{Side}: " + (_requiredJumps.Any()
                ? "Choose cell for required jump"
                : "Choose cell for turn"));
        }

        private void OnTakeTurnChanged(IGameTurn turn) => TakeTurn?.Invoke(this, turn);

        public void Clear()
        {
            IsJumpsContinue = false;
            _turns.Clear();
        }
    }
}
