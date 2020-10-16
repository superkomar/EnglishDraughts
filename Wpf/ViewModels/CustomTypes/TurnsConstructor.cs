using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

using Wpf.Interfaces;
using Wpf.Properties;

namespace Wpf.ViewModels.CustomTypes
{
    internal class TurnsConstructor : ITurnsConstructor
    {
        private GameField _curField;
        private IReporter _reporter;
        private IEnumerable<GameTurn> _requiredJumps;
        private IResultSender<GameTurn> _sender;
        private List<GameTurn> _turns;

        public enum Result
        {
            Ok,
            Fail,
            Continue
        }

        #region ITurnsConstructor

        public bool DoJumpsContinue { get; private set; }
        
        public PlayerSide Side { get; private set; }
        
        public bool CheckTurnStart(int cellIdx) =>
            !_requiredJumps.Any() || _requiredJumps.FirstOrDefault(x => x.Start == cellIdx) != null;

        public void Clear()
        {
            DoJumpsContinue = false;
            _turns.Clear();
        }

        public Result TryMakeTurn(int start, int end)
        {
            var gameTurn = GameTurnUtils.CreateTurnByTwoCells(_curField, Side, start, end);
            
            if (gameTurn == null || (gameTurn.IsSimple && _requiredJumps.Any()))
            {
                return Result.Fail;
            }

            _turns.Add(gameTurn);

            GameFieldUtils.TryCreateField(_curField, gameTurn, out GameField newField);

            // Not the last jump
            if (!gameTurn.IsSimple && !gameTurn.IsLevelUp &&
                GameTurnUtils.FindTurnsForCell(newField, gameTurn.Steps.Last(), TurnType.Jump).Any())
            {
                _reporter?.ReportStatus(
                    $"{Side}: {Resources.WpfPlayer_JumpTurn_Continue}");

                DoJumpsContinue = true;

                _curField = newField;

                return Result.Continue;
            }

            _sender?.Send(GameTurnUtils.CreateCompositeJump(_turns));

            return Result.Ok;
        }

        #endregion

        public void UpdateState(GameField newField, PlayerSide side, IReporter reporter, IResultSender<GameTurn> sender)
        {
            Side = side;
            _curField = newField;
            _reporter = reporter;
            _sender = sender;

            _turns = new List<GameTurn>();
            _requiredJumps = GameTurnUtils.FindRequiredJumps(_curField, Side);

            DoJumpsContinue = false;

            _reporter?.ReportStatus(string.Format("{0}: {1}", Side,
                _requiredJumps.Any()
                ? Resources.WpfPlayer_JumpTurn_Start
                : Resources.WpfPlayer_SimpleTurn));
        }
    }
}
